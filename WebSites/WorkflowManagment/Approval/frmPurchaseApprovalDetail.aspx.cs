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
    public partial class frmPurchaseApprovalDetail : POCBasePage, IPurchaseApprovalDetailView
    {
        private PurchaseApprovalDetailPresenter _presenter;
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
                //BindSearchPurchaseRequestGrid();
            }
            this._presenter.OnViewLoaded();
            if (!this.IsPostBack)
            {
                BindSearchPurchaseRequestGrid();
            }
            if (_presenter.CurrentPurchaseRequest != null)
            {
                if (_presenter.CurrentPurchaseRequest.Id != 0)
                {
                    PrintTransaction();
                }
            }
            lnkBidRequest.Visible = false;
            lnkSoleVendor.Visible = false;
        }
        [CreateNew]
        public PurchaseApprovalDetailPresenter Presenter
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
        public int PurchaseRequestId
        {
            get
            {
                if (grvPurchaseRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvPurchaseRequestList.SelectedDataKey.Value);
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
            ApprovalSetting AS = _presenter.GetApprovalSetting(RequestType.Purchase_Request.ToString().Replace('_', ' ').ToString(), 0);
            string will = "";
            foreach (ApprovalLevel AL in AS.ApprovalLevels)
            {
                if (AL.EmployeePosition.PositionName == "Superviser/Line Manager" || AL.EmployeePosition.PositionName == "Program Manager" && _presenter.CurrentPurchaseRequest.CurrentLevel == 1)
                {
                    will = "Approve";
                    break;

                }
                else if (_presenter.GetUser(_presenter.CurrentPurchaseRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName)
                {
                    will = AL.Will;
                }

            }
            return will;
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
        private void BindSearchPurchaseRequestGrid()
        {
            grvPurchaseRequestList.DataSource = _presenter.ListPurchaseRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text, ddlSrchProgressStatus.SelectedValue);
            grvPurchaseRequestList.DataBind();
        }
        private void BindPurchaseRequestStatus()
        {
            foreach (PurchaseRequestStatus PRS in _presenter.CurrentPurchaseRequest.PurchaseRequestStatuses)
            {
                if (PRS.WorkflowLevel == _presenter.CurrentPurchaseRequest.CurrentLevel && _presenter.CurrentPurchaseRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    btnApprove.Enabled = true;
                }

                if (_presenter.CurrentPurchaseRequest.CurrentLevel == _presenter.CurrentPurchaseRequest.PurchaseRequestStatuses.Count && !String.IsNullOrEmpty(PRS.ApprovalStatus))
                {
                    btnPrint.Enabled = true;
                    btnApprove.Enabled = false;
                    ddlApprovalStatus.Enabled = false;
                }
                else
                {
                    btnPrint.Enabled = false;
                    btnApprove.Enabled = true;
                    ddlApprovalStatus.Enabled = true;
                }
            }
            if (_presenter.CurrentUser().EmployeePosition.PositionName == "Procurement Officer" && _presenter.CurrentPurchaseRequest.CurrentStatus != ApprovalStatus.Rejected.ToString())
            {
                lnkBidRequest.Visible = true;
                lnkSoleVendor.Visible = true;
            }
        }
        private void ShowPrint()
        {
            if (_presenter.CurrentPurchaseRequest.CurrentLevel == _presenter.CurrentPurchaseRequest.PurchaseRequestStatuses.Count)
            {
                btnPrint.Enabled = true;
            }
        }
        private void SendEmail(PurchaseRequestStatus PRS)
        {
            if (_presenter.GetUser(PRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(_presenter.GetUser(PRS.Approver).Email, "Purchase Request", _presenter.GetUser(_presenter.CurrentPurchaseRequest.Requester).FullName + " Requests for Purchase with Request No. " + (_presenter.CurrentPurchaseRequest.RequestNo).ToUpper());
            }
            else
            {
                EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(PRS.Approver).AssignedTo).Email, "Purchase Request", _presenter.GetUser(_presenter.CurrentPurchaseRequest.Requester).FullName + "Requests for Purchase with Request No." + (_presenter.CurrentPurchaseRequest.RequestNo).ToUpper());
            }
        }
        private void SendEmailToRequester()
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentPurchaseRequest.Requester).Email, "Purchase Request ", "Your Purchase Request with Purchase Request No. - '" + (_presenter.CurrentPurchaseRequest.RequestNo).ToUpper() + "' was Completed and a bid process is initiated.");
        }
        private void SendEmailRejected(PurchaseRequestStatus PRS)
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentPurchaseRequest.Requester).Email, "Purchase Request Rejection", "Your Purchase Request with Request No. - '" + (_presenter.CurrentPurchaseRequest.RequestNo.ToString()).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (PRS.RejectedReason).ToUpper() + "'");

            if (PRS.WorkflowLevel > 1)
            {
                for (int i = 0; i + 1 < PRS.WorkflowLevel; i++)
                {
                    EmailSender.Send(_presenter.GetUser(_presenter.CurrentPurchaseRequest.PurchaseRequestStatuses[i].Approver).Email, "Purchase Request Rejection", "Purchase Request with Request No. - '" + (_presenter.CurrentPurchaseRequest.RequestNo.ToString()).ToUpper() + "' made by " + (_presenter.GetUser(_presenter.CurrentPurchaseRequest.Requester).FullName).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (PRS.RejectedReason).ToUpper() + "'");
                }
            }
        }
        private void GetNextApprover()
        {
            foreach (PurchaseRequestStatus PRS in _presenter.CurrentPurchaseRequest.PurchaseRequestStatuses)
            {
                if (PRS.ApprovalStatus == null)
                {
                    // SendEmail(PRS);
                    _presenter.CurrentPurchaseRequest.CurrentApprover = PRS.Approver;
                    _presenter.CurrentPurchaseRequest.CurrentLevel = PRS.WorkflowLevel;
                    _presenter.CurrentPurchaseRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                    break;

                }
            }
        }
        private void SavePurchaseRequestStatus()
        {
            foreach (PurchaseRequestStatus PRRS in _presenter.CurrentPurchaseRequest.PurchaseRequestStatuses)
            {
                if ((PRRS.Approver == _presenter.CurrentUser().Id || _presenter.CurrentUser().Id == (_presenter.GetAssignedJobbycurrentuser(PRRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(PRRS.Approver).AssignedTo : 0)) && PRRS.WorkflowLevel == _presenter.CurrentPurchaseRequest.CurrentLevel)
                {
                    PRRS.ApprovalStatus = ddlApprovalStatus.SelectedValue;
                    PRRS.RejectedReason = txtRejectedReason.Text;
                    PRRS.ApprovalDate = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                    if (PRRS.ApprovalStatus != ApprovalStatus.Rejected.ToString())
                    {
                        if (_presenter.CurrentPurchaseRequest.CurrentLevel == _presenter.CurrentPurchaseRequest.PurchaseRequestStatuses.Count)
                        {
                            _presenter.CurrentPurchaseRequest.CurrentApprover = PRRS.Approver;
                            _presenter.CurrentPurchaseRequest.CurrentLevel = PRRS.WorkflowLevel;
                            _presenter.CurrentPurchaseRequest.CurrentStatus = PRRS.ApprovalStatus;
                            //Completed is commented out because we're going to complete purchase request
                            //when Bid or SoleVendor is completed. 

                            //_presenter.CurrentPurchaseRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                            SendEmailToRequester();
                        }
                        GetNextApprover();
                        PRRS.Approver = _presenter.CurrentUser().Id;
                        Log.Info(_presenter.GetUser(PRRS.Approver).FullName + " has " + PRRS.ApprovalStatus + " Purchase Request made by " + _presenter.GetUser(_presenter.CurrentPurchaseRequest.Requester).FullName);
                        SendEmail(PRRS);
                    }
                    else if (PRRS.ApprovalStatus == ApprovalStatus.Canceled.ToString())
                    {
                        _presenter.CurrentPurchaseRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        _presenter.CurrentPurchaseRequest.CurrentStatus = PRRS.ApprovalStatus;
                        PRRS.Approver = _presenter.CurrentUser().Id;
                        SendCanceledEmail();
                    }
                    else
                    {
                        _presenter.CurrentPurchaseRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        SendEmailRejected(PRRS);
                    }
                    break;
                }

            }
        }
        protected void lnkBidRequest_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("../Request/frmBidAnalysisRequest.aspx?PurchaseRequestId={0}", _presenter.CurrentPurchaseRequest.Id));
        }
        protected void lnkSoleVendor_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("../Request/frmSoleVendorRequest.aspx?PurchaseRequestId={0}", _presenter.CurrentPurchaseRequest.Id));
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
            BindSearchPurchaseRequestGrid();
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            pnlApproval.Visible = false;
        }
        private void PrintTransaction()
        {
            lblRequestNoResult.Text = _presenter.CurrentPurchaseRequest.RequestNo.ToString();
            lblRequestedDateResult.Text = _presenter.CurrentPurchaseRequest.RequestedDate.ToShortDateString();
            lblRequesterResult.Text = _presenter.GetUser(_presenter.CurrentPurchaseRequest.Requester).FullName;
            lblSuggestedSupplierResult.Text = _presenter.CurrentPurchaseRequest.SuggestedSupplier.ToString();

            lblRemarkResult.Text = _presenter.CurrentPurchaseRequest.Comment;
            lblDelivertoResult.Text = _presenter.CurrentPurchaseRequest.DeliverTo;
            lblRequestedDateResult.Text = _presenter.CurrentPurchaseRequest.Requireddateofdelivery.ToShortDateString();
            grvDetails.DataSource = _presenter.CurrentPurchaseRequest.PurchaseRequestDetails;
            grvDetails.DataBind();

            grvStatuses.DataSource = _presenter.CurrentPurchaseRequest.PurchaseRequestStatuses;
            grvStatuses.DataBind();


        }
        private void SendCanceledEmail()
        {
            PurchaseRequest thisRequest = _presenter.CurrentPurchaseRequest;
            //To the requester
            EmailSender.Send(_presenter.GetUser(thisRequest.Requester).Email, "Purchase Request Canceled", " Your Purchase Request with Request No. " + thisRequest.RequestNo + " was Canceled!");
            //To the approvers
            foreach (PurchaseRequestStatus statuses in thisRequest.PurchaseRequestStatuses)
            {
                EmailSender.Send(_presenter.GetUser(statuses.Approver).Email, "Purchase  Request Canceled", " The Purchase  Request with Request No. " + thisRequest.RequestNo + " has been Canceled!");
            }
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentPurchaseRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    SavePurchaseRequestStatus();
                    _presenter.SaveOrUpdatePurchaseRequest(_presenter.CurrentPurchaseRequest);
                    ShowPrint();
                    Master.ShowMessage(new AppMessage("Purchase Approval Processed", RMessageType.Info));
                    btnApprove.Enabled = false;
                    BindSearchPurchaseRequestGrid();
                    if (_presenter.CurrentUser().EmployeePosition.PositionName == "Procurement Officer" && _presenter.CurrentPurchaseRequest.CurrentStatus != ApprovalStatus.Rejected.ToString())
                    {
                        lnkBidRequest.Visible = true;
                        lnkSoleVendor.Visible = true;
                    }
                    pnlApproval_ModalPopupExtender.Show();
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: While Approving Purchase Request!", RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void grvPurchaseRequestList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewItem")
            {
                reqID = (int)grvPurchaseRequestList.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;
                Session["purchaseReqId"] = reqID;
                _presenter.CurrentPurchaseRequest = _presenter.GetPurchaseRequestById(reqID);
                //_presenter.OnViewLoaded();
                dgPurchaseRequestDetail.DataSource = _presenter.CurrentPurchaseRequest.PurchaseRequestDetails;
                dgPurchaseRequestDetail.DataBind();
                pnlDetail_ModalPopupExtender.Show();
            }
        }
        protected void grvPurchaseRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //grvPaymentReimbursementRequestList.SelectedDataKey.Value
            _presenter.OnViewLoaded();
            PopApprovalStatus();
            //grvAttachments.DataSource = _presenter.CurrentPaymentReimbursementRequest.CPRAttachments;
            //grvAttachments.DataBind();
            BindPurchaseRequestStatus();
            PrintTransaction();
            txtRejectedReason.Visible = false;
            rfvRejectedReason.Enabled = false;
            pnlApproval_ModalPopupExtender.Show();
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
            if (_presenter.CurrentPurchaseRequest.ProgressStatus == ProgressStatus.Completed.ToString())
            {
                ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Canceled.ToString().Replace('_', ' '), ApprovalStatus.Canceled.ToString().Replace('_', ' ')));
            }

        }
        protected void grvPurchaseRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Button btnStatus = e.Row.FindControl("btnStatus") as Button;
            PurchaseRequest CSR = e.Row.DataItem as PurchaseRequest;
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
            pnlApproval_ModalPopupExtender.Show();
        }
        protected void grvPurchaseRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvPurchaseRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
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

        #region PurchaseRequestDetail
        private void BindPurchaseRequestDetails()
        {
            _presenter.CurrentPurchaseRequest = _presenter.GetPurchaseRequestById((int)Session["purchaseReqId"]);
            dgPurchaseRequestDetail.DataSource = _presenter.CurrentPurchaseRequest.PurchaseRequestDetails;
            dgPurchaseRequestDetail.DataBind();

        }
        protected void dgPurchaseRequestDetail_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgPurchaseRequestDetail.EditItemIndex = -1;
            BindPurchaseRequestDetails();
        }
        protected void dgPurchaseRequestDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgPurchaseRequestDetail.EditItemIndex = e.Item.ItemIndex;
            BindPurchaseRequestDetails();
            pnlDetail_ModalPopupExtender.Show();
        }
        protected void dgPurchaseRequestDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
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
                DropDownList ddlUnitOfMeasurment = e.Item.FindControl("ddlUnitOfMeasurment") as DropDownList;
                if(ddlUnitOfMeasurment != null)
                {
                    ListItem liI = ddlUnitOfMeasurment.Items.FindByValue(_presenter.CurrentPurchaseRequest.PurchaseRequestDetails[e.Item.DataSetIndex].UnitOfMeasurment);
                    if (liI != null)
                        liI.Selected = true;
                }
                DropDownList ddlPurposeOfPurchase = e.Item.FindControl("ddlPurposeOfPurchase") as DropDownList;
                if (ddlPurposeOfPurchase != null)
                {
                    ListItem liI = ddlPurposeOfPurchase.Items.FindByValue(_presenter.CurrentPurchaseRequest.PurchaseRequestDetails[e.Item.DataSetIndex].PurposeOfPurchase);
                    if (liI != null)
                        liI.Selected = true;
                }
            }

        }
        protected void dgPurchaseRequestDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgPurchaseRequestDetail.DataKeys[e.Item.ItemIndex];
            _presenter.CurrentPurchaseRequest = _presenter.GetPurchaseRequestById((int)Session["purchaseReqId"]);
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
                Detail.Item = txtItem.Text;
                TextBox txtApprovedQuantity = e.Item.FindControl("txtApprovedQuantity") as TextBox;
                Detail.ApprovedQuantity = Convert.ToInt32(txtApprovedQuantity.Text);
                DropDownList ddlPurposeOfPurchase = e.Item.FindControl("ddlPurposeOfPurchase") as DropDownList;
                Detail.PurposeOfPurchase = ddlPurposeOfPurchase.SelectedValue;
                DropDownList ddlUnitOfMeasurment = e.Item.FindControl("ddlUnitOfMeasurment") as DropDownList;
                Detail.UnitOfMeasurment = ddlUnitOfMeasurment.SelectedValue;
                DropDownList ddlProject = e.Item.FindControl("ddlProject") as DropDownList;
                Detail.Project = _presenter.GetProject(int.Parse(ddlProject.SelectedValue));
                TextBox txtGrant = e.Item.FindControl("txtGrant") as TextBox;
                Detail.Grant = _presenter.GetGrantByCode(txtGrant.Text);
                Detail.PurchaseRequest = _presenter.CurrentPurchaseRequest;
                Master.ShowMessage(new AppMessage("Purchase Request Detail  Updated successfully.", RMessageType.Info));
                dgPurchaseRequestDetail.EditItemIndex = -1;
                BindPurchaseRequestDetails();
                pnlDetail_ModalPopupExtender.Show();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Purchase Request Detail. " + ex.Message, RMessageType.Error));
            }
        }
        #endregion
    }
}