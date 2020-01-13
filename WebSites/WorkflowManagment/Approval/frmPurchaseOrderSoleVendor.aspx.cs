using Chai.WorkflowManagment.CoreDomain.Approval;
using Chai.WorkflowManagment.CoreDomain.Request;
using Chai.WorkflowManagment.CoreDomain.Requests;
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

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public partial class frmPurchaseOrderSoleVendor : POCBasePage, IPurchaseOrderSoleVendorView
    {
        private PurchaseOrderSoleVendorPresenter _presenter;
        private SoleVendorRequest _solevendorrequest;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();

                if (_presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors == null)
                {
                    _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors = new PurchaseOrderSoleVendor();
                }

                BindSoleVendorsGrid();
            }
            this._presenter.OnViewLoaded();

            if (_presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors != null)
            {
                if (_presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.Id != 0)
                {
                    PrintTransaction();
                }
            }
        }

        [CreateNew]
        public PurchaseOrderSoleVendorPresenter Presenter
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
                return "{64D3AC5F-DD78-414C-98F8-63EC02CB9673}";
            }
        }
        public SoleVendorRequest SoleVendorRequest
        {
            get
            {
                return _solevendorrequest;
            }
            set
            {
                _solevendorrequest = value;
            }
        }
        public int SoleVendorRequestId
        {
            get
            {
                if (Convert.ToInt32(Request.QueryString["SoleVendorRequestId"]) != 0)
                {
                    return Convert.ToInt32(Request.QueryString["SoleVendorRequestId"]);
                }
                return 0;
            }
        }
        public string RequestType
        {
            get
            {
                if (Request.QueryString["RequestType"].ToString() != string.Empty)
                {
                    return Request.QueryString["RequestType"].ToString();
                }
                return string.Empty;
            }
        }
        private void AutoNumber()
        {
            txtPONo.Text = "POSV-" + (_presenter.GetLastPurchaseOrderSoleVendorId() + 1);
        }
        private void BindSoleVendorsGrid()
        {
            grvSoleVendPO.DataSource = _presenter.CurrentSoleVendorRequest.GetPendingPurchaseOrderDetails();
            grvSoleVendPO.DataBind();
        }
        private void SavePurchaseOrder()
        {
            try
            {
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PoNumber = txtPONo.Text;
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PODate = Convert.ToDateTime(txtDate.Text);
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.Billto = txtBillto.Text;
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.ShipTo = txtShipTo.Text;
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.DeliveryFees = Convert.ToDecimal(txtDeliveeryFees.Text);
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PaymentTerms = txtPaymentTerms.Text;
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.DeliveryDate = Convert.ToDateTime(txtDeliveryDate.Text);
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.DeliveryLocation = txtDeliveryLocation.Text;
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.DeliveryBy = txtDeliveryBy.Text;
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.Status = "Completed";

                List<int> checkedSoleVendorDetailIds = (List<int>)Session["checkedSoleVendorDetailIds"];
                /*for (int i = 0; i < checkedSoleVendorDetailIds.Count; i++)
                {
                    _presenter.CurrentSoleVendorRequest.GetSoleVendorRequestDetail(checkedSoleVendorDetailIds[i]).POStatus = "Completed";
                }*/


                Master.ShowMessage(new AppMessage("Purchase Order Successfully Approved", RMessageType.Info));

            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("There was an error Saving the Purchase Order", RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        private void BindPODetailForSole()
        {
            dgPODetail.DataSource = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PurchaseOrderSoleVendorDetails;
            dgPODetail.DataBind();
        }
        private void PrintTransaction()
        {
            if (_presenter.CurrentSoleVendorRequest != null)
            {
                lblPOCreatedDate.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PODate.ToShortDateString();
                lblPurchaseOrderNo.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PoNumber;
                lblBillToResult.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.Billto;
                lblShipTo.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.ShipTo;
                lblPaymentTerms.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PaymentTerms;
                lblDeliveryFees.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.DeliveryFees.ToString();
                lblSupplier.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.SoleVendorSupplier.SupplierName;
                lblSupplierContact.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.SoleVendorSupplier.SupplierContact;
                lblSupplierEmail.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.SoleVendorSupplier.Email;
                lblDeliverLocation.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.DeliveryLocation;
                lblDeliveryDate.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.DeliveryDate.ToShortDateString();
                lblDeliveryBy.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.DeliveryBy;

                grvDetails.DataSource = _presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails;
                grvDetails.DataBind();
            }
        }
        protected void btnCreatePO_Click(object sender, EventArgs e)
        {
            try
            {
                _presenter.OnViewLoaded();
                btnRequest.Visible = true;
                pnlInfo.Visible = false;
                if (_presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails != null)
                {
                    List<int> checkedSoleVendorDetailIds = new List<int>();
                    foreach (GridViewRow item in grvSoleVendPO.Rows)
                    {
                        int soleVendorDetailId = (int)grvSoleVendPO.DataKeys[item.RowIndex].Value;
                        if (item.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chk = (CheckBox)item.FindControl("chkSelect");
                            if (chk.Checked)
                            {
                                //Collect the Ids of the selected Sole Vendor Detail objects                                
                                checkedSoleVendorDetailIds.Add(soleVendorDetailId);

                                AutoNumber();
                                txtDate.Text = DateTime.Today.ToString();
                                txtRequester.Text = _presenter.GetUser(_presenter.CurrentSoleVendorRequest.AppUser.Id).FullName;
                                //Assign the Sole Vendor Supplier value to the Purchase Order
                                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.SoleVendorSupplier = _presenter.CurrentSoleVendorRequest.GetSoleVendorRequestDetail(soleVendorDetailId).SoleVendorSupplier;
                                txtSupplierName.Text = _presenter.CurrentSoleVendorRequest.GetSoleVendorRequestDetail(soleVendorDetailId).SoleVendorSupplier.SupplierName;
                                txtSupplierAddress.Text = _presenter.CurrentSoleVendorRequest.GetSoleVendorRequestDetail(soleVendorDetailId).SoleVendorSupplier.SupplierAddress;
                                txtSupplierContact.Text = _presenter.CurrentSoleVendorRequest.GetSoleVendorRequestDetail(soleVendorDetailId).SoleVendorSupplier.SupplierContact;
                                if (_presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PurchaseOrderSoleVendorDetails.Count == 0)
                                {
                                    _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PurchaseOrderSoleVendorDetails = new List<PurchaseOrderSoleVendorDetail>();
                                    foreach (SoleVendorRequestDetail svDetail in _presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails)
                                    {
                                        PurchaseOrderSoleVendorDetail POD = new PurchaseOrderSoleVendorDetail();
                                        POD.ItemAccount = _presenter.GetItemAccount(svDetail.ItemAccount.Id);
                                        POD.Item = svDetail.ItemDescription;
                                        POD.Qty = svDetail.Qty;
                                        POD.UnitCost = svDetail.UnitCost;
                                        POD.TotalCost = svDetail.TotalCost;
                                        _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PurchaseOrderSoleVendorDetails.Add(POD);
                                    }
                                    BindPODetailForSole();
                                }
                                else
                                {
                                    BindPODetailForSole();
                                }
                            }
                        }
                    }
                    Session["checkedSoleVendorDetailIds"] = checkedSoleVendorDetailIds;
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to bind Purchase Order " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void btnRequest_Click(object sender, EventArgs e)
        {
            try
            {
                SavePurchaseOrder();
                _presenter.SaveOrUpdateSoleVendorRequest(_presenter.CurrentSoleVendorRequest);
                PrintTransaction();
                btnPrintPurchaseOrder.Enabled = true;
                btnRequest.Enabled = false;
                Master.ShowMessage(new AppMessage("Successfully did a Sole Vendor Purchase Order, Reference No - <b>'" + _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PoNumber + "'</b>", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Unable to save Purchase order", RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }

        }
    }
}