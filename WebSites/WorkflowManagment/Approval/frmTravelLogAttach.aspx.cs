using System;
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
using System.IO;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public partial class frmTravelLogAttach : POCBasePage, ITravelLogAttachView
    {
        private TravelLogAttachPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        private bool needsApproval = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();

            }
            this._presenter.OnViewLoaded();
            BindSearchVehicleRequestGrid();
        }
        [CreateNew]
        public TravelLogAttachPresenter Presenter
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
                return "{7E42140E-DD62-4230-983E-32BD9FA35817}";
            }
        }
        #region Field Getters
        public int GetVehicleRequestId
        {
            get
            {
                if (grvVehicleRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvVehicleRequestList.SelectedDataKey.Value);
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion
        private void BindSearchVehicleRequestGrid()
        {
            grvVehicleRequestList.DataSource = _presenter.ListPendingVehicleRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text);
            grvVehicleRequestList.DataBind();
        }
        private void BindVehicleRequestStatus()
        {
            // TravelLogAttachPresenter _presenterm = new   TravelLogAttachPresenter;
            foreach (VehicleRequestStatus VRS in _presenter.CurrentVehicleRequest.VehicleRequestStatuses)
            {
                if (VRS.WorkflowLevel == _presenter.CurrentVehicleRequest.CurrentLevel && _presenter.CurrentVehicleRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    btnSave.Enabled = true;
                }
                if (_presenter.CurrentVehicleRequest.CurrentLevel == _presenter.CurrentVehicleRequest.VehicleRequestStatuses.Count && !String.IsNullOrEmpty(VRS.ApprovalStatus))
                {
                    btnPrint.Enabled = true;
                }
            }
        }
        private void ShowPrint()
        {
            if (_presenter.CurrentVehicleRequest.CurrentLevel == _presenter.CurrentVehicleRequest.VehicleRequestStatuses.Count)
            {
                btnPrint.Enabled = true;
                SendEmailToRequester();
            }
        }
        private void SendEmail(VehicleRequestStatus VRS)
        {
            if (_presenter.GetUser(VRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(_presenter.GetUser(VRS.Approver).Email, "Vehicle Request", (_presenter.CurrentVehicleRequest.AppUser.FullName).ToUpper() + " Requests  Vehicle for Request No. " + (_presenter.CurrentVehicleRequest.RequestNo).ToUpper());
            }
            else
            {
                EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(VRS.Approver).AssignedTo).Email, "Vehicle Request", (_presenter.CurrentVehicleRequest.AppUser.FullName).ToUpper() + "Requests Vehicle for Request No." + (_presenter.CurrentVehicleRequest.RequestNo).ToUpper());
            }
        }
        private void SendEmailRejected(VehicleRequestStatus VRS)
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentVehicleRequest.AppUser.Id).Email, "Vehicle Request Rejection", " Your Vehicle Request with RequestNo." + (_presenter.CurrentVehicleRequest.RequestNo).ToUpper() + " made by " + (_presenter.CurrentVehicleRequest.AppUser.FullName).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for reason" + (VRS.RejectedReason).ToUpper());
            Log.Info(_presenter.GetUser(VRS.Approver).FullName + " has rejected a Vehicle Request made by " + _presenter.CurrentVehicleRequest.AppUser.FullName);
        }
        private void SendCompletedEmail(VehicleRequestStatus VRS)
        {
            foreach (VehicleRequestDetail assignedVehicle in _presenter.CurrentVehicleRequest.VehicleRequestDetails)
            {



                EmailSender.Send(_presenter.GetUser(_presenter.CurrentVehicleRequest.AppUser.Id).Email, "Vehicle Request ", "Your Vehicle Request has been proccessed by " + (_presenter.GetUser(VRS.Approver).FullName).ToUpper() + " and Your assigned Driver is " + (assignedVehicle.AppUser.FullName).ToUpper() + ". The Car's Plate Number is " + (assignedVehicle.PlateNo).ToUpper());

                Log.Info(_presenter.GetUser(VRS.Approver).FullName + " has approved a Vehicle Request made by " + (_presenter.CurrentVehicleRequest.AppUser.FullName).ToUpper() + " and assigned a Car Rental company named " + (_presenter.GetCarRental(assignedVehicle.CarRental.Id).Name).ToUpper());

            }
        }
        private void SendEmailToRequester()
        {
            foreach (VehicleRequestDetail assignedVehicle in _presenter.CurrentVehicleRequest.VehicleRequestDetails)
            {
                EmailSender.Send(_presenter.GetUser(_presenter.CurrentVehicleRequest.AppUser.Id).Email, "Vehicle Request Completion", " Your Vehicle Request was Completed.  and Your assigned Driver is " + (assignedVehicle.AppUser.FullName).ToUpper() + ". The Car's Plate Number is " + (assignedVehicle.PlateNo).ToUpper());
            }
        }
        protected void grvVehicleRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //grvVehicleRequestList.SelectedDataKey.Value            
            _presenter.OnViewLoaded();
            BindVehicleRequestStatus();
            if (_presenter.CurrentVehicleRequest.TravelLogStatus == ProgressStatus.Completed.ToString())
            {
                btnSave.Enabled = false;
            }
            pnlApproval_ModalPopupExtender.Show();

        }
        protected void grvVehicleRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Button btnStatus = e.Row.FindControl("btnStatus") as Button;
            VehicleRequest CSR = e.Row.DataItem as VehicleRequest;
            if (e.Row.RowType == DataControlRowType.DataRow)
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
        protected void grvVehicleRequestList_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                // If multiple ButtonField column fields are used, use the
                // CommandName property to determine which button was clicked.
                if (e.CommandName == "TravelLog")
                {
                    // Convert the row index stored in the CommandArgument
                    // property to an Integer.
                    int index = Convert.ToInt32(e.CommandArgument);

                    int rowID = Convert.ToInt32(grvVehicleRequestList.DataKeys[index].Value);
                    string url = String.Format("~/Request/frmTravelLog.aspx?requestId={0}", rowID);
                    _presenter.navigate(url);
                }
            }

        }
        protected void grvVehicleRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvVehicleRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindSearchVehicleRequestGrid();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //Attached Certificates
                if (fuTravelLog.HasFile)
                {
                    string fileName = Path.GetFileName(fuTravelLog.PostedFile.FileName);
                    _presenter.CurrentVehicleRequest.TravelLogAttachment = "~/TravelLog/" + fileName;
                    fuTravelLog.PostedFile.SaveAs(Server.MapPath("~/TravelLog/") + fileName);
                }
                _presenter.CurrentVehicleRequest.ActualDaysTravelled = Convert.ToInt32(txtActualDaysTrav.Text);
                _presenter.CurrentVehicleRequest.TravelLogStatus = ProgressStatus.Completed.ToString();
                _presenter.SaveOrUpdateVehicleRequest(_presenter.CurrentVehicleRequest);
                ShowPrint();
                Master.ShowMessage(new AppMessage("Travel Log Successfully Attached ", Chai.WorkflowManagment.Enums.RMessageType.Info));
                btnSave.Enabled = false;
                BindSearchVehicleRequestGrid();
                pnlApproval_ModalPopupExtender.Show();
            }
            catch (Exception ex)
            {

            }
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentVehicleRequest.VehicleRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentVehicleRequest.VehicleRequestStatuses[e.Row.RowIndex].Approver).FullName;
                }
            }
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            pnlApproval.Visible = false;
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Default.aspx");
        }



    }
}