using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using log4net;
using log4net.Config;
using Microsoft.Practices.ObjectBuilder;
using System.IO;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public partial class frmSoleVendorRequest : POCBasePage, ISoleVendorRequestView
    {
        private SoleVendorRequestPresenter _presenter;
        private IList<SoleVendorRequest> _SoleVendorRequests;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        int prId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                CheckApprovalSettings();
                PopPurchaseRequestsDropDown();
                pnlInfo.Visible = false;
                PopPurchaseRequest();
                BindSoleVendorRequestDetails();
                if (_presenter.CurrentSoleVendorRequest.Id <= 0)
                {
                    AutoNumber();
                    btnDelete.Visible = false;
                }
            }
            txtRequestDate.Text = DateTime.Today.Date.ToShortDateString();
            this._presenter.OnViewLoaded();

            BindSoleVendorRequests();
            // PopPurchaseRequest();
            if (_presenter.CurrentSoleVendorRequest != null)
            {
                if (_presenter.CurrentSoleVendorRequest.Id != 0)
                {
                    BindSoleVendorRequestDetails();
                }
            }

        }
        [CreateNew]
        public SoleVendorRequestPresenter Presenter
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
                return "{D1B7939C-7154-4403-B535-B4D33684CE21}";
            }
        }

        #region Field Getters
        public int GetSoleVendorRequestId
        {
            get
            {
                if (Convert.ToInt32(Request.QueryString["SoleVendorRequestId"]) != 0)
                {
                    return Convert.ToInt32(Request.QueryString["SoleVendorRequestId"]);
                }
                else if (grvSoleVendorRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvSoleVendorRequestList.SelectedDataKey.Value);
                }
                return 0;
            }
        }
        public int GetPurchaseRequestId
        {
            get
            {
                if (Convert.ToInt32(Request.QueryString["PurchaseRequestId"]) != 0)
                {
                    return Convert.ToInt32(Request.QueryString["PurchaseRequestId"]);
                }
                return 0;
            }
        }
        public string GetRequestNo
        {
            get { return AutoNumber(); }
        }
        public string GetComment
        {
            get { return txtComment.Text; }
        }
        public IList<SoleVendorRequest> SoleVendorRequests
        {
            get
            {
                return _SoleVendorRequests;
            }
            set
            {
                _SoleVendorRequests = value;
            }
        }
        #endregion
        private string AutoNumber()
        {
            return "SVR-" + (_presenter.GetLastSoleVendorRequestId() + 1).ToString();
        }
        private void CheckApprovalSettings()
        {
            if (_presenter.GetApprovalSetting(RequestType.SoleVendor_Request.ToString().Replace('_', ' '), 0) == null)
            {
                pnlWarning.Visible = true;
            }
        }
        private void ClearFormFields()
        {
            //txtContactPersonNumber.Text = String.Empty;
            //txtSoleSource.Text = String.Empty;

        }
        private void BindItems(DropDownList ddlItems)
        {
            ddlItems.DataSource = _presenter.GetItemAccounts();
            ddlItems.DataBind();
        }
        private void BindSupplier(DropDownList ddlSupplier)
        {
            ddlSupplier.DataSource = _presenter.GetSuppliers();
            ddlSupplier.DataBind();
        }
        private void BindSoleVendorRequests()
        {
            grvSoleVendorRequestList.DataSource = _presenter.ListSoleVendorRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text);
            grvSoleVendorRequestList.DataBind();
        }
        private void BindSoleVendorRequestFields()
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentSoleVendorRequest != null)
            {


                //ddlProject.SelectedValue = _presenter.CurrentSoleVendorRequest.Project.Id.ToString();
                //PopGrants(Convert.ToInt32(ddlProject.SelectedValue));
                //ddlGrant.SelectedValue = _presenter.CurrentSoleVendorRequest.Grant.Id.ToString();
                BindSoleVendorRequestDetails();
                BindSoleVendorRequests();
            }
        }
        private void BindSoleVendorRequestDetails()
        {
            dgSoleVenderDetail.DataSource = _presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails;
            dgSoleVenderDetail.DataBind();

            grvAttachments.DataSource = _presenter.CurrentSoleVendorRequest.SVRAttachments;
            grvAttachments.DataBind();

        }
        private void BindSoleVendorRequestforprint()
        {
            lblRequestNoresult.Text = _presenter.CurrentSoleVendorRequest.RequestNo;
            lblRequestedDateresult.Text = _presenter.CurrentSoleVendorRequest.RequestDate.ToString();
            lblapprovalstatusres.Text = _presenter.CurrentSoleVendorRequest.CurrentStatus;
            lblRequesterres.Text = _presenter.GetUser(_presenter.CurrentSoleVendorRequest.AppUser.Id).FullName;

            grvStatuses.DataSource = _presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses;
            grvStatuses.DataBind();
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_presenter.GetUser(_presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses[e.Row.RowIndex].Approver) != null)
                    {
                        e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses[e.Row.RowIndex].Approver).FullName;
                    }
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails.Count != 0)
                {
                    if (_presenter.CurrentSoleVendorRequest.SVRAttachments.Count != 0)
                    {
                        int PRID = Convert.ToInt32(Session["PRID"]);
                        _presenter.SaveOrUpdateSoleVendorRequest(PRID);
                        if (_presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses.Count != 0 && _presenter.CurrentSoleVendorRequest.SVRAttachments.Count != 0)
                        {
                            BindSoleVendorRequests();
                            Master.ShowMessage(new AppMessage("Successfully did a Sole Vendor  Request, Reference No - <b>'" + _presenter.CurrentSoleVendorRequest.RequestNo + "'</b>", Chai.WorkflowManagment.Enums.RMessageType.Info));
                            Log.Info(_presenter.CurrentUser().FullName + " has requested a for Sole Vendor");
                            btnSave.Visible = false;
                            BindSoleVendorRequestforprint();
                            btnPrint.Enabled = true;
                        }
                    }
                    else
                    {
                        Master.ShowMessage(new AppMessage("Please Attach Sole Vendor Quotation", RMessageType.Error));
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Violation of UNIQUE KEY"))
                    {
                        Master.ShowMessage(new AppMessage("Please Click Request button Again,There is a duplicate Number", RMessageType.Error));
                        //AutoNumber();
                    }
                }
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }

        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            _presenter.DeleteSoleVendorRequest(_presenter.CurrentSoleVendorRequest);
            ClearFormFields();
            BindSoleVendorRequests();
            btnDelete.Enabled = false;
            Master.ShowMessage(new AppMessage("Sole Vendor Request Successfully Deleted", Chai.WorkflowManagment.Enums.RMessageType.Info));
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindSoleVendorRequests();
            //pnlSearch_ModalPopupExtender.Show();
            ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            pnlWarning.Visible = false;
            _presenter.CancelPage();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmSoleVendorRequest.aspx");
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string fileName = Path.GetFileName(fuReciept.PostedFile.FileName);
            try
            {
                if (fileName != String.Empty)
                {
                    SVRAttachment attachment = new SVRAttachment();
                    attachment.FilePath = "~/SVUploads/" + fileName;
                    fuReciept.PostedFile.SaveAs(Server.MapPath("~/SVUploads/") + fileName);
                    //Response.Redirect(Request.Url.AbsoluteUri);
                    _presenter.CurrentSoleVendorRequest.SVRAttachments.Add(attachment);

                    grvAttachments.DataSource = _presenter.CurrentSoleVendorRequest.SVRAttachments;
                    grvAttachments.DataBind();
                    Master.ShowMessage(new AppMessage("Successfully uploaded the attachment", RMessageType.Info));
                }
                else
                {
                    Master.ShowMessage(new AppMessage("Please select file ", RMessageType.Error));
                }
            }
            catch (HttpException ex)
            {
                Master.ShowMessage(new AppMessage("Unable to upload the file; the file is to big or the internet is too slow! " + ex.InnerException.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void btnCreateSoleVendor_Click(object sender, EventArgs e)
        {
            try
            {
                _presenter.OnViewLoaded();
                btnSave.Visible = true;
                pnlInfo.Visible = false;

                foreach (GridViewRow item in grvDetails.Rows)
                {
                    int prDetailID = (int)grvDetails.DataKeys[item.RowIndex].Value;
                    Session["prId"] = prDetailID;

                    if (item.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chk = (CheckBox)item.FindControl("chkSelect");
                        if (chk.Checked)
                        {
                            if (_presenter.GetPurchaseRequestDetail(prDetailID) != null)
                            {
                                PurchaseRequestDetail PRDetail = _presenter.GetPurchaseRequestDetail(prDetailID);
                                SoleVendorRequestDetail svDetail = new SoleVendorRequestDetail();
                                svDetail.PRDetailID = PRDetail.Id;
                                svDetail.ItemAccount = PRDetail.ItemAccount;
                                svDetail.ItemDescription = PRDetail.Item;
                                svDetail.Qty = PRDetail.Qty;
                                svDetail.Project = PRDetail.Project;
                                svDetail.Grant = PRDetail.Grant;
                                _presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails.Add(svDetail);
                                _presenter.CurrentSoleVendorRequest.PurchaseRequest = PRDetail.PurchaseRequest;
                                BindSoleVendorRequestDetails();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Add SoleVendor Item " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }

        }
        protected void DownloadFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            Response.End();
        }
        protected void DeleteFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            _presenter.CurrentSoleVendorRequest.RemoveSVRAttachment(filePath);
            File.Delete(Server.MapPath(filePath));
            grvAttachments.DataSource = _presenter.CurrentSoleVendorRequest.SVRAttachments;
            grvAttachments.DataBind();
            //Response.Redirect(Request.Url.AbsoluteUri);
        }
        private void PopPurchaseRequest()
        {
            if (Convert.ToInt32(ddlPurchaseReq.SelectedValue) > 0)
            {
                grvDetails.DataSource = _presenter.ListPRDetailsInProgressById(Convert.ToInt32(ddlPurchaseReq.SelectedValue));
                //grvDetails.DataSource = _presenter.ListPurchaseReqInProgress();
                grvDetails.DataBind();
            }
        }
        private void PopPurchaseRequestsDropDown()
        {
            ddlPurchaseReq.Items.Clear();
            ListItem lst = new ListItem();
            lst.Text = "--Select Request No--";
            lst.Value = "0";
            ddlPurchaseReq.Items.Add(lst);
            ddlPurchaseReq.DataSource = _presenter.GetPurchaseRequestListInProgress();
            ddlPurchaseReq.DataBind();
        }
        protected void ddlPurchaseReq_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopPurchaseRequest();
            pnlInfo.Visible = true;
            grvDetails.Visible = true;
        }
        protected void dgSoleVenderDetail_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {


        }
        protected void dgSoleVenderDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (_presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails != null)
            {
                if (e.Item.ItemType == ListItemType.EditItem)
                {
                    DropDownList ddlEdtSupplier = e.Item.FindControl("ddlEdtSupplier") as DropDownList;
                    if (ddlEdtSupplier != null)
                    {
                        BindSupplier(ddlEdtSupplier);

                        if (_presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails[e.Item.DataSetIndex].SoleVendorSupplier != null)
                        {
                            if (_presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails[e.Item.DataSetIndex].SoleVendorSupplier.Id != 0)
                            {
                                ListItem liI = ddlEdtSupplier.Items.FindByValue(_presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails[e.Item.DataSetIndex].SoleVendorSupplier.Id.ToString());
                                if (liI != null)
                                    liI.Selected = true;
                            }
                        }
                    }
                }
            }
        }
        protected void dgSoleVenderDetail_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void dgSoleVenderDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                int id = (int)dgSoleVenderDetail.DataKeys[e.Item.ItemIndex];

                SoleVendorRequestDetail svDetail;
                if (id > 0)
                    svDetail = _presenter.CurrentSoleVendorRequest.GetSoleVendorRequestDetail(id);
                else
                    svDetail = _presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails[e.Item.ItemIndex];

                svDetail.SoleVendorRequest = _presenter.CurrentSoleVendorRequest;
                DropDownList ddlSup = e.Item.FindControl("ddlEdtSupplier") as DropDownList;
                svDetail.SoleVendorSupplier = _presenter.GetSoleVendorSupplier(Convert.ToInt32(ddlSup.SelectedValue));
                DropDownList ddlEdtSoleVendorJustification = e.Item.FindControl("ddlEdtSoleVendorJustification") as DropDownList;
                svDetail.SoleVendorJustificationType = ddlEdtSoleVendorJustification.SelectedValue;
                TextBox txtEdtReason = e.Item.FindControl("txtEdtReason") as TextBox;
                svDetail.ReasonForSelection = txtEdtReason.Text;
                TextBox txtUnitCost = e.Item.FindControl("txtEdtUnitCost") as TextBox;
                svDetail.UnitCost = Convert.ToDecimal(txtUnitCost.Text);
                TextBox txtTotalCost = e.Item.FindControl("txtEdtTotalCost") as TextBox;
                svDetail.TotalCost = Convert.ToInt32(svDetail.Qty) * Convert.ToDecimal(txtUnitCost.Text);
                svDetail.POStatus = "InProgress";

                //Change the status of the Purchase Request Detail so that the SoleVendor Item assosiated with it won't appear in the request page
                _presenter.CurrentSoleVendorRequest.PurchaseRequest.GetPurchaseRequestDetail(svDetail.PRDetailID).BidAnalysisRequestStatus = "Pending";

                dgSoleVenderDetail.EditItemIndex = -1;
                BindSoleVendorRequestDetails();
                Master.ShowMessage(new AppMessage("Sole Vendor Detail Successfully Updated", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Sole Vendor Detail" + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgSoleVenderDetail_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgSoleVenderDetail.DataKeys[e.Item.ItemIndex];
            int SVRDId = (int)dgSoleVenderDetail.DataKeys[e.Item.ItemIndex];
            SoleVendorRequestDetail tard;

            if (SVRDId > 0)
                tard = _presenter.CurrentSoleVendorRequest.GetSoleVendorRequestDetail(SVRDId);
            else
                tard = (SoleVendorRequestDetail)_presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails[e.Item.ItemIndex];
            try
            {
                if (SVRDId > 0)
                {
                    _presenter.CurrentSoleVendorRequest.RemoveSoleVendorRequestDetail(id);
                    if (_presenter.GetSoleVendorRequestDetail(id) != null)
                        _presenter.DeleteSoleVendorRequestDetail(_presenter.GetSoleVendorRequestDetail(id));
                    _presenter.SaveOrUpdateSoleVendorRequest(_presenter.CurrentSoleVendorRequest);
                }
                else { _presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails.Remove(tard); }


                Master.ShowMessage(new AppMessage("Sole Vendor Request Detail was Removed Successfully", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Sole Vendor Request Detail. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgSoleVenderDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            dgSoleVenderDetail.EditItemIndex = e.Item.ItemIndex;
            BindSoleVendorRequestDetails();
        }
        protected void grvSoleVendorRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvSoleVendorRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void grvSoleVendorRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["SoleVendorRequest"] = true;
            BindSoleVendorRequestFields();
            if (_presenter.CurrentSoleVendorRequest.CurrentStatus != null)
            {
                btnSave.Visible = false;
                btnDelete.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
                btnDelete.Visible = true;
            }
        }
        protected void grvSoleVendorRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                SoleVendorRequest soleVendorRequest = e.Row.DataItem as SoleVendorRequest;
                if (soleVendorRequest.CurrentStatus == "Rejected")
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        protected void grvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvDetails.PageIndex = e.NewPageIndex;
            PopPurchaseRequest();
        }
    }
}