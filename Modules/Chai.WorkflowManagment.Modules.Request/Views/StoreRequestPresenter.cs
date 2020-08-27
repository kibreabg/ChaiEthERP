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
    public class StoreRequestPresenter : Presenter<IStoreRequestView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
         private Chai.WorkflowManagment.Modules.Request.RequestController _controller;
         private Chai.WorkflowManagment.Modules.Setting.SettingController _settingcontroller;
         private StoreRequest _StoreRequest;
         public StoreRequestPresenter([CreateNew] Chai.WorkflowManagment.Modules.Request.RequestController controller, [CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController settingcontroller)
         {
         		_controller = controller;
                _settingcontroller = settingcontroller;
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
                 if (_StoreRequest == null)
                 {
                     int id = View.StoreRequestId;
                     if (id > 0)
                         _StoreRequest = _controller.GetStoreRequest(id);
                     else
                         _StoreRequest = new StoreRequest();
                 }
                 return _StoreRequest;
             }
             set { _StoreRequest = value; }
         }
         public override void OnViewInitialized()
         {
             if (_StoreRequest == null)
             {
                 int id = View.StoreRequestId;
                 if (id > 0)
                     _controller.CurrentObject = _controller.GetStoreRequest(id);
                 else
                     _controller.CurrentObject = new StoreRequest();
             }
         }
         public IList<StoreRequest> GetStoreRequests()
         {
             return _controller.GetStoreRequests();
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
         public void SaveOrUpdateStoreRequest(StoreRequest StoreRequest)
         {
             _controller.SaveOrUpdateEntity(StoreRequest);
         }
         public int GetLastStoreRequestId()
         {
             return _controller.GetLastStoreRequestId();
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

         public ApprovalSetting GetApprovalSetting(string RequestType, decimal value)
         {
             return _settingcontroller.GetApprovalSettingforProcess(RequestType, value);
         }
         public ApprovalSetting GetApprovalSettingforPurchaseProcess(string RequestType, decimal value)
         {
             return _settingcontroller.GetApprovalSettingforPurchaseProcess(RequestType, value);
         }
         public AssignJob GetAssignedJobbycurrentuser()
         {
             return _controller.GetAssignedJobbycurrentuser();
         }
         public AssignJob GetAssignedJobbycurrentuser(int UserId)
         {
             return _controller.GetAssignedJobbycurrentuser(UserId);
         }
         public IList<StoreRequest> ListStoreRequests(string requestNo,string RequestDate)
         {
             return _controller.ListStoreRequests(requestNo, RequestDate);

         }
         public IList<ItemAccount> GetItemAccounts()
         {
             return _settingcontroller.GetItemAccounts();

         }
        public IList<Project> ListProjects(int programID)
        {
            return _settingcontroller.GetProjectsByProgramId(programID);
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
         public IList<Grant> GetGrants()
         {
             return _settingcontroller.GetGrants();

         }
       
         public Grant GetGrant(int Id)
         {
             return _settingcontroller.GetGrant(Id);

         }
         public StoreRequestDetail GetStoreRequestDetail(int Id)
         {
             return _controller.GetStoreRequestDetail(Id);

         }
        public IList<Program> GetPrograms()
        {
            return _controller.GetPrograms();
        }
        public AppUser CurrentUser()
         {
             return _controller.GetCurrentUser();
         }
         public void DeleteStoreRequestDetail(StoreRequestDetail StoreRequestDetail)
         {
             _controller.DeleteEntity(StoreRequestDetail);
         }
         public void Commit()
         {
             _controller.Commit();
         }

         public IList<Grant> GetGrantbyprojectId(int projectId)
         {
             return _settingcontroller.GetProjectGrantsByprojectId(projectId);

         }
     
    }
}




