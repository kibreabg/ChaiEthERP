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
    public class ItemSubCategoryPresenter : Presenter<IItemSubCategoryView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private SettingController _controller;
        public ItemSubCategoryPresenter([CreateNew] SettingController controller)
        {
            _controller = controller;
        }
        public override void OnViewLoaded()
        {
            View.GetItemSubCategories = _controller.ListItemSubCategories(View.GetSubCategoryName, View.GetSubCategoryCode);
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
        public IList<ItemSubCategory> GetItemSubCategories()
        {
            return _controller.GetItemSubCategories();
        }                
        public ItemSubCategory GetItemSubCategoryById(int id)
        {
            return _controller.GetItemSubCategory(id);
        }
        public IList<ItemSubCategory> ListItemSubCategories(string ItemSubCategoryName, string ItemSubCategoryCode)
        {
            return _controller.ListItemSubCategories(ItemSubCategoryName, ItemSubCategoryCode);
        }
        public void DeleteItemSubCategory(ItemSubCategory ItemSubCategory)
        {
            _controller.DeleteEntity(ItemSubCategory);
        }
        public void SaveOrUpdateItemSubCategory(ItemSubCategory ItemSubCategory)
        {
            _controller.SaveOrUpdateEntity(ItemSubCategory);
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




