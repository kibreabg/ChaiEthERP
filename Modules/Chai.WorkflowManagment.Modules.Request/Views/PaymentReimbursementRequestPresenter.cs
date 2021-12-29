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
    public class PaymentReimbursementRequestPresenter : Presenter<IPaymentReimbursementRequestView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private RequestController _controller;
        private AdminController _adminController;
        private SettingController _settingController;
        private CashPaymentRequest _CashPaymentRequest;
        public PaymentReimbursementRequestPresenter([CreateNew] RequestController controller, AdminController adminController, SettingController settingController)
        {
            _controller = controller;
            _adminController = adminController;
            _settingController = settingController;
        }
        public override void OnViewLoaded()
        {
            if (View.GetTARequestId > 0)
            {
                _controller.CurrentObject = _controller.GetCashPaymentRequest(View.GetTARequestId);
            }
            CurrentCashPaymentRequest = _controller.CurrentObject as CashPaymentRequest;
            if (CurrentCashPaymentRequest.PaymentReimbursementRequest == null)
            { CurrentCashPaymentRequest.PaymentReimbursementRequest = new PaymentReimbursementRequest(); }
        }
        public override void OnViewInitialized()
        {

        }
        public CashPaymentRequest CurrentCashPaymentRequest
        {
            get
            {
                if (_CashPaymentRequest == null)
                {
                    int id = View.GetTARequestId;
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
        public IList<PaymentReimbursementRequest> GetPaymentReimbursementRequests()
        {
            return _controller.GetPaymentReimbursementRequests();
        }
        private void SavePaymentReimbursementRequestStatus()
        {
            if (GetApprovalSetting(RequestType.ExpenseLiquidation_Request.ToString().Replace('_', ' '), 0) != null)
            {
                int i = 1;
                foreach (ApprovalLevel AL in GetApprovalSetting(RequestType.PaymentReimbursement_Request.ToString().Replace('_', ' '), 0).ApprovalLevels)
                {
                    PaymentReimbursementRequestStatus PRRS = new PaymentReimbursementRequestStatus();
                    PRRS.PaymentReimbursementRequest = _CashPaymentRequest.PaymentReimbursementRequest;

                    if (AL.EmployeePosition.PositionName == "Superviser/Line Manager")
                    {
                        if (CurrentUser().Superviser != 0)
                            PRRS.Approver = CurrentUser().Superviser.Value;
                        else
                        {
                            PRRS.ApprovalStatus = ApprovalStatus.Approved.ToString();
                            PRRS.Date = Convert.ToDateTime(DateTime.Today.Date.ToShortDateString());
                        }
                    }
                    else if (AL.EmployeePosition.PositionName == "Program Manager")
                    {
                        if (CurrentCashPaymentRequest.PaymentReimbursementRequest.Project != null)
                        {
                            if (CurrentCashPaymentRequest.PaymentReimbursementRequest.Project.AppUser.Id != CurrentUser().Id)
                            {
                                PRRS.Approver = GetProject(CurrentCashPaymentRequest.PaymentReimbursementRequest.Project.Id).AppUser.Id;
                            }
                            else
                            {
                                PRRS.Approver = CurrentUser().Superviser.Value;
                            }
                        }
                    }
                    else
                    {
                        if (Approver(AL.EmployeePosition.Id) != null)
                        {
                            if (AL.EmployeePosition.PositionName == "Accountant")
                            {
                                PRRS.ApproverPosition = AL.EmployeePosition.Id; //So that we can entertain more than one finance manager to handle the request
                            }
                            else
                            {
                                PRRS.Approver = Approver(AL.EmployeePosition.Id).Id;
                            }
                        }
                        else
                            PRRS.Approver = 0;
                    }
                    PRRS.WorkflowLevel = i;
                    i++;
                    CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestStatuses.Add(PRRS);
                }
            }
        }
        private void GetCurrentApprover()
        {
            if (_CashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestStatuses != null)
            {
                foreach (PaymentReimbursementRequestStatus ELRS in _CashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestStatuses)
                {
                    if (ELRS.ApprovalStatus == null)
                    {
                        if (ELRS.Approver == 0)
                        {
                            //This is to handle multiple Finance Officers responding to this request
                            //SendEmailToFinanceOfficers;
                            CurrentCashPaymentRequest.PaymentReimbursementRequest.CurrentApproverPosition = ELRS.ApproverPosition;
                        }
                        CurrentCashPaymentRequest.PaymentReimbursementRequest.CurrentApprover = ELRS.Approver;
                        CurrentCashPaymentRequest.PaymentReimbursementRequest.CurrentLevel = ELRS.WorkflowLevel;
                        CurrentCashPaymentRequest.PaymentReimbursementRequest.CurrentStatus = ELRS.ApprovalStatus;
                        SendEmail(ELRS);
                        break;
                    }
                }
            }
        }
        public void SaveOrUpdatePaymentReimbursementRequest(int paymentId)
        {

            CurrentCashPaymentRequest.PaymentReimbursementRequest.RequestDate = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            CurrentCashPaymentRequest.PaymentReimbursementRequest.Comment = View.GetComment;
            CurrentCashPaymentRequest.IsLiquidated = true;
            CurrentCashPaymentRequest.PaymentReimbursementRequest.ProgressStatus = ProgressStatus.InProgress.ToString();

            CurrentCashPaymentRequest.PaymentReimbursementRequest.CashPaymentRequest = _controller.GetCashPaymentRequest(paymentId);

            if (CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestStatuses.Count == 0)
                SavePaymentReimbursementRequestStatus();
            GetCurrentApprover();
            _controller.SaveOrUpdateEntity(CurrentCashPaymentRequest);
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Request/Default.aspx?{0}=3", AppConstants.TABID));
        }
        public void DeletePaymentReimbursementRequest(PaymentReimbursementRequest PaymentReimbursementRequest)
        {
            _controller.DeleteEntity(PaymentReimbursementRequest);
        }
        public PaymentReimbursementRequestDetail GetPaymentReimbursementRequestDetail(int CPRDId)
        {
            return _controller.GetPaymentReimbursementRequestDetail(CPRDId);
        }

        public PaymentReimbursementRequest GetPaymentReimbursementRequest(int id)
        {
            return _controller.GetPaymentReimbursementRequest(id);
        }
        public IList<PaymentReimbursementRequest> ListPaymentReimbursementRequests(string requestDate)
        {
            return _controller.ListPaymentReimbursementRequests(requestDate);
        }
        public void DeletePaymentReimbursementRequestDetail(PaymentReimbursementRequestDetail PaymentReimbursementRequestDetailt)
        {
            _controller.DeleteEntity(PaymentReimbursementRequestDetailt);
        }
        public IList<CashPaymentRequest> ListCashPaymentsNotExpensed()
        {
            return _controller.ListCashPaymentsNotExpensed();
        }
        public AppUser Approver(int Position)
        {
            return _controller.Approver(Position);
        }
        public IList<AppUser> GetUsers()
        {
            return _adminController.GetUsers();
        }
        public IList<Program> GetPrograms()
        {
            return _settingController.GetPrograms();
        }
        public Grant GetGrant(int GrantId)
        {
            return _settingController.GetGrant(GrantId);
        }
        public ItemAccount GetItemAccount(int ItemAccountId)
        {
            return _settingController.GetItemAccount(ItemAccountId);
        }
        public IList<Project> ListProjects(int programID)
        {
            return _settingController.GetProjectsByProgramId(programID);
        }
        public IList<Grant> ListGrants()
        {
            return _settingController.GetGrants();
        }
        public IList<Grant> GetGrantbyprojectId(int projectId)
        {
            return _settingController.GetProjectGrantsByprojectId(projectId);

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
        public Project GetProject(int ProjectId)
        {
            return _settingController.GetProject(ProjectId);
        }
        public IList<ItemAccount> ListItemAccounts()
        {
            return _settingController.GetItemAccounts();
        }
        public ApprovalSetting GetApprovalSetting(string RequestType, int value)
        {
            return _settingController.GetApprovalSettingforProcess(RequestType, value);
        }
        private void SendEmail(PaymentReimbursementRequestStatus ELRS)
        {
            if (ELRS.Approver != 0)
            {
                if (GetSuperviser(ELRS.Approver).IsAssignedJob != true)
                {
                    EmailSender.Send(GetSuperviser(ELRS.Approver).Email, "Payment Reimbursement Request", (CurrentCashPaymentRequest.AppUser.FullName).ToUpper() + " Requests for Payment Reimbursement with Voucher No. - '" + (CurrentCashPaymentRequest.VoucherNo).ToUpper() + "'");
                }
                else
                {
                    EmailSender.Send(GetSuperviser(_controller.GetAssignedJobbycurrentuser(ELRS.Approver).AssignedTo).Email, "Payment Reimbursement Request", (CurrentCashPaymentRequest.AppUser.FullName).ToUpper() + " Requests for Payment Reimbursement with Voucher No. - '" + (CurrentCashPaymentRequest.VoucherNo).ToUpper() + "'");
                }
            }
            else
            {
                foreach (AppUser accountant in _settingController.GetAppUsersByEmployeePosition(ELRS.ApproverPosition))
                {
                    if (accountant.IsAssignedJob != true)
                    {
                        EmailSender.Send(accountant.Email, "Payment Reimbursement Request", (CurrentCashPaymentRequest.AppUser.FullName).ToUpper() + " Requests for Payment Reimbursement with Request No. - '" + (CurrentCashPaymentRequest.RequestNo).ToUpper() + "'");
                    }
                    else
                    {
                        EmailSender.Send(GetSuperviser(_controller.GetAssignedJobbycurrentuser(accountant.Id).AssignedTo).Email, "Payment Reimbursement Request", (CurrentCashPaymentRequest.AppUser.FullName).ToUpper() + " Requests for Payment Reimbursement with Request No. - '" + (CurrentCashPaymentRequest.RequestNo).ToUpper() + "'");
                    }

                }
            }
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}




