using Chai.WorkflowManagment.CoreDomain.Requests;
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

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public partial class frmMaintenanceRequest : POCBasePage, IMaintenanceRequestView
    {
        private MaintenaceRequestPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        private MaintenanceRequest _maintenancerequest;
        private int _leaverequestId = 0;
        private int _totalprice = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //CheckApprovalSettings();
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                BindSearchMaintenanceRequestGrid();
                BindMaintenanceRequestDetails();
                BindInitialValues();
                PopInternalVehicles();
            }
            lblPlate.Visible = false;
            ddlPlate.Visible = false;
            this._presenter.OnViewLoaded();



        }

        [CreateNew]
        public MaintenaceRequestPresenter Presenter
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
                return "{334AAED8-456F-44AC-A203-FC4CE87FC3CD}";
            }
        }
        private void CheckApprovalSettings()
        {
            if (_presenter.GetApprovalSetting(RequestType.Purchase_Request.ToString().Replace('_', ' '), 0) == null)
            {
                pnlWarning.Visible = true;
            }
        }
        private void BindInitialValues()
        {
            AppUser CurrentUser = _presenter.CurrentUser();
            txtRequester.Text = CurrentUser.FirstName + " " + CurrentUser.LastName;

            if (_presenter.CurrentMaintenanceRequest.Id <= 0)
            {
                AutoNumber();
                txtRequestDate.Text = DateTime.Today.Date.ToShortDateString();

            }
        }
        private string AutoNumber()
        {
            return "PR-" + (_presenter.GetLastMaintenanceRequestId() + 1).ToString();
        }
        private void BindMaintenanceRequest()
        {

            if (_presenter.CurrentMaintenanceRequest.Id > 0)
            {
                // txtRequestNo.Text = _presenter.CurrentMaintenanceRequest.RequestNo;
                txtRequestDate.Text = _presenter.CurrentMaintenanceRequest.RequestedDate.ToShortDateString();
                txtKMReading.Text = "";
              

            }
        }
        private void SaveMaintenanceRequest()
        {
            AppUser CurrentUser = _presenter.CurrentUser();
            try
            {
                _presenter.CurrentMaintenanceRequest.Requester = CurrentUser.Id;
                _presenter.CurrentMaintenanceRequest.RequestedDate = Convert.ToDateTime(txtRequestDate.Text);
                _presenter.CurrentMaintenanceRequest.RequestNo = AutoNumber();
                _presenter.CurrentMaintenanceRequest.KmReading = txtKMReading.Text;
                _presenter.CurrentMaintenanceRequest.Comment = "";
                

                
                _presenter.CurrentMaintenanceRequest.IsVehicle = GetIsVehicle;
                _presenter.CurrentMaintenanceRequest.PlateNo = GetPlateNo;
                //Determine total cost
                /*       decimal cost = 0;
                       if (_presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails.Count > 0)
                       {

                           foreach (MaintenanceRequestDetail detail in _presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails)
                           {
                               cost = cost + detail.EstimatedCost;
                           }
                       }
                       _presenter.CurrentMaintenanceRequest.TotalPrice = cost;*/
                //Determine total cost end
                SaveMaintenanceRequestStatus();
                GetCurrentApprover();
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
            }

        }
        private void SaveMaintenanceRequestStatus()
        {
            if (_presenter.CurrentMaintenanceRequest.Id <= 0)
            {
                if (_presenter.GetApprovalSettingforMaintenanceProcess(RequestType.Maintenance_Request.ToString().Replace('_', ' '), 0) != null)
                {
                    int i = 1;
                    foreach (ApprovalLevel AL in _presenter.GetApprovalSettingforMaintenanceProcess(RequestType.Maintenance_Request.ToString().Replace('_', ' '), 0).ApprovalLevels)
                    {
                        MaintenanceRequestStatus PRS = new MaintenanceRequestStatus();
                        PRS.MaintenanceRequest = _presenter.CurrentMaintenanceRequest;
                        if (AL.EmployeePosition.PositionName == "Superviser/Line Manager")
                        {
                            if (_presenter.CurrentUser().Superviser.Value != 0)
                            {
                                PRS.Approver = _presenter.CurrentUser().Superviser.Value;
                            }
                            else
                            {
                                PRS.ApprovalStatus = ApprovalStatus.Approved.ToString();
                                PRS.Date = DateTime.Today.Date;
                            }
                        }
                        else if (AL.EmployeePosition.PositionName == "Program Manager")
                        {
                            if (_presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails[0].Project.Id != 0)
                            {
                                PRS.Approver = _presenter.GetProject(_presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails[0].Project.Id).AppUser.Id;
                            }
                        }
                        else
                        {
                            PRS.Approver = _presenter.Approver(AL.EmployeePosition.Id).Id;
                        }
                        PRS.WorkflowLevel = i;
                        i++;
                        _presenter.CurrentMaintenanceRequest.MaintenanceRequestStatuses.Add(PRS);
                    }
                }
                else { pnlWarning.Visible = true; }
            }
        }
        private void GetCurrentApprover()
        {
            foreach (MaintenanceRequestStatus PRS in _presenter.CurrentMaintenanceRequest.MaintenanceRequestStatuses)
            {
                if (PRS.ApprovalStatus == null)
                {
                    SendEmail(PRS);
                    _presenter.CurrentMaintenanceRequest.CurrentApprover = PRS.Approver;
                    _presenter.CurrentMaintenanceRequest.CurrentLevel = PRS.WorkflowLevel;
                    _presenter.CurrentMaintenanceRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                    break;
                }
            }
        }
        private void SendEmail(MaintenanceRequestStatus PRS)
        {
            if (_presenter.GetSuperviser(PRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(_presenter.GetSuperviser(PRS.Approver).Email, "Maintenance Request", _presenter.GetUser(_presenter.CurrentMaintenanceRequest.Requester).FullName + "' Request for Car Maintenance Request No. '" + _presenter.CurrentMaintenanceRequest.RequestNo + "'");
            }
            else
            {
                EmailSender.Send(_presenter.GetSuperviser(_presenter.GetAssignedJobbycurrentuser(PRS.Approver).AssignedTo).Email, "Maintenance Request", _presenter.GetUser(_presenter.CurrentMaintenanceRequest.Requester).FullName + "' Request for Car Maintenance Request No. '" + _presenter.CurrentMaintenanceRequest.RequestNo + "'");
            }
        }

        private void PopInternalVehicles()
        {
            ddlPlate.DataSource = _presenter.GetVehicles();
            ddlPlate.DataBind();
        }
        public MaintenanceRequest MaintenanceRequest
        {
            get
            {
                return _maintenancerequest;
            }
            set
            {
                _maintenancerequest = value;
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

        public bool GetIsVehicle
        {
            get { return ckIsVehicle.Checked; }
        }
        public string GetPlateNo
        {
            get { return ddlPlate.SelectedValue; }
        }
        public int MaintenanceRequestId
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
        private void BindAccount(DropDownList ddlItemAccount)
        {
            ddlItemAccount.DataSource = _presenter.GetItemAccounts();
            ddlItemAccount.DataBind();

        }
        private void BindProject(DropDownList ddlProject)
        {
            ddlProject.DataSource = _presenter.GetProjects();
            ddlProject.DataBind();

        }
        private void BindGrant(DropDownList ddlGrant, int projectId)
        {
            ddlGrant.Items.Clear();
            ListItem lst = new ListItem();
            lst.Text = "Select Grant";
            lst.Value = "";
            ddlGrant.Items.Add(lst);
            ddlGrant.DataSource = _presenter.GetGrantbyprojectId(projectId);
            ddlGrant.DataBind();

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
            ddlServiceTypeDet.DataSource = _presenter.GetServiceTypeDetails(serviceTypeId);
            ddlServiceTypeDet.DataBind();

        }
        private void ClearForm()
        {
            //txtRequestNo.Text = "";
            txtRequestDate.Text = "";
            txtKMReading.Text = "";
            txtRemark.Text = "";
          


        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmMaintenanceRequest.aspx");
        }
        protected void grvMaintenanceRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["MaintenanceRequest"] = true;
            // ClearForm();
            //BindLeaveRequest();
            _leaverequestId = Convert.ToInt32(grvMaintenanceRequestList.SelectedDataKey[0]);
            _presenter.OnViewLoaded();
            BindMaintenanceRequest();
            BindMaintenanceRequestDetails();
            if (_presenter.CurrentMaintenanceRequest.CurrentLevel > 1)
            {
                btnRequest.Visible = false;
                btnDelete.Visible = false;
                dgMaintenanceRequestDetail.Columns[8].Visible = false;
            }
            else
            {
                btnRequest.Visible = true;
                btnDelete.Visible = true;
                dgMaintenanceRequestDetail.Columns[8].Visible = true;
            }
        }
        protected void grvMaintenanceRequestList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            _presenter.DeleteMaintenanceRequest(_presenter.GetMaintenanceRequestById(Convert.ToInt32(grvMaintenanceRequestList.DataKeys[e.RowIndex].Value)));

            btnFind_Click(sender, e);
            Master.ShowMessage(new AppMessage("Maintenance Request Successfully Deleted", Chai.WorkflowManagment.Enums.RMessageType.Info));

        }
        protected void grvMaintenanceRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //LeaveRequest leaverequest = e.Row.DataItem as LeaveRequest;
            //if (leaverequest != null)
            //{
            //    if (leaverequest.GetLeaveRequestStatusworkflowLevel(1).ApprovalStatus != null)
            //    {
            //        e.Row.Cells[5].Enabled = false;
            //        e.Row.Cells[6].Enabled = false;
            //    }

            //}
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //LinkButton db = (LinkButton)e.Row.Cells[5].Controls[0];
                //db.OnClientClick = "return confirm('Are you sure you want to delete this Recieve?');";
            }
        }
        protected void grvMaintenanceRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvMaintenanceRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
            ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
            //pnlSearch_ModalPopupExtender.Show();
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindSearchMaintenanceRequestGrid();
            ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
            //pnlSearch_ModalPopupExtender.Show();
        }
        private void BindSearchMaintenanceRequestGrid()
        {
            grvMaintenanceRequestList.DataSource = _presenter.ListMaintenanceRequests(txtRequestNosearch.Text, txtRequestDatesearch.Text);
            grvMaintenanceRequestList.DataBind();
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            _presenter.CancelPage();
        }
        protected void ddlFProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlFGrant = ddl.FindControl("ddlFGrant") as DropDownList;
            BindGrant(ddlFGrant, Convert.ToInt32(ddl.SelectedValue));
            if (ckIsVehicle.Checked == true)
            {
                lblPlate.Visible = true;


                ddlPlate.Visible = true;
                i.Visible = true;
            }
            else if (ckIsVehicle.Checked == false)
            {
                lblPlate.Visible = false;


                ddlPlate.Visible = false;
                i.Visible = false;
            }
        }
        protected void ddlGrant_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlGrant = ddl.FindControl("ddlGrant") as DropDownList;
            BindGrant(ddlGrant, Convert.ToInt32(ddl.SelectedValue));
            if (ckIsVehicle.Checked == true)
            {
                lblPlate.Visible = true;


                ddlPlate.Visible = true;
                i.Visible = true;
            }
            else if (ckIsVehicle.Checked == false)
            {
                lblPlate.Visible = false;


                ddlPlate.Visible = false;
                i.Visible = false;
            }
        }
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlGrant = ddl.FindControl("ddlGrant") as DropDownList;
            BindGrant(ddlGrant, Convert.ToInt32(ddl.SelectedValue));
            if (ckIsVehicle.Checked == true)
            {
                lblPlate.Visible = true;


                ddlPlate.Visible = true;
                i.Visible = true;
            }
            else if (ckIsVehicle.Checked == false)
            {
                lblPlate.Visible = false;


                ddlPlate.Visible = false;
                i.Visible = false;
            }
        }

        protected void ddlServiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlServiceTypeDetail = ddl.FindControl("ddlServiceTypeDetail") as DropDownList;
            BindGrant(ddlServiceTypeDetail, Convert.ToInt32(ddl.SelectedValue));
           
        }
        protected void ddlServiceTypeDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlServiceTypeDetail = ddl.FindControl("ddlServiceTypeDetail") as DropDownList;
            BindGrant(ddlServiceTypeDetail, Convert.ToInt32(ddl.SelectedValue));
          
        }
        protected void btnRequest_Click(object sender, EventArgs e)
        {
            try
            {
                SaveMaintenanceRequest();
                if (_presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails.Count != 0)
                {
                    if (_presenter.CurrentMaintenanceRequest.MaintenanceRequestStatuses.Count != 0)
                    {
                        _presenter.SaveOrUpdateLeaveMaintenance(_presenter.CurrentMaintenanceRequest);
                        ClearForm();
                        BindSearchMaintenanceRequestGrid();
                        Master.ShowMessage(new AppMessage("Successfully did a Maintenance Request, Reference No - <b>'" + _presenter.CurrentMaintenanceRequest.RequestNo + "'</b> ", Chai.WorkflowManagment.Enums.RMessageType.Info));
                        // Log.Info(_presenter.CurrentUser().FullName + " has requested for a Purchase of Total Price " + _presenter.CurrentMaintenanceRequest.TotalPrice);
                    }
                    else
                    {
                        Master.ShowMessage(new AppMessage("There is an error constracting Approval Process", Chai.WorkflowManagment.Enums.RMessageType.Error));

                    }

                }
                else
                {
                    Master.ShowMessage(new AppMessage("You have to insert at least one purchase item detail", Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {





            try
            {
                if (_presenter.CurrentMaintenanceRequest.CurrentStatus == null)
                {
                    _presenter.DeleteMaintenanceRequest(_presenter.CurrentMaintenanceRequest);
                    ClearForm();
                    Master.ShowMessage(new AppMessage("Maintenance Request Deleted ", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    BindSearchMaintenanceRequestGrid();
                    BindMaintenanceRequestDetails();
                    btnDelete.Visible = false;
                }
                else
                    Master.ShowMessage(new AppMessage("Warning: Unable to Delete Maintenance Request ", Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Warning: Unable to Delete Maintenance Request " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
      
        #region MaintenanceRequestDetail
        private void BindMaintenanceRequestDetails()
        {
            dgMaintenanceRequestDetail.DataSource = _presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails;
            dgMaintenanceRequestDetail.DataBind();
        }
        protected void dgMaintenanceRequestDetail_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            dgMaintenanceRequestDetail.EditItemIndex = -1;
            BindMaintenanceRequestDetails();
        }
        protected void dgMaintenanceRequestDetail_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgMaintenanceRequestDetail.DataKeys[e.Item.ItemIndex];
            int PRDId = (int)dgMaintenanceRequestDetail.DataKeys[e.Item.ItemIndex];
            MaintenanceRequestDetail prd;

            if (PRDId > 0)
                prd = _presenter.CurrentMaintenanceRequest.GetMaintenanceRequestDetail(PRDId);
            else
                prd = _presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails[e.Item.ItemIndex];
            try
            {
                if (PRDId > 0)
                {
                    _presenter.CurrentMaintenanceRequest.RemoveMaintenanceRequestDetail(id);
                    _presenter.DeleteMaintenanceRequestDetail(_presenter.GetMaintenanceRequestDetail(id));
                    //  _presenter.CurrentMaintenanceRequest.TotalPrice = _presenter.CurrentMaintenanceRequest.TotalPrice - prd.EstimatedCost;
                    //  txtTotal.Text = (_presenter.CurrentMaintenanceRequest.TotalPrice).ToString();
                    _presenter.SaveOrUpdateLeaveMaintenance(_presenter.CurrentMaintenanceRequest);
                }
                else
                {
                    _presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails.Remove(prd);
                    //  _presenter.CurrentMaintenanceRequest.TotalPrice = _presenter.CurrentMaintenanceRequest.TotalPrice - prd.EstimatedCost;
                    //  txtTotal.Text = (_presenter.CurrentMaintenanceRequest.TotalPrice).ToString();
                }
                BindMaintenanceRequestDetails();

                Master.ShowMessage(new AppMessage("Maintenance Request Detail was Removed Successfully", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Maintenance Request Detail. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgMaintenanceRequestDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgMaintenanceRequestDetail.EditItemIndex = e.Item.ItemIndex;
            BindMaintenanceRequestDetails();
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
                    DropDownList ddlFServiceTypeDetail = e.Item.FindControl("ddlServiceTypeDet") as DropDownList;
                 //   Detail.DriverServiceType = _presenter.GetServiceTypeDetails(int.Parse(ddlFServiceTypeDetail.SelectedValue));
                    DropDownList ddlFMeServiceTypeDetail = e.Item.FindControl("ddlMeServiceTypeDet") as DropDownList;
                 //   Detail.MechanicServiceType = _presenter.GetServiceTypeDetails(int.Parse(ddlFMeServiceTypeDetail.SelectedValue));
                    TextBox txtFRemark = e.Item.FindControl("txtFRemark") as TextBox;
                    Detail.TechnicianRemark = txtFRemark.Text;                   
                  
                    DropDownList ddlFProject = e.Item.FindControl("ddlFProject") as DropDownList;
                    Detail.Project = _presenter.GetProject(int.Parse(ddlFProject.SelectedValue));
                    DropDownList ddlFGrant = e.Item.FindControl("ddlFGrant") as DropDownList;
                    Detail.Grant = _presenter.GetGrant(int.Parse(ddlFGrant.SelectedValue));
                  
                    Detail.MaintenanceRequest = _presenter.CurrentMaintenanceRequest;
                    _presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails.Add(Detail);
                    Master.ShowMessage(new AppMessage("Maintenance Request Detail added successfully.", RMessageType.Info));
                    dgMaintenanceRequestDetail.EditItemIndex = -1;
                    BindMaintenanceRequestDetails();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Purchase Request Detail. " + ex.Message, RMessageType.Error));
                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
                }
            }
        }
        protected void dgMaintenanceRequestDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlFServiceType = e.Item.FindControl("ddlFServiceTpe") as DropDownList;
                BindServiceType(ddlFServiceType);
                DropDownList ddlFServiceTypeDet = e.Item.FindControl("ddlFGrant") as DropDownList;
                BindServiceTypeDetails(ddlFServiceTypeDet, Convert.ToInt32(ddlFServiceType.SelectedValue));
                DropDownList ddlFProject = e.Item.FindControl("ddlFProject") as DropDownList;
                BindProject(ddlFProject);
                DropDownList ddlFGrant = e.Item.FindControl("ddlFGrant") as DropDownList;
                BindGrant(ddlFGrant, Convert.ToInt32(ddlFProject.SelectedValue));
            }
            else
            {
                if (_presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails != null)
                {

                   
                    DropDownList ddlProject = e.Item.FindControl("ddlProject") as DropDownList;

                    if (ddlProject != null)
                    {
                        BindProject(ddlProject);

                        if (_presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails[e.Item.DataSetIndex].Project != null)
                        {
                            ListItem li = ddlProject.Items.FindByValue(_presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails[e.Item.DataSetIndex].Project.Id.ToString());
                            if (li != null)
                                li.Selected = true;
                        }
                    }
                    DropDownList ddlGrant = e.Item.FindControl("ddlGrant") as DropDownList;
                    if (ddlGrant != null)
                    {
                        BindGrant(ddlGrant, Convert.ToInt32(ddlProject.SelectedValue));
                        if (_presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails[e.Item.DataSetIndex].Grant != null)
                        {
                            ListItem liI = ddlGrant.Items.FindByValue(_presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails[e.Item.DataSetIndex].Grant.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }
                    }
                }
            }
        }
        protected void dgMaintenanceRequestDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgMaintenanceRequestDetail.DataKeys[e.Item.ItemIndex];
            MaintenanceRequestDetail Detail;
            if (id > 0)
                Detail = _presenter.CurrentMaintenanceRequest.GetMaintenanceRequestDetail(id);
            else
                Detail = _presenter.CurrentMaintenanceRequest.MaintenanceRequestDetails[e.Item.ItemIndex];

            try
            {
                DropDownList ddlFServiceTpe = e.Item.FindControl("ddlServiceTpe") as DropDownList;

                Detail.ServiceType = _presenter.GetServiceType(int.Parse(ddlFServiceTpe.SelectedValue));
                DropDownList ddlFServiceTypeDetail = e.Item.FindControl("ddlEdtServiceTypeDet") as DropDownList;
               // Detail.DriverServiceType = _presenter.GetServiceTypeDetails(int.Parse(ddlFServiceTypeDetail.SelectedValue));
                DropDownList ddlFMeServiceTypeDetail = e.Item.FindControl("ddlEdtMeServiceTypeDet") as DropDownList;
               // Detail.MechanicServiceType = _presenter.GetServiceTypeDetails(int.Parse(ddlFMeServiceTypeDetail.SelectedValue));
                TextBox txtFRemark = e.Item.FindControl("txtRemark") as TextBox;
                Detail.TechnicianRemark = txtFRemark.Text;

                DropDownList ddlFProject = e.Item.FindControl("ddlProject") as DropDownList;
                Detail.Project = _presenter.GetProject(int.Parse(ddlFProject.SelectedValue));
                DropDownList ddlFGrant = e.Item.FindControl("ddlGrant") as DropDownList;
                Detail.Grant = _presenter.GetGrant(int.Parse(ddlFGrant.SelectedValue));

        
                Detail.MaintenanceRequest = _presenter.CurrentMaintenanceRequest;
                Master.ShowMessage(new AppMessage("Maintenance Request Detail  Updated successfully.", RMessageType.Info));
                dgMaintenanceRequestDetail.EditItemIndex = -1;
                BindMaintenanceRequestDetails();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Maintenance Request Detail. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        #endregion



        protected void ckIsVehicle_CheckedChanged(object sender, EventArgs e)
        {
            if (ckIsVehicle.Checked == true)
            {
                lblPlate.Visible = true;


                ddlPlate.Visible = true;
                i.Visible = true;
            }
            else if (ckIsVehicle.Checked == false)
            {
                lblPlate.Visible = false;


                ddlPlate.Visible = false;
                i.Visible = false;
            }
        }
    }
}