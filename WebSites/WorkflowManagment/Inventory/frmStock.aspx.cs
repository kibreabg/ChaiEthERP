using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Inventory;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using log4net;
using log4net.Config;
using Microsoft.Practices.ObjectBuilder;
using System.Web.Configuration;

namespace Chai.WorkflowManagment.Modules.Inventory.Views
{
    public partial class frmStock : POCBasePage, IStockListView
    {
        private StockPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                BindStocks();
                PopItems();
            }
            this._presenter.OnViewLoaded();

        }

        [CreateNew]
        public StockPresenter Presenter
        {
            get
            {
                return this._presenter;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this._presenter = value;
                this._presenter.View = this;
            }
        }
        public override string PageID
        {
            get
            {
                return "{0FE99D8E-1881-4E13-8C52-6AAC5080071D}";
            }
        }
        #region Field Getters
        public int GetStockId
        {
            get
            {
                if (grvStockList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvStockList.SelectedDataKey.Value);
                }
                else
                {
                    return 0;
                }
            }
        }
        public string GetItem
        {
            get { return ddlItem.SelectedValue; }
        }
        public string GetQuantity
        {
            get { return txtQuantity.Text; }
        }
        #endregion
        private void BindStocks()
        {
            grvStockList.DataSource = _presenter.ListStocks(ddlSrchItem.SelectedValue);
            grvStockList.DataBind();
        }
        private void PopItems()
        {
            ddlItem.DataSource = _presenter.GetItems();
            ddlItem.DataBind();
            ddlSrchItem.DataSource = _presenter.GetItems();
            ddlSrchItem.DataBind();
        }
        protected void grvStockList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int stockId = (int)grvStockList.SelectedIndex;
            Session["Stock"] = stockId;
            _presenter.CurrentStock = _presenter.GetStockById(stockId);
            ddlItem.SelectedValue = _presenter.CurrentStock.Item.Id.ToString();
            txtQuantity.Text = _presenter.CurrentStock.Quantity.ToString();
            btnSave.Visible = true;
        }
        protected void grvStockList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            btnFind_Click(sender, e);
            grvStockList.PageIndex = e.NewPageIndex;

        }
        protected void grvStockList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Stock Stock = e.Row.DataItem as Stock;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _presenter.SaveOrUpdateStock();
                BindStocks();
                Master.ShowMessage(new AppMessage("Successfully updated the stock information for the Item", RMessageType.Info));
                btnSave.Visible = false;
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindStocks();
        }
    }
}