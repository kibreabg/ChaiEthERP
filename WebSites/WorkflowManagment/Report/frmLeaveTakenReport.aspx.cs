using System;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.HRM;

namespace Chai.WorkflowManagment.Modules.Report.Views
{
    public partial class frmLeaveTakenReport : POCBasePage, IfrmLeaveTakenReportView
    {
		private frmLeaveTakenReportPresenter _presenter;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this._presenter.OnViewInitialized();
                BindEmployee();
                BindProgram();
                GRVEmployeeList.DataSource = _presenter.ListEmployees(ddlEmployeeName.SelectedValue,Convert.ToInt32(ddlProgram.SelectedValue));
                GRVEmployeeList.DataBind();

            }
			this._presenter.OnViewLoaded();
		}

		[CreateNew]
        public frmLeaveTakenReportPresenter Presenter
		{
			get
			{
				return this._presenter;
			}
			set
			{
				if(value == null)
					throw new ArgumentNullException("value");

				this._presenter = value;
				this._presenter.View = this;
			}
		}
        public override string PageID
        {
            get
            {
                return "{A91F2C96-C8E9-492B-8D47-072946D3BA93}}";
            }
        }
        private void BindEmployee()
        {
            ddlEmployeeName.DataSource = _presenter.ListEmployees(string.Empty, Convert.ToInt32(ddlProgram.SelectedValue));
            ddlEmployeeName.DataBind();
        }
        private void BindProgram()
        {
            ddlProgram.DataSource = _presenter.GetPrograms();
            ddlProgram.DataBind();
        }
        protected void GRVEmployeeList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            GRVEmployeeList.PageIndex = e.NewPageIndex;
            GRVEmployeeList.DataSource = _presenter.ListEmployees(ddlEmployeeName.SelectedValue, Convert.ToInt32(ddlProgram.SelectedValue));
            GRVEmployeeList.DataBind();
        }
        protected void GRVEmployeeList_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            int tempcounter = 0;
            if (_presenter.ListEmployees(ddlEmployeeName.SelectedValue, Convert.ToInt32(ddlProgram.SelectedValue)) != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    Employee emp = e.Row.DataItem as Employee;
                    e.Row.Cells[5].Text = (Math.Round((emp.EmployeeLeaveBalanceYE() - _presenter.EmpLeaveTaken(emp.Id, emp.LeaveSettingDate.Value)) * 2, MidpointRounding.AwayFromZero) / 2).ToString();
                    e.Row.Cells[4].Text = emp.GetActiveContract() != null ? (Math.Round((emp.EmployeeLeaveBalanceCED(emp.GetActiveContract().ContractEndDate) - _presenter.EmpLeaveTaken(emp.Id, emp.LeaveSettingDate.Value)) * 2, MidpointRounding.AwayFromZero) / 2).ToString() : "";
                    e.Row.Cells[3].Text = (Math.Round((emp.EmployeeLeaveBalance() - _presenter.EmpLeaveTaken(emp.Id, emp.LeaveSettingDate.Value)) * 2, MidpointRounding.AwayFromZero) / 2).ToString();
                    e.Row.Cells[2].Text = _presenter.EmpLeaveTaken(emp.Id, emp.LeaveSettingDate.Value).ToString();
                    tempcounter = tempcounter + 1;
                    if (tempcounter == 10)
                    {
                        e.Row.Attributes.Add("style", "page-break-after: always;");
                        tempcounter = 0;
                    }
                }

               
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            GRVEmployeeList.DataSource = _presenter.ListEmployees(ddlEmployeeName.SelectedValue, Convert.ToInt32(ddlProgram.SelectedValue));
            GRVEmployeeList.DataBind();
        }
         protected void chkdisable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkdisable.Checked == true)
            {
                GRVEmployeeList.AllowPaging = false;
                GRVEmployeeList.DataSource = _presenter.ListEmployees(ddlEmployeeName.SelectedValue, Convert.ToInt32(ddlProgram.SelectedValue));
                GRVEmployeeList.DataBind();
                GRVEmployeeList.UseAccessibleHeader = true;
                GRVEmployeeList.HeaderRow.TableSection = TableRowSection.TableHeader;

            }
        }
    }
}

