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

namespace Chai.WorkflowManagment.Modules.Inventory.Views
{
    public class RecievePresenter : Presenter<IRecieveListView>
    {
        private InventoryController _controller;
        private SettingController _settingcontroller;
        private Receive _recieve;
        public RecievePresenter([CreateNew] InventoryController controller, [CreateNew] SettingController settingcontroller)
        {
            _controller = controller;
            _settingcontroller = settingcontroller;
        }

        public override void OnViewLoaded()
        {

            if (View.GetId > 0)
            {
                _recieve = _controller.GetReceive(View.GetId);
            }
            else
            {
                _recieve = new Receive();
            }

            CurrentRecieve = _controller.CurrentObject as Receive;
        }

        public override void OnViewInitialized()
        {
            if (_recieve == null)
            {
                int id = View.GetId;
                if (id > 0)
                {
                    _controller.CurrentObject = _controller.GetReceive(id);
                }
                else
                {
                    _controller.CurrentObject = new Receive();
                }

            }
        }
        public Receive CurrentRecieve
        {
            get
            {
                if (_recieve == null)
                {
                    int id = View.GetId;
                    if (id > 0)
                        _recieve = _controller.GetReceive(id);
                    else
                        _recieve = new Receive();
                }
                return _recieve;
            }
            set { _recieve = value; }            
        }
        public IList<Item> GetItemList()
        {
            return _settingcontroller.GetItems();
        }
        public Item GetItem(int Id)
        {
            return _settingcontroller.GetItem(Id);
        }
        public int GetLastId()
        {
            return _controller.GetLastReceiveId();
        }
        public Stock GetStock(int ItemId)
        {
            return _controller.GetStock(ItemId);
        }
        public Stock GetStocks(int ItemId)
        {
            return _controller.GetStocks(ItemId);
        }
        public AppUser GetUser()
        {
            return _controller.GetCurrentUser();
        }
        public void SaveOrUpdateRecieve(Receive recieve)
        {
            _controller.SaveOrUpdateEntity(recieve);
        }
        public void SaveOrUpdateStock(Stock stock)
        {
            _controller.SaveOrUpdateEntity(stock);
        }        
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/ERP/Default.aspx?{0}=3", AppConstants.TABID));
        }
        public void DeleteRecieve(Receive recieve)
        {
            _controller.DeleteEntity(recieve);
        }
        public void DeleteRecieveDetail(ReceiveDetail recievedetail)
        {
            _controller.DeleteEntity(recievedetail);
        }
        public Receive GetReceiveById(int id)
        {
            return _controller.GetReceive(id);
        }
        public ReceiveDetail GetReceiveDetailById(int id)
        {
            return _controller.GetReceiveDetail(id);
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}




