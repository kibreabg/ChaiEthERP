using Chai.WorkflowManagment.CoreDomain.Approval;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using Microsoft.Practices.ObjectBuilder;
using System;
using System.Collections.Generic;
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
                if (_presenter.CurrentSoleVendorRequest != null)
                {
                    BindSoleVendorsGrid();
                }
            }
            this._presenter.OnViewLoaded();

            if (_presenter.CurrentSoleVendorRequest != null)
            {
                if (_presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor != null)
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
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.PoNumber = txtPONo.Text;
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.PODate = Convert.ToDateTime(txtDate.Text);
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.Billto = txtBillto.Text;
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.ShipTo = txtShipTo.Text;
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.DeliveryFees = Convert.ToDecimal(txtDeliveeryFees.Text);
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.PaymentTerms = txtPaymentTerms.Text;
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.DeliveryDate = Convert.ToDateTime(txtDeliveryDate.Text);
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.DeliveryLocation = txtDeliveryLocation.Text;
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.DeliveryBy = txtDeliveryBy.Text;
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.Status = "Completed";

                List<int> checkedSoleVendorDetailIds = (List<int>)Session["checkedSoleVendorDetailIds"];
                /*for (int i = 0; i < checkedSoleVendorDetailIds.Count; i++)
                {
                    _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.SoleVendorRequest.GetSoleVendorRequestDetail(checkedSoleVendorDetailIds[i]).POStatus = "Completed";
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
            dgPODetail.DataSource = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.PurchaseOrderSoleVendorDetails;
            dgPODetail.DataBind();
        }
        private void PrintTransaction()
        {
            if (_presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.SoleVendorRequest != null)
            {
                lblPOCreatedDate.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.PODate.ToShortDateString();
                lblPurchaseOrderNo.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.PoNumber;
                lblBillToResult.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.Billto;
                lblShipTo.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.ShipTo;
                lblPaymentTerms.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.PaymentTerms;
                lblDeliveryFees.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.DeliveryFees.ToString();
                lblSupplier.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.SoleVendorSupplier.SupplierName;
                lblSupplierContact.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.SoleVendorSupplier.SupplierContact;
                lblSupplierEmail.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.SoleVendorSupplier.Email;
                lblDeliverLocation.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.DeliveryLocation;
                lblDeliveryDate.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.DeliveryDate.ToShortDateString();
                lblDeliveryBy.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.DeliveryBy;

                grvDetails.DataSource = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.SoleVendorRequest.SoleVendorRequestDetails;
                grvDetails.DataBind();
            }
        }
        protected void grvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                decimal totalVAT = 0;
                decimal grandTotal = 0;
                decimal subTotal = 0;
                foreach (PurchaseOrderSoleVendorDetail posvd in _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.PurchaseOrderSoleVendorDetails)
                {
                    subTotal = subTotal + posvd.TotalCost;
                    totalVAT = totalVAT + posvd.VAT;
                    grandTotal = grandTotal + posvd.GrandTotal;
                }
                Label lblSubTotal = e.Row.FindControl("lblSubTotal") as Label;
                lblSubTotal.Text = subTotal.ToString();
                Label lblVAT = e.Row.FindControl("lblVAT") as Label;
                lblVAT.Text = totalVAT.ToString();
                Label lblGrandTotal = e.Row.FindControl("lblGrandTotal") as Label;
                lblGrandTotal.Text = grandTotal.ToString();
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
                                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.SoleVendorSupplier = _presenter.CurrentSoleVendorRequest.GetSoleVendorRequestDetail(soleVendorDetailId).SoleVendorSupplier;
                                txtSupplierName.Text = _presenter.CurrentSoleVendorRequest.GetSoleVendorRequestDetail(soleVendorDetailId).SoleVendorSupplier.SupplierName;
                                txtSupplierAddress.Text = _presenter.CurrentSoleVendorRequest.GetSoleVendorRequestDetail(soleVendorDetailId).SoleVendorSupplier.SupplierAddress;
                                txtSupplierContact.Text = _presenter.CurrentSoleVendorRequest.GetSoleVendorRequestDetail(soleVendorDetailId).SoleVendorSupplier.SupplierContact;
                                if (_presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.PurchaseOrderSoleVendorDetails.Count == 0)
                                {
                                    _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.PurchaseOrderSoleVendorDetails = new List<PurchaseOrderSoleVendorDetail>();
                                    foreach (SoleVendorRequestDetail svDetail in _presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails)
                                    {
                                        PurchaseOrderSoleVendorDetail POD = new PurchaseOrderSoleVendorDetail();
                                        POD.ItemAccount = _presenter.GetItemAccount(svDetail.ItemAccount.Id);
                                        POD.Item = svDetail.ItemDescription;
                                        POD.Qty = svDetail.Qty;
                                        POD.UnitCost = svDetail.UnitCost;
                                        POD.TotalCost = svDetail.TotalCost;
                                        POD.VAT = svDetail.VAT;
                                        POD.GrandTotal = svDetail.GrandTotal;
                                        _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.PurchaseOrderSoleVendorDetails.Add(POD);
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
                Master.ShowMessage(new AppMessage("Successfully did a Sole Vendor Purchase Order, Reference No - <b>'" + _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendor.PoNumber + "'</b>", RMessageType.Info));
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