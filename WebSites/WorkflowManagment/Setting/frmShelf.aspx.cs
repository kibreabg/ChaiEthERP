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
    public partial class frmShelf : POCBasePage, IShelfView
    {
        private ShelfPresenter _presenter;
        private IList<Shelf> _Shelfs;
        private Shelf _Shelf;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindShelf();
            }

            this._presenter.OnViewLoaded();
            _Shelf = Session["Shelf"] as Shelf;

        }

        [CreateNew]
        public ShelfPresenter Presenter
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
        protected void BindShelf()
        {
            dgShelf.DataSource = _presenter.ListShelfs(GetShelfName, GetShelfCode);
            dgShelf.DataBind();
        }
        private void BindSections(DropDownList ddlSection)
        {
            if (ddlSection != null)
            {
                ddlSection.DataSource = _presenter.GetSections();
                ddlSection.DataValueField = "Id";
                ddlSection.DataTextField = "Name";
                ddlSection.DataBind();
            }
        }
      
        #region Interface      
        public string GetShelfName
        {
            get { return txtName.Text; }
        }
        public string GetShelfCode
        {
            get { return txtCode.Text; }
        }
        public IList<Shelf> GetShelfs
        {
            get
            {
                return _Shelfs;
            }
            set
            {
                _Shelfs = value;
            }
        }
        #endregion
        protected void btnFind_Click(object sender, EventArgs e)
        {
            _presenter.ListShelfs(GetShelfName, GetShelfCode);
            BindShelf();
        }
        protected void dgShelf_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgShelf.EditItemIndex = -1;
        }
        protected void dgShelf_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgShelf.DataKeys[e.Item.ItemIndex];
            Shelf Shelf = _presenter.GetShelfById(id);
            try
            {
                _presenter.DeleteShelf(Shelf);
                BindShelf();

                Master.ShowMessage(new AppMessage("Shelf was removed successfully", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Shelf. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgShelf_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Shelf shelf = new Shelf();
            if (e.CommandName == "AddNew")
            {
                try
                {
                    DropDownList ddlSection= e.Item.FindControl("ddlSection") as DropDownList;
                    shelf.Section = _presenter.GetSectionById(Convert.ToInt32(ddlSection.SelectedValue));
                    TextBox txtFName = e.Item.FindControl("txtFName") as TextBox;
                    shelf.Name = txtFName.Text;
                    TextBox txtFCode = e.Item.FindControl("txtFCode") as TextBox;
                    shelf.Code = txtFCode.Text;
                   
                    DropDownList ddlFStatus = e.Item.FindControl("ddlFStatus") as DropDownList;
                    shelf.Status = ddlFStatus.SelectedValue;

                    SaveShelf(shelf);
                    dgShelf.EditItemIndex = -1;
                    BindShelf();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Item " + ex.Message, RMessageType.Error));
                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
                }
            }
        }
        private void SaveShelf(Shelf shelf)
        {
            try
            {
                if (shelf.Id <= 0)
                {
                    _presenter.SaveOrUpdateShelf(shelf);
                    Master.ShowMessage(new AppMessage("Shelf Saved", RMessageType.Info));
                }
                else
                {
                    _presenter.SaveOrUpdateShelf(shelf);
                    Master.ShowMessage(new AppMessage("Shelf Updated", RMessageType.Info));
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgShelf_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgShelf.EditItemIndex = e.Item.ItemIndex;
            BindShelf();
        }
        protected void dgShelf_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlFStore = e.Item.FindControl("ddlFStore") as DropDownList;
                BindStore(ddlFStore);
                DropDownList ddlSection = e.Item.FindControl("ddlSection") as DropDownList;
                BindSection(ddlSection, Convert.ToInt32(ddlFStore.SelectedValue));
                
            }
            else
            {
                //DropDownList ddlStore = e.Item.FindControl("ddlStore") as DropDownList;
                //BindStore(ddlStore);
                //if (ddlStore != null)
                //{
                //    ListItem liI = ddlStore.Items.FindByValue(_Shelfs[e.Item.DataSetIndex].Section.Store.Id.ToString());
                //    if (liI != null)
                //        liI.Selected = true;
                //}
                //DropDownList ddlEdtSection = e.Item.FindControl("ddlEdtSection") as DropDownList;
                //if (ddlEdtSection != null)
                //{
                //    BindSection(ddlEdtSection, Convert.ToInt32(_presenter.GetShelfByStoreId(Project.Id));
                //    if (_presenter.GetCostSharingSettings()[e.Item.DataSetIndex].Grant != null)
                //    {
                //        if (_presenter.GetCostSharingSettings()[e.Item.DataSetIndex].Grant.Id != null)
                //        {
                //            ListItem liI = ddlGrant.Items.FindByValue(_presenter.GetCostSharingSettings()[e.Item.DataSetIndex].Grant.Id.ToString());
                //            if (liI != null)
                //                liI.Selected = true;
                //        }
                //    }

                //}
               


                DropDownList ddlStatus = e.Item.FindControl("ddlEdtStatus") as DropDownList;
                if (ddlStatus != null)
                {
                    ListItem liI = ddlStatus.Items.FindByValue(_Shelfs[e.Item.DataSetIndex].Status);
                    if (liI != null)
                        liI.Selected = true;
                }

            }
        }
        protected void dgShelf_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgShelf.DataKeys[e.Item.ItemIndex];
            Shelf shelf = _presenter.GetShelfById(id);

            try
            {
                DropDownList ddlEdtSection = e.Item.FindControl("ddlEdtSection") as DropDownList;
                shelf.Section = _presenter.GetSectionById(Convert.ToInt32(ddlEdtSection.SelectedValue));
                TextBox txtEdtName = e.Item.FindControl("txtEdtName") as TextBox;
                shelf.Name = txtEdtName.Text;
                TextBox txtEdtCode = e.Item.FindControl("txtEdtCode") as TextBox;
                shelf.Code = txtEdtCode.Text;

                DropDownList ddlEdtStatus = e.Item.FindControl("ddlEdtStatus") as DropDownList;
                shelf.Status = ddlEdtStatus.SelectedValue;

                SaveShelf(shelf);
                dgShelf.EditItemIndex = -1;
                BindShelf();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Shelf. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgShelf_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ShelfID = (int)dgShelf.DataKeys[dgShelf.SelectedIndex];
            Session["ShelfID"] = ShelfID;
            dgShelf.SelectedItemStyle.BackColor = System.Drawing.Color.BurlyWood;
            shelfDiv.Visible = true;
            _Shelf = _presenter.GetShelfById(ShelfID);
            Session["Shelf"] = _Shelf;
        }

        private void BindStore(DropDownList ddlStore)
        {
            ddlStore.DataSource = _presenter.GetStores();
            ddlStore.DataBind();

        }

        private void BindSection(DropDownList ddlSection, int storeId)
        {
            ddlSection.Items.Clear();
            ListItem lst = new ListItem();
            lst.Text = "Select Section";
            lst.Value = "";
            ddlSection.Items.Add(lst);
            ddlSection.DataSource = _presenter.GetShelfByStoreId(storeId);
            ddlSection.DataValueField = "Id";
            ddlSection.DataTextField = "Name";
            ddlSection.DataBind();
           
        }
        protected void ddlFStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlSection = ddl.FindControl("ddlSection") as DropDownList;
            BindSection(ddlSection, Convert.ToInt32(ddl.SelectedValue));

        }

        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlSection = ddl.FindControl("ddlEdtSection") as DropDownList;
            BindSection(ddlSection, Convert.ToInt32(ddl.SelectedValue));
        }
    }
}