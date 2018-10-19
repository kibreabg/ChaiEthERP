using System;
using Microsoft.Practices.ObjectBuilder;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.HRM;

namespace Chai.WorkflowManagment.Modules.HRM.Views
{
	public partial class EmployeeList : POCBasePage, IEmployeeListView
    {
		private EmployeeListPresenter _presenter;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this._presenter.OnViewInitialized();
            GRVEmployeeList.DataSource = _presenter.ListEmployees(txtSrchEmpNo.Text, txtSrchSrchFullName.Text, int.Parse(ddlSrchSrchProgram.SelectedValue));
            GRVEmployeeList.DataBind();
        }
            this._presenter.OnViewLoaded();

        }

		[CreateNew]
		public EmployeeListPresenter Presenter
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
                return "{F5FE9AB4-0AF8-432F-92B4-DFA2EEECE42B}";
            }
        }


        protected void GRVEmployeeList_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (_presenter.ListEmployees(txtSrchEmpNo.Text, txtSrchSrchFullName.Text, int.Parse(ddlSrchSrchProgram.SelectedValue))!= null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    Employee emp = e.Row.DataItem as Employee;
                    e.Row.Cells[2].Text = emp.GetEmployeeProgram();
                    e.Row.Cells[3].Text = emp.GetEmployeePosition();
                    decimal balance = Convert.ToInt32(emp.EmployeeLeaveBalance()) - _presenter.EmpLeaveTaken(emp.Id, emp.LeaveSettingDate.Value);
                    e.Row.Cells[4].Text = emp.AppUser.HiredDate.ToString();
                    e.Row.Cells[5].Text = balance.ToString();
                    decimal balanceYE = Convert.ToInt32(emp.EmployeeLeaveBalanceYE() - _presenter.EmpLeaveTaken(emp.Id, emp.LeaveSettingDate.Value));
                    e.Row.Cells[7].Text = balanceYE.ToString();
                    if (txtContractEndDate.Text != "")
                    {

                        decimal balanceCED = Convert.ToInt32(emp.EmployeeLeaveBalanceCED(Convert.ToDateTime(txtContractEndDate.Text))) - _presenter.EmpLeaveTaken(emp.Id, emp.LeaveSettingDate.Value);
                        e.Row.Cells[6].Text = balanceCED.ToString();
                    }
        }

            }
        }

        protected void GRVEmployeeList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            GRVEmployeeList.PageIndex = e.NewPageIndex;
            GRVEmployeeList.DataSource = _presenter.ListEmployees(txtSrchEmpNo.Text, txtSrchSrchFullName.Text, int.Parse(ddlSrchSrchProgram.SelectedValue)); ;
            GRVEmployeeList.DataBind();
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            GRVEmployeeList.DataSource = _presenter.ListEmployees(txtSrchEmpNo.Text, txtSrchSrchFullName.Text, int.Parse(ddlSrchSrchProgram.SelectedValue));
            GRVEmployeeList.DataBind();
        }

        protected void ddlAction1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            GridViewRow grdRow = GRVEmployeeList.SelectedRow;

            DropDownList ddlAction = row.FindControl("ddlAction1") as DropDownList;
            int index = GRVEmployeeList.SelectedIndex;
            if (ddlAction.SelectedItem.Text == "Manage HR")
                Response.Redirect(String.Format("~/HRM/frmManageHR.aspx?{0}=6&GetId={1}", AppConstants.TABID, GRVEmployeeList.DataKeys[row.RowIndex].Values[0]));
            else if (ddlAction.SelectedItem.Text == "Preview")
                Response.Redirect(String.Format("~/HRM/frmEmployeeProfile.aspx?{0}=6&EmpId={1}", AppConstants.TABID, GRVEmployeeList.DataKeys[row.RowIndex].Values[0]));
                
        }

        protected void txtContractEndDate_TextChanged(object sender, EventArgs e)
        {
            GRVEmployeeList.DataSource = _presenter.ListEmployees(txtSrchEmpNo.Text, txtSrchSrchFullName.Text, int.Parse(ddlSrchSrchProgram.SelectedValue));
            GRVEmployeeList.DataBind();
        }
    }
}

