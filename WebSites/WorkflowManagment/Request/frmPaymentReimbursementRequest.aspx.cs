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
            dgPaymentReimbursementDetail.DataSource = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails;
            dgPaymentReimbursementDetail.DataBind();
        }
        private void CheckandBindCashPaymentDetails()
        {
            if (_presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails.Count == 0)
            {
                PopulateReimbursement();
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
            grvCashPayments.Visible = false;
            pnlInfo.Visible = false;
            CheckandBindCashPaymentDetails();
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
        protected void dgPaymentReimbursementDetail_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgPaymentReimbursementDetail.EditItemIndex = -1;
        }
        protected void dgPaymentReimbursementDetail_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgPaymentReimbursementDetail.DataKeys[e.Item.ItemIndex];
            int CPRDId = (int)dgPaymentReimbursementDetail.DataKeys[e.Item.ItemIndex];
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
                    if (_presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.GetPaymentReimbursementRequestDetail(id) != null)
                        _presenter.DeletePaymentReimbursementRequestDetail(_presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.GetPaymentReimbursementRequestDetail(id));
                    _presenter.SaveOrUpdatePaymentReimbursementRequest(tarId);
                }
                else { _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails.Remove(cprd); }
                BindCashPaymentDetails();

                Master.ShowMessage(new AppMessage("Payment Reimbursement Request Detail was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Payment Request Reimbursement Detail. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgPaymentReimbursementDetail_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "AddNew")
            {
                try
                {
                    PaymentReimbursementRequestDetail cprd = new PaymentReimbursementRequestDetail();
                    TextBox txtAmount = e.Item.FindControl("txtAmount") as TextBox;
                    cprd.ActualExpendture = Convert.ToDecimal(txtAmount.Text);
                    TextBox txtAccountCode = e.Item.FindControl("txtAccountCode") as TextBox;
                    DropDownList ddlAccountDescription = e.Item.FindControl("ddlAccountDescription") as DropDownList;
                    cprd.ItemAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlAccountDescription.SelectedValue));
                    CheckBox ckSupDocAttached = e.Item.FindControl("ckSupDocAttached") as CheckBox;
                    cprd.SupportDocAttached = ckSupDocAttached.Checked;
                    txtImbursement.Text  += cprd.ActualExpendture;
                                       
                    _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails.Add(cprd);

                    dgPaymentReimbursementDetail.EditItemIndex = -1;
                    BindCashPaymentDetails();
                    Master.ShowMessage(new AppMessage("Payment Reimbursement Detail Successfully Added", RMessageType.Info));
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Save Payment Reimbursement detail" + ex.Message, RMessageType.Error));
                }
            }
        }
        protected void dgPaymentReimbursementDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgPaymentReimbursementDetail.EditItemIndex = e.Item.ItemIndex;
            BindCashPaymentDetails();
            //BindCarRentals();
        }
        protected void dgPaymentReimbursementDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            decimal previousAmount = 0;
            int CPRDId = (int)dgPaymentReimbursementDetail.DataKeys[e.Item.ItemIndex];
            PaymentReimbursementRequestDetail cprd;

            if (CPRDId > 0)
                cprd = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.GetPaymentReimbursementRequestDetail(CPRDId);
            else
                cprd = (PaymentReimbursementRequestDetail)_presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails[e.Item.ItemIndex];

            try
            {
                
                TextBox txtAmount = e.Item.FindControl("txtEdtAmount") as TextBox;
                previousAmount = cprd.ActualExpendture; //This is the Total Amount of this request before any edit
                cprd.ActualExpendture = Convert.ToDecimal(txtAmount.Text);
                TextBox txtEdtAccountCode = e.Item.FindControl("txtEdtAccountCode") as TextBox;
                //cprd.AccountCode = txtEdtAccountCode.Text;
                DropDownList ddlAccountDescription = e.Item.FindControl("ddlEdtAccountDescription") as DropDownList;
                cprd.ItemAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlAccountDescription.SelectedValue));
                CheckBox ckEdtSupDocAttached = e.Item.FindControl("ckEdtSupDocAttached") as CheckBox;
                cprd.SupportDocAttached = ckEdtSupDocAttached.Checked;
         
                txtImbursement.Text += cprd.ActualExpendture -= previousAmount; //Subtract the previous Total amount
                txtImbursement.Text += cprd.ActualExpendture += cprd.ActualExpendture; //Then add the new individual amounts to the Total amount



                dgPaymentReimbursementDetail.EditItemIndex = -1;
                BindCashPaymentDetails();
                Master.ShowMessage(new AppMessage("Payment Reimbursement Detail Successfully Updated", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Payment Reimbursement. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgPaymentReimbursementDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlProject = e.Item.FindControl("ddlProject") as DropDownList;
                int programID = Convert.ToInt32(ddlProgram.SelectedValue);
             
               // DropDownList ddlGrant = e.Item.FindControl("ddlGrant") as DropDownList;
               // BindGrant(ddlGrant, Convert.ToInt32(ddlProject.SelectedValue));
                DropDownList ddlAccountDescription = e.Item.FindControl("ddlAccountDescription") as DropDownList;
                BindAccountDescription(ddlAccountDescription);
            }
            else
            {
                if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails != null)
                {
                    DropDownList ddlProject = e.Item.FindControl("ddlEdtProject") as DropDownList;
                    int programID = Convert.ToInt32(ddlProgram.SelectedValue);
                   
                    DropDownList ddlAccountDescription = e.Item.FindControl("ddlEdtAccountDescription") as DropDownList;
                    if (ddlAccountDescription != null)
                    {
                        BindAccountDescription(ddlAccountDescription);
                        if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].ItemAccount.Id != 0)
                        {
                            ListItem liI = ddlAccountDescription.Items.FindByValue(_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].ItemAccount.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }
                    }
        
                }
            }
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
                List<CPRAttachment> attachments = (List<CPRAttachment>)Session["attachments"];
                foreach (CPRAttachment attachment in attachments)
                {
                    if (attachment.ItemAccountChecklists[0].ChecklistName == attachmentRow.Cells[1].Text)
                    {
                        attachment.FilePath = "~/PRUploads/" + fileName;
                        fuReciept.PostedFile.SaveAs(Server.MapPath("~/PRUploads/") + fileName);
                    }
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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (CheckReceiptsAttached() == true)
            {
             _presenter.SaveOrUpdatePaymentReimbursementRequest(Convert.ToInt32(Session["tarId"]));
            BindPaymentReimbursementRequests();
            Master.ShowMessage(new AppMessage("Payment Reimbursement Request Successfully Reimbursed!", Chai.WorkflowManagment.Enums.RMessageType.Info));
            btnSave.Visible = false;
            Session["tarId"] = null;
            }
            else { Master.ShowMessage(new AppMessage("Please attach the required receipts!", Chai.WorkflowManagment.Enums.RMessageType.Error)); }
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
            dgPaymentReimbursementDetail.DataSource = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails;
            dgPaymentReimbursementDetail.DataBind();
        }
        protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCashPaymentDetails();
        }
    }
}