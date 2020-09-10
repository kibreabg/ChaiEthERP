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
using System.Data;

namespace Chai.WorkflowManagment.Modules.Inventory.Views
{
    public partial class frmIssue : POCBasePage, IIssueListView
    {
        private IssuePresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                BindIssues();
                GetItemsFromStoreReq();
                PopStores();
                PopEmployees();
                FixedAssetStatusReset();
            }
            txtIssueDate.Text = DateTime.Today.Date.ToShortDateString();
            this._presenter.OnViewLoaded();

        }

        [CreateNew]
        public IssuePresenter Presenter
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
                return "{C676B157-97FB-4581-B39C-23D60160581B}";
            }
        }
        #region Field Getters
        public int GetIssueId
        {
            get
            {
                if (grvIssueList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvIssueList.SelectedDataKey.Value);
                }
                else
                {
                    return 0;
                }
            }
        }
        public DateTime GetIssueDate
        {
            get { return Convert.ToDateTime(txtIssueDate.Text); }
        }
        public string GetIssueNo
        {
            get { return AutoNumber(); }
        }
        public string GetPurpose
        {
            get { return ddlPurpose.SelectedValue; }
        }
        public int GetHandedOverBy
        {
            get { return Convert.ToInt32(ddlHandedOverBy.SelectedValue); }
        }
        #endregion
        private string AutoNumber()
        {
            return "II-" + _presenter.CurrentUser().Id.ToString() + "-" + (_presenter.GetLastIssueId() + 1).ToString();
        }
        private void ClearFormFields()
        {
            ddlStore.SelectedValue = "0";
            ddlSection.SelectedValue = "0";
            ddlShelf.SelectedValue = "0";
            txtQuantity.Text = "";
            txtUnitCost.Text = "";
            txtRemark.Text = "";
        }
        private void BindIssueDetails()
        {
            grvIssueDetails.DataSource = _presenter.CurrentIssue.IssueDetails;
            grvIssueDetails.DataBind();
        }
        private void FixedAssetStatusReset()
        {
            foreach (FixedAsset fa in _presenter.GetToBeIssuedFixedAssets())
            {
                fa.AssetStatus = FixedAssetStatus.UpdatedInStore.ToString();
                _presenter.SaveOrUpdateFixedAsset(fa);
            }
        }
        private void GetItemsFromStoreReq()
        {
            int storeReqId = Convert.ToInt32(Request.QueryString["StoreReqId"]);
            if (storeReqId != 0)
            {
                StoreRequest storeReq = _presenter.GetStoreRequest(storeReqId);
                foreach (StoreRequestDetail srDetail in storeReq.StoreRequestDetails)
                {
                    if (srDetail.Item.ItemType != "Fixed Asset" && (srDetail.IssuedQuantity < srDetail.QtyApproved))
                    {
                        IssueDetail issueDet = new IssueDetail();
                        issueDet.ItemCategory = srDetail.Item.ItemSubCategory.ItemCategory;
                        issueDet.ItemSubCategory = srDetail.Item.ItemSubCategory;
                        issueDet.Item = srDetail.Item;
                        issueDet.Quantity = srDetail.QtyApproved;
                        _presenter.CurrentIssue.IssueDetails.Add(issueDet);
                    }
                    else
                    {
                        for (int i = 1; i <= (srDetail.QtyApproved - srDetail.IssuedQuantity); i++)
                        {
                            IssueDetail issueDet = new IssueDetail();
                            issueDet.ItemCategory = srDetail.Item.ItemSubCategory.ItemCategory;
                            issueDet.ItemSubCategory = srDetail.Item.ItemSubCategory;
                            issueDet.Item = srDetail.Item;
                            issueDet.Quantity = 1;
                            _presenter.CurrentIssue.IssueDetails.Add(issueDet);
                        }
                    }
                }
            }
            BindIssueDetails();
        }
        private void BindIssues()
        {
            grvIssueList.DataSource = _presenter.ListIssues(txtSrchRequestNo.Text, txtSrchRequestDate.Text);
            grvIssueList.DataBind();
        }
        private void PopEmployees()
        {
            ddlHandedOverBy.DataSource = _presenter.GetUsers();
            ddlHandedOverBy.DataBind();
            ddlHandedOverBy.SelectedValue = _presenter.CurrentUser().Id.ToString();
            ddlCustodian.DataSource = _presenter.GetUsers();
            ddlCustodian.DataBind();
        }
        private void PopFixedAsset()
        {
            ddlAssetCode.Items.Clear();
            ddlAssetCode.DataSource = _presenter.GetUpdatedFixedAssets();
            ddlAssetCode.DataBind();
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
        private void BindIssueFields()
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentIssue != null)
            {
                ddlHandedOverBy.SelectedValue = _presenter.CurrentIssue.HandedOverBy.ToString();
                BindIssueDetails();
                BindIssues();
            }
        }
        protected void grvIssueList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["Issue"] = true;
            BindIssueFields();
            btnSave.Visible = true;
            btnDelete.Visible = true;
        }
        protected void grvIssueList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            btnFind_Click(sender, e);
            grvIssueList.PageIndex = e.NewPageIndex;

        }
        protected void grvIssueList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Issue Issue = e.Row.DataItem as Issue;
            }
        }
        protected void grvIssueDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            int issueDetailId = grvIssueDetails.SelectedRow.RowIndex;
            Session["IssueDetailId"] = issueDetailId;

            if (_presenter.CurrentIssue.IssueDetails[issueDetailId].Item.ItemType == "Fixed Asset")
            {
                PopFixedAsset();
                ScriptManager.RegisterStartupScript(this, GetType(), "showFixedAssetModal", "showFixedAssetModal();", true);
            }
            else
            {
                txtQuantity.Text = _presenter.CurrentIssue.IssueDetails[issueDetailId].Quantity.ToString();
                txtUnitCost.Text = _presenter.CurrentIssue.IssueDetails[issueDetailId].UnitCost.ToString();
                txtRemark.Text = _presenter.CurrentIssue.IssueDetails[issueDetailId].Remark;

                ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
            }

        }
        protected void grvIssueDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
        }
        protected void grvIssueDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                IssueDetail issueDetail = e.Row.DataItem as IssueDetail;
                LinkButton lb = e.Row.Cells[5].Controls[0] as LinkButton;

                //If the row contains a fixed item, change the operation link
                if (issueDetail.Item.ItemType == "Fixed Asset")
                {
                    e.Row.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                    lb.Text = "Update Fixed Asset";
                }
            }
        }
        protected void btnSaveDetail_Click(object sender, EventArgs e)
        {
            try
            {
                int recDetId = (int)Session["IssueDetailId"];
                IssueDetail issueDetail = _presenter.CurrentIssue.IssueDetails[recDetId];

                issueDetail.Store = _presenter.GetStore(Convert.ToInt32(ddlStore.SelectedValue));
                issueDetail.Section = _presenter.GetSection(Convert.ToInt32(ddlSection.SelectedValue));
                issueDetail.Shelf = _presenter.GetShelf(Convert.ToInt32(ddlShelf.SelectedValue));
                issueDetail.Quantity = Convert.ToInt32(txtQuantity.Text);
                issueDetail.UnitCost = Convert.ToDecimal(txtUnitCost.Text);
                issueDetail.TotalQuantity = issueDetail.Quantity * issueDetail.UnitCost;
                issueDetail.Remark = txtRemark.Text;

                _presenter.CurrentIssue.IssueDetails[recDetId] = issueDetail;

                BindIssueDetails();
                Session["IssueDetailId"] = null;

                Master.ShowMessage(new AppMessage("Item Issue Detail Successfully Updated", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void btnFAIssue_Click(object sender, EventArgs e)
        {
            try
            {
                int recDetId = (int)Session["IssueDetailId"];

                FixedAsset fa = _presenter.GetFixedAsset(Convert.ToInt32(ddlAssetCode.SelectedValue));
                fa.AssetStatus = FixedAssetStatus.ToBeIssued.ToString();
                _presenter.SaveOrUpdateFixedAsset(fa);

                fa.Custodian = _presenter.GetUser(Convert.ToInt32(ddlCustodian.SelectedValue)).FullName;

                FixedAssetHistory fah = new FixedAssetHistory();
                fah.TransactionDate = DateTime.Now;
                fah.Custodian = _presenter.GetUser(Convert.ToInt32(ddlCustodian.SelectedValue)).FullName;
                fah.Operation = "Issue";

                fa.FixedAssetHistories.Add(fah);

                IssueDetail issueDetail = _presenter.CurrentIssue.IssueDetails[recDetId];

                issueDetail.Store = fa.Store;
                issueDetail.Section = fa.Section;
                issueDetail.Shelf = fa.Shelf;
                issueDetail.UnitCost = fa.UnitCost;
                issueDetail.Custodian = _presenter.GetUser(Convert.ToInt32(ddlCustodian.SelectedValue)).FullName;
                issueDetail.FixedAsset = fa;

                _presenter.CurrentIssue.IssueDetails[recDetId] = issueDetail;

                BindIssueDetails();
                Session["IssueDetailId"] = null;

                Master.ShowMessage(new AppMessage("Issue Detail for Fixed Asset Updated!", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool allDetailsUpdated = true;
                foreach (IssueDetail isDet in _presenter.CurrentIssue.IssueDetails)
                {
                    if (string.IsNullOrEmpty(isDet.Custodian))
                    {
                        allDetailsUpdated = false;
                    }
                }
                if (allDetailsUpdated)
                {
                    _presenter.SaveOrUpdateIssue();
                    BindIssues();
                    Master.ShowMessage(new AppMessage("Successfully Issued the Item from the Store, Reference No - <b>'" + _presenter.CurrentIssue.IssueNo + "'</b>", RMessageType.Info));
                    btnSave.Visible = false;
                }
                else
                {
                    Master.ShowMessage(new AppMessage("Please update all items in the Issue Detail!", RMessageType.Error));
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            _presenter.DeleteIssue(_presenter.CurrentIssue);
            ClearFormFields();
            BindIssues();
            BindIssueDetails();
            btnDelete.Visible = false;
            Master.ShowMessage(new AppMessage("Item Issue entry successfully deleted", RMessageType.Info));
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindIssues();
            ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmIssue.aspx");
        }
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopSections(Convert.ToInt32(ddlStore.SelectedValue));
            ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
        }
        protected void ddlSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopShelves(Convert.ToInt32(ddlSection.SelectedValue));
            ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
        }
    }
}