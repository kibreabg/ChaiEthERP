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
    public class IssuePresenter : Presenter<IIssueListView>
    {
        private InventoryController _controller;
        private SettingController _settingController;
        private AdminController _adminController;
        private Issue _Issue;
        public IssuePresenter([CreateNew] InventoryController controller, [CreateNew] SettingController settingController, [CreateNew] AdminController adminController)
        {
            _controller = controller;
            _settingController = settingController;
            _adminController = adminController;
        }

        public override void OnViewLoaded()
        {
            if (View.GetIssueId > 0)
            {
                _controller.CurrentObject = _controller.GetIssue(View.GetIssueId);
            }
            CurrentIssue = _controller.CurrentObject as Issue;
        }
        public override void OnViewInitialized()
        {
            if (_Issue == null)
            {
                int id = View.GetIssueId;
                if (id > 0)
                {
                    _controller.CurrentObject = _controller.GetIssue(id);
                }
                else
                {
                    _controller.CurrentObject = new Issue();
                }

            }
        }
        public Issue CurrentIssue
        {
            get
            {
                if (_Issue == null)
                {
                    int id = View.GetIssueId;
                    if (id > 0)
                        _Issue = _controller.GetIssue(id);
                    else
                        _Issue = new Issue();
                }
                return _Issue;
            }
            set { _Issue = value; }
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
        public int GetLastIssueId()
        {
            return _controller.GetLastIssueId();
        }
        public Stock GetStock(int ItemId)
        {
            return _controller.GetStock(ItemId);
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
        public IList<AppUser> GetUsers()
        {
            return _adminController.GetUsers();
        }
        public Issue GetIssueById(int id)
        {
            return _controller.GetIssue(id);
        }
        public IList<Issue> ListIssues(string IssueNo, string IssueDate)
        {
            return _controller.ListIssues(IssueNo, IssueDate);
        }
        public IssueDetail GetIssueDetailById(int id)
        {
            return _controller.GetIssueDetail(id);
        }
        public ApprovalSetting GetApprovalSetting(string RequestType, decimal value)
        {
            return _settingController.GetApprovalSettingforProcess(RequestType, value);
        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public void SaveOrUpdateIssue()
        {
            Issue issue = CurrentIssue;

            if (issue.Id <= 0)
            {
                issue.IssueNo = View.GetIssueNo;
            }
            issue.IssueDate = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            issue.HandedOverBy = View.GetHandedOverBy;
            issue.IssuedTo = View.GetIssuedTo;
            issue.Purpose = View.GetPurpose;

            _controller.SaveOrUpdateEntity(issue);
        }
        public void SaveOrUpdateStock(Stock stock)
        {
            _controller.SaveOrUpdateEntity(stock);
        }
        public void DeleteIssue(Issue Issue)
        {
            _controller.DeleteEntity(Issue);
        }
        public void DeleteIssueDetail(IssueDetail IssueDetail)
        {
            _controller.DeleteEntity(IssueDetail);
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




