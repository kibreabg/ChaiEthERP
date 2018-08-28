using System;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Shared;
using log4net;
using Chai.WorkflowManagment.CoreDomain.HRM;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Chai.WorkflowManagment.Modules.HRM.Views
{
    public partial class EmployeeProfile : POCBasePage, IEmployeeProfileView
    {
        private EmployeeProfilePresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        private int famId;
        private int emergId;
        private int eduId;
        private int workExpId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindEmployee();
                BindFamilyDetails();
                BindEmergencyContacts();
                BindEducations();
                BindWorkExperiences();
            }
            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public EmployeeProfilePresenter Presenter
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

        #region Field Getters
        public int GetEmployeeId
        {
            get
            {
                if (Convert.ToInt32(Request.QueryString["EmpId"]) != 0)
                {
                    return Convert.ToInt32(Request.QueryString["EmpId"]);
                }
                else
                { return 0; }
            }
        }

        public string GetFirstName
        {
            get { return txtFirstName.Text; }
        }

        public string GetLastName
        {
            get { return txtLastName.Text; }
        }

        public string GetGender
        {
            get { return ddlGender.SelectedValue; }
        }

        public DateTime GetDateOfBirth
        {
            get { return Convert.ToDateTime(txtDateOfBirth.Text); }
        }

        public string GetMaritalStatus
        {
            get { return ddlMaritalStatus.SelectedValue; }
        }

        public string GetNationality
        {
            get { return txtNationality.Text; }
        }

        public string GetAddress
        {
            get { return txtAddress.Text; }
        }

        public string GetCity
        {
            get { return txtCity.Text; }
        }

        public string GetCountry
        {
            get { return txtCountry.Text; }
        }

        public string GetPhone
        {
            get { return txtPhone.Text; }
        }

        public string GetCellPhone
        {
            get { return txtCellPhone.Text; }
        }

        public string GetPersonalEmail
        {
            get { return txtPersonalEmail.Text; }
        }

        public string GetChaiEmail
        {
            get { return txtChaiEmail.Text; }
        }

        public string GetPhoto
        {
            get { return txtPhone.Text; }
        }

        #endregion

        private void BindEmployee()
        {
            txtFirstName.Text = _presenter.CurrentEmployee.FirstName;
            txtLastName.Text = _presenter.CurrentEmployee.LastName;
            ddlGender.SelectedValue = _presenter.CurrentEmployee.Gender;
            txtDateOfBirth.Text = Convert.ToDateTime(_presenter.CurrentEmployee.DateOfBirth).ToShortDateString();
            ddlMaritalStatus.SelectedValue = _presenter.CurrentEmployee.MaritalStatus;
            txtNationality.Text = _presenter.CurrentEmployee.Nationality;
            txtPhone.Text = _presenter.CurrentEmployee.Phone;
            txtCellPhone.Text = _presenter.CurrentEmployee.CellPhone;
            txtChaiEmail.Text = _presenter.CurrentEmployee.ChaiEMail;
            txtPersonalEmail.Text = _presenter.CurrentEmployee.PersonalEmail;
            txtCountry.Text = _presenter.CurrentEmployee.Country;
            txtCity.Text = _presenter.CurrentEmployee.City;
            txtAddress.Text = _presenter.CurrentEmployee.Address;

        }
        private void BindFamilyDetails()
        {
            grvFamilyDetails.DataSource = _presenter.CurrentEmployee.FamilyDetails;
            grvFamilyDetails.DataBind();
        }

        private void BindEmergencyContacts()
        {
            grvEmergContacts.DataSource = _presenter.CurrentEmployee.EmergencyContacts;
            grvEmergContacts.DataBind();
        }

        private void BindEducations()
        {
            grvEducations.DataSource = _presenter.CurrentEmployee.Educations;
            grvEducations.DataBind();
        }

        private void BindWorkExperiences()
        {
            grvWorkExperiences.DataSource = _presenter.CurrentEmployee.WorkExperiences;
            grvWorkExperiences.DataBind();
        }

        protected void grvFamilyDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvFamilyDetails.PageIndex = e.NewPageIndex;
            BindFamilyDetails();
        }

        protected void grvFamilyDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            famId = Convert.ToInt32(grvFamilyDetails.SelectedDataKey[0]);
            FamilyDetail familyDetail = _presenter.GetFamilyDetail(famId);
            txtFamFirstName.Text = familyDetail.FirstName;
            txtFamLastName.Text = familyDetail.LastName;
            txtFamCellPhone.Text = familyDetail.CellPhone;
            txtFamDateOfBirth.Text = Convert.ToDateTime(familyDetail.DateOfBirth).ToShortDateString();
            txtFamDateOfMarriage.Text = Convert.ToDateTime(familyDetail.DateOfMarriage).ToShortDateString();
            ddlFamGender.SelectedValue = familyDetail.Gender;
            ddlFamRelationship.SelectedValue = familyDetail.Relationship;

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetofamily();", true);
        }

        protected void grvEmergContacts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvEmergContacts.PageIndex = e.NewPageIndex;
            BindEmergencyContacts();
        }

        protected void grvEmergContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            emergId = Convert.ToInt32(grvEmergContacts.SelectedDataKey[0]);
            EmergencyContact emergencyContact = _presenter.GetEmergencyContact(emergId);
            txtEmergFullName.Text = emergencyContact.FullName;
            txtEmergSubCity.Text = emergencyContact.SubCity;
            txtEmergWoreda.Text = emergencyContact.Woreda;
            txtEmergHouseNo.Text = emergencyContact.HouseNo;
            txtEmergTelephoneHome.Text = emergencyContact.TelephoneHome;
            txtEmergTelephoneOffice.Text = emergencyContact.TelephoneOffice;

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetoemergency();", true);
        }

        protected void grvEducations_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvEducations.PageIndex = e.NewPageIndex;
            BindEducations();
        }

        protected void grvEducations_SelectedIndexChanged(object sender, EventArgs e)
        {
            eduId = Convert.ToInt32(grvEducations.SelectedDataKey[0]);
            Education education = _presenter.GetEducation(eduId);
            txtEduInstName.Text = education.InstitutionName;
            txtEduInstType.Text = education.InstitutionType;
            txtEduInstLocation.Text = education.InstitutionLocation;
            txtEduMajor.Text = education.Major;
            txtEduLevel.Text = education.EducationalLevel;
            txtEduGradYear.Text = Convert.ToDateTime(education.GraduationYear).ToShortDateString();
            txtEduSpecialAward.Text = education.SpecialAward;

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetoeducation();", true);
        }

        protected void grvWorkExperiences_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvWorkExperiences.PageIndex = e.NewPageIndex;
            BindWorkExperiences();
        }

        protected void grvWorkExperiences_SelectedIndexChanged(object sender, EventArgs e)
        {
            workExpId = Convert.ToInt32(grvWorkExperiences.SelectedDataKey[0]);
            WorkExperience workExperience = _presenter.GetWorkExperience(workExpId);
            txtWorkEmpName.Text = workExperience.EmployerName;
            txtWorkEmpAddress.Text = workExperience.EmployerAddress;
            txtWorkStartDate.Text = Convert.ToDateTime(workExperience.StartDate).ToShortDateString();
            txtWorkEndDate.Text = Convert.ToDateTime(workExperience.EndDate).ToShortDateString();
            txtWorkJobTitle.Text = workExperience.JobTitle;
            txtWorkTypeOfEmp.Text = workExperience.TypeOfEmployer;

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetowork();", true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _presenter.SaveOrUpdateEmployee();
                Master.ShowMessage(new AppMessage("You've Successfully Updated Your Profile!", Chai.WorkflowManagment.Enums.RMessageType.Info));
                Log.Info(_presenter.CurrentUser().FullName + " has updated his/her Profile");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Violation of UNIQUE KEY"))
                    {
                        Master.ShowMessage(new AppMessage("Please Click Request button Again,There is a duplicate Number", Chai.WorkflowManagment.Enums.RMessageType.Error));
                        //AutoNumber();
                    }
                }
            }
        }

        protected void btnFamSave_Click(object sender, EventArgs e)
        {
            try
            {
                FamilyDetail familyDetail = new FamilyDetail();
                familyDetail.FirstName = txtFamFirstName.Text;
                familyDetail.LastName = txtFamLastName.Text;
                familyDetail.DateOfBirth = Convert.ToDateTime(txtFamDateOfBirth.Text);
                familyDetail.Gender = ddlFamGender.SelectedValue;
                familyDetail.Relationship = ddlFamRelationship.SelectedValue;
                familyDetail.CellPhone = txtFamCellPhone.Text;
                familyDetail.DateOfMarriage = Convert.ToDateTime(txtFamDateOfMarriage.Text);

                _presenter.CurrentEmployee.FamilyDetails.Add(familyDetail);
                _presenter.SaveOrUpdateEmployee(_presenter.CurrentEmployee);
                BindFamilyDetails();
                Master.ShowMessage(new AppMessage("You've Successfully Updated Your Family Information!", Chai.WorkflowManagment.Enums.RMessageType.Info));
                Log.Info(_presenter.CurrentUser().FullName + " has updated his/her Family Information");
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetofamily();", true);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Violation of UNIQUE KEY"))
                    {
                        Master.ShowMessage(new AppMessage("Please Click Request button Again, There is a duplicate Number", Chai.WorkflowManagment.Enums.RMessageType.Error));
                        //AutoNumber();
                    }
                }
            }
        }

        protected void btnEmergSave_Click(object sender, EventArgs e)
        {
            try
            {
                EmergencyContact emergencyContact = new EmergencyContact();
                emergencyContact.FullName = txtEmergFullName.Text;
                emergencyContact.SubCity = txtEmergSubCity.Text;
                emergencyContact.Woreda = txtEmergWoreda.Text;
                emergencyContact.HouseNo = txtEmergHouseNo.Text;
                emergencyContact.TelephoneHome = txtEmergTelephoneHome.Text;
                emergencyContact.TelephoneOffice = txtEmergTelephoneOffice.Text;

                _presenter.CurrentEmployee.EmergencyContacts.Add(emergencyContact);
                _presenter.SaveOrUpdateEmployee(_presenter.CurrentEmployee);
                BindEmergencyContacts();
                Master.ShowMessage(new AppMessage("You've Successfully Updated Your Emergency Contacts!", Chai.WorkflowManagment.Enums.RMessageType.Info));
                Log.Info(_presenter.CurrentUser().FullName + " has updated his/her Emergency Contacts");
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetoemergency();", true);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Violation of UNIQUE KEY"))
                    {
                        Master.ShowMessage(new AppMessage("Please Click Request button Again, There is a duplicate Number", Chai.WorkflowManagment.Enums.RMessageType.Error));
                        //AutoNumber();
                    }
                }
            }
        }

        protected void btnEduSave_Click(object sender, EventArgs e)
        {
            try
            {
                Education education = new Education();
                education.InstitutionType = txtEduInstType.Text;
                education.InstitutionName = txtEduInstName.Text;
                education.InstitutionLocation = txtEduInstLocation.Text;
                education.Major = txtEduMajor.Text;
                education.EducationalLevel = txtEduLevel.Text;
                education.GraduationYear = Convert.ToDateTime(txtEduGradYear.Text);
                education.SpecialAward = txtEduSpecialAward.Text;

                _presenter.CurrentEmployee.Educations.Add(education);
                _presenter.SaveOrUpdateEmployee(_presenter.CurrentEmployee);
                BindEducations();
                Master.ShowMessage(new AppMessage("You've Successfully Updated Your Education Information!", Chai.WorkflowManagment.Enums.RMessageType.Info));
                Log.Info(_presenter.CurrentUser().FullName + " has updated his/her Education Information");
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetoeducation();", true);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Violation of UNIQUE KEY"))
                    {
                        Master.ShowMessage(new AppMessage("Please Click Request button Again, There is a duplicate Number", Chai.WorkflowManagment.Enums.RMessageType.Error));
                        //AutoNumber();
                    }
                }
            }
        }

        protected void btnWorkSave_Click(object sender, EventArgs e)
        {
            try
            {
                WorkExperience workExperience = new WorkExperience();
                workExperience.EmployerName = txtWorkEmpName.Text;
                workExperience.EmployerAddress = txtWorkEmpAddress.Text;
                workExperience.StartDate = Convert.ToDateTime(txtWorkStartDate.Text);
                workExperience.EndDate = Convert.ToDateTime(txtWorkEndDate.Text);
                workExperience.JobTitle = txtWorkJobTitle.Text;
                workExperience.TypeOfEmployer = txtWorkTypeOfEmp.Text;

                _presenter.CurrentEmployee.WorkExperiences.Add(workExperience);
                _presenter.SaveOrUpdateEmployee(_presenter.CurrentEmployee);
                BindWorkExperiences();
                Master.ShowMessage(new AppMessage("You've Successfully Updated Your Work Experiences!", Chai.WorkflowManagment.Enums.RMessageType.Info));
                Log.Info(_presenter.CurrentUser().FullName + " has updated his/her Work Experiences");
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetowork();", true);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Violation of UNIQUE KEY"))
                    {
                        Master.ShowMessage(new AppMessage("Please Click Request button Again, There is a duplicate Number", Chai.WorkflowManagment.Enums.RMessageType.Error));
                        //AutoNumber();
                    }
                }
            }
        }

    }
}

