using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.CoreDomain.Inventory;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class StorePresenter : Presenter<IStore>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private SettingController _controller;
        public StorePresenter([CreateNew] SettingController controller)
        {
            _controller = controller;
        }
        public override void OnViewLoaded()
        {
            View.GetStores = _controller.ListStores(View.GetName, View.GetLocation);
        }
        public override void OnViewInitialized()
        {

        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public IList<Store> GetStores()
        {
            return _controller.GetStores();
        }
        public Store GetStoreById(int id)
        {
            return _controller.GetStore(id);
        }
        public IList<Store> ListStores(string Name, string Location)
        {
            return _controller.ListStores(Name, Location);
        }
        public void DeleteStore(Store Store)
        {
            _controller.DeleteEntity(Store);
        }
        public void SaveOrUpdateStore(Store Store)
        {
            _controller.SaveOrUpdateEntity(Store);
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}




