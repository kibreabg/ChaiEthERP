using System;
using Microsoft.Practices.ObjectBuilder;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.Shared;

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
			}
			this._presenter.OnViewLoaded();
            GRVEmployeeList.DataSource = _presenter.ListEmployees(txtSrchEmpNo.Text, txtSrchSrchFullName.Text, int.Parse(ddlSrchSrchProgram.SelectedValue));
            GRVEmployeeList.DataBind();
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
                return "{F5FE9AB4-0AF8-432F-92B4-DFA2EEECE42B}";
            }
        }


        protected void GRVEmployeeList_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

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
        protected void ddlAction_SelectedIndexChanged1(object sender, EventArgs e)
        {

            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            GridViewRow grdRow = GRVEmployeeList.SelectedRow;

            DropDownList ddlAction = row.FindControl("ddlAction") as DropDownList;
            int index = GRVEmployeeList.SelectedIndex;
            if (ddlAction.SelectedItem.Text == "Manage HR")
                Response.Redirect(String.Format("~/HRM/frmManageHR.aspx?{0}=2&Id={1}", AppConstants.TABID, GRVEmployeeList.DataKeys[row.RowIndex].Values[0]));
            else if (ddlAction.SelectedItem.Text == "Preview")
                Response.Redirect(String.Format("~/HRM/frmEmployeeProfile.aspx?{0}=2&Id={1}", AppConstants.TABID, GRVEmployeeList.DataKeys[row.RowIndex].Values[0]));



        }
    }
}

