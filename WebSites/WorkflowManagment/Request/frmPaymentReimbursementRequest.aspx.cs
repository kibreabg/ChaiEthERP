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
                
            }
        }
        private void BindPaymentReimbursementDetails()
        {
            if (_presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails.Count == 0)
            {
                PopulateReimbursement();
            }
            dgPaymentReimbursementDetail.DataSource = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails;
            dgPaymentReimbursementDetail.DataBind();
        }
        private void SetReimbursementDetails()
        {
            int index = 0;
            foreach (DataGridItem dgi in dgPaymentReimbursementDetail.Items)
            {
                int id = (int)dgPaymentReimbursementDetail.DataKeys[dgi.ItemIndex];

                PaymentReimbursementRequestDetail detail;
                if (id > 0)
                    detail = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.GetPaymentReimbursementRequestDetail(id);
                else
                    detail = (PaymentReimbursementRequestDetail)_presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails[index];

                TextBox txtActualExpenditure = dgi.FindControl("txtActualExpenditure") as TextBox;
                detail.ActualExpenditure = Convert.ToDecimal(txtActualExpenditure.Text);
                TextBox txtVariance = dgi.FindControl("txtVariance") as TextBox;
                detail.Variance = Convert.ToDecimal(txtVariance.Text);
                index++;
                _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails.Add(detail);
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
            BindPaymentReimbursementDetails();
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
        protected void ddlEdtProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlEdtGrant = ddl.FindControl("ddlEdtGrant") as DropDownList;
            BindGrant(ddlEdtGrant, Convert.ToInt32(ddl.SelectedValue));
        }
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlFGrant = ddl.FindControl("ddlGrant") as DropDownList;
            BindGrant(ddlFGrant, Convert.ToInt32(ddl.SelectedValue));
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

                Master.ShowMessage(new AppMessage("Payment eimbursement Request Detail was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Payment Request eimbursement Detail. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
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
                    cprd.ActualExpenditure = Convert.ToDecimal(txtAmount.Text);
                    TextBox txtAccountCode = e.Item.FindControl("txtAccountCode") as TextBox;
                    DropDownList ddlAccountDescription = e.Item.FindControl("ddlAccountDescription") as DropDownList;
                    cprd.ItemAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlAccountDescription.SelectedValue));
                    DropDownList ddlProject = e.Item.FindControl("ddlProject") as DropDownList;
                    cprd.Project = _presenter.GetProject(Convert.ToInt32(ddlProject.SelectedValue));
                    DropDownList ddlGrant = e.Item.FindControl("ddlGrant") as DropDownList;
                    //cprd. = _presenter.GetGrant(int.Parse(ddlGrant.SelectedValue));
                    txtImbursement.Text  += cprd.ActualExpenditure;
                    //_presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.TotalAmount += cprd.ActualExpenditure;
                   
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
                previousAmount = cprd.ActualExpenditure; //This is the Total Amount of this request before any edit
                cprd.ActualExpenditure = Convert.ToDecimal(txtAmount.Text);
                TextBox txtEdtAccountCode = e.Item.FindControl("txtEdtAccountCode") as TextBox;
                //cprd.AccountCode = txtEdtAccountCode.Text;
                DropDownList ddlAccountDescription = e.Item.FindControl("ddlEdtAccountDescription") as DropDownList;
                cprd.ItemAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlAccountDescription.SelectedValue));
                DropDownList ddlProject = e.Item.FindControl("ddlEdtProject") as DropDownList;
                cprd.Project = _presenter.GetProject(Convert.ToInt32(ddlProject.SelectedValue));
                DropDownList ddlGrant = e.Item.FindControl("ddlEdtGrant") as DropDownList;
                // cprd.Grant = _presenter.GetGrant(int.Parse(ddlGrant.SelectedValue));
                txtImbursement.Text += cprd.ActualExpenditure -= previousAmount; //Subtract the previous Total amount
                txtImbursement.Text += cprd.ActualExpenditure += cprd.ActualExpenditure; //Then add the new individual amounts to the Total amount



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
                BindProject(ddlProject, programID);
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
                    if (ddlProject != null)
                    {
                        BindProject(ddlProject, programID);
                        if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].Project.Id != 0)
                        {
                            ListItem liI = ddlProject.Items.FindByValue(_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].Project.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }
                    }
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
                    //DropDownList ddlEdtGrant = e.Item.FindControl("ddlEdtGrant") as DropDownList;
                    //if (ddlEdtGrant != null)
                    //{
                    //    BindGrant(ddlEdtGrant, Convert.ToInt32(ddlProject.SelectedValue));
                    //    if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].Grant.Id != 0)
                    //    {
                    //        ListItem liI = ddlEdtGrant.Items.FindByValue(_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].Grant.Id.ToString());
                    //        if (liI != null)
                    //            liI.Selected = true;
                    //    }

                    //}
                }
            }
        }
        protected void txtActualExpenditure_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            HiddenField hfAmountAdvanced = txt.FindControl("hfAmountAdvanced") as HiddenField;
            TextBox txtActualExpenditure = txt.FindControl("txtActualExpenditure") as TextBox;
            TextBox txtVariance = txt.FindControl("txtVariance") as TextBox;
            txtVariance.Text = ((Convert.ToDecimal(hfAmountAdvanced.Value) - Convert.ToDecimal(txtActualExpenditure.Text))).ToString();
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            UploadFile();
        }
        private void UploadFile()
        {
            string fileName = Path.GetFileName(fuReciept.PostedFile.FileName);
            try
            {
                if (fileName != String.Empty)
                {



                    PRAttachment attachment = new PRAttachment();
                    attachment.FilePath = "~/PRUploads/" + fileName;
                    fuReciept.PostedFile.SaveAs(Server.MapPath("~/PRUploads/") + fileName);
                    //Response.Redirect(Request.Url.AbsoluteUri);
                    _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PRAttachments.Add(attachment);

                    grvAttachments.DataSource = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PRAttachments;
                    grvAttachments.DataBind();
                    Master.ShowMessage(new AppMessage("Successfully uploaded the attachment", RMessageType.Info));

                }
                else
                {
                    Master.ShowMessage(new AppMessage("Please select file ", Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
            catch (HttpException ex)
            {
                Master.ShowMessage(new AppMessage("Unable to upload the file,The file is to big or The internet is too slow " + ex.InnerException.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
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
            /*
            string filePath = (sender as LinkButton).CommandArgument;
            _presenter.CurrentCashPaymentRequest.RemoveCPAttachment(filePath);
            File.Delete(Server.MapPath(filePath));
            grvAttachments.DataSource = _presenter.CurrentCashPaymentRequest.CPRAttachments;
            grvAttachments.DataBind();
            //Response.Redirect(Request.Url.AbsoluteUri);*/
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (_presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PRAttachments.Count != 0)
            {
                SetReimbursementDetails();
            _presenter.SaveOrUpdatePaymentReimbursementRequest(Convert.ToInt32(Session["tarId"]));
            BindPaymentReimbursementRequests();
            Master.ShowMessage(new AppMessage("Cash Payment Successfully Reimbursed!", Chai.WorkflowManagment.Enums.RMessageType.Info));
            btnSave.Visible = false;
            Session["tarId"] = null;
            }
            else { Master.ShowMessage(new AppMessage("Please attach Receipt", Chai.WorkflowManagment.Enums.RMessageType.Error)); }
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