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
    public partial class frmHoliday : POCBasePage, IHolidayView
    {
        private HolidayPresenter _presenter;
        private IList<Holiday> _Holidays;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindHoliday();
            }
            
            this._presenter.OnViewLoaded();
            

        }

        [CreateNew]
        public HolidayPresenter Presenter
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
                return "{990C732F-25EE-3387-A093-423026F33676}";
            }
        }

        void BindHoliday()
        {
            dgHoliday.DataSource = _presenter.ListHolidays();
            dgHoliday.DataBind();
        }
        #region interface


        public IList<CoreDomain.Setting.Holiday> holidays
        {
            get
            {
                return _Holidays;
            }
            set
            {
                _Holidays = value;
            }
        }
        #endregion
        protected void btnFind_Click(object sender, EventArgs e)
        {
            _presenter.ListHolidays();
            BindHoliday();
        }
        protected void dgHoliday_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgHoliday.EditItemIndex = -1;
        }
        protected void dgHoliday_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgHoliday.DataKeys[e.Item.ItemIndex];
            Chai.WorkflowManagment.CoreDomain.Setting.Holiday Holiday = _presenter.GetHolidayById(id);
            try
            {
                _presenter.DeleteHoliday(Holiday);
                _presenter.SaveOrUpdateHoliday(Holiday);
                BindHoliday();

                Master.ShowMessage(new AppMessage("Holiday was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Holiday. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgHoliday_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Chai.WorkflowManagment.CoreDomain.Setting.Holiday Holiday = new Chai.WorkflowManagment.CoreDomain.Setting.Holiday();
            if (e.CommandName == "AddNew")
            {
                try
                {

                    TextBox txtFHolidayName = e.Item.FindControl("txtFHolidayName") as TextBox;
                    Holiday.HolidayName = txtFHolidayName.Text;
                    TextBox txtFDate = e.Item.FindControl("txtFDate") as TextBox;
                    Holiday.Date = Convert.ToDateTime(txtFDate.Text);

                    SaveHoliday(Holiday);
                    dgHoliday.EditItemIndex = -1;
                    BindHoliday();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Holiday " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }

        private void SaveHoliday(Chai.WorkflowManagment.CoreDomain.Setting.Holiday Holiday)
        {
            try
            {
                if(Holiday.Id  <= 0)
                {
                    _presenter.SaveOrUpdateHoliday(Holiday);
                    Master.ShowMessage(new AppMessage("Holiday saved", RMessageType.Info));
                    //_presenter.CancelPage();
                }
                else
                {
                    _presenter.SaveOrUpdateHoliday(Holiday);
                    Master.ShowMessage(new AppMessage("Holiday Updated", RMessageType.Info));
                   // _presenter.CancelPage();
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
            }
        }
        protected void dgHoliday_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgHoliday.EditItemIndex = e.Item.ItemIndex;

            BindHoliday();
        }
        protected void dgHoliday_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void dgHoliday_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int id = (int)dgHoliday.DataKeys[e.Item.ItemIndex];
            Chai.WorkflowManagment.CoreDomain.Setting.Holiday Holiday = _presenter.GetHolidayById(id);

            try
            {


                TextBox txtName = e.Item.FindControl("txtHolidayName") as TextBox;
                Holiday.HolidayName = txtName.Text;
                TextBox txtDate = e.Item.FindControl("txtDate") as TextBox;
                Holiday.Date = Convert.ToDateTime(txtDate.Text);
                SaveHoliday(Holiday);
                dgHoliday.EditItemIndex = -1;
                BindHoliday();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Holiday. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        } 


     
    }
}