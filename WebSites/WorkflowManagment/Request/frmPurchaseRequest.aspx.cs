﻿using Chai.WorkflowManagment.CoreDomain.Requests;
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
    public partial class frmPurchaseRequest : POCBasePage, IPurchaseRequestView
    {
        private PurchaseRequestPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        private PurchaseRequest _purchaserequest;
        private int _leaverequestId = 0;
        private int _totalprice = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //CheckApprovalSettings();
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                BindSearchPurchaseRequestGrid();
                BindPurchaseRequestDetails();
                BindInitialValues();
                PopMaintenanceRequestsDropDown();
            }

            lblMainReq.Visible = false;
            ddlMaintenanceReq.Visible = false;

            this._presenter.OnViewLoaded();



        }
        [CreateNew]
        public PurchaseRequestPresenter Presenter
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

            if (_presenter.CurrentPurchaseRequest.Id <= 0)
            {
                AutoNumber();
                txtRequestDate.Text = DateTime.Today.Date.ToShortDateString();

            }
        }
        private string AutoNumber()
        {
            return "PR-" + _presenter.CurrentUser().Id.ToString() + "-" + (_presenter.GetLastPurchaseRequestId() + 1).ToString();
        }
        private void BindPurchaseRequest()
        {

            if (_presenter.CurrentPurchaseRequest.Id > 0)
            {
                // txtRequestNo.Text = _presenter.CurrentPurchaseRequest.RequestNo;
                txtRequestDate.Text = _presenter.CurrentPurchaseRequest.RequestedDate.ToShortDateString();
                txtComment.Text = "";
                txtDeliverto.Text = _presenter.CurrentPurchaseRequest.DeliverTo.ToString();
                txtdeliveryDate.Text = _presenter.CurrentPurchaseRequest.Requireddateofdelivery.ToShortDateString();
                txtSuggestedSupplier.Text = _presenter.CurrentPurchaseRequest.SuggestedSupplier.ToString();


            }
        }
        private void SavePurchaseRequest()
        {
            AppUser CurrentUser = _presenter.CurrentUser();

            try
            {
                _presenter.CurrentPurchaseRequest.Requester = CurrentUser.Id;
                _presenter.CurrentPurchaseRequest.RequestedDate = Convert.ToDateTime(txtRequestDate.Text);
                _presenter.CurrentPurchaseRequest.RequestNo = AutoNumber();
                _presenter.CurrentPurchaseRequest.DeliverTo = txtDeliverto.Text;
                _presenter.CurrentPurchaseRequest.Comment = "";
                _presenter.CurrentPurchaseRequest.SuggestedSupplier = txtSuggestedSupplier.Text;
                _presenter.CurrentPurchaseRequest.Requireddateofdelivery = Convert.ToDateTime(txtdeliveryDate.Text);
                _presenter.CurrentPurchaseRequest.IsVehicle = GetIsVehicle;
                _presenter.CurrentPurchaseRequest.MaintenanceRequestNo = GetMaintenanceRequestNo;
                if (!String.IsNullOrEmpty(ddlMaintenanceReq.SelectedValue))
                {
                    MaintenanceRequest mreq = _presenter.GetMaintenanceRequestById(Convert.ToInt32(ddlMaintenanceReq.SelectedValue));
                    _presenter.CurrentPurchaseRequest.MaintenanceId = mreq.Id;
                }

                //Determine total cost
                /*       decimal cost = 0;
                       if (_presenter.CurrentPurchaseRequest.PurchaseRequestDetails.Count > 0)
                       {

                           foreach (PurchaseRequestDetail detail in _presenter.CurrentPurchaseRequest.PurchaseRequestDetails)
                           {
                               cost = cost + detail.EstimatedCost;
                           }
                       }
                       _presenter.CurrentPurchaseRequest.TotalPrice = cost;*/
                //Determine total cost end
                SavePurchaseRequestStatus();
                GetCurrentApprover();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Violation of UNIQUE KEY"))
                    {
                        Master.ShowMessage(new AppMessage("Please Click Request button Again,There is a duplicate Number", RMessageType.Error));
                        //AutoNumber();
                    }
                }
            }

        }
        private void SavePurchaseRequestStatus()
        {
            if (_presenter.CurrentPurchaseRequest.Id <= 0)
            {
                if (_presenter.CurrentPurchaseRequest.PurchaseRequestDetails[0].Project.ProjectCode == "CHET-GS")
                {
                    int i = 1;
                    foreach (ApprovalLevel AL in _presenter.GetApprovalSettingPurchaseGS().ApprovalLevels)
                    {
                        PurchaseRequestStatus PRS = new PurchaseRequestStatus();
                        PRS.PurchaseRequest = _presenter.CurrentPurchaseRequest;
                        if (AL.EmployeePosition.PositionName == "Program Manager")
                        {
                            if (_presenter.CurrentPurchaseRequest.PurchaseRequestDetails[0].Project != null)
                            {
                                if (_presenter.CurrentPurchaseRequest.PurchaseRequestDetails[0].Project.AppUser.Id != _presenter.CurrentUser().Id)
                                {
                                    PRS.Approver = _presenter.GetProject(_presenter.CurrentPurchaseRequest.PurchaseRequestDetails[0].Project.Id).AppUser.Id;
                                }
                                else
                                {
                                    PRS.Approver = _presenter.CurrentUser().Superviser.Value;
                                }
                            }
                        }
                        else
                        {
                            PRS.Approver = _presenter.Approver(AL.EmployeePosition.Id).Id;
                        }
                        PRS.WorkflowLevel = i;
                        i++;
                        _presenter.CurrentPurchaseRequest.PurchaseRequestStatuses.Add(PRS);
                    }
                }
                else
                {
                    if (_presenter.GetApprovalSettingforPurchaseProcess(RequestType.Purchase_Request.ToString().Replace('_', ' '), 0) != null)
                    {
                        int i = 1;
                        foreach (ApprovalLevel AL in _presenter.GetApprovalSettingforPurchaseProcess(RequestType.Purchase_Request.ToString().Replace('_', ' '), 0).ApprovalLevels)
                        {
                            PurchaseRequestStatus PRS = new PurchaseRequestStatus();
                            PRS.PurchaseRequest = _presenter.CurrentPurchaseRequest;
                            if (AL.EmployeePosition.PositionName == "Superviser/Line Manager")
                            {
                                if (_presenter.CurrentUser().Superviser.Value != 0)
                                {
                                    PRS.Approver = _presenter.CurrentUser().Superviser.Value;
                                }
                                else
                                {
                                    PRS.ApprovalStatus = ApprovalStatus.Approved.ToString();
                                    PRS.ApprovalDate = DateTime.Today.Date;
                                }
                            }
                            else if (AL.EmployeePosition.PositionName == "Program Manager")
                            {
                                if (_presenter.CurrentPurchaseRequest.PurchaseRequestDetails[0].Project != null)
                                {
                                    if (_presenter.CurrentPurchaseRequest.PurchaseRequestDetails[0].Project.AppUser.Id != _presenter.CurrentUser().Id)
                                    {
                                        PRS.Approver = _presenter.GetProject(_presenter.CurrentPurchaseRequest.PurchaseRequestDetails[0].Project.Id).AppUser.Id;
                                    }
                                    else
                                    {
                                        PRS.Approver = _presenter.CurrentUser().Superviser.Value;
                                    }
                                }
                            }
                            else
                            {
                                PRS.Approver = _presenter.Approver(AL.EmployeePosition.Id).Id;
                            }
                            PRS.WorkflowLevel = i;
                            i++;
                            _presenter.CurrentPurchaseRequest.PurchaseRequestStatuses.Add(PRS);
                        }
                    }
                    else { pnlWarning.Visible = true; }
                }

            }
        }
        private void GetCurrentApprover()
        {
            foreach (PurchaseRequestStatus PRS in _presenter.CurrentPurchaseRequest.PurchaseRequestStatuses)
            {
                if (PRS.ApprovalStatus == null)
                {
                    SendEmail(PRS);
                    _presenter.CurrentPurchaseRequest.CurrentApprover = PRS.Approver;
                    _presenter.CurrentPurchaseRequest.CurrentLevel = PRS.WorkflowLevel;
                    _presenter.CurrentPurchaseRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                    break;
                }
            }
        }
        private void SendEmail(PurchaseRequestStatus PRS)
        {
            if (_presenter.GetSuperviser(PRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(_presenter.GetSuperviser(PRS.Approver).Email, "Purchase Request", _presenter.GetUser(_presenter.CurrentPurchaseRequest.Requester).FullName + "' Request for Item procurment No. '" + _presenter.CurrentPurchaseRequest.RequestNo + "'");
            }
            else
            {
                EmailSender.Send(_presenter.GetSuperviser(_presenter.GetAssignedJobbycurrentuser(PRS.Approver).AssignedTo).Email, "Purchase Request", _presenter.GetUser(_presenter.CurrentPurchaseRequest.Requester).FullName + "' Request for Item procurment No. '" + _presenter.CurrentPurchaseRequest.RequestNo + "'");
            }
        }
        private void PopMaintenanceRequestsDropDown()
        {
            ddlMaintenanceReq.Items.Clear();
            ListItem lst = new ListItem();
            lst.Text = "Select Maintenance Request";
            lst.Value = "";
            ddlMaintenanceReq.Items.Add(lst);
            ddlMaintenanceReq.DataSource = _presenter.GetMaintenanceRequestCompleted();
            ddlMaintenanceReq.DataBind();


        }
        public PurchaseRequest PurchaseRequest
        {
            get
            {
                return _purchaserequest;
            }
            set
            {
                _purchaserequest = value;
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
        public string GetMaintenanceRequestNo
        {
            get { return ddlMaintenanceReq.SelectedValue; }
        }
        public int PurchaseRequestId
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
        private void ClearForm()
        {
            //txtRequestNo.Text = "";
            txtRequestDate.Text = "";
            txtComment.Text = "";
            txtDeliverto.Text = "";
            txtdeliveryDate.Text = "";
            txtSuggestedSupplier.Text = "";


        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmPurchaseRequest.aspx");
        }
        protected void grvPurchaseRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["PurchaseRequest"] = true;
            // ClearForm();
            //BindLeaveRequest();
            _leaverequestId = Convert.ToInt32(grvPurchaseRequestList.SelectedDataKey[0]);
            _presenter.OnViewLoaded();
            BindPurchaseRequest();
            BindPurchaseRequestDetails();
            if (_presenter.CurrentPurchaseRequest.CurrentLevel > 1)
            {
                btnRequest.Visible = false;
                btnDelete.Visible = false;
                btnPrint.Visible = true;
                PrintTransaction();
                dgPurchaseRequestDetail.Columns[8].Visible = false;
            }
            else
            {
                btnRequest.Visible = true;
                btnDelete.Visible = true;
                dgPurchaseRequestDetail.Columns[8].Visible = true;
            }
        }
        protected void grvPurchaseRequestList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            _presenter.DeletePurchaseRequest(_presenter.GetPurchaseRequestById(Convert.ToInt32(grvPurchaseRequestList.DataKeys[e.RowIndex].Value)));

            btnFind_Click(sender, e);
            Master.ShowMessage(new AppMessage("Purchase Request Successfully Deleted", RMessageType.Info));

        }
        protected void grvPurchaseRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void grvPurchaseRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvPurchaseRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
            ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
            //pnlSearch_ModalPopupExtender.Show();
        }
        protected void grvMaintenanceStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.GetMaintenanceRequestById(_presenter.CurrentPurchaseRequest.MaintenanceId).MaintenanceRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_presenter.GetMaintenanceRequestById(_presenter.CurrentPurchaseRequest.MaintenanceId).MaintenanceRequestStatuses[e.Row.RowIndex].Approver > 0)
                        e.Row.Cells[1].Text = _presenter.GetUser(_presenter.GetMaintenanceRequestById(_presenter.CurrentPurchaseRequest.MaintenanceId).MaintenanceRequestStatuses[e.Row.RowIndex].Approver).FullName;
                }
            }
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentPurchaseRequest.PurchaseRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_presenter.CurrentPurchaseRequest.PurchaseRequestStatuses[e.Row.RowIndex].Approver != 0)
                        e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentPurchaseRequest.PurchaseRequestStatuses[e.Row.RowIndex].Approver).FullName;
                }
            }
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindSearchPurchaseRequestGrid();
            ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
            //pnlSearch_ModalPopupExtender.Show();
        }
        private void BindSearchPurchaseRequestGrid()
        {
            grvPurchaseRequestList.DataSource = _presenter.ListPurchaseRequests(txtRequestNosearch.Text, txtRequestDatesearch.Text);
            grvPurchaseRequestList.DataBind();
        }
        private void PrintTransaction()
        {
            lblRequestNoResult.Text = _presenter.CurrentPurchaseRequest.RequestNo.ToString();
            lblRequestedDateResult.Text = _presenter.CurrentPurchaseRequest.RequestedDate.ToShortDateString();
            lblRequesterResult.Text = _presenter.GetUser(_presenter.CurrentPurchaseRequest.Requester).FullName;
            lblSuggestedSupplierResult.Text = _presenter.CurrentPurchaseRequest.SuggestedSupplier.ToString();

            lblRemarkResult.Text = _presenter.CurrentPurchaseRequest.Comment;
            lblDelivertoResult.Text = _presenter.CurrentPurchaseRequest.DeliverTo;
            lblReqDateofDeliveryResult.Text = _presenter.CurrentPurchaseRequest.Requireddateofdelivery.ToShortDateString();
            grvDetails.DataSource = _presenter.CurrentPurchaseRequest.PurchaseRequestDetails;
            grvDetails.DataBind();
            if (_presenter.CurrentPurchaseRequest.MaintenanceId > 0)
            {
                lblApprovalDetPrint.Visible = true;
                lblMainDetailPrint.Visible = true;
                grvMaintenaceDet.DataSource = _presenter.GetMaintenanceRequestById(_presenter.CurrentPurchaseRequest.MaintenanceId).MaintenanceRequestDetails;
                grvMaintenaceDet.DataBind();
                grvMainSta.DataSource = _presenter.GetMaintenanceRequestById(_presenter.CurrentPurchaseRequest.MaintenanceId).MaintenanceRequestStatuses;
                grvMainSta.DataBind();
                grvStatuses.DataSource = _presenter.CurrentPurchaseRequest.PurchaseRequestStatuses;
                grvStatuses.DataBind();
            }

            else
            {
                lblApprovalDetPrint.Visible = false;
                lblMainDetailPrint.Visible = false;
                grvStatuses.DataSource = _presenter.CurrentPurchaseRequest.PurchaseRequestStatuses;
                grvStatuses.DataBind();
            }
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

                lblMainReq.Visible = true;



                ddlMaintenanceReq.Visible = true;

            }
            else if (ckIsVehicle.Checked == false)
            {

                lblMainReq.Visible = false;


                ddlMaintenanceReq.Visible = false;

            }
        }
        protected void ddlGrant_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlGrant = ddl.FindControl("ddlGrant") as DropDownList;
            BindGrant(ddlGrant, Convert.ToInt32(ddl.SelectedValue));
            if (ckIsVehicle.Checked == true)
            {

                lblMainReq.Visible = true;


                ddlMaintenanceReq.Visible = true;

            }
            else if (ckIsVehicle.Checked == false)
            {

                lblMainReq.Visible = false;


                ddlMaintenanceReq.Visible = false;

            }
        }
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlGrant = ddl.FindControl("ddlGrant") as DropDownList;
            BindGrant(ddlGrant, Convert.ToInt32(ddl.SelectedValue));
            if (ckIsVehicle.Checked == true)
            {

                lblMainReq.Visible = true;


                ddlMaintenanceReq.Visible = true;

            }
            else if (ckIsVehicle.Checked == false)
            {

                lblMainReq.Visible = false;

                ddlMaintenanceReq.Visible = false;

            }
        }
        protected void btnRequest_Click(object sender, EventArgs e)
        {
            try
            {
                SavePurchaseRequest();
                if (_presenter.CurrentPurchaseRequest.PurchaseRequestDetails.Count != 0)
                {
                    if (_presenter.CurrentPurchaseRequest.PurchaseRequestStatuses.Count != 0)
                    {
                        _presenter.SaveOrUpdateLeavePurchase(_presenter.CurrentPurchaseRequest);
                        ClearForm();
                        BindSearchPurchaseRequestGrid();
                        Master.ShowMessage(new AppMessage("Successfully did a Purchase Request, Reference No - <b>'" + _presenter.CurrentPurchaseRequest.RequestNo + "'</b> ", RMessageType.Info));
                        // Log.Info(_presenter.CurrentUser().FullName + " has requested for a Purchase of Total Price " + _presenter.CurrentPurchaseRequest.TotalPrice);
                    }
                    else
                    {
                        Master.ShowMessage(new AppMessage("There is an error constracting Approval Process", RMessageType.Error));
                    }
                }
                else
                {
                    Master.ShowMessage(new AppMessage("You have to insert at least one purchase item detail", RMessageType.Error));
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {





            try
            {
                if (_presenter.CurrentPurchaseRequest.CurrentStatus == null)
                {
                    _presenter.DeletePurchaseRequest(_presenter.CurrentPurchaseRequest);
                    ClearForm();
                    Master.ShowMessage(new AppMessage("Purchase Request Deleted ", RMessageType.Info));
                    BindSearchPurchaseRequestGrid();
                    BindPurchaseRequestDetails();
                    btnDelete.Visible = false;
                }
                else
                    Master.ShowMessage(new AppMessage("Warning: Unable to Delete Purchase Request ", RMessageType.Error));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Warning: Unable to Delete Purchase Request " + ex.Message, RMessageType.Error));
            }
        }
        protected void ddlFAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            TextBox txtAccountCode = ddl.FindControl("txtFAccountCode") as TextBox;
            txtAccountCode.Text = _presenter.GetItemAccount(Convert.ToInt32(ddl.SelectedValue)).AccountCode;
        }
        protected void ddlAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            TextBox txtAccountCode = ddl.FindControl("txtAccountCode") as TextBox;
            txtAccountCode.Text = _presenter.GetItemAccount(Convert.ToInt32(ddl.SelectedValue)).AccountCode;
        }
        protected void ckIsVehicle_CheckedChanged(object sender, EventArgs e)
        {
            if (ckIsVehicle.Checked == true)
            {
                lblMainReq.Visible = true;
                ddlMaintenanceReq.Visible = true;
            }
            else if (ckIsVehicle.Checked == false)
            {
                lblMainReq.Visible = false;
                ddlMaintenanceReq.Visible = false;
            }
        }
        #region PurchaseRequestDetail
        private void BindPurchaseRequestDetails()
        {
            dgPurchaseRequestDetail.DataSource = _presenter.CurrentPurchaseRequest.PurchaseRequestDetails;
            dgPurchaseRequestDetail.DataBind();
        }
        protected void dgPurchaseRequestDetail_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            dgPurchaseRequestDetail.EditItemIndex = -1;
            BindPurchaseRequestDetails();
        }
        protected void dgPurchaseRequestDetail_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgPurchaseRequestDetail.DataKeys[e.Item.ItemIndex];
            int PRDId = (int)dgPurchaseRequestDetail.DataKeys[e.Item.ItemIndex];
            PurchaseRequestDetail prd;

            if (PRDId > 0)
                prd = _presenter.CurrentPurchaseRequest.GetPurchaseRequestDetail(PRDId);
            else
                prd = _presenter.CurrentPurchaseRequest.PurchaseRequestDetails[e.Item.ItemIndex];
            try
            {
                if (PRDId > 0)
                {
                    _presenter.CurrentPurchaseRequest.RemovePurchaseRequestDetail(id);
                    _presenter.DeletePurchaseRequestDetail(_presenter.GetPurchaseRequestDetail(id));
                    //  _presenter.CurrentPurchaseRequest.TotalPrice = _presenter.CurrentPurchaseRequest.TotalPrice - prd.EstimatedCost;
                    //  txtTotal.Text = (_presenter.CurrentPurchaseRequest.TotalPrice).ToString();
                    _presenter.SaveOrUpdateLeavePurchase(_presenter.CurrentPurchaseRequest);
                }
                else
                {
                    _presenter.CurrentPurchaseRequest.PurchaseRequestDetails.Remove(prd);
                    //  _presenter.CurrentPurchaseRequest.TotalPrice = _presenter.CurrentPurchaseRequest.TotalPrice - prd.EstimatedCost;
                    //  txtTotal.Text = (_presenter.CurrentPurchaseRequest.TotalPrice).ToString();
                }
                BindPurchaseRequestDetails();

                Master.ShowMessage(new AppMessage("Purchase Request Detail was Removed Successfully", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Purchase Request Detail. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgPurchaseRequestDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgPurchaseRequestDetail.EditItemIndex = e.Item.ItemIndex;
            BindPurchaseRequestDetails();
        }
        protected void dgPurchaseRequestDetail_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "AddNew")
            {
                try
                {
                    PurchaseRequestDetail Detail = new PurchaseRequestDetail();
                    DropDownList ddlFAccount = e.Item.FindControl("ddlFAccount") as DropDownList;
                    Detail.ItemAccount = _presenter.GetItemAccount(int.Parse(ddlFAccount.SelectedValue));
                    TextBox txtFAccountCode = e.Item.FindControl("txtFAccountCode") as TextBox;
                    Detail.AccountCode = txtFAccountCode.Text;
                    TextBox txtFItem = e.Item.FindControl("txtFItem") as TextBox;
                    Detail.ItemDescription = txtFItem.Text;
                    TextBox txtFQty = e.Item.FindControl("txtFQty") as TextBox;
                    Detail.Qty = Convert.ToInt32(txtFQty.Text);
                    Detail.ApprovedQuantity = Convert.ToInt32(txtFQty.Text);
                    DropDownList ddlFPurposeOfPurchase = e.Item.FindControl("ddlFPurposeOfPurchase") as DropDownList;
                    Detail.PurposeOfPurchase = ddlFPurposeOfPurchase.SelectedValue;
                    DropDownList ddlFUnitOfMeasurment = e.Item.FindControl("ddlFUnitOfMeasurment") as DropDownList;
                    Detail.UnitOfMeasurment = ddlFUnitOfMeasurment.SelectedValue;
                    TextBox txtFRemark = e.Item.FindControl("txtFRemark") as TextBox;
                    Detail.Remark = txtFRemark.Text;
                    DropDownList ddlFProject = e.Item.FindControl("ddlFProject") as DropDownList;
                    Detail.Project = _presenter.GetProject(Convert.ToInt32(ddlFProject.SelectedValue));
                    DropDownList ddlFGrant = e.Item.FindControl("ddlFGrant") as DropDownList;
                    Detail.Grant = _presenter.GetGrant(Convert.ToInt32(ddlFGrant.SelectedValue));
                    Detail.BidAnalysisRequestStatus = "InProgress";
                    Detail.PurchaseRequest = _presenter.CurrentPurchaseRequest;
                    _presenter.CurrentPurchaseRequest.PurchaseRequestDetails.Add(Detail);
                    Master.ShowMessage(new AppMessage("Purchase Request Detail added successfully.", RMessageType.Info));
                    dgPurchaseRequestDetail.EditItemIndex = -1;
                    BindPurchaseRequestDetails();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Purchase Request Detail. " + ex.Message, RMessageType.Error));
                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
                }
            }
        }
        protected void dgPurchaseRequestDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlFItemAccount = e.Item.FindControl("ddlFAccount") as DropDownList;
                BindAccount(ddlFItemAccount);
                DropDownList ddlFProject = e.Item.FindControl("ddlFProject") as DropDownList;
                BindProject(ddlFProject);
                DropDownList ddlFGrant = e.Item.FindControl("ddlFGrant") as DropDownList;
                BindGrant(ddlFGrant, Convert.ToInt32(ddlFProject.SelectedValue));
            }
            else
            {
                if (_presenter.CurrentPurchaseRequest.PurchaseRequestDetails != null)
                {

                    DropDownList ddlItemAccount = e.Item.FindControl("ddlAccount") as DropDownList;
                    if (ddlItemAccount != null)
                    {
                        BindAccount(ddlItemAccount);
                        if (_presenter.CurrentPurchaseRequest.PurchaseRequestDetails[e.Item.DataSetIndex].ItemAccount != null)
                        {
                            ListItem liI = ddlItemAccount.Items.FindByValue(_presenter.CurrentPurchaseRequest.PurchaseRequestDetails[e.Item.DataSetIndex].ItemAccount.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }
                    }
                    DropDownList ddlProject = e.Item.FindControl("ddlProject") as DropDownList;

                    if (ddlProject != null)
                    {
                        BindProject(ddlProject);

                        if (_presenter.CurrentPurchaseRequest.PurchaseRequestDetails[e.Item.DataSetIndex].Project != null)
                        {
                            ListItem li = ddlProject.Items.FindByValue(_presenter.CurrentPurchaseRequest.PurchaseRequestDetails[e.Item.DataSetIndex].Project.Id.ToString());
                            if (li != null)
                                li.Selected = true;
                        }
                    }
                    DropDownList ddlGrant = e.Item.FindControl("ddlGrant") as DropDownList;
                    if (ddlGrant != null)
                    {
                        BindGrant(ddlGrant, Convert.ToInt32(ddlProject.SelectedValue));
                        if (_presenter.CurrentPurchaseRequest.PurchaseRequestDetails[e.Item.DataSetIndex].Grant != null)
                        {
                            ListItem liI = ddlGrant.Items.FindByValue(_presenter.CurrentPurchaseRequest.PurchaseRequestDetails[e.Item.DataSetIndex].Grant.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }
                    }
                }
            }
        }
        protected void dgPurchaseRequestDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgPurchaseRequestDetail.DataKeys[e.Item.ItemIndex];
            PurchaseRequestDetail Detail;
            if (id > 0)
                Detail = _presenter.CurrentPurchaseRequest.GetPurchaseRequestDetail(id);
            else
                Detail = _presenter.CurrentPurchaseRequest.PurchaseRequestDetails[e.Item.ItemIndex];

            try
            {
                DropDownList ddlAccount = e.Item.FindControl("ddlAccount") as DropDownList;
                Detail.ItemAccount = _presenter.GetItemAccount(int.Parse(ddlAccount.SelectedValue));
                TextBox txtAccountCode = e.Item.FindControl("txtAccountCode") as TextBox;
                Detail.AccountCode = txtAccountCode.Text;
                TextBox txtItem = e.Item.FindControl("txtItem") as TextBox;
                Detail.ItemDescription = txtItem.Text;
                TextBox txtQty = e.Item.FindControl("txtQty") as TextBox;
                Detail.Qty = Convert.ToInt32(txtQty.Text);
                Detail.ApprovedQuantity = Convert.ToInt32(txtQty.Text);
                DropDownList ddlPurposeOfPurchase = e.Item.FindControl("ddlPurposeOfPurchase") as DropDownList;
                Detail.PurposeOfPurchase = ddlPurposeOfPurchase.SelectedValue;
                DropDownList ddlUnitOfMeasurment = e.Item.FindControl("ddlUnitOfMeasurment") as DropDownList;
                Detail.UnitOfMeasurment = ddlUnitOfMeasurment.SelectedValue;
                TextBox txtRemark = e.Item.FindControl("txtRemark") as TextBox;
                Detail.Remark = txtRemark.Text;
                DropDownList ddlProject = e.Item.FindControl("ddlProject") as DropDownList;
                Detail.Project = _presenter.GetProject(int.Parse(ddlProject.SelectedValue));
                DropDownList ddlGrant = e.Item.FindControl("ddlGrant") as DropDownList;
                Detail.Grant = _presenter.GetGrant(int.Parse(ddlGrant.SelectedValue));
                Detail.BidAnalysisRequestStatus = "InProgress";
                Detail.PurchaseRequest = _presenter.CurrentPurchaseRequest;
                Master.ShowMessage(new AppMessage("Purchase Request Detail successfully updated.", RMessageType.Info));
                dgPurchaseRequestDetail.EditItemIndex = -1;
                BindPurchaseRequestDetails();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Purchase Request Detail. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        #endregion
        protected void ddlMaintenanceReq_SelectedIndexChanged(object sender, EventArgs e)
        {
            MaintenanceRequest maintenance = null;
            dgPurchaseRequestDetail.DataSource = null;
            if (_presenter.GetMaintenanceRequestById(Convert.ToInt32(ddlMaintenanceReq.SelectedValue)) != null)
            {
                maintenance = _presenter.GetMaintenanceRequestById(Convert.ToInt32(ddlMaintenanceReq.SelectedValue));
            }

            foreach (MaintenanceSparePart msp in maintenance.MaintenanceSpareParts)
            {
                PurchaseRequestDetail prd = new PurchaseRequestDetail();
                prd.Item = msp.Item;
                prd.ItemDescription = msp.Item.Name;
                prd.Project = msp.MaintenanceRequest.Project;
                prd.Grant = msp.MaintenanceRequest.Grant;

                _presenter.CurrentPurchaseRequest.PurchaseRequestDetails.Add(prd);
            }
            lblMainReq.Visible = true;
            ddlMaintenanceReq.Visible = true;

            dgPurchaseRequestDetail.DataSource = _presenter.CurrentPurchaseRequest.PurchaseRequestDetails;
            dgPurchaseRequestDetail.DataBind();

        }        
    }
}