using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Request;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.Modules.Setting;
using Chai.WorkflowManagment.Modules.Admin;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public class MaintenanceApprovalPresenter : Presenter<IMaintenanceApprovalView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private ApprovalController _controller;
        private SettingController _settingController;
        private AdminController _admincontroller;
        private MaintenanceRequest _maintenanceRequest;
        public MaintenanceApprovalPresenter([CreateNew] ApprovalController controller, [CreateNew] SettingController settingcontroller, [CreateNew] AdminController admincontroller)
        {
            _controller = controller;
            _settingController = settingcontroller;
            _admincontroller = admincontroller;
        }

        public override void OnViewLoaded()
        {
            if (View.MaintenanceRequestId > 0)
            {
                _controller.CurrentObject = _controller.GetMaintenanceRequest(View.MaintenanceRequestId);
            }
            CurrentMaintenanceRequest = _controller.CurrentObject as MaintenanceRequest;
        }
        public MaintenanceRequest CurrentMaintenanceRequest
        {
            get
            {
                if (_maintenanceRequest == null)
                {
                    int id = View.MaintenanceRequestId;
                    if (id > 0)
                        _maintenanceRequest = _controller.GetMaintenanceRequest(id);
                    else
                        _maintenanceRequest = new MaintenanceRequest();
                }
                return _maintenanceRequest;
            }
            set { _maintenanceRequest = value; }
        }
        public override void OnViewInitialized()
        {
            if (_maintenanceRequest == null)
            {
                int id = View.MaintenanceRequestId;
                if (id > 0)
                    _controller.CurrentObject = _controller.GetMaintenanceRequest(id);
                else
                    _controller.CurrentObject = new MaintenanceRequest();
            }
        }
        public AppUser Approver(int Position)
        {
            return _controller.Approver(Position);
        }
        public AppUser GetUser(int UserId)
        {
            return _admincontroller.GetUser(UserId);
        }
       
        public ItemAccount GetItemAccount(int ItemAccountId)
        {
            return _settingController.GetItemAccount(ItemAccountId);
        }
        public void SaveOrUpdateMaintenanceRequest(MaintenanceRequest MaintenanceRequest)
        {
            _controller.SaveOrUpdateEntity(MaintenanceRequest);
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }
        public void DeleteMaintenanceRequest(MaintenanceRequest MaintenanceRequest)
        {
            _controller.DeleteEntity(MaintenanceRequest);
        }
        public MaintenanceRequest GetMaintenanceRequestById(int id)
        {
            return _controller.GetMaintenanceRequest(id);
        }
        public ApprovalSetting GetApprovalSetting(string RequestType, int value)
        {
            return _settingController.GetApprovalSettingforProcess(RequestType, value);
        }
        public ApprovalSetting GetApprovalSettingforProcess(string Requesttype, decimal value)
        {
            return _settingController.GetApprovalSettingforProcess(Requesttype, value);
        }

        public IList<MaintenanceRequest> ListMaintenanceRequests(string requestNo, string RequestDate, string ProgressStatus)
        {
            return _controller.ListMaintenanceRequests(requestNo, RequestDate, ProgressStatus);
        }

        public AssignJob GetAssignedJobbycurrentuser()
        {
            return _controller.GetAssignedJobbycurrentuser();
        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public AssignJob GetAssignedJobbycurrentuser(int userId)
        {
            return _controller.GetAssignedJobbycurrentuser(userId);
        }
        public int GetAssignedUserbycurrentuser()
        {
            return _controller.GetAssignedUserbycurrentuser();
        }
        public void Commit()
        {
            _controller.Commit();
        }// TODO: Handle other view events and set state in the view

    }
}




