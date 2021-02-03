using Chai.WorkflowManagment.CoreDomain.Request;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Shared.MailSender;
using Microsoft.Practices.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.Reflection;
using log4net.Config;
using Chai.WorkflowManagment.CoreDomain.HRM;
using Chai.WorkflowManagment.Shared.Settings;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public partial class frmLeaveRequest : POCBasePage, ILeaveRequestView
    {
        private LeaveRequestPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        private LeaveRequest _leaverequest;
        private int _leaverequestId = 0;
        private decimal requesteddays = 0;
        private decimal totalsickleavetaken = 0;
        private Employee employee = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                CheckApprovalSettings();
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                PopLeaveType();
                BindSearchLeaveRequestGrid();
                           

            }
            this._presenter.OnViewLoaded();
            employee = _presenter.GetEmployee(_presenter.CurrentUser().Id);
            //txtLeaveAsOfCalEndDate.Text = (Math.Round((employee.EmployeeLeaveBalanceYE() - _presenter.EmpLeaveTaken(employee.Id, employee.LeaveSettingDate.Value)) * 2, MidpointRounding.AwayFromZero) / 2).ToString();
            txtLeaveAsOfToday.Text = (Math.Round((employee.EmployeeLeaveBalance() - _presenter.EmpLeaveTaken(employee.Id, employee.LeaveSettingDate.Value)) * 2, MidpointRounding.AwayFromZero) / 2).ToString();
            if (employee != null)
                BindInitialValues();
            if (_presenter.NotCompletRequest(_presenter.CurrentUser().Id))
            {
                Master.ShowMessage(new AppMessage("Your Previous Leave Request is not completed. Please review the dashboard and contact the approver ", Chai.WorkflowManagment.Enums.RMessageType.Info));
                btnRequest.Enabled = false;
            }

        }

        private string AutoNumber()
        {
            return "LR-" + _presenter.CurrentUser().Id.ToString() + "-" + (_presenter.GetLastLeaveRequestId() + 1).ToString();
        }
        [CreateNew]
        public LeaveRequestPresenter Presenter
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
                return "{EEB211B3-70FC-429B-8662-927A0C8A9511}";
            }
        }
        private void IsFindPostBack()
        {
        }
        private void CheckApprovalSettings()
        {
            if (_presenter.GetApprovalSetting(RequestType.Leave_Request.ToString().Replace('_', ' '), 0) == null)
            {
                pnlWarning.Visible = true;
            }
        }
        private void BindInitialValues()
        {
            AppUser CurrentUser = _presenter.CurrentUser();
            txtRequester.Text = CurrentUser.FirstName + " " + CurrentUser.LastName;
            // txtEmployeeNo.Text = employee.EmpId;
            //txtEmployeeNo.Text = CurrentUser.EmployeeNo;
             txtDutystation.Text = employee.GetEmployeeDutyStation();
            txtEmployeeNo.Text = CurrentUser.EmployeeNo;
            if (_presenter.CurrentLeaveRequest.Id <= 0)
            {

                txtRequestDate.Text = DateTime.Today.Date.ToShortDateString();


            }
        }
        private void BindLeaveRequest()
        {

            if (_presenter.CurrentLeaveRequest.Id > 0)
            {



                //txtRequestNo.Text = _presenter.CurrentLeaveRequest.RequestNo;
                txtRequestDate.Text = _presenter.CurrentLeaveRequest.RequestedDate.ToString();
                ddlLeaveType.SelectedValue = _presenter.CurrentLeaveRequest.LeaveType != null ? _presenter.CurrentLeaveRequest.LeaveType.Id.ToString() : "0";
                txtDateFrom.Text = _presenter.CurrentLeaveRequest.DateFrom.ToString();
                txtDateTo.Text = _presenter.CurrentLeaveRequest.DateTo.ToString();
                txtAddress.Text = _presenter.CurrentLeaveRequest.Addresswhileonleave;
                txtapplyfor.Text = _presenter.CurrentLeaveRequest.Applyfor.ToString();
                txtCompReason.Text = _presenter.CurrentLeaveRequest.CompassionateReason;
                txtbalance.Text = _presenter.CurrentLeaveRequest.Balance.ToString();
                txtforward.Text = _presenter.CurrentLeaveRequest.Forward.ToString();
                SelectedLeaveType();
            }
        }
        private void SaveLeaveRequest()
        {
            AppUser CurrentUser = _presenter.CurrentUser();
            try
            {
                _presenter.CurrentLeaveRequest.Requester = CurrentUser.Id;
                _presenter.CurrentLeaveRequest.EmployeeNo = txtEmployeeNo.Text;
                _presenter.CurrentLeaveRequest.RequestedDate = Convert.ToDateTime(txtRequestDate.Text);
                _presenter.CurrentLeaveRequest.RequestNo = AutoNumber();
                _presenter.CurrentLeaveRequest.RequestedDate = Convert.ToDateTime(txtRequestDate.Text);
                _presenter.CurrentLeaveRequest.LeaveType = _presenter.GetLeaveType(int.Parse(ddlLeaveType.SelectedValue));
                _presenter.CurrentLeaveRequest.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                _presenter.CurrentLeaveRequest.DateTo = Convert.ToDateTime(txtDateTo.Text);
                _presenter.CurrentLeaveRequest.Addresswhileonleave = txtAddress.Text;
                _presenter.CurrentLeaveRequest.Applyfor = Convert.ToInt32(txtapplyfor.Text);
                _presenter.CurrentLeaveRequest.RequestedDays = Convert.ToDecimal(Session["Requesteddays"]);
                _presenter.CurrentLeaveRequest.CompassionateReason = txtCompReason.Text;
                _presenter.CurrentLeaveRequest.Balance = txtbalance.Text != "" ? Convert.ToDecimal(txtbalance.Text) : 0;
                _presenter.CurrentLeaveRequest.Forward = txtforward.Text != "" ? Convert.ToDecimal(txtforward.Text) : 0;
                _presenter.CurrentLeaveRequest.Type = ddltype.SelectedValue;
                if(ddlLeaveType.SelectedItem.Text == "Sick Leave")
                    _presenter.CurrentLeaveRequest.FilePath = UploadFile();
                SaveLeaveRequestStatus();

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Violation of UNIQUE KEY"))
                    {
                        Master.ShowMessage(new AppMessage("Please Click Request button Again,There is a duplicate Number", Chai.WorkflowManagment.Enums.RMessageType.Error));
                        //AutoNumber();
                    }
                }
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }

        }
        private void SaveLeaveRequestStatus()
        {
            if (_presenter.CurrentLeaveRequest.Id <= 0)
            {
                if (_presenter.GetApprovalSetting(RequestType.Leave_Request.ToString().Replace('_', ' '), 0) != null)
                {
                    int i = 1;
                    foreach (ApprovalLevel AL in _presenter.GetApprovalSetting(RequestType.Leave_Request.ToString().Replace('_', ' '), 0).ApprovalLevels)
                    {
                        LeaveRequestStatus LRS = new LeaveRequestStatus();
                        LRS.LeaveRequest = _presenter.CurrentLeaveRequest;
                        if (AL.EmployeePosition.PositionName == "Superviser/Line Manager")
                        {
                            if (_presenter.CurrentUser().Superviser.Value != 0)
                            {
                                LRS.Approver = _presenter.CurrentUser().Superviser.Value;
                            }
                            else
                            {
                                LRS.ApprovalStatus = ApprovalStatus.Approved.ToString();
                                LRS.ApprovalDate = DateTime.Today.Date;
                            }
                        }
                        else
                        {
                            LRS.Approver = _presenter.Approver(AL.EmployeePosition.Id).Id;
                        }
                        LRS.WorkflowLevel = i;
                        i++;
                        _presenter.CurrentLeaveRequest.LeaveRequestStatuses.Add(LRS);

                    }
                }
            }
        }
        private void GetCurrentApprover()
        {
            txtbalance.Text = txtbalance.Text != "" ? txtbalance.Text : "0";
            if (!(ddlLeaveType.SelectedItem.Text == "Annual Leave" && Convert.ToDecimal(txtbalance.Text) < 0))
            {
                foreach (LeaveRequestStatus LRS in _presenter.CurrentLeaveRequest.LeaveRequestStatuses)
                {
                    if (LRS.ApprovalStatus == null)
                    {
                     
                        _presenter.CurrentLeaveRequest.CurrentApprover = LRS.Approver;
                        _presenter.CurrentLeaveRequest.CurrentLevel = LRS.WorkflowLevel;
                        _presenter.CurrentLeaveRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                        SendEmail(LRS);
                        break;

                    }
                }
            }
        }
        private void SendEmail(LeaveRequestStatus LRS)
        {
            if (_presenter.GetSuperviser(LRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(_presenter.GetSuperviser(LRS.Approver).Email, "Leave Request", "'" + (_presenter.GetUser(_presenter.CurrentLeaveRequest.Requester).FullName).ToUpper() + "' Requests for Leave Request No. '" + (_presenter.CurrentLeaveRequest.RequestNo).ToUpper() + "'");
            }
            else
            {
                EmailSender.Send(_presenter.GetSuperviser(_presenter.GetAssignedJobbycurrentuser(LRS.Approver).AssignedTo).Email, "Leave Request", "'" + (_presenter.GetUser(_presenter.CurrentLeaveRequest.Requester).FullName).ToUpper() + "' Requests for Leave Request No. '" + (_presenter.CurrentLeaveRequest.RequestNo).ToUpper() + "' ");
            }
        }
        public CoreDomain.Request.LeaveRequest LeaveRequest
        {
            get
            {
                return _leaverequest;
            }
            set
            {
                _leaverequest = value;
            }
        }
        public string RequestNo
        {
            get { return txtRequestNosearch.Text; }
        }
        public string RequestDate
        {
            get { return txtRequestDatesearch.Text; }
        }
        public int LeaveRequestId
        {
            get
            {
                if (_leaverequestId != 0)
                {
                    return _leaverequestId;
                }
                else
                {
                    return 0;
                }
            }
        }
        protected void btnRequest_Click(object sender, EventArgs e)
        {

            SaveLeaveRequest();

            if (_presenter.CurrentLeaveRequest.LeaveRequestStatuses.Count != 0)
            {
                if (ddlLeaveType.SelectedItem.Text != "Annual Leave")
                {
                    if (ddlLeaveType.SelectedItem.Text != "Sick Leave")
                    {
                        GetCurrentApprover();
                        _presenter.SaveOrUpdateLeaveRequest(_presenter.CurrentLeaveRequest);

                        ClearForm();
                        BindSearchLeaveRequestGrid();
                        Master.TransferMessage(new AppMessage("Successfully did a Leave  Request, Reference No - <b>'" + _presenter.CurrentLeaveRequest.RequestNo + "'</b>", Chai.WorkflowManagment.Enums.RMessageType.Info));
                        _presenter.RedirectPage(String.Format("frmLeaveRequest.aspx?{0}=0", AppConstants.TABID));
                       // Master.ShowMessage(new AppMessage("Successfully did a Leave  Request, Reference No - <b>'" + _presenter.CurrentLeaveRequest.RequestNo + "'</b>", Chai.WorkflowManagment.Enums.RMessageType.Info));
                        Log.Info(_presenter.CurrentUser().FullName + " has requested for a Leave Type of " + ddlLeaveType.SelectedValue);
                    }
                    else if (ddlLeaveType.SelectedItem.Text == "Sick Leave")
                    {
                        if (_presenter.CurrentLeaveRequest.FilePath != "")
                        {
                            if ((totalsickleavetaken + Convert.ToInt32(txtapplyfor.Text)) < Convert.ToDecimal(UserSettings.GetEntitledSickLeave))
                            {
                                GetCurrentApprover();
                                _presenter.SaveOrUpdateLeaveRequest(_presenter.CurrentLeaveRequest);

                                ClearForm();
                                BindSearchLeaveRequestGrid();
                                
                                Master.TransferMessage(new AppMessage("Successfully did a Leave  Request, Reference No - <b>'" + _presenter.CurrentLeaveRequest.RequestNo + "'</b>", Chai.WorkflowManagment.Enums.RMessageType.Info));
                                _presenter.RedirectPage(String.Format("frmLeaveRequest.aspx?{0}=0", AppConstants.TABID));
                                Log.Info(_presenter.CurrentUser().FullName + " has requested for a Leave Type of " + ddlLeaveType.SelectedValue);
                            }
                            else
                            {
                                Master.ShowMessage(new AppMessage("Please contact HR, Your Sick Leave balance exceeded from your Entitlement", Chai.WorkflowManagment.Enums.RMessageType.Error));
                            }
                        }
                        else
                        {
                            Master.ShowMessage(new AppMessage("Please Attach Sick leave letter", Chai.WorkflowManagment.Enums.RMessageType.Error));
                        }

                    }

                }
                else if (ddlLeaveType.SelectedItem.Text == "Annual Leave" && Convert.ToDecimal(Session["Requesteddays"]) <= Convert.ToDecimal(txtforward.Text))
                {
                    GetCurrentApprover();
                    _presenter.SaveOrUpdateLeaveRequest(_presenter.CurrentLeaveRequest);
                    ClearForm();
                    BindSearchLeaveRequestGrid();
                    Master.TransferMessage(new AppMessage("Successfully did a Leave  Request, Reference No - <b>'" + _presenter.CurrentLeaveRequest.RequestNo + "'</b>", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    _presenter.RedirectPage(String.Format("frmLeaveRequest.aspx?{0}=0", AppConstants.TABID));
                    Log.Info(_presenter.CurrentUser().FullName + " has requested for a Leave Type of " + ddlLeaveType.SelectedValue);
                }
                else
                { Master.ShowMessage(new AppMessage("You don't have sufficient Annual Leave days", Chai.WorkflowManagment.Enums.RMessageType.Error)); }

            }
            else
            {
                Master.ShowMessage(new AppMessage("There is an error constracting Approval Process", Chai.WorkflowManagment.Enums.RMessageType.Error));

            }
        }
        private void ClearForm()
        {
            //txtRequestNo.Text = "";
            txtRequestDate.Text = "";
            ddlLeaveType.SelectedValue = "0";
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            txtAddress.Text = "";
            txtapplyfor.Text = "";
            txtCompReason.Text = "";
            txtbalance.Text = "";
            txtforward.Text = "";

        }
        
        protected void grvLeaveRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Session["ApprovalLevel"] = true;
            // ClearForm();
            //BindLeaveRequest();
            _leaverequestId = Convert.ToInt32(grvLeaveRequestList.SelectedDataKey[0]);
            _presenter.OnViewLoaded();
            BindLeaveRequest();
            if (_presenter.CurrentLeaveRequest.LeaveType.LeaveTypeName.Contains("Annual Leave"))
            {
                lblBalance.Visible = true;
                txtbalance.Visible = true;
                lblforward.Visible = true;
                txtforward.Visible = true;
            }
        }
        protected void grvLeaveRequestList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            _presenter.DeleteLeaveRequestg(_presenter.GetLeaveRequestById(Convert.ToInt32(grvLeaveRequestList.DataKeys[e.RowIndex].Value)));

            btnFind_Click(sender, e);
            Master.ShowMessage(new AppMessage("Leave Request Successfully Deleted", Chai.WorkflowManagment.Enums.RMessageType.Info));

        }
        protected void grvLeaveRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            LeaveRequest leaverequest = e.Row.DataItem as LeaveRequest;
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (leaverequest != null)
                {
                    if (leaverequest.GetLeaveRequestStatusworkflowLevel(1).ApprovalStatus != null)
                    {
                        e.Row.Cells[5].Enabled = false;
                       
                    }

                }
                //LinkButton db = (LinkButton)e.Row.Cells[5].Controls[0];
                //db.OnClientClick = "return confirm('Are you sure you want to delete this Recieve?');";
            }
        }
        protected void grvLeaveRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvLeaveRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        private void PopLeaveType()
        {
            ddlLeaveType.DataSource = _presenter.GetLeaveTypes();
            ddlLeaveType.DataBind();

        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindSearchLeaveRequestGrid();
            ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
            // pnlPopUpSearch_ModalPopupExtender.Show();
        }
        private void BindSearchLeaveRequestGrid()
        {
            grvLeaveRequestList.DataSource = _presenter.ListLeaveRequests(txtRequestNosearch.Text, txtRequestDatesearch.Text);
            grvLeaveRequestList.DataBind();
        }
        protected void ddlLeaveType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void SelectedLeaveType()
        {
            if (_presenter.CurrentLeaveRequest.LeaveType != null)
            {
                if (_presenter.CurrentLeaveRequest.LeaveType.LeaveTypeName == "Annual")
                {
                    txtAddress.Visible = true;
                    lblAddress.Visible = true;
                    txtCompReason.Visible = false;
                    lblCompReason.Visible = false;
                    lblBalance.Visible = true;
                    lblforward.Visible = true;
                    txtforward.Visible = true;
                    txtbalance.Visible = true;

                }
                else if (_presenter.CurrentLeaveRequest.LeaveType.LeaveTypeName == "Compassionate" || _presenter.CurrentLeaveRequest.LeaveType.LeaveTypeName == "Special")
                {
                    txtCompReason.Visible = true;
                    lblCompReason.Visible = true;
                    txtAddress.Visible = false;
                    lblAddress.Visible = false;
                    lblBalance.Visible = false;
                    lblforward.Visible = false;
                    txtforward.Visible = false;
                    txtbalance.Visible = false;
                    FileUpload1.Visible = false;


                }
                else if (_presenter.CurrentLeaveRequest.LeaveType.LeaveTypeName == "Sick leave")
                {

                    txtCompReason.Visible = false;
                    lblCompReason.Visible = false;
                    txtAddress.Visible = false;
                    lblAddress.Visible = false;
                    lblBalance.Visible = false;
                    lblforward.Visible = false;
                    txtforward.Visible = false;
                    txtbalance.Visible = false;
                    FileUpload1.Visible = true;

                }
                else
                {

                    txtCompReason.Visible = false;
                    lblCompReason.Visible = false;
                    txtAddress.Visible = false;
                    lblAddress.Visible = false;
                    lblBalance.Visible = false;
                    lblforward.Visible = false;
                    txtforward.Visible = false;
                    txtbalance.Visible = false;
                    FileUpload1.Visible = false;
                }
            }
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            _presenter.CancelPage();
        }
        private decimal CalculateLeave(EmployeeLeave empleave)
        {
            decimal workingdays = Convert.ToDecimal((DateTime.Today.Date - empleave.StartDate).TotalDays);
            decimal leavedays = (workingdays / 30) * empleave.Rate;
            decimal res = (empleave.BeginingBalance + leavedays) - empleave.LeaveTaken;
            if (res < 0)
                return 0;
            else
                return Math.Round(res);

        }
        protected void ddlLeaveType_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (ddlLeaveType.SelectedItem.Text.Contains("Annual"))
            {
                txtAddress.Visible = true;
                lblAddress.Visible = true;

                txtCompReason.Visible = false;
                lblCompReason.Visible = false;
                lblBalance.Visible = true;
                lblforward.Visible = true;
                txtforward.Visible = true;
                txtbalance.Visible = true;

                if (employee != null)
                {
                    txtforward.Text = (Math.Round((employee.EmployeeLeaveBalance() - _presenter.EmpLeaveTaken(employee.Id, employee.LeaveSettingDate.Value)) * 2,MidpointRounding.AwayFromZero)/2).ToString();

                }
                else
                {
                    txtforward.Text = "0";
                    lblnoempleavesetting.Text = "Your Leave setting is not defined,Please contact HR Officer.";
                }


            }
            else if (ddlLeaveType.SelectedItem.Text.Contains("Compassionate") || ddlLeaveType.SelectedItem.Text.Contains("Special"))
            {
                txtCompReason.Visible = true;
                lblCompReason.Visible = true;
                txtAddress.Visible = false;
                lblAddress.Visible = false;
                lblBalance.Visible = false;
                lblforward.Visible = false;
                txtforward.Visible = false;
                txtbalance.Visible = false;
                FileUpload1.Visible = false;


            }
            else if (ddlLeaveType.SelectedItem.Text.Contains("Sick Leave") || ddlLeaveType.SelectedItem.Text.Contains("Exam Leave") || ddlLeaveType.SelectedItem.Text.Contains("Other Leaves"))
            {


                txtCompReason.Visible = false;
                lblCompReason.Visible = false;
                txtAddress.Visible = false;
                lblAddress.Visible = false;
                lblBalance.Visible = false;
                lblforward.Visible = false;
                txtforward.Visible = false;
                txtbalance.Visible = false;
                FileUpload1.Visible = true;
                totalsickleavetaken = _presenter.getTotalSickLeaveTaken(employee.Id);

            }
            else
            {


                txtCompReason.Visible = false;
                lblCompReason.Visible = false;
                txtAddress.Visible = false;
                lblAddress.Visible = false;
                lblBalance.Visible = false;
                lblforward.Visible = false;
                txtforward.Visible = false;
                txtbalance.Visible = false;
                FileUpload1.Visible = false;
            }
            GetLeaveBalance();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (_presenter.CurrentLeaveRequest.Id > 0)
            {
                _presenter.DeleteLeaveRequestg(_presenter.CurrentLeaveRequest);
                ClearForm();

                btnDelete.Visible = false;
                Master.ShowMessage(new AppMessage("Leave Request Successfully Deleted", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
        }
        private void GetLeaveBalance()
        {
            if (ddlLeaveType.SelectedItem.Text == "Annual Leave")
            {
                if (txtforward.Text != "" && txtapplyfor.Text != "")
                {
                    decimal bal = (Convert.ToDecimal(txtforward.Text) - Convert.ToDecimal(txtapplyfor.Text));
                    if (bal < 0)

                        txtbalance.Text = Convert.ToString(0);
                    else
                        txtbalance.Text = bal.ToString();
                }
                //else
                //{
                //    Master.ShowMessage(new AppMessage("Please Insert Leave day's brought forward OR I wish to apply for ", Chai.WorkflowManagment.Enums.RMessageType.Error));
                //}
            }
        }
        protected void txtDateFrom_TextChanged(object sender, EventArgs e)
        {
            // CalculateRequestedDays();
        }
        protected void txtDateTo_TextChanged(object sender, EventArgs e)
        {
            //CalculateRequestedDays();
            //txtforward_TextChanged(sender, e);
            //txtapplyfor_TextChanged(sender, e);
        }
        private void CalculateRequestedDays()
        {
            DateTime Datefrom = Convert.ToDateTime(txtDateFrom.Text);
            DateTime DateTo = Convert.ToDateTime(txtDateTo.Text);
            TimeSpan interval = DateTo - Datefrom;
            txtapplyfor.Text = interval.TotalDays.ToString();
        }
        protected void txtforward_TextChanged(object sender, EventArgs e)
        {
            // GetLeaveBalance();
        }
        protected void txtapplyfor_TextChanged(object sender, EventArgs e)
        {
            ddltype_SelectedIndexChanged(sender, e);
            // GetLeaveBalance();
        }

        protected void FileUpload1_DataBinding(object sender, EventArgs e)
        {
            UploadFile();
        }
        private string UploadFile()
        {
            string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);

            if (fileName != String.Empty)
            {

                FileUpload1.PostedFile.SaveAs(Server.MapPath("~/SickLeaveAttachment/") + fileName);
                return "~/SickLeaveAttachment/" + fileName;
            }
            else
            {
               // Master.ShowMessage(new AppMessage("Please select file ", Chai.WorkflowManagment.Enums.RMessageType.Error));
                return "";
            }
        }

        protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime Datefrom = Convert.ToDateTime(txtDateFrom.Text);
            //int days = GetLVWorkingDays(Convert.ToDateTime(txtDateFrom.Text), Convert.ToInt32(txtapplyfor.Text));
                        
            if (ddlLeaveType.SelectedItem.Text.Contains("Annual Leave"))
            {
                if (txtDateFrom.Text != "")
                {

                    if (ddltype.SelectedValue == "Full Day")
                    {

                       // txtDateTo.Text = Datefrom.AddDays(days).ToString();
                        requesteddays = txtapplyfor.Text != "" ? Convert.ToDecimal(txtapplyfor.Text) : Convert.ToDecimal("0");
                        txtbalance.Text = (Convert.ToDecimal(txtforward.Text) - requesteddays).ToString();
                    }
                    else
                    {
                        if (txtapplyfor.Text == "1")
                        {
                            //txtDateTo.Text = txtDateFrom.Text;
                        }
                        else
                        {
                           // txtDateTo.Text = Datefrom.AddDays(days).ToString();
                        }
                        requesteddays = txtapplyfor.Text != "" ? Convert.ToDecimal(txtapplyfor.Text) / 2 : Convert.ToDecimal("0");
                        txtbalance.Text = (Convert.ToDecimal(txtforward.Text) - requesteddays).ToString();
                    }
                    Session["Requesteddays"] = requesteddays;
                    lblrequesteddays.Visible = true;
                    lblRdays.Visible = true;
                    lblRdays.Text = requesteddays.ToString();
                }
            }
            else if (ddlLeaveType.SelectedItem.Text.Contains("Maternity Leave"))
            {
                if (txtDateFrom.Text != "")
                {
                    //  DateTime Datefrom = Convert.ToDateTime(txtDateFrom.Text);
                    if (ddltype.SelectedValue == "Full Day")
                    {
                        //txtDateTo.Text = Datefrom.AddDays(Convert.ToInt32(txtapplyfor.Text)).ToString();
                        requesteddays = Convert.ToDecimal(txtapplyfor.Text);
                        // txtbalance.Text = (Convert.ToDecimal(txtforward.Text) - requesteddays).ToString();
                    }


                }
                Session["Requesteddays"] = requesteddays;
                lblrequesteddays.Visible = true;
                lblRdays.Visible = true;
                lblRdays.Text = requesteddays.ToString();

            }
            else
            {
                if (txtDateFrom.Text != "")
                {
                    //  DateTime Datefrom = Convert.ToDateTime(txtDateFrom.Text);
                    if (ddltype.SelectedValue == "Full Day")
                    {
                       // txtDateTo.Text = Datefrom.AddDays(days).ToString();
                        requesteddays = txtapplyfor.Text != "" ? Convert.ToDecimal(txtapplyfor.Text) : Convert.ToDecimal("0");
                        // txtbalance.Text = (Convert.ToDecimal(txtforward.Text) - requesteddays).ToString();
                    }
                    else
                    {
                        if (txtapplyfor.Text == "1")
                        {
                           // txtDateTo.Text = txtDateFrom.Text;
                        }
                        else
                        {
                           // txtDateTo.Text = Datefrom.AddDays(Convert.ToInt32(txtapplyfor.Text)).ToString();
                        }

                        requesteddays = txtapplyfor.Text != "" ? Convert.ToDecimal(txtapplyfor.Text) / 2 : Convert.ToDecimal("0");

                        // txtbalance.Text = (Convert.ToDecimal(txtforward.Text) - requesteddays).ToString();
                    }
                    Session["Requesteddays"] = requesteddays;
                    lblrequesteddays.Visible = true;
                    lblRdays.Visible = true;
                    lblRdays.Text = requesteddays.ToString();
                }
            }
        }

    }
}