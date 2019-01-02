using Chai.WorkflowManagment.CoreDomain.HRM;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using Microsoft.Practices.ObjectBuilder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public partial class frmEmpLeaveSetting : POCBasePage, IEmpLeaveSettingView
    {
        private EmpLeaveSettingPresenter _presenter;
        decimal totalPercentage = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindCostSharing();

            }

            this._presenter.OnViewLoaded();


        }

        [CreateNew]
        public EmpLeaveSettingPresenter Presenter
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
                return "{FAC4E85E-A360-4F0E-55A6-099B38F4D615}";
            }
        }

        private void BindCostSharing()
        {

            dgItemDetail.DataSource = _presenter.GetEmployees(txtSrchSrchFullName.Text);
            dgItemDetail.DataBind();
        }


        private void SetEmployeeOpeningBalance()
        {
            try
            {
                int index = 0;
                bool isupdate = false;
                foreach (DataGridItem dgi in dgItemDetail.Items)
                {
                    int id = (int)dgItemDetail.DataKeys[dgi.ItemIndex];
                    CheckBox chkselect = dgi.FindControl("chkselect") as CheckBox;
                    Employee detail;
              
                    detail = _presenter.GetEmployee(id);
                    if (detail != null)
                    {
                        if (chkselect.Checked)
                        {
                            TextBox txtOpeningLeavebalance = dgi.FindControl("txtOpeningLeavebalance") as TextBox;
                            detail.SDLeaveBalance = Convert.ToDecimal(txtOpeningLeavebalance.Text);
                            TextBox txtLeaveSettingDate = dgi.FindControl("txtOpeningLeavebalancedate") as TextBox;
                            detail.LeaveSettingDate = Convert.ToDateTime(txtLeaveSettingDate.Text);
                            detail.AppUser = detail.AppUser;
                            _presenter.SaveOrUpdateEmpLeaveSetting(detail);
                            isupdate = true;
                        }
                    }
                    index++;

                }
                if (isupdate==true)
                    Master.ShowMessage(new AppMessage("Employee opening balance successfully saved", Chai.WorkflowManagment.Enums.RMessageType.Info));
                else
                    Master.ShowMessage(new AppMessage("Please select at least one employee to set opening balance ", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state has the following validation errors:",
                        eve.Entry.Entity.GetType().Name);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            
        }

        private void SetEmployeeEndingBalance()
        {
            int index = 0;
            bool isupdate = false;
            foreach (DataGridItem dgi in dgItemDetail.Items)
            {
                int id = (int)dgItemDetail.DataKeys[dgi.ItemIndex];
                CheckBox chkselect = dgi.FindControl("chkselect") as CheckBox;
                Employee detail;

                detail = _presenter.GetEmployee(id);
                if (detail != null)
                {
                    if (chkselect.Checked)
                    {
                        decimal balance = Convert.ToDecimal(detail.EmployeeLeaveBalanceYE()) - _presenter.EmpLeaveTaken(detail.Id, detail.LeaveSettingDate.Value);
                        if (balance > 20)
                        {
                            detail.SDLeaveBalance = 20;
                            detail.ExpiredLeave = balance - 20;
                        }
                        else
                        {
                            detail.SDLeaveBalance = balance;
                        }
                        detail.LeaveSettingDate = new DateTime(DateTime.Today.Year, 01, 01); 
                        _presenter.SaveOrUpdateEmpLeaveSetting(detail);
                    }
                }
                index++;

            }
            if (isupdate == true)
                Master.ShowMessage(new AppMessage("Employee ending balance successfully saved", Chai.WorkflowManagment.Enums.RMessageType.Info));
            else
                Master.ShowMessage(new AppMessage("Please select at least one employee to set ending balance ", Chai.WorkflowManagment.Enums.RMessageType.Info));
            
        }

        protected void dgItemDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                if (_presenter.GetEmployees(txtSrchSrchFullName.Text) != null)
                {
                    Employee emp = e.Item.DataItem as Employee;
                    TextBox txtOpeningLeaveBalanceDate = e.Item.FindControl("txtOpeningLeavebalancedate") as TextBox;
                    if (emp.LeaveSettingDate != null)
                        txtOpeningLeaveBalanceDate.Text = emp.LeaveSettingDate.Value.ToShortDateString();
                    else
                        txtOpeningLeaveBalanceDate.Text = DateTime.Now.Date.ToShortDateString();
                }
            }
        }

        protected void btnEnd_Click(object sender, EventArgs e)
        {
            SetEmployeeEndingBalance();
        }

        protected void btnOpen_Click(object sender, EventArgs e)
        {
            SetEmployeeOpeningBalance();
        }
        protected void chkCheackAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkCheackAll = (CheckBox)sender;

            foreach (DataGridItem row in dgItemDetail.Items)
            {
                CheckBox chkrow = (CheckBox)row.FindControl("chkselect");
                if (chkCheackAll.Checked == true)
                {
                    chkrow.Checked = true;
                }
                else

                {
                    chkrow.Checked = true;
                }
            }
        }

        //Added by wwp **add 'Select All' Tick box
        protected void chkItems_CheckedChanged(object sender, EventArgs e)
        {
            SelectAllgrvWorkSheet();
        }
        private void SelectAllgrvWorkSheet()
        {
            try
            {
                if (chkItems.Checked)
                {
                    if (dgItemDetail.Items.Count > 0)
                    {
                        foreach (DataGridItem items in dgItemDetail.Items)
                        {
                            CheckBox chkselect = items.FindControl("chkselect") as CheckBox;
                            chkselect.Checked = true;
                        }
                    }
                }
                else
                {
                    if (dgItemDetail.Items.Count > 0)
                    {
                        foreach (DataGridItem items in dgItemDetail.Items)
                        {
                            CheckBox chkselect = items.FindControl("chkselect") as CheckBox;
                            chkselect.Checked = false;
                        }
                    }
                }

            }
            catch
            {

            }
        }

        protected void dgItemDetail_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindCostSharing();
        }
    }
}