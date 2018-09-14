using System;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.CoreDomain.HRM;
using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Web.UI.WebControls;
//using Microsoft.Office.Interop.Word;
//using Microsoft.Office.Core;
using System.Reflection;
//using Word = Microsoft.Office.Interop;
using System.IO;
using System.Diagnostics;
using System.Drawing.Drawing2D;


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
            if (btnAddcontract.Text == "Update Contract")
            {
                var id = Convert.ToInt32(dgContractDetail.SelectedDataKey[0]) != 0 ? Convert.ToInt32(dgContractDetail.SelectedDataKey[0]) : 0;

                if (id != 0)
                {

                    var StartDate = txtStartDate.Text != "" ? txtStartDate.Text : "";
                    var EndDate = txtEndDate.Text != "" ? txtEndDate.Text : " ";

                    Contract cont = _presenter.CurrentEmployee.GetContract(id);

                    cont.ContractStartDate = Convert.ToDateTime(StartDate);
                    cont.ContractEndDate = Convert.ToDateTime(EndDate);
                    cont.Reason = ddlReason.Text;
                    cont.Status = ddlStatus.Text;
                    _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
                    dgContractDetail.EditIndex = -1;
                    dgContractDetail.DataSource = _presenter.CurrentEmployee.Contracts;
                    dgContractDetail.DataBind();
                }
                
            }

            else if (btnAddcontract.Text == "Add Contract")
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
            
        }


       



        private void ClearEmpDetailFormFields()
        {
            ddlJobTitle.SelectedValue = "0";
            ddlPosition.SelectedValue = "0";
            ddlProgram.SelectedValue = "0";
            ddlDutyStation.SelectedValue = string.Empty;
            txtDescJT.Text = string.Empty;
            txtSalary.Text = string.Empty;
            txtHoursPerWeek.Text = string.Empty;
            txtBaseCount.Text = string.Empty;
            txtBaseCity.Text = string.Empty;
            txtBaseState.Text = string.Empty;
            txtClass.Text = string.Empty;

            txtEmployeeStatus.Text = string.Empty;
            ddlSuperVisor.SelectedValue = "0";
            ddlReportsTo.SelectedValue = "0";
        }
        private void ClearContractFormFields()
        {
            txtStartDate.Text = string.Empty;
            txtEndDate.Text = string.Empty;

            ddlReason.SelectedValue = string.Empty;
            ddlStatus.SelectedValue = string.Empty;
        }

        private void ClearTerminationFormFields()
        {
            txtTerminationDate.Text = string.Empty;
            txtLastDate.Text = string.Empty;
            ddlRecommendation.SelectedValue = string.Empty;
            txtTerminationReason.Text = string.Empty;
        }
        private void BindContracts()
        {
            if (_presenter.CurrentEmployee.Contracts.Count != 0)
            {
                if (_presenter.CurrentEmployee.GetContract(_presenter.GetLastContrcatId()).Reason == "New Hire")
                {
                    ddlReason.Items.FindByValue("New Hire").Attributes.Add("Disabled", "Disabled");
                }
                dgContractDetail.DataSource = _presenter.CurrentEmployee.Contracts;
                dgContractDetail.DataBind();
            }
            else
            {
                dgContractDetail.DataSource = _presenter.CurrentEmployee.Contracts;
                dgContractDetail.DataBind();
            }
        }



        protected void dgContractDetail_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {

        }


        private void PrintTransaction()
        {
            //lblEmployeeMainIDRes.Text=_presenter.CurrentEmployee.AppUser.EmployeeNo;
            lblFirstNameResult.Text = _presenter.CurrentEmployee.FirstName;
            lbliddleNameResult.Text = _presenter.CurrentEmployee.LastName;
            lblLastNameResult.Text = _presenter.CurrentEmployee.LastName;
            lblEmailResult.Text = _presenter.CurrentEmployee.ChaiEMail;
            lblBaseCityCurr.Text = _presenter.CurrentEmployee.GetEmployeeDetails(_presenter.GetLastEmployeeDetailId()).BaseCity.ToString();
            lblBaseCityChange.Text = txtBaseCity.Text;
            lblBaseCountryCurr.Text = _presenter.CurrentEmployee.GetEmployeeDetails(_presenter.GetLastEmployeeDetailId()).BaseCountry.ToString();
            lblBaseCountryChange.Text = txtBaseCount.Text;
            lblBaseStateCurr.Text = _presenter.CurrentEmployee.GetEmployeeDetails(_presenter.GetLastEmployeeDetailId()).BaseState.ToString();
            lblBaseStateChange.Text = txtBaseState.Text;
            lblClassCurr.Text = _presenter.CurrentEmployee.GetEmployeeDetails(_presenter.GetLastEmployeeDetailId()).Class.ToString();
            lblClassChange.Text = txtClass.Text;
            lblCountryTeamCurr.Text = _presenter.CurrentEmployee.GetEmployeeDetails(_presenter.GetLastEmployeeDetailId()).CountryTeam.ToString();
            lblCountryTeamChange.Text = txtCountryTeam.Text;
            lblDescJobCurr.Text = _presenter.CurrentEmployee.GetEmployeeDetails(_presenter.GetLastEmployeeDetailId()).DescriptiveJobTitle.ToString();
            lblDescJobChange.Text = txtDescJT.Text;
            // lblDurationOfCont.Text = Convert.ToDateTime((_presenter.CurrentEmployee.GetContract(_presenter.GetLastEmployeeDetailId()).ContractEndDate - _presenter.CurrentEmployee.GetContract(_presenter.GetLastEmployeeDetailId()).ContractEndDate)).Month.ToString()+"Month";
            lblEffectiveDateRes.Text = txtStartDate.Text;
            lblAnnualBaseSalaryCurr.Text = (12 * (_presenter.CurrentEmployee.GetEmployeeDetails(_presenter.GetLastEmployeeDetailId()).Salary)).ToString();
            lblAnnualBaseSalaryChange.Text = (12 * Convert.ToDecimal(txtSalary.Text)).ToString();
            lblEmpManCurr.Text = _presenter.CurrentEmployee.GetEmployeeDetails(_presenter.GetLastEmployeeDetailId()).Supervisor.ToString();
            lblEmpManChange.Text = ddlSuperVisor.Text;
           
        }
        private void PrintTransactionnew()
        {
            //lblEmployeeMainIDRes.Text=_presenter.CurrentEmployee.AppUser.EmployeeNo;
            lblFirstNameResultnew.Text = _presenter.CurrentEmployee.FirstName;
            lbliddleNameResultnew.Text = _presenter.CurrentEmployee.LastName;
            lblLastNameResultnew.Text = _presenter.CurrentEmployee.LastName;
            lblEmailResultnew.Text = _presenter.CurrentEmployee.ChaiEMail;
            lblBaseCitynew.Text = txtBaseCity.Text;
           lblBasCountrynew.Text = txtBaseCount.Text;
            lblBaseStatenew.Text = txtBaseState.Text;
            lblClassnew.Text = txtClass.Text;
            lblCountryTeamnew.Text = txtCountryTeam.Text;
            lblDescJobnew.Text = txtDescJT.Text;
            // lblDurationOfCont.Text = Convert.ToDateTime((_presenter.CurrentEmployee.GetContract(_presenter.GetLastEmployeeDetailId()).ContractEndDate - _presenter.CurrentEmployee.GetContract(_presenter.GetLastEmployeeDetailId()).ContractEndDate)).Month.ToString()+"Month";
            lblEffectiveDateRes.Text = txtStartDate.Text;
            lblAnnualBaseSalaryres.Text = (12 * Convert.ToDecimal(txtSalary.Text)).ToString();
            lblEmpManres.Text = ddlSuperVisor.Text;
           
        }
        private void PrintTransactionTermination()
        {

            lblFirstNameResultTer.Text = _presenter.CurrentEmployee.FirstName;
            lbliddleNameResultTer.Text = _presenter.CurrentEmployee.LastName;
            lblLastNameResultTer.Text = _presenter.CurrentEmployee.LastName;
            lblEmailResultTer.Text = _presenter.CurrentEmployee.ChaiEMail;
            lblBasCountryResTer.Text = _presenter.CurrentEmployee.Country;

            lblClassResTer.Text = _presenter.CurrentEmployee.GetEmployeeDetails(_presenter.GetLastEmployeeDetailId()).Class.ToString();

            
            // lblDurationOfCont.Text = Convert.ToDateTime((_presenter.CurrentEmployee.GetContract(_presenter.GetLastEmployeeDetailId()).ContractEndDate - _presenter.CurrentEmployee.GetContract(_presenter.GetLastEmployeeDetailId()).ContractEndDate)).Month.ToString()+"Month";


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
            
                txtFirstName.Text = _presenter.CurrentEmployee.FirstName;
                txtLastName.Text = _presenter.CurrentEmployee.LastName;
                ddlGender.Text = _presenter.CurrentEmployee.Gender;
                txtDOB.Text = Convert.ToDateTime(_presenter.CurrentEmployee.DateOfBirth).ToShortDateString();



           

        }



        public void AddEmployeeDetail()
        {
            if (btnAddChange.Text == "Update Change")
            {
                var id = Convert.ToInt32(dgChange.SelectedDataKey[0]) != 0 ? Convert.ToInt32(dgChange.SelectedDataKey[0]) : 0;
                if (id != 0)
                {
                    EmployeeDetail empdetail = _presenter.CurrentEmployee.GetEmployeeDetails(id);
                    empdetail.JobTitle = _presenter.GetJobTitle(Convert.ToInt32(ddlJobTitle.Text));
                    empdetail.Position = _presenter.GetEmployeePosition(Convert.ToInt32(ddlPosition.Text));
                    empdetail.Program = _presenter.GetProgram(Convert.ToInt32(ddlProgram.Text));
                    empdetail.Employee = _presenter.CurrentEmployee;
                    empdetail.DutyStation = ddlDutyStation.Text;
                    empdetail.DescriptiveJobTitle = txtDescJT.Text;
                    empdetail.Salary = Convert.ToDecimal(txtSalary.Text);
                    empdetail.HoursPerWeek = txtHoursPerWeek.Text;
                    empdetail.BaseCountry = txtBaseCount.Text;
                    empdetail.BaseCity = txtBaseCity.Text;
                    empdetail.BaseState = txtBaseState.Text;
                    empdetail.Class = Convert.ToInt32(txtClass.Text);
                    empdetail.CountryTeam = txtCountryTeam.Text;
                    empdetail.EmploymentStatus = Convert.ToInt32(txtEmployeeStatus.Text);

                    empdetail.Supervisor = Convert.ToInt32(ddlSuperVisor.Text);
                    empdetail.ReportsTo = Convert.ToInt32(ddlReportsTo.Text);


                    _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);

                    dgChange.DataSource = _presenter.CurrentEmployee.EmployeeDetails;
                    dgChange.DataBind();

                }
                if (_presenter.CurrentEmployee.GetContract(_presenter.GetLastContrcatId()).Reason == "New Hire")
                {
                   
                    PrintTransactionnew();
                    
                        
            
                   
                }
                else if (_presenter.CurrentEmployee.GetContract(_presenter.GetLastContrcatId()).Reason == "Renewal")
                {
                   
                    PrintTransaction();

                    
                }
                
            }

            else if (btnAddChange.Text == "Add Change")
            {



                EmployeeDetail empdetail = new EmployeeDetail();

                empdetail.JobTitle = _presenter.GetJobTitle(Convert.ToInt32(ddlJobTitle.Text));
                empdetail.Position = _presenter.GetEmployeePosition(Convert.ToInt32(ddlPosition.Text));
                empdetail.Program = _presenter.GetProgram(Convert.ToInt32(ddlProgram.Text));
                empdetail.Employee = _presenter.CurrentEmployee;
                empdetail.DutyStation = ddlDutyStation.Text;
                empdetail.DescriptiveJobTitle = txtDescJT.Text;
                empdetail.Salary = Convert.ToDecimal(txtSalary.Text);
                empdetail.HoursPerWeek = txtHoursPerWeek.Text;
                empdetail.BaseCountry = txtBaseCount.Text;
                empdetail.BaseCity = txtBaseCity.Text;
                empdetail.BaseState = txtBaseState.Text;
                empdetail.Class = Convert.ToInt32(txtClass.Text);
                empdetail.CountryTeam = txtCountryTeam.Text;
                empdetail.EmploymentStatus = Convert.ToInt32(txtEmployeeStatus.Text);

                empdetail.Supervisor = Convert.ToInt32(ddlSuperVisor.Text);
                empdetail.ReportsTo = Convert.ToInt32(ddlReportsTo.Text);

                _presenter.CurrentEmployee.EmployeeDetails.Add(empdetail);


                dgChange.DataSource = _presenter.CurrentEmployee.EmployeeDetails;
                dgChange.DataBind();

           
            if (_presenter.CurrentEmployee.GetContract(_presenter.GetLastContrcatId()).Reason == "New Hire")
            {
                    
                    PrintTransactionnew();
                
            }
            else if (_presenter.CurrentEmployee.GetContract(_presenter.GetLastContrcatId()).Reason == "Renewal")
            {
                   
                    PrintTransaction();
                
            }
            }
        }


        public void BindEmployeeDetail()
        {
            dgChange.DataSource = _presenter.CurrentEmployee.EmployeeDetails;
            dgChange.DataBind();
        }




        public void AddTermination()
        {
            if (btnAddTerm.Text == "Update Termination")
            {
                var id = Convert.ToInt32(dgTermination.SelectedDataKey[0]) != 0 ? Convert.ToInt32(dgTermination.SelectedDataKey[0]) : 0;
                if (id != 0)
                {
                    var TermDate = txtTerminationDate.Text != "" ? txtTerminationDate.Text : "";
                    var LastDate = txtLastDate.Text != "" ? txtLastDate.Text : " ";
                    Termination term = _presenter.CurrentEmployee.GetTerminations(id);
                    term.TerminationDate = Convert.ToDateTime(TermDate);
                    term.LastDateOfEmployee = Convert.ToDateTime(LastDate);
                    term.Employee = _presenter.CurrentEmployee;
                    term.ReccomendationForRehire = ddlRecommendation.Text;

                    _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
                    dgTermination.EditIndex = -1;
                    dgTermination.DataSource = _presenter.CurrentEmployee.Terminations;
                    dgTermination.DataBind();
                    _presenter.CurrentEmployee.GetContract(id).Status = "In Active";
                    _presenter.CurrentEmployee.AppUser.IsActive = false;

                }
                PrintTransactionTermination();
                ClearTerminationFormFields();
            }

            else if (btnAddTerm.Text == "Add Termination")
            {
                var TermDate = txtTerminationDate.Text != "" ? txtTerminationDate.Text : "";
                var LastDate = txtLastDate.Text != "" ? txtLastDate.Text : " ";
                Termination term = new Termination();
                term.TerminationDate = Convert.ToDateTime(TermDate);
                term.LastDateOfEmployee = Convert.ToDateTime(LastDate);
                term.Employee = _presenter.CurrentEmployee;
                term.ReccomendationForRehire = ddlRecommendation.Text;
                term.Employee.GetContract(0).Status = "In Active";
                _presenter.CurrentEmployee.AppUser.IsActive = false;
                _presenter.CurrentEmployee.Terminations.Add(term);

                dgTermination.EditIndex = -1;
                dgTermination.DataSource = _presenter.CurrentEmployee.Terminations;
                dgTermination.DataBind();
            }
            PrintTransactionTermination();
            ClearTerminationFormFields();
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
            txtStartDate.Text = cont.ContractStartDate.ToShortDateString();
            txtEndDate.Text = cont.ContractEndDate.ToShortDateString();

            ddlReason.SelectedValue = cont.Reason;
            ddlStatus.SelectedValue = cont.Status;
            btnAddTerm.Text = "Update Contracts";





        }

        protected void dgChange_EditCommand(object source, DataGridCommandEventArgs e)
        {

        }

        protected void dgChange_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {

        }

        protected void dgChange_SelectedIndexChanged1(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgChange.SelectedDataKey[0]);

            EmployeeDetail empdetail = _presenter.CurrentEmployee.GetEmployeeDetails(id);
            ddlJobTitle.SelectedValue = empdetail.JobTitle.Id.ToString();
            ddlPosition.SelectedValue = empdetail.Position.Id.ToString();
            ddlProgram.SelectedValue = empdetail.Program.Id.ToString();
            ddlDutyStation.SelectedValue = empdetail.DutyStation;
            txtDescJT.Text = empdetail.DescriptiveJobTitle;
            txtSalary.Text = empdetail.Salary.ToString();
            txtHoursPerWeek.Text = empdetail.HoursPerWeek;
            txtBaseCount.Text = empdetail.BaseCountry;
            txtBaseCity.Text = empdetail.BaseCity;
            txtBaseState.Text = empdetail.BaseState;
            txtClass.Text = empdetail.Class.ToString();

            txtEmployeeStatus.Text = empdetail.EmploymentStatus.ToString();
            ddlSuperVisor.SelectedValue = empdetail.Supervisor.ToString();
            ddlReportsTo.SelectedValue = empdetail.ReportsTo.ToString();
            btnAddChange.Text = "Update Change";


        }





        protected void dgTermination_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgTermination.SelectedDataKey[0]);

            Termination termination = _presenter.CurrentEmployee.GetTerminations(id);

            txtTerminationDate.Text = termination.TerminationDate.ToShortDateString();
            txtLastDate.Text = termination.LastDateOfEmployee.ToShortDateString();
            ddlRecommendation.SelectedValue = termination.ReccomendationForRehire.ToString();
            txtTerminationReason.Text = termination.TerminationReason;
            btnAddTerm.Text = "Update Termination";

        }







        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkDelete = sender as LinkButton;
                GridViewRow gvrow = lnkDelete.NamingContainer as GridViewRow;
                int index = gvrow.RowIndex;
                int id = Convert.ToInt32(dgChange.SelectedDataKey[index]);
                _presenter.CurrentEmployee.RemoveContract(id);
                    _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
                    dgContractDetail.DataSource = _presenter.CurrentEmployee.Contracts;
                    dgContractDetail.DataBind();
                
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Contratcs. " + ex.Message, Enums.RMessageType.Error));
            }
        }

        protected void lnkDeleteemp_Click(object sender, EventArgs e)
        {
            try
            {
          
                //   int id = Convert.ToInt32(dgChange.SelectedDataKey[0]);
                //int id = Convert.ToInt32(dgChange.SelectedRow.Cells[0].ID);
                LinkButton lnkDeleteemp = sender as LinkButton;
                GridViewRow gvrow = lnkDeleteemp.NamingContainer as GridViewRow;
                int index = gvrow.RowIndex;
                int id = Convert.ToInt32(dgChange.SelectedDataKey[index]);
                _presenter.CurrentEmployee.RemoveEmployeeDetail(id);
                    _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
                    dgChange.DataSource = _presenter.CurrentEmployee.EmployeeDetails;
                    dgChange.DataBind();
                
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Employee History. " + ex.Message, Enums.RMessageType.Error));
            }
        }

        protected void lnkDeleteter_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkDeleteter = sender as LinkButton;
                GridViewRow gvrow = lnkDeleteter.NamingContainer as GridViewRow;
                int index = gvrow.RowIndex;
                int id = Convert.ToInt32(dgChange.SelectedDataKey[index]);
                _presenter.CurrentEmployee.RemoveTermination(id);
                    _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
                    dgTermination.DataSource = _presenter.CurrentEmployee.Terminations;
                    dgTermination.DataBind();
               
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Employee History. " + ex.Message, Enums.RMessageType.Error));
            }
        }
    }





}

