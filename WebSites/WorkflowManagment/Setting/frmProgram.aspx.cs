using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using Microsoft.Practices.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public partial class frmProgram : POCBasePage, IProgramView
    {
        private ProgramPresenter _presenter;
        private IList<Program> _Programs;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindProgram();
            }
            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public ProgramPresenter Presenter
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
                return "{1A250DF8-AA19-4D36-9680-59B2B842B289}";
            }
        }
        #region Field Getters
        public string ProgramName
        {
            get { return txtProgramName.Text; }
        }
        public string ProgramCode
        {
            get { return txtProgramCode.Text; }
        }
        public IList<Program> Program
        {
            get
            {
                return _Programs;
            }
            set
            {
                _Programs = value;
            }
        }
        #endregion
        void BindProgram()
        {
            dgProgram.DataSource = _presenter.ListPrograms(txtProgramName.Text, txtProgramCode.Text);
            dgProgram.DataBind();
        }        
        protected void btnFind_Click(object sender, EventArgs e)
        {
            _presenter.ListPrograms(ProgramName, ProgramCode);
            BindProgram();
        }
        protected void dgProgram_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgProgram.EditItemIndex = -1;
        }
        protected void dgProgram_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgProgram.DataKeys[e.Item.ItemIndex];
            Program Program = _presenter.GetProgramById(id);
            try
            {
                Program.Status = "InActive";
                _presenter.SaveOrUpdateProgram(Program);

                BindProgram();

                Master.ShowMessage(new AppMessage("Program was Removed Successfully", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Program. " + ex.Message, RMessageType.Error));
            }
        }
        protected void dgProgram_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Program Program = new Program();
            if (e.CommandName == "AddNew")
            {
                try
                {
                    TextBox txtFProgramName = e.Item.FindControl("txtFProgramName") as TextBox;
                    Program.ProgramName = txtFProgramName.Text;
                    TextBox txtFProgramCode = e.Item.FindControl("txtFProgramCode") as TextBox;
                    Program.ProgramCode = txtFProgramCode.Text;
                    TextBox txtFDescription = e.Item.FindControl("txtFDescription") as TextBox;
                    Program.Description = txtFDescription.Text;
                    Program.Status = "Active";
                    SaveProgram(Program);
                    dgProgram.EditItemIndex = -1;
                    BindProgram();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Program " + ex.Message, RMessageType.Error));
                }
            }
        }
        private void SaveProgram(Program Program)
        {
            try
            {
                if (Program.Id <= 0)
                {
                    _presenter.SaveOrUpdateProgram(Program);
                    Master.ShowMessage(new AppMessage("Program saved", RMessageType.Info));
                    //_presenter.CancelPage();
                }
                else
                {
                    _presenter.SaveOrUpdateProgram(Program);
                    Master.ShowMessage(new AppMessage("Program Updated", RMessageType.Info));
                    // _presenter.CancelPage();
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
            }
        }
        protected void dgProgram_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgProgram.EditItemIndex = e.Item.ItemIndex;

            BindProgram();
        }
        protected void dgProgram_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void dgProgram_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int id = (int)dgProgram.DataKeys[e.Item.ItemIndex];
            Program Program = _presenter.GetProgramById(id);

            try
            {
                TextBox txtName = e.Item.FindControl("txtProgramName") as TextBox;
                Program.ProgramName = txtName.Text;
                TextBox txtCode = e.Item.FindControl("txtProgramCode") as TextBox;
                Program.ProgramCode = txtCode.Text;
                TextBox txtDescription = e.Item.FindControl("txtDescription") as TextBox;
                Program.Description = txtDescription.Text;
                SaveProgram(Program);
                dgProgram.EditItemIndex = -1;
                BindProgram();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Program. " + ex.Message, RMessageType.Error));
            }
        }
        
    }
}