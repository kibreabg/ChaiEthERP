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

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public partial class frmPaymentReimbursementRequest : POCBasePage, IPaymentReimbursementRequestView
    {
        private PaymentReimbursementRequestPresenter _presenter;
        private IList<PaymentReimbursementRequest> _PaymentReimbursementRequests;
        int tarId;
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
                if (tarId != 0)
                {
                    return Convert.ToInt32(tarId);
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
        private void BindPrograms()
        {
            ddlProgram.DataSource = _presenter.GetPrograms();
            ddlProgram.DataBind();
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
                _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.TotalAmount = 0;
                _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails = null;
                BindPaymentReimbursementRequests();
                //grvAttachments.DataSource = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.CPRAttachments;
                //grvAttachments.DataBind();
            }
        }
        private void PopulateReimbursement()
        {
            foreach (CashPaymentRequestDetail CPRD in _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails)
            {

                _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.ReceivableAmount += CPRD.Amount;
                _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.Project = CPRD.Project;
                _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.Grant = CPRD.Grant;
                txtProject.Text = CPRD.Project.ProjectCode;
                txtGrant.Text = CPRD.Grant.GrantCode;

            }
        }
        private void BindPaymentReimbursementRequestDetails()
        {
            dgCashPaymentDetail.DataSource = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails;
            dgCashPaymentDetail.DataBind();
        }
        private void CheckandBindCashPaymentDetails()
        {
            PopulateReimbursement();
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
            //ClearForm();
            BindPaymentReimbursementRequestFields();
            btnSave.Visible = true;
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
            tarId = Convert.ToInt32(grvCashPayments.SelectedDataKey[0]);
            Session["tarId"] = Convert.ToInt32(grvCashPayments.SelectedDataKey[0]);
            _presenter.OnViewLoaded();
            btnSave.Visible = true;
            btnSave.Enabled = true;
            txtReceivables.Text = grvCashPayments.SelectedRow.Cells[4].Text;
            //grvCashPayments.Visible = false;
            pnlInfo.Visible = false;
            CheckandBindCashPaymentDetails();
            BindPaymentReimbursementRequestDetails();
            BindAttachments();
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

        private void BindProject(DropDownList ddlProject, int programID)
        {
            ddlProject.DataSource = _presenter.ListProjects(programID);
            ddlProject.DataValueField = "Id";
            ddlProject.DataTextField = "ProjectCode";
            ddlProject.DataBind();
        }
        private void BindGrant(DropDownList ddlGrant, int ProjectId)
        {
            ddlGrant.Items.Clear();
            ddlGrant.DataSource = _presenter.GetGrantbyprojectId(ProjectId);
            ddlGrant.DataValueField = "Id";
            ddlGrant.DataTextField = "GrantCode";
            ddlGrant.DataBind();
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
                    _presenter.SaveOrUpdatePaymentReimbursementRequest(tarId);
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
                    _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.TotalAmount += cprd.ActualExpenditure;
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
                _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.TotalAmount -= previousAmount; //Subtract the previous Total amount
                _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.TotalAmount += cprd.ActualExpenditure; //Then add the new individual amounts to the Total amount
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
            if (Convert.ToDecimal(txtImbursement.Text) >= Convert.ToDecimal(txtReceivables.Text))
            {
                if (CheckReceiptsAttached() == true)
                {
                    _presenter.SaveOrUpdatePaymentReimbursementRequest(Convert.ToInt32(Session["tarId"]));
                    BindPaymentReimbursementRequests();
                    Master.ShowMessage(new AppMessage("Payment Settlement Request Successfully Saved, If you have over spend refund please Contact Finance", RMessageType.Info));
                    btnSave.Visible = false;
                    Session["tarId"] = null;
                }
                else { Master.ShowMessage(new AppMessage("Please attach the required receipts!", RMessageType.Error)); }
            }
            else
            {
                Master.ShowMessage(new AppMessage("Please Make sure that settled amount is greater than or equal to advance taken.", RMessageType.Error));
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
            pnlSearch_ModalPopupExtender.Show();
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
        protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCashPaymentDetails();
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
            string fileName = Path.GetFileName(fuReciept.PostedFile.FileName);
            if (fileName != String.Empty)
            {
                List<PRAttachment> attachments = (List<PRAttachment>)Session["attachments"];
                int index = 0;
                foreach (PRAttachment attachment in attachments)
                {
                    if (attachment.ItemAccountChecklists[0].ChecklistName == attachmentRow.Cells[1].Text && attachmentRow.DataItemIndex == index)
                    {
                        attachment.FilePath = "~/PRUploads/" + fileName;
                        fuReciept.PostedFile.SaveAs(Server.MapPath("~/PRUploads/") + fileName);
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