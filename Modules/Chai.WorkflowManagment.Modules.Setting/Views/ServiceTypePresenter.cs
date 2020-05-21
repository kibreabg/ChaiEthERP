using System;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class ServiceTypePresenter : Presenter<IServiceTypeView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private SettingController _controller;
        public ServiceTypePresenter([CreateNew] SettingController controller)
        {
            _controller = controller;
        }
        public override void OnViewLoaded()
        {
            View.ServiceTypes = _controller.GetServiceTypes();
        }
        public override void OnViewInitialized()
        {

        }
        public IList<ServiceType> GetServiceTypes()
        {
            return _controller.GetServiceTypes();
        }
        public void SaveOrUpdateServiceType(ServiceType sType)
        {
            _controller.SaveOrUpdateEntity(sType);
        }
        public ServiceType GetServiceType(int sTypeId)
        {
            return _controller.GetServiceType(sTypeId);
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }
        public void DeleteServiceType(ServiceType serviceType)
        {
            _controller.DeleteEntity(serviceType);
        }
        public void DeleteServiceTypeDetail(ServiceTypeDetail serviceTypeDetail)
        {
            _controller.DeleteEntity(serviceTypeDetail);
        }
        public ServiceTypeDetail GetServiceTypeDetail(int Id)
        {
            return _controller.GetServiceTypeDetail(Id);
        }
        public IList<ServiceType> ListServiceTypes(string stName)
        {
            return _controller.ListServiceTypes(stName);
        }       
        public AppUser GetUser(int userid)
        {
            return _controller.GetUser(userid);
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




