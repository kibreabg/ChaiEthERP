using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Inventory;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Modules.Setting;
using Chai.WorkflowManagment.Modules.Admin;

namespace Chai.WorkflowManagment.Modules.Inventory.Views
{
    public class FixedAssetHistoriesPresenter : Presenter<IFixedAssetHistoriesView>
    {
        private InventoryController _controller;
        private SettingController _settingController;
        private AdminController _adminController;
        public FixedAssetHistoriesPresenter([CreateNew] InventoryController controller, [CreateNew] SettingController settingController, [CreateNew] AdminController adminController)
        {
            _controller = controller;
            _settingController = settingController;
            _adminController = adminController;
        }

        public override void OnViewLoaded()
        {
        }
        public override void OnViewInitialized()
        {
        }
        public IList<Item> GetItems()
        {
            return _settingController.GetItems();
        }
        public IList<FixedAsset> GetFixedAssets()
        {
            return _controller.GetFixedAssets();
        }
        public IList<Program> GetPrograms()
        {
            return _settingController.GetPrograms();
        }
        public IList<FixedAssetHistory> ListFixedAssetHistories(string assetCode, string serialNo)
        {
            return _controller.ListFixedAssetHistories(assetCode, serialNo);
        }
    }
}




