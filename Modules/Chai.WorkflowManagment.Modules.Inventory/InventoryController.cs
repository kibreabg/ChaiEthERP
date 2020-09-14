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
        public IList<Receive> ListReceives(string ReceiveNo, string ReceiveDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM Receives Where 1 = Case when '" + ReceiveNo + "' = '' Then 1 When Receives.ReceiveNo = '" + ReceiveNo + "' Then 1 END And  1 = CASE WHEN '" + ReceiveDate + "' = '' Then 1 When Receives.ReceiveDate = '" + ReceiveDate + "'  Then 1 END AND Receives.Receiver = '" + GetCurrentUser().Id + "' ORDER BY Receives.Id Desc";
            return _workspace.SqlQuery<Receive>(filterExpression).ToList();
        }
        public Item GetItem(int id)
        {
            return _workspace.Single<Item>(x => x.Id == id);
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
        public IList<Issue> ListIssues(string IssueNo, string IssueDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM Issues WHERE 1 = CASE WHEN '" + IssueNo + "' = '' THEN 1 WHEN Issues.IssueNo = '" + IssueNo + "' THEN 1 END AND  1 = CASE WHEN '" + IssueDate + "' = '' THEN 1 WHEN Issues.IssueDate = '" + IssueDate + "'  Then 1 END AND Issues.HandedOverBy = '" + GetCurrentUser().Id + "' ORDER BY Issues.Id Desc";
            return _workspace.SqlQuery<Issue>(filterExpression).ToList();
        }
        public IssueDetail GetIssueDetail(int Id)
        {
            return _workspace.Single<IssueDetail>(x => x.Id == Id);
        }
        #endregion        
        #region Stock
        public IList<Stock> GetStocks()
        {
            return WorkspaceFactory.CreateReadOnly().Query<Stock>(null).OrderBy(x => x.Id).ToList();
        }
        public Stock GetStockByItem(int itemId)
        {
            return _workspace.Single<Stock>(x => x.Item.Id == itemId, y => y.Item);
        }
        public Stock GetStock(int id)
        {
            return _workspace.Single<Stock>(x => x.Id == id, y => y.Item);
        }
        public IList<Stock> ListStocks(string item)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM Stocks WHERE 1 = CASE WHEN '" + item + "' = '' THEN 1 WHEN Stocks.Item_Id = '" + item + "' THEN 1 END ORDER BY Stocks.Id Desc";
            return _workspace.SqlQuery<Stock>(filterExpression).ToList();
        }
        #endregion
        #region Fixed Asset
        public IList<FixedAsset> GetUpdatedFixedAssetsByItem(int itemId)
        {
            return WorkspaceFactory.CreateReadOnly().Query<FixedAsset>(x => x.AssetStatus == "UpdatedInStore" && x.Item.Id == itemId).OrderBy(x => x.Id).ToList();
        }
        public IList<FixedAsset> GetToBeIssuedFixedAssets()
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM FixedAssets WHERE AssetStatus = 'ToBeIssued'";
            return _workspace.SqlQuery<FixedAsset>(filterExpression).ToList();
        }
        public IList<FixedAsset> GetFixedAssets()
        {
            return WorkspaceFactory.CreateReadOnly().Query<FixedAsset>(null).OrderBy(x => x.Id).ToList();
        }
        public IList<FixedAsset> ListFixedAssets(string item, string assetStatus)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM FixedAssets WHERE 1 = CASE WHEN '" + item + "' = '' THEN 1 WHEN FixedAssets.Item_Id = '" + item + "' THEN 1 END AND  1 = CASE WHEN '" + assetStatus + "' = '' THEN 1 WHEN FixedAssets.AssetStatus = '" + assetStatus + "'  Then 1 END ORDER BY FixedAssets.Id Desc";
            return _workspace.SqlQuery<FixedAsset>(filterExpression).ToList();
        }
        public FixedAsset GetFixedAsset(int faId)
        {
            return _workspace.Single<FixedAsset>(x => x.Id == faId, y => y.FixedAssetHistories);
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
