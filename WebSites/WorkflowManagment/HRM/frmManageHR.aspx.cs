using System;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.CoreDomain.HRM;
using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Web.UI.WebControls;
using Microsoft.Office.Interop.Word;
using System.Reflection;
using Word = Microsoft.Office.Interop;
using System.IO;
using System.Diagnostics;
using System.Drawing.Drawing2D;


namespace Chai.WorkflowManagment.Modules.HRM.Views
{

    public partial class frmManageHR : POCBasePage, IManageHRView
    {
        private ManageHRPresenter _presenter;


        private void FindAndReplace(Microsoft.Office.Interop.Word.Application wordApp, object findText, object replaceWithText)
        {
            object matchCase = true;
            object matchWholeWord = true;
            object matchWildCards = true;
            object matchSoundLike = true;
            object matchAllForms = true;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiactities = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;

            wordApp.Selection.Find.Execute(ref findText, ref matchCase, ref matchWholeWord, ref matchSoundLike, ref matchAllForms, ref forward, ref wrap, ref format, ref replaceWithText, ref replace, ref matchKashida, ref matchDiactities, ref matchAlefHamza, ref matchControl);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();

                BindEmployee();
                BindContracts();
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
            btnPAFNew.Visible = false;
            btnPAFNew.Visible = false;
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
            if (btnAddcontract.Text == "Update Contracts")
            {
                var id = Convert.ToInt32(dgContractDetail.SelectedDataKey[0]) != 0 ? Convert.ToInt32(dgContractDetail.SelectedDataKey[0]) : 0;

                if (id != 0)
                {

                    var StartDate = txtStartDate.Text != "" ? txtStartDate.Text : "";
                    var EndDate = txtEndDate.Text != "" ? txtEndDate.Text : " ";
                    Contract cont = _presenter.CurrentEmployee.GetContract(id);


                   // if (_presenter.CurrentEmployee.GetContract(_presenter.GetLastContrcatId()).ContractEndDate < Convert.ToDateTime(StartDate))
                   // {
                        cont.ContractStartDate = Convert.ToDateTime(StartDate);
                        cont.ContractEndDate = Convert.ToDateTime(EndDate);
                        cont.Reason = ddlReason.SelectedItem.Text;
                        cont.Status = ddlStatus.SelectedItem.Text;
                    _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
                    
                        dgContractDetail.EditIndex = -1;
                        dgContractDetail.DataSource = _presenter.CurrentEmployee.Contracts;
                        dgContractDetail.DataBind();
                        ClearContractFormFields();
                   // }
                   // else
                      //  Master.ShowMessage(new AppMessage("Current Contract Date must not be less than previous Contract Date ", Chai.WorkflowManagment.Enums.RMessageType.Info));
                }
            }
            else if (btnAddcontract.Text == "Add Contract")
            {


                var StartDate = txtStartDate.Text != "" ? txtStartDate.Text : "";
                var EndDate = txtEndDate.Text != "" ? txtEndDate.Text : " ";
                Contract cont = new Contract();

                if (_presenter.CurrentEmployee.GetContract(_presenter.GetLastContrcatId()).ContractEndDate < Convert.ToDateTime(StartDate))
                {
                    cont.ContractStartDate = Convert.ToDateTime(StartDate);
                    cont.ContractEndDate = Convert.ToDateTime(EndDate);
                    cont.Reason = ddlReason.SelectedItem.Text;
                    cont.Status = ddlStatus.SelectedItem.Text;
                    _presenter.CurrentEmployee.Contracts.Add(cont);
                    dgContractDetail.EditIndex = -1;
                    dgContractDetail.DataSource = _presenter.CurrentEmployee.Contracts;
                    dgContractDetail.DataBind();
                    ClearContractFormFields();
                }
                else
                    Master.ShowMessage(new AppMessage("Current Contract Date must not be less than previous Contract Date ", Chai.WorkflowManagment.Enums.RMessageType.Info));



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
            txtCountryTeam.Text = string.Empty;
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

            ddlReason.SelectedValue = "0";
            ddlStatus.SelectedValue = "0";
        }

        private void ClearTerminationFormFields()
        {
            txtTerminationDate.Text = string.Empty;
            txtLastDate.Text = string.Empty;
            ddlRecommendation.SelectedValue = string.Empty;
            txtTerminationReason.Text = string.Empty;
        }



        /*      if (_presenter.CurrentEmployee.GetContract(_presenter.GetLastContrcatId()).Reason == "New Hire")
                {


                        PrintTransactionnew();

        }
                else if (_presenter.CurrentEmployee.GetContract(_presenter.GetLastContrcatId()).Reason == "Renewal")
                {


                        PrintTransaction();

    }*/
        private void BindContracts()
        {
            if (_presenter.CurrentEmployee.Contracts.Count != 0)
            {

                if (_presenter.CurrentEmployee.GetContract(_presenter.GetLastContrcatId()).Reason == "New Hire" || _presenter.CurrentEmployee.GetContract(_presenter.GetLastContrcatId()).Reason == "Renewal" || _presenter.CurrentEmployee.GetContract(_presenter.GetLastContrcatId()).Reason == "Rehire")

                {
                    ddlReason.Items.FindByValue("New Hire").Attributes.Add("Disabled", "Disabled");
                    if (_presenter.CurrentEmployee.GetContract(_presenter.GetLastContrcatId()).Status == "In Active")
                    {

                        ddlReason.Items.FindByValue("Renewal").Text = "Rehire";
                        
                    }
                }
                

            }
            else
            {


              
            }
            dgContractDetail.DataSource = _presenter.CurrentEmployee.Contracts;
            dgContractDetail.DataBind();
           
        }

        public void BindEmployeeDetail()
        {
            if (_presenter.CurrentEmployee.GetContract(_presenter.GetLastContrcatId()).Reason == "New Hire")
            {
                btnPAFNew.Visible = true;
            }
            else{
                btnPAFChange.Visible = true;
            }
                dgChange.DataSource = _presenter.CurrentEmployee.EmployeeDetails;
            dgChange.DataBind();
        }


        protected void dgContractDetail_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {

        }


        private void PrintTransaction()
        {
            
            GridViewRow row = dgChange.Rows[dgChange.Rows.Count - 1];
            GridViewRow row1 = dgChange.Rows[dgChange.Rows.Count - 2];
          
            Int32 rowIndex = row.RowIndex;
            int change = Convert.ToInt32(dgChange.DataKeys[rowIndex].Value);
            Int32 rowIndex1 = row1.RowIndex;
            int current = Convert.ToInt32(dgChange.DataKeys[rowIndex1].Value);
            GridViewRow row2 = dgContractDetail.Rows[dgContractDetail.Rows.Count - 1];
           
             
            //lblEmployeeMainIDRes.Text=_presenter.CurrentEmployee.AppUser.EmployeeNo;
            lblFirstNameResult.Text = _presenter.CurrentEmployee.FirstName;
            lbliddleNameResult.Text = _presenter.CurrentEmployee.LastName;
            lblLastNameResult.Text = _presenter.CurrentEmployee.LastName;
            lblEmailResult.Text = _presenter.CurrentEmployee.ChaiEMail;
            lblBaseCityCurr.Text = _presenter.CurrentEmployee.GetEmployeeDetails(current).BaseCity.ToString();
            lblBaseCityChange.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).BaseCity.ToString();            
            lblBaseCountryCurr.Text = _presenter.CurrentEmployee.GetEmployeeDetails(current).BaseCountry.ToString();
            lblBaseCountryChange.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).BaseCountry.ToString();
            lblBaseStateCurr.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).BaseState.ToString();
            lblBaseStateChange.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).BaseState.ToString();
            lblClassCurr.Text = _presenter.CurrentEmployee.GetEmployeeDetails(current).Class.ToString();
            lblClassChange.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).Class.ToString(); 
            lblEmpStatCurr.Text= _presenter.CurrentEmployee.GetEmployeeDetails(current).EmploymentStatus.ToString();
            lblEmpStatChange.Text= _presenter.CurrentEmployee.GetEmployeeDetails(change).EmploymentStatus.ToString();
            lblCountryTeamCurr.Text = _presenter.CurrentEmployee.GetEmployeeDetails(current).CountryTeam.ToString();
            lblCountryTeamChange.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).CountryTeam.ToString();
            lblOffJobCurr.Text = _presenter.CurrentEmployee.GetEmployeeDetails(current).JobTitle.ToString();
            lblOffJobChange.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).JobTitle.ToString();
            lblDescJobCurr.Text = _presenter.CurrentEmployee.GetEmployeeDetails(current).DescriptiveJobTitle.ToString();
            lblDescJobChange.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).DescriptiveJobTitle.ToString();
            lblProgramCurr.Text = _presenter.CurrentEmployee.GetEmployeeDetails(current).Program.ProgramName.ToString();
            lblProgramChange.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).Program.ProgramName.ToString();
            if (row2.Cells[3].Text == "Active")
            {
                lblEffectiveDateRes.Text = _presenter.CurrentEmployee.GetContract(_presenter.GetLastContrcatId()).ContractStartDate.ToShortDateString();
                lblReasonRes.Text = _presenter.CurrentEmployee.GetContract(_presenter.GetLastContrcatId()).Reason.ToString();
            }
            // lblDurationOfCont.Text = Convert.ToDateTime((_presenter.CurrentEmployee.GetContract(_presenter.GetLastEmployeeDetailId()).ContractEndDate - _presenter.CurrentEmployee.GetContract(_presenter.GetLastEmployeeDetailId()).ContractEndDate)).Month.ToString()+"Month";
           
            lblAnnualBaseSalaryCurr.Text = (12 * (_presenter.CurrentEmployee.GetEmployeeDetails(current).Salary)).ToString();
            lblAnnualBaseSalaryChange.Text = (12 * (_presenter.CurrentEmployee.GetEmployeeDetails(change).Salary)).ToString();
            lblEmpManCurr.Text = _presenter.CurrentEmployee.GetEmployeeDetails(current).Supervisor.ToString();
            lblEmpManCurr.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).Supervisor.ToString();
            lblReporttoCurr.Text = _presenter.CurrentEmployee.GetEmployeeDetails(current).ReportsTo.ToString();
            lblReporttoChange.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).ReportsTo.ToString();


        }
        private void PrintTransactionnew()
        {

            GridViewRow row = dgChange.Rows[dgChange.Rows.Count - 1];
           

            Int32 rowIndex = row.RowIndex;
            int change = Convert.ToInt32(dgChange.DataKeys[rowIndex].Value);
           
            GridViewRow row2 = dgContractDetail.Rows[dgContractDetail.Rows.Count - 1];


            //lblEmployeeMainIDRes.Text=_presenter.CurrentEmployee.AppUser.EmployeeNo;
            lblFirstNameResult.Text = _presenter.CurrentEmployee.FirstName;
            lbliddleNameResult.Text = _presenter.CurrentEmployee.LastName;
            lblLastNameResult.Text = _presenter.CurrentEmployee.LastName;
            lblEmailResult.Text = _presenter.CurrentEmployee.ChaiEMail;
            lblBaseCityres.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).BaseCity.ToString();
            lblBaseCountryres.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).BaseCountry.ToString();
          
            lblBaseStateres.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).BaseState.ToString();
            lblClassres.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).Class.ToString();
            lblEmpStatRes.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).EmploymentStatus.ToString();
            lblCountryTeamres.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).CountryTeam.ToString();
            lblOffJobres.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).JobTitle.ToString();
            lblDescJobres.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).DescriptiveJobTitle.ToString();
            lblProgramres.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).Program.ProgramName.ToString();
            if (row2.Cells[3].Text == "Active")
            {
                lblEffectiveDateResnew.Text = _presenter.CurrentEmployee.GetContract(_presenter.GetLastContrcatId()).ContractStartDate.ToShortDateString();
                lblReasonResnew.Text = _presenter.CurrentEmployee.GetContract(_presenter.GetLastContrcatId()).Reason.ToString();
            }
            // lblDurationOfCont.Text = Convert.ToDateTime((_presenter.CurrentEmployee.GetContract(_presenter.GetLastEmployeeDetailId()).ContractEndDate - _presenter.CurrentEmployee.GetContract(_presenter.GetLastEmployeeDetailId()).ContractEndDate)).Month.ToString()+"Month";    
            lblAnnualBaseSalaryres.Text = (12 * (_presenter.CurrentEmployee.GetEmployeeDetails(change).Salary)).ToString();
            lblEmpManres.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).Supervisor.ToString();
            lblReporttores.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).ReportsTo.ToString();

        }
        private void PrintTransactionTermination()
        {

            lblFirstNameResultTer.Text = _presenter.CurrentEmployee.FirstName;
            lbliddleNameResultTer.Text = _presenter.CurrentEmployee.LastName;
            lblLastNameResultTer.Text = _presenter.CurrentEmployee.LastName;
            lblEmailResultTer.Text = _presenter.CurrentEmployee.ChaiEMail;
            lblBasCountryResTer.Text = _presenter.CurrentEmployee.Country;

            lblClassResTer.Text = _presenter.CurrentEmployee.GetEmployeeDetails(_presenter.GetLastEmployeeDetailId()).Class.ToString();

            ClearTerminationFormFields();
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
            imgProfilePic.ImageUrl = _presenter.CurrentEmployee.AppUser.Employee.Photo;
            txtFirstName.Text = _presenter.CurrentEmployee.FirstName;
            txtLastName.Text = _presenter.CurrentEmployee.LastName;
            txtempid.Text = _presenter.CurrentEmployee.AppUser.EmployeeNo;





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
                    empdetail.Class = txtClass.Text;
                    empdetail.CountryTeam = txtCountryTeam.Text;
                    empdetail.EmploymentStatus = Convert.ToInt32(txtEmployeeStatus.Text);

                    empdetail.Supervisor = Convert.ToInt32(ddlSuperVisor.Text);
                    empdetail.ReportsTo = Convert.ToInt32(ddlReportsTo.Text);


                    _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);

                    dgChange.DataSource = _presenter.CurrentEmployee.EmployeeDetails;
                    dgChange.DataBind();

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
                empdetail.Class = txtClass.Text;
                empdetail.CountryTeam = txtCountryTeam.Text;
                empdetail.EmploymentStatus = Convert.ToInt32(txtEmployeeStatus.Text);

                empdetail.Supervisor = Convert.ToInt32(ddlSuperVisor.Text);
                empdetail.ReportsTo = Convert.ToInt32(ddlReportsTo.Text);

                _presenter.CurrentEmployee.EmployeeDetails.Add(empdetail);


                dgChange.DataSource = _presenter.CurrentEmployee.EmployeeDetails;
                dgChange.DataBind();
             


            }
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
                    term.TerminationReason = txtTerminationReason.Text;

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
                term.TerminationReason = txtTerminationReason.Text;
                term.Employee.GetContract(_presenter.GetLastContrcatId()).Status = "In Active";
                _presenter.CurrentEmployee.AppUser.IsActive = false;
                _presenter.CurrentEmployee.Terminations.Add(term);

                dgTermination.EditIndex = -1;
                dgTermination.DataSource = _presenter.CurrentEmployee.Terminations;
                dgTermination.DataBind();
            }
            // PrintTransactionTermination();
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

            ddlReason.SelectedItem.Text = cont.Reason;
            ddlStatus.SelectedItem.Text = cont.Status;
            btnAddcontract.Text = "Update Contracts";





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









        protected void lnkDeleteemp_Click(object sender, EventArgs e)
        {
            try
            {

                //   int id = Convert.ToInt32(dgChange.SelectedDataKey[0]);
                //int id = Convert.ToInt32(dgChange.SelectedRow.Cells[0].ID);
                LinkButton lnkDeleteemp = sender as LinkButton;
                GridViewRow gvrow = lnkDeleteemp.NamingContainer as GridViewRow;
                int index = gvrow.RowIndex;
                int id = Convert.ToInt32(dgChange.SelectedDataKey[0]);
                //int id = Convert.ToInt32(dgChange.SelectedDataKey[index]);
                _presenter.CurrentEmployee.RemoveEmployeeDetail(id);
                _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);


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





        protected void dgContractDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btnPAFNew_Click(object sender, EventArgs e)
        {
            PrintTransactionnew();
            ClearEmpDetailFormFields();
        }

        protected void btnPAFChange_Click(object sender, EventArgs e)
        {
            PrintTransaction();
            ClearEmpDetailFormFields();
        }

        protected void dgContractDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = (int)dgContractDetail.DataKeys[e.RowIndex].Value;
            _presenter.CurrentEmployee.RemoveContract(id);
            _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
            dgContractDetail.DataSource = _presenter.CurrentEmployee.Contracts;
            dgContractDetail.DataBind();
            Master.ShowMessage(new AppMessage("Contract Detail Is Successfully Deleted!", RMessageType.Info));
        }


        protected void dgChange_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = (int)dgChange.DataKeys[e.RowIndex].Value;
            _presenter.CurrentEmployee.RemoveEmployeeDetail(id);
            _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
            dgChange.DataSource = _presenter.CurrentEmployee.EmployeeDetails;
            dgChange.DataBind();
            Master.ShowMessage(new AppMessage("Employee Detail Is Successfully Deleted!", RMessageType.Info));
        }

        protected void dgTermination_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = (int)dgTermination.DataKeys[e.RowIndex].Value;
            _presenter.CurrentEmployee.RemoveTermination(id);
            _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
            dgTermination.DataSource = _presenter.CurrentEmployee.Terminations;
            dgTermination.DataBind();
            Master.ShowMessage(new AppMessage("Termination Is Successfully Deleted!", RMessageType.Info));
        }





      




      
    }

}

