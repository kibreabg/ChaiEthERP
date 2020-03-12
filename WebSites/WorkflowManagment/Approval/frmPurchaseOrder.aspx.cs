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
    public partial class frmPurchaseOrder : POCBasePage, IPurchaseOrderView
    {
        private PurchaseOrderPresenter _presenter;
        private BidAnalysisRequest _bidanalysisrequest;
        BidAnalysisRequest bid;
        private int reqid;
        decimal tot = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Session["BAR"] = _presenter.CurrentBidAnalysisRequest.Id;
              
                this._presenter.OnViewInitialized();
                if (_presenter.CurrentBidAnalysisRequest.PurchaseOrders == null)
                {
                    _presenter.CurrentBidAnalysisRequest.PurchaseOrders = new PurchaseOrder();
                }


                //  PopPurchaseRequestForPO();
                 BindPurchaseOrder();
                BindPOforBid();
                btnPrintPurchaseForm.Enabled = true;
              
               //  PrintTransaction();
                  
               // BindRepeater();  
            }
            this._presenter.OnViewLoaded();
            
            // BindPurchaseOrder();
            if (_presenter.CurrentBidAnalysisRequest.PurchaseOrders != null)
            {
                if (_presenter.CurrentBidAnalysisRequest.PurchaseOrders.Id != 0)
                {
                    PrintTransaction();
                 

                }
            }
           

            //btnPrintworksheet.Attributes.Add("onclick", "javascript:Clickheretoprint('divprint'); return false;");
            //BindJS();
        }

        [CreateNew]
        public PurchaseOrderPresenter Presenter
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
        public CoreDomain.Requests.BidAnalysisRequest BidAnalysisRequest
        {
            get
            {
                return _bidanalysisrequest;
            }
            set
            {
                _bidanalysisrequest = value;
            }
        }
      
        public string RequestNo
        {
            get { return string.Empty; }
        }
        public string RequestDate
        {
            get { return string.Empty; }
        }
        public int BidAnalysisRequestId
        {

            get
            {
               
                if (Convert.ToInt32(Request.QueryString["BidAnalysisRequestId"]) != 0)
                {
                    return Convert.ToInt32(Request.QueryString["BidAnalysisRequestId"]);
                }
                else if (Convert.ToInt32(Session["ReqID"]) != 0)
                {
                    return Convert.ToInt32(Session["ReqID"]);
                }

                else
                {
                    return 0;
                }
            }
            set
            {
                reqid = value;
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
            txtPONo.Text = "POBA-" + (_presenter.GetLastPurchaseOrderId() + 1);
        }

        //private void PopPurchaseRequestForPO()
        //{
        //    ddlPurchaseReqForPO.Items.Clear();
        //    ListItem lst = new ListItem();
        //    lst.Text = " Select Request No ";
        //    lst.Value = "0";
        //    ddlPurchaseReqForPO.Items.Add(lst);
        //    ddlPurchaseReqForPO.DataSource = _presenter.GetPurchaseRequestListInProgressForPO();
        //    ddlPurchaseReqForPO.DataBind();
        //}
        private void BindPurchaseOrder()
        {
            this._presenter.OnViewLoaded();
                     
                    if (_presenter.CurrentBidAnalysisRequest.BidderItemDetails != null)
                    {



                        foreach (GridViewRow item in grvBidforPO.Rows)
                        {
                            int BId = (int)grvBidforPO.DataKeys[item.RowIndex].Value;
                            Session["BAR"] = BId;
                            // check row is datarow
                            if (item.RowType == DataControlRowType.DataRow)
                            {
                                CheckBox chk = (CheckBox)item.FindControl("chkSelect");
                                if (chk.Checked)
                                {
                                    foreach (Bidder detail in _presenter.CurrentBidAnalysisRequest.GetBidderbyRank())
                                    {
                                if (detail.Id == BId && detail.Rank == 1 && chk.Checked)
                                {
                                    PurchaseOrderDetail POD = new PurchaseOrderDetail();
                                    POD.ItemAccount = _presenter.GetItemAccount(detail.BidderItemDetail.ItemAccount.Id);
                                    POD.Qty = detail.Qty;
                                    POD.UnitCost = detail.UnitCost;
                                    POD.TotalCost = detail.TotalCost;
                                    POD.Rank = detail.Rank;
                                    POD.Vat = Convert.ToDecimal((POD.TotalCost * 15) / 100);
                                   // _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PurchaseOrderDetails.Add(POD);

                                    txtPONo.Text = "PO-" + (_presenter.GetLastPurchaseOrderId() + 1).ToString();
                                    txtRequester.Text = _presenter.CurrentUser().FirstName + " " + _presenter.CurrentUser().LastName;
                                    txtDate.Text = DateTime.Today.Date.ToShortDateString();
                                    txtSupplierName.Text = detail.Supplier.SupplierNameType;
                                    txtSupplierAddress.Text = detail.Supplier.SupplierAddress;
                                    //detail.POStatus = "Completed";
                                }
                               }
                                }
                            
                       
                    }


                }
            }
        }

        private void BindPOforBid()
        {
            int id = Convert.ToInt32(Session["BAR"]);

            foreach(Bidder detail in _presenter.CurrentBidAnalysisRequest.GetBidderbyRankBIDID(id))
            {
                if (detail.POStatus == "InProgress")
                {
                    grvBidforPO.DataSource = _presenter.CurrentBidAnalysisRequest.GetBidderbyRankBIDID(id);
                    //  grvDetails.DataSource = _presenter.ListPurchaseReqInProgress();
                    
                    grvBidforPO.DataBind();
                }
            }


        }
        private void SavePurchaseOrder()
        {
            
            try
            {
                
                _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PoNumber = txtPONo.Text;
                _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PODate = Convert.ToDateTime(txtDate.Text);
                _presenter.CurrentBidAnalysisRequest.PurchaseOrders.Billto = txtBillto.Text;
                _presenter.CurrentBidAnalysisRequest.PurchaseOrders.ShipTo = txtShipTo.Text;
                _presenter.CurrentBidAnalysisRequest.PurchaseOrders.DeliveryFees = Convert.ToDecimal(txtDeliveeryFees.Text);
                _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PaymentTerms = txtPaymentTerms.Text;
                _presenter.CurrentBidAnalysisRequest.PurchaseOrders.TotalPrice = 0;
                _presenter.CurrentBidAnalysisRequest.PurchaseOrders.DeliveryDate = Convert.ToDateTime(txtDeliveryDate.Text);
                _presenter.CurrentBidAnalysisRequest.PurchaseOrders.DeliveryLocation = txtDelLoc.Text;
                _presenter.CurrentBidAnalysisRequest.PurchaseOrders.DeliveryBy = txtDeliveryBy.Text;
                _presenter.CurrentBidAnalysisRequest.PurchaseOrders.Status = "Completed";
               // _presenter.CurrentBidAnalysisRequest.PurchaseOrders.BidAnalysisRequest = _presenter.CurrentBidAnalysisRequest;
                //if (_presenter.CurrentBidAnalysisRequest != null)
                //{
                //    if (_presenter.CurrentBidAnalysisRequest.BidderItemDetails[0].Bidders[0].Rank == 1)
                //    {
                //        _presenter.CurrentBidAnalysisRequest.PurchaseOrders.Supplier = txtSupplierName;
                //    }
                //}

                List<int> checkedBidItemDetailIds = (List<int>)Session["checkedBidItemDetailIds"];
                AddPurchasingItem();


                //_presenter.CurrentBidAnalysisRequest.PurchaseOrders.Status = "Completed";       
                Master.ShowMessage(new AppMessage("Purchase Order Successfully Produced", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("There was an error Saving Purchase Order", Chai.WorkflowManagment.Enums.RMessageType.Error));

            }
           

               

            
        }
        private void BindPODetail()
        {

            foreach (BidderItemDetail BID in _presenter.CurrentBidAnalysisRequest.BidderItemDetails)
            {
                if (BID.GetBidderbyRank() != null)
                {
                    dgPODetail.DataSource = BID.Bidders;
                    dgPODetail.DataBind();
                }
            }

        }


        #region PurchaseOrderDetail
        private void AddPurchasingItem()
        {
            

                if (_presenter.CurrentBidAnalysisRequest != null)
                {


                foreach (PurchaseOrderDetail det in _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PurchaseOrderDetails)
                {
                    det.Status = "Closed";
                }


                    foreach (GridViewRow item in grvBidforPO.Rows)
                    {
                        int BId = (int)grvBidforPO.DataKeys[item.RowIndex].Value;
                        Session["BAR"] = BId;
                        // check row is datarow
                        if (item.RowType == DataControlRowType.DataRow)
                        {
                        foreach (Bidder detail in _presenter.CurrentBidAnalysisRequest.GetBidderbyRank())
                        {
                            CheckBox chk = (CheckBox)item.FindControl("chkSelect");
                            if (chk.Checked && BId==detail.Id )
                            {
                              

                                
                                    if (detail.POStatus != "Completed" && chk.Checked)
                                {
                                    PurchaseOrderDetail POD = new PurchaseOrderDetail();
                                    decimal x = detail.TotalCost * 15 / 100;
                                    POD.ItemAccount = _presenter.GetItemAccount(detail.BidderItemDetail.ItemAccount.Id);
                                    POD.ItemDescription = detail.Item;
                                    POD.Vat = x;
                                    POD.Qty = detail.Qty;
                                    POD.UnitCost = detail.UnitCost;
                                    POD.TotalCost = detail.TotalCost;
                                    POD.Bidder = _presenter.GetBidderbyId(detail.Id);
                                    
                                    POD.Rank = detail.Rank;
                                    POD.Supplier= detail.Supplier;
                                    POD.Status = "Open";
                                  
                                    _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PurchaseOrderDetails.Add(POD);
                                }
                                }
                            }
                        }

                    }

                    //BindPODetail();
              
            }
        }



        #endregion
        protected void btnRequest_Click(object sender, EventArgs e)
        {
           
                try
                {
                    SavePurchaseOrder();
                    _presenter.SaveOrUpdateBidAnalysisRequest(_presenter.CurrentBidAnalysisRequest);
                   // BindRepeater();
                    PrintTransaction();
                  
                    btnPrintPurchaseForm.Enabled = true;
                btnRequest.Enabled = false;
                Master.ShowMessage(new AppMessage("Successfully did a Purchase Order, Reference No - <b>'" + _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PoNumber + "'</b>", Chai.WorkflowManagment.Enums.RMessageType.Info));
                // Response.Redirect(String.Format("frmPurchaseApproval.aspx?PurchaseRequestId={0}&PnlStatus={1}", _presenter.CurrentBidAnalysisRequest.Id, "Enabled"));
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("frmPurchaseApproval.aspx?PurchaseRequestId={0}&PnlStatus={1}", _presenter.CurrentBidAnalysisRequest.Id, "Enabled"));
        }
        private void PrintTransaction()
        {
            if (_presenter.CurrentBidAnalysisRequest != null)
            {
                lblPOCreatedDate.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PODate.ToString();
            lblPurchaseOrderNo.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PoNumber;
            lblBillToResult.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.Billto;
            lblShipTo.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.ShipTo;
            lblPaymentTerms.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PaymentTerms;
            lblDeliveryFees.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.DeliveryFees.ToString();
            lblDeliverLocation.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.DeliveryLocation;
            lblDeliveryDate.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.DeliveryDate.ToString();
            lblDeliveryBy.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.DeliveryBy;
              
                List<int> checkedBidItemDetailIds = new List<int>();
                Session["checkedBidItemDetailIds"] = checkedBidItemDetailIds;

                //if (_presenter.CurrentBidAnalysisRequest != null && _presenter.CurrentBidAnalysisRequest.BidderItemDetails[0].Bidders[0].Rank == 1)
                //{
                //    lblItemResult.Text = _presenter.CurrentBidAnalysisRequest.BidderItemDetails[0].ItemDescription;
                //}
                //lblDeliveryDateresult.Text = _presenter.CurrentBidAnalysisRequest.PurchaseRequest.Requireddateofdelivery.ToShortDateString();
                //    lblSuggestedSupplierResult.Text = _presenter.CurrentBidAnalysisRequest.BidderItemDetails[0].Bidders[0].Supplier.SupplierName;
                foreach (GridViewRow item in grvBidforPO.Rows)
                {
                    int bidDetailId = (int)grvBidforPO.DataKeys[item.RowIndex].Value;

                    CheckBox chk = (CheckBox)item.FindControl("chkSelect");
                  
                     

                        foreach (Bidder detail in _presenter.CurrentBidAnalysisRequest.GetBidderbyRank())
                        {
                        if (chk.Checked && bidDetailId == detail.Id)
                        {
                            if ((detail.Supplier.Id == _presenter.CurrentBidAnalysisRequest.GetBidderbySelectedItem(bidDetailId).Supplier.Id) && (chk.Checked) && (detail.Id == bidDetailId))

                            {
                                //lblSelectedbyResult.Text = _presenter.GetUser(_presenter.CurrentBidAnalysisRequest.AppUser.Id).FullName;
                                //dgPODetail.DataSource = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PurchaseOrderDetails;
                                //dgPODetail.DataBind();


                                lblSupp.Text = _presenter.CurrentBidAnalysisRequest.GetBidderbySelectedItem(bidDetailId).Supplier.SupplierName;
                            }
                        }
                      }

                   
                  
                    
                }
                foreach (PurchaseOrderDetail pod in _presenter.CurrentBidAnalysisRequest.PurchaseOrders.GetPurchaseOrderDetailByStatus())
                {

                    if (_presenter.CurrentBidAnalysisRequest.PurchaseOrders.PurchaseOrderDetails.Count > 0)
                    {
                        lblTotCost.Text = "";
                        decimal delfee = Convert.ToDecimal(lblDeliveryFees.Text);
                        tot = tot + pod.TotalCost;
                        decimal vat = tot * Convert.ToDecimal(0.15);
                        lblTotCost.Text = Math.Round(Convert.ToDecimal(delfee + tot + vat), 2).ToString();
                      
                        //grvDetails.DataSource = _presenter.CurrentBidAnalysisRequest.GetBidderbySelectedItem(bidDetailId);

                    }
                }

                grvDetails.DataSource = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.GetPurchaseOrderDetailByStatus();
                grvDetails.DataBind();
            }
            }
               
           
        
        
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
            if (_presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses[e.Row.RowIndex].Approver).FullName;
                }
            }
           
        }

        protected void ddlPurchaseReqForPO_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

      
           
              
        protected void btnCreatePO_Click(object sender, EventArgs e)
        {

            string x;
            string y;


            try
            {
                _presenter.OnViewLoaded();
                //_presenter.CurrentBidAnalysisRequest.PurchaseOrders.RemovePurchaseOrderAll();
                
                btnRequest.Visible = true;
                pnlInfo.Visible = false;
                if (_presenter.CurrentBidAnalysisRequest.BidderItemDetails != null)
                {
                        List<int> checkedBidItemDetailIds = new List<int>();
                    foreach (GridViewRow item in grvBidforPO.Rows)
                    {

                        x = grvBidforPO.Rows[0].Cells[2].Text;
                        bool control = true;

                        for (int i = 0; i < grvBidforPO.Rows.Count; i++)
                        {
                            for (int col = 0; col < grvBidforPO.Columns.Count; col++)
                            {
                               
                                    y = grvBidforPO.Rows[i].Cells[2].Text;
                                    if (x == y)
                                    {

                                        int bidDetailId = (int)grvBidforPO.DataKeys[item.RowIndex].Value;
                                        if (item.RowType == DataControlRowType.DataRow)
                                        {
                                            CheckBox chk = (CheckBox)item.FindControl("chkSelect");

                                            foreach (Bidder detail in _presenter.CurrentBidAnalysisRequest.GetBidderbyRank())
                                            {
                                                if ((detail.Supplier.Id == _presenter.CurrentBidAnalysisRequest.GetBidderbySelectedItem(bidDetailId).Supplier.Id) && (chk.Checked) && (detail.Id == bidDetailId))

                                                {
                                                    //Collect the Ids of the selected Sole Vendor Detail objects                                
                                                    checkedBidItemDetailIds.Add(bidDetailId);
                                                    txtDate.Text = DateTime.Today.ToString();
                                                    txtDeliveryDate.Text = DateTime.Today.ToString();
                                                    txtPONo.Text = "POBA-" + (_presenter.GetLastPurchaseOrderId() + 1).ToString();

                                                    txtRequester.Text = _presenter.GetUser(_presenter.CurrentBidAnalysisRequest.AppUser.Id).FullName;
                                                    //Assign the Sole Vendor Supplier value to the Purchase Order

                                                    txtSupplierName.Text = detail.Supplier.SupplierNameType;
                                                    txtSupplierAddress.Text = detail.Supplier.SupplierAddress;

                                                }






                                                // }
                                            }
                                            Session["checkedBidItemDetailIds"] = checkedBidItemDetailIds;

                                        }
                                    }
                                    else
                                    {
                                        Master.ShowMessage(new AppMessage("Error: Suppliers are different  ", RMessageType.Error));
                                        break;
                                    }
                                
                                }
                            }




                    }
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to bind Purchase Order " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }

        protected void grvRankedBidders_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            
           int BId   = Convert.ToInt32(grvRankedBidders.SelectedDataKey[0]);
            Session["BAR"] = BId;
            
            _presenter.OnViewLoaded();
            btnRequest.Visible = true;
        
            pnlInfo.Visible = false;

            BindPurchaseOrder();
        }
    }
}