using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Chai.WorkflowManagment.Modules.Shell.Views;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Modules.Shell.MasterPages;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;
using Ardalis.GuardClauses;
using Chai.WorkflowManagment.CoreDomain.Requests;

public partial class ShellDefault : Microsoft.Practices.CompositeWeb.Web.UI.Page, IBaseMasterView
{
    private BaseMasterPresenter _presenter;
    private AppUser currentUser;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this._presenter.OnViewInitialized();
            BindCashPaymentRequests();
            BindLeaveRequests();
            BindVehicleRequests();
            BindCostSharingRequests();
            BindTravelAdvanceRequests();
            BindExpenseLiquidationRequests();
            BindPurchaseRequests();
            BindBankPaymentRequests();
            BindBidAnalysisRequests();
            BindSoleVendorRequests();
            BindMaintenanceRequests();
            BindStoreRequests();
            BindPaymentReimburesmentRequests();
        }
        this._presenter.OnViewLoaded();
        MyTasks();
        MyRequests();
        if (_presenter.CurrentUser.EmployeePosition.PositionName == "Admin/HR Officer" || _presenter.CurrentUser.EmployeePosition.PositionName == "Operational Manager" || _presenter.CurrentUser.EmployeePosition.PositionName == "Country Director" || _presenter.CurrentUser.EmployeePosition.PositionName == "Finance Officer" || _presenter.CurrentUser.EmployeePosition.PositionName == "Finance Manager")
        {
            reimbersmentstatuses.Visible = true;
            ReimbersmentStatus();
        }
    }

    [CreateNew]
    public BaseMasterPresenter Presenter
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

    public string TabId
    {
        get { return Request.QueryString[AppConstants.TABID]; }
    }
    public string NodeId
    {
        get { return Request.QueryString[AppConstants.NODEID]; }
    }
    public Chai.WorkflowManagment.CoreDomain.Users.AppUser CurrentUser
    {
        get
        {
            return currentUser;
        }
        set
        {
            currentUser = value;
        }
    }
    private void MyTasks()
    {
        if (_presenter.GetLeaveTasks() != 0)
        {
            lblLeaverequests.Text = _presenter.GetLeaveTasks().ToString();
            if (_presenter.GetLeaveTasks() > 0)
                lblLeaverequests.ForeColor = System.Drawing.Color.Red;
            lnkLeaveRequest.Enabled = true;
            lnkLeaveRequest.PostBackUrl = ResolveUrl("Approval/frmLeaveApproval.aspx");
        }
        else
        {
            lblLeaverequests.Text = Convert.ToString(0);
        }
        if (_presenter.GetVehicleTasks() != 0)
        {
            lblVehicleRequest.Text = _presenter.GetVehicleTasks().ToString();
            if (_presenter.GetVehicleTasks() > 0)
                lblVehicleRequest.ForeColor = System.Drawing.Color.Red;
            lnkVehicleRequest.Enabled = true;
            lnkVehicleRequest.PostBackUrl = ResolveUrl("Approval/frmVehicleApproval.aspx");
        }
        else
        {
            lblVehicleRequest.Text = Convert.ToString(0);
        }
        if (_presenter.GetCashPaymentRequestTasks() != 0)
        {
            lblPaymentRequest.Text = _presenter.GetCashPaymentRequestTasks().ToString();
            if (_presenter.GetCashPaymentRequestTasks() > 0)
                lblPaymentRequest.ForeColor = System.Drawing.Color.Red;
            lnkPaymentRequest.Enabled = true;
            lnkPaymentRequest.PostBackUrl = lnkPaymentRequest.ResolveUrl("Approval/frmCashPaymentApproval.aspx");
        }
        else { lblPaymentRequest.Text = Convert.ToString(0); }
        if (_presenter.GetCostSharingRequestTasks() != 0)
        {
            lblCostSharingRequest.Text = _presenter.GetCostSharingRequestTasks().ToString();
            if (_presenter.GetCostSharingRequestTasks() > 0)
                lblCostSharingRequest.ForeColor = System.Drawing.Color.Red;
            lnkCostSharingRequest.Enabled = true;
            lnkCostSharingRequest.PostBackUrl = lnkCostSharingRequest.ResolveUrl("Approval/frmCostSharingApproval.aspx");
        }
        else { lblCostSharingRequest.Text = Convert.ToString(0); }
        if (_presenter.GetPurchaseRequestsTasks() != 0)
        {
            lblpurchaserequest.Text = _presenter.GetPurchaseRequestsTasks().ToString();
            if (_presenter.GetPurchaseRequestsTasks() > 0)
                lblpurchaserequest.ForeColor = System.Drawing.Color.Red;
            lnkPurchaseRequest.Enabled = true;
            lnkPurchaseRequest.PostBackUrl = ResolveUrl("Approval/frmPurchaseApprovalDetail.aspx");
        }
        else { lblpurchaserequest.Text = Convert.ToString(0); }

        if (_presenter.GetTravelAdvanceRequestTasks() != 0)
        {
            lblTravelAdvanceRequest.Text = _presenter.GetTravelAdvanceRequestTasks().ToString();
            if (_presenter.GetTravelAdvanceRequestTasks() > 0)
                lblTravelAdvanceRequest.ForeColor = System.Drawing.Color.Red;
            lnkTravelAdvanceRequest.Enabled = true;
            lnkTravelAdvanceRequest.PostBackUrl = ResolveUrl("Approval/frmTravelAdvanceApproval.aspx");
        }
        else { lblTravelAdvanceRequest.Text = Convert.ToString(0); }

        if (_presenter.GetReviewExpenseLiquidationRequestsTasks() != 0)
        {
            lblreviewliquidation.Text = _presenter.GetReviewExpenseLiquidationRequestsTasks().ToString();
            if (_presenter.GetReviewExpenseLiquidationRequestsTasks() > 0)
                lblreviewliquidation.ForeColor = System.Drawing.Color.Red;
            lnkreviewliquidation.Enabled = true;
            lnkreviewliquidation.PostBackUrl = ResolveUrl("Approval/frmExpenseLiquidationApproval.aspx");
        }
        else
        { lblreviewliquidation.Text = Convert.ToString(0); }
        if (_presenter.GetExpenseLiquidationRequestsTasks() != 0)
        {
            lblExpenseLiquidation.Text = _presenter.GetExpenseLiquidationRequestsTasks().ToString();
            if (_presenter.GetExpenseLiquidationRequestsTasks() > 0)
                lblExpenseLiquidation.ForeColor = System.Drawing.Color.Red;
            lnkExpenseLiquidation.Enabled = true;
            lnkExpenseLiquidation.PostBackUrl = ResolveUrl("Request/frmExpenseLiquidationRequest.aspx");
        }
        else
        {
            lblExpenseLiquidation.Text = Convert.ToString(0);
        }
        if (_presenter.GetReviewPaymentReimbursementTasks() != 0)
        {
            lblreviewsettlemnt.Text = _presenter.GetReviewPaymentReimbursementTasks().ToString();
            if (_presenter.GetReviewPaymentReimbursementTasks() > 0)
                lblreviewsettlemnt.ForeColor = System.Drawing.Color.Red;
            lnkReviewpaymentsettlemnt.Enabled = true;
            lnkReviewpaymentsettlemnt.PostBackUrl = ResolveUrl("Approval/frmPaymentReimbursementApproval.aspx");
        }
        else
        { lblreviewsettlemnt.Text = Convert.ToString(0); }
        if (_presenter.GetPaymentReimbursementTasks() != 0)
        {
            lblReimbursement.Text = _presenter.GetPaymentReimbursementTasks().ToString();
            if (_presenter.GetPaymentReimbursementTasks() > 0)
                lblReimbursement.ForeColor = System.Drawing.Color.Red;
            lnkPaymentReimbursement.Enabled = true;
            lnkPaymentReimbursement.PostBackUrl = ResolveUrl("Request/frmPaymentReimbursementRequest.aspx");
        }
        else
        {
            lblReimbursement.Text = Convert.ToString(0);
        }
        if (_presenter.GetBankPaymentRequestsTasks() != 0)
        {
            lblbankpayment.Text = _presenter.GetBankPaymentRequestsTasks().ToString();
            if (_presenter.GetBankPaymentRequestsTasks() > 0)
                lblbankpayment.ForeColor = System.Drawing.Color.Red;
            lnkbankpayment.Enabled = true;
            lnkbankpayment.PostBackUrl = ResolveUrl("Approval/frmOperationalControlApproval.aspx");
        }
        else
        {
            lblbankpayment.Text = Convert.ToString(0);
        }

        if (_presenter.GetBidAnalysisRequestsTasks() != 0)
        {
            lblBidAnalysis.Text = _presenter.GetBidAnalysisRequestsTasks().ToString();
            if (_presenter.GetBidAnalysisRequestsTasks() > 0)
                lblBidAnalysis.ForeColor = System.Drawing.Color.Red;
            lnkBidAnalysis.Enabled = true;
            lnkBidAnalysis.PostBackUrl = ResolveUrl("Approval/frmBidAnalysisApproval.aspx");
        }
        else
        {
            lblBidAnalysis.Text = Convert.ToString(0);
        }
        if (_presenter.GetSoleVendorRequestsTasks() != 0)
        {
            lblSolVendor.Text = _presenter.GetSoleVendorRequestsTasks().ToString();
            if (_presenter.GetSoleVendorRequestsTasks() > 0)
                lblSolVendor.ForeColor = System.Drawing.Color.Red;
            lnkSoleVendor.Enabled = true;
            lnkSoleVendor.PostBackUrl = ResolveUrl("Approval/frmSoleVendorApproval.aspx");
        }
        else
        {
            lblSolVendor.Text = Convert.ToString(0);
        }

        if (_presenter.GetMaintenanceRequestsTasks() != 0)
        {
            lblMaintenanc.Text = _presenter.GetMaintenanceRequestsTasks().ToString();
            if (_presenter.GetMaintenanceRequestsTasks() > 0)
                lblMaintenanc.ForeColor = System.Drawing.Color.Red;
            lnkMaintenance.Enabled = true;
            lnkMaintenance.PostBackUrl = ResolveUrl("Approval/frmMaintenanceApproval.aspx");
        }
        else
        {
            lblMaintenanc.Text = Convert.ToString(0);

        }
        if (_presenter.GetStoreRequestsTasks() != 0)
        {
            lblStore.Text = _presenter.GetStoreRequestsTasks().ToString();
            if (_presenter.GetStoreRequestsTasks() > 0)
                lblStore.ForeColor = System.Drawing.Color.Red;
            lnkStore.Enabled = true;
            lnkStore.PostBackUrl = ResolveUrl("Approval/frmStoreApproval.aspx");
        }
        else
        {
            lblStore.Text = Convert.ToString(0);
        }
    }
    private void MyRequests()
    {
        if (_presenter.GetLeaveMyRequest() != 0)
        {
            lblLeaveStatus.Text = ProgressStatus.InProgress.ToString();
            lblLeaveStatus.ForeColor = System.Drawing.Color.Green;
        }

        if (_presenter.GetVehicleMyRequest() != 0)
        {
            lblVehicleStatus.Text = ProgressStatus.InProgress.ToString();
            lblVehicleStatus.ForeColor = System.Drawing.Color.Green;

        }

        if (_presenter.GetCashPaymentRequestMyRequests() != 0)
        {
            lblPaymentStatus.Text = ProgressStatus.InProgress.ToString();
            lblPaymentStatus.ForeColor = System.Drawing.Color.Green;

        }
        if (_presenter.GetCostSharingRequestMyRequests() != 0)
        {
            lblCostSharingStatus.Text = ProgressStatus.InProgress.ToString();
            lblCostSharingStatus.ForeColor = System.Drawing.Color.Green;

        }
        if (_presenter.GetTravelAdvanceRequestMyRequest() != 0)
        {
            lblTravelStatus.Text = ProgressStatus.InProgress.ToString();
            lblTravelStatus.ForeColor = System.Drawing.Color.Green;
        }
        if (_presenter.GetExpenseLiquidationMyRequest() != 0)
        {
            lblExpenseStatus.Text = ProgressStatus.InProgress.ToString();
            lblExpenseStatus.ForeColor = System.Drawing.Color.Green;
        }
        if (_presenter.GetPaymentReimbursementRequestMyRequest() != 0)
        {
            lblPaymentReimburesment.Text = ProgressStatus.InProgress.ToString();
            lblPaymentReimburesment.ForeColor = System.Drawing.Color.Green;
        }
        if (_presenter.GetPurchaseRequestsMyRequest() != 0)
        {
            lblPurchaseStatus.Text = ProgressStatus.InProgress.ToString();
            lblPurchaseStatus.ForeColor = System.Drawing.Color.Green;
        }
        if (_presenter.GetBankRequestsMyRequest() != 0)
        {
            lblBankRequestStatus.Text = ProgressStatus.InProgress.ToString();
            lblBankRequestStatus.ForeColor = System.Drawing.Color.Green;

        }
        if (_presenter.GetBidAnalysisRequestsMyRequest() != 0)
        {
            lblBidAnalysisStatus.Text = ProgressStatus.InProgress.ToString();
            lblBidAnalysisStatus.ForeColor = System.Drawing.Color.Green;

        }
        if (_presenter.GetSoleVendorRequestsMyRequest() != 0)
        {
            lblSoleVendorStatus.Text = ProgressStatus.InProgress.ToString();
            lblSoleVendorStatus.ForeColor = System.Drawing.Color.Green;

        }
        if (_presenter.GetMaintenanceRequestsMyRequest() != 0)
        {
            lblMaintenanceStatus.Text = ProgressStatus.InProgress.ToString();
            lblMaintenanceStatus.ForeColor = System.Drawing.Color.Green;

        }
        if (_presenter.GetStoreRequestsMyRequest() != 0)
        {
            lblStoreStatus.Text = ProgressStatus.InProgress.ToString();
            lblStoreStatus.ForeColor = System.Drawing.Color.Green;

        }

    }
    private void ReimbersmentStatus()
    {
        if (_presenter.GetCashPaymentReimbersment() != 0)
        {
            lblCashPaymentreimbersment.Text = _presenter.GetCashPaymentReimbersment().ToString();

        }
        else
        {
            lblCashPaymentreimbersment.Text = Convert.ToString(0);
        }

        if (_presenter.GetCostSharingPaymentReimbersment() != 0)
        {
            lblCostPaymentreimbersment.Text = _presenter.GetCostSharingPaymentReimbersment().ToString();

        }
        else
        {
            lblCostPaymentreimbersment.Text = Convert.ToString(0);
        }
    }
    private void BindLeaveRequests()
    {
        grvLeaveProgress.DataSource = _presenter.ListLeaveApprovalProgress();
        grvLeaveProgress.DataBind();
    }
    private void BindVehicleRequests()
    {
        grvVehicleProgress.DataSource = _presenter.ListVehicleApprovalProgress();
        grvVehicleProgress.DataBind();
    }
    private void BindCashPaymentRequests()
    {
        grvPaymentProgress.DataSource = _presenter.ListPaymentApprovalProgress();
        grvPaymentProgress.DataBind();
    }
    private void BindCostSharingRequests()
    {
        grvCostProgress.DataSource = _presenter.ListCostApprovalProgress();
        grvCostProgress.DataBind();
    }
    private void BindTravelAdvanceRequests()
    {
        grvTravelProgress.DataSource = _presenter.ListTravelApprovalProgress();
        grvTravelProgress.DataBind();
    }
    private void BindExpenseLiquidationRequests()
    {
        grvExpenseProgress.DataSource = _presenter.ListExpenseLiquidationProgress();
        grvExpenseProgress.DataBind();
    }
    private void BindPurchaseRequests()
    {
        grvPurchaseProgress.DataSource = _presenter.ListPurchaseApprovalProgress();
        grvPurchaseProgress.DataBind();
    }
    private void BindBankPaymentRequests()
    {
        grvBankProgress.DataSource = _presenter.ListBankPaymentApprovalProgress();
        grvBankProgress.DataBind();
    }
    private void BindPaymentReimburesmentRequests()
    {
        grvSettlement.DataSource = _presenter.ListPaymentReimbursementApprovalProgress();
        grvSettlement.DataBind();
    }
    private void BindBidAnalysisRequests()
    {
        grvBidAnalysisProgress.DataSource = _presenter.ListBidAnalysisApprovalProgress();
        grvBidAnalysisProgress.DataBind();
    }

    private void BindSoleVendorRequests()
    {
        grvSoleVendorProgress.DataSource = _presenter.ListSoleVendorApprovalProgress();
        grvSoleVendorProgress.DataBind();
    }
    private void BindMaintenanceRequests()
    {
        grvMaintenanceProgress.DataSource = _presenter.ListMaintenanceApprovalProgress();
        grvMaintenanceProgress.DataBind();
    }
    private void BindStoreRequests()
    {
        grvStoreProgress.DataSource = _presenter.ListStoreApprovalProgress();
        grvStoreProgress.DataBind();
    }
    protected void grvLeaveProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListLeaveApprovalProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.ListLeaveApprovalProgress()[e.Row.RowIndex].CurrentApprover != 0)
                    e.Row.Cells[2].Text = _presenter.GetUser(_presenter.ListLeaveApprovalProgress()[e.Row.RowIndex].CurrentApprover).FullName;
            }
        }
    }
    protected void grvVehicleProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListVehicleApprovalProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.ListVehicleApprovalProgress()[e.Row.RowIndex].CurrentApprover != 0)
                    e.Row.Cells[2].Text = _presenter.GetUser(_presenter.ListVehicleApprovalProgress()[e.Row.RowIndex].CurrentApprover).FullName;
            }
        }
    }
    protected void grvPaymentProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListPaymentApprovalProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.ListPaymentApprovalProgress().Count != 0)
                {
                    if (_presenter.ListPaymentApprovalProgress()[e.Row.RowIndex].CurrentApprover != 0)
                        e.Row.Cells[2].Text = _presenter.GetUser(_presenter.ListPaymentApprovalProgress()[e.Row.RowIndex].CurrentApprover).FullName;
                    else
                        e.Row.Cells[2].Text = "Accountant";
                }
            }
        }
    }
    protected void grvCostProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListCostApprovalProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.ListCostApprovalProgress()[e.Row.RowIndex].CurrentApprover != 0)
                    e.Row.Cells[2].Text = _presenter.GetUser(_presenter.ListCostApprovalProgress()[e.Row.RowIndex].CurrentApprover).FullName;
            }
        }
    }
    protected void grvTravelProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListTravelApprovalProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.ListTravelApprovalProgress().Count != 0)
                {
                    if (_presenter.ListTravelApprovalProgress()[e.Row.RowIndex].CurrentApprover != 0)
                        e.Row.Cells[2].Text = _presenter.GetUser(_presenter.ListTravelApprovalProgress()[e.Row.RowIndex].CurrentApprover).FullName;
                    else
                        e.Row.Cells[2].Text = "Accountant";
                }
            }
        }
    }
    protected void grvExpenseProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListExpenseLiquidationProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.ListExpenseLiquidationProgress().Count != 0)
                {
                    if (_presenter.ListExpenseLiquidationProgress()[e.Row.RowIndex].CurrentApprover != 0)
                        e.Row.Cells[2].Text = _presenter.GetUser(_presenter.ListExpenseLiquidationProgress()[e.Row.RowIndex].CurrentApprover).FullName;
                    else
                        e.Row.Cells[2].Text = "Accountant";
                }
            }
        }
    }
    protected void grvPurchaseProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListPurchaseApprovalProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.ListPurchaseApprovalProgress()[e.Row.RowIndex].CurrentApprover != 0)
                    e.Row.Cells[2].Text = _presenter.GetUser(_presenter.ListPurchaseApprovalProgress()[e.Row.RowIndex].CurrentApprover).FullName;
            }
        }
    }
    protected void grvBankProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListBankPaymentApprovalProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                OperationalControlRequest theBankPayment = _presenter.ListBankPaymentApprovalProgress()[e.Row.RowIndex];
                if (theBankPayment.CurrentApprover != 0)
                    e.Row.Cells[3].Text = _presenter.GetUser(theBankPayment.CurrentApprover).FullName;
                if (theBankPayment.PaymentId > 0)
                {
                    CashPaymentRequest thePayment = _presenter.GetCashPaymentRequest(theBankPayment.PaymentId);
                    e.Row.Cells[2].Text = Guard.Against.Null(Guard.Against.Null(thePayment, " CashPaymentRequest ID: " + theBankPayment.PaymentId.ToString()).AppUser, " AppUser").FullName;
                }
                else if (theBankPayment.TravelAdvanceId > 0)
                {
                    TravelAdvanceRequest theTravel = _presenter.GetTravelAdvanceRequest(theBankPayment.TravelAdvanceId);
                    e.Row.Cells[2].Text = Guard.Against.Null(Guard.Against.Null(theTravel, " TravelAdvanceRequest ID: " + theBankPayment.TravelAdvanceId.ToString()).AppUser, " AppUser").FullName;
                }
                else if (theBankPayment.LiquidationId > 0)
                {
                    ExpenseLiquidationRequest theLiquidation = _presenter.GetExpenseLiquidationRequest(theBankPayment.LiquidationId);
                    e.Row.Cells[2].Text = Guard.Against.Null(Guard.Against.Null(theLiquidation, " ExpenseLiquidationRequest ID: " + theBankPayment.LiquidationId.ToString()).TravelAdvanceRequest.AppUser, " AppUser").FullName;
                }
                else if (theBankPayment.SettlementId > 0)
                {
                    PaymentReimbursementRequest theReimbursement = _presenter.GetPaymentReimbursementRequest(theBankPayment.SettlementId);
                    e.Row.Cells[2].Text = Guard.Against.Null(Guard.Against.Null(theReimbursement, " PaymentReimbursement ID: " + theBankPayment.SettlementId.ToString()).CashPaymentRequest.AppUser, " AppUser").FullName;
                }
            }
        }
    }
    protected void grvBidAnalysisProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListBidAnalysisApprovalProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.ListBidAnalysisApprovalProgress()[e.Row.RowIndex].CurrentApprover != 0)
                    e.Row.Cells[2].Text = _presenter.GetUser(_presenter.ListBidAnalysisApprovalProgress()[e.Row.RowIndex].CurrentApprover).FullName;
            }
        }
    }
    protected void grvSoleVendorProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListSoleVendorApprovalProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.ListSoleVendorApprovalProgress()[e.Row.RowIndex].CurrentApprover != 0)
                    e.Row.Cells[2].Text = _presenter.GetUser(_presenter.ListSoleVendorApprovalProgress()[e.Row.RowIndex].CurrentApprover).FullName;
            }
        }
    }
    protected void grvSettlementProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListPaymentReimbursementApprovalProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.ListPaymentReimbursementApprovalProgress().Count != 0)
                {
                    if (_presenter.ListPaymentReimbursementApprovalProgress()[e.Row.RowIndex].CurrentApprover != 0)
                        e.Row.Cells[2].Text = _presenter.GetUser(_presenter.ListPaymentReimbursementApprovalProgress()[e.Row.RowIndex].CurrentApprover).FullName;
                    else
                        e.Row.Cells[2].Text = "Accountant";
                }
            }
        }
    }
    protected void grvMaintenanceProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListMaintenanceApprovalProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.ListMaintenanceApprovalProgress()[e.Row.RowIndex].CurrentApprover != 0)
                    e.Row.Cells[2].Text = _presenter.GetUser(_presenter.ListMaintenanceApprovalProgress()[e.Row.RowIndex].CurrentApprover).FullName;
            }
        }
    }
    protected void grvStoreProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListStoreApprovalProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.ListStoreApprovalProgress()[e.Row.RowIndex].CurrentApprover != 0)
                    e.Row.Cells[2].Text = _presenter.GetUser(_presenter.ListStoreApprovalProgress()[e.Row.RowIndex].CurrentApprover).FullName;
            }
        }
    }
}
