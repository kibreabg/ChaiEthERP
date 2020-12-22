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
using Chai.WorkflowManagment.CoreDomain.Inventory;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public class StoreApprovalPresenter : Presenter<IStoreApprovalView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private ApprovalController _controller;
        private SettingController _settingcontroller;
        private AdminController _admincontroller;
        private StoreRequest _storerequest;
        public StoreApprovalPresenter([CreateNew] ApprovalController controller, [CreateNew] Setting.SettingController settingcontroller, [CreateNew] AdminController admincontroller)
        {
            _controller = controller;
            _settingcontroller = settingcontroller;
            _admincontroller = admincontroller;
        }
        public override void OnViewLoaded()
        {
            if (View.StoreRequestId > 0)
            {
                _controller.CurrentObject = _controller.GetStoreRequest(View.StoreRequestId);
            }
            CurrentStoreRequest = _controller.CurrentObject as StoreRequest;
        }
        public StoreRequest CurrentStoreRequest
        {
            get
            {
                if (_storerequest == null)
                {
                    int id = View.StoreRequestId;
                    if (id > 0)
                        _storerequest = _controller.GetStoreRequest(id);
                    else
                        _storerequest = new StoreRequest();
                }
                return _storerequest;
            }
            set { _storerequest = value; }
        }
        public override void OnViewInitialized()
        {
            if (_storerequest == null)
            {
                int id = View.StoreRequestId;
                if (id > 0)
                    _controller.CurrentObject = _controller.GetStoreRequest(id);
                else
                    _controller.CurrentObject = new StoreRequest();
            }
        }
        public Item GetItem(int Id)
        {
            return _settingcontroller.GetItem(Id);
        }
        public IList<Item> GetItems()
        {
            return _settingcontroller.GetItems();
        }
        public IList<ItemAccount> GetItemAccounts()
        {
            return _settingcontroller.GetItemAccounts();

        }
        public ItemAccount GetItemAccount(int Id)
        {
            return _settingcontroller.GetItemAccount(Id);
        }
        public IList<Project> GetProjects()
        {
            return _settingcontroller.GetProjects();
        }
        public Project GetProject(int Id)
        {
            return _settingcontroller.GetProject(Id);
        }
        public IList<Grant> GetGrantbyprojectId(int projectId)
        {
            return _settingcontroller.GetProjectGrantsByprojectId(projectId);
        }
        public IList<Grant> GetGrants()
        {
            return _settingcontroller.GetGrants();
        }
        public Grant GetGrantByCode(string grantCode)
        {
            return _settingcontroller.GetGrantByCode(grantCode);
        }
        public AppUser Approver(int Position)
        {
            return _controller.Approver(Position);
        }
        public AppUser GetUser(int UserId)
        {
            return _admincontroller.GetUser(UserId);
        }
        public void SaveOrUpdateStoreRequest(StoreRequest StoreRequest)
        {
            _controller.SaveOrUpdateEntity(StoreRequest);
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
        public void DeleteStoreRequest(StoreRequest StoreRequest)
        {
            _controller.DeleteEntity(StoreRequest);
        }
        public StoreRequest GetStoreRequestById(int id)
        {
            return _controller.GetStoreRequest(id);
        }
        public ApprovalSetting GetApprovalSetting(string RequestType, int value)
        {
            return _settingcontroller.GetApprovalSettingforProcess(RequestType, value);
        }
        public IList<StoreRequest> ListStoreRequests(string requestNo, string RequestDate, string ProgressStatus)
        {
            return _controller.ListStoreRequests(requestNo, RequestDate, ProgressStatus);

        }
        public IList<Supplier> GetSuppliers()
        {
            return _settingcontroller.GetSuppliers();
        }
        public IList<Supplier> GetSuppliers(int SupplierTypeId)
        {
            return _settingcontroller.GetSuppliers(SupplierTypeId);
        }
        public IList<SupplierType> GetSupplierTypes()
        {
            return _settingcontroller.GetSupplierTypes();
        }
        public Supplier GetSupplier(int Id)
        {
            return _settingcontroller.GetSupplier(Id);
        }
        public SupplierType GetSupplierType(int Id)
        {
            return _settingcontroller.GetSupplierType(Id);
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




