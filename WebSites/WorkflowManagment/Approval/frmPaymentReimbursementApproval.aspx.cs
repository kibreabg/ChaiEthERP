using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Modules.Approval.Views;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Shared.MailSender;
using Microsoft.Practices.ObjectBuilder;
using System.IO;
using log4net;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public partial class frmPaymentReimbursementApproval : POCBasePage, IPaymentReimbursementApprovalView
    {
        private PaymentReimbursementApprovalPresenter _presenter;
        private int reqID = 0;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                PopProgressStatus();
            }
            this._presenter.OnViewLoaded();
            BindSearchPaymentReimbursementRequestGrid();

            if (_presenter.CurrentPaymentReimbursementRequest.Id != 0)
            {
                PrintTransaction();
            }
        }
        [CreateNew]
        public PaymentReimbursementApprovalPresenter Presenter
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
                return "{A1C33CA4-8F4D-477F-82D2-15A7B689B697}";
            }
        }
        #region Field Getters
        public int GetPaymentReimbursementRequestId
        {
            get
            {
                if (grvPaymentReimbursementRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvPaymentReimbursementRequestList.SelectedDataKey.Value);
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion
        private void PopProgressStatus()
        {
            string[] s = Enum.GetNames(typeof(ProgressStatus));

            for (int i = 0; i < s.Length; i++)
            {
                ddlSrchProgressStatus.Items.Add(new ListItem(s[i].Replace('_', ' '), s[i].Replace('_', ' ')));
                ddlSrchProgressStatus.DataBind();
            }
        }
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
            //if (_presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestStatuses.Count == _presenter.CurrentPaymentReimbursementRequest.CurrentLevel)
            //{
            //    ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Bank_Payment.ToString().Replace('_', ' '), ApprovalStatus.Bank_Payment.ToString().Replace('_', ' ')));
            //}
            ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Rejected.ToString().Replace('_', ' '), ApprovalStatus.Rejected.ToString().Replace('_', ' ')));

        }
        private string GetWillStatus()
        {
            ApprovalSetting AS = null;
            AS = _presenter.GetApprovalSettingforProcess(RequestType.PaymentReimbursement_Request.ToString().Replace('_', ' ').ToString(), _presenter.CurrentPaymentReimbursementRequest.TotalAmount);


            string will = "";
            foreach (ApprovalLevel AL in AS.ApprovalLevels)
            {
                if ((AL.EmployeePosition.PositionName == "Superviser/Line Manager" || AL.EmployeePosition.PositionName == "Program Manager"))
                {
                    will = "Approve";
                    //break;
                }
                /*else if (_presenter.GetUser(_presenter.CurrentCashPaymentRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName)
                {
                    will = AL.Will;
                }*/
                else
                {
                    try
                    {
                        if (_presenter.GetUser(_presenter.CurrentPaymentReimbursementRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName && AL.WorkflowLevel == _presenter.CurrentPaymentReimbursementRequest.CurrentLevel)
                        {
                            will = AL.Will;
                            break;
                        }
                    }
                    catch
                    {
                        if (_presenter.CurrentPaymentReimbursementRequest.CurrentApproverPosition == AL.EmployeePosition.Id && AL.WorkflowLevel == _presenter.CurrentPaymentReimbursementRequest.CurrentLevel)
                        {
                            will = AL.Will;
                            break;
                        }
                    }
                }

            }
            return will;
        }
        private void BindSearchPaymentReimbursementRequestGrid()
        {
            grvPaymentReimbursementRequestList.DataSource = _presenter.ListPaymentReimbursementRequests(txtSrchRequestDate.Text, ddlSrchProgressStatus.SelectedValue);
            grvPaymentReimbursementRequestList.DataBind();
        }
        private void BindPaymentReimbursementRequestStatus()
        {
            // PaymentReimbursementApprovalPresenter _presenterm = new   PaymentReimbursementApprovalPresenter;
            foreach (PaymentReimbursementRequestStatus PRRS in _presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestStatuses)
            {
                if (PRRS.WorkflowLevel == _presenter.CurrentPaymentReimbursementRequest.CurrentLevel && _presenter.CurrentPaymentReimbursementRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    btnApprove.Enabled = true;
                }
                else
                // btnApprove.Enabled = false;
                if (PRRS.ApprovalStatus != null)
                {
                    btnPrint.Enabled = true;
                }
            }
        }
        private void ShowPrint()
        {
            btnPrint.Enabled = true;
            if (ddlApprovalStatus.SelectedValue != ApprovalStatus.Rejected.ToString())
                SendEmailToRequester();
        }
        private void SendEmail(PaymentReimbursementRequestStatus PRRS)
        {
            if (PRRS.Approver != 0)
            {
                if (_presenter.GetUser(PRRS.Approver).IsAssignedJob != true)
                {
                    EmailSender.Send(_presenter.GetUser(PRRS.Approver).Email, "Settlement Approval", (_presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.AppUser.FullName).ToUpper() + " Settlement for Payment with Request No. " + (_presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.RequestNo).ToUpper());
                }
                else
                {
                    EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(PRRS.Approver).AssignedTo).Email, "Settlement Approval", (_presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.AppUser.FullName).ToUpper() + " Settlement for Payment with Request No. " + (_presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.RequestNo).ToUpper());
                }
            }
            else
            {
                foreach (AppUser Payer in _presenter.GetAppUsersByEmployeePosition(PRRS.ApproverPosition))
                {
                    if (Payer.IsAssignedJob != true)
                    {
                        EmailSender.Send(Payer.Email, "Settlement Approval", (_presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.AppUser.FullName).ToUpper() + " Settlement for Payment with Request No. " + (_presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.RequestNo).ToUpper());
                    }
                    else
                    {
                        EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(Payer.Id).AssignedTo).Email, "Settlement Approval", (_presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.AppUser.FullName).ToUpper() + " Settlement for Payment with Request No. '" + (_presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.RequestNo).ToUpper());
                    }
                }
            }
        }
        private void SavePaymentReimbursementRequestStatus()
        {
            foreach (PaymentReimbursementRequestStatus PRRS in _presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestStatuses)
            {
                if ((PRRS.Approver == _presenter.CurrentUser().Id || (PRRS.ApproverPosition == _presenter.CurrentUser().EmployeePosition.Id) || _presenter.CurrentUser().Id == (_presenter.GetAssignedJobbycurrentuser(PRRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(PRRS.Approver).AssignedTo : 0)) && PRRS.WorkflowLevel == _presenter.CurrentPaymentReimbursementRequest.CurrentLevel)
                {
                    PRRS.ApprovalStatus = ddlApprovalStatus.SelectedValue;
                    PRRS.RejectedReason = txtRejectedReason.Text;
                    PRRS.AssignedBy = _presenter.GetAssignedJobbycurrentuser(PRRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(PRRS.Approver).AppUser.FullName : "";
                    PRRS.Date = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                    if (PRRS.ApprovalStatus != ApprovalStatus.Rejected.ToString())
                    {
                        if (_presenter.CurrentPaymentReimbursementRequest.CurrentLevel == _presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestStatuses.Count)
                        {
                            _presenter.CurrentPaymentReimbursementRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                            _presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.PaymentReimbursementStatus = "Finished";

                        }


                        GetNextApprover();
                        PRRS.Approver = _presenter.CurrentUser().Id;
                        _presenter.CurrentPaymentReimbursementRequest.CurrentStatus = PRRS.ApprovalStatus;
                        _presenter.SaveOrUpdatePaymentReimbursementRequest(_presenter.CurrentPaymentReimbursementRequest);
                        Log.Info(_presenter.GetUser(PRRS.Approver).FullName + " has " + PRRS.ApprovalStatus + " Settlement Request made by " + _presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.AppUser.FullName);
                    }
                    else
                    {
                        SendEmailRejected(PRRS);
                        DeleteSettlementIfRejected();
                        Log.Info(_presenter.GetUser(PRRS.Approver).FullName + " has " + PRRS.ApprovalStatus + " Settlement Request made by " + _presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.AppUser.FullName);
                    }
                    break;
                }

            }
        }
        private void SendEmailRejected(PaymentReimbursementRequestStatus CPRS)
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.AppUser.Id).Email, "Settlement Request Rejection", "Your Settlement Request with Payment Request No. " + (_presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.RequestNo).ToUpper() +  " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (CPRS.RejectedReason).ToUpper() + ". Please Re-Settle'");

            if (CPRS.WorkflowLevel > 1)
            {
                for (int i = 0; i + 1 < CPRS.WorkflowLevel; i++)
                {
                    EmailSender.Send(_presenter.GetUser(_presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestStatuses[i].Approver).Email, "Settlement Request Rejection", "Settlement Request with Payment Request No. " + (_presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.RequestNo).ToUpper() + " made by " + (_presenter.GetUser(_presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.AppUser.Id).FullName).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (CPRS.RejectedReason).ToUpper() + ". Please Re-Settle'");
                }
            }
        }
        private void SendEmailToRequester()
        {
           // if (_presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementStatus != "Bank Payment")
                EmailSender.Send(_presenter.GetUser(_presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.AppUser.Id).Email, "Settlement", "Your Settlement Request for Cash Payment - '" + (_presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.RequestNo).ToUpper() + "' was Completed");
        }
        private void GetNextApprover()
        {
            foreach (PaymentReimbursementRequestStatus PRRS in _presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestStatuses)
            {
                if (PRRS.ApprovalStatus == null)
                {
                    if (PRRS.Approver == 0)
                    {
                        //This is to handle multiple Finance Officers responding to this request
                        //SendEmailToFinanceOfficers;
                        _presenter.CurrentPaymentReimbursementRequest.CurrentApproverPosition = PRRS.ApproverPosition;
                    }
                    else
                    {
                        _presenter.CurrentPaymentReimbursementRequest.CurrentApproverPosition = 0;
                    }
                    SendEmail(PRRS);
                    _presenter.CurrentPaymentReimbursementRequest.CurrentApprover = PRRS.Approver;
                    _presenter.CurrentPaymentReimbursementRequest.CurrentLevel = PRRS.WorkflowLevel;
                    _presenter.CurrentPaymentReimbursementRequest.CurrentStatus = PRRS.ApprovalStatus;
                    _presenter.CurrentPaymentReimbursementRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                    break;
                }
            }
        }
        private void BindAttachments()
        {
            List<PRAttachment> attachments = new List<PRAttachment>();
            foreach (PaymentReimbursementRequestDetail detail in _presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestDetails)
            {
                attachments.AddRange(detail.PRAttachments);
                Session["attachments"] = attachments;
            }

            grvAttachments.DataSource = attachments;
            grvAttachments.DataBind();
        }
        protected void grvPaymentReimbursementRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //grvPaymentReimbursementRequestList.SelectedDataKey.Value
            _presenter.OnViewLoaded();
            PopApprovalStatus();
            BindAttachments();
            BindPaymentReimbursementRequestStatus();
            txtRejectedReason.Visible = false;
            rfvRejectedReason.Enabled = false;
            pnlApproval_ModalPopupExtender.Show();

        }
        protected void grvPaymentReimbursementRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Button btnStatus = e.Row.FindControl("btnStatus") as Button;
            PaymentReimbursementRequest CSR = e.Row.DataItem as PaymentReimbursementRequest;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (CSR != null)
                {
                   if (CSR.ProgressStatus == ProgressStatus.InProgress.ToString())
                    {
                        btnStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFF6C");

                    }
                    else if (CSR.ProgressStatus == ProgressStatus.Completed.ToString())
                    {
                        btnStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF7251");

                    }
                }
            }
        }
        protected void grvPaymentReimbursementRequestList_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewItem")
            {
                reqID = (int)grvPaymentReimbursementRequestList.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;
                _presenter.CurrentPaymentReimbursementRequest = _presenter.GetPaymentReimbursementRequest(reqID);
                //_presenter.OnViewLoaded();
                dgReimbursementDetail.DataSource = _presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestDetails;
                dgReimbursementDetail.DataBind();
                pnlDetail.Visible = true;
            }
        }
        protected void grvPaymentReimbursementRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvPaymentReimbursementRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
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
            pnlApproval_ModalPopupExtender.Show();
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindSearchPaymentReimbursementRequestGrid();
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentPaymentReimbursementRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    SavePaymentReimbursementRequestStatus();

                    ShowPrint();
                    btnApprove.Enabled = false;
                    BindSearchPaymentReimbursementRequestGrid();
                    pnlApproval_ModalPopupExtender.Show();
                }
                Master.ShowMessage(new AppMessage("Payment Settlement Approval Processed", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            pnlApproval_ModalPopupExtender.Hide();
        }
        private void PrintTransaction()
        {
            pnlApproval_ModalPopupExtender.Hide();
            if(_presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest != null)
                lblRequestNoResult.Text =  _presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.RequestNo.ToString() ;
            lblRequestedDateResult.Text = _presenter.CurrentPaymentReimbursementRequest.RequestDate.Value.ToShortDateString();
            if (_presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest != null)
                lblRequesterResult.Text = _presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.AppUser.UserName;
            lblCommentResult.Text = _presenter.CurrentPaymentReimbursementRequest.Comment.ToString();
            lblApprovalStatusResult.Text = _presenter.CurrentPaymentReimbursementRequest.ProgressStatus.ToString();
            lbladvancetakenresult.Text = _presenter.CurrentPaymentReimbursementRequest.ReceivableAmount.ToString();
            lblActualExpenditureresult.Text = _presenter.CurrentPaymentReimbursementRequest.TotalAmount.ToString();
            grvDetails.DataSource = _presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestDetails;
            grvDetails.DataBind();

            grvStatuses.DataSource = _presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestStatuses;
            grvStatuses.DataBind();
        }
        protected void DownloadFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            Response.End();

            pnlApproval_ModalPopupExtender.Show();
        }
        protected void btnCancelPopup2_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestStatuses[e.Row.RowIndex].Approver != 0)
                        e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestStatuses[e.Row.RowIndex].Approver).FullName;
                }
            }
        }

        private void DeleteSettlementIfRejected()
        {
            _presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.IsLiquidated = false;
            _presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.SettlementRejectionCount = _presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.SettlementRejectionCount + 1;
            _presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.SettlementRejectionReasons = _presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.SettlementRejectionReasons + "," + txtRejectedReason.Text;
            _presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.SettlementRejetcedby = _presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.SettlementRejetcedby + "," + _presenter.CurrentUser().Id;
            IList<PaymentReimbursementRequestDetail> detailList = _presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestDetails.ToList();
            foreach (PaymentReimbursementRequestDetail detail in detailList)
            {
                if (detail.PRAttachments != null)
                {
                    IList<PRAttachment> attachemntList = detail.PRAttachments.ToList();
                    foreach (PRAttachment attach in attachemntList)
                    {
                        string filePath = HttpContext.Current.Server.MapPath(attach.FilePath);
                        Response.Flush();
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                       // Application.CompleteRequest()
                        detail.RemovePRAttachment(attach.Id);
                        //detail.PRAttachments.Remove(attach);
                    }

                }

                _presenter.CurrentPaymentReimbursementRequest.RemovePaymentReimbursementRequestDetail(detail.Id);
            }

            _presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestStatuses.Clear();
            PaymentReimbursementRequest request = _presenter.CurrentPaymentReimbursementRequest;
            _presenter.SaveOrUpdatePaymentReimbursementRequest(_presenter.CurrentPaymentReimbursementRequest);
           
            _presenter.DeletePaymentReimbursementRequest(request);
        }
    }
}