using Chai.WorkflowManagment.CoreDomain.HRM;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;
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
               
            dgItemDetail.DataSource = _presenter.GetEmployees();
            dgItemDetail.DataBind();
        }
      
       
        private void SetEmployeeOpeningBalance()
        {
            int index = 0;
            
            foreach (DataGridItem dgi in dgItemDetail.Items)
            {
                int id = (int)dgItemDetail.DataKeys[dgi.ItemIndex];

                Employee detail;
                
                detail = _presenter.GetEmployee(id);
                if (detail != null)
                {
                    TextBox txtOpeningLeavebalance = dgi.FindControl("txtOpeningLeavebalance") as TextBox;
                    detail.SDLeaveBalance = Convert.ToInt32(txtOpeningLeavebalance.Text);
                    TextBox txtLeaveSettingDate = dgi.FindControl("txtOpeningLeavebalancedate") as TextBox;
                    detail.LeaveSettingDate = Convert.ToDateTime(txtLeaveSettingDate.Text);
                    _presenter.SaveOrUpdateEmpLeaveSetting(detail);
                }
                index++;

            }
           
            Master.ShowMessage(new AppMessage("Employee Opening Balance successfully saved", Chai.WorkflowManagment.Enums.RMessageType.Info));
        }

        private void SetEmployeeEndingBalance()
        {
            int index = 0;

            foreach (DataGridItem dgi in dgItemDetail.Items)
            {
                int id = (int)dgItemDetail.DataKeys[dgi.ItemIndex];

                Employee detail;

                detail = _presenter.GetEmployee(id);
                if (detail != null)
                {
                  decimal balance =  Convert.ToInt32(detail.EmployeeLeaveBalanceYE()) -  _presenter.EmpLeaveTaken(detail.Id, detail.LeaveSettingDate.Value);
                    if (balance > 20)
                        detail.SDLeaveBalance = 20;
                    else
                        detail.SDLeaveBalance = balance;
                    detail.LeaveSettingDate = new DateTime(DateTime.Now.Year, 12, 31); ;
                    _presenter.SaveOrUpdateEmpLeaveSetting(detail);
                }
                index++;

            }

            Master.ShowMessage(new AppMessage("Employee Ending Balance successfully saved", Chai.WorkflowManagment.Enums.RMessageType.Info));
        }

        protected void dgItemDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                if (_presenter.GetEmployees() != null)
                {
                    TextBox txtOpeningLeaveBalanceDate = e.Item.FindControl("txtOpeningLeavebalancedate") as TextBox;
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
    }
}