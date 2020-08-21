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
    public partial class frmStore : POCBasePage, IStore
    {
        private StorePresenter _presenter;
        private IList<Store> _stores;
        private Store _Store;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindStore();
            }

            this._presenter.OnViewLoaded();
            _Store = Session["Store"] as Store;

        }

        [CreateNew]
        public StorePresenter Presenter
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
        void BindStore()
        {
            dgStore.DataSource = _presenter.ListStores(GetName, GetLocation);
            dgStore.DataBind();
        }
        #region Interface      
        public string GetName
        {
            get { return txtName.Text; }
        }
      
        public IList<Store> GetStores
        {
            get
            {
                return _stores;
            }
            set
            {
                _stores = value;
            }
        }

      

      

        public string GetLocation
        {
            get
            {
               return txtLocation.Text;
            }
        }

      
        #endregion
        protected void btnFind_Click(object sender, EventArgs e)
        {
            _presenter.ListStores(GetName, GetLocation);
            BindStore();
        }
        protected void dgStore_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgStore.EditItemIndex = -1;
        }
        protected void dgStore_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgStore.DataKeys[e.Item.ItemIndex];
            Store Store = _presenter.GetStoreById(id);
            try
            {
                _presenter.DeleteStore(Store);
                BindStore();

                Master.ShowMessage(new AppMessage("Store was removed successfully", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete  Store. " + ex.Message, RMessageType.Error));
            }
        }
        protected void dgStore_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Store Store = new Store();
            if (e.CommandName == "AddNew")
            {
                try
                {
                    TextBox txtFName = e.Item.FindControl("txtFName") as TextBox;
                    Store.Name = txtFName.Text;
                    TextBox txtFLocation = e.Item.FindControl("txtFLocation") as TextBox;
                    Store.Location = txtFLocation.Text;
                    DropDownList ddlStatus = e.Item.FindControl("ddlFStatus") as DropDownList;
                    Store.Status = ddlStatus.SelectedValue;
                    SaveStore(Store);
                    dgStore.EditItemIndex = -1;
                    BindStore();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Item Category " + ex.Message, RMessageType.Error));
                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
                }
            }
        }
        private void SaveStore(Store Store)
        {
            try
            {
                if (Store.Id <= 0)
                {
                    _presenter.SaveOrUpdateStore(Store);
                    Master.ShowMessage(new AppMessage("Store saved", RMessageType.Info));
                }
                else
                {
                    _presenter.SaveOrUpdateStore(Store);
                    Master.ShowMessage(new AppMessage("Store Updated", RMessageType.Info));
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgStore_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgStore.EditItemIndex = e.Item.ItemIndex;
            BindStore();
        }
        protected void dgStore_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void dgStore_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgStore.DataKeys[e.Item.ItemIndex];
            Store Store = _presenter.GetStoreById(id);

            try
            {
                TextBox txtEdtName = e.Item.FindControl("txtEdtName") as TextBox;
                Store.Name = txtEdtName.Text;
                TextBox txtEdtLocation = e.Item.FindControl("txtEdtLocation") as TextBox;
                Store.Location = txtEdtLocation.Text;
                DropDownList ddlStatus = e.Item.FindControl("ddlStatus") as DropDownList;
                Store.Status = ddlStatus.SelectedValue;
                SaveStore(Store);
                dgStore.EditItemIndex = -1;
                BindStore();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Location. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            int StoreID = (int)dgStore.DataKeys[dgStore.SelectedIndex];
            Session["StoreID"] = StoreID;
            dgStore.SelectedItemStyle.BackColor = System.Drawing.Color.BurlyWood;
            storeDiv.Visible = true;
            _Store = _presenter.GetStoreById(StoreID);
            Session["Store"] = _Store;
        }

    }
}