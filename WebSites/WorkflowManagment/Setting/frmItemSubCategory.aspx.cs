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
    public partial class frmItemSubCategory : POCBasePage, IItemSubCategoryView
    {
        private ItemSubCategoryPresenter _presenter;
        private IList<ItemSubCategory> _ItemSubCategories;
        private ItemSubCategory _ItemSubCategory;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindItemSubCategory();
            }

            this._presenter.OnViewLoaded();
            _ItemSubCategory = Session["ItemSubCategory"] as ItemSubCategory;

        }

        [CreateNew]
        public ItemSubCategoryPresenter Presenter
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
                return "{D133723C-DEB8-4035-9A60-F9F8F7514EBC}";
            }
        }
        void BindItemSubCategory()
        {
            dgItemSubCategory.DataSource = _presenter.ListItemSubCategories(GetSubCategoryName, GetSubCategoryCode);
            dgItemSubCategory.DataBind();
        }
        private void BindItemCategories(DropDownList ddlItemCategories)
        {
            if (ddlItemCategories != null)
            {
                ddlItemCategories.DataSource = _presenter.GetItemCategories();
                ddlItemCategories.DataValueField = "Id";
                ddlItemCategories.DataTextField = "Name";
                ddlItemCategories.DataBind();
            }
        }
        #region Interface      
        public string GetSubCategoryName
        {
            get { return txtName.Text; }
        }
        public string GetSubCategoryCode
        {
            get { return txtCode.Text; }
        }
        public IList<ItemSubCategory> GetItemSubCategories
        {
            get
            {
                return _ItemSubCategories;
            }
            set
            {
                _ItemSubCategories = value;
            }
        }
        #endregion
        protected void btnFind_Click(object sender, EventArgs e)
        {
            _presenter.ListItemSubCategories(GetSubCategoryName, GetSubCategoryCode);
            BindItemSubCategory();
        }
        protected void dgItemSubCategory_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgItemSubCategory.EditItemIndex = -1;
        }
        protected void dgItemSubCategory_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgItemSubCategory.DataKeys[e.Item.ItemIndex];
            ItemSubCategory ItemSubCategory = _presenter.GetItemSubCategoryById(id);
            try
            {
                _presenter.DeleteItemSubCategory(ItemSubCategory);
                BindItemSubCategory();

                Master.ShowMessage(new AppMessage("Item Sub Category was removed successfully", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Item Sub Category. " + ex.Message, RMessageType.Error));
            }
        }
        protected void dgItemSubCategory_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            ItemSubCategory itemSubCategory = new ItemSubCategory();
            if (e.CommandName == "AddNew")
            {
                try
                {
                    DropDownList ddlItemCategory = e.Item.FindControl("ddlItemCategory") as DropDownList;
                    itemSubCategory.ItemCategory = _presenter.GetItemCategoryById(Convert.ToInt32(ddlItemCategory.SelectedValue));
                    TextBox txtFName = e.Item.FindControl("txtFName") as TextBox;
                    itemSubCategory.Name = txtFName.Text;
                    TextBox txtFCode = e.Item.FindControl("txtFCode") as TextBox;
                    itemSubCategory.Code = txtFCode.Text;

                    SaveItemSubCategory(itemSubCategory);
                    dgItemSubCategory.EditItemIndex = -1;
                    BindItemSubCategory();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Item Sub Category " + ex.Message, RMessageType.Error));
                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
                }
            }
        }
        private void SaveItemSubCategory(ItemSubCategory ItemSubCategory)
        {
            try
            {
                if (ItemSubCategory.Id <= 0)
                {
                    _presenter.SaveOrUpdateItemSubCategory(ItemSubCategory);
                    Master.ShowMessage(new AppMessage("Item Sub Category saved", RMessageType.Info));
                }
                else
                {
                    _presenter.SaveOrUpdateItemSubCategory(ItemSubCategory);
                    Master.ShowMessage(new AppMessage("Item Sub Category Updated", RMessageType.Info));
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgItemSubCategory_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgItemSubCategory.EditItemIndex = e.Item.ItemIndex;
            BindItemSubCategory();
        }
        protected void dgItemSubCategory_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlItemCategory = e.Item.FindControl("ddlItemCategory") as DropDownList;
                BindItemCategories(ddlItemCategory);
            }
            else
            {
                DropDownList ddlEdtItemCategory = e.Item.FindControl("ddlEdtItemCategory") as DropDownList;
                BindItemCategories(ddlEdtItemCategory);
                if (ddlEdtItemCategory != null)
                {
                    ListItem liI = ddlEdtItemCategory.Items.FindByValue(_ItemSubCategories[e.Item.DataSetIndex].ItemCategory.Id.ToString());
                    if (liI != null)
                        liI.Selected = true;                    
                }

            }
        }
        protected void dgItemSubCategory_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgItemSubCategory.DataKeys[e.Item.ItemIndex];
            ItemSubCategory itemSubCategory = _presenter.GetItemSubCategoryById(id);

            try
            {
                DropDownList ddlEdtItemCategory = e.Item.FindControl("ddlEdtItemCategory") as DropDownList;
                itemSubCategory.ItemCategory = _presenter.GetItemCategoryById(Convert.ToInt32(ddlEdtItemCategory.SelectedValue));
                TextBox txtEdtName = e.Item.FindControl("txtEdtName") as TextBox;
                itemSubCategory.Name = txtEdtName.Text;
                TextBox txtEdtCode = e.Item.FindControl("txtEdtCode") as TextBox;
                itemSubCategory.Code = txtEdtCode.Text;
                SaveItemSubCategory(itemSubCategory);
                dgItemSubCategory.EditItemIndex = -1;
                BindItemSubCategory();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Item Sub Category. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgItemSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ItemSubCategoryID = (int)dgItemSubCategory.DataKeys[dgItemSubCategory.SelectedIndex];
            Session["ItemSubCategoryID"] = ItemSubCategoryID;
            dgItemSubCategory.SelectedItemStyle.BackColor = System.Drawing.Color.BurlyWood;
            categoryDiv.Visible = true;
            _ItemSubCategory = _presenter.GetItemSubCategoryById(ItemSubCategoryID);
            Session["ItemSubCategory"] = _ItemSubCategory;
        }

    }
}