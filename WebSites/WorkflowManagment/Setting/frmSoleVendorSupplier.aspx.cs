using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using Microsoft.Practices.ObjectBuilder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public partial class frmSoleVendorSupplier : POCBasePage, ISoleVendorSupplierView
    {
        private SoleVendorSupplierPresenter _presenter;
        private IList<SoleVendorSupplier> _SVendorSuppliers;
        private SupplierType _SupplierType;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindSoleVendorSupplier();
            }

            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public SoleVendorSupplierPresenter Presenter
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
                return "{8E5FAA6E-E9E9-4B21-888D-11539FA9BBA8}";
            }
        }

        void BindSoleVendorSupplier()
        {
            dgSupplier.DataSource = _presenter.ListSoleVendorSuppliers(txtSupplierName.Text);
            dgSupplier.DataBind();
        }
        private void BindSupplierTypes(DropDownList ddlSupplierTypes)
        {
            if (ddlSupplierTypes != null)
            {
                ddlSupplierTypes.DataSource = _presenter.GetSupplierTypes();
                ddlSupplierTypes.DataValueField = "Id";
                ddlSupplierTypes.DataTextField = "SupplierTypeName";
                ddlSupplierTypes.DataBind();
            }
        }

        #region interface
      
        #endregion
        protected void btnFind_Click(object sender, EventArgs e)
        {
            _presenter.ListSoleVendorSuppliers(SupplierName);
            BindSoleVendorSupplier();
        }
        protected void dgSupplier_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgSupplier.EditItemIndex = -1;
        }
        protected void dgSupplier_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgSupplier.DataKeys[e.Item.ItemIndex];
            Chai.WorkflowManagment.CoreDomain.Setting.SoleVendorSupplier SoleVendorSupplier = _presenter.GetSoleVendorSupplierById(id);
            try
            {
                SoleVendorSupplier.Status = "InActive";
                _presenter.SaveOrUpdateSoleVendorSupplier(SoleVendorSupplier);

                BindSoleVendorSupplier();

                Master.ShowMessage(new AppMessage("Supplier was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Supplier. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgSupplier_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Chai.WorkflowManagment.CoreDomain.Setting.SoleVendorSupplier supplier = new Chai.WorkflowManagment.CoreDomain.Setting.SoleVendorSupplier();
            if (e.CommandName == "AddNew")
            {
                try
                {
                    DropDownList ddlSuppliertype = e.Item.FindControl("ddlSupplierType") as DropDownList;
                    supplier.SupplierType = _presenter.GetSupplierTypeById(Convert.ToInt32(ddlSuppliertype.SelectedValue));
                    TextBox txtFSupplierName = e.Item.FindControl("txtFSupplierName") as TextBox;
                    supplier.SupplierName = txtFSupplierName.Text;
                    TextBox txtFSupplierAddress = e.Item.FindControl("txtFSupplierAddress") as TextBox;
                    supplier.SupplierAddress = txtFSupplierAddress.Text;
                    TextBox txtFSupplierContact = e.Item.FindControl("txtFSupplierContact") as TextBox;
                    supplier.SupplierContact = txtFSupplierContact.Text;
                    TextBox txtFSupplierphoneContact = e.Item.FindControl("txtFSupplierphoneContact") as TextBox;
                    supplier.ContactPhone = txtFSupplierphoneContact.Text;
                    TextBox txtFSupplierEmail = e.Item.FindControl("txtFSupplierEmail") as TextBox;
                    supplier.Email = txtFSupplierEmail.Text;
                    TextBox txtStartingDate = e.Item.FindControl("txtStartingDate") as TextBox;
                    supplier.StartingDate = Convert.ToDateTime(txtStartingDate.Text);
                    TextBox txtEndDate = e.Item.FindControl("txtEndDate") as TextBox;
                    supplier.EndDate = Convert.ToDateTime(txtEndDate.Text);
                    DropDownList ddlStatus = e.Item.FindControl("ddlFStatus") as DropDownList;
                    supplier.Status = ddlStatus.SelectedValue;
                    SaveSoleVendorSupplier(supplier);
                    dgSupplier.EditItemIndex = -1;
                    BindSoleVendorSupplier();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Supplier " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }

        private void SaveSoleVendorSupplier(Chai.WorkflowManagment.CoreDomain.Setting.SoleVendorSupplier Supplier)
        {
            try
            {
                if (Supplier.Id <= 0)
                {
                    _presenter.SaveOrUpdateSoleVendorSupplier(Supplier);
                    Master.ShowMessage(new AppMessage("Supplier saved", RMessageType.Info));
                    //_presenter.CancelPage();
                }
                else
                {
                    _presenter.SaveOrUpdateSoleVendorSupplier(Supplier);
                    Master.ShowMessage(new AppMessage("Supplier Updated", RMessageType.Info));
                    // _presenter.CancelPage();
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
            }
        }
        protected void dgSupplier_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgSupplier.EditItemIndex = e.Item.ItemIndex;

            BindSoleVendorSupplier();
        }
        protected void dgSupplier_ItemDataBound(object sender, DataGridItemEventArgs e)
        {



            if (e.Item.ItemType == ListItemType.Footer)
            {

                DropDownList ddlSupplierType = e.Item.FindControl("ddlSupplierType") as DropDownList;
                DropDownList ddlFStatus = e.Item.FindControl("ddlFStatus") as DropDownList;
            }
            else
            {

                DropDownList ddlEdtSupplierType = e.Item.FindControl("ddlEdtSupplierType") as DropDownList;
                if (ddlEdtSupplierType != null)
                {
                    if (_SVendorSuppliers[e.Item.DataSetIndex].Status != null)
                    {
                        ListItem liI = ddlEdtSupplierType.Items.FindByValue(_SVendorSuppliers[e.Item.DataSetIndex].SupplierType.ToString());
                        if (liI != null)
                            liI.Selected = true;
                    }
                }


                DropDownList ddlStatus = e.Item.FindControl("ddlStatus") as DropDownList;
                if (ddlStatus != null)
                {
                    if (_SVendorSuppliers[e.Item.DataSetIndex].Status != null)
                    {
                        ListItem liI = ddlStatus.Items.FindByValue(_SVendorSuppliers[e.Item.DataSetIndex].Status.ToString());
                        if (liI != null)
                            liI.Selected = true;
                    }
                }
            }
            
        }
        protected void dgSupplier_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int id = (int)dgSupplier.DataKeys[e.Item.ItemIndex];
            SoleVendorSupplier Supplier = _presenter.GetSoleVendorSupplierById(id);

            try
            {
                DropDownList ddlSuppliertype = e.Item.FindControl("ddlEdtSupplierType") as DropDownList;
                Supplier.SupplierType = _presenter.GetSupplierTypeById(Convert.ToInt32(ddlSuppliertype.SelectedValue));
                TextBox txtName = e.Item.FindControl("txtSupplierName") as TextBox;
                Supplier.SupplierName = txtName.Text;
                TextBox txtSupplierAddress = e.Item.FindControl("txtSupplierAddress") as TextBox;
                Supplier.SupplierAddress = txtSupplierAddress.Text;
                TextBox txtSupplierContact = e.Item.FindControl("txtSupplierContact") as TextBox;
                Supplier.SupplierContact = txtSupplierContact.Text;
                TextBox txtSupplierphoneContact = e.Item.FindControl("txtSupplierphoneContact") as TextBox;
                Supplier.ContactPhone = txtSupplierphoneContact.Text;
                TextBox txtFSupplierEmail = e.Item.FindControl("txtSupplierEmail") as TextBox;
                Supplier.Email = txtFSupplierEmail.Text;
                TextBox txtEdtStartingDate = e.Item.FindControl("txtEdtStartingDate") as TextBox;
                Supplier.StartingDate = Convert.ToDateTime(txtEdtStartingDate.Text);
                TextBox txtEdtEndDate = e.Item.FindControl("txtEdtEndDate") as TextBox;
                Supplier.EndDate = Convert.ToDateTime(txtEdtEndDate.Text);
                DropDownList ddlStatus = e.Item.FindControl("ddlStatus") as DropDownList;
                Supplier.Status = ddlStatus.SelectedValue;

                _presenter.SaveOrUpdateSoleVendorSupplier(Supplier);
                Master.ShowMessage(new AppMessage("Sole Vendor Supplier  Updated Successfully.", Chai.WorkflowManagment.Enums.RMessageType.Info));
                dgSupplier.EditItemIndex = -1;
                BindSoleVendorSupplier();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Sole Vendor Supplier. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }


        public string SupplierName
        {
            get { return txtSupplierName.Text; }
        }

       public IList<CoreDomain.Setting.SoleVendorSupplier> soleVendorSupplier
        {
            get
            {
                return _SVendorSuppliers;
            }

            set
            {
                _SVendorSuppliers = value;
            }
        }

        protected void dgSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            int suppliertypeid = (int)dgSupplier.DataKeys[dgSupplier.SelectedIndex];
            Session["SupplierTypeId"] = suppliertypeid;
            _SupplierType = _presenter.GetSupplierTypeById(suppliertypeid);
            Session["SupplierType"] = _SupplierType;
           
        }
    }
}