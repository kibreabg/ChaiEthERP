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
    public partial class frmFixedAsset : POCBasePage, IFixedAssetView
    {
        private FixedAssetPresenter _presenter;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                BindFixedAssets();
                PopCustodians();
                PopItems();
                PopAssetStatuses();
            }
            this._presenter.OnViewLoaded();
        }
        [CreateNew]
        public FixedAssetPresenter Presenter
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
                return "{0398CC80-8F0A-449C-89F3-C36F6AEF4308}";
            }
        }
        private void PopItems()
        {
            ddlFilterItem.Items.Clear();
            ListItem lst = new ListItem();
            lst.Text = "Select Item";
            lst.Value = "";
            ddlFilterItem.Items.Add(lst);
            ddlFilterItem.DataSource = _presenter.GetItems();
            ddlFilterItem.DataBind();
        }
        private void PopAssetStatuses()
        {
            ddlFilterStatus.Items.Clear();
            ListItem lst = new ListItem();
            lst.Text = "Select Asset Status";
            lst.Value = "";
            ddlFilterStatus.Items.Add(lst);

            var statuses = Enum.GetValues(typeof(FixedAssetStatus));

            foreach (var status in statuses)
            {
                ListItem list = new ListItem();
                list.Text = status.ToString();
                list.Value = status.ToString();
                ddlFilterStatus.Items.Add(list);
            }
            ddlFilterStatus.DataBind();
            
        }
        private void PopCustodians()
        {
            ddlNewCustodian.DataSource = _presenter.GetUsers();
            ddlNewCustodian.DataBind();
        }
        private void PopStores()
        {
            ddlStore.DataSource = _presenter.GetStores();
            ddlStore.DataBind();
        }
        private void PopSections(int storeId)
        {
            ddlSection.DataSource = _presenter.GetSectionsByStoreId(storeId);
            ddlSection.DataBind();
        }
        private void PopShelves(int sectionId)
        {
            ddlShelf.DataSource = _presenter.GetShelvesBySectionId(sectionId);
            ddlShelf.DataBind();
        }
        protected void BindFixedAssets()
        {
            dgFixedAsset.DataSource = _presenter.ListFixedAssets(ddlFilterItem.SelectedValue, ddlFilterStatus.SelectedValue);
            dgFixedAsset.DataBind();
        }
        protected void dgFixedAsset_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            dgFixedAsset.CurrentPageIndex = e.NewPageIndex;
            BindFixedAssets();
        }
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopSections(Convert.ToInt32(ddlStore.SelectedValue));
            ScriptManager.RegisterStartupScript(this, GetType(), "showReturnFAModal", "showReturnFAModal();", true);
        }
        protected void ddlSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopShelves(Convert.ToInt32(ddlSection.SelectedValue));
            ScriptManager.RegisterStartupScript(this, GetType(), "showReturnFAModal", "showReturnFAModal();", true);
        }
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            BindFixedAssets();
        }
        protected void ddlOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlOperation = (DropDownList)sender;
            DataGridItem datagridRow = (DataGridItem)ddlOperation.NamingContainer;
            int fixedAssetId = (int)dgFixedAsset.DataKeys[datagridRow.ItemIndex];
            Session["fixedAssetId"] = fixedAssetId;

            FixedAsset fa = _presenter.GetFixedAsset(fixedAssetId);

            if (ddlOperation.SelectedValue == "Update")
            {
                txtAssetCode.Text = fa.AssetCode;
                txtSerialNo.Text = fa.SerialNo;
                txtLocation.Text = fa.Location;
                txtCondition.Text = fa.Condition;
                txtTotalLife.Text = fa.TotalLife.ToString();
                txtRemark.Text = fa.Remark;

                ScriptManager.RegisterStartupScript(this, GetType(), "showUpdateFAModal", "showUpdateFAModal();", true);
            }
            else if (ddlOperation.SelectedValue == "Transfer")
            {
                lblCurrentCustodian.Text = fa.Custodian;
                ScriptManager.RegisterStartupScript(this, GetType(), "showTransferFAModal", "showTransferFAModal();", true);
            }
            else if (ddlOperation.SelectedValue == "Return")
            {
                PopStores();
                lblRtrnCrntCust.Text = fa.Custodian;
                ScriptManager.RegisterStartupScript(this, GetType(), "showReturnFAModal", "showReturnFAModal();", true);
            }
            ddlOperation.SelectedValue = "";
        }
        protected void btnUpdateFA_Click(object sender, EventArgs e)
        {
            int fixedAssetId = (int)Session["fixedAssetId"];

            FixedAsset fa = _presenter.GetFixedAsset(fixedAssetId);

            fa.AssetCode = txtAssetCode.Text;
            fa.SerialNo = txtSerialNo.Text;
            fa.Location = txtLocation.Text;
            fa.Condition = txtCondition.Text;
            fa.TotalLife = Convert.ToInt32(txtTotalLife.Text);
            fa.Remark = txtRemark.Text;
            fa.AssetStatus = FixedAssetStatus.UpdatedInStore.ToString();

            FixedAssetHistory fah = new FixedAssetHistory();
            fah.TransactionDate = DateTime.Now;
            fah.Custodian = "Store";
            fah.Operation = FixedAssetStatus.UpdatedInStore.ToString();

            fa.FixedAssetHistories.Add(fah);

            _presenter.SaveOrUpdateFixedAsset(fa);
            BindFixedAssets();
            Session["fixedAssetId"] = null;

            Master.ShowMessage(new AppMessage("Fixed Asset successfully updated!", RMessageType.Info));

        }

        protected void btnTransferFA_Click(object sender, EventArgs e)
        {
            int fixedAssetId = (int)Session["fixedAssetId"];

            FixedAsset fa = _presenter.GetFixedAsset(fixedAssetId);

            if (fa.Custodian == "Store")
            {
                Master.ShowMessage(new AppMessage("Transfer can only be done from a person to another person!", RMessageType.Error));
            }
            else
            {
                fa.Custodian = ddlNewCustodian.SelectedValue;

                FixedAssetHistory fah = new FixedAssetHistory();
                fah.TransactionDate = DateTime.Now;
                fah.Custodian = ddlNewCustodian.SelectedValue;
                fah.Operation = "Transfered";

                fa.FixedAssetHistories.Add(fah);

                _presenter.SaveOrUpdateFixedAsset(fa);
                BindFixedAssets();
                Session["fixedAssetId"] = null;

                Master.ShowMessage(new AppMessage("Fixed Asset successfully transfered!", RMessageType.Info));
            }
        }
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            int fixedAssetId = (int)Session["fixedAssetId"];

            FixedAsset fa = _presenter.GetFixedAsset(fixedAssetId);

            if (fa.Custodian == "Store")
            {
                Master.ShowMessage(new AppMessage("The current item is still in the Store!", RMessageType.Error));
            }
            else
            {
                fa.Custodian = "Store";

                FixedAssetHistory fah = new FixedAssetHistory();
                fah.TransactionDate = DateTime.Now;
                fah.Custodian = "Store";
                fah.Operation = "Returned";

                fa.FixedAssetHistories.Add(fah);

                //Add the received quantity to the stock
                Stock stock = _presenter.GetStockByItem(fa.Item.Id);
                stock.Quantity = stock.Quantity + 1;
                _presenter.SaveOrUpdateStock(stock);

                _presenter.SaveOrUpdateFixedAsset(fa);
                BindFixedAssets();
                Session["fixedAssetId"] = null;

                Master.ShowMessage(new AppMessage("Fixed Asset successfully returned to the Store!", RMessageType.Info));
            }
        }
    }
}
