﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using log4net;
using log4net.Config;
using Microsoft.Practices.ObjectBuilder;
using System.Data;
using OfficeOpenXml;
using System.IO;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public partial class frmExpenseLiquidationApproval : POCBasePage, IExpenseLiquidationApprovalView
    {
        private ExpenseLiquidationApprovalPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        private int liqID = 0;
        decimal _totalUnitPrice = 0;
        decimal _totalAmountAdvanced = 0;
        decimal _totalVariance = 0;
        decimal _totalActualExpenditure = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                PopProgressStatus();
                PopRequesters(ddlSrchRequester);
            }
            this._presenter.OnViewLoaded();
            BindSearchExpenseLiquidationRequestGrid();
            if (_presenter.CurrentExpenseLiquidationRequest != null)
            {
                if (_presenter.CurrentExpenseLiquidationRequest.ProgressStatus == ProgressStatus.Completed.ToString())
                {
                    PrintTransaction();
                }
            }
        }
        [CreateNew]
        public ExpenseLiquidationApprovalPresenter Presenter
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
                return "{136836C4-1353-4DEF-A912-BF65AA84497C}";
            }
        }

        #region Field Getters
        public int GetExpenseLiquidationRequestId
        {
            get
            {
                if (grvExpenseLiquidationRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvExpenseLiquidationRequestList.SelectedDataKey.Value);
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
        #endregion
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
        private void PopRequesters(DropDownList ddl)
        {
            ddl.DataSource = _presenter.GetEmployeeList();
            ddl.DataTextField = "FullName";
            ddl.DataValueField = "ID";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem("Select Requester", "0"));
            ddl.SelectedIndex = 0;
        }
        private void PopApprovalStatus()
        {
            ddlApprovalStatus.Items.Clear();
            ddlApprovalStatus.Items.Add(new ListItem("Select Status", "0"));
            string[] s = Enum.GetNames(typeof(ApprovalStatus));

            for (int i = 0; i < s.Length; i++)
            {
                if (GetWillStatus().Substring(0, 3) == s[i].Substring(0, 3))
                {
                    ddlApprovalStatus.Items.Add(new ListItem(s[i].Replace('_', ' '), s[i].Replace('_', ' ')));
                }
            }
            if (_presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses.Count == _presenter.CurrentExpenseLiquidationRequest.CurrentLevel)
            {
                ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Bank_Payment.ToString().Replace('_', ' '), ApprovalStatus.Bank_Payment.ToString().Replace('_', ' ')));
            }
            ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Rejected.ToString().Replace('_', ' '), ApprovalStatus.Rejected.ToString().Replace('_', ' ')));

        }
        private string GetWillStatus()
        {
            ApprovalSetting AS = _presenter.GetApprovalSettingforProcess(RequestType.ExpenseLiquidation_Request.ToString().Replace('_', ' ').ToString(), 0);
            string will = "";
            foreach (ApprovalLevel AL in AS.ApprovalLevels)
            {
                if (AL.EmployeePosition.PositionName == "Superviser/Line Manager" || AL.EmployeePosition.PositionName == "Program Manager" && AL.WorkflowLevel == _presenter.CurrentExpenseLiquidationRequest.CurrentLevel)
                {
                    will = "Approve";
                    break;
                }
                /*else if (_presenter.GetUser(_presenter.CurrentExpenseLiquidationRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName)
                {
                    will = AL.Will;
                }*/
                else
                {
                    try
                    {
                        if (_presenter.GetUser(_presenter.CurrentExpenseLiquidationRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName && AL.WorkflowLevel == _presenter.CurrentExpenseLiquidationRequest.CurrentLevel)
                        {
                            will = AL.Will;
                            break;
                        }
                    }
                    catch
                    {
                        if (_presenter.CurrentExpenseLiquidationRequest.CurrentApproverPosition == AL.EmployeePosition.Id && AL.WorkflowLevel == _presenter.CurrentExpenseLiquidationRequest.CurrentLevel)
                        {
                            will = AL.Will;
                            break;
                        }
                    }
                }

            }
            return will;
        }
        private void BindSearchExpenseLiquidationRequestGrid()
        {
            grvExpenseLiquidationRequestList.DataSource = _presenter.ListExpenseLiquidationRequests(txtSrchTravelAdvanceNo.Text, txtSrchRequestDate.Text, ddlSrchProgressStatus.SelectedValue, ddlSrchRequester.SelectedValue);
            grvExpenseLiquidationRequestList.DataBind();
        }
        private void BindExpenseLiquidationRequestStatus()
        {
            foreach (ExpenseLiquidationRequestStatus ELRS in _presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses)
            {
                if (ELRS.WorkflowLevel == _presenter.CurrentExpenseLiquidationRequest.CurrentLevel && _presenter.CurrentExpenseLiquidationRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    btnApprove.Enabled = true;
                }

                if (ELRS.WorkflowLevel == _presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses.Count && ELRS.ApprovalStatus != null)
                {
                    btnPrint.Enabled = true;
                    btnApprove.Enabled = false;
                }
                else if (_presenter.CurrentExpenseLiquidationRequest.CurrentStatus == ApprovalStatus.Rejected.ToString())
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

            //Bank Payment should be initiated if CHAI is the one who's going to pay (variance is Positive)
            decimal variance = _presenter.CurrentExpenseLiquidationRequest.TotalActualExpenditure - _presenter.CurrentExpenseLiquidationRequest.TotalTravelAdvance;

            if (_presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses.Last().ApprovalStatus == "Bank Payment" && !IsBankPaymentRequested() && variance > 0 && _presenter.CurrentExpenseLiquidationRequest.CurrentStatus != ApprovalStatus.Rejected.ToString())
                btnBankPayment.Visible = true;
            else
                btnBankPayment.Visible = false;
        }
        private bool IsBankPaymentRequested()
        {
            OperationalControlRequest ocr = _presenter.GetOperationalControlRequestByLiquidationId(_presenter.CurrentExpenseLiquidationRequest.Id);
            if (ocr != null)
                return true;
            else
                return false;
        }
        private bool IsBankPaymentRequested(int liquidationId)
        {
            OperationalControlRequest ocr = _presenter.GetOperationalControlRequestByLiquidationId(liquidationId);
            if (ocr != null)
                return true;
            else
                return false;
        }
        private void BindAttachments()
        {
            List<ELRAttachment> attachments = new List<ELRAttachment>();
            foreach (ExpenseLiquidationRequestDetail detail in _presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestDetails)
            {
                attachments.AddRange(detail.ELRAttachments);
                Session["attachments"] = attachments;
            }

            grvAttachments.DataSource = attachments;
            grvAttachments.DataBind();
        }
        private void ShowPrint()
        {
            if (_presenter.CurrentExpenseLiquidationRequest.CurrentLevel == _presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses.Count && _presenter.CurrentExpenseLiquidationRequest.ProgressStatus == ProgressStatus.Completed.ToString())
            {
                btnPrint.Enabled = true;

                //Bank Payment should be initiated if CHAI is the one who's going to pay (variance is Positive)
                decimal variance = _presenter.CurrentExpenseLiquidationRequest.TotalActualExpenditure - _presenter.CurrentExpenseLiquidationRequest.TotalTravelAdvance;
                if (_presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses.Last().ApprovalStatus == "Bank Payment" && variance > 0 && _presenter.CurrentExpenseLiquidationRequest.CurrentStatus != ApprovalStatus.Rejected.ToString())
                    btnBankPayment.Visible = true;
            }
        }
        private void SendEmail(ExpenseLiquidationRequestStatus ELRS)
        {
            if (ELRS.Approver != 0)
            {
                if (_presenter.GetUser(ELRS.Approver).IsAssignedJob != true)
                {
                    EmailSender.Send(_presenter.GetUser(ELRS.Approver).Email, "Expense Liquidation Request", (_presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.AppUser.FullName).ToUpper() + " Requests for Expense Liquidation to Travel Advance No. '" + (_presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.TravelAdvanceNo).ToUpper() + "'");
                }
                else
                {
                    EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(ELRS.Approver).AssignedTo).Email, "Expense Liquidation Request", (_presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.AppUser.FullName).ToUpper() + " Requests for Expense Liquidation to Travel Advance No. '" + (_presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.TravelAdvanceNo).ToUpper() + "'");
                }
            }
            else
            {
                foreach (AppUser Payer in _presenter.GetAppUsersByEmployeePosition(ELRS.ApproverPosition))
                {
                    if (Payer.IsAssignedJob != true)
                    {
                        //Commented out because Finance team got tired of emails
                        //EmailSender.Send(Payer.Email, "Expense Liquidation Request", (_presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.AppUser.FullName).ToUpper() + " Requests for Expense Liquidation to Travel Advance No. '" + (_presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.TravelAdvanceNo).ToUpper() + "'");
                    }
                    else
                    {
                        //Commented out because Finance team got tired of emails
                        //EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(Payer.Id).AssignedTo).Email, "Expense Liquidation Request", (_presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.AppUser.FullName).ToUpper() + " Requests for Expense Liquidation to Travel Advance No. '" + (_presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.TravelAdvanceNo).ToUpper() + "'");
                    }
                }
            }
        }
        private void SendEmailRejected(ExpenseLiquidationRequestStatus ELRS)
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.AppUser.Id).Email, "Expense Liquidation Request Rejection", "Your Liquidation Request for Travel Advance No. '" + (_presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.TravelAdvanceNo).ToUpper() + "' was Rejected by " + _presenter.CurrentUser().FullName + " for this reason '" + (ELRS.RejectedReason).ToUpper() + "'");

            if (ELRS.WorkflowLevel > 1)
            {
                for (int i = 0; i + 1 < ELRS.WorkflowLevel; i++)
                {
                    EmailSender.Send(_presenter.GetUser(_presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses[i].Approver).Email, "Expense Liquidation Request Rejection", "Expense Liquidation Request made by " + (_presenter.GetUser(_presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.AppUser.Id).FullName).ToUpper() + " for Travel Advance No. '" + (_presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.TravelAdvanceNo).ToUpper() + "' was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (ELRS.RejectedReason).ToUpper() + "'");
                }
            }
        }
        private void SaveExpenseLiquidationRequestStatus()
        {
            foreach (ExpenseLiquidationRequestStatus ELRS in _presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses)
            {
                if ((ELRS.Approver == _presenter.CurrentUser().Id || (ELRS.ApproverPosition == _presenter.CurrentUser().EmployeePosition.Id) || _presenter.CurrentUser().Id == (_presenter.GetAssignedJobbycurrentuser(ELRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(ELRS.Approver).AssignedTo : 0)) && ELRS.WorkflowLevel == _presenter.CurrentExpenseLiquidationRequest.CurrentLevel)
                {
                    ELRS.ApprovalStatus = ddlApprovalStatus.SelectedValue;
                    ELRS.RejectedReason = txtRejectedReason.Text;
                    ELRS.Date = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                    ELRS.AssignedBy = _presenter.GetAssignedJobbycurrentuser(ELRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(ELRS.Approver).AppUser.FullName : "";
                    if (ELRS.ApprovalStatus != ApprovalStatus.Rejected.ToString())
                    {
                        _presenter.CurrentExpenseLiquidationRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        _presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.ExpenseLiquidationStatus = "Finished";
                        GetNextApprover();
                        ELRS.Approver = _presenter.CurrentUser().Id;
                        _presenter.CurrentExpenseLiquidationRequest.CurrentStatus = ELRS.ApprovalStatus;
                        Log.Info(_presenter.GetUser(ELRS.Approver).FullName + " has " + ELRS.ApprovalStatus + " Expense Liquidation Request made by " + _presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.AppUser.FullName);
                    }
                    else
                    {
                        ELRS.Approver = _presenter.CurrentUser().Id;
                        _presenter.CurrentExpenseLiquidationRequest.CurrentStatus = ApprovalStatus.Rejected.ToString();
                        SendEmailRejected(ELRS);
                        Log.Info(_presenter.CurrentUser().FullName + " has " + (ELRS.ApprovalStatus).ToUpper() + " Expense Liquidation Request made by " + (_presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.AppUser.FullName).ToUpper());
                        DeleteLiquidationIfRejected();
                    }
                    break;
                }

            }
        }
        private int GetFirstApprover()
        {
            int approver = 0;
            foreach (ExpenseLiquidationRequestStatus ELRS in _presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses)
            {
                approver = ELRS.Approver;
                break;
            }

            return approver;
        }
        private void GetNextApprover()
        {
            foreach (ExpenseLiquidationRequestStatus ELRS in _presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses)
            {
                if (ELRS.ApprovalStatus == null)
                {
                    if (ELRS.Approver == 0)
                    {
                        //This is to handle multiple Finance Officers responding to this request
                        //SendEmailToFinanceOfficers;
                        _presenter.CurrentExpenseLiquidationRequest.CurrentApproverPosition = ELRS.ApproverPosition;
                    }
                    else
                    {
                        _presenter.CurrentExpenseLiquidationRequest.CurrentApproverPosition = 0;
                    }
                    SendEmail(ELRS);
                    _presenter.CurrentExpenseLiquidationRequest.CurrentApprover = ELRS.Approver;
                    _presenter.CurrentExpenseLiquidationRequest.CurrentLevel = ELRS.WorkflowLevel;
                    _presenter.CurrentExpenseLiquidationRequest.CurrentStatus = ELRS.ApprovalStatus;
                    _presenter.CurrentExpenseLiquidationRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                    break;
                }
            }
        }
        private void DeleteLiquidationIfRejected()
        {
            _presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.ExpenseLiquidationStatus = "Completed";
            _presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.LiquidationRejectionCount = _presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.LiquidationRejectionCount + 1;
            _presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.LiquidationRejectionReasons = _presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.LiquidationRejectionReasons + "," + txtRejectedReason.Text;
            _presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.LiquidationRejectedBy = _presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.LiquidationRejectedBy + "," + _presenter.CurrentUser().Id;
            IList<ExpenseLiquidationRequestDetail> detailList = _presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestDetails.ToList();
            foreach (ExpenseLiquidationRequestDetail detail in detailList)
            {
                if (detail.ELRAttachments != null)
                {
                    IList<ELRAttachment> attachemntList = detail.ELRAttachments.ToList();
                    foreach (ELRAttachment attach in attachemntList)
                    {
                        string filePath = HttpContext.Current.Server.MapPath(attach.FilePath);
                        Response.Flush();
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        detail.RemoveELAttachment(filePath);
                    }
                }
                _presenter.CurrentExpenseLiquidationRequest.RemoveExpenseLiquidationRequestDetail(detail.Id);
            }

            _presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses.Clear();
            ExpenseLiquidationRequest request = _presenter.CurrentExpenseLiquidationRequest;
            _presenter.SaveOrUpdateExpenseLiquidationRequest(_presenter.CurrentExpenseLiquidationRequest);
            _presenter.DeleteExpenseLiquidationRequest(request);
        }
        protected void grvExpenseLiquidationRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentExpenseLiquidationRequest.ProgressStatus == ProgressStatus.Completed.ToString())
            {
                PrintTransaction();
            }
            PopApprovalStatus();
            Session["PaymentId"] = _presenter.CurrentExpenseLiquidationRequest.Id;
            btnApprove.Enabled = true;
            BindExpenseLiquidationRequestStatus();
            BindAttachments();
            ScriptManager.RegisterStartupScript(this, GetType(), "showApprovalModal", "showApprovalModal();", true);

        }
        protected void grvExpenseLiquidationRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Button btnStatus = e.Row.FindControl("btnStatus") as Button;
            ExpenseLiquidationRequest ELR = e.Row.DataItem as ExpenseLiquidationRequest;

            if (ELR != null)
            {
                //Bank Payment should be initiated if CHAI is the one who's going to pay (variance is Positive)
                decimal variance = ELR.TotalActualExpenditure - ELR.TotalTravelAdvance;

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (ELR.ProgressStatus == ProgressStatus.InProgress.ToString())
                    {
                        btnStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFF6C");

                    }
                    else if (ELR.ExpenseLiquidationRequestStatuses.Last().ApprovalStatus == "Bank Payment" && !IsBankPaymentRequested(ELR.Id) && variance > 0 && ELR.CurrentStatus != ApprovalStatus.Rejected.ToString())
                    {
                        btnStatus.BackColor = System.Drawing.Color.Green;
                    }
                    else if (ELR.ProgressStatus == ProgressStatus.Completed.ToString())
                    {
                        btnStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF7251");
                    }
                }
            }
        }
        protected void grvExpenseLiquidationRequestList_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                liqID = (int)grvExpenseLiquidationRequestList.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;
                Session["ReqID"] = liqID;
                _presenter.CurrentExpenseLiquidationRequest = _presenter.GetExpenseLiquidationRequest(liqID);
                if (e.CommandName == "ViewItem")
                {
                    //_presenter.OnViewLoaded();
                    dgLiquidationRequestDetail.DataSource = _presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestDetails;
                    dgLiquidationRequestDetail.DataBind();
                    grvLiquidationRequestStatuses.DataSource = _presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses;
                    grvLiquidationRequestStatuses.DataBind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
                }
            }
        }
        protected void grvLiquidationRequestStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses[e.Row.RowIndex].Approver > 0)
                    e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses[e.Row.RowIndex].Approver).FullName;
            }
        }
        protected void grvExpenseLiquidationRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvExpenseLiquidationRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses[e.Row.RowIndex].Approver != 0)
                        e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses[e.Row.RowIndex].Approver).FullName;
                    if (e.Row.Cells[3].Text == "Pay")
                    {
                        e.Row.Cells[3].Text = "Reviewed";
                    }
                }
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
        protected void grvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotalAmountAdv = (Label)e.Row.FindControl("lblTotalAmountAdv");
                lblTotalAmountAdv.Text = _presenter.CurrentExpenseLiquidationRequest.TotalTravelAdvance.ToString();

                Label lblTotalActualExp = (Label)e.Row.FindControl("lblTotalActualExp");
                lblTotalActualExp.Text = _presenter.CurrentExpenseLiquidationRequest.TotalActualExpenditure.ToString();

                Label lblTotalVariance = (Label)e.Row.FindControl("lblTotalVariance");
                lblTotalVariance.Text = (_presenter.CurrentExpenseLiquidationRequest.TotalTravelAdvance - _presenter.CurrentExpenseLiquidationRequest.TotalActualExpenditure).ToString();
            }
        }
        protected void ddlApprovalStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlApprovalStatus.SelectedValue == "Rejected")
            {
                lblRejectedReason.Visible = true;
                txtRejectedReason.Visible = true;
            }
            else
            {
                lblRejectedReason.Visible = false;
                txtRejectedReason.Visible = false;
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "showApprovalModal", "showApprovalModal();", true);
        }
        protected void ddlEdtAccountDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            TextBox txtAccountCode = ddl.FindControl("txtEdtAccountCode") as TextBox;
            if (_presenter.GetItemAccount(Convert.ToInt32(ddl.SelectedValue)) != null)
                txtAccountCode.Text = _presenter.GetItemAccount(Convert.ToInt32(ddl.SelectedValue)).AccountCode;
            ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindSearchExpenseLiquidationRequestGrid();
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentExpenseLiquidationRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    decimal totalTravelAdvance = _presenter.CurrentExpenseLiquidationRequest.TotalTravelAdvance;
                    decimal totalActualExpenditure = _presenter.CurrentExpenseLiquidationRequest.TotalActualExpenditure;
                    decimal variance = totalTravelAdvance - totalActualExpenditure;

                    if (totalTravelAdvance < totalActualExpenditure && ddlApprovalStatus.SelectedValue == ProgressStatus.Completed.ToString())
                    {
                        Master.ShowMessage(new AppMessage("Please initiate a bank payment for the remaining " + variance.ToString() + " birr to be paid for the requester!", RMessageType.Error));
                    }
                    else
                    {
                        SaveExpenseLiquidationRequestStatus();
                        if (ddlApprovalStatus.SelectedValue != "Rejected")
                            _presenter.SaveOrUpdateExpenseLiquidationRequest(_presenter.CurrentExpenseLiquidationRequest);
                        ShowPrint();
                        if (ddlApprovalStatus.SelectedValue != "Rejected")
                        {
                            Master.ShowMessage(new AppMessage("Expense Liquidation Approval Processed", RMessageType.Info));
                        }
                        else
                        {
                            Master.ShowMessage(new AppMessage("Expense Liquidation Approval Rejected", RMessageType.Info));
                        }
                        btnApprove.Enabled = false;
                        BindSearchExpenseLiquidationRequestGrid();
                        ScriptManager.RegisterStartupScript(this, GetType(), "showApprovalModal", "showApprovalModal();", true); ;
                        PrintTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Approve Expense Liquidation " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void btnBankPayment_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("../Request/frmOperationalControlRequest.aspx?paymentId={0}&Page={1}", Convert.ToInt32(Session["PaymentId"]), "ExpenseLiquidation"));
        }
        private void PrintTransaction()
        {
            TravelAdvanceRequest taRequest = _presenter.GetTravelAdvanceRequest(_presenter.CurrentExpenseLiquidationRequest.Id);
            lblRequestNoResult.Text = taRequest.TravelAdvanceNo;
            lblRequestedDateResult.Text = _presenter.CurrentExpenseLiquidationRequest.RequestDate.Value.ToShortDateString();
            lblRequesterResult.Text = taRequest.AppUser.FullName;
            lblPurposeofAdvanceResult.Text = _presenter.CurrentExpenseLiquidationRequest.Comment.ToString();
            lblApprovalStatusResult.Text = _presenter.CurrentExpenseLiquidationRequest.ProgressStatus.ToString();
            lblArrRetTimeResult.Text = _presenter.CurrentExpenseLiquidationRequest.ArrivalDateTime + " - " + _presenter.CurrentExpenseLiquidationRequest.ReturnDateTime;
            grvDetails.DataSource = _presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestDetails;
            grvDetails.DataBind();

            grvStatuses.DataSource = _presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses;
            grvStatuses.DataBind();
        }
        public string GetMimeTypeByFileName(string sFileName)
        {
            string sMime = "application/octet-stream";

            string sExtension = System.IO.Path.GetExtension(sFileName);
            if (!string.IsNullOrEmpty(sExtension))
            {
                sExtension = sExtension.Replace(".", "");
                sExtension = sExtension.ToLower();

                if (sExtension == "xls" || sExtension == "xlsx")
                {
                    sMime = "application/ms-excel";
                }
                else if (sExtension == "doc" || sExtension == "docx")
                {
                    sMime = "application/msword";
                }
                else if (sExtension == "ppt" || sExtension == "pptx")
                {
                    sMime = "application/ms-powerpoint";
                }
                else if (sExtension == "rtf")
                {
                    sMime = "application/rtf";
                }
                else if (sExtension == "zip")
                {
                    sMime = "application/zip";
                }
                else if (sExtension == "mp3")
                {
                    sMime = "audio/mpeg";
                }
                else if (sExtension == "bmp")
                {
                    sMime = "image/bmp";
                }
                else if (sExtension == "gif")
                {
                    sMime = "image/gif";
                }
                else if (sExtension == "jpg" || sExtension == "jpeg")
                {
                    sMime = "image/jpeg";
                }
                else if (sExtension == "png")
                {
                    sMime = "image/png";
                }
                else if (sExtension == "tiff" || sExtension == "tif")
                {
                    sMime = "image/tiff";
                }
                else if (sExtension == "txt")
                {
                    sMime = "text/plain";
                }
            }

            return sMime;
        }
        private void BindProject(DropDownList ddlProject)
        {
            ddlProject.DataSource = _presenter.GetProjectList();
            ddlProject.DataValueField = "Id";
            ddlProject.DataTextField = "ProjectCode";
            ddlProject.DataBind();
        }
        private void BindAccountDescription(DropDownList ddlAccountDescription)
        {
            ddlAccountDescription.DataSource = _presenter.GetItemAccountList();
            ddlAccountDescription.DataValueField = "Id";
            ddlAccountDescription.DataTextField = "AccountName";
            ddlAccountDescription.DataBind();
        }
        protected void dgLiquidationRequestDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (_presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestDetails != null)
            {
                DropDownList ddlProject = e.Item.FindControl("ddlEdtProject") as DropDownList;
                if (ddlProject != null)
                {
                    BindProject(ddlProject);
                    if (_presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestDetails[e.Item.DataSetIndex].Project != null)
                    {
                        ListItem liI = ddlProject.Items.FindByValue(_presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestDetails[e.Item.DataSetIndex].Project.Id.ToString());
                        if (liI != null)
                            liI.Selected = true;
                    }
                }
                DropDownList ddlAccountDescription = e.Item.FindControl("ddlEdtAccountDescription") as DropDownList;
                if (ddlAccountDescription != null)
                {
                    BindAccountDescription(ddlAccountDescription);
                    if (_presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestDetails[e.Item.DataSetIndex].ItemAccount != null)
                    {
                        ListItem liI = ddlAccountDescription.Items.FindByValue(_presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestDetails[e.Item.DataSetIndex].ItemAccount.Id.ToString());
                        if (liI != null)
                            liI.Selected = true;
                    }
                }

                if (e.Item.ItemType == ListItemType.Footer)
                {
                    _totalVariance = 0;
                    foreach (ExpenseLiquidationRequestDetail detail in _presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestDetails)
                    {
                        _totalVariance = _totalVariance + detail.Variance;
                    }


                    Label lblTotalVariance = e.Item.FindControl("lblTotalVariance") as Label;
                    lblTotalVariance.Text = _totalVariance.ToString();
                    lblTotalVariance.ForeColor = System.Drawing.Color.Green;
                    lblTotalVariance.Font.Bold = true;
                }
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    _totalAmountAdvanced = 0;
                    foreach (ExpenseLiquidationRequestDetail detail in _presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestDetails)
                    {
                        _totalAmountAdvanced = _totalAmountAdvanced + detail.AmountAdvanced;
                    }


                    Label lblTotalAdvAmount = e.Item.FindControl("lblTotalAdvAmount") as Label;
                    lblTotalAdvAmount.Text = _totalAmountAdvanced.ToString();
                    lblTotalAdvAmount.ForeColor = System.Drawing.Color.Green;
                    lblTotalAdvAmount.Font.Bold = true;
                }
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    _totalActualExpenditure = 0;
                    foreach (ExpenseLiquidationRequestDetail detail in _presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestDetails)
                    {
                        _totalActualExpenditure = _totalActualExpenditure + detail.ActualExpenditure;
                    }


                    Label lblTotalActualExp = e.Item.FindControl("lblTotalActualExp") as Label;
                    lblTotalActualExp.Text = _totalActualExpenditure.ToString();
                    lblTotalActualExp.ForeColor = System.Drawing.Color.Green;
                    lblTotalActualExp.Font.Bold = true;
                }
            }

        }
        protected void dgLiquidationRequestDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgLiquidationRequestDetail.EditItemIndex = e.Item.ItemIndex;
            dgLiquidationRequestDetail.DataSource = _presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestDetails;
            dgLiquidationRequestDetail.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
        }
        protected void dgLiquidationRequestDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int CPRDId = (int)dgLiquidationRequestDetail.DataKeys[e.Item.ItemIndex];
            ExpenseLiquidationRequestDetail cprd;

            if (CPRDId > 0)
                cprd = _presenter.CurrentExpenseLiquidationRequest.GetExpenseLiquidationRequestDetail(CPRDId);
            else
                cprd = (ExpenseLiquidationRequestDetail)_presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestDetails[e.Item.ItemIndex];

            try
            {
                cprd.ExpenseLiquidationRequest = _presenter.CurrentExpenseLiquidationRequest;
                DropDownList ddlAccountDescription = e.Item.FindControl("ddlEdtAccountDescription") as DropDownList;
                cprd.ItemAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlAccountDescription.SelectedValue));
                DropDownList ddlProject = e.Item.FindControl("ddlEdtProject") as DropDownList;
                cprd.Project = _presenter.GetProject(Convert.ToInt32(ddlProject.SelectedValue));

                dgLiquidationRequestDetail.EditItemIndex = -1;
                dgLiquidationRequestDetail.DataSource = _presenter.CurrentExpenseLiquidationRequest.ExpenseLiquidationRequestDetails;
                dgLiquidationRequestDetail.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
                Master.ShowMessage(new AppMessage("Expense Liquidation Detail Successfully Updated", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Expense Liquidation Detail. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt1 = new DataTable();

                dt1 = _presenter.ExportTravelAdvance(_presenter.CurrentExpenseLiquidationRequest.Id).Tables[0];

                // mySqlDataAdapter.Fill(dt1);

                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Journal Entry - '" + _presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.TravelAdvanceNo + "'");


                    ws.Cells["A1"].LoadFromDataTable(dt1, true);


                    //Write it back to the client
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=Journal Entry - '" + _presenter.CurrentExpenseLiquidationRequest.TravelAdvanceRequest.TravelAdvanceNo + "'.xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error exporting to Excel. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "showApprovalModal", "showApprovalModal();", true);
        }
    }
}


