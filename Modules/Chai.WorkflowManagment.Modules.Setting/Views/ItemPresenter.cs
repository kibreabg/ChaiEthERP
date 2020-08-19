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
    public class ItemPresenter : Presenter<IItemView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private SettingController _controller;
        public ItemPresenter([CreateNew] SettingController controller)
        {
            _controller = controller;
        }
        public override void OnViewLoaded()
        {
            View.GetItems = _controller.ListItems(View.GetItemName, View.GetItemCode);
        }
        public override void OnViewInitialized()
        {

        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public IList<ItemSubCategory> GetItemSubCategories()
        {
            return _controller.GetItemSubCategories();
        }
        public ItemSubCategory GetItemSubCategoryById(int id)
        {
            return _controller.GetItemSubCategory(id);
        }
        public IList<UnitOfMeasurement> GetUnitOfMeasurements()
        {
            return _controller.GetUnitOfMeasurements();
        }
        public UnitOfMeasurement GetUnitOfMeasurement(int id)
        {
            return _controller.GetUnitOfMeasurement(id);
        }
        public IList<Item> GetItems()
        {
            return _controller.GetItems();
        }                
        public Item GetItemById(int id)
        {
            return _controller.GetItem(id);
        }
        public IList<Item> ListItems(string ItemName, string ItemCode)
        {
            return _controller.ListItems(ItemName, ItemCode);
        }
        public void DeleteItem(Item Item)
        {
            _controller.DeleteEntity(Item);
        }
        public void SaveOrUpdateItem(Item Item)
        {
            _controller.SaveOrUpdateEntity(Item);
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




