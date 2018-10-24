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
using System.Drawing;

namespace Chai.WorkflowManagment.Modules.HRM.Views
{

    public partial class frmManageHR : POCBasePage, IManageHRView
    {
        private ManageHRPresenter _presenter;

        string pathImage = null;
        Contract chan;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();

                BindEmployee();
                BindContracts();

                //BindEmployeeDetail();
                BindTermination();
                BindPosition();
                BindProgram();
                BindReportsTo();
                BindSupervisor();


            }
            this._presenter.OnViewLoaded();

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


                    if (_presenter.CurrentEmployee.GetEmpContract(GetId).ContractEndDate < Convert.ToDateTime(StartDate))
                    {
                        cont.ContractStartDate = Convert.ToDateTime(StartDate);
                        cont.ContractEndDate = Convert.ToDateTime(EndDate);
                        cont.Reason = ddlReason.SelectedItem.Text;
                        cont.Status = ddlStatus.SelectedItem.Text;

                        _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);

                        dgContractDetail.EditIndex = -1;
                        dgContractDetail.DataSource = _presenter.CurrentEmployee.Contracts;
                        dgContractDetail.DataBind();
                        ClearContractFormFields();

                    }
                    else
                        Master.ShowMessage(new AppMessage("Current Contract Date must not be less than previous Contract Date ", Chai.WorkflowManagment.Enums.RMessageType.Info));
                }

            }
            else if (btnAddcontract.Text == "Add Contract")
            {


                var StartDate = txtStartDate.Text != "" ? txtStartDate.Text : "";
                var EndDate = txtEndDate.Text != "" ? txtEndDate.Text : " ";
                Contract cont = new Contract();
                if (_presenter.CurrentEmployee.Contracts.Count != 0)
                {
                    if (_presenter.CurrentEmployee.GetEmpContract(GetId).ContractEndDate < Convert.ToDateTime(StartDate))
                    {
                        cont.ContractStartDate = Convert.ToDateTime(StartDate);
                        cont.ContractEndDate = Convert.ToDateTime(EndDate);
                        cont.Reason = ddlReason.SelectedItem.Text;
                        cont.Status = ddlStatus.SelectedItem.Text;
                        if (_presenter.CurrentEmployee.Contracts.Count > 1)
                        {
                            _presenter.CurrentEmployee.GetPreviousContract().Status = "In Active";
                        }

                        _presenter.CurrentEmployee.Contracts.Add(cont);
                        dgContractDetail.EditIndex = -1;
                        dgContractDetail.DataSource = _presenter.CurrentEmployee.Contracts;
                        dgContractDetail.DataBind();
                        ClearContractFormFields();

                    }

                    else
                        Master.ShowMessage(new AppMessage("Current Contract Date must not be less than previous Contract Date ", Chai.WorkflowManagment.Enums.RMessageType.Info));
                }
                else
                {


                    cont.ContractStartDate = Convert.ToDateTime(StartDate);
                    cont.ContractEndDate = Convert.ToDateTime(EndDate);
                    cont.Reason = ddlReason.SelectedItem.Text;
                    cont.Status = ddlStatus.SelectedItem.Text;
                    _presenter.CurrentEmployee.GetContract(_presenter.GetLastContrcatId()).Status = "In Active";
                    _presenter.CurrentEmployee.Contracts.Add(cont);
                    dgContractDetail.EditIndex = -1;
                    dgContractDetail.DataSource = _presenter.CurrentEmployee.Contracts;
                    dgContractDetail.DataBind();
                    ClearContractFormFields();



                }
            }





        }









        private void ClearEmpDetailFormFields()
        {

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
            txtClass.SelectedValue = string.Empty;
            txtEmployeeStatus.SelectedValue = string.Empty;
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




        private void BindContracts()
        {

            if (_presenter.CurrentEmployee.Contracts.Count != 0)
            {


                ddlReason.Items.FindByValue("New Hire").Attributes.Add("Disabled", "Disabled");


                if (_presenter.CurrentEmployee.GetInActiveContract() == true)
                {

                    ddlReason.Items.FindByValue("Renewal").Attributes.Add("Disabled", "Disabled");

                }
                else if (_presenter.CurrentEmployee.GetInActiveContract() == false)
                {
                    ddlReason.Items.FindByValue("Rehire").Attributes.Add("Disabled", "Disabled");
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


        private void BindEmpDetail(Contract con)
        {
            chan = Session["chan"] as Contract;
            dgChange.DataSource = chan.EmployeeDetails;
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
            txtHiredDate.Text = "Hired Date : " + _presenter.CurrentEmployee.AppUser.HiredDate.Value.ToShortDateString();
            txtEmpId.Text = _presenter.CurrentEmployee.AppUser.EmployeeNo;
            txtFullName.Text = _presenter.CurrentEmployee.AppUser.FullName;
            txtProgram.Text = _presenter.CurrentEmployee.GetEmployeeProgram();
            txtPosition.Text = _presenter.CurrentEmployee.GetEmployeePosition();
            txtEmail.Text = _presenter.CurrentEmployee.ChaiEMail;
            lnkEmail.HRef = _presenter.CurrentEmployee.ChaiEMail;
            txtPhoneNo.Text = _presenter.CurrentEmployee.Phone;
            txtLeaveAsOfCalEndDate.Text = Math.Round(_presenter.CurrentEmployee.EmployeeLeaveBalanceYE()).ToString();
            txtLeaveAsOfContractEndDate.Text = _presenter.CurrentEmployee.GetActiveContract() != null ? Math.Round(_presenter.CurrentEmployee.EmployeeLeaveBalanceCED(_presenter.CurrentEmployee.GetActiveContract().ContractEndDate)).ToString() : "";
            txtLeaveAsOfToday.Text = Math.Round(_presenter.CurrentEmployee.EmployeeLeaveBalance()).ToString();

        }



        public void AddEmployeeDetail()
        {
            if (btnAddChange.Text == "Update Change")
            {
                EmployeeDetail empdetail;
                var id = Convert.ToInt32(dgChange.SelectedDataKey[0]) != 0 ? Convert.ToInt32(dgChange.SelectedDataKey[0]) : 0;
                int TEMPChid = Convert.ToInt32(dgChange.DataKeys[dgChange.SelectedRow.RowIndex].Value);
                int Id = dgChange.SelectedRow.RowIndex;

                if (id != 0)
                {
                    chan = Session["chan"] as Contract;

                    empdetail = chan.GetEmployeeDetails(TEMPChid);

                    empdetail.Position = _presenter.GetEmployeePosition(Convert.ToInt32(ddlPosition.Text));
                    empdetail.Program = _presenter.GetProgram(Convert.ToInt32(ddlProgram.Text));

                    empdetail.DutyStation = ddlDutyStation.Text;
                    empdetail.DescriptiveJobTitle = txtDescJT.Text;
                    empdetail.Salary = Convert.ToDecimal(txtSalary.Text);
                    empdetail.HoursPerWeek = txtHoursPerWeek.Text;
                    empdetail.BaseCountry = txtBaseCount.Text;
                    empdetail.BaseCity = txtBaseCity.Text;
                    empdetail.BaseState = txtBaseState.Text;
                    empdetail.Class = txtClass.SelectedItem.Text;
                    empdetail.CountryTeam = txtCountryTeam.Text;
                    empdetail.EmploymentStatus = txtEmployeeStatus.SelectedItem.Text;

                    empdetail.Supervisor = Convert.ToInt32(ddlSuperVisor.Text);
                    empdetail.ReportsTo = Convert.ToInt32(ddlReportsTo.Text);


                    _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);

                    BindEmpDetail(empdetail.Contract);

                    ClearEmpDetailFormFields();
                    pnlEMPHIST_ModalPopupExtender.Show();


                }


            }

            else if (btnAddChange.Text == "Add Change")
            {


                chan = Session["chan"] as Contract;
                EmployeeDetail empdetail = new EmployeeDetail();
                empdetail.Contract = chan;

                empdetail.Position = _presenter.GetEmployeePosition(Convert.ToInt32(ddlPosition.Text));
                empdetail.Program = _presenter.GetProgram(Convert.ToInt32(ddlProgram.Text));

                empdetail.DutyStation = ddlDutyStation.Text;
                empdetail.DescriptiveJobTitle = txtDescJT.Text;
                empdetail.Salary = Convert.ToDecimal(txtSalary.Text);
                empdetail.HoursPerWeek = txtHoursPerWeek.Text;
                empdetail.BaseCountry = txtBaseCount.Text;
                empdetail.BaseCity = txtBaseCity.Text;
                empdetail.BaseState = txtBaseState.Text;
                empdetail.Class = txtClass.Text;
                empdetail.CountryTeam = txtCountryTeam.Text;
                empdetail.EmploymentStatus = txtEmployeeStatus.Text;

                empdetail.Supervisor = Convert.ToInt32(ddlSuperVisor.Text);
                empdetail.ReportsTo = Convert.ToInt32(ddlReportsTo.Text);


                if (_presenter.CurrentEmployee.Id > 0)
                    _presenter.CurrentEmployee.GetContract(Convert.ToInt32(hfDetailId.Value)).EmployeeDetails.Add(empdetail);
                else
                    _presenter.CurrentEmployee.Contracts[Convert.ToInt32(hfDetailId.Value)].EmployeeDetails.Add(empdetail);
                BindEmpDetail(empdetail.Contract);

                ClearEmpDetailFormFields();
                pnlEMPHIST_ModalPopupExtender.Show();





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

            /*   int id = Convert.ToInt32(dgContractDetail.SelectedDataKey[0]);

                Contract cont = _presenter.CurrentEmployee.GetContract(id);
                txtStartDate.Text = cont.ContractStartDate.ToShortDateString();
                txtEndDate.Text = cont.ContractEndDate.ToShortDateString();

                ddlReason.SelectedItem.Text = cont.Reason;
                ddlStatus.SelectedItem.Text = cont.Status;
                btnAddcontract.Text = "Update Contracts";


              */








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
                //////  _presenter.CurrentEmployee.RemoveEmployeeDetail(id);
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
            /////   _presenter.CurrentEmployee.RemoveEmployeeDetail(id);
            _presenter.SaveOrUpdateEmployeeActivity(_presenter.CurrentEmployee);
            ////  dgChange.DataSource = _presenter.CurrentEmployee.EmployeeDetails;
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



            /*    int contid = _presenter.GetLastContrcatId();
                foreach (Contract cn in _presenter.GetContracts())
                {
                    cn.Status = "In Active";

                }

                _presenter.CurrentEmployee.GetContract(contid).Status = "Active";

                foreach (GridViewRow item in dgContractDetail.Rows)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {





                        if (item.Cells[3].Text == "In Active")//orderstatus index
                        {


                            e.Row.Enabled = false;
                        }



                    }
                }*/

        }

        protected void btnEMPhist_Click(object sender, EventArgs e)
        {

            /* int TEMPChid = Convert.ToInt32(dgChange.DataKeys[dgChange.SelectedRow.RowIndex].Value);
             int Id = dgChange.SelectedRow.RowIndex;


             if (TEMPChid > 0)
                 Session["chan"] = _presenter.CurrentEmployee.GetContract(TEMPChid);
             else
                 Session["chan"] = _presenter.CurrentEmployee.Contracts[dgChange.SelectedRow.RowIndex];


             int recordId = (int)dgChange.SelectedRow.RowIndex;
             if (_presenter.CurrentEmployee.Id > 0)
             {
                 hfDetailId.Value = TEMPChid.ToString();
             }
             else
             {
                 hfDetailId.Value = dgChange.SelectedIndex.ToString();
             }
             BindEmpDetail(chan);

             pnlEMPHIST_ModalPopupExtender.Show();*/

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

                pnlEMPHIST_ModalPopupExtender.Show();
                int Row = Convert.ToInt32(e.CommandArgument);
                chan = Session["chan"] as Contract;
                int TEMPChid = Convert.ToInt32(dgChange.DataKeys[Row].Value);
                //   int TEMPChid = Convert.ToInt32(dgContractDetail.DataKeys[dgContractDetail.SelectedRow.RowIndex].Value.ToString());


                if (TEMPChid > 0)
                {
                    EmployeeDetail empdetail = chan.GetEmployeeDetails(TEMPChid);
                    ddlPosition.SelectedValue = empdetail.Position.Id.ToString();
                    ddlProgram.SelectedValue = Convert.ToInt32(empdetail.Program.Id).ToString();
                    ddlDutyStation.SelectedValue = empdetail.DutyStation.ToString();
                    txtDescJT.Text = empdetail.DescriptiveJobTitle;
                    txtSalary.Text = Convert.ToDecimal(empdetail.Salary).ToString();
                    txtHoursPerWeek.Text = empdetail.HoursPerWeek;
                    txtBaseCount.Text = empdetail.BaseCountry;
                    txtBaseCity.Text = empdetail.BaseCity;
                    txtBaseState.Text = empdetail.BaseState;
                    txtClass.SelectedValue = empdetail.Class;
                    txtCountryTeam.Text = empdetail.CountryTeam;
                    txtEmployeeStatus.SelectedValue = empdetail.EmploymentStatus;
                    ddlSuperVisor.SelectedValue = Convert.ToInt32(empdetail.Supervisor).ToString();
                    ddlReportsTo.SelectedValue = Convert.ToInt32(empdetail.ReportsTo).ToString();


                    btnAddChange.Text = "Update Change";

                    pnlEMPHIST_ModalPopupExtender.Show();

                }






            }

            else if (e.CommandName == "Delete")
            {
                int Row = Convert.ToInt32(e.CommandArgument);
                chan = Session["chan"] as Contract;
                int TEMPChid = Convert.ToInt32(dgChange.DataKeys[Row].Value);

                EmployeeDetail emp = _presenter.GetEmployeeDetail(TEMPChid);

                if (TEMPChid > 0)
                    emp = _presenter.GetEmployeeDetail(TEMPChid);
                //  else
                //  emp = (EmployeeDetail)chan.EmployeeDetails[e.CommandArgumen.Item.ItemIndex];

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
                    pnlEMPHIST_ModalPopupExtender.Show();

                    Master.ShowMessage(new AppMessage("Employee History was removed successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to delete Employee History. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }

            }
        }

        protected void dgContractDetail_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "History")
            {
                int Row = Convert.ToInt32(e.CommandArgument);

                int TEMPChid = Convert.ToInt32(dgContractDetail.DataKeys[Row].Value);
                //   int TEMPChid = Convert.ToInt32(dgContractDetail.DataKeys[dgContractDetail.SelectedRow.RowIndex].Value.ToString());


                if (TEMPChid > 0)
                    Session["chan"] = _presenter.CurrentEmployee.GetContract(TEMPChid);
                else
                    Session["chan"] = _presenter.CurrentEmployee.Contracts[dgChange.SelectedRow.DataItemIndex];



                if (_presenter.CurrentEmployee.Id > 0)
                {
                    hfDetailId.Value = TEMPChid.ToString();
                }
                else
                {
                    hfDetailId.Value = dgChange.SelectedRow.DataItemIndex.ToString();
                }
                BindEmpDetail(chan);
                pnlEMPHIST_ModalPopupExtender.Show();


            }

            else if (e.CommandName == "Select")
            {


                int Row = Convert.ToInt32(e.CommandArgument);

                int TEMPChid = Convert.ToInt32(dgContractDetail.DataKeys[Row].Value);
                //   int TEMPChid = Convert.ToInt32(dgContractDetail.DataKeys[dgContractDetail.SelectedRow.RowIndex].Value.ToString());


                if (TEMPChid > 0)
                    Session["chan"] = _presenter.CurrentEmployee.GetContract(TEMPChid);
                else
                    Session["chan"] = _presenter.CurrentEmployee.Contracts[dgChange.SelectedRow.DataItemIndex];




                Contract cont = _presenter.CurrentEmployee.GetContract(TEMPChid);
                txtStartDate.Text = cont.ContractStartDate.ToShortDateString();
                txtEndDate.Text = cont.ContractEndDate.ToShortDateString();

                ddlReason.SelectedItem.Text = cont.Reason;
                ddlStatus.SelectedItem.Text = cont.Status;
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
                        dgContractDetail.DataSource = _presenter.CurrentEmployee.Contracts;
                        dgContractDetail.DataBind();
                        Master.ShowMessage(new AppMessage("Employee Detail Is Successfully Deleted!", RMessageType.Info));
                    }
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to delete Employee History. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }

            }
        }


    }

}

