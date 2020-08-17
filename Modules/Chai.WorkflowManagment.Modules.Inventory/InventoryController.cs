using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Utility;
using Microsoft.Practices.ObjectBuilder;

using Chai.WorkflowManagment.CoreDomain;
using Chai.WorkflowManagment.CoreDomain.DataAccess;
using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Services;
using Chai.WorkflowManagment.Shared.Navigation;


using System.Data;
using Chai.WorkflowManagment.CoreDomain.Inventory;
using Chai.WorkflowManagment.CoreDomain.Setting;


namespace Chai.WorkflowManagment.Modules.Inventory
{
    public class InventoryController : ControllerBase
    {
        private IWorkspace _workspace;

        [InjectionConstructor]
        public InventoryController([ServiceDependency] IHttpContextLocatorService httpContextLocatorService, [ServiceDependency]INavigationService navigationService)
            : base(httpContextLocatorService, navigationService)
        {
            _workspace = ZadsServices.Workspace;
        }
        
        #region CurrenrObject
        public object CurrentObject
        {
            get
            {
                return GetCurrentContext().Session["CurrentObject"];
            }
            set
            {
                GetCurrentContext().Session["CurrentObject"] = value;
            }
        }
        #endregion
           
        #region Item Receive
        public int GetLastReceiveId()
        {
            if (_workspace.Last<Receive>() != null)
            {
                return _workspace.Last<Receive>().Id;
            }
            else { return 0; }
        }      
        public Receive GetReceive(int Id)
        {
            return _workspace.Single<Receive>(x => x.Id == Id, y => y.ReceiveDetails);
        }
        public ReceiveDetail GetReceiveDetail(int Id)
        {
            return _workspace.Single<ReceiveDetail>(x => x.Id == Id);
        }
        public IList<Receive> ListReceive(string InvoiceNo, DateTime ReceiveDate)
        {
            return WorkspaceFactory.CreateReadOnly().Query<Receive>(x => x.InvoiceNo == InvoiceNo && x.ReceiveDate == ReceiveDate).ToList();
        }

        #endregion
        #region Item Issuance
        public int GetLastIssueId()
        {
            if (_workspace.Last<Issue>() != null)
            {
                return _workspace.Last<Issue>().Id;
            }
            else { return 0; }
        }
        public Issue GetIssue(int Id)
        {
            return _workspace.Single<Issue>(x => x.Id == Id, y => y.IssueDetails);
        }
        public IList<Issue> ListIssue(string issueNo, DateTime issueDate)
        {
            return WorkspaceFactory.CreateReadOnly().Query<Issue>(x => x.IssueNo == issueNo && x.IssueDate == issueDate).ToList();
        }
        public IssueDetail GetIssueDetail(int Id)
        {
            return _workspace.Single<IssueDetail>(x => x.Id == Id);
        }
        #endregion
        
        #region Stock
        public Stock GetStocks(int ItemId)
        {
            return _workspace.Single<Stock>(x => x.Item.Id == ItemId);
        }
        public Stock GetStock(int ItemId)
        {
            return _workspace.Single<Stock>(x => x.Item.Id == ItemId, y => y.Item);
        }
        #endregion
        #region Entity Manipulation
        public void SaveOrUpdateEntity<T>(T item) where T : class
        {
            IEntity entity = (IEntity)item;
            if (entity.Id == 0)
                _workspace.Add<T>(item);
            else
                _workspace.Update<T>(item);

            _workspace.CommitChanges();
        }
        public void DeleteEntity<T>(T item) where T : class
        {
            _workspace.Delete<T>(item);
            _workspace.CommitChanges();
        }

        public void Commit()
        {
            _workspace.CommitChanges();
        }
        #endregion               
    }
}
