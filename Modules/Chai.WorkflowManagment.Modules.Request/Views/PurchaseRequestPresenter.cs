using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Request;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Inventory;
using Chai.WorkflowManagment.Modules.Setting;
using Chai.WorkflowManagment.Modules.Inventory;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public class PurchaseRequestPresenter : Presenter<IPurchaseRequestView>
    {
        private RequestController _controller;
        private SettingController _settingController;
        private InventoryController _inventoryController;
        private PurchaseRequest _purchaseRequest;
        public PurchaseRequestPresenter([CreateNew] RequestController controller, [CreateNew] SettingController settingController, [CreateNew] InventoryController inventoryController)
        {
            _controller = controller;
            _settingController = settingController;
            _inventoryController = inventoryController;
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
                if (_purchaseRequest == null)
                {
                    int id = View.PurchaseRequestId;
                    if (id > 0)
                        _purchaseRequest = _controller.GetPurchaseRequest(id);
                    else
                        _purchaseRequest = new PurchaseRequest();
                }
                return _purchaseRequest;
            }
            set { _purchaseRequest = value; }
        }
        public override void OnViewInitialized()
        {
            if (_purchaseRequest == null)
            {
                int id = View.PurchaseRequestId;
                if (id > 0)
                    _controller.CurrentObject = _controller.GetPurchaseRequest(id);
                else
                    _controller.CurrentObject = new PurchaseRequest();
            }
        }
        public IList<PurchaseRequest> GetPurchaseRequests()
        {
            return _controller.GetPurchaseRequests();
        }
        public AppUser Approver(int Position)
        {
            return _controller.Approver(Position);
        }
        public AppUser GetUser(int UserId)
        {
            return _controller.GetSuperviser(UserId);
        }
        public AppUser GetSuperviser(int superviser)
        {
            return _controller.GetSuperviser(superviser);
        }
        public void SaveOrUpdateLeavePurchase(PurchaseRequest PurchaseRequest)
        {
            _controller.SaveOrUpdateEntity(PurchaseRequest);
        }
        public int GetLastPurchaseRequestId()
        {
            return _controller.GetLastPurchaseRequestId();
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
        public ApprovalSetting GetApprovalSetting(string RequestType, decimal value)
        {
            return _settingController.GetApprovalSettingforProcess(RequestType, value);
        }
        public ApprovalSetting GetApprovalSettingPurchaseGS()
        {
            return _settingController.GetApprovalSettingPurchaseGS();
        }
        public ApprovalSetting GetApprovalSettingforPurchaseProcess(string RequestType, decimal value)
        {
            return _settingController.GetApprovalSettingforPurchaseProcess(RequestType, value);
        }
        public AssignJob GetAssignedJobbycurrentuser()
        {
            return _controller.GetAssignedJobbycurrentuser();
        }
        public AssignJob GetAssignedJobbycurrentuser(int UserId)
        {
            return _controller.GetAssignedJobbycurrentuser(UserId);
        }
        public IList<PurchaseRequest> ListPurchaseRequests(string requestNo, string RequestDate)
        {
            return _controller.ListPurchaseRequests(requestNo, RequestDate);

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
        public IList<Grant> GetGrants()
        {
            return _settingController.GetGrants();

        }
        public IList<Item> GetItems()
        {
            return _settingController.GetItems();

        }
        public Item GetItemByName(string Name)
        {
            return _inventoryController.GetItemByName(Name);

        }
        public Grant GetGrant(int Id)
        {
            return _settingController.GetGrant(Id);

        }
        public PurchaseRequestDetail GetPurchaseRequestDetail(int Id)
        {
            return _controller.GetPurchaseRequestDetail(Id);

        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public void DeletePurchaseRequestDetail(PurchaseRequestDetail PurchaseRequestDetail)
        {
            _controller.DeleteEntity(PurchaseRequestDetail);
        }
        public void Commit()
        {
            _controller.Commit();
        }
        public IList<Grant> GetGrantbyprojectId(int projectId)
        {
            return _settingController.GetProjectGrantsByprojectId(projectId);

        }
        public IList<Vehicle> GetVehicles()
        {
            return _settingController.GetVehicles();
        }
        public IList<MaintenanceRequest> GetMaintenanceRequestCompleted()
        {
            return _controller.GetMaintenanceRequestsCompleted();
        }
        public MaintenanceRequest GetMaintenanceRequestById(int Id)
        {
            return _settingController.GetMaintenanceRequestById(Id);

        }
    }
}




