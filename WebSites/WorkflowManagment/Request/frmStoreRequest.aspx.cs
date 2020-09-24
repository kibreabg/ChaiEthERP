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
    public partial class frmStoreRequest : POCBasePage, IStoreRequestView
    {
        private StoreRequestPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        private StoreRequest _StoreRequest;
        private int _leaverequestId = 0;
        private int _totalprice = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //CheckApprovalSettings();
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                BindSearchStoreRequestGrid();
                BindStoreRequestDetails();
                PopPurchaseRequestsDropDown();
                BindInitialValues();
                BindPrograms();
            }
         
            this._presenter.OnViewLoaded();



        }
        [CreateNew]
        public StoreRequestPresenter Presenter
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
            if (_presenter.GetApprovalSetting(RequestType.Store_Request.ToString().Replace('_', ' '), 0) == null)
            {
                pnlWarning.Visible = true;
            }
        }
        private void BindInitialValues()
        {
            AppUser CurrentUser = _presenter.CurrentUser();
            txtRequester.Text = CurrentUser.FirstName + " " + CurrentUser.LastName;

            if (_presenter.CurrentStoreRequest.Id <= 0)
            {
                AutoNumber();
                txtRequestDate.Text = DateTime.Today.Date.ToShortDateString();

            }
        }
        private string AutoNumber()
        {
            return "SR-" + _presenter.CurrentUser().Id.ToString() + "-" + (_presenter.GetLastStoreRequestId() + 1).ToString();
        }
        private void BindStoreRequest()
        {

            if (_presenter.CurrentStoreRequest.Id > 0)
            {
                // txtRequestNo.Text = _presenter.CurrentStoreRequest.RequestNo;
                txtRequestDate.Text = _presenter.CurrentStoreRequest.RequestedDate.ToShortDateString();
               
                txtDeliverto.Text = _presenter.CurrentStoreRequest.DeliverTo.ToString();
               


            }
        }
        private void SaveStoreRequest()
        {
            AppUser CurrentUser = _presenter.CurrentUser();
            try
            {
                _presenter.CurrentStoreRequest.Requester = CurrentUser.Id;
                _presenter.CurrentStoreRequest.RequestedDate = Convert.ToDateTime(txtRequestDate.Text);
                _presenter.CurrentStoreRequest.RequestNo = AutoNumber();
                _presenter.CurrentStoreRequest.DeliverTo = txtDeliverto.Text;
                _presenter.CurrentStoreRequest.Comment = "";
             
                //Determine total cost
                /*       decimal cost = 0;
                       if (_presenter.CurrentStoreRequest.StoreRequestDetails.Count > 0)
                       {

                           foreach (StoreRequestDetail detail in _presenter.CurrentStoreRequest.StoreRequestDetails)
                           {
                               cost = cost + detail.EstimatedCost;
                           }
                       }
                       _presenter.CurrentStoreRequest.TotalPrice = cost;*/
                //Determine total cost end
                SaveStoreRequestStatus();
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
        private void BindPrograms()
        {
            ddlProgram.DataSource = _presenter.GetPrograms();
            ddlProgram.DataBind();
        }
        private void PopPurchaseRequest()
        {
            if (Convert.ToInt32(ddlPurchaseReq.SelectedValue) > 0 || Convert.ToInt32(ddlPurchaseReq.SelectedValue).ToString() != null)
            {
                grvDetails.DataSource = _presenter.ListPRDetailsCompletedById(Convert.ToInt32(ddlPurchaseReq.SelectedValue));
                //grvDetails.DataSource = _presenter.ListPurchaseReqInProgress();
                grvDetails.DataBind();
            }
        }


        private void PopPurchaseRequestsDropDown()
        {
            ddlPurchaseReq.Items.Clear();
            ListItem lst = new ListItem();
            lst.Text = " Select Request No ";
            lst.Value = "0";
            ddlPurchaseReq.Items.Add(lst);
            ddlPurchaseReq.DataSource = _presenter.GetDistinctCompletedPurchaseReqs();
            ddlPurchaseReq.DataBind();
        }
        private void SaveStoreRequestStatus()
        {
            if (_presenter.CurrentStoreRequest.Id <= 0)
            {
                if (_presenter.GetApprovalSetting(RequestType.Store_Request.ToString().Replace('_', ' '), 0) != null)
                {
                    int i = 1;
                    foreach (ApprovalLevel AL in _presenter.GetApprovalSetting(RequestType.Store_Request.ToString().Replace('_', ' '), 0).ApprovalLevels)
                    {
                        StoreRequestStatus PRS = new StoreRequestStatus();
                        PRS.StoreRequest = _presenter.CurrentStoreRequest;
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
                            if (_presenter.CurrentStoreRequest.StoreRequestDetails[0].Project.Id != 0)
                            {
                                PRS.Approver = _presenter.GetProject(_presenter.CurrentStoreRequest.StoreRequestDetails[0].Project.Id).AppUser.Id;
                            }
                        }
                        else
                        {
                            PRS.Approver = _presenter.Approver(AL.EmployeePosition.Id).Id;
                        }
                        PRS.WorkflowLevel = i;
                        i++;
                        _presenter.CurrentStoreRequest.StoreRequestStatuses.Add(PRS);
                    }
                }
                else { pnlWarning.Visible = true; }
            }
        }
        private void GetCurrentApprover()
        {
            foreach (StoreRequestStatus PRS in _presenter.CurrentStoreRequest.StoreRequestStatuses)
            {
                if (PRS.ApprovalStatus == null)
                {
                    SendEmail(PRS);
                    _presenter.CurrentStoreRequest.CurrentApprover = PRS.Approver;
                    _presenter.CurrentStoreRequest.CurrentLevel = PRS.WorkflowLevel;
                    _presenter.CurrentStoreRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                    break;
                }
            }
        }
        private void SendEmail(StoreRequestStatus PRS)
        {
            if (_presenter.GetSuperviser(PRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(_presenter.GetSuperviser(PRS.Approver).Email, "Store Request", _presenter.GetUser(_presenter.CurrentStoreRequest.Requester).FullName + "' Request for Store Requisition. '" + _presenter.CurrentStoreRequest.RequestNo + "'");
            }
            else
            {
                EmailSender.Send(_presenter.GetSuperviser(_presenter.GetAssignedJobbycurrentuser(PRS.Approver).AssignedTo).Email, "Purchase Request", _presenter.GetUser(_presenter.CurrentStoreRequest.Requester).FullName + "' Request for  Store Requisition. '" + _presenter.CurrentStoreRequest.RequestNo + "'");
            }
        }
       
        public StoreRequest StoreRequest
        {
            get
            {
                return _StoreRequest;
            }
            set
            {
                _StoreRequest = value;
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
      
        public int StoreRequestId
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
        private void BindProject(DropDownList ddlProject, int programID)
        {
            ddlProject.DataSource = _presenter.ListProjects(programID);
            ddlProject.DataValueField = "Id";
            ddlProject.DataTextField = "ProjectCode";
            ddlProject.DataBind();
        }
        //private void BindProject(DropDownList ddlProject)
        //{
        //    ddlProject.DataSource = _presenter.GetProjects();
        //    ddlProject.DataBind();

        //}
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
        private void BindItems(DropDownList ddlItem)
        {
            ddlItem.Items.Clear();
            ListItem lst = new ListItem();
            lst.Text = "Select Item";
            lst.Value = "";
            ddlItem.Items.Add(lst);
            ddlItem.DataSource = _presenter.GetItems();
            ddlItem.DataBind();
        }
        private void ClearForm()
        {
            //txtRequestNo.Text = "";
            txtRequestDate.Text = "";
            
            txtDeliverto.Text = "";
         


        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmStoreRequest.aspx");
        }
        protected void grvStoreRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["StoreRequest"] = true;
            // ClearForm();
            //BindLeaveRequest();
            _leaverequestId = Convert.ToInt32(grvStoreRequestList.SelectedDataKey[0]);
            _presenter.OnViewLoaded();
            BindStoreRequest();
            BindStoreRequestDetails();
            if (_presenter.CurrentStoreRequest.CurrentLevel > 1)
            {
                btnRequest.Visible = false;
                btnDelete.Visible = false;
                dgStoreRequestDetail.Columns[8].Visible = false;
            }
            else
            {
                btnRequest.Visible = true;
                btnDelete.Visible = true;
                dgStoreRequestDetail.Columns[8].Visible = true;
            }
        }
        protected void grvStoreRequestList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            _presenter.DeleteStoreRequest(_presenter.GetStoreRequestById(Convert.ToInt32(grvStoreRequestList.DataKeys[e.RowIndex].Value)));

            btnFind_Click(sender, e);
            Master.ShowMessage(new AppMessage("Store Request Successfully Deleted", Chai.WorkflowManagment.Enums.RMessageType.Info));

        }

        protected void ddlPurchaseReq_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopPurchaseRequest();
            pnlInfo.Visible = true;
            grvDetails.Visible = true;
        }
        protected void grvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvDetails.PageIndex = e.NewPageIndex;
            PopPurchaseRequest();
        }
        protected void grvStoreRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void grvStoreRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvStoreRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
            ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
            //pnlSearch_ModalPopupExtender.Show();
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindSearchStoreRequestGrid();
            ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
            //pnlSearch_ModalPopupExtender.Show();
        }
        private void BindSearchStoreRequestGrid()
        {
            grvStoreRequestList.DataSource = _presenter.ListStoreRequests(txtRequestNosearch.Text, txtRequestDatesearch.Text);
            grvStoreRequestList.DataBind();
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
           
        }
        protected void ddlGrant_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlGrant = ddl.FindControl("ddlGrant") as DropDownList;
            BindGrant(ddlGrant, Convert.ToInt32(ddl.SelectedValue));
          
        }
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlGrant = ddl.FindControl("ddlGrant") as DropDownList;
            BindGrant(ddlGrant, Convert.ToInt32(ddl.SelectedValue));
           
        }
        protected void btnRequest_Click(object sender, EventArgs e)
        {
            try
            {
                SaveStoreRequest();
                if (_presenter.CurrentStoreRequest.StoreRequestDetails.Count != 0)
                {
                    if (_presenter.CurrentStoreRequest.StoreRequestStatuses.Count != 0)
                    {
                        _presenter.SaveOrUpdateStoreRequest(_presenter.CurrentStoreRequest);
                        ClearForm();
                        BindSearchStoreRequestGrid();
                        Master.ShowMessage(new AppMessage("Successfully did a Store Request, Reference No - <b>'" + _presenter.CurrentStoreRequest.RequestNo + "'</b> ", Chai.WorkflowManagment.Enums.RMessageType.Info));
                        // Log.Info(_presenter.CurrentUser().FullName + " has requested for a Purchase of Total Price " + _presenter.CurrentStoreRequest.TotalPrice);
                    }
                    else
                    {
                        Master.ShowMessage(new AppMessage("There is an error constracting Approval Process", Chai.WorkflowManagment.Enums.RMessageType.Error));

                    }

                }
                else
                {
                    Master.ShowMessage(new AppMessage("You have to insert at least one Store item detail", Chai.WorkflowManagment.Enums.RMessageType.Error));
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
                if (_presenter.CurrentStoreRequest.CurrentStatus == null)
                {
                    _presenter.DeleteStoreRequest(_presenter.CurrentStoreRequest);
                    ClearForm();
                    Master.ShowMessage(new AppMessage("Store Request Deleted ", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    BindSearchStoreRequestGrid();
                    BindStoreRequestDetails();
                    btnDelete.Visible = false;
                }
                else
                    Master.ShowMessage(new AppMessage("Warning: Unable to Delete Store Request ", Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Warning: Unable to Delete Store Request " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void ddlFAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            TextBox txtAccountCode = ddl.FindControl("txtFAccountCode") as TextBox;
            txtAccountCode.Text = _presenter.GetItemAccount(Convert.ToInt32(ddl.SelectedValue)).AccountCode;
           
        }        
      
        #region StoreRequestDetail
        private void BindStoreRequestDetails()
        {
            dgStoreRequestDetail.DataSource = _presenter.CurrentStoreRequest.StoreRequestDetails;
            dgStoreRequestDetail.DataBind();
        }
        protected void dgStoreRequestDetail_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            dgStoreRequestDetail.EditItemIndex = -1;
            BindStoreRequestDetails();
        }
        protected void dgStoreRequestDetail_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgStoreRequestDetail.DataKeys[e.Item.ItemIndex];
            int PRDId = (int)dgStoreRequestDetail.DataKeys[e.Item.ItemIndex];
            StoreRequestDetail prd;

            if (PRDId > 0)
                prd = _presenter.CurrentStoreRequest.GetStoreRequestDetail(PRDId);
            else
                prd = _presenter.CurrentStoreRequest.StoreRequestDetails[e.Item.ItemIndex];
            try
            {
                if (PRDId > 0)
                {
                    _presenter.CurrentStoreRequest.RemoveStoreRequestDetail(id);
                    _presenter.DeleteStoreRequestDetail(_presenter.GetStoreRequestDetail(id));
                    //  _presenter.CurrentStoreRequest.TotalPrice = _presenter.CurrentStoreRequest.TotalPrice - prd.EstimatedCost;
                    //  txtTotal.Text = (_presenter.CurrentStoreRequest.TotalPrice).ToString();
                    _presenter.SaveOrUpdateStoreRequest(_presenter.CurrentStoreRequest);
                }
                else
                {
                    _presenter.CurrentStoreRequest.StoreRequestDetails.Remove(prd);
                    //  _presenter.CurrentStoreRequest.TotalPrice = _presenter.CurrentStoreRequest.TotalPrice - prd.EstimatedCost;
                    //  txtTotal.Text = (_presenter.CurrentStoreRequest.TotalPrice).ToString();
                }
                BindStoreRequestDetails();

                Master.ShowMessage(new AppMessage("Store Request Detail was Removed Successfully", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Store Request Detail. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgStoreRequestDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgStoreRequestDetail.EditItemIndex = e.Item.ItemIndex;
            BindStoreRequestDetails();
        }
        protected void dgStoreRequestDetail_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "AddNew")
            {
                try
                {
                    StoreRequestDetail Detail = new StoreRequestDetail();

                    DropDownList ddlFItem = e.Item.FindControl("ddlFItem") as DropDownList;
                    Detail.Item = _presenter.GetItem(Convert.ToInt32(ddlFItem.SelectedValue));
                    TextBox txtFQty = e.Item.FindControl("txtFQty") as TextBox;
                    Detail.Qty = Convert.ToInt32(txtFQty.Text);
                    //TextBox txtFQtyApp = e.Item.FindControl("txtFQtyApp") as TextBox;
                    Detail.QtyApproved = Convert.ToInt32(0);
                    DropDownList ddlFUnitOfMeasurment = e.Item.FindControl("ddlFUnitOfMeasurment") as DropDownList;
                    Detail.UnitOfMeasurment = ddlFUnitOfMeasurment.SelectedValue;
                    TextBox txtFRemark = e.Item.FindControl("txtFRemark") as TextBox;
                    Detail.Remark = txtFRemark.Text;
                    DropDownList ddlFProject = e.Item.FindControl("ddlFProject") as DropDownList;
                    Detail.Project = _presenter.GetProject(int.Parse(ddlFProject.SelectedValue));
                    DropDownList ddlFGrant = e.Item.FindControl("ddlFGrant") as DropDownList;
                    Detail.Grant = _presenter.GetGrant(int.Parse(ddlFGrant.SelectedValue));
                    
                    Detail.StoreRequest = _presenter.CurrentStoreRequest;
                    _presenter.CurrentStoreRequest.StoreRequestDetails.Add(Detail);
                    Master.ShowMessage(new AppMessage("Store Request Detail added successfully.", RMessageType.Info));
                    dgStoreRequestDetail.EditItemIndex = -1;
                    BindStoreRequestDetails();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Store Request Detail. " + ex.Message, RMessageType.Error));
                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
                }
            }
        }
        protected void dgStoreRequestDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {

                DropDownList ddlProject = e.Item.FindControl("ddlFProject") as DropDownList;
                int programID = Convert.ToInt32(ddlProgram.SelectedValue);
                BindProject(ddlProject, programID);
                DropDownList ddlGrant = e.Item.FindControl("ddlFGrant") as DropDownList;
                BindGrant(ddlGrant, Convert.ToInt32(ddlProject.SelectedValue));
                DropDownList ddlItem = e.Item.FindControl("ddlFItem") as DropDownList;
                BindItems(ddlItem);
             
            }
            else
            {
                DropDownList ddlProject = e.Item.FindControl("ddlEdtProject") as DropDownList;
                int programID = Convert.ToInt32(ddlProgram.SelectedValue);
                if (ddlProject != null)
                {
                    BindProject(ddlProject, programID);
                    if (_presenter.CurrentStoreRequest.StoreRequestDetails[e.Item.DataSetIndex].Project.Id != 0)
                    {
                        ListItem liI = ddlProject.Items.FindByValue(_presenter.CurrentStoreRequest.StoreRequestDetails[e.Item.DataSetIndex].Project.Id.ToString());
                        if (liI != null)
                            liI.Selected = true;
                    }
                }
                DropDownList ddlItem = e.Item.FindControl("ddlItem") as DropDownList;
                if (ddlItem != null)
                {
                    BindItem(ddlItem);
                    if (_presenter.CurrentStoreRequest.StoreRequestDetails[e.Item.DataSetIndex].Item != null)
                    {
                        ListItem liI = ddlItem.Items.FindByValue(_presenter.CurrentStoreRequest.StoreRequestDetails[e.Item.DataSetIndex].Item.Id.ToString());
                        if (liI != null)
                            liI.Selected = true;
                    }
                }


                //if (_presenter.CurrentStoreRequest.StoreRequestDetails != null)
                //{


                //    DropDownList ddlProject = e.Item.FindControl("ddlProject") as DropDownList;

                //    if (ddlProject != null)
                //    {
                //        BindProject(ddlProject);

                //        if (_presenter.CurrentStoreRequest.StoreRequestDetails[e.Item.DataSetIndex].Project != null)
                //        {
                //            ListItem li = ddlProject.Items.FindByValue(_presenter.CurrentStoreRequest.StoreRequestDetails[e.Item.DataSetIndex].Project.Id.ToString());
                //            if (li != null)
                //                li.Selected = true;
                //        }
                //    }
                //    DropDownList ddlGrant = e.Item.FindControl("ddlGrant") as DropDownList;
                //    if (ddlGrant != null)
                //    {
                //        BindGrant(ddlGrant, Convert.ToInt32(ddlProject.SelectedValue));
                //        if (_presenter.CurrentStoreRequest.StoreRequestDetails[e.Item.DataSetIndex].Grant != null)
                //        {
                //            ListItem liI = ddlGrant.Items.FindByValue(_presenter.CurrentStoreRequest.StoreRequestDetails[e.Item.DataSetIndex].Grant.Id.ToString());
                //            if (liI != null)
                //                liI.Selected = true;
                //        }
                //    }
                //}
            }
        }
        protected void dgStoreRequestDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgStoreRequestDetail.DataKeys[e.Item.ItemIndex];
            StoreRequestDetail Detail;
            if (id > 0)
                Detail = _presenter.CurrentStoreRequest.GetStoreRequestDetail(id);
            else
                Detail = _presenter.CurrentStoreRequest.StoreRequestDetails[e.Item.ItemIndex];

            try
            {

                DropDownList ddlItem = e.Item.FindControl("ddlItem") as DropDownList;
                Detail.Item = _presenter.GetItem(Convert.ToInt32(ddlItem.SelectedValue));
                TextBox txtQty = e.Item.FindControl("txtQty") as TextBox;
                Detail.Qty = Convert.ToInt32(txtQty.Text);
                //TextBox txtQtyApp = e.Item.FindControl("txtQtyApp") as TextBox;
                Detail.QtyApproved = Convert.ToInt32(0);
                DropDownList ddlUnitOfMeasurment = e.Item.FindControl("ddlUnitOfMeasurment") as DropDownList;
                Detail.UnitOfMeasurment = ddlUnitOfMeasurment.SelectedValue;
                TextBox txtRemark = e.Item.FindControl("txtRemark") as TextBox;
                Detail.Remark = txtRemark.Text;
                DropDownList ddlProject = e.Item.FindControl("ddlProject") as DropDownList;
                Detail.Project = _presenter.GetProject(int.Parse(ddlProject.SelectedValue));
                DropDownList ddlGrant = e.Item.FindControl("ddlGrant") as DropDownList;
                Detail.Grant = _presenter.GetGrant(int.Parse(ddlGrant.SelectedValue));
                Detail.StoreRequest = _presenter.CurrentStoreRequest;
                Master.ShowMessage(new AppMessage("Store Request Detail  Updated successfully.", RMessageType.Info));
                dgStoreRequestDetail.EditItemIndex = -1;
                BindStoreRequestDetails();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Store Request Detail. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        #endregion
        private void BindItem(DropDownList ddlItem)
        {
            ddlItem.DataSource = _presenter.GetItems();
            ddlItem.DataBind();

        }
        protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindStoreRequestDetails();
        }
        protected void btnCreateStoreItem_Click(object sender, EventArgs e)
        {
            _presenter.CurrentStoreRequest.StoreRequestDetails.Clear();
            foreach (GridViewRow item in grvDetails.Rows)
            {
                int prId = (int)grvDetails.DataKeys[item.RowIndex].Value;
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkSelect");
                    if (chk.Checked)
                    {
                        if (_presenter.GetPurchaseRequestDetail(prId) != null)
                        {
                            PurchaseRequestDetail prd = _presenter.GetPurchaseRequestDetail(prId);

                            StoreRequestDetail srd = new StoreRequestDetail();

                            srd.Item = prd.Item;

                            _presenter.CurrentStoreRequest.StoreRequestDetails.Add(srd);                            
                        }
                    }
                }
            }
            dgStoreRequestDetail.DataSource = _presenter.CurrentStoreRequest.StoreRequestDetails;
            dgStoreRequestDetail.DataBind();
        }
    }
}