using Chai.WorkflowManagment.CoreDomain.Inventory;
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
    public partial class frmItem : POCBasePage, IItemView
    {
        private ItemPresenter _presenter;
        private IList<Item> _Items;
        private Item _Item;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindItem();
            }

            this._presenter.OnViewLoaded();
            _Item = Session["Item"] as Item;

        }

        [CreateNew]
        public ItemPresenter Presenter
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
                return "{84CAF50C-FD8F-44F7-8438-E5AE97355565}";
            }
        }
        protected void BindItem()
        {
            dgItem.DataSource = _presenter.ListItems(GetItemName, GetItemCode);
            dgItem.DataBind();
        }
        private void BindItemSubCategories(DropDownList ddlItemSubCategory)
        {
            if (ddlItemSubCategory != null)
            {
                ddlItemSubCategory.DataSource = _presenter.GetItemSubCategories();
                ddlItemSubCategory.DataValueField = "Id";
                ddlItemSubCategory.DataTextField = "Name";
                ddlItemSubCategory.DataBind();
            }
        }
        private void BindUnitOfMeasurements(DropDownList ddlUnitOfMeas)
        {
            if (ddlUnitOfMeas != null)
            {
                ddlUnitOfMeas.DataSource = _presenter.GetUnitOfMeasurements();
                ddlUnitOfMeas.DataValueField = "Id";
                ddlUnitOfMeas.DataTextField = "Name";
                ddlUnitOfMeas.DataBind();
            }
        }
        #region Interface      
        public string GetItemName
        {
            get { return txtName.Text; }
        }
        public string GetItemCode
        {
            get { return txtCode.Text; }
        }
        public IList<Item> GetItems
        {
            get
            {
                return _Items;
            }
            set
            {
                _Items = value;
            }
        }
        #endregion
        protected void btnFind_Click(object sender, EventArgs e)
        {
            _presenter.ListItems(GetItemName, GetItemCode);
            BindItem();
        }
        protected void dgItem_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgItem.EditItemIndex = -1;
        }
        protected void dgItem_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgItem.DataKeys[e.Item.ItemIndex];
            Item Item = _presenter.GetItemById(id);
            try
            {
                _presenter.DeleteItem(Item);
                BindItem();

                Master.ShowMessage(new AppMessage("Item was removed successfully", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Item. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgItem_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Item item = new Item();
            if (e.CommandName == "AddNew")
            {
                try
                {
                    DropDownList ddlItemSubCategory = e.Item.FindControl("ddlItemSubCategory") as DropDownList;
                    item.ItemSubCategory = _presenter.GetItemSubCategoryById(Convert.ToInt32(ddlItemSubCategory.SelectedValue));
                    TextBox txtFName = e.Item.FindControl("txtFName") as TextBox;
                    item.Name = txtFName.Text;
                    TextBox txtFCode = e.Item.FindControl("txtFCode") as TextBox;
                    item.Code = txtFCode.Text;
                    DropDownList ddlItemType = e.Item.FindControl("ddlItemType") as DropDownList;
                    item.ItemType = ddlItemType.SelectedValue;
                    CheckBox ckIsSparePart = e.Item.FindControl("ckIsSparePart") as CheckBox;
                    item.IsSparePart = ckIsSparePart.Checked;
                    TextBox txtFReOrderQty = e.Item.FindControl("txtFReOrderQty") as TextBox;
                    item.ReOrderQuantity = Convert.ToInt32(txtFReOrderQty.Text);
                    DropDownList ddlUnitOfMeas = e.Item.FindControl("ddlUnitOfMeas") as DropDownList;
                    item.UnitOfMeasurement = _presenter.GetUnitOfMeasurement(Convert.ToInt32(ddlUnitOfMeas.SelectedValue));
                    DropDownList ddlFStatus = e.Item.FindControl("ddlFStatus") as DropDownList;
                    item.Status = ddlFStatus.SelectedValue;

                    SaveItem(item);
                    dgItem.EditItemIndex = -1;
                    BindItem();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Item " + ex.Message, RMessageType.Error));
                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
                }
            }
        }
        private void SaveItem(Item item)
        {
            try
            {
                if (item.Id <= 0)
                {
                    _presenter.SaveOrUpdateItem(item);
                    Master.ShowMessage(new AppMessage("Item Saved", RMessageType.Info));
                }
                else
                {
                    _presenter.SaveOrUpdateItem(item);
                    Master.ShowMessage(new AppMessage("Item Updated", RMessageType.Info));
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgItem_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgItem.EditItemIndex = e.Item.ItemIndex;
            BindItem();
        }
        protected void dgItem_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlItemSubCategory = e.Item.FindControl("ddlItemSubCategory") as DropDownList;
                BindItemSubCategories(ddlItemSubCategory);
                DropDownList ddlUnitOfMeas = e.Item.FindControl("ddlUnitOfMeas") as DropDownList;
                BindUnitOfMeasurements(ddlUnitOfMeas);

            }
            else
            {
                DropDownList ddlEdtItemSubCategory = e.Item.FindControl("ddlEdtItemSubCategory") as DropDownList;
                BindItemSubCategories(ddlEdtItemSubCategory);
                if (ddlEdtItemSubCategory != null)
                {
                    ListItem liI = ddlEdtItemSubCategory.Items.FindByValue(_Items[e.Item.DataSetIndex].ItemSubCategory.Id.ToString());
                    if (liI != null)
                        liI.Selected = true;
                }
                DropDownList ddlEdtUnitOfMeas = e.Item.FindControl("ddlEdtUnitOfMeas") as DropDownList;
                BindUnitOfMeasurements(ddlEdtUnitOfMeas);
                if (ddlEdtUnitOfMeas != null)
                {
                    ListItem liI = ddlEdtUnitOfMeas.Items.FindByValue(_Items[e.Item.DataSetIndex].UnitOfMeasurement.Id.ToString());
                    if (liI != null)
                        liI.Selected = true;
                }
                DropDownList ddlEdtItemType = e.Item.FindControl("ddlEdtItemType") as DropDownList;
                if (ddlEdtItemType != null)
                {
                    ListItem liI = ddlEdtItemType.Items.FindByValue(_Items[e.Item.DataSetIndex].ItemType);
                    if (liI != null)
                        liI.Selected = true;
                }
                DropDownList ddlStatus = e.Item.FindControl("ddlStatus") as DropDownList;
                if (ddlStatus != null)
                {
                    ListItem liI = ddlStatus.Items.FindByValue(_Items[e.Item.DataSetIndex].Status);
                    if (liI != null)
                        liI.Selected = true;
                }

            }
        }
        protected void dgItem_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgItem.DataKeys[e.Item.ItemIndex];
            Item item = _presenter.GetItemById(id);

            try
            {
                DropDownList ddlEdtItemSubCategory = e.Item.FindControl("ddlEdtItemSubCategory") as DropDownList;
                item.ItemSubCategory = _presenter.GetItemSubCategoryById(Convert.ToInt32(ddlEdtItemSubCategory.SelectedValue));
                TextBox txtEdtName = e.Item.FindControl("txtEdtName") as TextBox;
                item.Name = txtEdtName.Text;
                TextBox txtEdtCode = e.Item.FindControl("txtEdtCode") as TextBox;
                item.Code = txtEdtCode.Text;
                DropDownList ddlEdtItemType = e.Item.FindControl("ddlEdtItemType") as DropDownList;
                item.ItemType = ddlEdtItemType.SelectedValue;
                CheckBox ckEdtIsSparePart = e.Item.FindControl("ckEdtIsSparePart") as CheckBox;
                item.IsSparePart = ckEdtIsSparePart.Checked;
                TextBox txtEdtReOrderQty = e.Item.FindControl("txtEdtReOrderQty") as TextBox;
                item.ReOrderQuantity = Convert.ToInt32(txtEdtReOrderQty.Text);
                DropDownList ddlEdtUnitOfMeas = e.Item.FindControl("ddlEdtUnitOfMeas") as DropDownList;
                item.UnitOfMeasurement = _presenter.GetUnitOfMeasurement(Convert.ToInt32(ddlEdtUnitOfMeas.SelectedValue));
                DropDownList ddlStatus = e.Item.FindControl("ddlStatus") as DropDownList;
                item.Status = ddlStatus.SelectedValue;

                SaveItem(item);
                dgItem.EditItemIndex = -1;
                BindItem();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Item. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ItemID = (int)dgItem.DataKeys[dgItem.SelectedIndex];
            Session["ItemID"] = ItemID;
            dgItem.SelectedItemStyle.BackColor = System.Drawing.Color.BurlyWood;
            categoryDiv.Visible = true;
            _Item = _presenter.GetItemById(ItemID);
            Session["Item"] = _Item;
        }

    }
}