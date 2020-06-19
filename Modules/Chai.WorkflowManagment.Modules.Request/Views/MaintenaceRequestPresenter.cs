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

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public class MaintenaceRequestPresenter : Presenter<IMaintenanceRequestView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
         private Chai.WorkflowManagment.Modules.Request.RequestController _controller;
         private Chai.WorkflowManagment.Modules.Setting.SettingController _settingcontroller;
         private MaintenanceRequest _maintenancerequest;
         public MaintenaceRequestPresenter([CreateNew] Chai.WorkflowManagment.Modules.Request.RequestController controller, [CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController settingcontroller)
         {
         		_controller = controller;
                _settingcontroller = settingcontroller;
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
                 if (_maintenancerequest == null)
                 {
                     int id = View.MaintenanceRequestId;
                     if (id > 0)
                        _maintenancerequest = _controller.GetMaintenanceRequest(id);
                     else
                        _maintenancerequest = new MaintenanceRequest();
                 }
                 return _maintenancerequest;
             }
             set { _maintenancerequest = value; }
         }
         public override void OnViewInitialized()
         {
             if (_maintenancerequest == null)
             {
                 int id = View.MaintenanceRequestId;
                 if (id > 0)
                     _controller.CurrentObject = _controller.GetMaintenanceRequest(id);
                 else
                     _controller.CurrentObject = new MaintenanceRequest();
             }
         }
         public IList<MaintenanceRequest> GetMaintenanceRequests()
         {
             return _controller.GetMaintenanceRequests();
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
         public void SaveOrUpdateLeaveMaintenance(MaintenanceRequest MaintenanceRequest)
         {
             _controller.SaveOrUpdateEntity(MaintenanceRequest);
         }
         public int GetLastMaintenanceRequestId()
         {
             return _controller.GetLastMaintenanceRequestId();
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

         public ApprovalSetting GetApprovalSetting(string RequestType, decimal value)
         {
             return _settingcontroller.GetApprovalSettingforProcess(RequestType, value);
         }
         public ApprovalSetting GetApprovalSettingforMaintenanceProcess(string RequestType, decimal value)
         {
             return _settingcontroller.GetApprovalSettingforProcess(RequestType, value);
         }
         public AssignJob GetAssignedJobbycurrentuser()
         {
             return _controller.GetAssignedJobbycurrentuser();
         }
         public AssignJob GetAssignedJobbycurrentuser(int UserId)
         {
             return _controller.GetAssignedJobbycurrentuser(UserId);
         }
         public IList<MaintenanceRequest> ListMaintenanceRequests(string requestNo,string RequestDate)
         {
             return _controller.ListMaintenanceRequests(requestNo, RequestDate);

         }
         public IList<ItemAccount> GetItemAccounts()
         {
             return _settingcontroller.GetItemAccounts();

         }
         public ItemAccount GetItemAccount(int Id)
         {
             return _settingcontroller.GetItemAccount(Id);

         }
        public IList<ServiceType> GetServiceTypes()
        {
            return _settingcontroller.GetServiceTypes();

        }
        public ServiceType GetServiceType(int Id)
        {
            return _settingcontroller.GetServiceType(Id);

        }
        public IList<ServiceTypeDetail> GetServiceTypeDetails(int Id)
        {
           return _settingcontroller.GetServiceTypeDetails(Id);

        }
        //public IList<ServiceTypeDetail> GetServiceTypeDetails()
        //{
        //    return _settingcontroller.GetServiceTypeDetails();

        //}
        public IList<Project> GetProjects()
         {
             return _settingcontroller.GetProjects();

         }
         public Project GetProject(int Id)
         {
             return _settingcontroller.GetProject(Id);

         }
         public IList<Grant> GetGrants()
         {
             return _settingcontroller.GetGrants();

         }
       
         public Grant GetGrant(int Id)
         {
             return _settingcontroller.GetGrant(Id);

         }
         public MaintenanceRequestDetail GetMaintenanceRequestDetail(int Id)
         {
             return _controller.GetMaintenanceRequestDetail(Id);

         }
         public AppUser CurrentUser()
         {
             return _controller.GetCurrentUser();
         }
         public void DeleteMaintenanceRequestDetail(MaintenanceRequestDetail MaintenanceRequestDetail)
         {
             _controller.DeleteEntity(MaintenanceRequestDetail);
         }
         public void Commit()
         {
             _controller.Commit();
         }

         public IList<Grant> GetGrantbyprojectId(int projectId)
         {
             return _settingcontroller.GetProjectGrantsByprojectId(projectId);

         }
        public IList<Vehicle> GetVehicles()
        {
            return _settingcontroller.GetVehicles();
        }
    }
}




