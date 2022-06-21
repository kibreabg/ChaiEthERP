using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.CoreDomain.Setting;
using System.Text.RegularExpressions;
using System.Data.Entity.Infrastructure;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public partial class frmPaymentReimbursementRequest : POCBasePage, IPaymentReimbursementRequestView
    {
        private PaymentReimbursementRequestPresenter _presenter;
        private IList<PaymentReimbursementRequest> _PaymentReimbursementRequests;
        int cprId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                CheckApprovalSettings();
                BindCashPayments();
                BindPaymentReimbursementRequests();
            }
            txtRequestDate.Text = DateTime.Today.Date.ToShortDateString();
            this._presenter.OnViewLoaded();
        }
        [CreateNew]
        public PaymentReimbursementRequestPresenter Presenter
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
                return "{0B827E67-C83E-45CB-B048-A744BD4625C8}";
            }
        }

        #region Field Getters
        public int GetTARequestId
        {
            get
            {
                if (cprId != 0)
                {
                    return Convert.ToInt32(cprId);
                }
                else
                {
                    return 0;
                }
            }
        }
        public string GetComment
        {
            get { return txtComment.Text; }
        }
        public IList<PaymentReimbursementRequest> PaymentReimbursementRequests
        {
            get
            {
                return _PaymentReimbursementRequests;
            }
            set
            {
                _PaymentReimbursementRequests = value;
            }
        }
        #endregion
        private void CheckApprovalSettings()
        {
            if (_presenter.GetApprovalSetting(RequestType.PaymentReimbursement_Request.ToString().Replace('_', ' '), 0) == null)
            {
                pnlWarning.Visible = true;
            }
        }
        private void BindPaymentReimbursementRequests()
        {
            grvPaymentReimbursementRequestList.DataSource = _presenter.ListPaymentReimbursementRequests(txtSrchRequestDate.Text);
            grvPaymentReimbursementRequestList.DataBind();
        }
        private void BindCashPayments()
        {
            grvCashPayments.DataSource = _presenter.ListCashPaymentsNotExpensed();
            grvCashPayments.DataBind();
        }
        private void BindAttachments()
        {
            List<PRAttachment> attachments = new List<PRAttachment>();
            foreach (PaymentReimbursementRequestDetail detail in _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails)
            {
                attachments.AddRange(detail.PRAttachments);
                Session["attachments"] = attachments;
            }

            grvAttachments.DataSource = attachments;
            grvAttachments.DataBind();
        }
        private void BindPaymentReimbursementRequestFields()
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentCashPaymentRequest != null)
            {
                txtComment.Text = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.Comment;
                txtProject.Text = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.Project.ProjectCode;
                txtGrant.Text = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.Grant.GrantCode;
                BindPaymentReimbursementRequestDetails();
                BindPaymentReimbursementRequests();
                BindAttachments();
            }
        }
        private void PopulateReimbursement()
        {
            foreach (CashPaymentRequestDetail cprd in _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails)
            {
                txtProject.Text = cprd.Project.ProjectCode;
                txtGrant.Text = cprd.Grant.GrantCode;
            }
        }
        private void PrintTransaction()
        {
            if (_presenter.CurrentCashPaymentRequest != null)
            {
                lblRequestNoResult.Text = _presenter.CurrentCashPaymentRequest.RequestNo.ToString();
                lblRequesterResult.Text = _presenter.CurrentCashPaymentRequest.AppUser.UserName;
            }
            lblRequestedDateResult.Text = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.RequestDate.Value.ToShortDateString();
            lblCommentResult.Text = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.Comment.ToString();
            lblApprovalStatusResult.Text = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.ProgressStatus.ToString();
            lbladvancetakenresult.Text = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.ReceivableAmount.ToString();
            lblActualExpenditureresult.Text = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.TotalAmount.ToString();
            grvDetails.DataSource = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails;
            grvDetails.DataBind();

            grvStatuses.DataSource = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestStatuses;
            grvStatuses.DataBind();
        }
        private void BindPaymentReimbursementRequestDetails()
        {
            PopulateReimbursement();
            dgCashPaymentDetail.DataSource = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails;
            dgCashPaymentDetail.DataBind();
        }
        private void CheckandBindCashPaymentDetails()
        {
            if (_presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails.Count > 0)
            {
                foreach (PaymentReimbursementRequestDetail detail in _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails)
                {
                    txtImbursement.Text = ((txtImbursement.Text != "" ? Convert.ToDecimal(txtImbursement.Text) : 0) + detail.ActualExpenditure).ToString();
                }
            }

        }
        protected void grvPaymentReimbursementRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["PaymentReimbursementRequest"] = true;
            cprId = (int)grvPaymentReimbursementRequestList.DataKeys[grvPaymentReimbursementRequestList.SelectedIndex].Value;
            Session["cprId"] = (int)grvPaymentReimbursementRequestList.DataKeys[grvPaymentReimbursementRequestList.SelectedIndex].Value;
            BindPaymentReimbursementRequestFields();

            //This is done so that the user can not ammend a settlement while it's in an approval process. But one can ammend a rejected settlement.
            if (_presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.CurrentStatus != null)
            {
                if (_presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.CurrentStatus == ApprovalStatus.Rejected.ToString())
                {
                    btnSave.Visible = true;
                    btnDelete.Visible = true;
                }
                else
                {
                    btnSave.Visible = false;
                    btnDelete.Visible = false;
                    PrintTransaction();
                    btnPrint.Enabled = true;
                }
            }
            else
            {
                btnSave.Visible = true;
                btnDelete.Visible = true;
            }
        }
        protected void grvPaymentReimbursementRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //LinkButton db = (LinkButton)e.Row.Cells[5].Controls[0];
                //db.OnClientClick = "return confirm('Are you sure you want to delete this Recieve?');";
            }
        }
        protected void grvPaymentReimbursementRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvPaymentReimbursementRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void grvCashPayments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvCashPayments.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void grvCashPayments_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void grvCashPayments_SelectedIndexChanged(object sender, EventArgs e)
        {
            cprId = Convert.ToInt32(grvCashPayments.SelectedDataKey[0]);
            Session["cprId"] = Convert.ToInt32(grvCashPayments.SelectedDataKey[0]);
            _presenter.OnViewLoaded();
            btnSave.Visible = true;
            btnSave.Enabled = true;
            txtReceivables.Text = grvCashPayments.SelectedRow.Cells[4].Text;
            grvCashPayments.Visible = false;
            pnlInfo.Visible = false;
            _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest = new PaymentReimbursementRequest();
            CheckandBindCashPaymentDetails();
            BindPaymentReimbursementRequestDetails();
            BindAttachments();
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestStatuses[e.Row.RowIndex].Approver != 0)
                        e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestStatuses[e.Row.RowIndex].Approver).FullName;
                }
            }
        }
        protected void ddlAccountDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            TextBox txtAccountCode = ddl.FindControl("txtAccountCode") as TextBox;
            txtAccountCode.Text = _presenter.GetItemAccount(Convert.ToInt32(ddl.SelectedValue)).AccountCode;
        }
        protected void ddlEdtAccountDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            TextBox txtEdtAccountCode = ddl.FindControl("txtEdtAccountCode") as TextBox;
            txtEdtAccountCode.Text = _presenter.GetItemAccount(Convert.ToInt32(ddl.SelectedValue)).AccountCode;
        }
        private void BindAccountDescription(DropDownList ddlAccountDescription)
        {
            ddlAccountDescription.DataSource = _presenter.ListItemAccounts();

            ddlAccountDescription.DataValueField = "Id";
            ddlAccountDescription.DataTextField = "AccountName";
            ddlAccountDescription.DataBind();
        }
        protected void dgCashPaymentDetail_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgCashPaymentDetail.EditItemIndex = -1;
        }
        protected void dgCashPaymentDetail_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgCashPaymentDetail.DataKeys[e.Item.ItemIndex];
            int CPRDId = (int)dgCashPaymentDetail.DataKeys[e.Item.ItemIndex];
            PaymentReimbursementRequestDetail cprd;

            if (CPRDId > 0)
                cprd = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.GetPaymentReimbursementRequestDetail(CPRDId);
            else
                cprd = (PaymentReimbursementRequestDetail)_presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails[e.Item.ItemIndex];
            try
            {
                if (CPRDId > 0)
                {
                    _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.RemovePaymentReimbursementRequestDetail(id);
                    if (_presenter.GetPaymentReimbursementRequestDetail(id) != null)
                        _presenter.DeletePaymentReimbursementRequestDetail(_presenter.GetPaymentReimbursementRequestDetail(id));
                    _presenter.SaveOrUpdatePaymentReimbursementRequest(cprId);
                }
                else { _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails.Remove(cprd); }
                txtImbursement.Text = ((txtImbursement.Text != "" ? Convert.ToDecimal(txtImbursement.Text) : 0) - cprd.ActualExpenditure).ToString();
                _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.TotalAmount = (_presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.TotalAmount - cprd.ActualExpenditure);
                BindCashPaymentDetails();

                Master.ShowMessage(new AppMessage("Payment Request Detail was Removed Successfully", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Payment Request Detail. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgCashPaymentDetail_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "AddNew")
            {
                try
                {
                    PaymentReimbursementRequestDetail cprd = new PaymentReimbursementRequestDetail();
                    cprd.PaymentReimbursementRequest = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest;
                    TextBox txtAmount = e.Item.FindControl("txtAmount") as TextBox;
                    cprd.ActualExpenditure = Convert.ToDecimal(txtAmount.Text);
                    TextBox txtAccountCode = e.Item.FindControl("txtAccountCode") as TextBox;
                    cprd.AccountCode = txtAccountCode.Text;
                    DropDownList ddlAccountDescription = e.Item.FindControl("ddlAccountDescription") as DropDownList;
                    cprd.ItemAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlAccountDescription.SelectedValue));
                    DropDownList ddlProject = e.Item.FindControl("ddlProject") as DropDownList;
                    CheckBox ckSupDocAttached = e.Item.FindControl("ckSupDocAttached") as CheckBox;
                    cprd.SupportDocAttached = ckSupDocAttached.Checked;
                    txtImbursement.Text = ((txtImbursement.Text != "" ? Convert.ToDecimal(txtImbursement.Text) : 0) + cprd.ActualExpenditure).ToString();
                    //Add Checklists for attachments if available                    
                    foreach (ItemAccountChecklist checklist in cprd.ItemAccount.ItemAccountChecklists)
                    {
                        PRAttachment attachment = new PRAttachment();
                        attachment.PaymentReimbursementRequestDetail = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.GetDetailByItemAccount(cprd.ItemAccount.Id);
                        attachment.ItemAccountChecklists.Add(checklist);
                        cprd.PRAttachments.Add(attachment);
                    }
                    _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails.Add(cprd);
                    BindAttachments();

                    dgCashPaymentDetail.EditItemIndex = -1;
                    BindCashPaymentDetails();

                    foreach (PRAttachment attachment in _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails[e.Item.ItemIndex + 1].PRAttachments)
                    {
                        attachment.PaymentReimbursementRequestDetail = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.GetPaymentReimbursementRequestDetail((int)dgCashPaymentDetail.DataKeys[e.Item.ItemIndex + 1]);
                    }

                    Master.ShowMessage(new AppMessage("Payment Detail Successfully Added", RMessageType.Info));
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Save Payment Detail" + ex.Message, RMessageType.Error));
                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
                }
            }
        }
        protected void dgCashPaymentDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgCashPaymentDetail.EditItemIndex = e.Item.ItemIndex;
            BindCashPaymentDetails();
            //BindCarRentals();
        }
        protected void dgCashPaymentDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            decimal previousAmount = 0;
            int CPRDId = (int)dgCashPaymentDetail.DataKeys[e.Item.ItemIndex];
            PaymentReimbursementRequestDetail cprd;

            if (CPRDId > 0)
                cprd = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.GetPaymentReimbursementRequestDetail(CPRDId);
            else
                cprd = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails[e.Item.ItemIndex];

            try
            {
                cprd.PaymentReimbursementRequest = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest;
                TextBox txtAmount = e.Item.FindControl("txtEdtAmount") as TextBox;
                previousAmount = cprd.ActualExpenditure; //This is the Total Amount of this request before any edit
                cprd.ActualExpenditure = Convert.ToDecimal(txtAmount.Text);
                TextBox txtEdtAccountCode = e.Item.FindControl("txtEdtAccountCode") as TextBox;
                cprd.AccountCode = txtEdtAccountCode.Text;
                DropDownList ddlAccountDescription = e.Item.FindControl("ddlEdtAccountDescription") as DropDownList;
                cprd.ItemAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlAccountDescription.SelectedValue));
                CheckBox ckEdtSupDocAttached = e.Item.FindControl("ckEdtSupDocAttached") as CheckBox;
                cprd.SupportDocAttached = ckEdtSupDocAttached.Checked;
                txtImbursement.Text = ((txtImbursement.Text != "" ? Convert.ToDecimal(txtImbursement.Text) : 0) - previousAmount).ToString();
                txtImbursement.Text = (Convert.ToDecimal(txtImbursement.Text) + cprd.ActualExpenditure).ToString();

                //Add Checklists for attachments if available but clear all attachments first because this is update                    
                cprd.PRAttachments = new List<PRAttachment>();
                foreach (ItemAccountChecklist checklist in cprd.ItemAccount.ItemAccountChecklists)
                {
                    PRAttachment attachment = new PRAttachment();
                    attachment.PaymentReimbursementRequestDetail = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.GetDetailByItemAccount(cprd.ItemAccount.Id);
                    attachment.ItemAccountChecklists.Add(checklist);

                    cprd.PRAttachments.Add(attachment);
                }
                BindAttachments();

                dgCashPaymentDetail.EditItemIndex = -1;
                BindCashPaymentDetails();
                Master.ShowMessage(new AppMessage("Payment Detail Successfully Updated", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Payment. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgCashPaymentDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {

                DropDownList ddlAccountDescription = e.Item.FindControl("ddlAccountDescription") as DropDownList;
                BindAccountDescription(ddlAccountDescription);
                foreach (PaymentReimbursementRequestDetail Detail in _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails)
                {
                    foreach (ListItem item in ddlAccountDescription.Items)
                    {
                        if (Detail.ItemAccount.Id == Convert.ToInt32(item.Value))
                        {
                            item.Enabled = false;
                        }
                    }
                }

            }
            else
            {
                if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails != null)
                {

                    DropDownList ddlAccountDescription = e.Item.FindControl("ddlEdtAccountDescription") as DropDownList;
                    if (ddlAccountDescription != null)
                    {
                        BindAccountDescription(ddlAccountDescription);
                        if (_presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails[e.Item.DataSetIndex].ItemAccount.Id != 0)
                        {
                            ListItem liI = ddlAccountDescription.Items.FindByValue(_presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails[e.Item.DataSetIndex].ItemAccount.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }
                    }

                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDecimal(txtImbursement.Text) >= Convert.ToDecimal(txtReceivables.Text))
                {
                    if (CheckReceiptsAttached() == true)
                    {
                        _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.ReceivableAmount = _presenter.CurrentCashPaymentRequest.TotalAmount;
                        _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.Project = _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[0].Project;
                        _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.Grant = _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[0].Grant;

                        //For update cases make the totals equal to zero first then add up the individuals
                        _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.TotalAmount = 0;
                        foreach (PaymentReimbursementRequestDetail prrd in _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails)
                        {
                            _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.TotalAmount += prrd.ActualExpenditure;
                        }

                        _presenter.SaveOrUpdatePaymentReimbursementRequest(Convert.ToInt32(Session["cprId"]));
                        BindPaymentReimbursementRequests();
                        Master.ShowMessage(new AppMessage("Payment Settlement Request Successfully Saved! (If you have over-spend refund, please contact finance.)", RMessageType.Info));
                        btnSave.Visible = false;
                        Session["cprId"] = null;
                    }
                    else { Master.ShowMessage(new AppMessage("Please attach the required receipts!", RMessageType.Error)); }
                }
                else
                {
                    decimal variance = (Convert.ToDecimal(txtReceivables.Text) - Convert.ToDecimal(txtImbursement.Text));
                    Master.ShowMessage(new AppMessage("Please settle the remaining " + variance.ToString() + " birr with your receipt from Bank!", RMessageType.Error));
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName + " (Handled Concurrency Exception)");

                foreach (var entry in ex.Entries)
                {
                    // Update original values from the database
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
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

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindPaymentReimbursementRequests();
            ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            pnlWarning.Visible = false;
            _presenter.CancelPage();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmPaymentReimbursementRequest.aspx");
        }
        private void BindCashPaymentDetails()
        {
            dgCashPaymentDetail.DataSource = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails;
            dgCashPaymentDetail.DataBind();
        }
        #region Attachments
        protected bool CheckReceiptsAttached()
        {
            int numAttachedReceipts = 0;
            int numChecklists = 0;
            foreach (PaymentReimbursementRequestDetail detail in _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails)
            {
                foreach (PRAttachment attachment in detail.PRAttachments)
                {
                    if (attachment.FilePath != null)
                    {
                        numAttachedReceipts++;
                    }
                }
            }
            foreach (PaymentReimbursementRequestDetail detail in _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails)
            {
                foreach (ItemAccountChecklist checklist in detail.ItemAccount.ItemAccountChecklists)
                {
                    numChecklists++;
                }
            }
            if (numAttachedReceipts == numChecklists)
                return true;
            else
                return false;
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            Button uploadBtn = (Button)sender;
            GridViewRow attachmentRow = (GridViewRow)uploadBtn.NamingContainer;
            FileUpload fuReciept = attachmentRow.FindControl("fuReciept") as FileUpload;
            string fileName = Path.GetFileNameWithoutExtension(fuReciept.PostedFile.FileName);
            Regex rx = new Regex(@"[^A-Za-z0-9]");
            fileName = rx.Replace(fileName, "");
            string extension = Path.GetExtension(fuReciept.PostedFile.FileName);

            if (fileName != String.Empty)
            {
                List<PRAttachment> attachments = (List<PRAttachment>)Session["attachments"];
                int index = 0;
                foreach (PRAttachment attachment in attachments)
                {
                    if (attachment.ItemAccountChecklists[0].ChecklistName == attachmentRow.Cells[1].Text && attachmentRow.DataItemIndex == index)
                    {
                        attachment.FilePath = "~/PRUploads/" + fileName + DateTime.Now.ToString("ddMMyyyyHHmmss") + extension;
                        fuReciept.PostedFile.SaveAs(Server.MapPath("~/PRUploads/") + fileName + DateTime.Now.ToString("ddMMyyyyHHmmss") + extension);
                    }
                    index++;
                }

                BindAttachments();
                Master.ShowMessage(new AppMessage("Successfully uploaded the attachment", RMessageType.Info));

            }
            else
            {
                Master.ShowMessage(new AppMessage("Please select file ", RMessageType.Error));
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
            //Deleting attachment shouldn't take place since there are checklists implemented now
            string filePath = (sender as LinkButton).CommandArgument;
            foreach (PaymentReimbursementRequestDetail detail in _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails)
            {
                foreach (PRAttachment attachment in detail.PRAttachments)
                {
                    if (attachment.FilePath == filePath)
                    {
                        detail.RemovePRAttachment(filePath);
                        File.Delete(Server.MapPath(filePath));
                    }
                }
            }
            BindAttachments();
            //Response.Redirect(Request.Url.AbsoluteUri);
        }
        #endregion        
    }
}