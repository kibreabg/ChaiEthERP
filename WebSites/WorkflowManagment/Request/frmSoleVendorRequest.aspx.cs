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

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public partial class frmSoleVendorRequest : POCBasePage, ISoleVendorRequestView
    {
        private SoleVendorRequestPresenter _presenter;
        private IList<SoleVendorRequest> _SoleVendorRequests;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        int prId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {

                Session["PR"] = _presenter.GetPurchaseRequestList();
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                CheckApprovalSettings();
                PurchaseRequest purchaseRequest = _presenter.GetPurchaseRequest(GetPurchaseRequestId);
                _presenter.CurrentSoleVendorRequest.PurchaseRequest = purchaseRequest;
                //PopProjects();
                PopPurchaseRequestsDropDown();
                //BindSoleVendorRequests();
                //PopPurchaseRequest();
                PopSoleVendorRequesters();
               

               // BindSoleVendorRequestDetails();
                PopSupplier();
                if (_presenter.CurrentSoleVendorRequest.Id <= 0)
                {
                    AutoNumber();
                    btnDelete.Visible = false;
                }
            }
            txtRequestDate.Text = DateTime.Today.Date.ToShortDateString();
            this._presenter.OnViewLoaded();

            BindSoleVendorRequests();
            PopPurchaseRequest();
            if (_presenter.CurrentSoleVendorRequest != null)
            {

                if (_presenter.CurrentSoleVendorRequest.Id != 0)
                {
                    BindSoleVendorRequestDetails();
                }



            }

            //if (_presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails.Count > 0)
            //{
            //    BindSoleVendorRequestDetails();
            //}

        }
        [CreateNew]
        public SoleVendorRequestPresenter Presenter
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
        public int GetSoleVendorRequestId
        {
            get
            {
                if (Convert.ToInt32(Request.QueryString["SoleVendorRequestId"]) != 0)
                {
                    return Convert.ToInt32(Request.QueryString["SoleVendorRequestId"]);
                }
                else if (grvSoleVendorRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvSoleVendorRequestList.SelectedDataKey.Value);
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
        public string GetRequestNo
        {
            get { return AutoNumber(); }
        }
        public DateTime GetRequestDate
        {
            get { return Convert.ToDateTime(txtRequestDate.Text); }
        }
       
        public string GetReasonForSelection
        {
            get { return txtselectionfor.Text; }
        }
        public int GetProjectId
        {
            get { return Convert.ToInt32(ddlProject.SelectedValue); }
        }
        public int GetGrantId
        {
            get { return Convert.ToInt32(ddlGrant.SelectedValue); }
        }


        public IList<SoleVendorRequest> SoleVendorRequests
        {
            get
            {
                return _SoleVendorRequests;
            }
            set
            {
                _SoleVendorRequests = value;
            }
        }

        public string GetContactPersonNumber
        {
            get
            {
                return txtContactPersonNumber.Text;
            }
        }

        public int GetProposedSupplier
        {
            get
            {
                { return Convert.ToInt32(ddlSupplier.SelectedValue); }
            }
        }

      
       

        

        public string GetSoleSource
        {
            get
            {
                return txtSoleSource.Text;
            }
        }

        public string GetSoleSourceJustificationPreparedBy
        {
            get
            {
                return txtRequester.Text;
            }
        }



        #endregion
        private string AutoNumber()
        {
            return "SVR-" + (_presenter.GetLastSoleVendorRequestId() + 1).ToString();
        }
        private void CheckApprovalSettings()
        {
            if (_presenter.GetApprovalSetting(RequestType.SoleVendor_Request.ToString().Replace('_', ' '), 0) == null)
            {
                pnlWarning.Visible = true;
            }
        }
        //private void PopProjects()
        //{
        //    ddlProject.DataSource = _presenter.GetProjects();
        //    ddlProject.DataBind();

        //    ddlProject.Items.Insert(0, new ListItem("---Select Project---", "0"));
        //    ddlProject.SelectedIndex = 0;
        //}
        //private void PopGrants(int ProjectId)
        //{
        //    ddlGrant.DataSource = _presenter.GetGrantbyprojectId(ProjectId);
        //    ddlGrant.DataBind();

        //    ddlGrant.Items.Insert(0, new ListItem("---Select Grant---", "0"));
        //    ddlGrant.SelectedIndex = 0;
        //}
        private void PopSupplier()
        {
            ddlSupplier.Items.Clear();
            ListItem lst = new ListItem();
            lst.Text = " Select Supplier ";
            lst.Value = "";
            ddlSupplier.Items.Add(lst);
            ddlSupplier.DataSource = _presenter.GetSoleVendorSuppliers();
            ddlSupplier.DataBind();
        }
        private void ClearFormFields()
        {
            //txtContactPersonNumber.Text = String.Empty;
            //txtSoleSource.Text = String.Empty;

        }        
        private void BindSoleVendorRequests()
        {
            grvSoleVendorRequestList.DataSource = _presenter.ListSoleVendorRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text);
            grvSoleVendorRequestList.DataBind();

        }
        private void BindSoleVendorRequestFields()
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentSoleVendorRequest != null)
            {
                txtRequestDate.Text = _presenter.CurrentSoleVendorRequest.RequestDate.Value.ToShortDateString();
                txtContactPersonNumber.Text = _presenter.CurrentSoleVendorRequest.ContactPersonNumber;
                PopSupplier();
                txtSoleSource.Text = _presenter.CurrentSoleVendorRequest.SoleSourceJustificationPreparedBy.ToString();
                txtselectionfor.Text = _presenter.CurrentSoleVendorRequest.ReasonForSelection;
                //ddlProject.SelectedValue = _presenter.CurrentSoleVendorRequest.Project.Id.ToString();
                //PopGrants(Convert.ToInt32(ddlProject.SelectedValue));
                //ddlGrant.SelectedValue = _presenter.CurrentSoleVendorRequest.Grant.Id.ToString();
                BindSoleVendorRequestDetails();
                BindSoleVendorRequests();
            }
        }
        private void BindSoleVendorRequestDetails()
        {
           /* int PuID = Convert.ToInt32(Session["prId"]);

            //if (_presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails.Count == 0)
            //{
            if (_presenter.GetPurchaseRequestbyPuID(PuID) != null)
            {
                PurchaseRequestDetail PD = _presenter.GetPurchaseRequestbyPuID(PuID);
                SoleVendorRequestDetail detail = new SoleVendorRequestDetail();
                detail.ItemAccount = PD.ItemAccount;
                detail.ItemDescription = PD.Item;
                detail.Qty = PD.Qty;
                detail.Project = PD.Project;
                detail.Grant = PD.Grant;
                _presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails.Add(detail);

            }*/
            //}
            dgSoleVenderDetail.DataSource = _presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails;
                dgSoleVenderDetail.DataBind();
           // prId = Convert.ToInt32(grvDetails.SelectedDataKey[0]);
            //PopPurchaseRequest();
           
           

            ////dgSoleVenderDetail.DataSource = _presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails;
            ////    dgSoleVenderDetail.DataBind();
            
            grvAttachments.DataSource = _presenter.CurrentSoleVendorRequest.SVRAttachments;
            grvAttachments.DataBind();

            
            PopPurchaseRequest();
        }
        private void BindSoleVendorRequestforprint()
        {
            lblRequestNoresult.Text = _presenter.CurrentSoleVendorRequest.RequestNo;
            lblRequestedDateresult.Text = _presenter.CurrentSoleVendorRequest.RequestDate.ToString();
            lblContactPersonNumberRes.Text = _presenter.CurrentSoleVendorRequest.ContactPersonNumber;
            lblProposedPurchasedpriceres.Text = _presenter.CurrentSoleVendorRequest.ProposedPurchasedPrice.ToString();
           // lblProposedSupplierresp.Text = _presenter.CurrentSoleVendorRequest.Supplier.SupplierName;
            lblSoleSourceJustificationPreparedByresp.Text = _presenter.CurrentSoleVendorRequest.SoleSourceJustificationPreparedBy;
            lblapprovalstatusres.Text = _presenter.CurrentSoleVendorRequest.CurrentStatus;
            lblRequesterres.Text = _presenter.GetUser(_presenter.CurrentSoleVendorRequest.AppUser.Id).FullName;


            grvStatuses.DataSource = _presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses;
            grvStatuses.DataBind();
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_presenter.GetUser(_presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses[e.Row.RowIndex].Approver) != null)
                    {
                        e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses[e.Row.RowIndex].Approver).FullName;
                    }
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails.Count != 0)
                {
                    if (_presenter.CurrentSoleVendorRequest.SVRAttachments.Count != 0)
                    {
                        int PRID = Convert.ToInt32(Session["PRID"]);
                        _presenter.SaveOrUpdateSoleVendorRequest(PRID);
                        if (_presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses.Count != 0 && _presenter.CurrentSoleVendorRequest.SVRAttachments.Count != 0)
                        {
                            BindSoleVendorRequests();
                            Master.ShowMessage(new AppMessage("Successfully did a Sole Vendor  Request, Reference No - <b>'" + _presenter.CurrentSoleVendorRequest.RequestNo + "'</b>", Chai.WorkflowManagment.Enums.RMessageType.Info));
                            Log.Info(_presenter.CurrentUser().FullName + " has requested a for Sole Vendor");
                            btnSave.Visible = false;
                            BindSoleVendorRequestforprint();
                            btnPrint.Enabled = true;
                        }
                    }
                    else
                    {
                        Master.ShowMessage(new AppMessage("Please Attach Sole Vendor Quotation", RMessageType.Error));
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Violation of UNIQUE KEY"))
                    {
                        Master.ShowMessage(new AppMessage("Please Click Request button Again,There is a duplicate Number", RMessageType.Error));
                        //AutoNumber();
                    }
                }
            }
           
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            _presenter.DeleteSoleVendorRequest(_presenter.CurrentSoleVendorRequest);
            ClearFormFields();
            BindSoleVendorRequests();
            btnDelete.Enabled = false;
            Master.ShowMessage(new AppMessage("Sole Vendor Request Successfully Deleted", Chai.WorkflowManagment.Enums.RMessageType.Info));
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void PopPurchaseRequestsDropDown()
        {
            ddlPurchaseReq.DataSource = _presenter.GetPurchaseRequestList();
            ddlPurchaseReq.DataBind();

            ddlPurchaseReq.Items.Insert(0, new ListItem("---Select Purchase Request---", ""));
            ddlPurchaseReq.SelectedIndex = 0;
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindSoleVendorRequests();
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
            Response.Redirect("frmSoleVendorRequest.aspx");
        }
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
            _presenter.CurrentSoleVendorRequest.RemoveSVRAttachment(filePath);
            File.Delete(Server.MapPath(filePath));
            grvAttachments.DataSource = _presenter.CurrentSoleVendorRequest.SVRAttachments;
            grvAttachments.DataBind();
            //Response.Redirect(Request.Url.AbsoluteUri);
        }
        private void UploadFile()
        {
            string fileName = Path.GetFileName(fuReciept.PostedFile.FileName);
            try
            {
                if (fileName != String.Empty)
                {
                    SVRAttachment attachment = new SVRAttachment();
                    attachment.FilePath = "~/SVUploads/" + fileName;
                    fuReciept.PostedFile.SaveAs(Server.MapPath("~/SVUploads/") + fileName);
                    //Response.Redirect(Request.Url.AbsoluteUri);
                    _presenter.CurrentSoleVendorRequest.SVRAttachments.Add(attachment);

                    grvAttachments.DataSource = _presenter.CurrentSoleVendorRequest.SVRAttachments;
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
        private void PopSoleVendorRequesters()
        {
            PurchaseRequest purchaseRequest = Session["PR"] as PurchaseRequest;
            if (_presenter.CurrentSoleVendorRequest != null && purchaseRequest != null)
            {

                txtRequester.Text = _presenter.CurrentUser().FirstName + " " + _presenter.CurrentUser().LastName;

                txtRequestDate.Text = purchaseRequest.RequestedDate.ToShortDateString();




                
                //ddlGrant.DataSource = _presenter.GetGrantbyprojectId(Convert.ToInt32(ddlProject.SelectedValue));
                //ddlGrant.DataBind();
                //ddlGrant.SelectedValue = purchaseRequest.PurchaseRequestDetails[0].Grant.Id.ToString();

                //    GridView1.DataSource = _presenter.CurrentBidAnalysisRequest.PurchaseRequest.PurchaseRequestDetails;
                //  GridView1.DataBind();
            }
            else if (purchaseRequest == null && _presenter.CurrentSoleVendorRequest != null)
            {
                PopPurchaseRequest();
            }
        }
        protected void dgSoleVenderDetail_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {


        }
        //protected void txtUnitCost_TextChanged(object sender, EventArgs e)
        //{
        //    TextBox txt = (TextBox)sender;
        //    TextBox hfQty = txt.FindControl("txtQty") as TextBox;
        //    TextBox txtUnitCost = txt.FindControl("txtUnitCost") as TextBox;
        //    TextBox txtTot = txt.FindControl("txtTotalCost") as TextBox;
        //    txtTot.Text = ((Convert.ToInt32(hfQty.Text) * Convert.ToDecimal(txtUnitCost.Text))).ToString();
        //}
        //protected void txtEdtUnitCost_TextChanged(object sender, EventArgs e)
        //{
        //    TextBox txt = (TextBox)sender;
        //    TextBox txtEdtQty = txt.FindControl("txtEdtQty") as TextBox;
        //    TextBox txtEdtUnitCost = txt.FindControl("txtEdtUnitCost") as TextBox;
        //    TextBox txtEdtTotalCost = txt.FindControl("txtEdtTotalCost") as TextBox;
        //    txtEdtTotalCost.Text = ((Convert.ToInt32(txtEdtQty.Text) * Convert.ToDecimal(txtEdtUnitCost.Text))).ToString();
        //}
        protected void dgSoleVenderDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

            
           if (_presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails != null)
            {
                DropDownList ddlItemAcc = e.Item.FindControl("ddlItemAcc") as DropDownList;
                if (ddlItemAcc != null)
                {
                    BindItems(ddlItemAcc);
                    if (_presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails[e.Item.DataSetIndex].ItemAccount.Id != 0)
                    {
                        ListItem liI = ddlItemAcc.Items.FindByValue(_presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails[e.Item.DataSetIndex].ItemAccount.Id.ToString());
                        if (liI != null)
                            liI.Selected = true;
                    }
                        
                    }
            }
            
        }
        private void BindItems(DropDownList ddlItems)
        {
            ddlItems.DataSource = _presenter.GetItemAccounts();
            ddlItems.DataBind();
        }
        protected void dgSoleVenderDetail_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void dgSoleVenderDetail_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "AddNew")
            {
                try
                {
                    SoleVendorRequestDetail detail = new SoleVendorRequestDetail();
                    detail.SoleVendorRequest = _presenter.CurrentSoleVendorRequest;
                    DropDownList ddlItem = e.Item.FindControl("ddlFItemAcc") as DropDownList;
                    detail.ItemAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlItem.SelectedValue));
                    TextBox txtItemDescription = e.Item.FindControl("txtFDescription") as TextBox;
                    detail.ItemDescription = txtItemDescription.Text;
                    DropDownList ddlSoleVendorJustification = e.Item.FindControl("ddlSoleVendorJustification") as DropDownList;
                    detail.SoleVendorJustificationType = ddlSoleVendorJustification.SelectedValue;
                    TextBox txtQty = e.Item.FindControl("txtQty") as TextBox;
                    detail.Qty = Convert.ToInt32(txtQty.Text);
                    TextBox txtUnitCost = e.Item.FindControl("txtUnitCost") as TextBox;
                    detail.UnitCost = Convert.ToDecimal(txtUnitCost.Text);
                    TextBox txtTotalCost = e.Item.FindControl("txtTotalCost") as TextBox;
                    detail.TotalCost = Convert.ToDecimal(txtTotalCost.Text);
                    _presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails.Add(detail);
                    dgSoleVenderDetail.EditItemIndex = -1;
                   // BindSoleVendorRequests();
                    BindSoleVendorRequestDetails();

                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Update Bidder " + ex.Message, RMessageType.Error));
                }
            }

        }
        protected void dgSoleVenderDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                int id = (int)dgSoleVenderDetail.DataKeys[e.Item.ItemIndex];

                SoleVendorRequestDetail detail;
                if (id > 0)
                    detail = _presenter.CurrentSoleVendorRequest.GetSoleVendorRequestDetailDetail(id);
                else
                    detail = (SoleVendorRequestDetail)_presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails[e.Item.ItemIndex];

                //   SoleVendorRequestDetail detail = new SoleVendorRequestDetail();

                detail.SoleVendorRequest = _presenter.CurrentSoleVendorRequest;
              //  DropDownList ddlItem = e.Item.FindControl("ddlItemAcc") as DropDownList;
                detail.ItemAccount = _presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails[0].ItemAccount;
              //  TextBox txtItemDescription = e.Item.FindControl("txtDescription") as TextBox;
                detail.ItemDescription = _presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails[0].ItemDescription;
                DropDownList ddlEdtSoleVendorJustification = e.Item.FindControl("ddlEdtSoleVendorJustification") as DropDownList;
                detail.SoleVendorJustificationType = ddlEdtSoleVendorJustification.SelectedValue;
             //   TextBox txtQty = e.Item.FindControl("txtEdtQty") as TextBox;
                detail.Qty = _presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails[0].Qty;
                TextBox txtUnitCost = e.Item.FindControl("txtEdtUnitCost") as TextBox;
                detail.UnitCost = Convert.ToDecimal(txtUnitCost.Text);
                TextBox txtTotalCost = e.Item.FindControl("txtEdtTotalCost") as TextBox;
                // detail.TotalCost = Convert.ToDecimal(txtTotalCost.Text);
                detail.TotalCost = Convert.ToInt32(detail.Qty) * Convert.ToDecimal(txtUnitCost.Text);
                dgSoleVenderDetail.EditItemIndex = -1;
                BindSoleVendorRequestDetails();
                Master.ShowMessage(new AppMessage("Sole Vendor Detail Successfully Updated", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Sole Vendor " + ex.Message, RMessageType.Error));
            }
        }
        protected void dgSoleVenderDetail_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgSoleVenderDetail.DataKeys[e.Item.ItemIndex];
            int TARDId = (int)dgSoleVenderDetail.DataKeys[e.Item.ItemIndex];
            SoleVendorRequestDetail tard;

            if (TARDId > 0)
                tard = _presenter.CurrentSoleVendorRequest.GetSoleVendorRequestDetailDetail(TARDId);
            else
                tard = (SoleVendorRequestDetail)_presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails[e.Item.ItemIndex];
            try
            {
                if (TARDId > 0)
                {
                    _presenter.CurrentSoleVendorRequest.RemoveSoleVendorRequestDetail(id);
                    if (_presenter.GetSoleVendorRequestDetail(id) != null)
                        _presenter.DeleteSoleVendorRequestDetail(_presenter.GetSoleVendorRequestDetail(id));
                    _presenter.SaveOrUpdateSoleVendorRequest(_presenter.CurrentSoleVendorRequest);
                }
                else { _presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails.Remove(tard); }


                Master.ShowMessage(new AppMessage("Sole Vendor Request Detail was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Sole Vendor Request Detail. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgSoleVenderDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgSoleVenderDetail.EditItemIndex = e.Item.ItemIndex;
            BindSoleVendorRequestDetails();
        }
        private void PopPurchaseRequest()
        {
          ////  txtRequester.Text = _presenter.CurrentSoleVendorRequest.PurchaseRequest..UserName.ToString();
          ////  // txtRequestDate.Text = _presenter.CurrentBidAnalysisRequest.RequestDate.Value.ToShortDateString();

          ////  //  txtselectionfor.Text = _presenter.CurrentBidAnalysisRequest.Neededfor;

          ////  txtselectionfor.Text = _presenter.CurrentSoleVendorRequest.Comment;
          //////  txtTotal.Text = Convert.ToDecimal(_presenter.CurrentSoleVendorRequest.t.TotalPrice).ToString();
           

          ////  grvDetails.DataSource = _presenter.CurrentSoleVendorRequest.PurchaseRequest.PurchaseRequestDetails;
          ////  grvDetails.DataBind();
            grvDetails.DataSource = _presenter.ListPurchaseReqInProgress();
            grvDetails.DataBind();

        }
        protected void grvSoleVendorRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvSoleVendorRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);            
        }
        protected void grvSoleVendorRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["SoleVendorRequest"] = true;
            BindSoleVendorRequestFields();
            if (_presenter.CurrentSoleVendorRequest.CurrentStatus != null)
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
        protected void grvSoleVendorRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                SoleVendorRequest soleVendorRequest = e.Row.DataItem as SoleVendorRequest;
                if (soleVendorRequest.CurrentStatus == "Rejected")
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void ddlPurchaseReq_SelectedIndexChanged(object sender, EventArgs e)
        {

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
            // int userid = _presenter.GetPurchaseRequestbyPuID(PuID).Requester;
            //Session["prId"] = _presenter.CurrentBidAnalysisRequest.PurchaseRequest;


            if (_presenter.GetPurchaseRequestbyPuID(PuID) != null)
            {
                PurchaseRequestDetail PD = _presenter.GetPurchaseRequestbyPuID(PuID);
                SoleVendorRequestDetail detail = new SoleVendorRequestDetail();
                    detail.ItemAccount = PD.ItemAccount;
                    detail.ItemDescription = PD.Item;
                    detail.Qty = PD.Qty;
                    detail.Project = PD.Project;
                    detail.Grant = PD.Grant;

                    _presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails.Add(detail);
                    txtRequester.Text = _presenter.CurrentUser().FirstName + " " + _presenter.CurrentUser().LastName;
                    lblPurchaseReqNo.Text = PD.PurchaseRequest.RequestNo;
                    int userid = _presenter.GetPurchaseRequestbyPuID(PuID).PurchaseRequest.Requester;
                    lblPurReqRequester.Text = _presenter.GetUser(userid).FullName;
                    PopProjects();
                    PopGrants();
                    PopSupplier();
                    ddlProject.SelectedItem.Text = detail.Project.ProjectCode;
                    ddlGrant.SelectedItem.Text = detail.Grant.GrantCode;
                   
                lblPurchaseRequestDate.Text= PD.PurchaseRequest.RequestedDate.ToShortDateString();


            }

            BindSoleVendorRequestDetails();
            grvDetails.DataSource = _presenter.ListPurchaseReqbyId(Convert.ToInt32(prId));
            grvDetails.DataBind();
            //dgSoleVenderDetail.DataSource = _presenter.ListPurchaseReqbyId(Convert.ToInt32(prId));
            //dgSoleVenderDetail.DataBind();
           

        }

        private void PopProjects()
        {
            ddlProject.DataSource = _presenter.GetProjects();
            ddlProject.DataBind();

            ddlProject.Items.Insert(0, new ListItem("---Select Project---", "0"));
            ddlProject.SelectedIndex = 0;
        }
        private void PopGrants()
        {
            ddlGrant.DataSource = _presenter.GetGrants();
            ddlGrant.DataBind();

            ddlGrant.Items.Insert(0, new ListItem("---Select Grant---", "0"));
            ddlGrant.SelectedIndex = 0;
        }

    }
}