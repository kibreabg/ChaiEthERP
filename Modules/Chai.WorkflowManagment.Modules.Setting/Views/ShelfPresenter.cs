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
    public class ShelfPresenter : Presenter<IShelfView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private SettingController _controller;
        public ShelfPresenter([CreateNew] SettingController controller)
        {
            _controller = controller;
        }
        public override void OnViewLoaded()
        {
            View.GetShelfs = _controller.ListShelfs(View.GetShelfName, View.GetShelfCode);
        }
        public override void OnViewInitialized()
        {

        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public IList<Section> GetSections()
        {
            return _controller.GetSections();
        }
        public Section GetSectionById(int id)
        {
            return _controller.GetSec(id);
        }
      
        public IList<Shelf> GetShelfs()
        {
            return _controller.GetShelfs();
        }
        public IList<Store> GetStores()
        {
            return _controller.GetStores();
        }
        public IList<Section> GetShelfByStoreId(int storeId)
        {
            return _controller.GetSectionBystoreId(storeId);

        }
        public Shelf GetShelfById(int id)
        {
            return _controller.GetShelf(id);
        }
        public IList<Shelf> ListShelfs(string ShelfName, string ShelfCode)
        {
            return _controller.ListShelfs(ShelfName, ShelfCode);
        }
        public void DeleteShelf(Shelf Shelf)
        {
            _controller.DeleteEntity(Shelf);
        }
        public void SaveOrUpdateShelf(Shelf Shelf)
        {
            _controller.SaveOrUpdateEntity(Shelf);
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




