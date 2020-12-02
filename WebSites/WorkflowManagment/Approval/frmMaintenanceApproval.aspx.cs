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
                BindMaintenanceSpareparts();
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



                //else if (_presenter.GetUser(_presenter.CurrentMaintenanceRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName)
                //{
                //    will = AL.Will;
                //}


                else
                {
                    try
                    {
                        if (_presenter.GetUser(_presenter.CurrentMaintenanceRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName && AL.WorkflowLevel == _presenter.CurrentMaintenanceRequest.CurrentLevel)
                        {
                            will = AL.Will;
                            break;
                        }
                    }
                    catch
                    {
                        if (_presenter.GetUser(_presenter.CurrentMaintenanceRequest.CurrentApprover).EmployeePosition.Id == AL.EmployeePosition.Id && AL.WorkflowLevel == _presenter.CurrentMaintenanceRequest.CurrentLevel)
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
        private void BindItem(DropDownList ddlItem)
        {
            ddlItem.DataSource = _presenter.GetSpareParts();
            ddlItem.DataBind();
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
        private void ShowPrint()
        {
            if (_presenter.CurrentMaintenanceRequest.CurrentLevel == _presenter.CurrentMaintenanceRequest.MaintenanceRequestStatuses.Count && _presenter.CurrentMaintenanceRequest.ProgressStatus == ProgressStatus.Completed.ToString())
            {
                btnPrint.Enabled = true;
                //  btnPurchaseOrder.Enabled = true;
                SendEmailToRequester();

            }

        }

        private void SendEmailtoMechanic()
        {
                   string message = "Your Reviewed Car Maintenance Request By " + (_presenter.CurrentMaintenanceRequest.AppUser.FullName).ToUpper() + " and Request Number :   '" + (_presenter.CurrentMaintenanceRequest.RequestNo).ToUpper() + " is Approved :   '";
                    EmailSender.Send(_presenter.GetMechanic().Email, "Maintenance Request ", message);
                    Log.Info((_presenter.GetMechanic().FullName).ToUpper() + " has Maintained a Maintenance Request made by " + _presenter.CurrentMaintenanceRequest.AppUser.FullName);
             
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
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentMaintenanceRequest.AppUser.Id).Email, "Maintenance Request ", "Your Maintenance Request with Maintenance Request No. - '" + (_presenter.CurrentMaintenanceRequest.RequestNo).ToUpper() + "' is Completed.");
        }

        private void SendEmailToRequesterForPurchase()
        {

            int x = _presenter.CurrentMaintenanceRequest.MaintenanceSpareParts.Count;

            if (x > 0)
            {
                string itemsPurchased = string.Empty;
                foreach (MaintenanceSparePart toBePurchased in _presenter.CurrentMaintenanceRequest.MaintenanceSpareParts)
                {

                    itemsPurchased = toBePurchased.Item.Name + "," + itemsPurchased;



                }
                EmailSender.Send(_presenter.GetUser(_presenter.CurrentMaintenanceRequest.AppUser.Id).Email, "Maintenance Request For Purchase", "Your Car Maintenance Request with Maintenance Request No.- '" + (_presenter.CurrentMaintenanceRequest.RequestNo).ToUpper() + "' is In Progress and You have to Request for Purchase Item '" + itemsPurchased.ToString() + "'");
            }

            else
            {
                EmailSender.Send(_presenter.GetUser(_presenter.CurrentMaintenanceRequest.AppUser.Id).Email, "Maintenance Request ", "Your Maintenance Request with Maintenance Request No. - '" + (_presenter.CurrentMaintenanceRequest.RequestNo).ToUpper() + "' is in a review Process.");
            }


            
        }
        private void GetNextApprover()
        {
            foreach (MaintenanceRequestStatus MRS in _presenter.CurrentMaintenanceRequest.MaintenanceRequestStatuses)
            {
                if (MRS.ApprovalStatus == null)
                {
                   
                       // SendEmailtoMechanic();
                   
                  
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
                           // SendEmailToRequesterForPurchase();
                            //  SendCompletedEmail(MRS);
                            break;
                        }
                        else
                        {
                            GetNextApprover();
                           // SendEmail(MRS);
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
                bool mechanicApproved = true;
                if (_presenter.CurrentMaintenanceRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    if (_presenter.CurrentUser().EmployeePosition.PositionName == "Driver/Mechanic")
                    {
                        foreach (MaintenanceRequestDetail mrd in _presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails)
                        {
                            if (mrd.MechanicServiceTypeDetail == null)
                            {
                                mechanicApproved = false;
                            }
                        }
                    }

                    if (mechanicApproved)
                    {
                        SaveMaintenanceRequestStatus();
                      
                        _presenter.SaveOrUpdateMaintenanceRequest(_presenter.CurrentMaintenanceRequest);
                       // SendEmailtoMechanic();
                        ShowPrint();
                        if (ddlApprovalStatus.SelectedValue != "Rejected")
                        {
                            if (ddlApprovalStatus.SelectedValue == "Approved")
                            {
                                Master.ShowMessage(new AppMessage("Maintenance Approval Processed", RMessageType.Info));
                                SendEmailtoMechanic();
                                // SendEmailToRequester();
                                SendEmailToRequesterForPurchase();
                            }
                            else
                            {
                                Master.ShowMessage(new AppMessage("Maintenance Approval Processed", RMessageType.Info));
                            }
                        }
                        else
                        {
                            Master.ShowMessage(new AppMessage("Maintenance Approval Rejected", RMessageType.Info));
                        }

                        btnApprove.Enabled = false;
                        BindSearchMaintenanceGrid();
                        pnlApproval_ModalPopupExtender.Show();
                    }
                    else
                    {
                        Master.ShowMessage(new AppMessage("Please update the Mechanic Review Section!", RMessageType.Error));
                    }

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
            ScriptManager.RegisterStartupScript(this, GetType(), "showMechanicDetail", "showMechanicDetail();", true);
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
            //e.Row.Cells[8].Visible = false;
            MaintenanceRequest MR = e.Row.DataItem as MaintenanceRequest;
            if (_presenter.CurrentMaintenanceRequest.MaintenanceRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if(_presenter.CurrentUser().EmployeePosition.PositionName == "Driver/Mechanic" && MR.ProgressStatus == ProgressStatus.Completed.ToString())
                    {

                        if (MR.MaintenanceStatus == "Maintained")
                        {
                           
                                e.Row.Cells[9].Visible = false;
                                e.Row.Cells[6].Visible = true;
                                e.Row.Cells[7].Visible = false;
                           
                        }
                        else if (MR.MaintenanceStatus != "Maintained")
                        {


                            e.Row.Cells[9].Visible = true;
                            e.Row.Cells[6].Visible = true;
                            e.Row.Cells[7].Visible = false;
                        }
                       
                    }
                    else if (_presenter.CurrentUser().EmployeePosition.PositionName == "Driver/Mechanic" && MR.ProgressStatus != ProgressStatus.Completed.ToString())
                    {
                       

                            e.Row.Cells[9].Visible = false;
                            e.Row.Cells[6].Visible = true;
                            e.Row.Cells[7].Visible = false;
                       

                    }
                    else
                    {
                      

                            e.Row.Cells[9].Visible = false;
                            e.Row.Cells[6].Visible = false;
                            e.Row.Cells[7].Visible = true;
                        

                    }
                    e.Row.Cells[1].Text = _presenter.GetUser(MR.Requester).FullName;
                }
            }
           
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
                    ScriptManager.RegisterStartupScript(this, GetType(), "showMechanicDetail", "showMechanicDetail();", true);
                }
                if (e.CommandName == "ViewItemPrev")
                {
                    grvPreviewDetail.DataSource = _presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails;
                    grvPreviewDetail.DataBind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showApproverDetail", "showApproverDetail();", true);
                }
                if (e.CommandName == "Maintained")
                {
                    if (_presenter.CurrentMaintenanceRequest.MaintenanceStatus != "Maintained")
                    {
                        _presenter.CurrentMaintenanceRequest.MaintenanceStatus = "Maintained";
                        grvPreviewDetail.DataBind();
                        _presenter.SaveOrUpdateMaintenanceRequest(_presenter.CurrentMaintenanceRequest);
                        SendEmailToRequester();
                        LoadData(Convert.ToInt32(e.CommandArgument));
                    }
                    else
                    {
                        LoadData(Convert.ToInt32(e.CommandArgument));
                    }

                }
            }
        }
        private void LoadData(int? rowNumber = null)
        {
            //if rowNumber is null use GridView1.SelectedIndex
            var index = rowNumber ?? grvMaintenanceRequestList.SelectedIndex;

            //Populate the input box with the value of selected row.
            GridViewRow gr = grvMaintenanceRequestList.Rows[index];

            gr.Cells[8].Visible = false;
        }
        private void LoadDataforMech(int? rowNumber = null)
        {
            //if rowNumber is null use GridView1.SelectedIndex
            var index = rowNumber ?? grvMaintenanceRequestList.SelectedIndex;

            //Populate the input box with the value of selected row.
            GridViewRow gr = grvMaintenanceRequestList.Rows[index];

            gr.Cells[8].Visible = true;
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
            ScriptManager.RegisterStartupScript(this, GetType(), "showMechanicDetail", "showMechanicDetail();", true);
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
                DropDownList ddlStatus = e.Item.FindControl("ddlstatus") as DropDownList;

                cprd.MaintenanceRequest = _presenter.CurrentMaintenanceRequest;
                dgMaintenanceRequestDetail.EditItemIndex = -1;
                dgMaintenanceRequestDetail.DataSource = _presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails;
                dgMaintenanceRequestDetail.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "showMechanicDetail", "showMechanicDetail();", true);
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
                    ScriptManager.RegisterStartupScript(this, GetType(), "showMechanicDetail", "showMechanicDetail();", true);
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
            ScriptManager.RegisterStartupScript(this, GetType(), "showMechanicDetail", "showMechanicDetail();", true);
        }
        protected void ddlServiceTpe_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlServiceTypeDetail = ddl.FindControl("ddlEdtMechanicServiceTypeDetail") as DropDownList;
            BindServiceTypeDetails(ddlServiceTypeDetail, Convert.ToInt32(ddl.SelectedValue));
            ScriptManager.RegisterStartupScript(this, GetType(), "showMechanicDetail", "showMechanicDetail();", true);
        }
        #region MaintenanceSparepart
        private void BindMaintenanceSpareparts()
        {
            dgSparepart.DataSource = _presenter.CurrentMaintenanceRequest.MaintenanceSpareParts;
            dgSparepart.DataBind();
        }
        protected void dgSparepart_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            dgSparepart.EditItemIndex = -1;
            BindMaintenanceSpareparts();
        }

        protected void dgSparepart_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgSparepart.DataKeys[e.Item.ItemIndex];
            int PRDId = (int)dgSparepart.DataKeys[e.Item.ItemIndex];
            MaintenanceSparePart prd;

            if (PRDId > 0)
                prd = _presenter.CurrentMaintenanceRequest.GetMaintenanceSparePart(PRDId);
            else
                prd = _presenter.CurrentMaintenanceRequest.MaintenanceSpareParts[e.Item.ItemIndex];
            try
            {
                if (PRDId > 0)
                {
                    _presenter.CurrentMaintenanceRequest.RemoveMaintenanceSparePart(id);
                    _presenter.DeleteMaintenanceSparepart(_presenter.GetMaintenanceSparePart(id));
                    //  _presenter.CurrentMaintenanceRequest.TotalPrice = _presenter.CurrentMaintenanceRequest.TotalPrice - prd.EstimatedCost;
                    //  txtTotal.Text = (_presenter.CurrentMaintenanceRequest.TotalPrice).ToString();
                    _presenter.SaveOrUpdateMaintenanceRequest(_presenter.CurrentMaintenanceRequest);
                }
                else
                {
                    _presenter.CurrentMaintenanceRequest.MaintenanceSpareParts.Remove(prd);
                    //  _presenter.CurrentMaintenanceRequest.TotalPrice = _presenter.CurrentMaintenanceRequest.TotalPrice - prd.EstimatedCost;
                    //  txtTotal.Text = (_presenter.CurrentMaintenanceRequest.TotalPrice).ToString();
                }
                BindMaintenanceSpareparts();

                Master.ShowMessage(new AppMessage("Maintenance Sparepart was Removed Successfully", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Maintenance Sparepart. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }

        protected void dgSparepart_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgSparepart.EditItemIndex = e.Item.ItemIndex;
            BindMaintenanceSpareparts();
        }

        protected void dgSparepart_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlFItem = e.Item.FindControl("ddlFItem") as DropDownList;
                BindItem(ddlFItem);


            }
            else
            {
                if (_presenter.CurrentMaintenanceRequest.MaintenanceSpareParts != null)
                {
                    DropDownList ddlItem = e.Item.FindControl("ddlItem") as DropDownList;

                    if (ddlItem != null)
                    {
                        BindItem(ddlItem);

                        if (_presenter.CurrentMaintenanceRequest.MaintenanceSpareParts[e.Item.DataSetIndex].Item != null)
                        {
                            ListItem li = ddlItem.Items.FindByValue(_presenter.CurrentMaintenanceRequest.MaintenanceSpareParts[e.Item.DataSetIndex].Item.Id.ToString());
                            if (li != null)
                                li.Selected = true;
                        }
                    }



                }
            }
        }
        protected void dgSparepart_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgSparepart.DataKeys[e.Item.ItemIndex];
            MaintenanceSparePart Spare;
            if (id > 0)
                Spare = _presenter.CurrentMaintenanceRequest.GetMaintenanceSparePart(id);
            else
                Spare = _presenter.CurrentMaintenanceRequest.MaintenanceSpareParts[e.Item.ItemIndex];
            try
            {

                DropDownList ddlFItem = e.Item.FindControl("ddlItem") as DropDownList;
                Spare.Item = _presenter.GetItem(int.Parse(ddlFItem.SelectedValue));
                //DropDownList ddlReturned = e.Item.FindControl("ddlEdtReturned") as DropDownList;
                //Spare.Returned = ddlReturned.SelectedValue;
                //TextBox txtDate = e.Item.FindControl("txtEdtReturnedDate") as TextBox;
                //Spare.ReturnedDate = Convert.ToDateTime(txtDate.Text);
                //TextBox txtFRemark = e.Item.FindControl("txtRemark") as TextBox;
                //Spare.StoreKeeperRemark = txtFRemark.Text;
                Spare.MaintenanceRequest = _presenter.CurrentMaintenanceRequest;
                Master.ShowMessage(new AppMessage("Maintenance Sparepart  Updated successfully.", RMessageType.Info));
                dgSparepart.EditItemIndex = -1;
                BindMaintenanceSpareparts();
            }

            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Maintenance Sparepart. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgSparepart_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "AddNew")
            {
                try
                {
                    MaintenanceSparePart Spare = new MaintenanceSparePart();
                    DropDownList ddlFItem = e.Item.FindControl("ddlFItem") as DropDownList;
                    Spare.Item = _presenter.GetItem(int.Parse(ddlFItem.SelectedValue));
                    //DropDownList ddlReturned = e.Item.FindControl("ddlReturned") as DropDownList;
                    //Spare.Returned = ddlReturned.SelectedValue;
                    //TextBox txtDate = e.Item.FindControl("txtReturnedDate") as TextBox;
                    //Spare.ReturnedDate = Convert.ToDateTime(txtDate.Text);
                    //TextBox txtFRemark = e.Item.FindControl("txtFRemark") as TextBox;
                    //Spare.StoreKeeperRemark = txtFRemark.Text;
                    _presenter.CurrentMaintenanceRequest.MaintenanceSpareParts.Add(Spare);
                    Master.ShowMessage(new AppMessage("Maintenance Sparepart  added successfully.", RMessageType.Info));
                    dgSparepart.EditItemIndex = -1;
                    BindMaintenanceSpareparts();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showMechanicDetail", "showMechanicDetail();", true);
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Maintenance Sparepart. " + ex.Message, RMessageType.Error));
                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
                }
            }
        }
        #endregion
    }

}