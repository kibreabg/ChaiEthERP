using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using log4net;
using log4net.Config;
using Microsoft.Practices.ObjectBuilder;
using System.IO;
using Chai.WorkflowManagment.CoreDomain.Request;
using System.Data;
using Chai.WorkflowManagment.CoreDomain.Setting;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public partial class frmBidAnalysisRequest : POCBasePage, IBidAnalysisRequestView
    {

        private BidAnalysisRequestPresenter _presenter;
        private IList<BidAnalysisRequest> _BidAnalysisRequests;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        BidderItemDetail bidditem;
        BidAnalysisRequest bid;
        Bidder bidd;
        private decimal totalamaount = 0;
        private decimal price = 0;
        int prId;
        int count = 0;
        //PurchaseRequest purchaseRequest = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {

                Session["PR"] = _presenter.GetPurchaseRequestList();
                bidd= Session["bidd"] as Bidder;
                
                this._presenter.OnViewInitialized();
               // BindBidItem(Session["bidditem"] as BidderItemDetail);
                XmlConfigurator.Configure();
                CheckApprovalSettings();
                //PopProjects();

                //Fill the Bid Analysis Request with the Purchase Request information
                PopPurchaseRequestsDropDown();
               
                PopBidAnalysisRequesters();
                // PopProjects();

                
                BindBidAnalysisRequestsList();


                if (_presenter.CurrentBidAnalysisRequest.Id <= 0)
                {
                    AutoNumber();
                    //  btnDelete.Visible = false;
                }
            }
            this._presenter.OnViewLoaded();
            
            //   PopPurchaseRequestsDropDown();
            PopPurchaseRequest();
           // PopBidAnalysisRequesters();
            BindBidAnalysisRequests();
            txtRequestDate.Text = DateTime.Today.Date.ToShortDateString();
            
            if (_presenter.CurrentBidAnalysisRequest != null)
            {
                
                if (_presenter.CurrentBidAnalysisRequest.Id != 0)
                {

                    PrintTransaction();
                    btnPrintworksheet.Enabled = true;
                    BindBidItem();
                }
               
                         
                
            }

            if (_presenter.CurrentBidAnalysisRequest.BidderItemDetails.Count > 0)
            {
                BindBidItem();
                BindBidders();
            }
            
            //else if (_presenter.CurrentBidAnalysisRequest.BidderItemDetails[0].Bidders.Count>0)
            //{

            //}


        }
        [CreateNew]
        public BidAnalysisRequestPresenter Presenter
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
                return "{D1B7939C-7154-4403-B535-B4D33684CE21}";
            }
        }
        #region Field Getters
        public int GetBidAnalysisRequestId
        {
            get
            {
                if (Convert.ToInt32(Request.QueryString["BidAnalysisRequestId"]) != 0)
                {
                    return Convert.ToInt32(Request.QueryString["BidAnalysisRequestId"]);
                }
                return 0;
            }
        }
        public int GetPurchaseRequestId
        {
            get
            {
                if (Convert.ToInt32(Request.QueryString["PurchaseRequestId"]) != 0)
                {
                    return Convert.ToInt32(Request.QueryString["PurchaseRequestId"]);
                }
                return 0;
            }
        }
        public int GetBARequestId
        {
            get
            {
                if (grvBidAnalysisRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvBidAnalysisRequestList.SelectedDataKey.Value);
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
        public DateTime GetRequestDate
        {
            get { return Convert.ToDateTime(txtRequestDate.Text); }
        }
      
        public string GetNeededFor
        {
            get { return txtselectionfor.Text; }
        }

        public string GetProject
        {
            get { return ddlProject.Text; }
        }
        public string GetGrant
        {
            get { return ddlGrant.Text; }
        }


        public IList<BidAnalysisRequest> BidAnalysisRequests
        {
            get
            {
                return _BidAnalysisRequests;
            }
            set
            {
                _BidAnalysisRequests = value;
            }
        }

        public string GetReasonForSelection
        {
            get
            {
                return txtselectionfor.Text;
            }
        }

      

     

       
        #endregion

        private void BindBidAnalysisRequests()
        {
            if (!this.IsPostBack)
            {
                PurchaseRequest purchaseRequest = Session["PR"] as PurchaseRequest;
                //_presenter.CurrentBidAnalysisRequest.PurchaseRequest = purchaseRequest;


                if (purchaseRequest != null)
                {
                    foreach (PurchaseRequestDetail PD in purchaseRequest.PurchaseRequestDetails)
                    {
                        BidderItemDetail detail = new BidderItemDetail();
                        detail.ItemAccount = PD.ItemAccount;
                        detail.ItemDescription = PD.Item;
                        detail.Qty = PD.Qty;
                        detail.Project = PD.Project;
                        detail.Grant = PD.Grant;
                        _presenter.CurrentBidAnalysisRequest.BidderItemDetails.Add(detail);
                    }

                }
                dgItemDetail.DataSource = _presenter.CurrentBidAnalysisRequest.BidderItemDetails;
                dgItemDetail.DataBind();
                grvAttachments.DataSource = _presenter.CurrentBidAnalysisRequest.BAAttachments;
                grvAttachments.DataBind();
                PopPurchaseRequest();
            }
        }
        #region BidderItemDetails
        protected void btnCancedetail_Click(object sender, EventArgs e)
        {
            decimal cost = 0;
            if (_presenter.CurrentBidAnalysisRequest.BidderItemDetails.Count > 0)
            {

                //////foreach (Bidder detail in _presenter.CurrentBidAnalysisRequest.BidderItemDetails[0].Bidders)
                //////{
                //////    if (detail.Rank == 1)
                //////    {
                //////        foreach (BidderItemDetail biddetail in detail.BidderItemDetail.GetBidderByItemId)
                //////        {

                //////            cost = cost + biddetail.TotalCost;
                //////        }
                //////    };
                //////}
            }
            _presenter.CurrentBidAnalysisRequest.TotalPrice = cost;
            txtTotal.Text = _presenter.CurrentBidAnalysisRequest.TotalPrice.ToString();
            pnlBidItem.Visible = false;
        }
        private void BindSupplier(DropDownList ddlSupplier, int SupplierTypeId)
        {
            if (ddlSupplier.Items.Count > 0)
            {
                ddlSupplier.Items.Clear();
            }
            ddlSupplier.DataSource = _presenter.GetSuppliers(SupplierTypeId);
            ddlSupplier.DataBind();
        }
        private void BindSupplierType(DropDownList ddlSupplierType)
        {
            ddlSupplierType.DataSource = _presenter.GetSupplierTypes();
            ddlSupplierType.DataBind();
        }


        private void BindItemdetailGrid(BidderItemDetail Tad)
        {
            bidditem = Session["bidditem"] as BidderItemDetail;
            dgBidders.DataSource = bidditem.Bidders;
            dgBidders.DataBind();
        }

        private void BindBidder(BidderItemDetail Bid)
        {
            bidditem = Session["bidditem"] as BidderItemDetail;
            dgBidders.DataSource = bidditem.Bidders;
            dgBidders.DataBind();
           
        }
        protected void dgBidders_SelectedIndexChanged(object sender, EventArgs e)
        {

            //int BidderId = (int)dgBidders.DataKeys[dgBidders.SelectedItem.ItemIndex];
            //int Id = dgBidders.SelectedItem.ItemIndex;


            //if (BidderId > 0)
            //    Session["bidd"] = _presenter.CurrentBidAnalysisRequest.GetBidder(BidderId);

            //else
            //    Session["bidd"] = _presenter.CurrentBidAnalysisRequest.Bidders[dgBidders.SelectedItem.ItemIndex];


            //int recordId = (int)dgBidders.SelectedIndex;
            //if (_presenter.CurrentBidAnalysisRequest.Id > 0)
            //{
            //    hfDetailId.Value = BidderId.ToString();
            //}
            //else
            //{
            //    hfDetailId.Value = dgBidders.SelectedItem.ItemIndex.ToString();
            //}


            //BindBidItem(bidd);
            //pnlBidItem_ModalPopupExtender.Show();
        }
        #endregion
        #region Bidders
        protected void btnAddItemdetail_Click(object sender, EventArgs e)
        {
            // SetBidderItemDetail();
        }
        private void BindBidders()
        {
           
            dgBidders.DataSource = _presenter.CurrentBidAnalysisRequest.BidderItemDetails[0].Bidders;
            dgBidders.DataBind();
        }

        /*   private void SetBidderItemDetail()
            {


                bidditem = Session["bidditem"] as Bidder;
                int index = 0;
                foreach (DataGridItem dgi in dgBidders.Items)
                {
                    int id = (int)dgBidders.DataKeys[dgi.ItemIndex];

                    Bidder detail;
                    if (id > 0)
                    {
                        detail = bidditem.GetTBidder(id);
                    }
                    else
                    {
                        detail = (Bidder)bidditem.Bidders[index];
                    }


                    TextBox txtQty = dgi.FindControl("txtQty") as TextBox;
                    detail.Qty = Convert.ToInt32(txtQty.Text);
                    TextBox txtUnitCost = dgi.FindControl("txtUnitCost") as TextBox;
                    detail.UnitCost = Convert.ToDecimal(txtUnitCost.Text);
                    TextBox txtTotalCost = dgi.FindControl("txtTotalCost") as TextBox;
                    detail.TotalCost = Convert.ToDecimal(txtTotalCost.Text);
                    bidd.BidderItemDetails.Add(detail);
                    index++;


                }
                Master.ShowMessage(new AppMessage("Bidder Items successfully saved!", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            */

        #endregion
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
        protected void ddlFSupplierType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindBidItem();
            DropDownList ddl = (DropDownList)sender;
            if (ddl.SelectedValue != "0")
            {
                pnlBidItem_ModalPopupExtender.Show();
                //BindBidItem(bidditem);

                // ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#pnlBidItem_ModalPopupExtender').modal('show')", true);
                //DropDownList ddl = (DropDownList)sender;
                DropDownList ddlFSupplier = ddl.FindControl("ddlFSupplier") as DropDownList;
                BindSupplier(ddlFSupplier, Convert.ToInt32(ddl.SelectedValue));
            }
        }
        protected void ddlSupplierType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindBidItem();
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlSupplier = ddl.FindControl("ddlSupplier") as DropDownList;
            BindSupplier(ddlSupplier, Convert.ToInt32(ddl.SelectedValue));
            
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            _presenter.DeleteBidAnalysisRequest(_presenter.CurrentBidAnalysisRequest);

            BindBidAnalysisRequests();
            // btnDelete.Enabled = false;
            Master.ShowMessage(new AppMessage("Bid Analysis Request Successfully Deleted", Chai.WorkflowManagment.Enums.RMessageType.Info));
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindBidAnalysisRequestsList();
            //pnlSearch_ModalPopupExtender.Show();
            ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            pnlWarning.Visible = false;
            _presenter.CancelPage();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmBidAnalysisRequest.aspx");
        }
        /*   protected void DeleteFile(object sender, EventArgs e)
           {
               string filePath = (sender as LinkButton).CommandArgument;
               _presenter.CurrentBidAnalysisRequest.ELRAttachments.Removet(filePath);
               File.Delete(Server.MapPath(filePath));
               grvAttachments.DataSource = _presenter.CurrentPurchaseRequest.BidAnalysises.BAAttachments;
               grvAttachments.DataBind();
               //Response.Redirect(Request.Url.AbsoluteUri);


           }*/
        /*   private void UploadFile()
           {
               string fileName = Path.GetFileName(fuReciept.PostedFile.FileName);

               if (fileName != String.Empty)
               {



                   BAAttachment attachment = new BAAttachment();
                   attachment.FilePath = "~/BAUploads/" + fileName;
                   fuReciept.PostedFile.SaveAs(Server.MapPath("~/BAUploads/") + fileName);
                   //Response.Redirect(Request.Url.AbsoluteUri);
                   _presenter.CurrentBidAnalysisRequest.ELRAttachments.Add(attachment);

                   grvAttachments.DataSource = _presenter.CurrentBidAnalysisRequest.ELRAttachments;
                   grvAttachments.DataBind();


               }
               else
               {
                   Master.ShowMessage(new AppMessage("Please select file ", Chai.WorkflowManagment.Enums.RMessageType.Error));
               }
           }*/
        /*     private void BindAttachments()
             {
                 if (_presenter.CurrentPurchaseRequest.BidAnalysises.Id > 0)
                 {
                     grvAttachments.DataSource = _presenter.CurrentPurchaseRequest.BidAnalysises.BAAttachments;
                     grvAttachments.DataBind();
                 }
             }*/
        #endregion

        private void PopPurchaseRequest()
        {
          
            grvDetails.DataSource = _presenter.ListPurchaseReqInProgress();
            grvDetails.DataBind();
        }

      

        private void BindBidAnalysisRequestsList()
        {
            grvBidAnalysisRequestList.DataSource = _presenter.ListBidAnalysisRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text);
            grvBidAnalysisRequestList.DataBind();
        }

        private string AutoNumber()
        {
            return "BAR-" + (_presenter.GetLastBidAnalysisRequestId() + 1).ToString();
        }
        private void CheckApprovalSettings()
        {

            if (_presenter.GetApprovalSetting(RequestType.Bid_Analysis_Request.ToString().Replace('_', ' '), 0) == null)
            {
                pnlWarning.Visible = true;
            }
        }
        private void BindBidAnalysisRequestFields()
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentBidAnalysisRequest != null)
            {
                txtRequester.Text = _presenter.CurrentBidAnalysisRequest.AppUser.UserName.ToString();
                // txtRequestDate.Text = _presenter.CurrentBidAnalysisRequest.RequestDate.Value.ToShortDateString();
              
                //  txtselectionfor.Text = _presenter.CurrentBidAnalysisRequest.Neededfor;
              
                txtselectionfor.Text = _presenter.CurrentBidAnalysisRequest.ReasonforSelection;
                txtTotal.Text = Convert.ToDecimal(_presenter.CurrentBidAnalysisRequest.TotalPrice).ToString();
                
                BindBidAnalysisRequests();
                btnPrintworksheet.Enabled = true;
                PrintTransaction();
            }
        }
        protected void dgBidders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //LinkButton db = (LinkButton)e.Row.Cells[5].Controls[0];
                //db.OnClientClick = "return confirm('Are you sure you want to delete this Recieve?');";
            }
        }
        protected void DeleteFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            _presenter.CurrentBidAnalysisRequest.RemoveBAAttachment(filePath);
            File.Delete(Server.MapPath(filePath));
            grvAttachments.DataSource = _presenter.CurrentBidAnalysisRequest.BAAttachments;
            grvAttachments.DataBind();
            //Response.Redirect(Request.Url.AbsoluteUri);


        }
        #region Bidders
        private void SaveBidAnalysis()
        {

            try
            {
                _presenter.CurrentBidAnalysisRequest.PurchaseRequest.Id = Convert.ToInt32(GetPurchaseRequestId);
              //   _presenter.CurrentBidAnalysisRequest.Neededfor = txtselectionfor.Text;
                
                _presenter.CurrentBidAnalysisRequest.ReasonforSelection = txtselectionfor.Text;

                //foreach (Bidder bider in _presenter.CurrentBidAnalysisRequest.Bidders)
                //{
                //    foreach (BidderItemDetail detail in bider.BidderItemDetails)
                //    {
                //        totalamaount = totalamaount + detail.TotalCost;
                //    }
                //    totalamaount = Convert.ToDecimal(txtTotal.Text);
                //}
                _presenter.CurrentBidAnalysisRequest.TotalPrice = totalamaount;
                _presenter.CurrentBidAnalysisRequest.SelectedBy = _presenter.CurrentUser().Id;
                if (_presenter.CurrentBidAnalysisRequest.Supplier != null)
                    _presenter.CurrentBidAnalysisRequest.Supplier = _presenter.CurrentBidAnalysisRequest.Supplier;



                _presenter.CurrentBidAnalysisRequest.Status = "Completed";

            }
            catch (Exception ex)
            {


            }

        }

        protected void dgBidders_CancelCommand(object source, DataGridCommandEventArgs e)
        {
           
        }

        protected void dgBidders_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            bidditem = Session["bidditem"] as BidderItemDetail;
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlFSupplierType = e.Item.FindControl("ddlFSupplierType") as DropDownList;
                BindSupplierType(ddlFSupplierType);
                DropDownList ddlFSupplier = e.Item.FindControl("ddlFSupplier") as DropDownList;
                BindSupplier(ddlFSupplier, int.Parse(ddlFSupplierType.SelectedValue));
                //DropDownList ddlFItemAcc = e.Item.FindControl("ddlFItemAcc") as DropDownList;
                //BindItems(ddlFItemAcc);
            }
            else
            {
                if (bidditem.Bidders != null)
                {
                    DropDownList ddlEdtSuppllierType = e.Item.FindControl("ddlSupplierType") as DropDownList;
                    if (ddlEdtSuppllierType != null)
                    {
                        BindSupplierType(ddlEdtSuppllierType);
                        if (_presenter.CurrentBidAnalysisRequest.GetBidderItemDetail(Convert.ToInt32(hfDetailId.Value)).Bidders[e.Item.DataSetIndex].SupplierType.Id != 0)
                        {
                            ListItem liI = ddlEdtSuppllierType.Items.FindByValue(_presenter.CurrentBidAnalysisRequest.GetBidderItemDetail(Convert.ToInt32(hfDetailId.Value)).Bidders[e.Item.DataSetIndex].SupplierType.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }
                    }
                    DropDownList ddlSupplier = e.Item.FindControl("ddlSupplier") as DropDownList;
                    if (ddlSupplier != null)
                    {
                        BindSupplier(ddlSupplier, int.Parse(ddlEdtSuppllierType.SelectedValue));
                        if (_presenter.CurrentBidAnalysisRequest.GetBidderItemDetail(Convert.ToInt32(hfDetailId.Value)).Bidders[e.Item.DataSetIndex].Supplier != null)
                        {
                            if (_presenter.CurrentBidAnalysisRequest.GetBidderItemDetail(Convert.ToInt32(hfDetailId.Value)).Bidders[e.Item.DataSetIndex].Supplier.Id != 0)
                            {
                                ListItem liI = ddlSupplier.Items.FindByValue(_presenter.CurrentBidAnalysisRequest.GetBidderItemDetail(Convert.ToInt32(hfDetailId.Value)).Bidders[e.Item.DataSetIndex].Supplier.Id.ToString());
                                if (liI != null)
                                    liI.Selected = true;
                            }
                        }
                    }

                }
            }

        
        }




        private void UploadFile()
        {
            string fileName = Path.GetFileName(fuReciept.PostedFile.FileName);
            try
            {
                if (fileName != String.Empty)
                {



                    BAAttachment attachment = new BAAttachment();
                    attachment.FilePath = "~/BAUploads/" + fileName;
                    fuReciept.PostedFile.SaveAs(Server.MapPath("~/BAUploads/") + fileName);
                    //Response.Redirect(Request.Url.AbsoluteUri);
                    _presenter.CurrentBidAnalysisRequest.BAAttachments.Add(attachment);

                    grvAttachments.DataSource = _presenter.CurrentBidAnalysisRequest.BAAttachments;
                    grvAttachments.DataBind();


                }
                else
                {
                    Master.ShowMessage(new AppMessage("Please select file ", Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
            catch (HttpException ex)
            {
                Master.ShowMessage(new AppMessage("Unable to upload the file,The file is to big or The internet is too slow " + ex.InnerException.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }



















        private void BindBidRequestforprint()
        {

            /*
           lblrequestNo.Text = _presenter.CurrentBidAnalysisRequest.RequestNo;

             lblRequester.Text = _presenter.GetUser(_presenter.CurrentBidAnalysisRequest.AppUser.Id).FullName;
             lblRequestDate0.Text = _presenter.CurrentBidAnalysisRequest.RequestDate.ToString();
             // lblApprovedBy.Text = _presenter.CurrentBidAnalysisRequest..ContactPersonNumber;
             lblSpecialNeed.Text = _presenter.CurrentBidAnalysisRequest.SpecialNeed;

             lblEstimatedTotalCost.Text = _presenter.CurrentBidAnalysisRequest.TotalPrice;
             lblselectedSupplier.Text = _presenter.CurrentBidAnalysisRequest.Supplier;
             lblReasonforsel.Text = _presenter.CurrentBidAnalysisRequest.ReasonforSelection;
             lblSelectedBy.Text = _presenter.CurrentBidAnalysisRequest.SelectedBy;

             */



        }




        #endregion
        protected void dgBidders_SelectedIndexChanged1(object sender, EventArgs e)
        {

            //int BidderId = (int)dgBidders.DataKeys[dgBidders.SelectedItem.ItemIndex];
            //int Id = dgBidders.SelectedItem.ItemIndex;


            //if (BidderId > 0)
            //    Session["bidditem"] = _presenter.CurrentBidAnalysisRequest.GetBidderItemDetail(BidderId);

            //else
            //    Session["bidditem"] = _presenter.CurrentBidAnalysisRequest.BidderItemDetails[dgBidders.SelectedItem.ItemIndex];


            //int recordId = (int)dgBidders.SelectedIndex;
            //if (_presenter.CurrentBidAnalysisRequest.Id > 0)
            //{
            //    hfDetailId.Value = BidderId.ToString();
            //}
            //else
            //{
            //    hfDetailId.Value = dgBidders.SelectedItem.ItemIndex.ToString();
            //}

            //// BindBidItemDetails();
            //BindBidItem(bidditem);
            //pnlBidItem_ModalPopupExtender.Show();
        }
        protected void txtUnitCost_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            TextBox hfQty = txt.FindControl("txtQty") as TextBox;
            TextBox txtUnitCost = txt.FindControl("txtUnitCost") as TextBox;
            TextBox txtTot = txt.FindControl("txtTotalCost") as TextBox;
            txtTot.Text = ((Convert.ToInt32(hfQty.Text) * Convert.ToDecimal(txtUnitCost.Text))).ToString();

        }

        private void BindItems(DropDownList ddlItems)
        {
            ddlItems.DataSource = _presenter.GetItemAccounts();
            ddlItems.DataBind();
        }
        private void PopBidAnalysisRequesters()
        {
            PurchaseRequest purchaseRequest = Session["PR"] as PurchaseRequest;
            if (_presenter.CurrentBidAnalysisRequest != null && purchaseRequest != null)
            {

                txtRequester.Text = _presenter.CurrentUser().FirstName + " " + _presenter.CurrentUser().LastName;

                txtRequestDate.Text = purchaseRequest.RequestedDate.ToShortDateString();

             

               
                if (_presenter.CurrentBidAnalysisRequest.TotalPrice != 0)
                {
                    txtTotal.Text = _presenter.CurrentBidAnalysisRequest.TotalPrice.ToString();
                }
                //ddlGrant.DataSource = _presenter.GetGrantbyprojectId(Convert.ToInt32(ddlProject.SelectedValue));
                //ddlGrant.DataBind();
                //ddlGrant.SelectedValue = purchaseRequest.PurchaseRequestDetails[0].Grant.Id.ToString();

                //    GridView1.DataSource = _presenter.CurrentBidAnalysisRequest.PurchaseRequest.PurchaseRequestDetails;
                //  GridView1.DataBind();
            }
            else if(purchaseRequest==null && _presenter.CurrentBidAnalysisRequest != null)
            {
                PopPurchaseRequest();
            }



        }

        private void PopPurchaseRequestsDropDown()
        {
            ddlPurchaseReq.DataSource = _presenter.GetPurchaseRequestListInProgress();
            ddlPurchaseReq.DataBind();

            ddlPurchaseReq.Items.Insert(0, new ListItem("---Select Purchase Request---", ""));
            ddlPurchaseReq.SelectedIndex = 0;
        }
       
        protected void dgBidders_ItemDataBound1(object sender, DataGridItemEventArgs e)
        {

         
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlFSupplierType = e.Item.FindControl("ddlFSupplierType") as DropDownList;
                BindSupplierType(ddlFSupplierType);
                DropDownList ddlFSupplier = e.Item.FindControl("ddlFSupplier") as DropDownList;
                BindSupplier(ddlFSupplier, int.Parse(ddlFSupplierType.SelectedValue));
                //DropDownList ddlFItemAcc = e.Item.FindControl("ddlFItemAcc") as DropDownList;
                //BindItems(ddlFItemAcc);
            }
            else
            {

                if (_presenter.CurrentBidAnalysisRequest.BidderItemDetails != null)
                {
                    DropDownList ddlEdtSuppllierType = e.Item.FindControl("ddlSupplierType") as DropDownList;
                    if (ddlEdtSuppllierType != null)
                    {
                        BindSupplierType(ddlEdtSuppllierType);
                        if (_presenter.CurrentBidAnalysisRequest.GetBidderItemDetail(Convert.ToInt32(hfDetailId.Value)).Bidders[e.Item.DataSetIndex].SupplierType.Id != 0)
                        {
                            ListItem liI = ddlEdtSuppllierType.Items.FindByValue(_presenter.CurrentBidAnalysisRequest.GetBidderItemDetail(Convert.ToInt32(hfDetailId.Value)).Bidders[e.Item.DataSetIndex].SupplierType.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }
                    }
                    DropDownList ddlSupplier = e.Item.FindControl("ddlSupplier") as DropDownList;
                    if (ddlSupplier != null)
                    {
                        BindSupplier(ddlSupplier, int.Parse(ddlEdtSuppllierType.SelectedValue));
                        if (_presenter.CurrentBidAnalysisRequest.GetBidderItemDetail(Convert.ToInt32(hfDetailId.Value)).Bidders[e.Item.DataSetIndex].Supplier != null)
                        {
                            if (_presenter.CurrentBidAnalysisRequest.GetBidderItemDetail(Convert.ToInt32(hfDetailId.Value)).Bidders[e.Item.DataSetIndex].Supplier.Id != 0)
                            {
                                ListItem liI = ddlSupplier.Items.FindByValue(_presenter.CurrentBidAnalysisRequest.GetBidderItemDetail(Convert.ToInt32(hfDetailId.Value)).Bidders[e.Item.DataSetIndex].Supplier.Id.ToString());
                                if (liI != null)
                                    liI.Selected = true;
                            }
                        }
                    }

                }
            }
            

        }

        protected void dgBidders_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            
            bidditem = Session["bidditem"] as BidderItemDetail;
            int id = (int)dgBidders.DataKeys[e.Item.ItemIndex];

            Chai.WorkflowManagment.CoreDomain.Requests.Bidder bidder = bidditem.GetTBidder(id);
            try
            {
                DropDownList ddlSupplierType = e.Item.FindControl("ddlSupplierType") as DropDownList;
                bidder.SupplierType = _presenter.GetSupplierType(Convert.ToInt32(ddlSupplierType.SelectedValue));
                DropDownList ddlSupplier = e.Item.FindControl("ddlSupplier") as DropDownList;
                bidder.Supplier = _presenter.GetSupplier(Convert.ToInt32(ddlSupplier.SelectedValue));
                TextBox txtFLeadTimefromSupplier = e.Item.FindControl("txtLeadTimefromSupplier") as TextBox;
                bidder.LeadTimefromSupplier = txtFLeadTimefromSupplier.Text;
                TextBox txtContactDetails = e.Item.FindControl("txtContactDetails") as TextBox;
                bidder.ContactDetails = txtContactDetails.Text;
                TextBox txtSpecialTermsDelivery = e.Item.FindControl("txtSpecialTermsDelivery") as TextBox;
                bidder.SpecialTermsDelivery = txtSpecialTermsDelivery.Text;
                TextBox txtQty = e.Item.FindControl("txtEdtQty") as TextBox;
                bidder.Qty = Convert.ToInt32(txtQty.Text);
                TextBox txtUnitCost = e.Item.FindControl("txtEdtUnitCost") as TextBox;
                bidder.UnitCost = Convert.ToDecimal(txtUnitCost.Text);
                bidder.TotalCost = Convert.ToInt32(txtQty.Text) * Convert.ToDecimal(txtUnitCost.Text);
                TextBox txtFRank = e.Item.FindControl("txtRank") as TextBox;
                bidder.Rank = Convert.ToInt32(txtFRank.Text);

                dgBidders.EditItemIndex = -1;
                BindBidders();
                pnlBidItem_ModalPopupExtender.Show();
                if (_presenter.CurrentBidAnalysisRequest.BidderItemDetails.Count > 0)
                {

                    foreach (BidderItemDetail detail in _presenter.CurrentBidAnalysisRequest.BidderItemDetails)
                    {

                        foreach (Bidder bidderdetail in detail.Bidders)
                        {
                            if (bidderdetail.Rank == 1)
                            {
                                totalamaount = totalamaount + bidderdetail.TotalCost;
                            }
                        }

                    }
                }


                txtTotal.Text = totalamaount.ToString();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Bidder " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }




        }
        protected void dgBidders_CancelCommand1(object source, DataGridCommandEventArgs e)
        {

        }
        protected void dgBidders_DeleteCommand1(object source, DataGridCommandEventArgs e)
        {
            bidditem = Session["bidderitem"] as BidderItemDetail;

         
            int id = (int)dgBidders.DataKeys[e.Item.ItemIndex];
            int BIDid = (int)dgBidders.DataKeys[e.Item.ItemIndex];
            Bidder bidd;

            if (BIDid > 0)
                bidd = _presenter.GetBidder(BIDid);
            else
                bidd = (Bidder)bidditem.Bidders[e.Item.ItemIndex];

            try
            {
                if (BIDid > 0)
                {
                    _presenter.CurrentBidAnalysisRequest.GetBidderItemDetail(Convert.ToInt32(hfDetailId.Value)).RemoveBidder(id);
                    if (_presenter.GetBidder(id) != null)
                        _presenter.DeleteBidder(_presenter.GetBidder(id));
                    _presenter.CurrentBidAnalysisRequest.TotalPrice = _presenter.CurrentBidAnalysisRequest.TotalPrice - bidd.TotalCost;
                    txtTotal.Text = _presenter.CurrentBidAnalysisRequest.TotalPrice.ToString();
                    _presenter.SaveOrUpdateBidAnalysisRequest(id);
                }
                else
                {

                    _presenter.CurrentBidAnalysisRequest.GetBidderItemDetail(Convert.ToInt32(hfDetailId.Value)).Bidders.Remove(bidd);
                    _presenter.CurrentBidAnalysisRequest.TotalPrice = _presenter.CurrentBidAnalysisRequest.TotalPrice - bidd.TotalCost;
                    txtTotal.Text = _presenter.CurrentBidAnalysisRequest.TotalPrice.ToString();
                }
                BindBidder(bidd.BidderItemDetail);
                pnlBidItem_ModalPopupExtender.Show();
                Master.ShowMessage(new AppMessage("Bidder was removed successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Bidder. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgBidders_ItemCommand1(object source, DataGridCommandEventArgs e)
        {
            bidditem = Session["bidditem"] as BidderItemDetail;
            if (e.CommandName == "AddNew")
            {
                try
                {
                    Bidder bidder = new Bidder();
                    bidder.BidderItemDetail = bidditem;
                    DropDownList ddlSupplierType = e.Item.FindControl("ddlFSupplierType") as DropDownList;
                    bidder.SupplierType = _presenter.GetSupplierType(Convert.ToInt32(ddlSupplierType.SelectedValue));
                    DropDownList ddlSupplier = e.Item.FindControl("ddlFSupplier") as DropDownList;
                    bidder.Supplier = _presenter.GetSupplier(Convert.ToInt32(ddlSupplier.SelectedValue));
                    TextBox txtFLeadTimefromSupplier = e.Item.FindControl("txtFLeadTimefromSupplier") as TextBox;
                    bidder.LeadTimefromSupplier = txtFLeadTimefromSupplier.Text;
                    TextBox txtFContactDetails = e.Item.FindControl("txtFContactDetails") as TextBox;
                    bidder.ContactDetails = txtFContactDetails.Text;
                    TextBox txtFSpecialTermsDelivery = e.Item.FindControl("txtFSpecialTermsDeliveryy") as TextBox;
                    bidder.SpecialTermsDelivery = txtFSpecialTermsDelivery.Text;
                    TextBox txtQty = e.Item.FindControl("txtQty") as TextBox;
                    bidder.Qty = Convert.ToInt32(txtQty.Text);
                    TextBox txtUnitCost = e.Item.FindControl("txtUnitCost") as TextBox;
                    bidder.UnitCost = Convert.ToDecimal(txtUnitCost.Text);


                    bidder.TotalCost = Convert.ToInt32(txtQty.Text) * Convert.ToDecimal(txtUnitCost.Text);
                    TextBox txtFRank = e.Item.FindControl("txtFRank") as TextBox;
                    bidder.Rank = Convert.ToInt32(txtFRank.Text);
                    _presenter.CurrentBidAnalysisRequest.BidderItemDetails[0].Bidders.Add(bidder);
                    

                    dgItemDetail.EditItemIndex = -1;

                    BindItemdetailGrid(bidder.BidderItemDetail);

                    pnlBidItem_ModalPopupExtender.Show();



                    if (_presenter.CurrentBidAnalysisRequest.BidderItemDetails.Count > 0)
                    {

                        foreach (BidderItemDetail detail in _presenter.CurrentBidAnalysisRequest.BidderItemDetails)
                        {

                            foreach (Bidder bidderdetail in detail.Bidders)
                            {
                                if (bidderdetail.Rank == 1)
                                {
                                    totalamaount = totalamaount + bidderdetail.TotalCost;
                                }
                            }

                        }
                    }


                    txtTotal.Text = totalamaount.ToString();



                    Master.ShowMessage(new AppMessage("Bidder Item Successfully Added", Chai.WorkflowManagment.Enums.RMessageType.Info));
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add BidderItem " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }


                //  _presenter.CurrentBidAnalysisRequest.Bidders.Add(bidder);
                //    dgBidders.EditItemIndex = -1;
                //    BindBidder();
                //}
                //catch (Exception ex)
                //{
                //    Master.ShowMessage(new AppMessage("Error: Unable to Add Bidder " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                //}
            }
        }

        protected void dgItemDetail_CancelCommand(object source, DataGridCommandEventArgs e)
        {

        }


        private void BindBidItemDetails()
        {
            dgItemDetail.DataSource = _presenter.CurrentBidAnalysisRequest.BidderItemDetails;
            dgItemDetail.DataBind();
        }
        private void BindBidItem()
        {

            //bidditem = Session["bidditem"] as BidderItemDetail;

            dgItemDetail.DataSource =_presenter.CurrentBidAnalysisRequest.BidderItemDetails;
            dgItemDetail.DataBind();
            
        }
      
       


        protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            foreach (PurchaseRequestDetail prd in _presenter.CurrentBidAnalysisRequest.PurchaseRequest.PurchaseRequestDetails)
            {



                Label lbl = e.Item.FindControl("lblName") as Label;
                prd.ItemAccount.AccountName = lbl.Text;
                Label lbl1 = e.Item.FindControl("lblName1") as Label;
                prd.AccountCode = lbl1.Text;
                Label lbl2 = e.Item.FindControl("lblName2") as Label;
                prd.EstimatedCost = Convert.ToDecimal(lbl2.Text);



            }
        }

        protected void grvBidAnlysisRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {

            //ClearForm();
            BindBidAnalysisRequestFields();
            grvAttachments.DataSource = _presenter.CurrentBidAnalysisRequest.BAAttachments;
            grvAttachments.DataBind();

            if (_presenter.CurrentBidAnalysisRequest.CurrentStatus != null)
            {
                btnSave.Visible = false;

            }
            else
            {
                btnSave.Visible = true;

            }
        }


        private void PrintTransaction()
        {
            lblRequester.Text = _presenter.CurrentBidAnalysisRequest.AppUser.UserName.ToString();
            lblRequestDate0.Text = _presenter.CurrentBidAnalysisRequest.RequestDate.ToString();
            lblSpecialNeed.Text = _presenter.CurrentBidAnalysisRequest.SpecialNeed;


            lblTot.Text = _presenter.CurrentBidAnalysisRequest.TotalPrice.ToString();

            txtselectionfor.Text = _presenter.CurrentBidAnalysisRequest.ReasonforSelection;
            lblReasonForSelection.Text = _presenter.CurrentBidAnalysisRequest.ReasonforSelection;

           // grvprtBidderItemDetails.DataSource = _presenter.CurrentBidAnalysisRequest.BidderItemDetails;
           // grvprtBidderItemDetails.DataBind();

            grvStatuses.DataSource = _presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses;
            grvStatuses.DataBind();


            foreach (BidderItemDetail detail in _presenter.CurrentBidAnalysisRequest.BidderItemDetails)
            {
                grvprtBidders.DataSource = detail.Bidders;
                grvprtBidders.DataBind();
            }
        }
        protected void grvBidAnalysisRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvBidAnalysisRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }

        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses[e.Row.RowIndex].Approver != 0)
                        e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses[e.Row.RowIndex].Approver).FullName;
                }
            }
        }

        protected void btncancelCost_Click(object sender, EventArgs e)
        {

        }

        protected void dgItemDetail_EditCommand1(object source, DataGridCommandEventArgs e)
        {

            this.dgItemDetail.EditItemIndex = e.Item.ItemIndex;
            BindBidItemDetails();
        }

       

        protected void dgItemDetail_SelectedIndexChanged(object sender, EventArgs e)
        {

           // int PuID = Convert.ToInt32(Session["prId"]);
            int ITEMId = (int)dgItemDetail.DataKeys[dgItemDetail.SelectedItem.ItemIndex];
            int Id = dgItemDetail.SelectedItem.ItemIndex;


            if (ITEMId > 0)
                Session["bidditem"] = _presenter.CurrentBidAnalysisRequest.GetBidderItemDetail(ITEMId);
            else
                Session["bidditem"] = _presenter.CurrentBidAnalysisRequest.BidderItemDetails[dgItemDetail.SelectedItem.ItemIndex];



            int recordId = (int)dgItemDetail.SelectedIndex;
            if (_presenter.CurrentBidAnalysisRequest.Id > 0)
            {
                hfDetailId.Value = ITEMId.ToString();
            }
            else
            {
                hfDetailId.Value = dgItemDetail.SelectedItem.ItemIndex.ToString();
            }
            // Session["bidditem"] = Convert.ToInt32((int)dgItemDetail.DataKeys[dgItemDetail.SelectedItem.ItemIndex]);
            //BidderItemDetail detail = Session["bidditem"] as BidderItemDetail;
            BindBidders();
            //BindBidItem(bidditem);
            pnlBidItem_ModalPopupExtender.Show();
        }
        
       
        protected void btnSave_Click(object sender, EventArgs e)
        {





            try
            {
                
                if (_presenter.CurrentBidAnalysisRequest.BidderItemDetails.Count != 0)
                {

                    int PRID = Convert.ToInt32(Session["PRID"]);
                 
                    _presenter.SaveOrUpdateBidAnalysisRequest(PRID);
                    BindBidAnalysisRequests();
                    Master.ShowMessage(new AppMessage("Successfully did a Bid Analysis  Request, Reference No - <b>'" + _presenter.CurrentBidAnalysisRequest.RequestNo + "'</b>", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    Log.Info(_presenter.CurrentUser().FullName + " has requested a For a Bid Analyis");
                    btnSave.Visible = false;
                    btnPrintworksheet.Enabled = true;
                    PrintTransaction();
                    
                }
                              
                else
                {
                    Master.ShowMessage(new AppMessage("Please Attach Bid Analysis Quotation", Chai.WorkflowManagment.Enums.RMessageType.Error));
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

        protected void ddlPurchaseReq_SelectedIndexChanged(object sender, EventArgs e)
        {
            // PopPurchaseRequest();
            grvDetails.DataSource = _presenter.ListPurchaseReqInProgressbyId(Convert.ToInt32(ddlPurchaseReq.SelectedValue));
            grvDetails.DataBind();


        }

        protected void grvDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            prId = Convert.ToInt32(grvDetails.SelectedDataKey[0]);
            Session["prId"] = Convert.ToInt32(grvDetails.SelectedDataKey[0]);
            _presenter.OnViewLoaded();
            btnSave.Visible = true;
            grvDetails.Visible = false;
            pnlInfo.Visible = false;
            int PuID = Convert.ToInt32(Session["prId"]);
           
            //Session["prId"] = _presenter.CurrentBidAnalysisRequest.PurchaseRequest;


            if (_presenter.GetPurchaseRequestbyPuID(PuID) != null)
            {
                PurchaseRequestDetail PD = _presenter.GetPurchaseRequestbyPuID(PuID);
                
                    BidderItemDetail detail = new BidderItemDetail();
                    detail.ItemAccount = PD.ItemAccount;
                    detail.ItemDescription = PD.Item;
                    detail.Qty = PD.Qty;
                    detail.Project= PD.Project;
                    detail.Grant = PD.Grant;
                  
                    _presenter.CurrentBidAnalysisRequest.BidderItemDetails.Add(detail);
                    txtRequester.Text = _presenter.CurrentUser().FirstName + " " + _presenter.CurrentUser().LastName;
                    lblPurchaseReqNo.Text = PD.PurchaseRequest.RequestNo;
                    int userid = _presenter.GetPurchaseRequestbyPuID(PuID).PurchaseRequest.Requester;
                    lblPurReqRequester.Text = _presenter.GetUser(userid).FullName;
                    ddlProject.Text = PD.Project.ProjectCode;
                    ddlGrant.Text = PD.Grant.GrantCode;
                    lblRequestedDate.Text = PD.PurchaseRequest.RequestedDate.ToShortDateString();
                   
                

            }
            //dgItemDetail.DataSource = _presenter.CurrentBidAnalysisRequest.BidderItemDetails;
            //dgItemDetail.DataBind();
            //Response.Redirect(String.Format("../Request/frmBidAnalysisRequest.aspx?PurchaseRequestId={0}", prId));

            dgItemDetail.DataSource = _presenter.CurrentBidAnalysisRequest.BidderItemDetails;
            dgItemDetail.DataBind();
            //grvDetails.DataSource = _presenter.ListPurchaseReqbyId(Convert.ToInt32(PuID));
            //grvDetails.DataBind();
            PopPurchaseRequest();
        }





        protected void dgBidders_EditCommand(object source, DataGridCommandEventArgs e)
        {
            bidditem = Session["bidditem"] as BidderItemDetail;
            this.dgBidders.EditItemIndex = e.Item.ItemIndex;
            int BiddId = (int)dgBidders.DataKeys[e.Item.ItemIndex];
            Bidder bidd;

            if (BiddId > 0)
                bidd = _presenter.GetBidder(BiddId);
            else
                bidd = (Bidder)bidditem.Bidders[e.Item.ItemIndex];
            BindBidder(bidd.BidderItemDetail);
            pnlBidItem_ModalPopupExtender.Show();
        }

        protected void grvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvDetails.PageIndex = e.NewPageIndex;
            BindBidItemDetails();
            
        }

      
    }
}