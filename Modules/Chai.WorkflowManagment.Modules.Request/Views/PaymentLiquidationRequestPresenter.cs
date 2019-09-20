using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.Modules.Admin;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Modules.Setting;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared.MailSender;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public class PaymentLiquidationRequestPresenter : Presenter<IPaymentLiquidationRequestView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private RequestController _controller;
        private AdminController _adminController;
        private SettingController _settingController;
        private CashPaymentRequest _CashPaymentRequest;
        public PaymentLiquidationRequestPresenter([CreateNew] RequestController controller, AdminController adminController, SettingController settingController)
        {
            _controller = controller;
            _adminController = adminController;
            _settingController = settingController;
        }
        public override void OnViewLoaded()
        {
            if (View.GetPayRequestId > 0)
            {
                _controller.CurrentObject = _controller.GetCashPaymentRequest(View.GetPayRequestId);
            }
            
            CurrentCashPaymentRequest = _controller.CurrentObject as CashPaymentRequest;
            if (CurrentCashPaymentRequest != null)
            {
                if (CurrentCashPaymentRequest.PaymentLiquidationRequest == null)
                { CurrentCashPaymentRequest.PaymentLiquidationRequest = new PaymentLiquidationRequest(); }
            }
        }
        public override void OnViewInitialized()
        {
            //if (_TravelAdvanceRequest == null)
            //{
            //    int id = View.GetTARequestId;
            //    if (id > 0)
            //        _controller.CurrentObject = _controller.GetTravelAdvanceRequest(id);
            //    else
            //        _controller.CurrentObject = new TravelAdvanceRequest();
            //}
        }
        public CashPaymentRequest CurrentCashPaymentRequest
        {
            get
            {
                if (_CashPaymentRequest == null)
                {
                    int id = View.GetPayRequestId;
                    if (id > 0)
                        _CashPaymentRequest = _controller.GetCashPaymentRequest(id);
                    else
                        _CashPaymentRequest = new CashPaymentRequest();
                }
                return _CashPaymentRequest;
            }
            set
            {
                _CashPaymentRequest = value;
            }
        }
        public IList<PaymentLiquidationRequest> GetPaymentLiquidationRequests()
        {
            return _controller.GetPaymentLiquidationRequests();
        }
        private void SavePaymentLiquidationRequestStatus()
        {
            if (GetApprovalSetting(RequestType.PaymentLiquidation_Request.ToString().Replace('_', ' '), 0) != null)
            {
                int i = 1;
                foreach (ApprovalLevel AL in GetApprovalSetting(RequestType.PaymentLiquidation_Request.ToString().Replace('_', ' '), 0).ApprovalLevels)
                {
                    PaymentLiquidationRequestStatus PLRS = new PaymentLiquidationRequestStatus();
                    PLRS.PaymentLiquidationRequest = _CashPaymentRequest.PaymentLiquidationRequest;

                    if (AL.EmployeePosition.PositionName == "Superviser/Line Manager")
                    {
                        if (CurrentUser().Superviser != 0)
                            PLRS.Approver = CurrentUser().Superviser.Value;
                        else
                        {
                            PLRS.ApprovalStatus = ApprovalStatus.Approved.ToString();
                            PLRS.Date = Convert.ToDateTime(DateTime.Today.Date.ToShortDateString());
                        }
                    }
                    else if (AL.EmployeePosition.PositionName == "Program Manager")
                    {
                        if (_CashPaymentRequest.PaymentLiquidationRequest.PaymentLiquidationRequestDetails[0].Project.Id != 0)
                        {
                            PLRS.Approver = GetProject(_CashPaymentRequest.PaymentLiquidationRequest.PaymentLiquidationRequestDetails[0].Project.Id).AppUser.Id;
                        }
                    }
                    else
                    {
                        if (Approver(AL.EmployeePosition.Id) != null)
                            PLRS.Approver = Approver(AL.EmployeePosition.Id).Id;
                        else
                            PLRS.Approver = 0;
                    }
                    PLRS.WorkflowLevel = i;
                    i++;
                    _CashPaymentRequest.PaymentLiquidationRequest.PaymentLiquidationRequestStatuses.Add(PLRS);
                }
            }
        }
        private void GetCurrentApprover()
        {
            if (_CashPaymentRequest.PaymentLiquidationRequest.PaymentLiquidationRequestStatuses != null)
            {
                foreach (PaymentLiquidationRequestStatus PLRS in _CashPaymentRequest.PaymentLiquidationRequest.PaymentLiquidationRequestStatuses)
                {
                    if (PLRS.ApprovalStatus == null || PLRS.ApprovalStatus == "Rejected")
                    {
                        SendEmail(PLRS);
                        CurrentCashPaymentRequest.PaymentLiquidationRequest.CurrentApprover = PLRS.Approver;
                        CurrentCashPaymentRequest.PaymentLiquidationRequest.CurrentLevel = PLRS.WorkflowLevel;
                        //CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.CurrentStatus = ELRS.ApprovalStatus;
                        break;
                    }
                }
            }
        }
        public void SaveOrUpdatePaymentLiquidationRequest(int prId)
        {

            CurrentCashPaymentRequest.PaymentLiquidationRequest.RequestDate = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            CurrentCashPaymentRequest.PaymentLiquidationRequest.RequestDate = Convert.ToDateTime(View.GetCashPayReqDate);
            CurrentCashPaymentRequest.PaymentLiquidationRequest.ExpenseType = View.GetExpenseType;
            CurrentCashPaymentRequest.PaymentLiquidationRequest.Comment = View.GetComment;
            CurrentCashPaymentRequest.PaymentLiquidationRequest.AdditionalComment = View.GetAdditionalComment;
            CurrentCashPaymentRequest.PaymentLiquidationRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
            CurrentCashPaymentRequest.PaymentLiquidationRequest.CurrentStatus = null;
            CurrentCashPaymentRequest.PaymentLiquidationRequest.CashPaymentRequest = _controller.GetCashPaymentRequest(prId);
            CurrentCashPaymentRequest.ExportStatus = "Not Exported";
            if (CurrentCashPaymentRequest.PaymentLiquidationRequest.PaymentLiquidationRequestStatuses.Count == 0)
                SavePaymentLiquidationRequestStatus();
            GetCurrentApprover();
            _controller.SaveOrUpdateEntity(CurrentCashPaymentRequest);
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Request/Default.aspx?{0}=3", AppConstants.TABID));
        }
        public void DeletePaymentLiquidationRequest(PaymentLiquidationRequest PaymentLiquidationRequest)
        {
            _controller.DeleteEntity(PaymentLiquidationRequest);
        }
        public PaymentLiquidationRequest GetPaymentLiquidationRequest(int id)
        {
            return _controller.GetPaymentLiquidationRequest(id);
        }
        public IList<PaymentLiquidationRequest> ListPaymentLiquidationRequests(string ExpenseType, string RequestDate)
        {
            return _controller.ListPaymentLiquidationRequests(ExpenseType, RequestDate);
        }
        public IList<CashPaymentRequest> ListCashPaymentNotLiquidated()
        {
            return _controller.ListCashPaymentRequestNotLiquidated();
        }
        public AppUser Approver(int Position)
        {
            return _controller.Approver(Position);
        }
        public IList<Project> ListProjects()
        {
            return _settingController.GetProjects();
        }
        public IList<AppUser> GetUsers()
        {
            return _adminController.GetUsers();
        }
        public AppUser GetUser(int id)
        {
            return _adminController.GetUser(id);
        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public AppUser GetSuperviser(int superviser)
        {
            return _controller.GetSuperviser(superviser);
        }
        public ApprovalSetting GetApprovalSetting(string RequestType, int value)
        {
            return _settingController.GetApprovalSettingforProcess(RequestType, value);
        }
        public Project GetProject(int projectId)
        {
            return _settingController.GetProject(projectId);
        }
        public Grant GetGrant(int GrantId)
        {
            return _settingController.GetGrant(GrantId);
        }
        public IList<Grant> GetGrantbyprojectId(int projectId)
        {
            return _settingController.GetProjectGrantsByprojectId(projectId);

        }
        public ItemAccount GetItemAccount(int ItemAccountId)
        {
            return _settingController.GetItemAccount(ItemAccountId);
        }
        public ExpenseType GetExpenseType(int Id)
        {
            return _settingController.GetExpenseType(Id);
        }
        public IList<ExpenseType> GetExpenseTypes()
        {
            return _settingController.GetExpenseTypes();
        }
        public IList<ItemAccount> ListItemAccounts()
        {
            return _settingController.GetItemAccounts();
        }
        private void SendEmail(PaymentLiquidationRequestStatus PLRS)
        {
            if (GetSuperviser(PLRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(GetSuperviser(PLRS.Approver).Email, "Payment Liquidation Request", (CurrentCashPaymentRequest.AppUser.FullName).ToUpper() + " Requests for Payment Liquidation for Cash Payment No. '" + (CurrentCashPaymentRequest.RequestNo).ToUpper()+ "'");
            }
            else
            {
                EmailSender.Send(GetSuperviser(_controller.GetAssignedJobbycurrentuser(PLRS.Approver).AssignedTo).Email, "Payment Liquidation Request", (CurrentCashPaymentRequest.AppUser.FullName).ToUpper() + " Requests for Payment Liquidation  for Request No. '" + (CurrentCashPaymentRequest.RequestNo).ToUpper() + "'");
            }
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}




