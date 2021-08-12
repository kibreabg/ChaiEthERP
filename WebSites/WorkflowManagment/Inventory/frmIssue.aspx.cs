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
                        issueDet.StoreRequestDetail = srDetail;
                        _presenter.CurrentIssue.IssueDetails.Add(issueDet);
                    }
                    else
                    {
                        for (int i = 1; i <= (srDetail.QtyApproved - srDetail.IssuedQuantity); i++)
                        {
                            IssueDetail issueDet = new IssueDetail();
                            issueDet.ItemCategory = srDetail.Item.ItemSubCategory.ItemCategory;
                            issueDet.ItemSubCategory = srDetail.Item.ItemSubCategory;
                            issueDet.StoreRequestDetail = srDetail;
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
            ddlCurAssetCustodian.DataSource = _presenter.GetUsers();
            ddlCurAssetCustodian.DataBind();
        }
        private void PopFixedAsset(int itemId, int progId)
        {
            ddlAssetCode.Items.Clear();
            ddlAssetCode.DataSource = _presenter.GetUpdatedFixedAssetsByItem(itemId, progId);
            ddlAssetCode.DataBind();
        }
        private void PopStores()
        {
            ddlStore.Items.Clear();
            ListItem lst = new ListItem();
            lst.Text = "Select Store";
            lst.Value = "0";
            ddlStore.Items.Add(lst);
            ddlStore.DataSource = _presenter.GetStores();
            ddlStore.DataBind();
        }
        private void PopSections(int storeId)
        {
            ddlSection.Items.Clear();
            ListItem lst = new ListItem();
            lst.Text = "Select Section";
            lst.Value = "0";
            ddlSection.Items.Add(lst);
            ddlSection.DataSource = _presenter.GetSectionsByStoreId(storeId);
            ddlSection.DataBind();
        }
        private void PopShelves(int sectionId)
        {
            ddlShelf.Items.Clear();
            ListItem lst = new ListItem();
            lst.Text = "Select Shelf";
            lst.Value = "0";
            ddlShelf.Items.Add(lst);
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
            int issueDetailId = Convert.ToInt32(grvIssueDetails.SelectedDataKey.Value);
            Session["IssueDetailId"] = issueDetailId;
            Session["detailIndex"] = grvIssueDetails.SelectedIndex;
            IssueDetail theIssueDetail;

            if (issueDetailId > 0)
            {
                theIssueDetail = _presenter.CurrentIssue.GetIssueDetail(issueDetailId);
            }
            else
                theIssueDetail = _presenter.CurrentIssue.IssueDetails[grvIssueDetails.SelectedIndex];

            if (theIssueDetail.Item.ItemType == "Fixed Asset")
            {
                int storeReqId = Convert.ToInt32(Request.QueryString["StoreReqId"]);
                if (storeReqId != 0)
                {
                    StoreRequest storeReq = _presenter.GetStoreRequest(storeReqId);
                    PopFixedAsset(theIssueDetail.Item.Id, storeReq.Program.Id);
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "showFixedAssetModal", "showFixedAssetModal();", true);
            }
            else
            {
                if (theIssueDetail.Store != null && theIssueDetail.Section != null && theIssueDetail.Shelf != null)
                {
                    int storeId = theIssueDetail.Store.Id;
                    int sectionId = theIssueDetail.Section.Id;
                    int shelfId = theIssueDetail.Shelf.Id;

                    ddlStore.SelectedValue = storeId.ToString();
                    PopSections(storeId);
                    ddlSection.SelectedValue = sectionId.ToString();
                    PopShelves(sectionId);
                    ddlShelf.SelectedValue = shelfId.ToString();
                }


                txtQuantity.Text = theIssueDetail.Quantity.ToString();
                txtUnitCost.Text = theIssueDetail.UnitCost.ToString();
                txtRemark.Text = theIssueDetail.Remark;
                ddlCurAssetCustodian.SelectedValue = theIssueDetail.Custodian;

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
                LinkButton lb = e.Row.Cells[6].Controls[0] as LinkButton;

                //If the row contains a fixed item, change the operation link
                if (issueDetail.Item.ItemType == "Fixed Asset")
                {
                    e.Row.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                    lb.Text = "Update Fixed Asset";
                }
                else
                    lb.Text = "Update Current Asset";
            }
        }
        protected void btnSaveDetail_Click(object sender, EventArgs e)
        {
            try
            {
                IssueDetail issueDetail;
                int issDetId = (int)Session["IssueDetailId"];

                if (issDetId > 0)
                {
                    issueDetail = _presenter.CurrentIssue.GetIssueDetail(issDetId);
                    issueDetail.PreviousQuantity = issueDetail.Quantity;
                }
                else
                    issueDetail = _presenter.CurrentIssue.IssueDetails[(int)Session["detailIndex"]];

                issueDetail.Store = _presenter.GetStore(Convert.ToInt32(ddlStore.SelectedValue));
                issueDetail.Section = _presenter.GetSection(Convert.ToInt32(ddlSection.SelectedValue));
                issueDetail.Shelf = _presenter.GetShelf(Convert.ToInt32(ddlShelf.SelectedValue));
                issueDetail.Custodian = ddlCurAssetCustodian.SelectedValue;
                issueDetail.Quantity = Convert.ToInt32(txtQuantity.Text);
                issueDetail.StoreRequestDetail.IssuedQuantity += issueDetail.Quantity;
                issueDetail.UnitCost = Convert.ToDecimal(txtUnitCost.Text);
                issueDetail.TotalQuantity = issueDetail.Quantity * issueDetail.UnitCost;
                issueDetail.Remark = txtRemark.Text;

                if (Session["IssueDetailId"] == null)
                {
                    _presenter.CurrentIssue.IssueDetails.Add(issueDetail);
                }
                else
                {
                    _presenter.CurrentIssue.IssueDetails[(int)Session["detailIndex"]] = issueDetail;
                }

                BindIssueDetails();
                Session["IssueDetailId"] = null;
                Session["detailIndex"] = null;
                btnSave.Visible = true;

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
                IssueDetail issueDetail;
                int issDetId = (int)Session["IssueDetailId"];

                if (issDetId > 0)
                {
                    issueDetail = _presenter.CurrentIssue.GetIssueDetail(issDetId);
                    issueDetail.PreviousQuantity = issueDetail.Quantity;
                }
                else
                    issueDetail = _presenter.CurrentIssue.IssueDetails[(int)Session["detailIndex"]];

                string issuedTo = "";
                if (rbChaiEmp.Checked)
                    issuedTo = _presenter.GetUser(Convert.ToInt32(ddlCustodian.SelectedValue)).FullName;
                else if (rbOther.Checked)
                    issuedTo = txtOtherCustodian.Text;

                FixedAsset fa = _presenter.GetFixedAsset(Convert.ToInt32(ddlAssetCode.SelectedValue));
                fa.Store = null;
                fa.Section = null;
                fa.Shelf = null;
                fa.AssetStatus = FixedAssetStatus.ToBeIssued.ToString();
                _presenter.SaveOrUpdateFixedAsset(fa);

                fa.Custodian = issuedTo;

                FixedAssetHistory fah = new FixedAssetHistory();
                fah.TransactionDate = DateTime.Now;
                fah.Custodian = issuedTo;
                fah.Operation = "Issue";

                fa.FixedAssetHistories.Add(fah);

                issueDetail.Store = fa.Store;
                issueDetail.Section = fa.Section;
                issueDetail.Shelf = fa.Shelf;
                issueDetail.UnitCost = fa.UnitCost;
                issueDetail.Quantity = 1;
                issueDetail.Custodian = issuedTo;
                issueDetail.FixedAsset = fa;

                if (Session["IssueDetailId"] == null)
                {
                    _presenter.CurrentIssue.IssueDetails.Add(issueDetail);
                }
                else
                {
                    _presenter.CurrentIssue.IssueDetails[(int)Session["detailIndex"]] = issueDetail;
                }

                BindIssueDetails();
                btnSave.Visible = true;
                Session["IssueDetailId"] = null;
                Session["detailIndex"] = null;

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
                bool availableToIssue = true;
                foreach (IssueDetail isDet in _presenter.CurrentIssue.IssueDetails)
                {
                    if (string.IsNullOrEmpty(isDet.Custodian))
                    {
                        allDetailsUpdated = false;
                    }
                    if (isDet.Item.ItemType == "Fixed Asset")
                    {
                        if (isDet.FixedAsset.AssetStatus != FixedAssetStatus.ToBeIssued.ToString())
                        {
                            availableToIssue = false;
                        }
                    }
                }
                if (allDetailsUpdated && availableToIssue)
                {
                    _presenter.SaveOrUpdateIssue();
                    BindIssues();
                    Master.ShowMessage(new AppMessage("Successfully Issued the Item from the Store, Reference No - <b>'" + _presenter.CurrentIssue.IssueNo + "'</b>", RMessageType.Info));
                    btnSave.Visible = false;
                }
                else
                {
                    if (allDetailsUpdated == false)
                    {
                        Master.ShowMessage(new AppMessage("Please update all items in the Issue Detail!", RMessageType.Error));
                    }
                    else
                    {
                        Master.ShowMessage(new AppMessage("One or more of the items are already issued!", RMessageType.Error));
                    }
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
        protected void rbChaiEmp_CheckedChanged(object sender, EventArgs e)
        {
            pnlChaiEmp.Visible = true;
            pnlOther.Visible = false;
            ScriptManager.RegisterStartupScript(this, GetType(), "showFixedAssetModal", "showFixedAssetModal();", true);
        }
        protected void rbOther_CheckedChanged(object sender, EventArgs e)
        {
            pnlChaiEmp.Visible = false;
            pnlOther.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "showFixedAssetModal", "showFixedAssetModal();", true);
        }
    }
}