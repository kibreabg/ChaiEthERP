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
using Chai.WorkflowManagment.Modules.Admin;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared.MailSender;
using Chai.WorkflowManagment.CoreDomain.Inventory;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public class MaintenaceRequestPresenter : Presenter<IMaintenanceRequestView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
         private Chai.WorkflowManagment.Modules.Request.RequestController _controller;
         private Chai.WorkflowManagment.Modules.Setting.SettingController _settingcontroller;
        private Chai.WorkflowManagment.Modules.Inventory.InventoryController _inventorycontroller;
        private AdminController _adminController;
        
        private MaintenanceRequest _maintenancerequest;
         public MaintenaceRequestPresenter([CreateNew] Chai.WorkflowManagment.Modules.Request.RequestController controller, [CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController settingcontroller, AdminController adminController, [CreateNew] Chai.WorkflowManagment.Modules.Inventory.InventoryController inventorycontroller)
         {
         		_controller = controller;
                _settingcontroller = settingcontroller;
                _adminController = adminController;
                _inventorycontroller = inventorycontroller;
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
        public IList<Item> GetItems()
        {
            return _settingcontroller.GetItems();

        }
        public ServiceType GetServiceType(int Id)
        {
            return _settingcontroller.GetServiceType(Id);

        }
        public ServiceTypeDetail GetServiceTypeDetail(int Id)
        {
           return _settingcontroller.GetServiceTypeDetail(Id);

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
        public MaintenanceSparePart GetMaintenanceSparePart(int Id)
        {
            return _controller.GetMaintenanceSparePart(Id);

        }
        public Item GetItem(int Id)
        {
            return _inventorycontroller.GetItem(Id);

        }
        private void SaveMaintenanceRequestStatus()
        {
            if (GetApprovalSetting(RequestType.Maintenance_Request.ToString().Replace('_', ' '), 0) != null)
            {
                int i = 1;
                foreach (ApprovalLevel AL in GetApprovalSetting(RequestType.Maintenance_Request.ToString().Replace('_', ' '), 0).ApprovalLevels)
                {
                    MaintenanceRequestStatus MRS = new MaintenanceRequestStatus();
                    MRS.MaintenanceRequest = CurrentMaintenanceRequest;
                    if (AL.EmployeePosition.PositionName == "Superviser/Line Manager")
                    {
                        if (CurrentUser().Superviser != 0)
                            MRS.Approver = CurrentUser().Superviser.Value;
                        else
                        {
                            MRS.ApprovalStatus = ApprovalStatus.Approved.ToString();
                            MRS.Date = Convert.ToDateTime(DateTime.Today.Date.ToShortDateString());
                        }

                    }
                    else if (AL.EmployeePosition.PositionName == "Program Manager")
                    {
                        if (CurrentMaintenanceRequest.Project.Id != 0)
                        {
                            MRS.Approver = GetProject(CurrentMaintenanceRequest.Project.Id).AppUser.Id;
                        }
                    }
                    else
                    {
                        if (Approver(AL.EmployeePosition.Id).Id != 0)
                            MRS.Approver = Approver(AL.EmployeePosition.Id).Id;
                        else
                            MRS.Approver = 0;
                    }

                    //else
                    //{
                    //    PRS.Approver = _presenter.Approver(AL.EmployeePosition.Id).Id;
                    //}
                    MRS.WorkflowLevel = i;
                    i++;
                    CurrentMaintenanceRequest.MaintenanceRequestStatuses.Add(MRS);
                }
            }
           
        }
        private void GetCurrentApprover()
        {
            if (CurrentMaintenanceRequest.MaintenanceRequestStatuses != null)
            {
                foreach (MaintenanceRequestStatus MRS in CurrentMaintenanceRequest.MaintenanceRequestStatuses)
                {
                    if (MRS.ApprovalStatus == null)
                    {
                        SendEmail(MRS);
                        CurrentMaintenanceRequest.CurrentApprover = MRS.Approver;
                        CurrentMaintenanceRequest.CurrentLevel = MRS.WorkflowLevel;
                        CurrentMaintenanceRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                        break;
                    }
                }
            }
        }
        private void SendEmail(MaintenanceRequestStatus VRS)
        {
            if (GetSuperviser(VRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(GetSuperviser(VRS.Approver).Email, "Car Maintenance Request", (CurrentMaintenanceRequest.AppUser.FullName).ToUpper() + " Requests for Car Maintenance with Request No. - " + (CurrentMaintenanceRequest.RequestNo).ToUpper() + "'");
            }
            else
            {
                EmailSender.Send(GetSuperviser(_controller.GetAssignedJobbycurrentuser(VRS.Approver).AssignedTo).Email, " Car Maintenance Request", (CurrentMaintenanceRequest.AppUser.FullName).ToUpper() + " Requests for Car Maintenance with Request No. - " + (CurrentMaintenanceRequest.RequestNo).ToUpper() + "'");
            }
        }
        public void SaveOrUpdateMaintenanceRequest()
        {
            MaintenanceRequest MaintenanceRequest = CurrentMaintenanceRequest;
            MaintenanceRequest.RequestNo = View.RequestNo;
            MaintenanceRequest.RequestDate = Convert.ToDateTime(View.RequestDate);
            MaintenanceRequest.PlateNo = View.GetPlateNo;
            MaintenanceRequest.KmReading = Convert.ToInt32(View.GetKmReading);
            MaintenanceRequest.Remark = View.GetRemark;
            MaintenanceRequest.ActionTaken = View.GetActionTaken;
            MaintenanceRequest.AppUser = _adminController.GetUser(CurrentUser().Id);
            MaintenanceRequest.Requester= Convert.ToInt32(CurrentMaintenanceRequest.AppUser.Id);
            MaintenanceRequest.AppUser = _adminController.GetUser(CurrentUser().Id);

            if (View.GetProjectId != 0)
                MaintenanceRequest.Project = _settingcontroller.GetProject(View.GetProjectId);
            if (View.GetGrantId != 0)
                MaintenanceRequest.Grant = _settingcontroller.GetGrant(View.GetGrantId);

            if (CurrentMaintenanceRequest.MaintenanceRequestStatuses.Count == 0)

                SaveMaintenanceRequestStatus();
            GetCurrentApprover();
            _controller.SaveOrUpdateEntity(MaintenanceRequest);
            _controller.CurrentObject = null;
        }
        public AppUser CurrentUser()
         {
             return _controller.GetCurrentUser();
         }
         public void DeleteMaintenanceRequestDetail(MaintenanceRequestDetail MaintenanceRequestDetail)
         {
             _controller.DeleteEntity(MaintenanceRequestDetail);
         }
        public void DeleteMaintenanceSparepart(MaintenanceSparePart MaintenanceSparePart)
        {
            _controller.DeleteEntity(MaintenanceSparePart);
        }
        public void Commit()
         {
             _controller.Commit();
         }

         public IList<Grant> GetGrantbyprojectId(int projectId)
         {
             return _settingcontroller.GetProjectGrantsByprojectId(projectId);

         }
        public IList<ServiceTypeDetail> GetServiceTypeDetbyTypeId(int serviceTypeId)
        {
            return _settingcontroller.GetServiceTypeDetbyTypeId(serviceTypeId);

        }
        public IList<Vehicle> GetVehicles()
        {
            return _settingcontroller.GetVehiclesForInternal();
        }
    }
}




