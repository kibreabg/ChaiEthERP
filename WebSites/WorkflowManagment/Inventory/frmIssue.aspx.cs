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
                BindIssueDetails();
                PopStores();
                PopEmployees();
                PopItemCategories();
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
        public int GetIssuedTo
        {
            get { return Convert.ToInt32(ddlIssuedTo.SelectedValue); }
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
            ddlCategory.SelectedValue = "0";
            ddlSubCategory.SelectedValue = "0";
            ddlItem.SelectedValue = "0";
            ddlStore.SelectedValue = "0";
            ddlSection.SelectedValue = "0";
            ddlShelf.SelectedValue = "0";
            txtQuantity.Text = "";
            txtExpiryDate.Text = "";
            txtUnitCost.Text = "";
            txtRemark.Text = "";
        }
        private void BindIssueDetails()
        {
            grvIssueDetails.DataSource = _presenter.CurrentIssue.IssueDetails;
            grvIssueDetails.DataBind();
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
            ddlIssuedTo.DataSource = _presenter.GetUsers();
            ddlIssuedTo.DataBind();
        }
        private void PopStores()
        {
            ddlStore.DataSource = _presenter.GetStores();
            ddlStore.DataBind();
        }
        private void PopItemCategories()
        {
            ddlCategory.DataSource = _presenter.GetItemCategories();
            ddlCategory.DataBind();
        }
        private void PopItemSubCategories(int categoryId)
        {
            ddlSubCategory.DataSource = _presenter.GetItemSubCatsByCategoryId(categoryId);
            ddlSubCategory.DataBind();
        }
        private void PopItems(int subCategoryId)
        {
            ddlItem.DataSource = _presenter.GetItemsBySubCatId(subCategoryId);
            ddlItem.DataBind();
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
            int IssueDetailId = Convert.ToInt32(grvIssueDetails.SelectedDataKey.Value);
            Session["IssueDetailId"] = IssueDetailId;
            Session["detailIndex"] = grvIssueDetails.SelectedIndex;
            

            txtQuantity.Text = _presenter.CurrentIssue.GetIssueDetail(IssueDetailId).Quantity.ToString();
            txtExpiryDate.Text = _presenter.CurrentIssue.GetIssueDetail(IssueDetailId).ExpiryDate.Value.ToShortDateString();
            txtUnitCost.Text = _presenter.CurrentIssue.GetIssueDetail(IssueDetailId).UnitCost.ToString();
            txtRemark.Text = _presenter.CurrentIssue.GetIssueDetail(IssueDetailId).Remark.ToString();

            ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
        }
        protected void grvIssueDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
        }
        protected void grvIssueDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
        protected void btnSaveDetail_Click(object sender, EventArgs e)
        {
            try
            {
                IssueDetail IssueDetail;
                if (Session["IssueDetailId"] != null)
                {
                    int recDetId = (int)Session["IssueDetailId"];                    

                    if (recDetId > 0)
                        IssueDetail = _presenter.CurrentIssue.GetIssueDetail(recDetId);
                    else
                        IssueDetail = _presenter.CurrentIssue.IssueDetails[(int)Session["detailIndex"]];
                }
                else
                {
                    IssueDetail = new IssueDetail();
                }             
                
                IssueDetail.ItemCategory = _presenter.GetItemCategory(Convert.ToInt32(ddlCategory.SelectedValue));
                IssueDetail.ItemSubCategory = _presenter.GetItemSubCategory(Convert.ToInt32(ddlSubCategory.SelectedValue));
                IssueDetail.Item = _presenter.GetItem(Convert.ToInt32(ddlItem.SelectedValue));
                IssueDetail.Store = _presenter.GetStore(Convert.ToInt32(ddlStore.SelectedValue));
                IssueDetail.Section = _presenter.GetSection(Convert.ToInt32(ddlSection.SelectedValue));
                IssueDetail.Shelf = _presenter.GetShelf(Convert.ToInt32(ddlShelf.SelectedValue));
                IssueDetail.Quantity = Convert.ToDecimal(txtQuantity.Text);
                IssueDetail.UnitCost = Convert.ToDecimal(txtUnitCost.Text);
                IssueDetail.TotalQuantity = IssueDetail.Quantity * IssueDetail.UnitCost;
                IssueDetail.ExpiryDate = Convert.ToDateTime(txtExpiryDate.Text);
                IssueDetail.Remark = txtRemark.Text;

                if (Session["IssueDetailId"] == null)
                {
                    _presenter.CurrentIssue.IssueDetails.Add(IssueDetail);
                }
                else
                {
                    _presenter.CurrentIssue.IssueDetails[(int)Session["detailIndex"]] = IssueDetail;
                }
                    
                BindIssueDetails();
                Session["IssueDetailId"] = null;
                Session["detailIndex"] = null;

                Master.ShowMessage(new AppMessage("Item Issue Detail Successfully Added", RMessageType.Info));
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
                if (_presenter.CurrentIssue.IssueDetails.Count != 0)
                {
                    _presenter.SaveOrUpdateIssue();
                    BindIssues();
                    Master.ShowMessage(new AppMessage("Successfully Issued the Item from the Store, Reference No - <b>'" + _presenter.CurrentIssue.IssueNo + "'</b>", RMessageType.Info));
                    btnSave.Visible = false;
                }
                else
                {
                    Master.ShowMessage(new AppMessage("Please insert at least one Issued Item Detail", RMessageType.Error));
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
        protected void btnNewDetail_Click(object sender, EventArgs e)
        {
            ClearFormFields();
            ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmIssue.aspx");
        }
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopItemSubCategories(Convert.ToInt32(ddlCategory.SelectedValue));
            ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
        }
        protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopItems(Convert.ToInt32(ddlSubCategory.SelectedValue));
            ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
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