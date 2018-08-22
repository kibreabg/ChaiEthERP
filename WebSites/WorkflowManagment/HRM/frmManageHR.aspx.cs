using System;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.CoreDomain.HRM;
using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Web.UI.WebControls;

namespace Chai.WorkflowManagment.Modules.HRM.Views
{
   
    public partial class frmManageHR : POCBasePage, IManageHRView
    {
		private ManageHRPresenter _presenter;
      
        protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this._presenter.OnViewInitialized();

                BindEmployee();
                
                BindEmployeeDetail();
                BindWarning();
                BindTermination();
                BindPosition();
                BindProgram();
                BindReportsTo();
                BindSupervisor();
                BindJobTitle();
                
            }
			this._presenter.OnViewLoaded();
            BindContracts();
            BindEmployeeDetail();
            BindWarning();
            BindTermination();
        }

        [CreateNew]
		public ManageHRPresenter Presenter
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
                return "{F5FE9AB4-0AF8-432E-92B4-DFA2DDACE42B}";
            }
        }

        public int GetId
        {
            get
            {              
                    if (Request.QueryString["GetId"] != "")

                        return Convert.ToInt32(Request.QueryString["GetId"]);
                    else
                        return 0;
                 }
        }

        public string Edit
        {
            get
            {
                if (Request.QueryString["Edit"] != "")
                    return Convert.ToString(Request.QueryString["Edit"]);
                else
                    return "";
            }
        }

      
      
        private void AddContracts()
        {
            var StartDate = txtStartDate.Text != "" ? txtStartDate.Text : "";
            var EndDate = txtEndDate.Text != "" ? txtEndDate.Text : " ";
            Contract cont = new Contract();
            cont.ContractStartDate = Convert.ToDateTime(StartDate);
            cont.ContractEndDate = Convert.ToDateTime(EndDate);
            cont.Reason = ddlReason.Text;
            cont.Status = ddlStatus.Text;
            _presenter.CurrentEmployee.Contracts.Add(cont);
            dgContractDetail.EditIndex = -1;
            dgContractDetail.DataSource = _presenter.CurrentEmployee.Contracts;
            dgContractDetail.DataBind();
        }

        private void BindContracts()
        {
           
                dgContractDetail.DataSource = _presenter.CurrentEmployee.Contracts;
                dgContractDetail.DataBind();
            
        }

      

        protected void dgContractDetail_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            
        }

      
      

       


        protected void btnSave_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (_presenter.CurrentEmployee.Contracts.Count != 0 || _presenter.CurrentEmployee.EmployeeDetails.Count != 0 || _presenter.CurrentEmployee.Warnings.Count != 0 || _presenter.CurrentEmployee.Terminations.Count != 0)
            //    {



            //  ddlBranch.SelectedValue = _presenter.CurrentBranchActivity.Branch.Id.ToString();
            // _presenter.CurrentBranchActivity.Branch = _presenter.GetBranch(Convert.ToInt32(ddlBranch.SelectedValue));
                    
                    _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);


                    Master.ShowMessage(new AppMessage("Saved Successfully ", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    

            //    }

                
            //}
            //catch (Exception ex)
            //{
            //    Master.ShowMessage(new AppMessage("No Data Inserted? Please insert Data ", Chai.WorkflowManagment.Enums.RMessageType.Info));
            //}

        }

        private void BindEmployee()
        {
            _presenter.CurrentEmployee.Id =1;
            


        }

        



        public void AddEmployeeDetail()
        {
            EmployeeDetail empdetail = new EmployeeDetail();

            empdetail.JobTitle = _presenter.GetJobTitle(Convert.ToInt32(ddlJobTitle.Text));
            empdetail.Position = _presenter.GetEmployeePosition(Convert.ToInt32(ddlPosition.Text));
            empdetail.Program =_presenter.GetProgram(Convert.ToInt32(ddlProgram.Text));
            empdetail.Employee = _presenter.CurrentEmployee;
            empdetail.DutyStationId = Convert.ToInt32(ddlDutyStation.Text);
            empdetail.DescriptiveJobTitle = txtDescJT.Text;
            empdetail.Salary =Convert.ToDecimal(txtSalary.Text);
            empdetail.HoursPerWeek = txtHoursPerWeek.Text;
            empdetail.BaseCountry = txtBaseCount.Text;
            empdetail.BaseCity = txtBaseCity.Text;
            empdetail.BaseState = txtBaseState.Text;
            empdetail.ClassId = Convert.ToInt32(txtClass.Text);
          
            empdetail.EmploymentStatusId = Convert.ToInt32(txtEmployeeStatus.Text);
            
            empdetail.SupervisorId = Convert.ToInt32(ddlSuperVisor.Text);
            empdetail.ReportsTo = Convert.ToInt32(ddlReportsTo.Text);
            _presenter.CurrentEmployee.EmployeeDetails.Add(empdetail);
           

            dgChange.DataSource = _presenter.CurrentEmployee.EmployeeDetails;
            dgChange.DataBind();
        }


        public void BindEmployeeDetail()
        {
            dgChange.DataSource = _presenter.CurrentEmployee.EmployeeDetails;
            dgChange.DataBind();
        }
        public void AddWarning()
        {
            Warning warn = new Warning();
            warn.WarningDate = Convert.ToDateTime(txtWarningDate);
            warn.WarningDescription = txtWarDesc.Text;
            warn.Employee = _presenter.CurrentEmployee;
            _presenter.CurrentEmployee.Warnings.Add(warn);
          
            dgWarning.DataSource = _presenter.CurrentEmployee.Warnings;
            dgWarning.DataBind();
        }

     public void BindWarning()
        {

            dgWarning.DataSource = _presenter.CurrentEmployee.Warnings;
            dgWarning.DataBind();
        }

    

        //protected void ddlAction_SelectedIndexChanged2(object sender, EventArgs e)
        //{

        //    DropDownList ddl = (DropDownList)sender;
        //    GridViewRow row = (GridViewRow)ddl.NamingContainer;
        //    GridViewRow grdRow = dgChange.SelectedRow;

        //    DropDownList ddlAction = row.FindControl("ddlAction") as DropDownList;
        //    int index = dgChange.SelectedIndex;
        //    if (ddlAction.SelectedItem.Value == "Edit")
        //        Response.Redirect(String.Format("~/HRM/frmManageHR.aspx?{0}=1&GetId={1}&Edit={2}", AppConstants.TABID, dgChange.DataKeys[row.RowIndex].Values[0], "Edit"));

        //}

        //protected void ddlAction_SelectedIndexChanged3(object sender, EventArgs e)
        //{
        //    DropDownList ddl = (DropDownList)sender;
        //    GridViewRow row = (GridViewRow)ddl.NamingContainer;
        //    GridViewRow grdRow = dgWarning.SelectedRow;

        //    DropDownList ddlAction = row.FindControl("ddlAction") as DropDownList;
        //    int index = dgWarning.SelectedIndex;
        //    if (ddlAction.SelectedItem.Value == "Edit")
        //        Response.Redirect(String.Format("~/HRM/frmManageHR.aspx?{0}=1&GetId={1}&Edit={2}", AppConstants.TABID, dgWarning.DataKeys[row.RowIndex].Values[0], "Edit"));
        //}

       

        //protected void ddlAction_SelectedIndexChanged4(object sender, EventArgs e)
        //{
        //    DropDownList ddl = (DropDownList)sender;
        //    GridViewRow row = (GridViewRow)ddl.NamingContainer;
        //    GridViewRow grdRow = dgTermination.SelectedRow;

        //    DropDownList ddlAction = row.FindControl("ddlAction") as DropDownList;
        //    int index = dgTermination.SelectedIndex;
        //    if (ddlAction.SelectedItem.Value == "Edit")
        //        Response.Redirect(String.Format("~/HRM/frmManageHR.aspx?{0}=1&GetId={1}&Edit={2}", AppConstants.TABID, dgTermination.DataKeys[row.RowIndex].Values[0], "Edit"));
        //}

       public void AddTermination()
        {
            Termination term = new Termination();
            term.TerminationDate = Convert.ToDateTime(txtTerminationDate.Text);
            term.LastDateOfEmployee = Convert.ToDateTime(txtLastDate.Text);
            term.Employee = _presenter.CurrentEmployee;
            term.ReccomendationForRehire = txtReccomendation.Text;
            
            _presenter.CurrentEmployee.Terminations.Add(term);
            dgTermination.EditIndex = -1;
            dgTermination.DataSource = _presenter.CurrentEmployee.Terminations;
            dgTermination.DataBind();
            // term.TerminationReason = _presenter.GetTerminationReason(Convert.ToInt32(txtTerminationReason.Text));
        }


        public void BindTermination()
        {
            dgTermination.DataSource = _presenter.CurrentEmployee.Terminations;
            dgTermination.DataBind();
        }

       private void BindProgram()
                    
        {
            ddlProgram.Items.Clear();

            ListItem lst = new ListItem();
            lst.Text = "Select Program";
            lst.Value = "0";
            ddlProgram.Items.Add(lst);
            ddlProgram.AppendDataBoundItems = true;



            ddlProgram.DataSource = _presenter.GetPrograms();
            ddlProgram.DataValueField = "Id";
            ddlProgram.DataTextField = "ProgramName";
            ddlProgram.DataBind();
        }

        private void BindJobTitle()

        {
            ddlJobTitle.Items.Clear();

            ListItem lst = new ListItem();
            lst.Text = "Select Job Title ";
            
            lst.Value = "0";
            ddlJobTitle.Items.Add(lst);
            ddlJobTitle.AppendDataBoundItems = true;



            ddlJobTitle.DataSource = _presenter.GetJobTitle();
            ddlJobTitle.DataValueField = "Id";
            ddlJobTitle.DataTextField = "JobTitleName";
            ddlJobTitle.DataBind();
        }
        private void BindPosition()

        {
            ddlPosition.Items.Clear();

            ListItem lst = new ListItem();
            lst.Text = "Select Position";
            lst.Value = "0";
            ddlPosition.Items.Add(lst);
            ddlPosition.AppendDataBoundItems = true;



            ddlPosition.DataSource = _presenter.GetEmployeePositions();
            ddlPosition.DataValueField = "Id";
            ddlPosition.DataTextField = "PositionName";
            ddlPosition.DataBind();
        }
        private void BindSupervisor()

        {
            ddlSuperVisor.Items.Clear();

            ListItem lst = new ListItem();
            lst.Text = "Select Supervisor";
            lst.Value = "0";
            ddlSuperVisor.Items.Add(lst);
            ddlSuperVisor.AppendDataBoundItems = true;



            ddlSuperVisor.DataSource = _presenter.GetEmployees();
            ddlSuperVisor.DataValueField = "Id";
            ddlSuperVisor.DataTextField = "FullName";
            ddlSuperVisor.DataBind();
        }
        private void BindReportsTo()

        {
            ddlReportsTo.Items.Clear();

            ListItem lst = new ListItem();
            lst.Text = "Select Reports To";
            lst.Value = "0";
            ddlReportsTo.Items.Add(lst);
            ddlReportsTo.AppendDataBoundItems = true;


            
            ddlReportsTo.DataSource = _presenter.GetEmployees();
            ddlReportsTo.DataValueField = "Id";
            ddlReportsTo.DataTextField = "FullName";
            ddlReportsTo.DataBind();
        }


        protected void btnAddTerm_Click(object sender, EventArgs e)
        {
            AddTermination();
            _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
        }

        protected void btnAddWarning_Click(object sender, EventArgs e)
        {
            AddWarning();
            _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
        }

        protected void btnAddChange_Click(object sender, EventArgs e)
        {
            AddEmployeeDetail();
            _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
        }

        protected void btnAddcontract_Click(object sender, EventArgs e)
        {
            AddContracts();
            _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
        }

        protected void dgContractDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            int id = Convert.ToInt32(dgContractDetail.SelectedDataKey[0]);
            
            Contract cont = _presenter.CurrentEmployee.GetContract(id);
            txtStartDate.Text=cont.ContractStartDate.ToShortDateString();
            txtEndDate.Text = cont.ContractStartDate.ToShortDateString();
          
           ddlReason.SelectedValue = cont.Reason;
           ddlStatus.Text= cont.Status;
            _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
            BindContracts();
        }

        protected void dgChange_EditCommand(object source, DataGridCommandEventArgs e)
        {

        }

        protected void dgChange_SelectedIndexChanged(object sender, EventArgs e)
        {
          //int id = Convert.ToInt32(dgChange.sSelectedIndex[0]);

          //  Contract cont = _presenter.CurrentEmployee.GetContract(id);
          //  txtStartDate.Text = cont.ContractStartDate.ToShortDateString();
          //  txtEndDate.Text = cont.ContractStartDate.ToShortDateString();

          //  ddlReason.SelectedValue = cont.Reason;
          //  ddlStatus.Text = cont.Status;
          //  _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
          //  BindContracts();*/
        }
    }





}

