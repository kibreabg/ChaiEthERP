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
    public partial class frmItemAccount : POCBasePage, IItemAccountView
    {
        private ItemAccountPresenter _presenter;
        private IList<ItemAccount> _ItemAccounts;
        private ItemAccount _ItemAccount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindItemAccount();
            }

            this._presenter.OnViewLoaded();
            _ItemAccount = Session["ItemAccount"] as ItemAccount;

        }

        [CreateNew]
        public ItemAccountPresenter Presenter
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
                return "{969246EF-65E7-4E32-87A9-86B481C365B1}";
            }
        }
        void BindItemAccount()
        {
            dgItemAccount.DataSource = _presenter.ListItemAccounts(txtItemAccountName.Text, txtItemAccountCode.Text);
            dgItemAccount.DataBind();
        }
        #region interface      
        public IList<ItemAccount> ItemAccount
        {
            get
            {
                return _ItemAccounts;
            }
            set
            {
                _ItemAccounts = value;
            }
        }
        #endregion
        protected void btnFind_Click(object sender, EventArgs e)
        {
            _presenter.ListItemAccounts(ItemAccountName, ItemAccountCode);
            BindItemAccount();
        }
        protected void dgItemAccount_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgItemAccount.EditItemIndex = -1;
        }
        protected void dgItemAccount_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgItemAccount.DataKeys[e.Item.ItemIndex];
            ItemAccount ItemAccount = _presenter.GetItemAccountById(id);
            try
            {
                ItemAccount.Status = "InActive";
                _presenter.SaveOrUpdateItemAccount(ItemAccount);
                BindItemAccount();

                Master.ShowMessage(new AppMessage("Item Account was Removed Successfully", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Item Account. " + ex.Message, RMessageType.Error));
            }
        }
        protected void dgItemAccount_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            ItemAccount ItemAccount = new ItemAccount();
            if (e.CommandName == "AddNew")
            {
                try
                {
                    TextBox txtFItemAccountName = e.Item.FindControl("txtFItemAccountName") as TextBox;
                    ItemAccount.AccountName = txtFItemAccountName.Text;
                    TextBox txtFItemAccountCode = e.Item.FindControl("txtFItemAccountCode") as TextBox;
                    ItemAccount.AccountCode = txtFItemAccountCode.Text;
                    ItemAccount.Status = "Active";

                    SaveItemAccount(ItemAccount);
                    dgItemAccount.EditItemIndex = -1;
                    BindItemAccount();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Item Account " + ex.Message, RMessageType.Error));
                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
                }
            }
        }
        private void SaveItemAccount(ItemAccount ItemAccount)
        {
            try
            {
                if (ItemAccount.Id <= 0)
                {
                    _presenter.SaveOrUpdateItemAccount(ItemAccount);
                    Master.ShowMessage(new AppMessage("Item Account saved", RMessageType.Info));
                    //_presenter.CancelPage();
                }
                else
                {
                    _presenter.SaveOrUpdateItemAccount(ItemAccount);
                    Master.ShowMessage(new AppMessage("Item Account Updated", RMessageType.Info));
                    // _presenter.CancelPage();
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
            }
        }
        protected void dgItemAccount_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgItemAccount.EditItemIndex = e.Item.ItemIndex;
            BindItemAccount();
        }
        protected void dgItemAccount_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void dgItemAccount_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgItemAccount.DataKeys[e.Item.ItemIndex];
            ItemAccount ItemAccount = _presenter.GetItemAccountById(id);

            try
            {
                TextBox txtName = e.Item.FindControl("txtItemAccountName") as TextBox;
                ItemAccount.AccountName = txtName.Text;
                TextBox txtCode = e.Item.FindControl("txtItemAccountCode") as TextBox;
                ItemAccount.AccountCode = txtCode.Text;
                SaveItemAccount(ItemAccount);
                dgItemAccount.EditItemIndex = -1;
                BindItemAccount();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Item Account. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgItemAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            int itemAccountID = (int)dgItemAccount.DataKeys[dgItemAccount.SelectedIndex];
            Session["itemAccountID"] = itemAccountID;
            dgItemAccount.SelectedItemStyle.BackColor = System.Drawing.Color.BurlyWood;
            accountDiv.Visible = true;
            _ItemAccount = _presenter.GetItemAccountById(itemAccountID);
            Session["ItemAccount"] = _ItemAccount;
            PnlChecklists.Visible = true;
            BindChecklists();
        }
        public string ItemAccountName
        {
            get { return txtItemAccountName.Text; }
        }
        public string ItemAccountCode
        {
            get { return txtItemAccountCode.Text; }
        }
        #region Checklists
        private void BindChecklists()
        {
            dgChecklists.DataSource = _ItemAccount.ItemAccountChecklists;
            dgChecklists.DataBind();
        }
        protected void dgChecklists_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgChecklists.EditItemIndex = -1;
            BindChecklists();
        }
        protected void dgChecklists_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgChecklists.DataKeys[e.Item.ItemIndex];

            try
            {
                _ItemAccount.RemoveChecklist(id);
                _presenter.DeleteChecklists(_presenter.GetChecklist(id));
                _presenter.SaveOrUpdateItemAccount(_ItemAccount);

                BindChecklists();

                Master.ShowMessage(new AppMessage("Checklist was removed successfully", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Checklist. " + ex.Message, RMessageType.Error));
            }
        }
        protected void dgChecklists_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgChecklists.EditItemIndex = e.Item.ItemIndex;
            BindChecklists();
        }
        protected void dgChecklists_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "AddNew")
            {
                try
                {
                    ItemAccountChecklist checklist = new ItemAccountChecklist();
                    TextBox txtFChecklistName = e.Item.FindControl("txtFChecklistName") as TextBox;
                    checklist.ChecklistName = txtFChecklistName.Text;
                    checklist.Status = "Active";
                    checklist.ItemAccount = _ItemAccount;
                    _ItemAccount.ItemAccountChecklists.Add(checklist);
                    _presenter.SaveOrUpdateItemAccount(_ItemAccount);
                    Master.ShowMessage(new AppMessage("Checklist Added Successfully.", RMessageType.Info));
                    dgChecklists.EditItemIndex = -1;
                    BindChecklists();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Checklist." + ex.Message, RMessageType.Error));
                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
                }
            }
        }
        protected void dgChecklists_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
        }
        protected void dgChecklists_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgChecklists.DataKeys[e.Item.ItemIndex];
            ItemAccountChecklist checklist = _ItemAccount.GetChecklist(id);
            try
            {
                TextBox txtFChecklistName = e.Item.FindControl("txtChecklistName") as TextBox;
                checklist.ChecklistName = txtFChecklistName.Text;
                _presenter.SaveOrUpdateItemAccount(_ItemAccount);
                Master.ShowMessage(new AppMessage("Checklist Updated Successfully.", RMessageType.Info));
                dgChecklists.EditItemIndex = -1;
                BindChecklists();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Checklist. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void btnCancelChecklist_Click(object sender, EventArgs e)
        {
            PnlChecklists.Visible = false;
        }
        #endregion


    }
}