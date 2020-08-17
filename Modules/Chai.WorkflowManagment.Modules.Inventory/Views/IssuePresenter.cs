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
    public class IssuePresenter : Presenter<IIssueListView>
    {


        private InventoryController _controller;
        private SettingController _settingcontroller;
        private Issue _issue;
        public IssuePresenter([CreateNew] InventoryController controller, [CreateNew] SettingController settingcontroller)
        {
            _controller = controller;
            _settingcontroller = settingcontroller;
        }

        public override void OnViewLoaded()
        {            
            if (View.GetId > 0)
            {
                _issue = _controller.GetIssue(View.GetId);
            }
            else
            {
                _issue = new Issue();
            }

            CurrentIssue = _controller.CurrentObject as Issue;
        
        }

        public override void OnViewInitialized()
        {
            if (_issue == null)
            {
                int id = View.GetId;
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
                if (_issue == null)
                {
                    int id = View.GetId;
                    if (id > 0)
                        _issue = _controller.GetIssue(id);
                    else
                        _issue = new Issue();
                }
                return _issue;
            }
            set { _issue = value; }
        }
               
        public AppUser GetUser()
        {
            return _controller.GetCurrentUser();
        }
        public void SaveOrUpdateIssue(Issue issue)
        {
            _controller.SaveOrUpdateEntity(issue);
        }
        public void SaveOrUpdateStock(Stock stock)
        {
            _controller.SaveOrUpdateEntity(stock);
        }
        public int GetLastId()
        {
            return _controller.GetLastIssueId();
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/ERP/Default.aspx?{0}=3", AppConstants.TABID));
        }
        public void DeleteIssue(Issue issue)
        {
            _controller.DeleteEntity(issue);
        }
        public void DeleteissueDetail(IssueDetail issuedetail)
        {
            _controller.DeleteEntity(issuedetail);
        }
        public Issue GetIssueById(int id)
        {
            return _controller.GetIssue(id);
        }
        public IssueDetail GetIssueDetailById(int id)
        {
            return _controller.GetIssueDetail(id);
        }
        public Stock GetStocks(int ItemId)
        {
            return _controller.GetStocks(ItemId);
        }       
        public IList<Item> GetItemList()
        {
            return _settingcontroller.GetItems();
        }
        public Item GetItem(int Id)
        {
            return _settingcontroller.GetItem(Id);
        }
         public void Commit()
        {
            _controller.Commit();
        }
    }
}




