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
    public class ReceivePresenter : Presenter<IReceiveListView>
    {
        private InventoryController _controller;
        private SettingController _settingController;
        private AdminController _adminController;
        private Receive _receive;
        public ReceivePresenter([CreateNew] InventoryController controller, [CreateNew] SettingController settingController, [CreateNew] AdminController adminController)
        {
            _controller = controller;
            _settingController = settingController;
            _adminController = adminController;
        }

        public override void OnViewLoaded()
        {
            if (View.GetReceiveId > 0)
            {
                _controller.CurrentObject = _controller.GetReceive(View.GetReceiveId);
            }
            CurrentReceive = _controller.CurrentObject as Receive;
        }
        public override void OnViewInitialized()
        {
            if (_receive == null)
            {
                int id = View.GetReceiveId;
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
        public Receive CurrentReceive
        {
            get
            {
                if (_receive == null)
                {
                    int id = View.GetReceiveId;
                    if (id > 0)
                        _receive = _controller.GetReceive(id);
                    else
                        _receive = new Receive();
                }
                return _receive;
            }
            set { _receive = value; }
        }
        public IList<Program> GetPrograms()
        {
            return _settingController.GetPrograms();
        }
        public IList<Project> ListProjects(int programID)
        {
            return _settingController.GetProjectsByProgramId(programID);
        }
        public IList<Grant> GetGrantbyprojectId(int projectId)
        {
            return _settingController.GetProjectGrantsByprojectId(projectId);

        }
        public IList<Supplier> GetSuppliers()
        {
            return _settingController.GetSuppliers();
        }
        public IList<Item> GetItemList()
        {
            return _settingController.GetItems();
        }
        public Item GetItem(int Id)
        {
            return _settingController.GetItem(Id);
        }
        public int GetLastReceiveId()
        {
            return _controller.GetLastReceiveId();
        }
        public Stock GetStock(int id)
        {
            return _controller.GetStock(id);
        }
        public Stock GetStockByItem(int itemId)
        {
            return _controller.GetStockByItem(itemId);
        }
        public ItemCategory GetItemCategory(int categoryId)
        {
            return _settingController.GetItemCategory(categoryId);
        }
        public IList<ItemCategory> GetItemCategories()
        {
            return _settingController.GetItemCategories();
        }
        public ItemSubCategory GetItemSubCategory(int subCategoryId)
        {
            return _settingController.GetItemSubCategory(subCategoryId);
        }
        public IList<ItemSubCategory> GetItemSubCatsByCategoryId(int catId)
        {
            return _settingController.GetItemSubCatsByCategoryId(catId);
        }
        public IList<Item> GetItemsBySubCatId(int subCatId)
        {
            return _settingController.GetItemsBySubCatId(subCatId);
        }
        public IList<Section> GetSectionsByStoreId(int storeId)
        {
            return _settingController.GetSectionBystoreId(storeId);
        }
        public Store GetStore(int storeId)
        {
            return _settingController.GetStore(storeId);
        }
        public IList<Store> GetStores()
        {
            return _settingController.GetStores();
        }
        public Section GetSection(int sectionId)
        {
            return _settingController.GetSection(sectionId);
        }
        public Shelf GetShelf(int shelfId)
        {
            return _settingController.GetShelf(shelfId);
        }
        public IList<Shelf> GetShelvesBySectionId(int sectionId)
        {
            return _settingController.GetShelvesBySectionId(sectionId);
        }
        public AppUser GetUser()
        {
            return _controller.GetCurrentUser();
        }
        public Receive GetReceiveById(int id)
        {
            return _controller.GetReceive(id);
        }
        public IList<Receive> ListReceives(string ReceiveNo, string ReceiveDate)
        {
            return _controller.ListReceives(ReceiveNo, ReceiveDate);
        }
        public ReceiveDetail GetReceiveDetailById(int id)
        {
            return _controller.GetReceiveDetail(id);
        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public void SaveOrUpdateReceive()
        {
            Receive receive = CurrentReceive;

            if (receive.Id <= 0)
            {
                receive.ReceiveNo = View.GetReceiveNo;
            }
            receive.ReceiveDate = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            receive.DeliveredBy = View.GetDeliveredBy;
            receive.InvoiceNo = View.GetInvoiceNo;
            receive.Program = _settingController.GetProgram(View.GetProgram);
            receive.Project = _settingController.GetProject(View.GetProject);
            receive.Grant = _settingController.GetGrant(View.GetGrant);
            receive.Receiver = _adminController.GetUser(CurrentUser().Id).Id;
            receive.Supplier = _settingController.GetSupplier(View.GetSupplier);

            foreach (ReceiveDetail recDet in CurrentReceive.ReceiveDetails)
            {
                if (recDet.Item.ItemType == "Fixed Asset")
                {
                    for (int i = 1; i <= recDet.Quantity; i++)
                    {
                        FixedAsset fa = new FixedAsset();
                        fa.Item = recDet.Item;
                        fa.ReceiveDate = CurrentReceive.ReceiveDate;
                        fa.Supplier = CurrentReceive.Supplier;
                        fa.Store = recDet.Store;
                        fa.Section = recDet.Section;
                        fa.Shelf = recDet.Shelf;
                        fa.ReceiveNo = CurrentReceive.ReceiveNo;
                        fa.Custodian = "Store";
                        fa.UnitCost = recDet.UnitCost;
                        fa.AssetStatus = "Received";

                        FixedAssetHistory fah = new FixedAssetHistory();
                        fah.Custodian = "Store";
                        fah.Operation = "Receive";
                        fah.TransactionDate = DateTime.Now;

                        fa.FixedAssetHistories.Add(fah);

                        _controller.SaveOrUpdateEntity(fa);
                    }
                }

                //Add the received quantity to the stock
                Stock stock = GetStockByItem(recDet.Item.Id);
                if (stock == null)
                {
                    stock = new Stock();
                }
                else
                {
                    stock.Quantity += recDet.Quantity;
                    _controller.SaveOrUpdateEntity(stock);
                }

            }

            _controller.SaveOrUpdateEntity(receive);
        }
        public void SaveOrUpdateStock(Stock stock)
        {
            _controller.SaveOrUpdateEntity(stock);
        }
        public void DeleteReceive(Receive receive)
        {
            _controller.DeleteEntity(receive);
        }
        public void DeleteReceiveDetail(ReceiveDetail receiveDetail)
        {
            _controller.DeleteEntity(receiveDetail);
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




