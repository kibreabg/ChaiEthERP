using Chai.WorkflowManagment.CoreDomain.Request;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Shared.MailSender;
using Microsoft.Practices.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.Setting;
using log4net;
using System.Reflection;
using log4net.Config;
using Chai.WorkflowManagment.CoreDomain.Requests;
using System.Data.Entity.Validation;
using System.IO;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public partial class frmMaintenanceApproval : POCBasePage, IMaintenanceApprovalView
    {
        private MaintenanceApprovalPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        private int reqID = 0;
        private MaintenanceRequest _maintenanceRequest;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                PopProgressStatus();
                BindMaintenanceRequestDetails();
                BindSearchMaintenanceGrid();
            }
            this._presenter.OnViewLoaded();
            if (_presenter.CurrentMaintenanceRequest != null)
            {
                if (_presenter.CurrentMaintenanceRequest.Id != 0)
                {
                    BindMaintenanceRequestforprint();
                }
            }

        }
        [CreateNew]
        public MaintenanceApprovalPresenter Presenter
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
                return "{282224A8-DCCA-4FED-AAB1-BEB6A5AA0653}";
            }
        }
        public MaintenanceRequest MaintenanceRequest
        {
            get
            {
                return _maintenanceRequest;
            }
            set
            {
                _maintenanceRequest = value;
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
        public int MaintenanceRequestId
        {
            get
            {
                if (grvMaintenanceRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvMaintenanceRequestList.SelectedDataKey.Value);
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
            ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Rejected.ToString().Replace('_', ' '), ApprovalStatus.Rejected.ToString().Replace('_', ' ')));

        }
        private string GetWillStatus()
        {
            ApprovalSetting AS = _presenter.GetApprovalSettingforProcess(RequestType.Maintenance_Request.ToString().Replace('_', ' ').ToString(), 0);
            string will = "";
            foreach (ApprovalLevel AL in AS.ApprovalLevels)
            {
                if ((AL.EmployeePosition.PositionName == "Superviser/Line Manager" || AL.EmployeePosition.PositionName == "Program Manager") && _presenter.CurrentMaintenanceRequest.CurrentLevel == 1)
                {
                    will = "Approve";
                    break;

                }

               
                   
                else if (_presenter.GetUser(_presenter.CurrentMaintenanceRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName)
                {
                    will = AL.Will;
                }

            }
            return will;
        }
        private void PopProgressStatus()
        {
            string[] s = Enum.GetNames(typeof(ProgressStatus));

            for (int i = 0; i < s.Length; i++)
            {
                ddlProgressStatus.Items.Add(new ListItem(s[i].Replace('_', ' '), s[i].Replace('_', ' ')));

            }


        }
        private void BindMaintenanceRequestStatus()
        {
            foreach (MaintenanceRequestStatus MRS in _presenter.CurrentMaintenanceRequest.MaintenanceRequestStatuses)
            {
                if (MRS.WorkflowLevel == _presenter.CurrentMaintenanceRequest.CurrentLevel && _presenter.CurrentMaintenanceRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    ddlApprovalStatus.SelectedValue = MRS.ApprovalStatus;
                    txtRejectedReason.Text = MRS.RejectedReason;
                    btnApprove.Enabled = true;

                }
                if (_presenter.CurrentMaintenanceRequest.CurrentLevel == _presenter.CurrentMaintenanceRequest.MaintenanceRequestStatuses.Count && MRS.ApprovalStatus != null)
                {
                    btnPrint.Enabled = true;
                   // btnPurchaseOrder.Enabled = true;
                    btnApprove.Enabled = false;
                    
                }
                else
                {
                    btnPrint.Enabled = false;
                  //  btnPurchaseOrder.Enabled = false;
                    btnApprove.Enabled = true;
                }

            }
        }
        private void BindMaintenanceRequestforprint()
        {
            lblRequestNoresult.Text = _presenter.CurrentMaintenanceRequest.RequestNo;
            lblRequestedDateresult.Text = _presenter.CurrentMaintenanceRequest.RequestDate.ToShortDateString();
            lblRequesterres.Text = _presenter.GetUser(_presenter.CurrentMaintenanceRequest.Requester).FullName;

            grvSoleDetailsPrint.DataSource = _presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails;
            grvSoleDetailsPrint.DataBind();

            grvStatuses.DataSource = _presenter.CurrentMaintenanceRequest.MaintenanceRequestStatuses;
            grvStatuses.DataBind();
        }
        private void BindSearchMaintenanceGrid()
        {
            grvMaintenanceRequestList.DataSource = _presenter.ListMaintenanceRequests(txtRequestNosearch.Text, txtRequestDatesearch.Text, ddlProgressStatus.SelectedValue);
            grvMaintenanceRequestList.DataBind();
        }
        private void ShowPrint()
        {
            if (_presenter.CurrentMaintenanceRequest.CurrentLevel == _presenter.CurrentMaintenanceRequest.MaintenanceRequestStatuses.Count && _presenter.CurrentMaintenanceRequest.ProgressStatus == ProgressStatus.Completed.ToString())
            {
                btnPrint.Enabled = true;
              //  btnPurchaseOrder.Enabled = true;
                SendEmailToRequester();

            }

        }
        private void SendEmail(MaintenanceRequestStatus MRS)
        {
            if (_presenter.GetUser(MRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(_presenter.GetUser(MRS.Approver).Email, "Maintenance Request", (_presenter.GetUser(_presenter.CurrentMaintenanceRequest.AppUser.Id).FullName).ToUpper() + " Requests for Maintenance Request No. - '" + (_presenter.CurrentMaintenanceRequest.RequestNo).ToUpper() + "'");
            }
            else
            {
                EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(MRS.Approver).AssignedTo).Email, "Maintenance Request", (_presenter.GetUser(_presenter.CurrentMaintenanceRequest.AppUser.Id).FullName).ToUpper() + " Requests for Maintenance with  Request No. - '" + (_presenter.CurrentMaintenanceRequest.RequestNo).ToUpper() + "'");
            }
        }
        private void SendEmailRejected(MaintenanceRequestStatus MRS)
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentMaintenanceRequest.AppUser.Id).Email, "Maintenance Request Rejection", "Your Maintenance Request with Sole Vendor Request No. " + (_presenter.CurrentMaintenanceRequest.RequestNo).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (MRS.RejectedReason).ToUpper() + "'");
            

            if (MRS.WorkflowLevel > 1)
            {
                for (int i = 0; i + 1 < MRS.WorkflowLevel; i++)
                {
                    EmailSender.Send(_presenter.GetUser(_presenter.CurrentMaintenanceRequest.MaintenanceRequestStatuses[i].Approver).Email, "Maintenance Request Rejection", "Maintenance  Request with Maintenance Request No. - " + (_presenter.CurrentMaintenanceRequest.RequestNo).ToUpper() + " made by " + (_presenter.GetUser(_presenter.CurrentMaintenanceRequest.AppUser.Id).FullName).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (MRS.RejectedReason).ToUpper() + "'");
                   
                }
            }
        }
        private void SendEmailToRequester()
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentMaintenanceRequest.AppUser.Id).Email, "Maintenance Request ", "Your Maintenance Request with Maintenance Request No. - '" + (_presenter.CurrentMaintenanceRequest.RequestNo).ToUpper() + "' was Completed.");
        }
        private void GetNextApprover()
        {
            foreach (MaintenanceRequestStatus MRS in _presenter.CurrentMaintenanceRequest.MaintenanceRequestStatuses)
            {
                if (MRS.ApprovalStatus == null)
                {
                    SendEmail(MRS);
                    _presenter.CurrentMaintenanceRequest.CurrentApprover = MRS.Approver;
                    _presenter.CurrentMaintenanceRequest.CurrentLevel = MRS.WorkflowLevel;
                    _presenter.CurrentMaintenanceRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                    _presenter.CurrentMaintenanceRequest.CurrentStatus = MRS.ApprovalStatus;
                    break;

                }
            }
        }
        private void SaveMaintenanceRequestStatus()
        {
            foreach (MaintenanceRequestStatus MRS in _presenter.CurrentMaintenanceRequest.MaintenanceRequestStatuses)
            {
                if ((MRS.Approver == _presenter.CurrentUser().Id || _presenter.CurrentUser().Id == (_presenter.GetAssignedJobbycurrentuser(MRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(MRS.Approver).AssignedTo : 0)) && MRS.WorkflowLevel == _presenter.CurrentMaintenanceRequest.CurrentLevel)
                {
                    MRS.Date = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    MRS.ApprovalStatus = ddlApprovalStatus.SelectedValue;
                    MRS.RejectedReason = txtRejectedReason.Text;
                    MRS.AssignedBy = _presenter.GetAssignedJobbycurrentuser(MRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(MRS.Approver).AppUser.FullName : "";
                 
                    if (MRS.ApprovalStatus != ApprovalStatus.Rejected.ToString())
                    {
                        if (_presenter.CurrentMaintenanceRequest.CurrentLevel == _presenter.CurrentMaintenanceRequest.MaintenanceRequestStatuses.Count)
                        {
                            _presenter.CurrentMaintenanceRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                            _presenter.CurrentMaintenanceRequest.CurrentStatus = MRS.ApprovalStatus;
                            MRS.Approver = _presenter.CurrentUser().Id;
                            _presenter.CurrentMaintenanceRequest.CurrentLevel = MRS.WorkflowLevel;
                            SendEmailToRequester();
                            //  SendCompletedEmail(MRS);
                            break;
                        }
                        else
                        {
                            GetNextApprover();
                        }
                        // _presenter.CurrentMaintenanceRequest.CurrentStatus = MRS.ApprovalStatus;
                        //GetNextApprover();
                    }
                 
                    else
                    {
                        _presenter.CurrentMaintenanceRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        _presenter.CurrentMaintenanceRequest.CurrentStatus = MRS.ApprovalStatus;
                        MRS.Approver = _presenter.CurrentUser().Id;
                        SendEmailRejected(MRS);
                    }
                    break;
                }
            

        }

        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentMaintenanceRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    SaveMaintenanceRequestStatus();
                    _presenter.SaveOrUpdateMaintenanceRequest(_presenter.CurrentMaintenanceRequest);
                    ShowPrint();
                    if (ddlApprovalStatus.SelectedValue != "Rejected")
                    {
                        Master.ShowMessage(new AppMessage("Maintenance Approval Processed", RMessageType.Info));
                    }
                    else
                    {
                        Master.ShowMessage(new AppMessage("Maintenance Approval Rejected", RMessageType.Info));
                    }

                    btnApprove.Enabled = false;
                    BindSearchMaintenanceGrid();
                    pnlApproval_ModalPopupExtender.Show();
                }
              
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }

        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindSearchMaintenanceGrid();
            // pnlPopUpSearch_ModalPopupExtender.Show();
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Default.aspx");
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            pnlApproval.Visible = false;
            pnlApproval_ModalPopupExtender.Hide();
        }
        protected void btnCancelPopup2_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
        }
        protected void btnPurchaseOrder_Click(object sender, EventArgs e)
        {
            int purchaseID = _presenter.CurrentMaintenanceRequest.Id;
            Response.Redirect(String.Format("frmPurchaseOrderSoleVendor.aspx?SoleVendorRequestId={0}", purchaseID));
        }
        protected void ddlApprovalStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlApprovalStatus.SelectedValue == "Rejected")
            {
                lblRejectedReason.Visible = true;
                txtRejectedReason.Visible = true;
                rfvRejectedReason.Enabled = true;
            }
            pnlApproval_ModalPopupExtender.Show();
        }
        protected void ddlEdtAccountDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            TextBox txtAccountCode = ddl.FindControl("txtEdtAccountCode") as TextBox;
            txtAccountCode.Text = _presenter.GetItemAccount(Convert.ToInt32(ddl.SelectedValue)).AccountCode;
            pnlDetail_ModalPopupExtender.Show();
        }
        protected void DownloadFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            Response.End();
        }
        protected void grvMaintenanceRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {

            _presenter.OnViewLoaded();
            PopApprovalStatus();
            BindMaintenanceRequestStatus();
           
            
            txtRejectedReason.Visible = false;
            rfvRejectedReason.Enabled = false;
            pnlApproval_ModalPopupExtender.Show();

        }
        protected void grvMaintenanceRequestList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {


        }
        protected void grvMaintenanceRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            /* Button btnStatus = e.Row.FindControl("btnStatus") as Button;
             LeaveRequest LR = e.Row.DataItem as LeaveRequest;
             if (LR != null)
             {
                 if (e.Row.RowType == DataControlRowType.DataRow)
                 {

                     if (e.Row.RowType == DataControlRowType.DataRow)
                     {
                         e.Row.Cells[1].Text = _presenter.GetUser(LR.Requester).FullName;
                     }
                 }
                 if (LR.ProgressStatus == ProgressStatus.InProgress.ToString())
                 {
                     btnStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFF6C");

                 }
                 else if (LR.ProgressStatus == ProgressStatus.Completed.ToString())
                 {
                     btnStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF7251");

                 }
             }*/


           // BindServiceTypeDetails

        }

        private void BindServiceTypeDetails(DropDownList ddlServiceTypeDet, string serviceTypename)
        {
            ddlServiceTypeDet.Items.Clear();
            ListItem lst = new ListItem();
            lst.Text = "Select Service Type Details";
            lst.Value = "";
            ddlServiceTypeDet.DataValueField = "Id";
            ddlServiceTypeDet.DataTextField = "Description";
            ddlServiceTypeDet.Items.Add(lst);
            ddlServiceTypeDet.DataSource = _presenter.GetServiceTypeDetbyname(serviceTypename);
            ddlServiceTypeDet.DataBind();

        }
        protected void grvMaintenanceRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvMaintenanceRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void grvMaintenanceRequestList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                reqID = (int)grvMaintenanceRequestList.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;
                Session["ReqID"] = reqID;
                _presenter.CurrentMaintenanceRequest = _presenter.GetMaintenanceRequestById(reqID);
                if (e.CommandName == "ViewItem")
                {

                    dgMaintenanceRequestDetail.DataSource = _presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails;
                    dgMaintenanceRequestDetail.DataBind();
                    pnlDetail_ModalPopupExtender.Show();
                }
            }
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentMaintenanceRequest.MaintenanceRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_presenter.GetUser(_presenter.CurrentMaintenanceRequest.MaintenanceRequestStatuses[e.Row.RowIndex].Approver) != null)
                    {
                        e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentMaintenanceRequest.MaintenanceRequestStatuses[e.Row.RowIndex].Approver).FullName;
                    }
                }
            }
        }
        private void BindServiceType(DropDownList ddlServiceType)
        {
            ddlServiceType.DataSource = _presenter.GetServiceTypes();
            ddlServiceType.DataBind();

        }
        private void BindServiceTypeDetails(DropDownList ddlServiceTypeDet, int serviceTypeId)
        {
            ddlServiceTypeDet.Items.Clear();
            ListItem lst = new ListItem();
            lst.Text = "Select Service Type Details";
            lst.Value = "";
            ddlServiceTypeDet.Items.Add(lst);
            ddlServiceTypeDet.DataSource = _presenter.GetServiceTypeDetbyTypeId(serviceTypeId);
            ddlServiceTypeDet.DataBind();

        }
        protected void dgMaintenanceRequestDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlFServiceType = e.Item.FindControl("ddlFServiceTpe") as DropDownList;
                BindServiceType(ddlFServiceType);
                DropDownList ddlFServiceTypeDet = e.Item.FindControl("ddlMecServiceTypeDet") as DropDownList;
                BindServiceTypeDetails(ddlFServiceTypeDet, Convert.ToInt32(ddlFServiceType.SelectedValue));
                //DropDownList ddlFMecServiceTypeDet = e.Item.FindControl("ddlMecServiceTypeDet") as DropDownList;
                //BindServiceTypeDetails(ddlFMecServiceTypeDet, Convert.ToInt32(ddlFServiceType.SelectedValue));
            }
            else
            {

                if (_presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails != null)
                {
                   
                    DropDownList ddlEdtMechanic = e.Item.FindControl("ddlEdtMechanicServiceTypeDetail") as DropDownList;
                    if (ddlEdtMechanic != null)
                    {
                        DropDownList ddlServiceType = e.Item.FindControl("ddlServiceTpe") as DropDownList;
                        BindServiceType(ddlServiceType);
                        BindServiceTypeDetails(ddlEdtMechanic, _presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails[e.Item.DataSetIndex].ServiceType.Name);
                        //if (_presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails[e.Item.DataSetIndex].MechanicServiceTypeDetail != null)
                        //{
                            
                            ListItem liI = ddlEdtMechanic.Items.FindByValue(_presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails[e.Item.DataSetIndex].DriverServiceTypeDetail.Description);
                            if (liI != null)
                                liI.Selected = true;
                        //}
                        //else
                        //{
                        //    DropDownList ddlEdtMechanicDet = e.Item.FindControl("ddlEdtMechanicServiceTypeDetail") as DropDownList;
                        //    BindServiceTypeDetails(ddlEdtMechanicDet, _presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails[e.Item.DataSetIndex].ServiceType.Name);
                        //}

                    }
                }
            }
        }

        protected void dgMaintenanceRequestDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgMaintenanceRequestDetail.EditItemIndex = e.Item.ItemIndex;
            dgMaintenanceRequestDetail.DataSource = _presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails;
            dgMaintenanceRequestDetail.DataBind();
            pnlDetail_ModalPopupExtender.Show();
        }

        protected void dgMaintenanceRequestDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int CPRDId = (int)dgMaintenanceRequestDetail.DataKeys[e.Item.ItemIndex];
            MaintenanceRequestDetail cprd;

            if (CPRDId > 0)
                cprd = _presenter.CurrentMaintenanceRequest.GetMaintenanceRequestDetail(CPRDId);
            else
                cprd = (MaintenanceRequestDetail)_presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails[e.Item.ItemIndex];

            try
            {
               

               
                // cprd.MaintenanceRequest = _presenter.CurrentMaintenanceRequest;
                TextBox txtEdtTechRemark = e.Item.FindControl("txtEdtTechnicianRemark") as TextBox;
                cprd.TechnicianRemark = txtEdtTechRemark.Text;
                DropDownList ddlTechServiceTypeDetail = e.Item.FindControl("ddlEdtMechanicServiceTypeDetail") as DropDownList;
                cprd.MechanicServiceTypeDetail = _presenter.GetServiceTypeDetail(Convert.ToInt32(ddlTechServiceTypeDetail.SelectedValue));
                cprd.MaintenanceRequest = _presenter.CurrentMaintenanceRequest;
                dgMaintenanceRequestDetail.EditItemIndex = -1;
                dgMaintenanceRequestDetail.DataSource = _presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails;
                dgMaintenanceRequestDetail.DataBind();
                pnlDetail_ModalPopupExtender.Show();
                Master.ShowMessage(new AppMessage("Car Maintenance Request Detail Successfully Updated", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Car Maintenance Detail. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        private void BindMaintenanceRequestDetails()
        {
            dgMaintenanceRequestDetail.DataSource = _presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails;
            dgMaintenanceRequestDetail.DataBind();
        }
        protected void dgMaintenanceRequestDetail_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "AddNew")
            {

                try
                {
                    MaintenanceRequestDetail Detail = new MaintenanceRequestDetail();
                    DropDownList ddlFServiceTpe = e.Item.FindControl("ddlFServiceTpe") as DropDownList;

                    Detail.ServiceType = _presenter.GetServiceType(int.Parse(ddlFServiceTpe.SelectedValue));
                    DropDownList ddlFServiceTypeDetail = e.Item.FindControl("ddlMecServiceTypeDet") as DropDownList;
                    Detail.MechanicServiceTypeDetail = _presenter.GetServiceTypeDetail(int.Parse(ddlFServiceTypeDetail.SelectedValue));
                    //DropDownList ddlFMeServiceTypeDetail = e.Item.FindControl("ddlMeServiceTypeDet") as DropDownList;
                    //Detail.MechanicServiceType = _presenter.GetServiceTypeDetail(int.Parse(ddlFMeServiceTypeDetail.SelectedValue));
                    //TextBox txtFRemark = e.Item.FindControl("txtFRemark") as TextBox;
                    //Detail.TechnicianRemark = txtFRemark.Text;                   

                    // cprd.MaintenanceRequest = _presenter.CurrentMaintenanceRequest;
                    TextBox txtEdtTechRemark = e.Item.FindControl("txtFRemark") as TextBox;
                    Detail.TechnicianRemark = txtEdtTechRemark.Text;
                  
                    _presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails.Add(Detail);
                    Master.ShowMessage(new AppMessage("Maintenance Request Detail added successfully.", RMessageType.Info));
                    dgMaintenanceRequestDetail.EditItemIndex = -1;
                    dgMaintenanceRequestDetail.DataSource = _presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails;
                    dgMaintenanceRequestDetail.DataBind();
                    pnlDetail_ModalPopupExtender.Show();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to  Add Car Maintenance Detail. " + ex.Message, RMessageType.Error));
                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
                }
            }
        }

        protected void ddlFServiceTpe_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlServiceTypeDetail = ddl.FindControl("ddlMecServiceTypeDet") as DropDownList;
            BindServiceTypeDetails(ddlServiceTypeDetail, Convert.ToInt32(ddl.SelectedValue));
            pnlDetail_ModalPopupExtender.Show();
        }



        protected void ddlServiceTpe_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlServiceTypeDetail = ddl.FindControl("ddlEdtMechanicServiceTypeDetail") as DropDownList;
            BindServiceTypeDetails(ddlServiceTypeDetail, Convert.ToInt32(ddl.SelectedValue));
            pnlDetail_ModalPopupExtender.Show();
        }
    }
}