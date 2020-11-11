using Chai.WorkflowManagment.CoreDomain.Inventory;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using Microsoft.Practices.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Chai.WorkflowManagment.Modules.Inventory.Views
{
    public partial class frmFixedAssetHistories : POCBasePage, IFixedAssetHistoriesView
    {
        private FixedAssetHistoriesPresenter _presenter;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                PopFixedAssets();
                BindFixedAssets();
            }
            this._presenter.OnViewLoaded();
        }
        [CreateNew]
        public FixedAssetHistoriesPresenter Presenter
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
                return "{E72E8156-4106-47DB-B97F-8215471754B7}";
            }
        }
        private void PopFixedAssets()
        {
            ddlFilterAssetCode.Items.Clear();
            ListItem lst = new ListItem();
            lst.Text = "Select Asset Code";
            lst.Value = "";
            ddlFilterAssetCode.Items.Add(lst);
            ddlFilterAssetCode.DataSource = _presenter.GetFixedAssets();
            ddlFilterAssetCode.DataBind();

            ddlFilterSerialNo.Items.Clear();
            ListItem lstSer = new ListItem();
            lstSer.Text = "Select Serial No";
            lstSer.Value = "";
            ddlFilterSerialNo.Items.Add(lstSer);
            ddlFilterSerialNo.DataSource = _presenter.GetFixedAssets();
            ddlFilterSerialNo.DataBind();
        }
        protected void BindFixedAssets()
        {
            dgFixedAssetHistory.DataSource = _presenter.ListFixedAssetHistories(ddlFilterAssetCode.SelectedValue, ddlFilterSerialNo.SelectedValue);
            dgFixedAssetHistory.DataBind();
        }
        protected void dgFixedAssetHistory_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            dgFixedAssetHistory.CurrentPageIndex = e.NewPageIndex;
            BindFixedAssets();
        }
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            BindFixedAssets();
        }
    }
}
