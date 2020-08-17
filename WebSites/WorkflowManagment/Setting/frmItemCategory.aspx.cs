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
    public partial class frmItemCategory : POCBasePage, IItemCategoryView
    {
        private ItemCategoryPresenter _presenter;
        private IList<ItemCategory> _itemCategories;
        private ItemCategory _itemCategory;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindItemCategory();
            }

            this._presenter.OnViewLoaded();
            _itemCategory = Session["ItemCategory"] as ItemCategory;

        }

        [CreateNew]
        public ItemCategoryPresenter Presenter
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
                return "{E27C476B-55F6-4765-B804-65EBDADDF72D}";
            }
        }
        void BindItemCategory()
        {
            dgItemCategory.DataSource = _presenter.ListItemCategories(GetCategoryName, GetCategoryCode);
            dgItemCategory.DataBind();
        }
        #region Interface      
        public string GetCategoryName
        {
            get { return txtName.Text; }
        }
        public string GetCategoryCode
        {
            get { return txtCode.Text; }
        }
        public IList<ItemCategory> GetItemCategories
        {
            get
            {
                return _itemCategories;
            }
            set
            {
                _itemCategories = value;
            }
        }
        #endregion
        protected void btnFind_Click(object sender, EventArgs e)
        {
            _presenter.ListItemCategories(GetCategoryName, GetCategoryCode);
            BindItemCategory();
        }
        protected void dgItemCategory_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgItemCategory.EditItemIndex = -1;
        }
        protected void dgItemCategory_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgItemCategory.DataKeys[e.Item.ItemIndex];
            ItemCategory itemCategory = _presenter.GetItemCategoryById(id);
            try
            {
                _presenter.DeleteItemCategory(itemCategory);
                BindItemCategory();

                Master.ShowMessage(new AppMessage("Item Category was removed successfully", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Item Category. " + ex.Message, RMessageType.Error));
            }
        }
        protected void dgItemCategory_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            ItemCategory itemCategory = new ItemCategory();
            if (e.CommandName == "AddNew")
            {
                try
                {
                    TextBox txtFName = e.Item.FindControl("txtFName") as TextBox;
                    itemCategory.Name = txtFName.Text;
                    TextBox txtFCode = e.Item.FindControl("txtFCode") as TextBox;
                    itemCategory.Code = txtFCode.Text;

                    SaveItemCategory(itemCategory);
                    dgItemCategory.EditItemIndex = -1;
                    BindItemCategory();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Item Category " + ex.Message, RMessageType.Error));
                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
                }
            }
        }
        private void SaveItemCategory(ItemCategory itemCategory)
        {
            try
            {
                if (itemCategory.Id <= 0)
                {
                    _presenter.SaveOrUpdateItemCategory(itemCategory);
                    Master.ShowMessage(new AppMessage("Item Category saved", RMessageType.Info));
                }
                else
                {
                    _presenter.SaveOrUpdateItemCategory(itemCategory);
                    Master.ShowMessage(new AppMessage("Item Category Updated", RMessageType.Info));
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgItemCategory_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgItemCategory.EditItemIndex = e.Item.ItemIndex;
            BindItemCategory();
        }
        protected void dgItemCategory_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void dgItemCategory_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgItemCategory.DataKeys[e.Item.ItemIndex];
            ItemCategory itemCategory = _presenter.GetItemCategoryById(id);

            try
            {
                TextBox txtEdtName = e.Item.FindControl("txtEdtName") as TextBox;
                itemCategory.Name = txtEdtName.Text;
                TextBox txtEdtCode = e.Item.FindControl("txtEdtCode") as TextBox;
                itemCategory.Code = txtEdtCode.Text;
                SaveItemCategory(itemCategory);
                dgItemCategory.EditItemIndex = -1;
                BindItemCategory();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Item Category. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgItemCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ItemCategoryID = (int)dgItemCategory.DataKeys[dgItemCategory.SelectedIndex];
            Session["ItemCategoryID"] = ItemCategoryID;
            dgItemCategory.SelectedItemStyle.BackColor = System.Drawing.Color.BurlyWood;
            categoryDiv.Visible = true;
            _itemCategory = _presenter.GetItemCategoryById(ItemCategoryID);
            Session["ItemCategory"] = _itemCategory;
        }        
        
    }
}