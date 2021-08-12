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
using Chai.WorkflowManagment.CoreDomain.Request;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public partial class frmStoreApproval : POCBasePage, IStoreApprovalView
    {
        private StoreApprovalPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        private bool needsApproval = false;
        private int reqID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                PopProgressStatus();
                //  BindPurchases();
                //BindSearchStoreRequestGrid();
            }
            this._presenter.OnViewLoaded();
            if (!this.IsPostBack)
            {
                BindSearchStoreRequestGrid();
            }
            if (_presenter.CurrentStoreRequest != null)
            {
                if (_presenter.CurrentStoreRequest.Id != 0)
                {
                    PrintTransaction();
                }
            }

        }
        [CreateNew]
        public StoreApprovalPresenter Presenter
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
        public int StoreRequestId
        {
            get
            {
                if (grvStoreRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvStoreRequestList.SelectedDataKey.Value);
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

            ddlSrchProgressStatus.Items.Clear();
            for (int i = 0; i < s.Length; i++)
            {
                ddlSrchProgressStatus.Items.Add(new ListItem(s[i].Replace('_', ' '), s[i].Replace('_', ' ')));
                ddlSrchProgressStatus.DataBind();
            }
        }
        private string GetWillStatus()
        {
            ApprovalSetting AS = _presenter.GetApprovalSetting(RequestType.Store_Request.ToString().Replace('_', ' ').ToString(), 0);
            string will = "";
            foreach (ApprovalLevel AL in AS.ApprovalLevels)
            {
                if (AL.EmployeePosition.PositionName == "Superviser/Line Manager" || AL.EmployeePosition.PositionName == "Program Manager" && _presenter.CurrentStoreRequest.CurrentLevel == 1)
                {
                    will = "Approve";
                    break;

                }
                else if (_presenter.GetUser(_presenter.CurrentStoreRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName)
                {
                    will = AL.Will;
                }

            }
            return will;
        }
        private void BindItem(DropDownList ddlItem)
        {
            ddlItem.Items.Clear();
            ListItem lst = new ListItem();
            lst.Text = "Select Item";
            lst.Value = "";
            ddlItem.Items.Add(lst);
            ddlItem.DataSource = _presenter.GetItems();
            ddlItem.DataBind();
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
        private void BindGrant(TextBox txtGrant, int projectId)
        {
            txtGrant.Text = _presenter.GetGrantbyprojectId(projectId).First().GrantCode;
        }
        private void BindSearchStoreRequestGrid()
        {
            grvStoreRequestList.DataSource = _presenter.ListStoreRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text, ddlSrchProgressStatus.SelectedValue);
            grvStoreRequestList.DataBind();
        }
        private void BindStoreRequestStatus()
        {
            foreach (StoreRequestStatus PRS in _presenter.CurrentStoreRequest.StoreRequestStatuses)
            {
                if (PRS.WorkflowLevel == _presenter.CurrentStoreRequest.CurrentLevel && _presenter.CurrentStoreRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    btnApprove.Enabled = true;
                }

                if (_presenter.CurrentStoreRequest.CurrentLevel == _presenter.CurrentStoreRequest.StoreRequestStatuses.Count && !String.IsNullOrEmpty(PRS.ApprovalStatus))
                {
                    btnPrint.Enabled = true;
                    btnApprove.Enabled = false;
                    ddlApprovalStatus.Enabled = false;
                }
                else
                {
                    btnPrint.Enabled = true;
                    btnApprove.Enabled = true;
                    ddlApprovalStatus.Enabled = true;
                }
            }

        }
        private void ShowPrint()
        {
            if (_presenter.CurrentStoreRequest.CurrentLevel == _presenter.CurrentStoreRequest.StoreRequestStatuses.Count)
            {
                btnPrint.Enabled = true;
            }
        }
        private void SendEmail(StoreRequestStatus PRS)
        {
            if (_presenter.GetUser(PRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(_presenter.GetUser(PRS.Approver).Email, "Store Request", _presenter.GetUser(_presenter.CurrentStoreRequest.Requester).FullName + " Requests for Store with Request No. " + (_presenter.CurrentStoreRequest.RequestNo).ToUpper());
            }
            else
            {
                EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(PRS.Approver).AssignedTo).Email, "Store Request", _presenter.GetUser(_presenter.CurrentStoreRequest.Requester).FullName + "Requests for Store with Request No." + (_presenter.CurrentStoreRequest.RequestNo).ToUpper());
            }
        }
        private void SendEmailRejected(StoreRequestStatus PRS)
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentStoreRequest.Requester).Email, "Store Request Rejection", "Your Store Request with Request No. - '" + (_presenter.CurrentStoreRequest.RequestNo.ToString()).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (PRS.RejectedReason).ToUpper() + "'");

            if (PRS.WorkflowLevel > 1)
            {
                for (int i = 0; i + 1 < PRS.WorkflowLevel; i++)
                {
                    EmailSender.Send(_presenter.GetUser(_presenter.CurrentStoreRequest.StoreRequestStatuses[i].Approver).Email, "Store Request Rejection", "Store Request with Request No. - '" + (_presenter.CurrentStoreRequest.RequestNo.ToString()).ToUpper() + "' made by " + (_presenter.GetUser(_presenter.CurrentStoreRequest.Requester).FullName).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (PRS.RejectedReason).ToUpper() + "'");
                }
            }
        }
        private void GetNextApprover()
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
        private void SaveStoreRequestStatus()
        {
            foreach (StoreRequestStatus PRRS in _presenter.CurrentStoreRequest.StoreRequestStatuses)
            {
                if ((PRRS.Approver == _presenter.CurrentUser().Id || _presenter.CurrentUser().Id == (_presenter.GetAssignedJobbycurrentuser(PRRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(PRRS.Approver).AssignedTo : 0)) && PRRS.WorkflowLevel == _presenter.CurrentStoreRequest.CurrentLevel)
                {
                    PRRS.ApprovalStatus = ddlApprovalStatus.SelectedValue;
                    PRRS.RejectedReason = txtRejectedReason.Text;
                    PRRS.ApprovalDate = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                    if (PRRS.ApprovalStatus != ApprovalStatus.Rejected.ToString())
                    {
                        if (_presenter.CurrentStoreRequest.CurrentLevel == _presenter.CurrentStoreRequest.StoreRequestStatuses.Count)
                        {
                            _presenter.CurrentStoreRequest.CurrentApprover = PRRS.Approver;
                            _presenter.CurrentStoreRequest.CurrentLevel = PRRS.WorkflowLevel;
                            _presenter.CurrentStoreRequest.CurrentStatus = PRRS.ApprovalStatus;
                            _presenter.CurrentStoreRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        }
                        GetNextApprover();
                        PRRS.Approver = _presenter.CurrentUser().Id;
                        Log.Info(_presenter.GetUser(PRRS.Approver).FullName + " has " + PRRS.ApprovalStatus + " Store Request made by " + _presenter.GetUser(_presenter.CurrentStoreRequest.Requester).FullName);
                    }
                    else if (PRRS.ApprovalStatus == ApprovalStatus.Canceled.ToString())
                    {
                        _presenter.CurrentStoreRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        _presenter.CurrentStoreRequest.CurrentStatus = PRRS.ApprovalStatus;
                        PRRS.Approver = _presenter.CurrentUser().Id;
                        SendCanceledEmail();
                    }
                    else
                    {
                        _presenter.CurrentStoreRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        _presenter.CurrentStoreRequest.CurrentStatus = PRRS.ApprovalStatus;
                        SendEmailRejected(PRRS);
                    }
                    break;
                }

            }
        }
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            TextBox txtGrant = ddl.FindControl("txtGrant") as TextBox;
            BindGrant(txtGrant, Convert.ToInt32(ddl.SelectedValue));
            pnlDetail_ModalPopupExtender.Show();
        }
        protected void ddlAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            TextBox txtAccountCode = ddl.FindControl("txtAccountCode") as TextBox;
            txtAccountCode.Text = _presenter.GetItemAccount(Convert.ToInt32(ddl.SelectedValue)).AccountCode;
            pnlDetail_ModalPopupExtender.Show();
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindSearchStoreRequestGrid();
        }
        private void PrintTransaction()
        {
            lblRequestNoResult.Text = _presenter.CurrentStoreRequest.RequestNo.ToString();
            lblRequestedDateResult.Text = _presenter.CurrentStoreRequest.RequestedDate.ToShortDateString();
            lblRequesterResult.Text = _presenter.GetUser(_presenter.CurrentStoreRequest.Requester).FullName;


            lblRemarkResult.Text = _presenter.CurrentStoreRequest.Comment;
            lblDelivertoResult.Text = _presenter.CurrentStoreRequest.DeliverTo;

            grvDetails.DataSource = _presenter.CurrentStoreRequest.StoreRequestDetails;
            grvDetails.DataBind();

            grvStatuses.DataSource = _presenter.CurrentStoreRequest.StoreRequestStatuses;
            grvStatuses.DataBind();


        }
        private void SendCanceledEmail()
        {
            StoreRequest thisRequest = _presenter.CurrentStoreRequest;
            //To the requester
            EmailSender.Send(_presenter.GetUser(thisRequest.Requester).Email, "Store Request Canceled", " Your Store Request with Request No. " + thisRequest.RequestNo + " was Canceled!");
            //To the approvers
            foreach (StoreRequestStatus statuses in thisRequest.StoreRequestStatuses)
            {
                EmailSender.Send(_presenter.GetUser(statuses.Approver).Email, "Store  Request Canceled", " The Store  Request with Request No. " + thisRequest.RequestNo + " has been Canceled!");
            }
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentStoreRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    SaveStoreRequestStatus();
                    _presenter.SaveOrUpdateStoreRequest(_presenter.CurrentStoreRequest);
                    ShowPrint();
                    Master.ShowMessage(new AppMessage("Store Approval Processed", RMessageType.Info));
                    btnApprove.Enabled = false;
                    BindSearchStoreRequestGrid();
                    if (_presenter.CurrentUser().EmployeePosition.PositionName == "Logistic Assistant" && _presenter.CurrentStoreRequest.CurrentStatus != ApprovalStatus.Rejected.ToString())
                    {
                        //lnkBidRequest.Visible = true;
                        // lnkSoleVendor.Visible = true;
                    }
                    ScriptManager.RegisterStartupScript(this, GetType(), "showApprovalModal", "showApprovalModal();", true);
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: While Approving Purchase Request!", RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void grvStoreRequestList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            reqID = (int)grvStoreRequestList.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;
            if (e.CommandName == "ViewItem")
            {
                Session["storeReqId"] = reqID;
                _presenter.CurrentStoreRequest = _presenter.GetStoreRequestById(reqID);
                //_presenter.OnViewLoaded();
                dgStoreRequestDetail.DataSource = _presenter.CurrentStoreRequest.StoreRequestDetails;
                dgStoreRequestDetail.DataBind();
                pnlDetail_ModalPopupExtender.Show();
            }
            else if (e.CommandName == "Issue")
            {
                Response.Redirect("../Inventory/frmIssue.aspx?StoreReqId=" + reqID);
            }
        }
        protected void grvStoreRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //grvPaymentReimbursementRequestList.SelectedDataKey.Value
            _presenter.OnViewLoaded();
            PopApprovalStatus();
            //grvAttachments.DataSource = _presenter.CurrentPaymentReimbursementRequest.CPRAttachments;
            //grvAttachments.DataBind();
            BindStoreRequestStatus();
            PrintTransaction();
            txtRejectedReason.Visible = false;
            rfvRejectedReason.Enabled = false;
            ScriptManager.RegisterStartupScript(this, GetType(), "showApprovalModal", "showApprovalModal();", true);
        }
        private void PopApprovalStatus()
        {
            ddlApprovalStatus.Items.Clear();
            ddlApprovalStatus.Items.Add(new ListItem("Select Status", "0"));
            string[] s = Enum.GetNames(typeof(ApprovalStatus));

            for (int i = 0; i < s.Length; i++)
            {
                if (GetWillStatus().Substring(0, 2) == s[i].Substring(0, 2))
                {
                    if (s[i] != ApprovalStatus.Rejected.ToString())
                        ddlApprovalStatus.Items.Add(new ListItem(s[i].Replace('_', ' '), s[i].Replace('_', ' ')));
                }

            }
            ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Rejected.ToString().Replace('_', ' '), ApprovalStatus.Rejected.ToString().Replace('_', ' ')));
            if (_presenter.CurrentStoreRequest.ProgressStatus == ProgressStatus.Completed.ToString())
            {
                ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Canceled.ToString().Replace('_', ' '), ApprovalStatus.Canceled.ToString().Replace('_', ' ')));
            }

        }
        protected void grvStoreRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Button btnStatus = e.Row.FindControl("btnStatus") as Button;
            StoreRequest CSR = e.Row.DataItem as StoreRequest;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnIssue = e.Row.Cells[5].Controls[0] as Button;
                btnIssue.Visible = false;

                if (CSR.ProgressStatus == ProgressStatus.InProgress.ToString())
                {
                    btnStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFF6C");
                }
                else if (CSR.ProgressStatus == ProgressStatus.Completed.ToString())
                {
                    btnStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF7251");
                    btnIssue.Visible = true;
                }
                e.Row.Cells[1].Text = _presenter.GetUser(CSR.Requester).FullName;

            }
        }
        protected void ddlApprovalStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlApprovalStatus.SelectedValue == "Rejected" || ddlApprovalStatus.SelectedValue == "Canceled")
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
        protected void grvStoreRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvStoreRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentStoreRequest.StoreRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_presenter.CurrentStoreRequest.StoreRequestStatuses[e.Row.RowIndex].Approver != 0)
                        e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentStoreRequest.StoreRequestStatuses[e.Row.RowIndex].Approver).FullName;
                }
            }
        }

        #region StoreRequestDetail
        private void BindStoreRequestDetails()
        {
            _presenter.CurrentStoreRequest = _presenter.GetStoreRequestById((int)Session["storeReqId"]);
            dgStoreRequestDetail.DataSource = _presenter.CurrentStoreRequest.StoreRequestDetails;
            dgStoreRequestDetail.DataBind();

        }
        protected void dgStoreRequestDetail_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgStoreRequestDetail.EditItemIndex = -1;
            BindStoreRequestDetails();
        }
        protected void dgStoreRequestDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgStoreRequestDetail.EditItemIndex = e.Item.ItemIndex;
            BindStoreRequestDetails();
            pnlDetail_ModalPopupExtender.Show();
        }
        protected void dgStoreRequestDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (_presenter.CurrentStoreRequest.StoreRequestDetails != null)
            {
                DropDownList ddlItem = e.Item.FindControl("ddlItem") as DropDownList;

                if (ddlItem != null)
                {
                    BindItem(ddlItem);

                    if (_presenter.CurrentStoreRequest.StoreRequestDetails[e.Item.DataSetIndex].Item != null)
                    {
                        ListItem li = ddlItem.Items.FindByValue(_presenter.CurrentStoreRequest.StoreRequestDetails[e.Item.DataSetIndex].Item.Id.ToString());
                        if (li != null)
                            li.Selected = true;
                    }
                }

                DropDownList ddlProject = e.Item.FindControl("ddlProject") as DropDownList;

                if (ddlProject != null)
                {
                    BindProject(ddlProject);

                    if (_presenter.CurrentStoreRequest.StoreRequestDetails[e.Item.DataSetIndex].Project != null)
                    {
                        ListItem li = ddlProject.Items.FindByValue(_presenter.CurrentStoreRequest.StoreRequestDetails[e.Item.DataSetIndex].Project.Id.ToString());
                        if (li != null)
                            li.Selected = true;
                    }
                }
                DropDownList ddlUnitOfMeasurment = e.Item.FindControl("ddlUnitOfMeasurment") as DropDownList;
                if (ddlUnitOfMeasurment != null)
                {
                    ListItem liI = ddlUnitOfMeasurment.Items.FindByValue(_presenter.CurrentStoreRequest.StoreRequestDetails[e.Item.DataSetIndex].UnitOfMeasurment);
                    if (liI != null)
                        liI.Selected = true;
                }

                TextBox txtApproved = e.Item.FindControl("txtQtyApp") as TextBox;
                if (txtApproved != null)
                    txtApproved.Text = Convert.ToInt32(_presenter.CurrentStoreRequest.StoreRequestDetails[e.Item.DataSetIndex].Qty).ToString();

            }

        }
        protected void dgStoreRequestDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgStoreRequestDetail.DataKeys[e.Item.ItemIndex];
            _presenter.CurrentStoreRequest = _presenter.GetStoreRequestById((int)Session["storeReqId"]);
            StoreRequestDetail Detail;
            if (id > 0)
                Detail = _presenter.CurrentStoreRequest.GetStoreRequestDetail(id);
            else
                Detail = _presenter.CurrentStoreRequest.StoreRequestDetails[e.Item.ItemIndex];

            try
            {
                DropDownList ddlAccount = e.Item.FindControl("ddlAccount") as DropDownList;

                DropDownList ddlItem = e.Item.FindControl("ddlItem") as DropDownList;
                Detail.Item = _presenter.GetItem(Convert.ToInt32(ddlItem.SelectedValue));
                TextBox txtQtyApp = e.Item.FindControl("txtQtyApp") as TextBox;
                Detail.QtyApproved = Convert.ToInt32(txtQtyApp.Text);
                DropDownList ddlUnitOfMeasurment = e.Item.FindControl("ddlUnitOfMeasurment") as DropDownList;
                Detail.UnitOfMeasurment = ddlUnitOfMeasurment.SelectedValue;
                TextBox txtRemark = e.Item.FindControl("txtRemark") as TextBox;
                Detail.Remark = txtRemark.Text;
                DropDownList ddlProject = e.Item.FindControl("ddlProject") as DropDownList;
                Detail.Project = _presenter.GetProject(int.Parse(ddlProject.SelectedValue));
                TextBox txtGrant = e.Item.FindControl("txtGrant") as TextBox;
                Detail.Grant = _presenter.GetGrantByCode(txtGrant.Text);
                Detail.StoreRequest = _presenter.CurrentStoreRequest;
                Master.ShowMessage(new AppMessage("Store Request Detail  Updated successfully.", RMessageType.Info));
                dgStoreRequestDetail.EditItemIndex = -1;
                BindStoreRequestDetails();
                pnlDetail_ModalPopupExtender.Show();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Store Request Detail. " + ex.Message, RMessageType.Error));
            }
        }
        #endregion
    }
}