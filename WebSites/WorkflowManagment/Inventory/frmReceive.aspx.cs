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
    public partial class frmReceive : POCBasePage, IReceiveListView
    {
        private ReceivePresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                BindReceives();
                BindReceiveDetails();
                PopSuppliers();
                PopStores();
                PopItemCategories();
                BindPrograms();
            }
            txtReceiveDate.Text = DateTime.Today.Date.ToShortDateString();
            this._presenter.OnViewLoaded();

        }

        [CreateNew]
        public ReceivePresenter Presenter
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
                return "{292A2860-3F98-49B8-92D2-815CA45418BA}";
            }
        }
        #region Field Getters
        public int GetReceiveId
        {
            get
            {
                if (grvReceiveList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvReceiveList.SelectedDataKey.Value);
                }
                else
                {
                    return 0;
                }
            }
        }
        public DateTime GetRecieveDate
        {
            get { return Convert.ToDateTime(txtReceiveDate.Text); }
        }
        public string GetReceiveNo
        {
            get { return AutoNumber(); }
        }
        public string GetInvoiceNo
        {
            get { return txtInvoiceNo.Text; }
        }
        public string GetDeliveredBy
        {
            get { return txtDeliveredBy.Text; }
        }
        public int GetProgram
        {
            get { return Convert.ToInt32(ddlProgram.SelectedValue); }
        }
        public int GetProject
        {
            get { return Convert.ToInt32(ddlProject.SelectedValue); }
        }
        public int GetGrant
        {
            get { return Convert.ToInt32(ddlGrant.SelectedValue); }
        }
        public int GetSupplier
        {
            get { return Convert.ToInt32(ddlSupplier.SelectedValue); }
        }
        #endregion
        private string AutoNumber()
        {
            return "IR-" + _presenter.CurrentUser().Id.ToString() + "-" + (_presenter.GetLastReceiveId() + 1).ToString();
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
        private void BindReceiveDetails()
        {
            grvReceiveDetails.DataSource = _presenter.CurrentReceive.ReceiveDetails;
            grvReceiveDetails.DataBind();
        }
        private void BindReceives()
        {
            grvReceiveList.DataSource = _presenter.ListReceives(txtSrchRequestNo.Text, txtSrchRequestDate.Text);
            grvReceiveList.DataBind();
        }
        private void PopSuppliers()
        {
            ddlSupplier.Items.Clear();
            ListItem lst = new ListItem();
            lst.Text = " Select Supplier ";
            lst.Value = "0";
            //ListItem empPayee = new ListItem();
            //empPayee.Text = _presenter.CurrentUser().FullName;
            //empPayee.Value = "-1";
            ddlSupplier.Items.Add(lst);
            //ddlSupplier.Items.Add(empPayee);
            ddlSupplier.DataSource = _presenter.GetSuppliers();
            ddlSupplier.DataBind();
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
        private void BindReceiveFields()
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentReceive != null)
            {
                if (_presenter.CurrentReceive.Supplier != null)
                    ddlSupplier.SelectedValue = _presenter.CurrentReceive.Supplier.Id.ToString();
                txtInvoiceNo.Text = _presenter.CurrentReceive.InvoiceNo;
                txtDeliveredBy.Text = _presenter.CurrentReceive.DeliveredBy;
                BindReceiveDetails();
                BindReceives();
            }
        }
        private void BindPrograms()
        {
            ddlProgram.DataSource = _presenter.GetPrograms();
            ddlProgram.DataBind();
        }
        private void BindProject(int programID)
        {
            if (programID != 0)
            {
                ddlProject.DataSource = _presenter.ListProjects(programID);
                ddlProject.DataValueField = "Id";
                ddlProject.DataTextField = "ProjectCode";
                ddlProject.DataBind();
            }

        }
        private void BindGrant(int projectId)
        {
            if (projectId != 0)
            {
                ddlGrant.DataSource = _presenter.GetGrantbyprojectId(projectId);
                ddlGrant.DataValueField = "Id";
                ddlGrant.DataTextField = "GrantCode";
                ddlGrant.DataBind();
            }
        }
        protected void grvReceiveList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["Receive"] = true;
            BindReceiveFields();
            btnSave.Visible = true;
            btnDelete.Visible = true;
        }
        protected void grvReceiveList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            btnFind_Click(sender, e);
            grvReceiveList.PageIndex = e.NewPageIndex;

        }
        protected void grvReceiveList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Receive receive = e.Row.DataItem as Receive;
            }
        }
        protected void grvReceiveDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            int receiveDetailId = Convert.ToInt32(grvReceiveDetails.SelectedDataKey.Value);
            Session["receiveDetailId"] = receiveDetailId;
            Session["detailIndex"] = grvReceiveDetails.SelectedIndex;
            

            txtQuantity.Text = _presenter.CurrentReceive.GetReceiveDetail(receiveDetailId).Quantity.ToString();
            txtExpiryDate.Text = _presenter.CurrentReceive.GetReceiveDetail(receiveDetailId).ExpiryDate.Value.ToShortDateString();
            txtUnitCost.Text = _presenter.CurrentReceive.GetReceiveDetail(receiveDetailId).UnitCost.ToString();
            txtRemark.Text = _presenter.CurrentReceive.GetReceiveDetail(receiveDetailId).Remark.ToString();

            ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
        }
        protected void grvReceiveDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
        }
        protected void grvReceiveDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
        protected void btnSaveDetail_Click(object sender, EventArgs e)
        {
            try
            {
                ReceiveDetail receiveDetail;
                if (Session["receiveDetailId"] != null)
                {
                    int recDetId = (int)Session["receiveDetailId"];                    

                    if (recDetId > 0)
                        receiveDetail = _presenter.CurrentReceive.GetReceiveDetail(recDetId);
                    else
                        receiveDetail = _presenter.CurrentReceive.ReceiveDetails[(int)Session["detailIndex"]];
                }
                else
                {
                    receiveDetail = new ReceiveDetail();
                }             
                
                receiveDetail.ItemCategory = _presenter.GetItemCategory(Convert.ToInt32(ddlCategory.SelectedValue));
                receiveDetail.ItemSubCategory = _presenter.GetItemSubCategory(Convert.ToInt32(ddlSubCategory.SelectedValue));
                receiveDetail.Item = _presenter.GetItem(Convert.ToInt32(ddlItem.SelectedValue));
                receiveDetail.Store = _presenter.GetStore(Convert.ToInt32(ddlStore.SelectedValue));
                receiveDetail.Section = _presenter.GetSection(Convert.ToInt32(ddlSection.SelectedValue));
                receiveDetail.Shelf = _presenter.GetShelf(Convert.ToInt32(ddlShelf.SelectedValue));
                receiveDetail.Quantity = Convert.ToDecimal(txtQuantity.Text);
                receiveDetail.UnitCost = Convert.ToDecimal(txtUnitCost.Text);
                receiveDetail.TotalQuantity = receiveDetail.Quantity * receiveDetail.UnitCost;
                receiveDetail.ExpiryDate = Convert.ToDateTime(txtExpiryDate.Text);
                receiveDetail.Remark = txtRemark.Text;

                if (Session["receiveDetailId"] == null)
                {
                    _presenter.CurrentReceive.ReceiveDetails.Add(receiveDetail);
                }
                else
                {
                    _presenter.CurrentReceive.ReceiveDetails[(int)Session["detailIndex"]] = receiveDetail;
                }
                    
                BindReceiveDetails();
                Session["receiveDetailId"] = null;
                Session["detailIndex"] = null;

                Master.ShowMessage(new AppMessage("Item Receive Detail Successfully Added", RMessageType.Info));
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
                if (_presenter.CurrentReceive.ReceiveDetails.Count != 0)
                {
                    _presenter.SaveOrUpdateReceive();
                    BindReceives();
                    Master.ShowMessage(new AppMessage("Successfully Received the Item to the Store, Reference No - <b>'" + _presenter.CurrentReceive.ReceiveNo + "'</b>", RMessageType.Info));
                    btnSave.Visible = false;
                }
                else
                {
                    Master.ShowMessage(new AppMessage("Please insert at least one Received Item Detail", RMessageType.Error));
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
            _presenter.DeleteReceive(_presenter.CurrentReceive);
            ClearFormFields();
            BindReceives();
            BindReceiveDetails();
            btnDelete.Visible = false;
            Master.ShowMessage(new AppMessage("Item Receive entry successfully deleted", RMessageType.Info));
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindReceives();
            ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
        }
        protected void btnNewDetail_Click(object sender, EventArgs e)
        {
            ClearFormFields();
            ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmReceive.aspx");
        }
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrant(Convert.ToInt32(ddlProject.SelectedValue));
        }
        protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindProject(Convert.ToInt32(ddlProgram.SelectedValue));
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