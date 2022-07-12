using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Request;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Approval;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.Modules.Admin;
using Chai.WorkflowManagment.Modules.Setting;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public class PurchaseApprovalDetailPresenter : Presenter<IPurchaseApprovalDetailView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private ApprovalController _controller;
        private SettingController _settingController;
        private AdminController _admincontroller;
        private PurchaseRequest _purchaserequest;
        public PurchaseApprovalDetailPresenter([CreateNew] ApprovalController controller, [CreateNew] Setting.SettingController settingcontroller, [CreateNew] AdminController admincontroller)
        {
            _controller = controller;
            _settingController = settingcontroller;
            _admincontroller = admincontroller;
        }
        public override void OnViewLoaded()
        {
            if (View.PurchaseRequestId > 0)
            {
                _controller.CurrentObject = _controller.GetPurchaseRequest(View.PurchaseRequestId);
            }
            CurrentPurchaseRequest = _controller.CurrentObject as PurchaseRequest;
        }
        public PurchaseRequest CurrentPurchaseRequest
        {
            get
            {
                if (_purchaserequest == null)
                {
                    int id = View.PurchaseRequestId;
                    if (id > 0)
                        _purchaserequest = _controller.GetPurchaseRequest(id);
                    else
                        _purchaserequest = new PurchaseRequest();
                }
                return _purchaserequest;
            }
            set { _purchaserequest = value; }
        }
        public override void OnViewInitialized()
        {
            if (_purchaserequest == null)
            {
                int id = View.PurchaseRequestId;
                if (id > 0)
                    _controller.CurrentObject = _controller.GetPurchaseRequest(id);
                else
                    _controller.CurrentObject = new PurchaseRequest();
            }
        }
        public IList<ItemAccount> GetItemAccounts()
        {
            return _settingController.GetItemAccounts();
        }
        public ItemAccount GetItemAccount(int Id)
        {
            return _settingController.GetItemAccount(Id);
        }
        public IList<Project> GetProjects()
        {
            return _settingController.GetProjects();
        }
        public Project GetProject(int Id)
        {
            return _settingController.GetProject(Id);
        }
        public IList<Grant> GetGrantbyprojectId(int projectId)
        {
            return _settingController.GetProjectGrantsByprojectId(projectId);
        }
        public IList<Grant> GetGrants()
        {
            return _settingController.GetGrants();
        }
        public Grant GetGrantByCode(string grantCode)
        {
            return _settingController.GetGrantByCode(grantCode);
        }
        public AppUser Approver(int Position)
        {
            return _controller.Approver(Position);
        }
        public AppUser GetUser(int UserId)
        {
            return _admincontroller.GetUser(UserId);
        }
        public void SaveOrUpdatePurchaseRequest(PurchaseRequest PurchaseRequest)
        {
            _controller.SaveOrUpdateEntity(PurchaseRequest);
        }
        public AssignJob GetAssignedJobbycurrentuser(int userId)
        {
            return _controller.GetAssignedJobbycurrentuser(userId);
        }
        public int GetAssignedUserbycurrentuser()
        {
            return _controller.GetAssignedUserbycurrentuser();
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }
        public void DeletePurchaseRequest(PurchaseRequest PurchaseRequest)
        {
            _controller.DeleteEntity(PurchaseRequest);
        }
        public PurchaseRequest GetPurchaseRequestById(int id)
        {
            return _controller.GetPurchaseRequest(id);
        }
        public MaintenanceRequest GetMaintenanceRequestById(int maintReqId)
        {
            return _controller.GetMaintenanceRequest(maintReqId);
        }
        public ApprovalSetting GetApprovalSetting(string RequestType, int value)
        {
            return _settingController.GetApprovalSettingforProcess(RequestType, value);
        }
        public ApprovalSetting GetApprovalSettingPurchaseGS()
        {
            return _settingController.GetApprovalSettingPurchaseGS();
        }
        public IList<PurchaseRequest> ListPurchaseRequests(string requestNo, string RequestDate, string ProgressStatus)
        {
            return _controller.ListPurchaseRequests(requestNo, RequestDate, ProgressStatus);
        }
        public IList<Supplier> GetSuppliers()
        {
            return _settingController.GetSuppliers();
        }
        public IList<Supplier> GetSuppliers(int SupplierTypeId)
        {
            return _settingController.GetSuppliers(SupplierTypeId);
        }
        public IList<SupplierType> GetSupplierTypes()
        {
            return _settingController.GetSupplierTypes();
        }
        public Supplier GetSupplier(int Id)
        {
            return _settingController.GetSupplier(Id);
        }
        public SupplierType GetSupplierType(int Id)
        {
            return _settingController.GetSupplierType(Id);
        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public void Commit()
        {
            _controller.Commit();
        }        
    }
}




