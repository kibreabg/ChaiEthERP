using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Modules.Approval.Views;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Shared.MailSender;
using log4net;
using log4net.Config;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public partial class frmTravelAdvanceApproval : POCBasePage, ITravelAdvanceApprovalView
    {
        private TravelAdvanceApprovalPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                PopProgressStatus();
                PopRequesters(ddlSrchRequester);
                BindSearchTravelAdvanceRequestGrid();
            }
            this._presenter.OnViewLoaded();
            if (_presenter.CurrentTravelAdvanceRequest != null)
            {
                if (_presenter.CurrentTravelAdvanceRequest.Id != 0)
                    PrintTransaction();
            }

        }
        [CreateNew]
        public TravelAdvanceApprovalPresenter Presenter
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
                return "{DBC4CB73-69E5-42F3-84F8-09F8EA69B06D}";
            }
        }

        #region Field Getters
        public int GetTravelAdvanceRequestId
        {
            get
            {
                if (grvTravelAdvanceRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvTravelAdvanceRequestList.SelectedDataKey.Value);
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion        
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
            if (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses.Count == _presenter.CurrentTravelAdvanceRequest.CurrentLevel)
            {
                ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Bank_Payment.ToString().Replace('_', ' '), ApprovalStatus.Bank_Payment.ToString().Replace('_', ' ')));
            }
            ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Rejected.ToString().Replace('_', ' '), ApprovalStatus.Rejected.ToString().Replace('_', ' ')));

        }
        private string GetWillStatus()
        {
            ApprovalSetting AS = _presenter.GetApprovalSettingforProcess(RequestType.TravelAdvance_Request.ToString().Replace('_', ' ').ToString(), 0);
            string will = "";
            foreach (ApprovalLevel AL in AS.ApprovalLevels)
            {
                if (AL.EmployeePosition.PositionName == "Superviser/Line Manager" || AL.EmployeePosition.PositionName == "Program Manager" && _presenter.CurrentTravelAdvanceRequest.CurrentLevel == 1)
                {
                    will = "Approve";
                    break;

                }
                /*else if (_presenter.GetUser(_presenter.CurrentTravelAdvanceRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName)
                {
                    will = AL.Will;
                }*/
                else
                {
                    try
                    {
                        if (_presenter.GetUser(_presenter.CurrentTravelAdvanceRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName)
                        {
                            will = AL.Will;
                        }
                    }
                    catch
                    {
                        if (_presenter.CurrentTravelAdvanceRequest.CurrentApproverPosition == AL.EmployeePosition.Id)
                        {
                            will = AL.Will;
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
        private void PopRequesters(DropDownList ddl)
        {
            ddl.DataSource = _presenter.GetEmployeeList();
            ddl.DataTextField = "FullName";
            ddl.DataValueField = "ID";
            ddl.DataBind();

            ddl.Items.Insert(0, new ListItem("Select Requester", "0"));
            ddl.SelectedIndex = 0;
        }
        private void BindSearchTravelAdvanceRequestGrid()
        {
            grvTravelAdvanceRequestList.DataSource = _presenter.ListTravelAdvanceRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text, ddlSrchProgressStatus.SelectedValue, ddlSrchRequester.SelectedValue);
            grvTravelAdvanceRequestList.DataBind();
        }
        private void BindTravelAdvanceRequestStatus()
        {
            foreach (TravelAdvanceRequestStatus TARS in _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses)
            {
                if (TARS.WorkflowLevel == _presenter.CurrentTravelAdvanceRequest.CurrentLevel && _presenter.CurrentTravelAdvanceRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    btnApprove.Enabled = true;
                }

                if (TARS.WorkflowLevel == _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses.Count && TARS.ApprovalStatus != null)
                {
                    btnPrint.Enabled = true;
                    btnApprove.Enabled = false;
                }
                else if (_presenter.CurrentTravelAdvanceRequest.CurrentStatus == ApprovalStatus.Rejected.ToString())
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

            if (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses.Last().ApprovalStatus == "Bank Payment" && !IsBankPaymentRequested() && _presenter.CurrentTravelAdvanceRequest.CurrentStatus != ApprovalStatus.Rejected.ToString())
                btnBankPayment.Visible = true;
            else
                btnBankPayment.Visible = false;
        }
        private void ShowPrint()
        {
            if (_presenter.CurrentTravelAdvanceRequest.CurrentLevel == _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses.Count && _presenter.CurrentTravelAdvanceRequest.ProgressStatus == ProgressStatus.Completed.ToString())
            {
                btnPrint.Enabled = true;
                if (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses.Last().PaymentType == "Bank Payment")
                    btnBankPayment.Visible = true;
                SendEmailToRequester();
            }
        }
        private void SendEmail(TravelAdvanceRequestStatus TARS)
        {
            if (TARS.Approver != 0)
            {
                if (_presenter.GetUser(TARS.Approver).IsAssignedJob != true)
                {
                    EmailSender.Send(_presenter.GetSuperviser(TARS.Approver).Email, "Travel Advance Approval", (_presenter.CurrentTravelAdvanceRequest.AppUser.FullName).ToUpper() + " Requests for Travel Advance with Travel Advance No. - " + (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceNo).ToUpper());
                }
                else
                {
                    EmailSender.Send(_presenter.GetSuperviser(_presenter.GetAssignedJobbycurrentuser(TARS.Approver).AssignedTo).Email, "Travel Advance Approval", (_presenter.CurrentTravelAdvanceRequest.AppUser.FullName).ToUpper() + " Requests for Travel Advance with Travel Advance No. - " + (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceNo).ToUpper());
                }
            }
            else
            {
                foreach (AppUser Payer in _presenter.GetAppUsersByEmployeePosition(TARS.ApproverPosition))
                {
                    if (Payer.IsAssignedJob != true)
                    {
                        //Commented out because Finance team got tired of emails
                        //EmailSender.Send(Payer.Email, "Travel Advance Approval", (_presenter.CurrentTravelAdvanceRequest.AppUser.FullName).ToUpper() + " Requests for Travel Advance with Travel Advance No. - " + (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceNo).ToUpper());
                    }
                    else
                    {
                        //Commented out because Finance team got tired of emails
                        //EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(Payer.Id).AssignedTo).Email, "Travel Advance Approval", (_presenter.CurrentTravelAdvanceRequest.AppUser.FullName).ToUpper() + " Requests for Travel Advance with Travel Advance No. - " + (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceNo).ToUpper());
                    }
                }
            }


        }
        private void SendEmailRejected(TravelAdvanceRequestStatus TARS)
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentTravelAdvanceRequest.AppUser.Id).Email, "Travel Advance Request Rejection", "Your Travel Advance Request with Travel Advance No. - '" + (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceNo).ToUpper() + "' made by " + (_presenter.GetUser(_presenter.CurrentTravelAdvanceRequest.AppUser.Id).FullName).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (TARS.RejectedReason).ToUpper() + "'");

            if (TARS.WorkflowLevel > 1)
            {
                for (int i = 0; i + 1 < TARS.WorkflowLevel; i++)
                {
                    EmailSender.Send(_presenter.GetUser(_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses[i].Approver).Email, "Travel Advance Request Rejection", "Travel Advance Request with Travel Advance No. - '" + (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceNo).ToUpper() + "' made by " + (_presenter.GetUser(_presenter.CurrentTravelAdvanceRequest.AppUser.Id).FullName).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (TARS.RejectedReason).ToUpper() + "'");
                }
            }
        }
        private void SendEmailToRequester()
        {
            if (_presenter.CurrentTravelAdvanceRequest.CurrentStatus != ApprovalStatus.Rejected.ToString() && _presenter.CurrentTravelAdvanceRequest.CurrentStatus != "Bank Payment")
                EmailSender.Send(_presenter.GetUser(_presenter.CurrentTravelAdvanceRequest.AppUser.Id).Email, "Tavel Adavnce Completion", "Your Travel Advance Request with Travel Advance No. - '" + (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceNo).ToUpper() + "' was completed. Please collect your payment.");
        }
        private bool IsBankPaymentRequested()
        {
            OperationalControlRequest ocr = _presenter.GetOperationalControlRequestByTravelId(_presenter.CurrentTravelAdvanceRequest.Id);
            if (ocr != null)
                return true;
            else
                return false;
        }
        private bool IsBankPaymentRequested(int travelId)
        {
            OperationalControlRequest ocr = _presenter.GetOperationalControlRequestByTravelId(travelId);
            if (ocr != null)
                return true;
            else
                return false;
        }
        private void GetNextApprover()
        {
            foreach (TravelAdvanceRequestStatus TARS in _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses)
            {
                if (TARS.ApprovalStatus == null)
                {
                    if (TARS.Approver == 0)
                    {
                        //This is to handle multiple Finance Officers responding to this request
                        //SendEmailToFinanceOfficers;
                        _presenter.CurrentTravelAdvanceRequest.CurrentApproverPosition = TARS.ApproverPosition;
                    }
                    else
                    {
                        _presenter.CurrentTravelAdvanceRequest.CurrentApproverPosition = 0;
                    }
                    SendEmail(TARS);
                    _presenter.CurrentTravelAdvanceRequest.CurrentApprover = TARS.Approver;
                    _presenter.CurrentTravelAdvanceRequest.CurrentLevel = TARS.WorkflowLevel;
                    _presenter.CurrentTravelAdvanceRequest.CurrentStatus = TARS.ApprovalStatus;
                    _presenter.CurrentTravelAdvanceRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                    break;
                }
            }
        }
        private void PrintTransaction()
        {
            lblRequestNoResult.Text = _presenter.CurrentTravelAdvanceRequest.TravelAdvanceNo;
            lblRequestedDateResult.Text = _presenter.CurrentTravelAdvanceRequest.RequestDate.ToString();
            lblRequesterResult.Text = _presenter.CurrentTravelAdvanceRequest.AppUser.FullName;
            lblVisitingTeamResult.Text = _presenter.CurrentTravelAdvanceRequest.VisitingTeam;
            lblPurposeOfTravelResult.Text = _presenter.CurrentTravelAdvanceRequest.PurposeOfTravel;
            lblCommentsResult.Text = _presenter.CurrentTravelAdvanceRequest.Comments.ToString();
            lblTotalTravelAdvanceResult.Text = _presenter.CurrentTravelAdvanceRequest.TotalTravelAdvance.ToString();
            lblApprovalStatusResult.Text = _presenter.CurrentTravelAdvanceRequest.ProgressStatus.ToString();
            lblProjectIdResult.Text = _presenter.CurrentTravelAdvanceRequest.Project.ProjectCode;
            lblGrantIdResult.Text = _presenter.CurrentTravelAdvanceRequest.Grant.GrantCode;

            grvDetails.DataSource = _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails;
            grvDetails.DataBind();

            grvStatuses.DataSource = _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses;
            grvStatuses.DataBind();

            IList<TravelAdvanceCost> allCosts = new List<TravelAdvanceCost>();

            foreach (TravelAdvanceRequestDetail detail in _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails)
            {
                foreach (TravelAdvanceCost cost in detail.TravelAdvanceCosts)
                {
                    allCosts.Add(cost);
                }
            }
            grvCost.DataSource = allCosts;
            grvCost.DataBind();
        }
        private void SaveTravelAdvanceRequestStatus()
        {
            foreach (TravelAdvanceRequestStatus TARS in _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses)
            {
                if ((TARS.Approver == _presenter.CurrentUser().Id || (TARS.ApproverPosition == _presenter.CurrentUser().EmployeePosition.Id) || _presenter.CurrentUser().Id == (_presenter.GetAssignedJobbycurrentuser(TARS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(TARS.Approver).AssignedTo : 0)) && TARS.WorkflowLevel == _presenter.CurrentTravelAdvanceRequest.CurrentLevel)
                {
                    TARS.ApprovalStatus = ddlApprovalStatus.SelectedValue;
                    TARS.Date = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                    TARS.AssignedBy = _presenter.GetAssignedJobbycurrentuser(TARS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(TARS.Approver).AppUser.FullName : "";
                    TARS.RejectedReason = txtRejectedReason.Text;
                    if (TARS.ApprovalStatus != ApprovalStatus.Rejected.ToString())
                    {
                        if (_presenter.CurrentTravelAdvanceRequest.CurrentLevel == _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses.Count)
                        {
                            _presenter.CurrentTravelAdvanceRequest.ProgressStatus = ProgressStatus.Completed.ToString();

                            TARS.Approver = _presenter.CurrentUser().Id;
                            _presenter.CurrentTravelAdvanceRequest.CurrentStatus = TARS.ApprovalStatus;
                            if (ddlApprovalStatus.SelectedValue == "Bank Payment")
                            {
                                btnBankPayment.Visible = true;
                            }
                            else
                            {
                                //For Petty Cash the process has ended and is now ready to be liquidated.
                                _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationStatus = ProgressStatus.Completed.ToString();
                            }
                        }
                        GetNextApprover();
                        Log.Info(_presenter.GetUser(TARS.Approver).FullName + " has " + TARS.ApprovalStatus + " Travel Advance Request made by " + _presenter.CurrentTravelAdvanceRequest.AppUser.FullName);
                    }
                    else
                    {
                        _presenter.CurrentTravelAdvanceRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        _presenter.CurrentTravelAdvanceRequest.CurrentStatus = ApprovalStatus.Rejected.ToString();
                        _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationStatus = "Finished";

                        TARS.Approver = _presenter.CurrentUser().Id;
                        SendEmailRejected(TARS);
                        Log.Info(_presenter.GetUser(TARS.Approver).FullName + " has " + TARS.ApprovalStatus + " Travel Advance Request made by " + _presenter.CurrentTravelAdvanceRequest.AppUser.FullName);
                    }
                    break;
                }

            }
        }
        protected void grvTravelAdvanceRequestList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                if (e.CommandName == "ViewItem")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    int reqId = Convert.ToInt32(grvTravelAdvanceRequestList.DataKeys[rowIndex].Value);
                    Session["CurrentTravelAdvanceRequest"] = _presenter.GetTravelAdvanceRequest(reqId);
                    _presenter.CurrentTravelAdvanceRequest = (TravelAdvanceRequest)Session["CurrentTravelAdvanceRequest"];
                    //_presenter.OnViewLoaded();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
                    dgTravelAdvanceRequestDetail.DataSource = _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails;
                    dgTravelAdvanceRequestDetail.DataBind();
                    grvTravelRequestStatuses.DataSource = _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses;
                    grvTravelRequestStatuses.DataBind();
                    grvTravelAdvanceCosts.DataSource = null;
                    grvTravelAdvanceCosts.DataBind();
                }
            }
        }
        protected void grvTravelRequestStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses[e.Row.RowIndex].Approver > 0)
                    e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses[e.Row.RowIndex].Approver).FullName;
            }
        }
        protected void grvTravelAdvanceRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Button btnStatus = e.Row.FindControl("btnStatus") as Button;
            TravelAdvanceRequest TAR = e.Row.DataItem as TravelAdvanceRequest;
            if (TAR != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (TAR.ProgressStatus == ProgressStatus.InProgress.ToString())
                    {
                        btnStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFF6C");
                    }
                    else if (TAR.TravelAdvanceRequestStatuses.Last().ApprovalStatus == "Bank Payment" && !IsBankPaymentRequested(TAR.Id) && TAR.CurrentStatus != ApprovalStatus.Rejected.ToString())
                    {
                        btnStatus.BackColor = System.Drawing.Color.Green;
                    }
                    else if (TAR.ProgressStatus == ProgressStatus.Completed.ToString())
                    {
                        btnStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF7251");
                    }

                    if (ddlSrchProgressStatus.SelectedValue == "Reviewed")
                    {
                        e.Row.Cells[9].Enabled = false;
                    }
                }
            }
        }
        protected void grvTravelAdvanceRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentTravelAdvanceRequest.ProgressStatus == ProgressStatus.Completed.ToString())
            {
                PrintTransaction();
            }
            PopApprovalStatus();
            Session["PaymentId"] = _presenter.CurrentTravelAdvanceRequest.Id;
            BindTravelAdvanceRequestStatus();
            txtRejectedReason.Visible = false;
            rfvRejectedReason.Enabled = false;
            ScriptManager.RegisterStartupScript(this, GetType(), "showApprovalModal", "showApprovalModal();", true);
        }
        protected void grvTravelAdvanceRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvTravelAdvanceRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses[e.Row.RowIndex].Approver != 0)
                        e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses[e.Row.RowIndex].Approver).FullName;
                }
            }
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindSearchTravelAdvanceRequestGrid();
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentTravelAdvanceRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    SaveTravelAdvanceRequestStatus();
                    //_presenter.CurrentTravelAdvanceRequest.Account = _presenter.GetAccount(Convert.ToInt32(ddlAccount.SelectedValue));
                    _presenter.SaveOrUpdateTravelAdvanceRequest(_presenter.CurrentTravelAdvanceRequest);
                    ShowPrint();
                    if (ddlApprovalStatus.SelectedValue != "Rejected")
                        Master.ShowMessage(new AppMessage("Travel Advance  Approval Processed", RMessageType.Info));
                    else
                        Master.ShowMessage(new AppMessage("Travel Advance  Approval Rejected", RMessageType.Info));
                    btnApprove.Enabled = false;
                    BindSearchTravelAdvanceRequestGrid();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showApprovalModal", "showApprovalModal();", true);
                }
                PrintTransaction();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Approve the Travel Advance! " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void btnBankPayment_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("../Request/frmOperationalControlRequest.aspx?paymentId={0}&Page={1}", Convert.ToInt32(Session["PaymentId"]), "TravelAdvance"));
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
        protected void dgTravelAdvanceRequestDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            //_presenter.OnViewLoaded();
            int recordId = (int)dgTravelAdvanceRequestDetail.DataKeys[dgTravelAdvanceRequestDetail.SelectedIndex];
            _presenter.CurrentTravelAdvanceRequest = (TravelAdvanceRequest)Session["CurrentTravelAdvanceRequest"];
            grvTravelAdvanceCosts.DataSource = _presenter.CurrentTravelAdvanceRequest.GetTravelAdvanceRequestDetail(recordId).TravelAdvanceCosts;
            grvTravelAdvanceCosts.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
        }
        protected void grvTravelAdvanceCosts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvTravelAdvanceCosts.PageIndex = e.NewPageIndex;
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Default.aspx");
        }
    }
}