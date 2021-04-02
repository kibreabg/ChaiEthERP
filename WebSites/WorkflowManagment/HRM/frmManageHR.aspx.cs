using System;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.CoreDomain.HRM;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Reflection;

using System.IO;
using System.Diagnostics;
using System.Web.UI;

namespace Chai.WorkflowManagment.Modules.HRM.Views
{

    public partial class frmManageHR : POCBasePage, IManageHRView
    {
        private ManageHRPresenter _presenter;
        string pathImage = null;
        Contract selectedContract;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                //BindEmployeeDetail();
                BindTermination();
                BindPosition();
                BindProgram();
                BindSupervisor();
            }
            this._presenter.OnViewLoaded();
            BindEmployee();
            BindContracts();

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
        private void ClearEmpDetailFormFields()
        {

            ddlPosition.SelectedValue = "0";
            ddlProgram.SelectedValue = "0";
            ddlDutyStation.SelectedValue = string.Empty;
            txtEffectDate.Text = string.Empty;
            txtSalary.Text = string.Empty;
            txtHoursPerWeek.Text = string.Empty;
            txtCountryTeam.Text = string.Empty;
            txtBaseCount.Text = string.Empty;
            txtBaseCity.Text = string.Empty;
            txtBaseState.Text = string.Empty;
            txtClass.Text = string.Empty;
            txtEmployeeStatus.Text = string.Empty;
            ddlSuperVisor.SelectedValue = "0";

        }
        private void ClearContractFormFields()
        {
            txtStartDate.Text = string.Empty;
            txtEndDate.Text = string.Empty;

            ddlReason.SelectedValue = "";
            ddlStatus.SelectedValue = "";
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

                ddlStatus.Items.FindByValue("In Active").Enabled = false;




                dgContractDetail.DataSource = _presenter.CurrentEmployee.Contracts;
                dgContractDetail.DataBind();
            }
            else
            {
                dgContractDetail.DataSource = _presenter.CurrentEmployee.Contracts;
                dgContractDetail.DataBind();

            }


        }
        private void BindEmpDetail(Contract contract)
        {
            EmployeeDetail employeeDetail = contract.GetLastEmployeeDetail();
            ddlPosition.SelectedValue = employeeDetail.Position.Id.ToString();
            ddlProgram.SelectedValue = employeeDetail.Program.Id.ToString();
            ddlDutyStation.SelectedValue = employeeDetail.DutyStation;
            txtSalary.Text = employeeDetail.Salary.ToString();
            txtEmployeeStatus.SelectedValue = employeeDetail.EmploymentStatus;
            txtClass.SelectedValue = employeeDetail.Class;
            txtHoursPerWeek.Text = employeeDetail.HoursPerWeek;
            txtBaseCount.Text = employeeDetail.BaseCountry;
            txtBaseCity.Text = employeeDetail.BaseCity;
            txtBaseState.Text = employeeDetail.BaseState;
            txtCountryTeam.Text = employeeDetail.CountryTeam;
            txtEffectDate.Text = employeeDetail.EffectiveDateOfChange.ToShortDateString();
            ddlSuperVisor.SelectedValue = employeeDetail.Supervisor.ToString();

            dgChange.DataSource = contract.EmployeeDetails;
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
            /*     lblBaseCityCurr.Text = _presenter.CurrentEmployee.GetEmployeeDetails(current).BaseCity.ToString();
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
                 lblReporttoChange.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).ReportsTo.ToString();*/


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
            /*       lblBaseCityres.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).BaseCity.ToString();
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
                   lblReporttores.Text = _presenter.CurrentEmployee.GetEmployeeDetails(change).ReportsTo.ToString();*/

        }
        private void PrintTransactionTermination()
        {

            lblFirstNameResultTer.Text = _presenter.CurrentEmployee.FirstName;
            lbliddleNameResultTer.Text = _presenter.CurrentEmployee.LastName;
            lblLastNameResultTer.Text = _presenter.CurrentEmployee.LastName;
            lblEmailResultTer.Text = _presenter.CurrentEmployee.ChaiEMail;
            lblBasCountryResTer.Text = _presenter.CurrentEmployee.Country;

            /////    lblClassResTer.Text = _presenter.CurrentEmployee.GetEmployeeDetails(_presenter.GetLastEmployeeDetailId()).Class.ToString();

            ClearTerminationFormFields();
            // lblDurationOfCont.Text = Convert.ToDateTime((_presenter.CurrentEmployee.GetContract(_presenter.GetLastEmployeeDetailId()).ContractEndDate - _presenter.CurrentEmployee.GetContract(_presenter.GetLastEmployeeDetailId()).ContractEndDate)).Month.ToString()+"Month";


        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
            Master.ShowMessage(new AppMessage("Saved Successfully ", RMessageType.Info));
        }
        private void BindEmployee()
        {
            imgProfilePic.ImageUrl = _presenter.CurrentEmployee.AppUser.Employee.Photo;
            txtHiredDate.Text = "Hired Date : " + _presenter.CurrentEmployee.AppUser.HiredDate.Value.ToShortDateString();
            txtEmpId.Text = _presenter.CurrentEmployee.AppUser.EmployeeNo;
            txtFullName.Text = _presenter.CurrentEmployee.AppUser.FullName;
            txtProgram.Text = _presenter.CurrentEmployee.GetEmployeeProgram();
            txtPosition.Text = _presenter.CurrentEmployee.GetEmployeePosition();
            txtEmail.Text = _presenter.CurrentEmployee.ChaiEMail;
            lnkEmail.HRef = _presenter.CurrentEmployee.ChaiEMail;
            txtPhoneNo.Text = _presenter.CurrentEmployee.Phone;

            txtLeaveAsOfCalEndDate.Text = (Math.Round((_presenter.CurrentEmployee.EmployeeLeaveBalanceYE() - Convert.ToDouble(_presenter.EmpLeaveTaken(_presenter.CurrentEmployee.Id, _presenter.CurrentEmployee.LeaveSettingDate.Value))) * 2, MidpointRounding.AwayFromZero) / 2).ToString();
            txtLeaveAsOfContractEndDate.Text = _presenter.CurrentEmployee.GetActiveContract() != null ? (Math.Round((_presenter.CurrentEmployee.EmployeeLeaveBalanceCED(_presenter.CurrentEmployee.GetActiveContract().ContractEndDate) - Convert.ToDouble(_presenter.EmpLeaveTaken(_presenter.CurrentEmployee.Id, _presenter.CurrentEmployee.LeaveSettingDate.Value))) * 2, MidpointRounding.AwayFromZero) / 2).ToString() : "";
            txtLeaveAsOfToday.Text = (Math.Round((_presenter.CurrentEmployee.EmployeeLeaveBalance() - Convert.ToDouble(_presenter.EmpLeaveTaken(_presenter.CurrentEmployee.Id, _presenter.CurrentEmployee.LeaveSettingDate.Value))) * 2, MidpointRounding.AwayFromZero) / 2).ToString();
            txttoalleavetaken.Text = _presenter.EmpLeaveTaken(_presenter.CurrentEmployee.Id, _presenter.CurrentEmployee.LeaveSettingDate.Value).ToString();
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
                    _presenter.CurrentEmployee.GetActiveContract().Status = "In Active";
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
                _presenter.CurrentEmployee.GetActiveContract().Status = "In Active";
                _presenter.CurrentEmployee.AppUser.IsActive = false;
                _presenter.CurrentEmployee.Terminations.Add(term);
                _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
                dgTermination.EditIndex = -1;
                dgTermination.DataSource = _presenter.CurrentEmployee.Terminations;
                dgTermination.DataBind();
            }

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
        protected void btnAddTerm_Click(object sender, EventArgs e)
        {
            AddTermination();
            _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
        }
        protected void btnAddChange_Click(object sender, EventArgs e)
        {
            if (btnAddChange.Text == "Update Change")
            {
                var EffectDate = txtEffectDate.Text != "" ? txtEffectDate.Text : "";
                EmployeeDetail empdetail;
                var id = Convert.ToInt32(dgChange.SelectedDataKey[0]) != 0 ? Convert.ToInt32(dgChange.SelectedDataKey[0]) : 0;
                int empDetailId = Convert.ToInt32(dgChange.DataKeys[dgChange.SelectedRow.RowIndex].Value);
                int Id = dgChange.SelectedRow.RowIndex;

                if (empDetailId != 0)
                {
                    selectedContract = Session["selectedContract"] as Contract;
                    empdetail = selectedContract.GetEmployeeDetails(empDetailId);

                    if (empdetail != null)
                    {
                        empdetail.Position = _presenter.GetEmployeePosition(Convert.ToInt32(ddlPosition.Text));
                        empdetail.Program = _presenter.GetProgram(Convert.ToInt32(ddlProgram.Text));
                        empdetail.DutyStation = ddlDutyStation.Text;
                        empdetail.DescriptiveJobTitle = empdetail.Position.PositionName.ToString();
                        empdetail.Salary = Convert.ToDecimal(txtSalary.Text);
                        empdetail.HoursPerWeek = txtHoursPerWeek.Text;
                        empdetail.BaseCountry = txtBaseCount.Text;
                        empdetail.BaseCity = txtBaseCity.Text;
                        empdetail.BaseState = txtBaseState.Text;
                        empdetail.Class = txtClass.SelectedItem.Text;
                        empdetail.CountryTeam = txtCountryTeam.Text;
                        empdetail.EmploymentStatus = txtEmployeeStatus.SelectedItem.Text;
                        empdetail.Supervisor = Convert.ToInt32(ddlSuperVisor.Text);
                        empdetail.ReportsTo = empdetail.Supervisor;
                        empdetail.EffectiveDateOfChange = Convert.ToDateTime(txtEffectDate.Text);
                    }
                    else
                    {
                        empdetail = new EmployeeDetail();
                        empdetail.Position = _presenter.GetEmployeePosition(Convert.ToInt32(ddlPosition.Text));
                        empdetail.Program = _presenter.GetProgram(Convert.ToInt32(ddlProgram.Text));
                        empdetail.DutyStation = ddlDutyStation.Text;
                        empdetail.DescriptiveJobTitle = empdetail.Position.PositionName.ToString();
                        empdetail.Salary = Convert.ToDecimal(txtSalary.Text);
                        empdetail.HoursPerWeek = txtHoursPerWeek.Text;
                        empdetail.BaseCountry = txtBaseCount.Text;
                        empdetail.BaseCity = txtBaseCity.Text;
                        empdetail.BaseState = txtBaseState.Text;
                        empdetail.Class = txtClass.SelectedItem.Text;
                        empdetail.CountryTeam = txtCountryTeam.Text;
                        empdetail.EmploymentStatus = txtEmployeeStatus.SelectedItem.Text;
                        empdetail.Supervisor = Convert.ToInt32(ddlSuperVisor.Text);
                        empdetail.ReportsTo = empdetail.Supervisor;
                        empdetail.EffectiveDateOfChange = Convert.ToDateTime(txtEffectDate.Text);
                        selectedContract.EmployeeDetails.Add(empdetail);
                    }

                    _presenter.SaveOrUpdateEmployeeActivity(selectedContract.Employee);
                    BindEmpDetail(_presenter.CurrentEmployee.GetActiveContractForEmp());
                    ScriptManager.RegisterStartupScript(this, GetType(), "showEmpHistoryModal", "showEmpHistoryModal();", true);
                    Master.ShowMessage(new AppMessage("Employee History Successfully Updated!", RMessageType.Info));
                }
            }
            else if (btnAddChange.Text == "Add Change")
            {
                var EffectDate = txtEffectDate.Text != "" ? txtEffectDate.Text : "";
                selectedContract = Session["selectedContract"] as Contract;
                EmployeeDetail empdetail = new EmployeeDetail();
                empdetail.Contract = selectedContract;

                empdetail.Position = _presenter.GetEmployeePosition(Convert.ToInt32(ddlPosition.Text));
                empdetail.Program = _presenter.GetProgram(Convert.ToInt32(ddlProgram.Text));
                empdetail.DutyStation = ddlDutyStation.Text;
                empdetail.DescriptiveJobTitle = empdetail.Position.PositionName.ToString();
                empdetail.Salary = Convert.ToDecimal(txtSalary.Text);
                empdetail.HoursPerWeek = txtHoursPerWeek.Text;
                empdetail.BaseCountry = txtBaseCount.Text;
                empdetail.BaseCity = txtBaseCity.Text;
                empdetail.BaseState = txtBaseState.Text;
                empdetail.Class = txtClass.Text;
                empdetail.CountryTeam = txtCountryTeam.Text;
                empdetail.EmploymentStatus = txtEmployeeStatus.Text;
                empdetail.Supervisor = Convert.ToInt32(ddlSuperVisor.Text);
                empdetail.ReportsTo = empdetail.Supervisor;
                empdetail.EffectiveDateOfChange = Convert.ToDateTime(txtEffectDate.Text);

                if (_presenter.CurrentEmployee.Id > 0)
                    _presenter.CurrentEmployee.GetContract(Convert.ToInt32(hfDetailId.Value)).EmployeeDetails.Add(empdetail);
                else
                    _presenter.CurrentEmployee.Contracts[Convert.ToInt32(hfDetailId.Value)].EmployeeDetails.Add(empdetail);
                                
                _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
                BindEmpDetail(_presenter.CurrentEmployee.GetActiveContractForEmp());
                ScriptManager.RegisterStartupScript(this, GetType(), "showEmpHistoryModal", "showEmpHistoryModal();", true);
                Master.ShowMessage(new AppMessage("Employee History Successfully Added!", RMessageType.Info));
            }
        }
        protected void btnAddcontract_Click(object sender, EventArgs e)
        {
            if (btnAddcontract.Text == "Update Contracts")
            {
                var id = Convert.ToInt32(dgContractDetail.SelectedDataKey[0]) != 0 ? Convert.ToInt32(dgContractDetail.SelectedDataKey[0]) : 0;

                if (id != 0)
                {
                    int x = _presenter.CurrentEmployee.Contracts.Count;
                    var StartDate = txtStartDate.Text != "" ? txtStartDate.Text : "";
                    var EndDate = txtEndDate.Text != "" ? txtEndDate.Text : " ";
                    Contract cont = _presenter.CurrentEmployee.GetContract(id);

                    cont.ContractStartDate = Convert.ToDateTime(StartDate);
                    cont.ContractEndDate = Convert.ToDateTime(EndDate);
                    cont.Reason = ddlReason.SelectedValue;
                    cont.Status = ddlStatus.SelectedValue;

                    EmployeeDetail emp = new EmployeeDetail();
                    emp.Contract = _presenter.GetContract(_presenter.CurrentEmployee.GetActiveContractForEmp().Id);
                    if (cont.EmployeeDetails.Count > 0)
                    {
                        emp.Position = _presenter.CurrentEmployee.Contracts[x - 1].EmployeeDetails[x - 1].Position;
                        emp.Program = _presenter.CurrentEmployee.Contracts[x - 1].EmployeeDetails[x - 1].Program;

                        emp.DutyStation = _presenter.CurrentEmployee.Contracts[x - 1].EmployeeDetails[x - 1].DutyStation;
                        emp.DescriptiveJobTitle = _presenter.CurrentEmployee.Contracts[x - 1].EmployeeDetails[x - 1].DescriptiveJobTitle;
                        emp.Salary = _presenter.CurrentEmployee.Contracts[x - 1].EmployeeDetails[x - 1].Salary;
                        emp.HoursPerWeek = _presenter.CurrentEmployee.Contracts[x - 1].EmployeeDetails[x - 1].HoursPerWeek;
                        emp.BaseCountry = _presenter.CurrentEmployee.Contracts[x - 1].EmployeeDetails[x - 1].BaseCountry;
                        emp.BaseCity = _presenter.CurrentEmployee.Contracts[x - 1].EmployeeDetails[x - 1].BaseCity;
                        emp.BaseState = _presenter.CurrentEmployee.Contracts[x - 1].EmployeeDetails[x - 1].BaseState;
                        emp.Class = _presenter.CurrentEmployee.Contracts[x - 1].EmployeeDetails[x - 1].Class;
                        emp.CountryTeam = _presenter.CurrentEmployee.Contracts[x - 1].EmployeeDetails[x - 1].CountryTeam;
                        emp.EmploymentStatus = _presenter.CurrentEmployee.Contracts[x - 1].EmployeeDetails[x - 1].EmploymentStatus;

                        emp.Supervisor = _presenter.CurrentEmployee.Contracts[x - 1].EmployeeDetails[x - 1].Supervisor;
                        emp.ReportsTo = _presenter.CurrentEmployee.Contracts[x - 1].EmployeeDetails[x - 1].ReportsTo;
                        emp.EffectiveDateOfChange = DateTime.Now;

                    }
                    emp.Contract = _presenter.GetContract(_presenter.CurrentEmployee.GetActiveContractForEmp().Id);
                    _presenter.CurrentEmployee.GetActiveContractForEmp().EmployeeDetails.Add(emp);
                    var prevCont = _presenter.CurrentEmployee.GetPreviousContract();
                    if (prevCont != null)
                    {
                        if (prevCont.EmployeeDetails.Count > 0)
                        {
                            prevCont.Status = "In Active";
                        }
                    }

                    _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);

                    dgContractDetail.EditIndex = -1;
                    ClearContractFormFields();
                    dgContractDetail.DataSource = _presenter.CurrentEmployee.Contracts;
                    dgContractDetail.DataBind();
                }

            }
            else if (btnAddcontract.Text == "Add Contract")
            {
                var StartDate = txtStartDate.Text != "" ? txtStartDate.Text : "";
                var EndDate = txtEndDate.Text != "" ? txtEndDate.Text : " ";
                Contract newContract = new Contract();

                if (_presenter.CurrentEmployee.Contracts.Count != 0)
                {
                    if (_presenter.CurrentEmployee.GetEmpContract(GetId).ContractEndDate < Convert.ToDateTime(StartDate))
                    {
                        newContract.ContractStartDate = Convert.ToDateTime(StartDate);
                        newContract.ContractEndDate = Convert.ToDateTime(EndDate);
                        newContract.Reason = ddlReason.SelectedItem.Text;
                        newContract.Status = ddlStatus.SelectedItem.Text;

                        Contract previousContract = _presenter.CurrentEmployee.GetActiveContractForEmp();
                        foreach (EmployeeDetail employeeDetail in previousContract.EmployeeDetails)
                        {
                            newContract.EmployeeDetails.Add(employeeDetail);
                        }
                        previousContract.Status = "In Active";
                        _presenter.SaveOrUpdateContract(previousContract);

                        _presenter.CurrentEmployee.Contracts.Add(newContract);
                        _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
                        dgContractDetail.EditIndex = -1;
                        ClearContractFormFields();
                        dgContractDetail.DataSource = _presenter.CurrentEmployee.Contracts;
                        dgContractDetail.DataBind();
                    }

                    else
                        Master.ShowMessage(new AppMessage("Current Contract Date must not be less than previous Contract Date ", RMessageType.Info));
                }
                else
                {
                    newContract.ContractStartDate = Convert.ToDateTime(StartDate);
                    newContract.ContractEndDate = Convert.ToDateTime(EndDate);
                    newContract.Reason = ddlReason.SelectedItem.Text;
                    newContract.Status = ddlStatus.SelectedItem.Text;
                    _presenter.CurrentEmployee.Contracts.Add(newContract);

                    _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
                    dgContractDetail.EditIndex = -1;
                    ClearContractFormFields();
                    dgContractDetail.DataSource = _presenter.CurrentEmployee.Contracts;
                    dgContractDetail.DataBind();
                }
            }
        }
        protected void dgContractDetail_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void lnkEdit_Click(object sender, EventArgs e)
        {

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


                LinkButton lnkDeleteemp = sender as LinkButton;
                GridViewRow gvrow = lnkDeleteemp.NamingContainer as GridViewRow;
                int index = gvrow.RowIndex;
                int id = Convert.ToInt32(dgChange.SelectedDataKey[0]);
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
        protected void dgContractDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void dgChange_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void dgTermination_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void dgContractDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            foreach (GridViewRow row in dgContractDetail.Rows)
            {
                for (int i = 0; i < dgContractDetail.Columns.Count; i++)
                {
                    String header = dgContractDetail.Columns[3].HeaderText;
                    String cellText = row.Cells[3].Text;

                    if (cellText == "Active")
                    {
                        row.Enabled = true;

                    }
                    else
                    {
                        row.Enabled = false;
                        row.Cells[4].Enabled = false;
                        row.Cells[5].Enabled = false;

                    }
                }
            }
        }
        protected void btnEMPhist_Click(object sender, EventArgs e)
        {


        }
        protected void RowCommandHandler(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void dgChange_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void dgChange_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "Select")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showEmpHistoryModal", "showEmpHistoryModal();", true);
                int Row = Convert.ToInt32(e.CommandArgument);
                selectedContract = Session["selectedContract"] as Contract;
                int TEMPChid = Convert.ToInt32(dgChange.DataKeys[Row].Value);
                //   int TEMPChid = Convert.ToInt32(dgContractDetail.DataKeys[dgContractDetail.SelectedRow.RowIndex].Value.ToString());

                if (TEMPChid > 0)
                {
                    EmployeeDetail empdetail = selectedContract.GetEmployeeDetails(TEMPChid);
                    if (empdetail != null)
                    {
                        ddlPosition.SelectedValue = empdetail.Position.Id.ToString();
                        ddlProgram.SelectedValue = Convert.ToInt32(empdetail.Program.Id).ToString();
                        ddlDutyStation.SelectedValue = empdetail.DutyStation.ToString();
                        txtSalary.Text = Convert.ToDecimal(empdetail.Salary).ToString();
                        txtHoursPerWeek.Text = empdetail.HoursPerWeek;
                        txtBaseCount.Text = empdetail.BaseCountry;
                        txtBaseCity.Text = empdetail.BaseCity;
                        txtBaseState.Text = empdetail.BaseState;
                        txtClass.SelectedValue = empdetail.Class;
                        txtCountryTeam.Text = empdetail.CountryTeam;
                        txtEmployeeStatus.SelectedValue = empdetail.EmploymentStatus;
                        ddlSuperVisor.SelectedValue = Convert.ToInt32(empdetail.Supervisor).ToString();

                        txtEffectDate.Text = empdetail.EffectiveDateOfChange.ToShortDateString();
                    }

                    btnAddChange.Text = "Update Change";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showEmpHistoryModal", "showEmpHistoryModal();", true);
                }
            }

            else if (e.CommandName == "Delete")
            {
                int Row = Convert.ToInt32(e.CommandArgument);
                selectedContract = Session["selectedContract"] as Contract;
                int TEMPChid = Convert.ToInt32(dgChange.DataKeys[Row].Value);

                EmployeeDetail emp = _presenter.GetEmployeeDetail(TEMPChid);

                if (TEMPChid > 0)
                    emp = _presenter.GetEmployeeDetail(TEMPChid);
                //  else
                //  emp = (EmployeeDetail)selectedContract.EmployeeDetails[e.CommandArgumen.Item.ItemIndex];

                try
                {
                    if (TEMPChid > 0)
                    {
                        _presenter.CurrentEmployee.GetContract(Convert.ToInt32(hfDetailId.Value)).RemoveEmployeeDetail(TEMPChid);
                        if (_presenter.GetEmployeeDetail(TEMPChid) != null)
                            _presenter.DeleteEmployeeDetails(_presenter.GetEmployeeDetail(TEMPChid));

                        _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
                    }
                    else
                    {
                        _presenter.CurrentEmployee.GetContract(Convert.ToInt32(hfDetailId.Value)).EmployeeDetails.Remove(emp);
                    }
                    BindEmpDetail(emp.Contract);
                    ScriptManager.RegisterStartupScript(this, GetType(), "showEmpHistoryModal", "showEmpHistoryModal();", true);

                    Master.ShowMessage(new AppMessage("Employee History was removed successfully", RMessageType.Info));
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to delete Employee History. ", RMessageType.Error));
                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentEmployee.AppUser.FullName);
                }

            }
        }
        protected void dgContractDetail_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "History")
            {
                int Row = Convert.ToInt32(e.CommandArgument);

                int TEMPChid = Convert.ToInt32(dgContractDetail.DataKeys[Row].Value);
                if (TEMPChid > 0)
                    Session["selectedContract"] = _presenter.CurrentEmployee.GetContract(TEMPChid);
                else
                    Session["selectedContract"] = _presenter.CurrentEmployee.Contracts[dgChange.SelectedRow.DataItemIndex];
                selectedContract = Session["selectedContract"] as Contract;
                if (_presenter.CurrentEmployee.Id > 0)
                {
                    hfDetailId.Value = TEMPChid.ToString();
                }
                else
                {
                    hfDetailId.Value = dgChange.SelectedRow.DataItemIndex.ToString();
                }
                BindEmpDetail(selectedContract);
                ScriptManager.RegisterStartupScript(this, GetType(), "showEmpHistoryModal", "showEmpHistoryModal();", true);
            }

            else if (e.CommandName == "Select")
            {
                int Row = Convert.ToInt32(e.CommandArgument);

                int TEMPChid = Convert.ToInt32(dgContractDetail.DataKeys[Row].Value);
                //   int TEMPChid = Convert.ToInt32(dgContractDetail.DataKeys[dgContractDetail.SelectedRow.RowIndex].Value.ToString());


                if (TEMPChid > 0)
                    Session["selectedContract"] = _presenter.CurrentEmployee.GetContract(TEMPChid);
                else
                    Session["selectedContract"] = _presenter.CurrentEmployee.Contracts[dgChange.SelectedRow.DataItemIndex];

                Contract cont = _presenter.CurrentEmployee.GetContract(TEMPChid);
                txtStartDate.Text = cont.ContractStartDate.ToShortDateString();
                txtEndDate.Text = cont.ContractEndDate.ToShortDateString();

                ddlReason.SelectedValue = cont.Reason;
                ddlStatus.SelectedValue = cont.Status;
                btnAddcontract.Text = "Update Contracts";

            }

            else if (e.CommandName == "Delete")
            {
                int Row = Convert.ToInt32(e.CommandArgument);

                int TEMPChid = Convert.ToInt32(dgContractDetail.DataKeys[Row].Value);
                try
                {
                    if (TEMPChid > 0)
                    {
                        _presenter.CurrentEmployee.RemoveContract(TEMPChid);
                        _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
                        if (_presenter.CurrentEmployee.Contracts.Count > 1)
                        {
                            _presenter.CurrentEmployee.GetLastInActiveContract().Status = "Active";
                        }
                        dgContractDetail.DataSource = _presenter.CurrentEmployee.Contracts;
                        dgContractDetail.DataBind();
                        Master.ShowMessage(new AppMessage("Contract Detail Is Successfully Deleted!", RMessageType.Info));
                    }
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Delete Contract. ", RMessageType.Error));
                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentEmployee.AppUser.FullName);
                }

            }
        }
        protected void dgContractDetail_RowDeleting1(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void dgChange_RowDeleting1(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void dgTermination_RowDeleting1(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void dgTermination_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "Select")
            {


                int Row = Convert.ToInt32(e.CommandArgument);

                int TEMPChid = Convert.ToInt32(dgTermination.DataKeys[Row].Value);
                //   int TEMPChid = Convert.ToInt32(dgContractDetail.DataKeys[dgContractDetail.SelectedRow.RowIndex].Value.ToString());


                if (TEMPChid > 0)
                    Session["selectedContract"] = _presenter.CurrentEmployee.GetTerminations(TEMPChid);
                else
                    Session["selectedContract"] = _presenter.CurrentEmployee.Terminations[dgTermination.SelectedRow.DataItemIndex];




                Termination term = _presenter.CurrentEmployee.GetTerminations(TEMPChid);
                txtTerminationDate.Text = term.TerminationDate.ToShortDateString();
                txtLastDate.Text = term.LastDateOfEmployee.ToShortDateString();
                ddlRecommendation.SelectedItem.Text = term.ReccomendationForRehire;
                txtTerminationReason.Text = term.TerminationReason;
                btnAddTerm.Text = "Update Terminations";



            }

            else if (e.CommandName == "Delete")
            {

                int Row = Convert.ToInt32(e.CommandArgument);

                int TEMPChid = Convert.ToInt32(dgTermination.DataKeys[Row].Value);
                try
                {
                    if (TEMPChid > 0)
                    {
                        _presenter.CurrentEmployee.RemoveTermination(TEMPChid);
                        _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
                        dgTermination.DataSource = _presenter.CurrentEmployee.Terminations;
                        dgTermination.DataBind();








                        Master.ShowMessage(new AppMessage("Termination  Is Successfully Deleted!", RMessageType.Info));
                    }
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Delete Termination. ", RMessageType.Error));

                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentEmployee.AppUser.FullName);
                }

            }
        }
        protected void btnGet_Click(object sender, EventArgs e)
        {
            if (txtthisdate.Text != "")
            {
                DateTime lastday = Convert.ToDateTime(txtthisdate.Text);
                lbllastdayleave.Text = (Math.Round((_presenter.CurrentEmployee.EmployeeLeaveBalanceLastDay(lastday) - Convert.ToDouble(_presenter.EmpLeaveTaken(_presenter.CurrentEmployee.Id, _presenter.CurrentEmployee.LeaveSettingDate.Value))) * 2, MidpointRounding.AwayFromZero) / 2).ToString();
            }
        }
    }

}

