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
    public class StockPresenter : Presenter<IStockListView>
    {
        private InventoryController _controller;
        private SettingController _settingController;
        private AdminController _adminController;
        private Stock _stock;
        public StockPresenter([CreateNew] InventoryController controller, [CreateNew] SettingController settingController, [CreateNew] AdminController adminController)
        {
            _controller = controller;
            _settingController = settingController;
            _adminController = adminController;
        }

        public override void OnViewLoaded()
        {
            if (View.GetStockId > 0)
            {
                _controller.CurrentObject = _controller.GetStock(View.GetStockId);
            }
            CurrentStock = _controller.CurrentObject as Stock;
        }
        public override void OnViewInitialized()
        {
            if (_stock == null)
            {
                int id = View.GetStockId;
                if (id > 0)
                {
                    _controller.CurrentObject = _controller.GetStock(id);
                }
                else
                {
                    _controller.CurrentObject = new Stock();
                }
            }
        }
        public Stock CurrentStock
        {
            get
            {
                if (_stock == null)
                {
                    int id = View.GetStockId;
                    if (id > 0)
                        _stock = _controller.GetStock(id);
                    else
                        _stock = new Stock();
                }
                return _stock;
            }
            set { _stock = value; }
        }
        public IList<Item> GetItems()
        {
            return _settingController.GetItems();
        }
        public Item GetItem(int Id)
        {
            return _settingController.GetItem(Id);
        }
        public Stock GetStock(int ItemId)
        {
            return _controller.GetStock(ItemId);
        }
        public AppUser GetUser()
        {
            return _controller.GetCurrentUser();
        }
        public Stock GetStockById(int id)
        {
            return _controller.GetStock(id);
        }
        public IList<Stock> ListStocks(string item)
        {
            return _controller.ListStocks(item);
        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public void SaveOrUpdateStock()
        {
            Stock stock = CurrentStock;
            stock.Item = GetItem(Convert.ToInt32(View.GetItem));
            stock.Quantity = Convert.ToInt32(View.GetQuantity);
            stock.Status = "Active";

            _controller.SaveOrUpdateEntity(stock);
        }
        public void SaveOrUpdateStock(Stock stock)
        {
            _controller.SaveOrUpdateEntity(stock);
        }
        public void DeleteStock(Stock Stock)
        {
            _controller.DeleteEntity(Stock);
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/WorkflowManagment/Default.aspx?{0}=3", AppConstants.TABID));
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}




