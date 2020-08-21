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
    public partial class frmSection : POCBasePage, ISectionView
    {
        private SectionPresenter _presenter;
        private IList<Section> _Sections;
        private Section _Section;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindSection();
            }

            this._presenter.OnViewLoaded();
            _Section = Session["Section"] as Section;

        }

        [CreateNew]
        public SectionPresenter Presenter
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
        void BindSection()
        {
            dgSection.DataSource = _presenter.ListSections(GetSectionName, GetSectionCode);
            dgSection.DataBind();
        }
        private void BindStores(DropDownList ddlStores)
        {
            if (ddlStores != null)
            {
                ddlStores.DataSource = _presenter.GetStores();
                ddlStores.DataValueField = "Id";
                ddlStores.DataTextField = "Name";
                ddlStores.DataBind();
            }
        }
        #region Interface      
        public string GetSectionName
        {
            get { return txtName.Text; }
        }
        public string GetSectionCode
        {
            get { return txtCode.Text; }
        }
        public IList<Section> GetSections
        {
            get
            {
                return _Sections;
            }
            set
            {
                _Sections = value;
            }
        }
        #endregion
        protected void btnFind_Click(object sender, EventArgs e)
        {
            _presenter.ListSections(GetSectionName, GetSectionCode);
            BindSection();
        }
        protected void dgSection_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgSection.EditItemIndex = -1;
        }
        protected void dgSection_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgSection.DataKeys[e.Item.ItemIndex];
            Section Section = _presenter.GetSectById(id);
            try
            {
                _presenter.DeleteSection(Section);
                BindSection();

                Master.ShowMessage(new AppMessage(" Section was removed successfully", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Section. " + ex.Message, RMessageType.Error));
            }
        }
        protected void dgSection_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Section Section = new Section();
            if (e.CommandName == "AddNew")
            {
                try
                {
                    DropDownList ddlStore = e.Item.FindControl("ddlStore") as DropDownList;
                    Section.Store = _presenter.GetSectionById(Convert.ToInt32(ddlStore.SelectedValue));
                    TextBox txtFName = e.Item.FindControl("txtFName") as TextBox;
                    Section.Name = txtFName.Text;
                    TextBox txtFCode = e.Item.FindControl("txtFCode") as TextBox;
                    Section.Code = txtFCode.Text;
                    DropDownList ddlFStatus = e.Item.FindControl("ddlFStatus") as DropDownList;
                    Section.Status = ddlFStatus.SelectedValue;
                    SaveSection(Section);
                    dgSection.EditItemIndex = -1;
                    BindSection();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Section " + ex.Message, RMessageType.Error));
                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
                }
            }
        }
        private void SaveSection(Section Section)
        {
            try
            {
                if (Section.Id <= 0)
                {
                    _presenter.SaveOrUpdateSection(Section);
                    Master.ShowMessage(new AppMessage("Section saved", RMessageType.Info));
                }
                else
                {
                    _presenter.SaveOrUpdateSection(Section);
                    Master.ShowMessage(new AppMessage("Section Updated", RMessageType.Info));
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgSection_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgSection.EditItemIndex = e.Item.ItemIndex;
            BindSection();
        }
        protected void dgSection_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlStore = e.Item.FindControl("ddlStore") as DropDownList;
                BindStores(ddlStore);
            }
            else
            {
                DropDownList ddlEdtStore = e.Item.FindControl("ddlEdtStore") as DropDownList;
                BindStores(ddlEdtStore);
                if (ddlEdtStore != null)
                {
                    ListItem liI = ddlEdtStore.Items.FindByValue(_Sections[e.Item.DataSetIndex].Store.Id.ToString());
                    if (liI != null)
                        liI.Selected = true;                    
                }

            }
        }
        protected void dgSection_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgSection.DataKeys[e.Item.ItemIndex];
            Section Section = _presenter.GetSectById(id);

            try
            {
                DropDownList ddlEdtStore = e.Item.FindControl("ddlEdtStore") as DropDownList;
                Section.Store = _presenter.GetSectionById(Convert.ToInt32(ddlEdtStore.SelectedValue));
                TextBox txtEdtName = e.Item.FindControl("txtEdtName") as TextBox;
                Section.Name = txtEdtName.Text;
                TextBox txtEdtCode = e.Item.FindControl("txtEdtCode") as TextBox;
                Section.Code = txtEdtCode.Text;
                DropDownList ddlStatus = e.Item.FindControl("ddlStatus") as DropDownList;
                Section.Status = ddlStatus.SelectedValue;
                SaveSection(Section);
                dgSection.EditItemIndex = -1;
                BindSection();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Section. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SectionID = (int)dgSection.DataKeys[dgSection.SelectedIndex];
            Session["SectionID"] = SectionID;
            dgSection.SelectedItemStyle.BackColor = System.Drawing.Color.BurlyWood;
            sectionDiv.Visible = true;
            _Section = _presenter.GetSectById(SectionID);
            Session["Section"] = _Section;
        }

    }
}