using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using log4net;
using log4net.Config;
using Microsoft.Practices.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public partial class frmCashPaymentRequest : POCBasePage, ICashPaymentRequestView
    {
        private CashPaymentRequestPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                CheckApprovalSettings();
                BindCashPaymentRequests();
                BindCashPaymentDetails();
                PopPayee();
                BindPrograms();
                txtArrivalTime.Attributes.Add("readonly", "readonly");
                txtReturnTime.Attributes.Add("readonly", "readonly");
            }
            txtRequestDate.Text = DateTime.Today.Date.ToShortDateString();
            this._presenter.OnViewLoaded();

        }

        [CreateNew]
        public CashPaymentRequestPresenter Presenter
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
                return "{BECD9CB0-C328-4E89-ABD7-A54D1B2B3570}";
            }
        }
        #region Field Getters
        public int GetCashPaymentRequestId
        {
            get
            {
                if (grvCashPaymentRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvCashPaymentRequestList.SelectedDataKey.Value);
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
        public int GetPayee
        {
            get
            {
                if (String.IsNullOrEmpty(ddlPayee.SelectedValue))
                    return 0;
                else
                    return Convert.ToInt32(ddlPayee.SelectedValue);
            }
        }
        public string GetRequestType
        {
            get { return ddlRequestType.SelectedValue; }
        }
        public string GetDescription
        {
            get { return txtDescription.Text; }
        }
        public string GetArrivalDateTime
        {
            get { return txtArrivalTime.Text; }
        }
        public string GetReturnDateTime
        {
            get { return txtReturnTime.Text; }
        }
        public string GetVoucherNo
        {
            get { return AutoNumber(); }
        }
        public string GetAmountType
        {
            get { return ddlAmountType.SelectedValue; }
        }
        public int GetProgram
        {
            get { return Convert.ToInt32(ddlProgram.SelectedValue); }
        }
        #endregion
        private string AutoNumber()
        {
            return "CPV-" + _presenter.CurrentUser().Id.ToString() + "-" + (_presenter.GetLastCashPaymentRequestId() + 1).ToString();
        }
        private void CheckApprovalSettings()
        {
            if (_presenter.GetApprovalSetting(RequestType.CashPayment_Request.ToString().Replace('_', ' '), 0) == null)
            {
                pnlWarning.Visible = true;
            }
        }
        private void ClearFormFields()
        {
            // txtVoucherNo.Text = String.Empty;
            // txtVoucherNo.Text = String.Empty;
            ddlAmountType.SelectedValue = "";
        }
        private void BindCashPaymentDetails()
        {
            dgCashPaymentDetail.DataSource = _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails;
            dgCashPaymentDetail.DataBind();
        }
        private void BindCashPaymentRequests()
        {
            grvCashPaymentRequestList.DataSource = _presenter.ListCashPaymentRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text, ddlSrchSupplier.SelectedValue);
            grvCashPaymentRequestList.DataBind();
        }
        private void BindAttachments()
        {
            List<CPRAttachment> attachments = new List<CPRAttachment>();
            foreach (CashPaymentRequestDetail detail in _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails)
            {
                attachments.AddRange(detail.CPRAttachments);
            }

            Session["attachments"] = attachments;
            grvAttachments.DataSource = attachments;
            grvAttachments.DataBind();
        }
        private void PrintTransaction()
        {

            lblRequesterResult.Text = _presenter.CurrentCashPaymentRequest.AppUser.FullName;
            lblRequestedDateResult.Text = _presenter.CurrentCashPaymentRequest.RequestDate.Value.ToShortDateString();
            if (_presenter.CurrentCashPaymentRequest.Supplier != null)
            {
                lblSupplierRes.Text = _presenter.CurrentCashPaymentRequest.Supplier.SupplierName.ToString() != null ? _presenter.CurrentCashPaymentRequest.Supplier.SupplierName.ToString() : "";
            }
            lblPayeeResult.Text = _presenter.CurrentCashPaymentRequest.Payee;
            lblVoucherNoResult.Text = _presenter.CurrentCashPaymentRequest.VoucherNo;
            lblTotalAmountResult.Text = _presenter.CurrentCashPaymentRequest.TotalAmount.ToString();
            lblApprovalStatusResult.Text = _presenter.CurrentCashPaymentRequest.ProgressStatus.ToString();
            lblDescResult.Text = _presenter.CurrentCashPaymentRequest.Description;
            grvDetails.DataSource = _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails;
            grvDetails.DataBind();

            grvStatuses.DataSource = _presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses;
            grvStatuses.DataBind();
        }
        private void PopPayee()
        {
            ddlPayee.Items.Clear();
            ListItem lst = new ListItem
            {
                Text = " Select Payee ",
                Value = ""
            };
            ddlPayee.Items.Add(lst);
            ddlPayee.DataSource = _presenter.GetSuppliers();
            ddlPayee.DataBind();

            ddlSrchSupplier.Items.Clear();
            ListItem lstItem = new ListItem
            {
                Text = " Select Payee ",
                Value = ""
            };
            ddlSrchSupplier.Items.Add(lstItem);
            ddlSrchSupplier.DataSource = _presenter.GetSuppliers();
            ddlSrchSupplier.DataBind();
        }
        private void BindCashPaymentRequestFields()
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentCashPaymentRequest != null)
            {
                if (_presenter.CurrentCashPaymentRequest.Supplier != null)
                    ddlPayee.SelectedValue = _presenter.CurrentCashPaymentRequest.Supplier.Id.ToString();
                else
                    ddlPayee.SelectedValue = "";
                if (_presenter.CurrentCashPaymentRequest.Program != null)
                    ddlProgram.SelectedValue = _presenter.CurrentCashPaymentRequest.Program.Id.ToString();
                else
                    ddlProgram.SelectedValue = "";
                ddlRequestType.SelectedValue = _presenter.CurrentCashPaymentRequest.RequestType;
                ddlAmountType.SelectedValue = _presenter.CurrentCashPaymentRequest.AmountType;
                txtDescription.Text = _presenter.CurrentCashPaymentRequest.Description;
                txtArrivalTime.Text = _presenter.CurrentCashPaymentRequest.ArrivalDateTime;
                txtReturnTime.Text = _presenter.CurrentCashPaymentRequest.ReturnDateTime;
                ddlAmountType.SelectedValue = _presenter.CurrentCashPaymentRequest.AmountType;
                BindCashPaymentDetails();
                BindCashPaymentRequests();
            }
        }
        private void BindPrograms()
        {
            ddlProgram.DataSource = _presenter.GetPrograms();
            ddlProgram.DataBind();
        }
        private void BindProject(DropDownList ddlProject, int programID)
        {
            ddlProject.DataSource = _presenter.ListProjects(programID);
            ddlProject.DataValueField = "Id";
            ddlProject.DataTextField = "ProjectCode";
            ddlProject.DataBind();
        }
        private void BindGrant(DropDownList ddlGrant, int ProjectId)
        {
            ddlGrant.Items.Clear();
            ddlGrant.DataSource = _presenter.GetGrantbyprojectId(ProjectId);
            ddlGrant.DataValueField = "Id";
            ddlGrant.DataTextField = "GrantCode";
            ddlGrant.DataBind();
        }
        private void BindAccountDescription(DropDownList ddlAccountDescription)
        {
            if (ddlAmountType.SelectedValue == "Advanced")
            {
                ddlAccountDescription.DataSource = _presenter.GetAdvanceAccount();
            }
            else
            {
                ddlAccountDescription.DataSource = _presenter.ListItemAccounts();
            }

            ddlAccountDescription.DataValueField = "Id";
            ddlAccountDescription.DataTextField = "AccountName";
            ddlAccountDescription.DataBind();
        }
        protected void dgCashPaymentDetail_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgCashPaymentDetail.EditItemIndex = -1;
        }
        protected void dgCashPaymentDetail_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int cprId = (int)dgCashPaymentDetail.DataKeys[e.Item.ItemIndex];
            CashPaymentRequestDetail cprd;
            decimal totalAmount = 0;

            if (cprId > 0)
                cprd = _presenter.CurrentCashPaymentRequest.GetCashPaymentRequestDetail(cprId);
            else
                cprd = _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.ItemIndex];
            try
            {
                if (cprId > 0)
                {
                    _presenter.CurrentCashPaymentRequest.RemoveCashPaymentRequestDetail(cprId);
                    _presenter.DeleteCashPaymentRequestDetail(cprd);
                    _presenter.SaveOrUpdateCashPaymentRequest(_presenter.CurrentCashPaymentRequest);
                }
                else
                {
                    _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails.Remove(cprd);
                }

                foreach (CashPaymentRequestDetail cpd in _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails)
                {
                    totalAmount += cpd.Amount;
                }

                //Update the Total Amount
                _presenter.CurrentCashPaymentRequest.TotalAmount = totalAmount;

                BindCashPaymentDetails();

                Master.ShowMessage(new AppMessage("Payment Request Detail was Removed Successfully", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Payment Request Detail. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgCashPaymentDetail_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "AddNew")
            {
                try
                {
                    CashPaymentRequestDetail cprd = new CashPaymentRequestDetail();
                    cprd.CashPaymentRequest = _presenter.CurrentCashPaymentRequest;
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
                    CheckBox ckSupDocAttached = e.Item.FindControl("ckSupDocAttached") as CheckBox;
                    cprd.SupportDocAttached = ckSupDocAttached.Checked;
                    _presenter.CurrentCashPaymentRequest.TotalAmount += cprd.Amount;
                    if (ddlAmountType.SelectedValue == "Actual Amount")
                    {
                        cprd.ActualExpendture = Convert.ToDecimal(txtAmount.Text);
                    }
                    //Add Checklists for attachments if available                    
                    foreach (ItemAccountChecklist checklist in cprd.ItemAccount.ItemAccountChecklists)
                    {
                        CPRAttachment attachment = new CPRAttachment();
                        attachment.ItemAccountChecklists.Add(checklist);
                        cprd.CPRAttachments.Add(attachment);
                    }
                    _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails.Add(cprd);
                    if (ddlAmountType.SelectedValue != "Advanced")
                        BindAttachments();

                    dgCashPaymentDetail.EditItemIndex = -1;
                    BindCashPaymentDetails();

                    Master.ShowMessage(new AppMessage("Payment Detail Successfully Added", RMessageType.Info));
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Save Payment Detail" + ex.Message, RMessageType.Error));
                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
                }
            }
        }
        protected void dgCashPaymentDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgCashPaymentDetail.EditItemIndex = e.Item.ItemIndex;
            BindCashPaymentDetails();
            //BindCarRentals();
        }
        protected void dgCashPaymentDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            decimal previousAmount = 0;
            int CPRDId = (int)dgCashPaymentDetail.DataKeys[e.Item.ItemIndex];
            CashPaymentRequestDetail cprd;

            if (CPRDId > 0)
                cprd = _presenter.CurrentCashPaymentRequest.GetCashPaymentRequestDetail(CPRDId);
            else
                cprd = _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.ItemIndex];

            try
            {
                cprd.CashPaymentRequest = _presenter.CurrentCashPaymentRequest;
                TextBox txtAmount = e.Item.FindControl("txtEdtAmount") as TextBox;
                previousAmount = cprd.Amount; //This is the Total Amount of this request before any edit
                cprd.Amount = Convert.ToDecimal(txtAmount.Text);
                TextBox txtEdtAccountCode = e.Item.FindControl("txtEdtAccountCode") as TextBox;
                cprd.AccountCode = txtEdtAccountCode.Text;
                DropDownList ddlAccountDescription = e.Item.FindControl("ddlEdtAccountDescription") as DropDownList;
                cprd.ItemAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlAccountDescription.SelectedValue));
                DropDownList ddlProject = e.Item.FindControl("ddlEdtProject") as DropDownList;
                cprd.Project = _presenter.GetProject(Convert.ToInt32(ddlProject.SelectedValue));
                DropDownList ddlGrant = e.Item.FindControl("ddlEdtGrant") as DropDownList;
                cprd.Grant = _presenter.GetGrant(int.Parse(ddlGrant.SelectedValue));
                CheckBox ckEdtSupDocAttached = e.Item.FindControl("ckEdtSupDocAttached") as CheckBox;
                cprd.SupportDocAttached = ckEdtSupDocAttached.Checked;
                _presenter.CurrentCashPaymentRequest.TotalAmount -= previousAmount; //Subtract the previous Total amount
                _presenter.CurrentCashPaymentRequest.TotalAmount += cprd.Amount; //Then add the new individual amounts to the Total amount

                if (ddlAmountType.SelectedValue == "Actual Amount")
                {
                    cprd.ActualExpendture = Convert.ToDecimal(txtAmount.Text);
                }
                //Add Checklists for attachments if available but clear all attachments first because this is update                    
                cprd.CPRAttachments = new List<CPRAttachment>();
                foreach (ItemAccountChecklist checklist in cprd.ItemAccount.ItemAccountChecklists)
                {
                    CPRAttachment attachment = new CPRAttachment();
                    attachment.ItemAccountChecklists.Add(checklist);
                    cprd.CPRAttachments.Add(attachment);
                }
                BindAttachments();

                dgCashPaymentDetail.EditItemIndex = -1;
                BindCashPaymentDetails();
                Master.ShowMessage(new AppMessage("Payment Detail Successfully Updated", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Payment. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgCashPaymentDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlProject = e.Item.FindControl("ddlProject") as DropDownList;
                int programID = Convert.ToInt32(ddlProgram.SelectedValue);
                BindProject(ddlProject, programID);
                DropDownList ddlGrant = e.Item.FindControl("ddlGrant") as DropDownList;
                BindGrant(ddlGrant, Convert.ToInt32(ddlProject.SelectedValue));
                DropDownList ddlAccountDescription = e.Item.FindControl("ddlAccountDescription") as DropDownList;
                BindAccountDescription(ddlAccountDescription);
            }
            else
            {
                if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails != null)
                {
                    DropDownList ddlProject = e.Item.FindControl("ddlEdtProject") as DropDownList;
                    int programID = Convert.ToInt32(ddlProgram.SelectedValue);
                    if (ddlProject != null)
                    {
                        BindProject(ddlProject, programID);
                        if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].Project.Id != 0)
                        {
                            ListItem liI = ddlProject.Items.FindByValue(_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].Project.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }
                    }
                    DropDownList ddlAccountDescription = e.Item.FindControl("ddlEdtAccountDescription") as DropDownList;
                    if (ddlAccountDescription != null)
                    {
                        BindAccountDescription(ddlAccountDescription);
                        if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].ItemAccount.Id != 0)
                        {
                            ListItem liI = ddlAccountDescription.Items.FindByValue(_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].ItemAccount.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }
                    }
                    DropDownList ddlEdtGrant = e.Item.FindControl("ddlEdtGrant") as DropDownList;
                    if (ddlEdtGrant != null)
                    {
                        BindGrant(ddlEdtGrant, Convert.ToInt32(ddlProject.SelectedValue));
                        if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].Grant.Id != 0)
                        {
                            ListItem liI = ddlEdtGrant.Items.FindByValue(_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].Grant.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }

                    }
                }
            }
        }
        protected void grvCashPaymentRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["CashPaymentRequest"] = true;
            BindCashPaymentRequestFields();
            BindAttachments();
            if (_presenter.CurrentCashPaymentRequest.CurrentStatus != null)
            {
                btnSave.Visible = false;
                btnDelete.Visible = false;
                btnPrint.Visible = true;
                PrintTransaction();
                //If the Request has undergone approval process hide the Actions buttons from the Details Datagrid
                foreach (DataGridColumn col in dgCashPaymentDetail.Columns)
                {
                    if (col.HeaderText.ToLower().Trim() == "actions")
                    {
                        col.Visible = false;
                    }
                }
                grvAttachments.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
                btnDelete.Visible = true;
                foreach (DataGridColumn col in dgCashPaymentDetail.Columns)
                {
                    if (col.HeaderText.ToLower().Trim() == "actions")
                    {
                        col.Visible = true;
                    }
                }
                grvAttachments.Visible = true;
            }
        }
        protected void grvCashPaymentRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvCashPaymentRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void grvCashPaymentRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                CashPaymentRequest cashPayment = e.Row.DataItem as CashPaymentRequest;
                if (cashPayment.CurrentStatus == "Rejected")
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses[e.Row.RowIndex].Approver != 0)
                        e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses[e.Row.RowIndex].Approver).FullName;
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails.Count != 0)
                {
                    if ((ddlRequestType.SelectedValue == "Medical Expense (In-Patient)" || ddlRequestType.SelectedValue == "Medical Expense (Out-Patient)"))
                    {
                        //Validate Medical Expense limit from Web.Config before request is saved
                        if (ValidateMedExpLimit())
                        {
                            if (ddlAmountType.SelectedValue == "Actual Amount" && CheckReceiptsAttached())
                            {
                                _presenter.SaveOrUpdateCashPaymentRequest();
                                BindCashPaymentRequests();
                                Master.ShowMessage(new AppMessage("Successfully did a Payment Request, Reference No - <b>'" + _presenter.CurrentCashPaymentRequest.VoucherNo + "'</b> & Total Amount of - <b>'" + _presenter.CurrentCashPaymentRequest.TotalAmount.ToString() + "'Birr!</b>", RMessageType.Info));
                                Log.Info(_presenter.CurrentUser().FullName + " has requested a Payment of Total Amount " + _presenter.CurrentCashPaymentRequest.TotalAmount.ToString());
                                btnSave.Visible = false;

                                //Once the Request is saved hide the Actions buttons from the Details Datagrid
                                foreach (DataGridColumn col in dgCashPaymentDetail.Columns)
                                {
                                    if (col.HeaderText.ToLower().Trim() == "actions")
                                    {
                                        col.Visible = false;
                                    }
                                }

                            }
                            else if (ddlAmountType.SelectedValue == "Advanced")
                            {
                                Master.ShowMessage(new AppMessage("Please select Actual Amount from Amount Type, as this is a Medical Expense!", RMessageType.Error));
                            }
                            else
                            {
                                Master.ShowMessage(new AppMessage("Please attach the required receipts!", RMessageType.Error));
                            }
                        }
                        else
                        {
                            Master.ShowMessage(new AppMessage("You have exceeded your Medical Expense limit!", RMessageType.Error));
                        }
                    }
                    else
                    {
                        if (ddlAmountType.SelectedValue == "Actual Amount" && CheckReceiptsAttached())
                        {
                            if (CheckPerDiemDetailFilled())
                            {
                                _presenter.SaveOrUpdateCashPaymentRequest();
                                BindCashPaymentRequests();
                                Master.ShowMessage(new AppMessage("Successfully did a Payment  Request, Reference No - <b>'" + _presenter.CurrentCashPaymentRequest.VoucherNo + "'</b>", RMessageType.Info));
                                Log.Info(_presenter.CurrentUser().FullName + " has requested a Payment of Total Amount " + _presenter.CurrentCashPaymentRequest.TotalAmount.ToString());
                                btnSave.Visible = false;

                                //Once the Request is saved hide the Actions buttons from the Details Datagrid
                                foreach (DataGridColumn col in dgCashPaymentDetail.Columns)
                                {
                                    if (col.HeaderText.ToLower().Trim() == "actions")
                                    {
                                        col.Visible = false;
                                    }
                                }
                            }
                            else
                            {
                                Master.ShowMessage(new AppMessage("Please specify your (Arrival Date & Time) and (Return Date & Time)!", RMessageType.Error));
                            }

                        }
                        else if (ddlAmountType.SelectedValue == "Advanced")
                        {
                            _presenter.SaveOrUpdateCashPaymentRequest();
                            BindCashPaymentRequests();
                            Master.ShowMessage(new AppMessage("Successfully did a Payment  Request, Reference No - <b>'" + _presenter.CurrentCashPaymentRequest.VoucherNo + "'</b>", RMessageType.Info));
                            Log.Info(_presenter.CurrentUser().FullName + " has requested a Payment of Total Amount " + _presenter.CurrentCashPaymentRequest.TotalAmount.ToString());
                            btnSave.Visible = false;

                            //Once the Request is saved hide the Actions buttons from the Details Datagrid
                            foreach (DataGridColumn col in dgCashPaymentDetail.Columns)
                            {
                                if (col.HeaderText.ToLower().Trim() == "actions")
                                {
                                    col.Visible = false;
                                }
                            }
                        }
                        else
                        {
                            Master.ShowMessage(new AppMessage("Please attach the required receipts!", RMessageType.Error));
                        }
                    }

                }
                else
                {
                    Master.ShowMessage(new AppMessage("Please insert at least one Payment Detail", RMessageType.Error));
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
            _presenter.DeleteCashPaymentRequest(_presenter.CurrentCashPaymentRequest);
            ClearFormFields();
            BindCashPaymentRequests();
            BindCashPaymentDetails();
            btnDelete.Visible = false;
            Master.ShowMessage(new AppMessage("Payment Request Successfully Deleted", RMessageType.Info));
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindCashPaymentRequests();
            ScriptManager.RegisterStartupScript(this, GetType(), "showPaymentSearch", "showPaymentSearch();", true);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmCashPaymentRequest.aspx");
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
        protected void ddlAmountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCashPaymentDetails();
        }
        protected void ddlEdtAccountDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            TextBox txtEdtAccountCode = ddl.FindControl("txtEdtAccountCode") as TextBox;
            txtEdtAccountCode.Text = _presenter.GetItemAccount(Convert.ToInt32(ddl.SelectedValue)).AccountCode;
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
        protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCashPaymentDetails();
        }
        protected bool ValidateMedExpLimit()
        {
            decimal previousAmounts = 0;
            decimal previousInPatientAmounts = 0;
            decimal sharedFromOutPatient = 0;
            decimal totalRequestedAmount = 0;
            decimal requestedAmount = 0;
            decimal totalAllowedExp = (2 * Convert.ToDecimal(WebConfigurationManager.AppSettings["InPatientMarried"]));

            if (_presenter.GetUser(_presenter.CurrentUser().Id).Employee.MaritalStatus == "Married")
            {
                if (ddlRequestType.SelectedValue == "Medical Expense (In-Patient)")
                {
                    foreach (CashPaymentRequest cpr in _presenter.GetAllMedCPReqsThisYear())
                    {
                        previousAmounts += cpr.TotalAmount;
                    }
                    foreach (CashPaymentRequestDetail cprd in _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails)
                    {
                        requestedAmount += cprd.Amount;
                    }
                    totalRequestedAmount = previousAmounts + requestedAmount;

                    //If the requested amount is greater than the total allowed medical expense
                    //We allow the user to use the available amount and discard the rest of his requested amoount
                    if (totalRequestedAmount > totalAllowedExp)
                    {
                        if (totalAllowedExp - previousAmounts <= 0)
                        {
                            return false;
                        }
                        else
                        {
                            _presenter.CurrentCashPaymentRequest.TotalAmount = totalAllowedExp - previousAmounts;
                            _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[0].Amount = totalAllowedExp - previousAmounts;
                            return true;
                        }
                    }
                    else
                    { return true; }

                }
                else if (ddlRequestType.SelectedValue == "Medical Expense (Out-Patient)")
                {
                    foreach (CashPaymentRequest cpr in _presenter.GetAllInPatMedCPReqsThisYear())
                    {
                        previousInPatientAmounts += cpr.TotalAmount;
                    }

                    //If aggregate of In-Patient expenses exceed the In-Patient limit, that means
                    //we have shared a portion from the Out-Patient expense
                    if (previousInPatientAmounts > Convert.ToDecimal(WebConfigurationManager.AppSettings["InPatientMarried"]))
                    {
                        sharedFromOutPatient = previousInPatientAmounts - Convert.ToDecimal(WebConfigurationManager.AppSettings["InPatientMarried"]);

                        foreach (CashPaymentRequest cpr in _presenter.GetAllOutPatMedCPReqsThisYear())
                        {
                            previousAmounts += cpr.TotalAmount;
                        }
                        foreach (CashPaymentRequestDetail cprd in _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails)
                        {
                            requestedAmount += cprd.Amount;
                        }
                        totalRequestedAmount = previousAmounts + requestedAmount + sharedFromOutPatient;
                    }
                    else
                    {
                        foreach (CashPaymentRequest cpr in _presenter.GetAllOutPatMedCPReqsThisYear())
                        {
                            previousAmounts += cpr.TotalAmount;
                        }
                        foreach (CashPaymentRequestDetail cprd in _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails)
                        {
                            requestedAmount += cprd.Amount;
                        }
                        totalRequestedAmount = previousAmounts + requestedAmount;
                    }

                    //If the requested amount is greater than the total allowed medical expense
                    //We allow the user to use the available amount and discard the rest of his requested amoount
                    if (totalRequestedAmount > Convert.ToDecimal(WebConfigurationManager.AppSettings["OutPatientMarried"]))
                    {
                        if (Convert.ToDecimal(WebConfigurationManager.AppSettings["OutPatientMarried"]) - (previousAmounts + sharedFromOutPatient) <= 0)
                        {
                            return false;
                        }
                        else
                        {
                            _presenter.CurrentCashPaymentRequest.TotalAmount = Convert.ToDecimal(WebConfigurationManager.AppSettings["OutPatientMarried"]) - (previousAmounts + sharedFromOutPatient);
                            _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[0].Amount = Convert.ToDecimal(WebConfigurationManager.AppSettings["OutPatientMarried"]) - (previousAmounts + sharedFromOutPatient);
                            return true;
                        }
                    }
                    else
                        return true;
                }
            }
            else
            {
                if (ddlRequestType.SelectedValue == "Medical Expense (In-Patient)")
                {
                    foreach (CashPaymentRequest cpr in _presenter.GetAllMedCPReqsThisYear())
                    {
                        previousAmounts += cpr.TotalAmount;
                    }
                    foreach (CashPaymentRequestDetail cprd in _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails)
                    {
                        requestedAmount += cprd.Amount;
                    }
                    totalRequestedAmount = previousAmounts + requestedAmount;
                    //If the requested amount is greater than the total allowed medical expense
                    //We allow the user to use the available amount and discard the rest of his requested amoount
                    if (totalRequestedAmount > totalAllowedExp)
                    {
                        if (totalAllowedExp - previousAmounts <= 0)
                        {
                            return false;
                        }
                        else
                        {
                            _presenter.CurrentCashPaymentRequest.TotalAmount = totalAllowedExp - previousAmounts;
                            _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[0].Amount = totalAllowedExp - previousAmounts;
                            return true;
                        }
                    }
                    else
                    { return true; }

                }
                else if (ddlRequestType.SelectedValue == "Medical Expense (Out-Patient)")
                {
                    foreach (CashPaymentRequest cpr in _presenter.GetAllInPatMedCPReqsThisYear())
                    {
                        previousInPatientAmounts += cpr.TotalAmount;
                    }

                    //If aggregate of In-Patient expenses exceed the In-Patient limit, that means
                    //we have shared a portion from the Out-Patient expense
                    if (previousInPatientAmounts > Convert.ToDecimal(WebConfigurationManager.AppSettings["InPatientMarried"]))
                    {
                        sharedFromOutPatient = previousInPatientAmounts - Convert.ToDecimal(WebConfigurationManager.AppSettings["InPatientMarried"]);

                        foreach (CashPaymentRequest cpr in _presenter.GetAllOutPatMedCPReqsThisYear())
                        {
                            previousAmounts += cpr.TotalAmount;
                        }
                        foreach (CashPaymentRequestDetail cprd in _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails)
                        {
                            requestedAmount += cprd.Amount;
                        }
                        totalRequestedAmount = previousAmounts + requestedAmount + sharedFromOutPatient;
                    }
                    else
                    {
                        foreach (CashPaymentRequest cpr in _presenter.GetAllOutPatMedCPReqsThisYear())
                        {
                            previousAmounts += cpr.TotalAmount;
                        }
                        foreach (CashPaymentRequestDetail cprd in _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails)
                        {
                            requestedAmount += cprd.Amount;
                        }
                        totalRequestedAmount = previousAmounts + requestedAmount;
                    }

                    //If the requested amount is greater than the total allowed medical expense
                    //We allow the user to use the available amount and discard the rest of his requested amoount
                    if (totalRequestedAmount > Convert.ToDecimal(WebConfigurationManager.AppSettings["OutPatientMarried"]))
                    {
                        if (Convert.ToDecimal(WebConfigurationManager.AppSettings["OutPatientMarried"]) - previousAmounts <= 0)
                        {
                            return false;
                        }
                        else
                        {
                            _presenter.CurrentCashPaymentRequest.TotalAmount = Convert.ToDecimal(WebConfigurationManager.AppSettings["OutPatientMarried"]) - previousAmounts;
                            _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[0].Amount = Convert.ToDecimal(WebConfigurationManager.AppSettings["OutPatientMarried"]) - previousAmounts;
                            return true;
                        }
                    }
                    else
                        return true;
                }
            }
            return false;
        }
        protected bool CheckPerDiemDetailFilled()
        {
            int perDiemIncluded = 0;
            foreach (CashPaymentRequestDetail detail in _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails)
            {
                if (detail.ItemAccount.AccountName.Contains("Per Diem"))
                {
                    perDiemIncluded++;
                }
            }
            if (perDiemIncluded > 0 && !String.IsNullOrEmpty(txtArrivalTime.Text) && !String.IsNullOrEmpty(txtReturnTime.Text))
                return true;
            else if (perDiemIncluded == 0)
                return true;
            else
                return false;
        }
        #region Attachments
        protected bool CheckReceiptsAttached()
        {
            int numAttachedReceipts = 0;
            int numChecklists = 0;
            foreach (CashPaymentRequestDetail detail in _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails)
            {
                foreach (CPRAttachment attachment in detail.CPRAttachments)
                {
                    if (attachment.FilePath != null)
                    {
                        numAttachedReceipts++;
                    }
                }
            }
            foreach (CashPaymentRequestDetail detail in _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails)
            {
                foreach (ItemAccountChecklist checklist in detail.ItemAccount.ItemAccountChecklists)
                {
                    numChecklists++;
                }
            }
            if (numAttachedReceipts == numChecklists)
                return true;
            else
                return false;
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            Button uploadBtn = (Button)sender;
            GridViewRow attachmentRow = (GridViewRow)uploadBtn.NamingContainer;
            FileUpload fuReciept = attachmentRow.FindControl("fuReciept") as FileUpload;
            string fileName = Path.GetFileNameWithoutExtension(fuReciept.PostedFile.FileName);
            Regex rx = new Regex(@"[^A-Za-z0-9]");
            fileName = rx.Replace(fileName, "");
            string extension = Path.GetExtension(fuReciept.PostedFile.FileName);
            int index = 0;
            if (fileName != String.Empty)
            {
                List<CPRAttachment> attachments = (List<CPRAttachment>)Session["attachments"];
                if (attachments != null)
                {
                    foreach (CPRAttachment attachment in attachments)
                    {
                        if (attachment.ItemAccountChecklists[0].ChecklistName == attachmentRow.Cells[2].Text && attachmentRow.DataItemIndex == index)
                        {
                            attachment.FilePath = "~/CPUploads/" + fileName + DateTime.Now.ToString("ddMMyyyyHHmmss") + extension;
                            fuReciept.PostedFile.SaveAs(Server.MapPath("~/CPUploads/") + fileName + DateTime.Now.ToString("ddMMyyyyHHmmss") + extension);
                        }
                        index++;
                    }
                }

                BindAttachments();
                Master.ShowMessage(new AppMessage("Successfully uploaded the attachment", RMessageType.Info));

            }
            else
            {
                Master.ShowMessage(new AppMessage("Please select a file ", RMessageType.Error));
            }
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
            //Deleting attachment shouldn't take place since there are checklists implemented now
            string filePath = (sender as LinkButton).CommandArgument;
            foreach (CashPaymentRequestDetail detail in _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails)
            {
                foreach (CPRAttachment attachment in detail.CPRAttachments)
                {
                    if (attachment.FilePath == filePath)
                    {
                        detail.RemoveCPAttachment(filePath);
                        File.Delete(Server.MapPath(filePath));
                    }
                }
            }
            BindAttachments();
            //Response.Redirect(Request.Url.AbsoluteUri);
        }
        #endregion        
    }
}