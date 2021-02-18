using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using log4net;
using log4net.Config;
using Microsoft.Practices.ObjectBuilder;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public partial class frmOperationalControlRequest : POCBasePage, IOperationalControlRequestView
    {
        private OperationalControlRequestPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                CheckApprovalSettings();
                BindOperationalControlRequests();
                PopBeneficiaries();
                PopBankAccounts();

            }
            txtRequestDate.Text = DateTime.Today.Date.ToShortDateString();
            this._presenter.OnViewLoaded();

            if (!this.IsPostBack)
            {
                PopulateBankPaymentDetail();
                BindOperationalControlDetails();
                Bindattachments();
            }
        }

        [CreateNew]
        public OperationalControlRequestPresenter Presenter
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
                return "{8C9E89D2-3048-4242-A0A9-88D41FF8C700}";
            }
        }
        #region Field Getters
        public int GetOperationalControlRequestId
        {
            get
            {
                if (grvOperationalControlRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvOperationalControlRequestList.SelectedDataKey.Value);
                }
                else
                {
                    return 0;
                }
            }
        }
        public string GetRequestNo
        {
            get { return AutoNumber(); }
        }
        public int GetBankAccountId
        {
            get { return Convert.ToInt32(ddlBankAccount.SelectedValue); }
        }
        public int GetBeneficiaryId
        {
            get { return int.Parse(ddlBeneficiary.SelectedValue); }
        }
        public string GetDescription
        {
            get { return txtDescription.Text; }
        }
        public string GetPayee
        {
            get { return txtPayee.Text; }
        }
        public string GetTelephoneNo
        {
            get { return txtTelephoneNo.Text; }
        }
        public string GetBankName
        {
            get { return txtBankName.Text; }
        }
        public string GetVoucherNo
        {
            get { return AutoNumber(); }
        }
        public string GetPaymentType
        {
            get
            {
                if (rbAccount.Checked)
                    return "Account";
                else if (rbCheque.Checked)
                    return "Cheque";
                else
                    return "Telegrpahic";
            }
        }
        #endregion
        private void PopulateBankPaymentDetail()
        {
            if (Request.QueryString["Page"] != null)
            {
                if (Request.QueryString["Page"].Contains("CashPayment"))
                {
                    if (Request.QueryString["PaymentId"] != null)
                    {
                        int paymentId = Convert.ToInt32(Request.QueryString["PaymentId"]);
                        CashPaymentRequest CPR = _presenter.GetCashPaymentRequest(paymentId);
                        if (CPR != null)
                        {
                            _presenter.CurrentOperationalControlRequest.Description = CPR.Description;
                            _presenter.CurrentOperationalControlRequest.PaymentId = paymentId;
                            txtOriginalRequester.Text = CPR.AppUser.FullName;

                            foreach (CashPaymentRequestDetail CPRD in CPR.CashPaymentRequestDetails)
                            {
                                OperationalControlRequestDetail OCRD = new OperationalControlRequestDetail();
                                OCRD.ItemAccount = CPRD.ItemAccount;
                                OCRD.Project = CPRD.Project;
                                OCRD.Grant = CPRD.Grant;
                                OCRD.Amount = CPRD.Amount;
                                OCRD.ActualExpendture = CPRD.Amount;
                                OCRD.AccountCode = CPRD.AccountCode;
                                _presenter.CurrentOperationalControlRequest.TotalAmount += OCRD.Amount;
                                _presenter.CurrentOperationalControlRequest.TotalActualExpendture += OCRD.Amount;
                                OCRD.OperationalControlRequest = _presenter.CurrentOperationalControlRequest;
                                _presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails.Add(OCRD);

                                if (CPRD.CPRAttachments.Count > 0)
                                {
                                    foreach (CPRAttachment CP in CPRD.CPRAttachments)
                                    {
                                        OCRAttachment OPA = new OCRAttachment();

                                        OPA.FilePath = CP.FilePath;
                                        OPA.OperationalControlRequest = _presenter.CurrentOperationalControlRequest;
                                        _presenter.CurrentOperationalControlRequest.OCRAttachments.Add(OPA);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (Request.QueryString["Page"].Contains("TravelAdvance"))
                {
                    if (Request.QueryString["PaymentId"] != null)
                    {
                        int travelAdvId = Convert.ToInt32(Request.QueryString["PaymentId"]);
                        TravelAdvanceRequest TAR = _presenter.GetTravelAdvanceRequest(travelAdvId);
                        if (TAR != null)
                        {
                            _presenter.CurrentOperationalControlRequest.Description = TAR.PurposeOfTravel;
                            _presenter.CurrentOperationalControlRequest.TravelAdvanceId = travelAdvId;
                            txtOriginalRequester.Text = TAR.AppUser.FullName;

                            foreach (TravelAdvanceRequestDetail TARD in TAR.TravelAdvanceRequestDetails)
                            {
                                foreach (TravelAdvanceCost TAC in TARD.TravelAdvanceCosts)
                                {
                                    OperationalControlRequestDetail OCRD = new OperationalControlRequestDetail();
                                    OCRD.ItemAccount = TAC.ItemAccount;
                                    OCRD.AccountCode = TAC.AccountCode;
                                    OCRD.Project = TAR.Project;
                                    OCRD.Grant = TAR.Grant;
                                    OCRD.Amount = TAC.Total;
                                    OCRD.ActualExpendture = TAC.Total;
                                    _presenter.CurrentOperationalControlRequest.TotalAmount += OCRD.Amount;
                                    _presenter.CurrentOperationalControlRequest.TotalActualExpendture += OCRD.Amount;
                                    OCRD.OperationalControlRequest = _presenter.CurrentOperationalControlRequest;
                                    _presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails.Add(OCRD);
                                }

                            }
                        }
                    }
                }
                else if (Request.QueryString["Page"].Contains("ExpenseLiquidation"))
                {
                    if (Request.QueryString["PaymentId"] != null)
                    {
                        int liquidationId = Convert.ToInt32(Request.QueryString["PaymentId"]);
                        ExpenseLiquidationRequest ELR = _presenter.GetExpenseLiquidation(liquidationId);
                        if (ELR != null)
                        {
                            _presenter.CurrentOperationalControlRequest.LiquidationId = liquidationId;
                            txtOriginalRequester.Text = ELR.TravelAdvanceRequest.AppUser.FullName;

                            OperationalControlRequestDetail OCRD = new OperationalControlRequestDetail();
                            OCRD.Amount = ELR.TotalActualExpenditure - ELR.TotalTravelAdvance;
                            OCRD.ItemAccount = _presenter.GetDefaultItemAccount();
                            OCRD.Project = ELR.ExpenseLiquidationRequestDetails[0].Project;
                            OCRD.Grant = ELR.ExpenseLiquidationRequestDetails[0].Grant;

                            _presenter.CurrentOperationalControlRequest.TotalAmount = OCRD.Amount;
                            _presenter.CurrentOperationalControlRequest.TotalActualExpendture = OCRD.Amount;
                            OCRD.OperationalControlRequest = _presenter.CurrentOperationalControlRequest;
                            _presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails.Add(OCRD);

                        }
                    }
                }
                else if (Request.QueryString["Page"].Contains("Settlement"))
                {
                    if (Request.QueryString["SettlementId"] != null)
                    {

                        int SettlementId = Convert.ToInt32(Request.QueryString["SettlementId"]);
                        PaymentReimbursementRequest PRR = _presenter.GetReimbursementRequest(SettlementId);
                        if (PRR != null)
                        {
                            _presenter.CurrentOperationalControlRequest.Description = PRR.Comment;
                            _presenter.CurrentOperationalControlRequest.SettlementId = SettlementId;
                            foreach (PaymentReimbursementRequestDetail detail in PRR.PaymentReimbursementRequestDetails)
                            {
                                OperationalControlRequestDetail OCRD = new OperationalControlRequestDetail();
                                OCRD.ItemAccount = PRR.CashPaymentRequest.CashPaymentRequestDetails[0].ItemAccount;//  detail.ItemAccount;
                                OCRD.Amount = PRR.ReceivableAmount - PRR.TotalAmount;
                                OCRD.Project = PRR.Project;
                                OCRD.Grant = PRR.Grant;
                                _presenter.CurrentOperationalControlRequest.TotalAmount = PRR.ReceivableAmount;
                                _presenter.CurrentOperationalControlRequest.TotalActualExpendture = PRR.TotalAmount;
                                OCRD.OperationalControlRequest = _presenter.CurrentOperationalControlRequest;
                                _presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails.Add(OCRD);
                                if (detail.PRAttachments.Count > 0)
                                {
                                    foreach (PRAttachment CP in detail.PRAttachments)
                                    {
                                        OCRAttachment OPA = new OCRAttachment();

                                        OPA.FilePath = CP.FilePath;
                                        OPA.OperationalControlRequest = _presenter.CurrentOperationalControlRequest;
                                        _presenter.CurrentOperationalControlRequest.OCRAttachments.Add(OPA);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private string AutoNumber()
        {
            return "BP-" + _presenter.CurrentUser().Id.ToString() + "-" + (_presenter.GetLastOperationalControlRequestId() + 1).ToString();
        }
        private void CheckApprovalSettings()
        {
            if (_presenter.GetApprovalSetting(RequestType.OperationalControl_Request.ToString().Replace('_', ' '), 0) == null)
            {
                pnlWarning.Visible = true;
            }
        }
        private void ClearFormFields()
        {
            // txtVoucherNo.Text = String.Empty;
            //txtPayee.Text = String.Empty;
            // txtVoucherNo.Text = String.Empty;
        }
        private void PopBankAccounts()
        {
            ddlBankAccount.DataSource = _presenter.GetBankAccounts();
            ddlBankAccount.DataBind();
        }
        private void PopBeneficiaries()
        {
            ddlBeneficiary.Items.Clear();
            ListItem lst = new ListItem();
            lst.Text = " Select Beneficiary ";
            lst.Value = "0";
            ddlBeneficiary.Items.Add(lst);
            ddlBeneficiary.DataSource = _presenter.GetBeneficiaries();
            ddlBeneficiary.DataBind();
        }
        private void BindOperationalControlDetails()
        {
            txtDescription.Text = _presenter.CurrentOperationalControlRequest.Description;
            dgOperationalControlDetail.DataSource = _presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails;
            dgOperationalControlDetail.DataBind();
        }
        private void Bindattachments()
        {
            grvAttachments.DataSource = _presenter.CurrentOperationalControlRequest.OCRAttachments;
            grvAttachments.DataBind();
        }
        private void BindOperationalControlRequests()
        {
            grvOperationalControlRequestList.DataSource = _presenter.ListOperationalControlRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text);
            grvOperationalControlRequestList.DataBind();
        }
        private void BindOperationalControlRequestFields()
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentOperationalControlRequest != null)
            {
                if (_presenter.CurrentOperationalControlRequest.Beneficiary != null)
                    ddlBeneficiary.SelectedValue = _presenter.CurrentOperationalControlRequest.Beneficiary.Id.ToString();
                txtPayee.Text = _presenter.CurrentOperationalControlRequest.Payee;
                txtTelephoneNo.Text = _presenter.CurrentOperationalControlRequest.TelephoneNo;
                txtBankName.Text = _presenter.CurrentOperationalControlRequest.BankName;
                ddlBankAccount.SelectedValue = _presenter.CurrentOperationalControlRequest.Account.Id.ToString();
                txtBankAccountNo.Text = _presenter.GetBankAccount(_presenter.CurrentOperationalControlRequest.Account.Id).AccountNo;
                BindOperationalControlDetails();
                BindOperationalControlRequests();

            }
        }
        private void BindProject(DropDownList ddlProject)
        {
            ddlProject.DataSource = _presenter.ListProjects();
            ddlProject.DataValueField = "Id";
            ddlProject.DataTextField = "ProjectCode";
            ddlProject.DataBind();
        }
        private void BindGrant(DropDownList ddlGrant, int ProjectId)
        {
            ddlGrant.DataSource = _presenter.GetGrantbyprojectId(ProjectId);
            ddlGrant.DataValueField = "Id";
            ddlGrant.DataTextField = "GrantCode";
            ddlGrant.DataBind();
        }
        private void BindAccountDescription(DropDownList ddlAccountDescription)
        {
            ddlAccountDescription.DataSource = _presenter.ListItemAccounts();
            ddlAccountDescription.DataValueField = "Id";
            ddlAccountDescription.DataTextField = "AccountName";
            ddlAccountDescription.DataBind();
        }
        protected void dgOperationalControlDetail_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgOperationalControlDetail.EditItemIndex = -1;
        }
        protected void dgOperationalControlDetail_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgOperationalControlDetail.DataKeys[e.Item.ItemIndex];
            int CPRDId = (int)dgOperationalControlDetail.DataKeys[e.Item.ItemIndex];
            OperationalControlRequestDetail cprd;

            if (CPRDId > 0)
                cprd = _presenter.CurrentOperationalControlRequest.GetOperationalControlRequestDetail(CPRDId);
            else
                cprd = (OperationalControlRequestDetail)_presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails[e.Item.ItemIndex];
            try
            {
                if (CPRDId > 0)
                {
                    _presenter.CurrentOperationalControlRequest.RemoveOperationalControlRequestDetail(id);
                    if (_presenter.GetOperationalControlRequestDetail(id) != null)
                        _presenter.DeleteOperationalControlRequestDetail(_presenter.GetOperationalControlRequestDetail(id));
                    _presenter.SaveOrUpdateOperationalControlRequest(_presenter.CurrentOperationalControlRequest);
                }
                else { _presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails.Remove(cprd); }
                BindOperationalControlDetails();

                Master.ShowMessage(new AppMessage("Bank Payment Request Detail was Removed Successfully", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Bank Payment Request Detail. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgOperationalControlDetail_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            //CarRental CarRental = new CarRental();
            if (e.CommandName == "AddNew")
            {
                try
                {
                    OperationalControlRequestDetail cprd = new OperationalControlRequestDetail();
                    cprd.OperationalControlRequest = _presenter.CurrentOperationalControlRequest;
                    TextBox txtAmount = e.Item.FindControl("txtAmount") as TextBox;
                    cprd.Amount = Convert.ToDecimal(txtAmount.Text);
                    TextBox txtAccountCode = e.Item.FindControl("txtAccountCode") as TextBox;
                    cprd.AccountCode = txtAccountCode.Text;
                    DropDownList ddlAccountDescription = e.Item.FindControl("ddlAccountDescription") as DropDownList;
                    cprd.ItemAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlAccountDescription.SelectedValue));
                    DropDownList ddlProject = e.Item.FindControl("ddlProject") as DropDownList;
                    cprd.Project = _presenter.GetProject(Convert.ToInt32(ddlProject.SelectedValue));
                    DropDownList ddlGrant = e.Item.FindControl("ddlGrant") as DropDownList;
                    cprd.Grant = _presenter.GetGrant(int.Parse(ddlGrant.SelectedValue));
                    _presenter.CurrentOperationalControlRequest.TotalAmount += cprd.Amount;

                    _presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails.Add(cprd);

                    dgOperationalControlDetail.EditItemIndex = -1;
                    BindOperationalControlDetails();
                    Master.ShowMessage(new AppMessage("Bank Payment Detail Successfully Added!", RMessageType.Info));
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Save Bank Payment " + ex.Message, RMessageType.Error));
                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
                }
            }
        }
        protected void dgOperationalControlDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgOperationalControlDetail.EditItemIndex = e.Item.ItemIndex;
            BindOperationalControlDetails();
            //BindCarRentals();
        }
        protected void dgOperationalControlDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int CPRDId = (int)dgOperationalControlDetail.DataKeys[e.Item.ItemIndex];
            OperationalControlRequestDetail cprd;

            if (CPRDId > 0)
                cprd = _presenter.CurrentOperationalControlRequest.GetOperationalControlRequestDetail(CPRDId);
            else
                cprd = (OperationalControlRequestDetail)_presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails[e.Item.ItemIndex];

            try
            {
                cprd.OperationalControlRequest = _presenter.CurrentOperationalControlRequest;
                TextBox txtAmount = e.Item.FindControl("txtEdtAmount") as TextBox;
                cprd.Amount = Convert.ToDecimal(txtAmount.Text);
                TextBox txtEdtAccountCode = e.Item.FindControl("txtEdtAccountCode") as TextBox;
                cprd.AccountCode = txtEdtAccountCode.Text;
                DropDownList ddlAccountDescription = e.Item.FindControl("ddlEdtAccountDescription") as DropDownList;
                cprd.ItemAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlAccountDescription.SelectedValue));
                DropDownList ddlProject = e.Item.FindControl("ddlEdtProject") as DropDownList;
                cprd.Project = _presenter.GetProject(Convert.ToInt32(ddlProject.SelectedValue));
                DropDownList ddlGrant = e.Item.FindControl("ddlEdtGrant") as DropDownList;
                cprd.Grant = _presenter.GetGrant(int.Parse(ddlGrant.SelectedValue));
                //  _presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails.Add(cprd);

                dgOperationalControlDetail.EditItemIndex = -1;
                BindOperationalControlDetails();
                Master.ShowMessage(new AppMessage("Bank Payment Detail Successfully Updated", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Bank Payment. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgOperationalControlDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlProject = e.Item.FindControl("ddlProject") as DropDownList;
                BindProject(ddlProject);
                DropDownList ddlGrant = e.Item.FindControl("ddlGrant") as DropDownList;
                BindGrant(ddlGrant, Convert.ToInt32(ddlProject.SelectedValue));
                DropDownList ddlAccountDescription = e.Item.FindControl("ddlAccountDescription") as DropDownList;
                BindAccountDescription(ddlAccountDescription);
            }
            else
            {
                if (_presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails != null)
                {
                    DropDownList ddlProject = e.Item.FindControl("ddlEdtProject") as DropDownList;
                    if (ddlProject != null)
                    {
                        BindProject(ddlProject);
                        if (_presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails[e.Item.DataSetIndex].Project != null)
                        {
                            ListItem liI = ddlProject.Items.FindByValue(_presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails[e.Item.DataSetIndex].Project.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }
                    }
                    DropDownList ddlAccountDescription = e.Item.FindControl("ddlEdtAccountDescription") as DropDownList;
                    if (ddlAccountDescription != null)
                    {
                        BindAccountDescription(ddlAccountDescription);
                        if (_presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails[e.Item.DataSetIndex].ItemAccount != null)
                        {
                            ListItem liI = ddlAccountDescription.Items.FindByValue(_presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails[e.Item.DataSetIndex].ItemAccount.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }
                    }
                    DropDownList ddlEdtGrant = e.Item.FindControl("ddlEdtGrant") as DropDownList;
                    if (ddlEdtGrant != null)
                    {
                        BindGrant(ddlEdtGrant, Convert.ToInt32(ddlProject.SelectedValue));
                        if (_presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails[e.Item.DataSetIndex].Grant != null)
                        {
                            ListItem liI = ddlEdtGrant.Items.FindByValue(_presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails[e.Item.DataSetIndex].Grant.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }

                    }
                }
            }
        }
        protected void grvOperationalControlRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["OperationalControlRequest"] = true;
            //ClearForm();
            BindOperationalControlRequestFields();
            grvAttachments.DataSource = _presenter.CurrentOperationalControlRequest.OCRAttachments;
            grvAttachments.DataBind();
            btnDelete.Visible = true;
            if (_presenter.CurrentOperationalControlRequest.CurrentStatus != null)
            {
                btnSave.Visible = false;
                btnDelete.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
                btnDelete.Visible = true;
            }
        }
        protected void grvOperationalControlRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvOperationalControlRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails.Count != 0)
                {
                    _presenter.SaveOrUpdateOperationalControlRequest();
                    BindOperationalControlRequests();
                    Master.ShowMessage(new AppMessage("Successfully requested a Bank Payment with a Reference No - <b>'" + _presenter.CurrentOperationalControlRequest.VoucherNo + "'</b>", RMessageType.Info));
                    Log.Info(_presenter.CurrentUser().FullName + " has requested a Bank Payment for a total amount of " + _presenter.CurrentOperationalControlRequest.TotalAmount.ToString());
                    btnSave.Visible = false;
                }
                else
                {
                    Master.ShowMessage(new AppMessage("Please insert payable amount", RMessageType.Error));
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Violation of UNIQUE KEY"))
                    {
                        Master.ShowMessage(new AppMessage("Please Click Request button Again, There is a duplicate Number", RMessageType.Error));
                        //AutoNumber();
                    }
                }
            }

        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            _presenter.DeleteOperationalControlRequest(_presenter.CurrentOperationalControlRequest);
            ClearFormFields();
            BindOperationalControlRequests();
            BindOperationalControlDetails();
            btnDelete.Visible = false;
            Master.ShowMessage(new AppMessage("Bank Payment Request Successfully Deleted!", RMessageType.Info));
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindOperationalControlRequests();
            ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
            //pnlSearch_ModalPopupExtender.Show();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmOperationalControlRequest.aspx");
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            pnlWarning.Visible = false;
            _presenter.CancelPage();
        }
        protected void ddlAccountDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            TextBox txtAccountCode = ddl.FindControl("txtAccountCode") as TextBox;
            txtAccountCode.Text = _presenter.GetItemAccount(Convert.ToInt32(ddl.SelectedValue)).AccountCode;
        }
        protected void ddlEdtAccountDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            TextBox txtAccountCode = ddl.FindControl("txtEdtAccountCode") as TextBox;
            txtAccountCode.Text = _presenter.GetItemAccount(Convert.ToInt32(ddl.SelectedValue)).AccountCode;
        }
        protected void ddlEdtProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlEdtGrant = ddl.FindControl("ddlEdtGrant") as DropDownList;
            BindGrant(ddlEdtGrant, Convert.ToInt32(ddl.SelectedValue));
        }
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlFGrant = ddl.FindControl("ddlGrant") as DropDownList;
            BindGrant(ddlFGrant, Convert.ToInt32(ddl.SelectedValue));
        }
        protected void ddlBankAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlAccount = (DropDownList)sender;
            if (Convert.ToInt32(ddlAccount.SelectedValue) != 0)
                txtBankAccountNo.Text = _presenter.GetBankAccount(Convert.ToInt32(ddlAccount.SelectedValue)).AccountNo;
        }
        protected void ddlBeneficiary_SelectedIndexChanged(object sender, EventArgs e)
        {
            Beneficiary beneficiary = _presenter.GetBeneficiary(Convert.ToInt32(ddlBeneficiary.SelectedValue));
            if (beneficiary != null)
            {
                txtBankName.Text = beneficiary.BankName;
                txtBenAccountNo.Text = beneficiary.AccountNumber;
            }
            else
            {
                txtBankName.Text = "";
                txtBenAccountNo.Text = "";
            }
        }
        protected void rbCheque_CheckedChanged(object sender, EventArgs e)
        {
            pnlBeneficiaries.Visible = false;
            pnlChequeTelegraphic.Visible = true;
            txtBankName.Text = "";
            txtBenAccountNo.Text = "";
            ddlBeneficiary.SelectedValue = "0";
            rfvddlBeneficiary.Enabled = false;
            rfvtxtPayee.Enabled = true;
            rfvtxtTelephoneNo.Enabled = true;
        }
        protected void rbAccount_CheckedChanged(object sender, EventArgs e)
        {
            pnlBeneficiaries.Visible = true;
            pnlChequeTelegraphic.Visible = false;
            rfvtxtPayee.Enabled = false;
            rfvtxtTelephoneNo.Enabled = false;
            rfvddlBeneficiary.Enabled = true;
        }
        protected void rbTelegraphic_CheckedChanged(object sender, EventArgs e)
        {
            pnlBeneficiaries.Visible = false;
            pnlChequeTelegraphic.Visible = true;
            txtBankName.Text = "";
            txtBenAccountNo.Text = "";
            ddlBeneficiary.SelectedValue = "0";
            rfvddlBeneficiary.Enabled = false;
            rfvtxtPayee.Enabled = true;
            rfvtxtTelephoneNo.Enabled = true;
        }
        #region Attachments
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            UploadFile();
        }
        protected void DownloadFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            Response.End();
        }
        protected void DeleteFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            _presenter.CurrentOperationalControlRequest.RemoveOCAttachment(filePath);
            File.Delete(Server.MapPath(filePath));
            grvAttachments.DataSource = _presenter.CurrentOperationalControlRequest.OCRAttachments;
            grvAttachments.DataBind();
            //Response.Redirect(Request.Url.AbsoluteUri);


        }
        private void UploadFile()
        {
            string fileName = Path.GetFileName(fuReciept.PostedFile.FileName);

            if (fileName != String.Empty)
            {
                OCRAttachment attachment = new OCRAttachment();
                attachment.FilePath = "~/OCUploads/" + fileName;
                fuReciept.PostedFile.SaveAs(Server.MapPath("~/OCUploads/") + fileName);
                //Response.Redirect(Request.Url.AbsoluteUri);
                _presenter.CurrentOperationalControlRequest.OCRAttachments.Add(attachment);

                grvAttachments.DataSource = _presenter.CurrentOperationalControlRequest.OCRAttachments;
                grvAttachments.DataBind();
                Master.ShowMessage(new AppMessage("Successfully uploaded the attachment", RMessageType.Info));

            }
            else
            {
                Master.ShowMessage(new AppMessage("Please select file ", RMessageType.Error));
            }
        }
        #endregion
        #region Beneficiaries
        void BindBeneficiaries()
        {
            dgBeneficiary.DataSource = _presenter.ListBeneficiaries("");
            dgBeneficiary.DataBind();
            PopBeneficiaries();
        }
        protected void dgBeneficiary_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgBeneficiary.EditItemIndex = -1;
            ScriptManager.RegisterStartupScript(this, GetType(), "receiveBeneficiaryModal", "showBeneficiaryModal();", true);
        }
        protected void dgBeneficiary_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgBeneficiary.DataKeys[e.Item.ItemIndex];
            Beneficiary beneficiary = _presenter.GetBeneficiaryById(id);
            try
            {
                _presenter.DeleteBeneficiary(beneficiary);
                BindBeneficiaries();

                Master.ShowMessage(new AppMessage("beneficiary was Removed Successfully", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete beneficiary. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "receiveBeneficiaryModal", "showBeneficiaryModal();", true);
        }
        protected void dgBeneficiary_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Beneficiary beneficiary = new Beneficiary();
            if (e.CommandName == "AddNew")
            {
                try
                {
                    TextBox txtName = e.Item.FindControl("txtBeneficiaryName") as TextBox;
                    beneficiary.BeneficiaryName = txtName.Text;
                    TextBox txtBankName = e.Item.FindControl("txtBankName") as TextBox;
                    beneficiary.BankName = txtBankName.Text;
                    TextBox txtAccountNumber = e.Item.FindControl("txtAccountNumber") as TextBox;
                    beneficiary.AccountNumber = txtAccountNumber.Text;
                    beneficiary.Status = "Active";
                    SaveBeneficiary(beneficiary);
                    dgBeneficiary.EditItemIndex = -1;
                    BindBeneficiaries();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Beneficiary " + ex.Message, RMessageType.Error));
                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
                }
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "receiveBeneficiaryModal", "showBeneficiaryModal();", true);
        }
        private void SaveBeneficiary(Beneficiary beneficiary)
        {
            try
            {
                if (beneficiary.Id <= 0)
                {
                    _presenter.SaveOrUpdateBeneficiary(beneficiary);
                    Master.ShowMessage(new AppMessage("Beneficiary Saved", RMessageType.Info));
                }
                else
                {
                    _presenter.SaveOrUpdateBeneficiary(beneficiary);
                    Master.ShowMessage(new AppMessage("Beneficiary Updated", RMessageType.Info));
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgBeneficiary_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgBeneficiary.EditItemIndex = e.Item.ItemIndex;
            BindBeneficiaries();
            ScriptManager.RegisterStartupScript(this, GetType(), "receiveBeneficiaryModal", "showBeneficiaryModal();", true);
        }
        protected void dgBeneficiary_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void dgBeneficiary_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int id = (int)dgBeneficiary.DataKeys[e.Item.ItemIndex];
            Beneficiary beneficiary = _presenter.GetBeneficiaryById(id);

            try
            {
                TextBox txtName = e.Item.FindControl("txtEdtBeneficiaryName") as TextBox;
                beneficiary.BeneficiaryName = txtName.Text;
                TextBox txtBankName = e.Item.FindControl("txtEdtBankName") as TextBox;
                beneficiary.BankName = txtBankName.Text;
                TextBox txtAccountNumber = e.Item.FindControl("txtEdtAccountNumber") as TextBox;
                beneficiary.AccountNumber = txtAccountNumber.Text;
                beneficiary.Status = "Active";
                SaveBeneficiary(beneficiary);
                dgBeneficiary.EditItemIndex = -1;
                BindBeneficiaries();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Beneficiary. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "receiveBeneficiaryModal", "showBeneficiaryModal();", true);
        }
        protected void lnkAddBeneficiary_Click(object sender, EventArgs e)
        {
            BindBeneficiaries();
            ScriptManager.RegisterStartupScript(this, GetType(), "receiveBeneficiaryModal", "showBeneficiaryModal();", true);
        }
        #endregion

    }

}
