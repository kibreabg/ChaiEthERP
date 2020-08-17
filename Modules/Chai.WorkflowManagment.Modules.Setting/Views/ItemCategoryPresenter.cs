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
    public class ItemCategoryPresenter : Presenter<IItemCategoryView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private SettingController _controller;
        public ItemCategoryPresenter([CreateNew] SettingController controller)
        {
            _controller = controller;
        }
        public override void OnViewLoaded()
        {
            View.GetItemCategories = _controller.ListItemCategories(View.GetCategoryName, View.GetCategoryCode);
        }
        public override void OnViewInitialized()
        {

        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public IList<ItemCategory> GetItemCategories()
        {
            return _controller.GetItemCategories();
        }                
        public ItemCategory GetItemCategoryById(int id)
        {
            return _controller.GetItemCategory(id);
        }
        public IList<ItemCategory> ListItemCategories(string itemCategoryName, string itemCategoryCode)
        {
            return _controller.ListItemCategories(itemCategoryName, itemCategoryCode);
        }
        public void DeleteItemCategory(ItemCategory itemCategory)
        {
            _controller.DeleteEntity(itemCategory);
        }
        public void SaveOrUpdateItemCategory(ItemCategory itemCategory)
        {
            _controller.SaveOrUpdateEntity(itemCategory);
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




