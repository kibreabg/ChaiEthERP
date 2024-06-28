﻿using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Shared.MailSender;
using log4net;
using log4net.Config;
using Microsoft.Practices.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public partial class frmCashPaymentApproval : POCBasePage, ICashPaymentApprovalView
    {
        private CashPaymentApprovalPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        private int reqID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                PopProgressStatus();
                ddlApprovalStatus.Items.Clear();
                PopRequesters(ddlSrchRequester);
                BindSearchCashPaymentRequestGrid();
            }
            this._presenter.OnViewLoaded();
            if (_presenter.CurrentCashPaymentRequest != null)
            {
                if (_presenter.CurrentCashPaymentRequest.Id != 0)
                {
                    PrintTransaction();
                }

            }
        }
        [CreateNew]
        public CashPaymentApprovalPresenter Presenter
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
                return "{00397B85-1427-4EE2-94D7-7A1E8650A568}";
            }
        }
        #region Field Getters
        public int GetCashPaymentRequestId
        {
            get
            {
                if (grvCashPaymentRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvCashPaymentRequestList.SelectedDataKey.Value);
                }
                else if (Convert.ToInt32(Session["ReqID"]) != 0)
                {
                    return Convert.ToInt32(Session["ReqID"]);
                }
                else
                {
                    return 0;
                }
            }
        }
        public int GetAccountId
        {
            get { return Convert.ToInt32(ddlAccount.SelectedValue); }
        }
        #endregion
        private void PopApprovalStatus()
        {
            ddlApprovalStatus.Items.Clear();
            ddlApprovalStatus.Items.Add(new ListItem("Select Status", "0"));

            string[] s = Enum.GetNames(typeof(ApprovalStatus));

            for (int i = 0; i < s.Length; i++)
            {
                if (GetWillStatus() != "")
                {
                    if (GetWillStatus().Substring(0, 3) == s[i].Substring(0, 3))
                    {
                        ddlApprovalStatus.Items.Add(new ListItem(s[i].Replace('_', ' '), s[i].Replace('_', ' ')));
                    }
                }
            }
            if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses.Count == _presenter.CurrentCashPaymentRequest.CurrentLevel && ddlAccount.SelectedValue.Equals(ApprovalStatus.Bank_Payment.ToString().Replace('_', ' ')))
            {
                ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Bank_Payment.ToString().Replace('_', ' '), ApprovalStatus.Bank_Payment.ToString().Replace('_', ' ')));
                ddlApprovalStatus.Items.Remove(new ListItem("Pay"));
            }
            ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Rejected.ToString().Replace('_', ' '), ApprovalStatus.Rejected.ToString().Replace('_', ' ')));

        }
        private void PopRequesters(DropDownList ddl)
        {
            ddl.DataSource = _presenter.GetEmployeeList();
            ddl.DataTextField = "FullName";
            ddl.DataValueField = "ID";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem("Select Requester", "0"));
            ddl.SelectedIndex = 0;
        }
        private string GetWillStatus()
        {
            ApprovalSetting AS = null;
            if (_presenter.CurrentCashPaymentRequest.RequestType == "Medical Expense (In-Patient)" || _presenter.CurrentCashPaymentRequest.RequestType == "Medical Expense (Out-Patient)")
            {
                AS = _presenter.GetApprovalSettingMedical();
            }
            else
            {
                AS = _presenter.GetApprovalSettingforProcess(RequestType.CashPayment_Request.ToString().Replace('_', ' ').ToString(), _presenter.CurrentCashPaymentRequest.TotalAmount);
            }

            string will = "";
            foreach (ApprovalLevel AL in AS.ApprovalLevels)
            {
                //If the approver is a Superviser/Line Manager OR
                //If the approver is a Program Manager OR
                //If the current approver is the supervisor of the requester
                //Eg. Dr. Rahel is supervisor of Seble and she must approve Medical requested by Seble rather than Seble approving her requests
                if ((AL.EmployeePosition.PositionName == "Superviser/Line Manager"
                    || AL.EmployeePosition.PositionName == "Program Manager"
                    || _presenter.CurrentCashPaymentRequest.AppUser.Superviser == _presenter.CurrentCashPaymentRequest.CurrentApprover)
                    && AL.WorkflowLevel == _presenter.CurrentCashPaymentRequest.CurrentLevel)
                {
                    will = "Approve";
                    break;
                }
                else
                {
                    try
                    {
                        if (_presenter.GetUser(_presenter.CurrentCashPaymentRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName && AL.WorkflowLevel == _presenter.CurrentCashPaymentRequest.CurrentLevel)
                        {
                            will = AL.Will;
                            break;
                        }
                    }
                    catch
                    {
                        if (_presenter.CurrentCashPaymentRequest.CurrentApproverPosition == AL.EmployeePosition.Id && AL.WorkflowLevel == _presenter.CurrentCashPaymentRequest.CurrentLevel)
                        {
                            will = AL.Will;
                            break;
                        }
                    }
                }

            }
            return will;
        }
        private void PopProgressStatus()
        {
            string[] s = Enum.GetNames(typeof(ProgressStatus));

            for (int i = 0; i < s.Length; i++)
            {
                ddlSrchProgressStatus.Items.Add(new ListItem(s[i].Replace('_', ' '), s[i].Replace('_', ' ')));
                ddlSrchProgressStatus.DataBind();
            }
            ddlSrchProgressStatus.Items.Add(new ListItem("Reviewed", "Reviewed"));
            ddlSrchProgressStatus.Items.Add(new ListItem(ApprovalStatus.Rejected.ToString(), ApprovalStatus.Rejected.ToString()));
        }
        private void BindSearchCashPaymentRequestGrid()
        {
            grvCashPaymentRequestList.DataSource = _presenter.ListCashPaymentRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text, ddlSrchProgressStatus.SelectedValue, ddlSrchRequester.SelectedValue);
            grvCashPaymentRequestList.DataBind();
        }
        private void BindCashPaymentRequestStatus()
        {
            foreach (CashPaymentRequestStatus CPRS in _presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses)
            {
                if (CPRS.WorkflowLevel == _presenter.CurrentCashPaymentRequest.CurrentLevel && _presenter.CurrentCashPaymentRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    btnApprove.Enabled = true;
                }

                if (_presenter.CurrentCashPaymentRequest.CurrentLevel == _presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses.Count && CPRS.ApprovalStatus != null)
                {
                    btnPrint.Enabled = true;
                    btnApprove.Enabled = false;
                }
                else if (_presenter.CurrentCashPaymentRequest.CurrentStatus == ApprovalStatus.Rejected.ToString())
                {
                    btnApprove.Enabled = false;
                    btnBankPayment.Visible = false;
                }
                else
                {
                    btnPrint.Enabled = false;
                    btnApprove.Enabled = true;
                }
            }

            if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses.Last().PaymentType == "Bank Payment" && !IsCashPaymentRequested() && _presenter.CurrentCashPaymentRequest.CurrentStatus != ApprovalStatus.Rejected.ToString())
                btnBankPayment.Visible = true;
            else
                btnBankPayment.Visible = false;
        }
        private void BindAttachments()
        {
            List<CPRAttachment> attachments = new List<CPRAttachment>();
            foreach (CashPaymentRequestDetail detail in _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails)
            {
                attachments.AddRange(detail.CPRAttachments);
                Session["attachments"] = attachments;
            }

            grvdetailAttachments.DataSource = attachments;
            grvdetailAttachments.DataBind();
        }
        private void BindAccounts()
        {
            ddlAccount.SelectedValue = " ";
            if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses.Count == _presenter.CurrentCashPaymentRequest.CurrentLevel && (_presenter.CurrentUser().EmployeePosition.PositionName == "Accountant"))
            {
                lblAccount.Visible = true;
                lblAccountdd.Visible = true;
            }
            else
            {
                lblAccount.Visible = false;
                lblAccountdd.Visible = false;
            }
        }
        private void BindProgram(DropDownList ddlProgram)
        {
            ddlProgram.DataSource = _presenter.GetPrograms();
            ddlProgram.DataValueField = "Id";
            ddlProgram.DataTextField = "ProgramName";
            ddlProgram.DataBind();
        }
        private void BindProject(DropDownList ddlProject, int programID)
        {
            ddlProject.Items.Clear();
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
        private void BindAccountDescription(DropDownList ddlAccountDescription)
        {
            ddlAccountDescription.DataSource = _presenter.ListItemAccounts();
            ddlAccountDescription.DataValueField = "Id";
            ddlAccountDescription.DataTextField = "AccountName";
            ddlAccountDescription.DataBind();
        }
        private void ShowPrint()
        {
            if (_presenter.CurrentCashPaymentRequest.CurrentLevel == _presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses.Count && _presenter.CurrentCashPaymentRequest.ProgressStatus == ProgressStatus.Completed.ToString())
            {
                btnPrint.Enabled = true;
                if (ddlApprovalStatus.SelectedValue != ApprovalStatus.Rejected.ToString())
                    SendEmailToRequester();
            }
        }
        private void SendEmail(CashPaymentRequestStatus CPRS)
        {
            if (CPRS.Approver != 0)
            {
                if (_presenter.GetUser(CPRS.Approver).IsAssignedJob != true)
                {
                    EmailSender.Send(_presenter.GetUser(CPRS.Approver).Email, "Payment Approval", (_presenter.CurrentCashPaymentRequest.AppUser.FullName).ToUpper() + " Requests for Payment with Request No. " + (_presenter.CurrentCashPaymentRequest.RequestNo).ToUpper());
                }
                else
                {
                    EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(CPRS.Approver).AssignedTo).Email, "Payment Approval", (_presenter.CurrentCashPaymentRequest.AppUser.FullName).ToUpper() + " Requests for Payment with Request No. " + (_presenter.CurrentCashPaymentRequest.RequestNo).ToUpper());
                }
            }
            else
            {
                foreach (AppUser Payer in _presenter.GetAppUsersByEmployeePosition(CPRS.ApproverPosition))
                {
                    if (Payer.IsAssignedJob != true)
                    {
                        //Commented out because Finance team got tired of emails
                        //EmailSender.Send(Payer.Email, "Payment Approval", (_presenter.CurrentCashPaymentRequest.AppUser.FullName).ToUpper() + " Requests for Payment with Request No. " + (_presenter.CurrentCashPaymentRequest.RequestNo).ToUpper());
                    }
                    else
                    {
                        //Commented out because Finance team got tired of emails
                        //EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(Payer.Id).AssignedTo).Email, "Payment Approval", (_presenter.CurrentCashPaymentRequest.AppUser.FullName).ToUpper() + " Requests for Payment with Request No. '" + (_presenter.CurrentCashPaymentRequest.RequestNo).ToUpper());
                    }
                }
            }

        }
        private void SendEmailRejected(CashPaymentRequestStatus CPRS)
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentCashPaymentRequest.AppUser.Id).Email, "Payment Request Rejection", "Your Payment Request with Voucher No. " + (_presenter.CurrentCashPaymentRequest.VoucherNo).ToUpper() + " made by " + (_presenter.GetUser(_presenter.CurrentCashPaymentRequest.AppUser.Id).FullName).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (CPRS.RejectedReason).ToUpper() + "'");

            if (CPRS.WorkflowLevel > 1)
            {
                for (int i = 0; i + 1 < CPRS.WorkflowLevel; i++)
                {
                    EmailSender.Send(_presenter.GetUser(_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses[i].Approver).Email, "Payment Request Rejection", "Payment Request with Voucher No. " + (_presenter.CurrentCashPaymentRequest.VoucherNo).ToUpper() + " made by " + (_presenter.GetUser(_presenter.CurrentCashPaymentRequest.AppUser.Id).FullName).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (CPRS.RejectedReason).ToUpper() + "'");
                }
            }
        }
        private void SendEmailToRequester()
        {
            if (_presenter.CurrentCashPaymentRequest.PaymentReimbursementStatus != "Bank Payment")
                EmailSender.Send(_presenter.GetUser(_presenter.CurrentCashPaymentRequest.AppUser.Id).Email, "Collect your Payment ", "Your Payment Request for Payment - '" + (_presenter.CurrentCashPaymentRequest.RequestNo).ToUpper() + "' was Completed, Please collect your payment");
            else
                EmailSender.Send(_presenter.GetUser(_presenter.CurrentCashPaymentRequest.AppUser.Id).Email, "Bank Payment Process Started ", "Your Payment Request for Payment - '" + (_presenter.CurrentCashPaymentRequest.RequestNo).ToUpper() + "' is being processed for Bank Payment");
        }
        private void GetNextApprover()
        {
            foreach (CashPaymentRequestStatus CPRS in _presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses)
            {
                if (CPRS.ApprovalStatus == null)
                {
                    if (CPRS.Approver == 0)
                    {
                        //This is to handle multiple Finance Officers responding to this request
                        //SendEmailToFinanceOfficers;
                        _presenter.CurrentCashPaymentRequest.CurrentApproverPosition = CPRS.ApproverPosition;
                    }
                    else
                    {
                        _presenter.CurrentCashPaymentRequest.CurrentApproverPosition = 0;
                    }
                    SendEmail(CPRS);
                    _presenter.CurrentCashPaymentRequest.CurrentApprover = CPRS.Approver;
                    _presenter.CurrentCashPaymentRequest.CurrentLevel = CPRS.WorkflowLevel;
                    _presenter.CurrentCashPaymentRequest.CurrentStatus = CPRS.ApprovalStatus;
                    _presenter.CurrentCashPaymentRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                    break;
                }
            }
        }
        private bool IsCashPaymentRequested()
        {
            OperationalControlRequest ocr = _presenter.GetOperationalControlRequestByPaymentId(_presenter.CurrentCashPaymentRequest.Id);
            if (ocr != null)
                return true;
            else
                return false;
        }
        private bool IsCashPaymentRequested(int cashPaymentId)
        {
            OperationalControlRequest ocr = _presenter.GetOperationalControlRequestByPaymentId(cashPaymentId);
            if (ocr != null)
                return true;
            else
                return false;
        }
        private void SaveCashPaymentRequestStatus()
        {
            foreach (CashPaymentRequestStatus CPRS in _presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses)
            {
                if ((CPRS.Approver == _presenter.CurrentUser().Id || (CPRS.ApproverPosition == _presenter.CurrentUser().EmployeePosition.Id) || _presenter.CurrentUser().Id == (_presenter.GetAssignedJobbycurrentuser(CPRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(CPRS.Approver).AssignedTo : 0)) && CPRS.WorkflowLevel == _presenter.CurrentCashPaymentRequest.CurrentLevel)
                {
                    CPRS.ApprovalStatus = ddlApprovalStatus.SelectedValue;
                    CPRS.RejectedReason = txtRejectedReason.Text;
                    CPRS.PaymentType = ddlAccount.SelectedValue;
                    CPRS.Date = DateTime.Now;
                    CPRS.AssignedBy = _presenter.GetAssignedJobbycurrentuser(CPRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(CPRS.Approver).AppUser.FullName : "";
                    if (CPRS.ApprovalStatus != ApprovalStatus.Rejected.ToString())
                    {
                        if (_presenter.CurrentCashPaymentRequest.CurrentLevel == _presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses.Count)
                        {
                            _presenter.CurrentCashPaymentRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        }
                        GetNextApprover();
                        CPRS.Approver = _presenter.CurrentUser().Id;
                        _presenter.CurrentCashPaymentRequest.CurrentStatus = CPRS.ApprovalStatus;
                        if (CPRS.PaymentType.Contains("Bank Payment"))
                        {
                            btnBankPayment.Visible = true;
                            _presenter.CurrentCashPaymentRequest.PaymentReimbursementStatus = "Bank Payment";
                        }
                        Log.Info(_presenter.GetUser(CPRS.Approver).FullName + " has " + CPRS.ApprovalStatus + " Payment Request made by " + _presenter.CurrentCashPaymentRequest.AppUser.FullName);
                    }
                    else
                    {
                        _presenter.CurrentCashPaymentRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        _presenter.CurrentCashPaymentRequest.CurrentStatus = ApprovalStatus.Rejected.ToString();
                        CPRS.Approver = _presenter.CurrentUser().Id;
                        SendEmailRejected(CPRS);
                        Log.Info(_presenter.GetUser(CPRS.Approver).FullName + " has " + CPRS.ApprovalStatus + " Payment Request made by " + _presenter.CurrentCashPaymentRequest.AppUser.FullName);
                    }
                    break;
                }

            }
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
                    if (attachment.ItemAccountChecklists[0].ChecklistName == attachmentRow.Cells[2].Text)
                    {
                        attachment.FilePath = "~/CPUploads/" + fileName;
                        fuReciept.PostedFile.SaveAs(Server.MapPath("~/CPUploads/") + fileName);
                    }
                }

                BindAttachments();
                Master.ShowMessage(new AppMessage("Successfully uploaded the attachment", RMessageType.Info));

            }
            else
            {
                Master.ShowMessage(new AppMessage("Please select file ", RMessageType.Error));
            }
            pnlReimbursement_ModalPopupExtender.Show();
        }
        protected void DownloadFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            Response.End();
            ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
        }
        protected void DownloadFile2(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            Response.End();
            pnlReimbursement_ModalPopupExtender.Show();
        }
        protected void DeleteFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            //_presenter.CurrentCashPaymentRequest.RemoveCPAttachment(filePath);
            File.Delete(Server.MapPath(filePath));
            BindAttachments();
            pnlReimbursement_ModalPopupExtender.Show();
        }
        protected void grvCashPaymentRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Button btnStatus = e.Row.FindControl("btnStatus") as Button;
            CashPaymentRequest CPR = e.Row.DataItem as CashPaymentRequest;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (CPR != null)
                {
                    if (CPR.ProgressStatus == ProgressStatus.InProgress.ToString())
                    {
                        btnStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFF6C");
                    }
                    else if (CPR.CashPaymentRequestStatuses.Last().PaymentType == "Bank Payment" && !IsCashPaymentRequested(CPR.Id) && CPR.CurrentStatus != ApprovalStatus.Rejected.ToString())
                    {
                        btnStatus.BackColor = System.Drawing.Color.Green;
                    }
                    else if (CPR.ProgressStatus == ProgressStatus.Completed.ToString())
                    {
                        btnStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF7251");
                    }

                    if (ddlSrchProgressStatus.SelectedValue == "Reviewed")
                    {
                        e.Row.Cells[8].Enabled = false;
                    }
                }
            }
        }
        protected void grvCashPaymentRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentCashPaymentRequest.ProgressStatus == ProgressStatus.Completed.ToString())
            {
                PrintTransaction();
            }
            Session["PaymentId"] = _presenter.CurrentCashPaymentRequest.Id;
            btnApprove.Enabled = true;
            BindAccounts();
            if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses.Count != _presenter.CurrentCashPaymentRequest.CurrentLevel)
                PopApprovalStatus();
            else
                ddlApprovalStatus.Items.Clear();
            BindCashPaymentRequestStatus();
            txtRejectedReason.Visible = false;
            rfvRejectedReason.Enabled = false;
            ScriptManager.RegisterStartupScript(this, GetType(), "showApprovalModal", "showApprovalModal();", true);
        }
        protected void grvCashPaymentRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvCashPaymentRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void grvCashPaymentRequestList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                reqID = (int)grvCashPaymentRequestList.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;
                Session["ReqID"] = reqID;
                _presenter.CurrentCashPaymentRequest = _presenter.GetCashPaymentRequest(reqID);
                if (e.CommandName == "ViewItem")
                {
                    dgCashPaymentRequestDetail.DataSource = _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails;
                    dgCashPaymentRequestDetail.DataBind();
                    grvPaymentRequestStatuses.DataSource = _presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses;
                    grvPaymentRequestStatuses.DataBind();
                    BindAttachments();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
                }
                else if (e.CommandName == "Retire")
                {
                    lblEstimatedAmountresult.Text = _presenter.CurrentCashPaymentRequest.TotalAmount.ToString();
                    txtActualExpenditure.Text = _presenter.CurrentCashPaymentRequest.TotalActualExpendture != 0 ? _presenter.CurrentCashPaymentRequest.TotalActualExpendture.ToString() : "";
                    BindAttachments();
                    grvReimbursementdetail.DataSource = _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails;
                    grvReimbursementdetail.DataBind();
                    GetActualAmount();
                    pnlReimbursement_ModalPopupExtender.Show();
                    if (_presenter.CurrentCashPaymentRequest.PaymentReimbursementStatus == "Retired")
                    {
                        btnPrintReimburse.Enabled = true;
                    }
                }
            }
        }
        protected void grvPaymentStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses[e.Row.RowIndex].Approver > 0)
                    e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses[e.Row.RowIndex].Approver).FullName;
            }
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindSearchCashPaymentRequestGrid();
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentCashPaymentRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    SaveCashPaymentRequestStatus();
                    _presenter.SaveOrUpdateCashPaymentRequest(_presenter.CurrentCashPaymentRequest);
                    ShowPrint();
                    if (ddlApprovalStatus.SelectedValue != "Rejected")
                    {
                        Master.ShowMessage(new AppMessage("Payment Approval Processed", RMessageType.Info));
                    }
                    else
                    {
                        Master.ShowMessage(new AppMessage("Payment Approval Rejected", RMessageType.Info));
                    }

                    btnApprove.Enabled = false;
                    BindSearchCashPaymentRequestGrid();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showApprovalModal", "showApprovalModal();", true);
                }
                PrintTransaction();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        private void PrintTransaction()
        {

            lblRequesterResult.Text = _presenter.CurrentCashPaymentRequest.AppUser.FullName;
            lblRequestedDateResult.Text = _presenter.CurrentCashPaymentRequest.RequestDate.Value.ToShortDateString();
            if (_presenter.CurrentCashPaymentRequest.Supplier != null)
            {
                lblSupplierRes.Text = _presenter.CurrentCashPaymentRequest.Supplier.SupplierName.ToString() != null ? _presenter.CurrentCashPaymentRequest.Supplier.SupplierName.ToString() : "";
            }
            lblPayeeResult.Text = _presenter.CurrentCashPaymentRequest.Payee;
            lblVoucherNoResult.Text = _presenter.CurrentCashPaymentRequest.VoucherNo;
            lblTotalAmountResult.Text = _presenter.CurrentCashPaymentRequest.TotalAmount.ToString();
            lblApprovalStatusResult.Text = _presenter.CurrentCashPaymentRequest.ProgressStatus.ToString();
            lblDescResult.Text = _presenter.CurrentCashPaymentRequest.Description;
            lblDepTimeResult.Text = _presenter.CurrentCashPaymentRequest.ArrivalDateTime;
            lblRetTimeResult.Text = _presenter.CurrentCashPaymentRequest.ReturnDateTime;
            grvDetails.DataSource = _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails;
            grvDetails.DataBind();

            grvStatuses.DataSource = _presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses;
            grvStatuses.DataBind();
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses[e.Row.RowIndex].Approver != 0)
                        e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses[e.Row.RowIndex].Approver).FullName;
                }
            }
        }
        protected void ddlApprovalStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlApprovalStatus.SelectedValue == "Rejected")
            {
                lblRejectedReason.Visible = true;
                txtRejectedReason.Visible = true;
                rfvRejectedReason.Enabled = true;
            }
            else
            {
                lblRejectedReason.Visible = false;
                txtRejectedReason.Visible = false;
                rfvRejectedReason.Enabled = false;
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "showApprovalModal", "showApprovalModal();", true);
        }
        protected void dgCashPaymentRequestDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
            }
            else
            {
                if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails != null)
                {
                    //For the last level (usually for the accountant) only allow editing of the accounts 
                    if ((_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses.Count == _presenter.CurrentCashPaymentRequest.CurrentLevel) && (_presenter.CurrentUser().Id == _presenter.CurrentCashPaymentRequest.CurrentApprover))
                    {
                        TextBox txtEdtAmount = e.Item.FindControl("txtEdtAmount") as TextBox;
                        if (txtEdtAmount != null)
                            txtEdtAmount.ReadOnly = true;
                        DropDownList ddlEditProgram = e.Item.FindControl("ddlEdtProgram") as DropDownList;
                        if (ddlEditProgram != null)
                            ddlEditProgram.Enabled = false;
                        DropDownList ddlEditProject = e.Item.FindControl("ddlEdtProject") as DropDownList;
                        if (ddlEditProject != null)
                            ddlEditProject.Enabled = false;
                        DropDownList ddlEditGrant = e.Item.FindControl("ddlEdtGrant") as DropDownList;
                        if (ddlEditGrant != null)
                            ddlEditGrant.Enabled = false;
                    }

                    DropDownList ddlProgram = e.Item.FindControl("ddlEdtProgram") as DropDownList;
                    if (ddlProgram != null)
                    {
                        BindProgram(ddlProgram);
                        if (_presenter.CurrentCashPaymentRequest.Program != null)
                        {
                            ListItem liI = ddlProgram.Items.FindByValue(_presenter.CurrentCashPaymentRequest.Program.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }
                    }

                    DropDownList ddlProject = e.Item.FindControl("ddlEdtProject") as DropDownList;
                    if (ddlProject != null)
                    {
                        BindProject(ddlProject, Convert.ToInt32(ddlProgram.SelectedValue));
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
                    DropDownList ddlEdtGrant = e.Item.FindControl("ddlEdtGrant") as DropDownList;
                    if (ddlEdtGrant != null)
                    {
                        BindGrant(ddlEdtGrant, Convert.ToInt32(ddlProject.SelectedValue));
                        if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].Grant.Id != 0)
                        {
                            ListItem liI = ddlEdtGrant.Items.FindByValue(_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].Grant.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }

                    }
                }
            }
        }
        protected void dgCashPaymentRequestDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgCashPaymentRequestDetail.EditItemIndex = e.Item.ItemIndex;
            dgCashPaymentRequestDetail.DataSource = _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails;
            dgCashPaymentRequestDetail.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
        }
        protected void dgCashPaymentRequestDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            decimal previousAmount = 0;
            int CPRDId = (int)dgCashPaymentRequestDetail.DataKeys[e.Item.ItemIndex];
            CashPaymentRequestDetail cprd;

            if (CPRDId > 0)
                cprd = _presenter.CurrentCashPaymentRequest.GetCashPaymentRequestDetail(CPRDId);
            else
                cprd = (CashPaymentRequestDetail)_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.ItemIndex];

            try
            {
                cprd.CashPaymentRequest = _presenter.CurrentCashPaymentRequest;
                TextBox txtEdtAccountCode = e.Item.FindControl("txtEdtAccountCode") as TextBox;
                cprd.AccountCode = txtEdtAccountCode.Text;
                DropDownList ddlAccountDescription = e.Item.FindControl("ddlEdtAccountDescription") as DropDownList;
                cprd.ItemAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlAccountDescription.SelectedValue));
                TextBox txtEdtAmount = e.Item.FindControl("txtEdtAmount") as TextBox;
                previousAmount = cprd.Amount; //This is the Total Amount of this request before any edit 
                cprd.Amount = Convert.ToDecimal(txtEdtAmount.Text);
                _presenter.CurrentCashPaymentRequest.TotalAmount -= previousAmount; //Subtract the previous Total amount
                _presenter.CurrentCashPaymentRequest.TotalAmount += cprd.Amount; //Then add the new individual amounts to the Total amount
                DropDownList ddlProject = e.Item.FindControl("ddlEdtProject") as DropDownList;
                cprd.Project = _presenter.GetProject(Convert.ToInt32(ddlProject.SelectedValue));
                DropDownList ddlGrant = e.Item.FindControl("ddlEdtGrant") as DropDownList;
                cprd.Grant = _presenter.GetGrant(int.Parse(ddlGrant.SelectedValue));

                dgCashPaymentRequestDetail.EditItemIndex = -1;
                dgCashPaymentRequestDetail.DataSource = _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails;
                dgCashPaymentRequestDetail.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
                Master.ShowMessage(new AppMessage("Payment Detail Successfully Updated", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Payment Detail. " + ex.Message, RMessageType.Error));
            }
        }
        protected void ddlAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopApprovalStatus();
            ScriptManager.RegisterStartupScript(this, GetType(), "showApprovalModal", "showApprovalModal();", true);
        }
        protected void ddlEdtAccountDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            TextBox txtAccountCode = ddl.FindControl("txtEdtAccountCode") as TextBox;
            txtAccountCode.Text = _presenter.GetItemAccount(Convert.ToInt32(ddl.SelectedValue)).AccountCode;
            ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
        }
        protected void ddlEdtProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlEdtGrant = ddl.FindControl("ddlEdtGrant") as DropDownList;
            BindGrant(ddlEdtGrant, Convert.ToInt32(ddl.SelectedValue));
            ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
        }
        protected void ddlEdtProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlEdtProject = ddl.FindControl("ddlEdtProject") as DropDownList;
            BindProject(ddlEdtProject, Convert.ToInt32(ddl.SelectedValue));
            ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Default.aspx");
        }
        protected void btnBankPayment_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("../Request/frmOperationalControlRequest.aspx?paymentId={0}&Page={1}", Convert.ToInt32(Session["PaymentId"]), "CashPayment"));
        }
        protected void btnReimburse_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[0].CPRAttachments.Count != 0)
                {
                    _presenter.CurrentCashPaymentRequest.TotalActualExpendture = Convert.ToDecimal(txtActualExpenditure.Text);
                    _presenter.CurrentCashPaymentRequest.PaymentReimbursementStatus = "Retired";
                    _presenter.SaveOrUpdateCashPaymentRequest(_presenter.CurrentCashPaymentRequest);
                    btnPrintReimburse.Enabled = true;
                    Master.ShowMessage(new AppMessage("Payment Retired Successfully", RMessageType.Info));
                    BindSearchCashPaymentRequestGrid();
                    //btnReimburse.Enabled = false;
                    pnlReimbursement_ModalPopupExtender.Show();
                    Session["ReqID"] = null;
                }
                else
                {
                    Master.ShowMessage(new AppMessage("Error,Please attach Receipt", RMessageType.Error));
                    pnlReimbursement_ModalPopupExtender.Show();
                }

                PrintTransaction();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error,'" + ex.Message + "'", RMessageType.Error));
            }

        }
        private void GetActualAmount()
        {
            foreach (CashPaymentRequestDetail detail in _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails)
            {
                if (detail.ActualExpendture > 0)
                {
                    txtActualExpenditure.Text = (txtActualExpenditure.Text != "" ? Convert.ToDecimal(txtActualExpenditure.Text) : 0 + detail.ActualExpendture).ToString();
                }
            }
        }
        protected void grvReimbursementdetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void grvReimbursementdetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.grvReimbursementdetail.EditItemIndex = e.Item.ItemIndex;
            grvReimbursementdetail.DataSource = _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails;
            grvReimbursementdetail.DataBind();
            pnlReimbursement_ModalPopupExtender.Show();
        }
        protected void grvReimbursementdetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            decimal currentActualExpendture = 0;
            int CPRDId = (int)grvReimbursementdetail.DataKeys[e.Item.ItemIndex];
            CashPaymentRequestDetail cprd;

            if (CPRDId > 0)
                cprd = _presenter.CurrentCashPaymentRequest.GetCashPaymentRequestDetail(CPRDId);
            else
                cprd = (CashPaymentRequestDetail)_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.ItemIndex];

            try
            {
                cprd.CashPaymentRequest = _presenter.CurrentCashPaymentRequest;
                TextBox txtEdtActualExpendture = e.Item.FindControl("txtEdtActualExpendture") as TextBox;
                currentActualExpendture = cprd.ActualExpendture;
                cprd.ActualExpendture = Convert.ToDecimal(txtEdtActualExpendture.Text);
                txtActualExpenditure.Text = (((txtActualExpenditure.Text != "" && txtActualExpenditure.Text != "0") ? Convert.ToDecimal(txtActualExpenditure.Text) : 0) - currentActualExpendture).ToString();
                txtActualExpenditure.Text = (((txtActualExpenditure.Text != "" && txtActualExpenditure.Text != "0") ? Convert.ToDecimal(txtActualExpenditure.Text) : 0) + cprd.ActualExpendture).ToString();
                _presenter.CurrentCashPaymentRequest.TotalActualExpendture = Convert.ToDecimal(txtActualExpenditure.Text);
                _presenter.SaveOrUpdateCashPaymentRequest(_presenter.CurrentCashPaymentRequest);
                grvReimbursementdetail.EditItemIndex = -1;
                grvReimbursementdetail.DataSource = _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails;
                grvReimbursementdetail.DataBind();
                Master.ShowMessage(new AppMessage("Payment Detail Successfully Updated", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Payment Detail. " + ex.Message, RMessageType.Error));
            }
            pnlReimbursement_ModalPopupExtender.Show();
        }

    }
}