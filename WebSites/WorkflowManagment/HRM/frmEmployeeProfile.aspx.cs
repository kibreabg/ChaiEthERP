using System;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Shared;
using log4net;
using Chai.WorkflowManagment.CoreDomain.HRM;
using System.Web.UI.WebControls;
using System.Web.UI;
using Chai.WorkflowManagment.Enums;
using System.IO;

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
            }
            this._presenter.OnViewLoaded();
            if (!this.IsPostBack)
            {
                BindEmployee();
                BindFamilyDetails();
                BindEmergencyContacts();
                BindEducations();
                BindWorkExperiences();
            }

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
            get
            {
                string fileName = String.Empty;
                if (fuProfilePic.HasFile)
                {
                    fileName = _presenter.CurrentAppUser.UserName + Path.GetExtension(fuProfilePic.PostedFile.FileName);
                    fuProfilePic.PostedFile.SaveAs(Server.MapPath("~/ProfilePics/") + fileName);
                }
                else if (!String.IsNullOrEmpty(hfProfilePic.Value))
                {
                    return hfProfilePic.Value;
                }

                return "~/ProfilePics/" + fileName;
            }
        }

        #endregion

        private void BindEmployee()
        {
            if (_presenter.CurrentAppUser.Employee == null)
            {
                txtFirstName.Text = _presenter.CurrentUser().FirstName;
                txtLastName.Text = _presenter.CurrentUser().LastName;
                txtChaiEmail.Text = _presenter.CurrentUser().Email;
                txtPersonalEmail.Text = _presenter.CurrentUser().PersonalEmail;
            }
            if (_presenter.CurrentAppUser.Employee != null)
            {
                txtFirstName.Text = _presenter.CurrentAppUser.Employee.FirstName;
                txtLastName.Text = _presenter.CurrentAppUser.Employee.LastName;
                ddlGender.SelectedValue = _presenter.CurrentAppUser.Employee.Gender;
                txtDateOfBirth.Text = Convert.ToDateTime(_presenter.CurrentAppUser.Employee.DateOfBirth).ToShortDateString();
                ddlMaritalStatus.SelectedValue = _presenter.CurrentAppUser.Employee.MaritalStatus;
                txtNationality.Text = _presenter.CurrentAppUser.Employee.Nationality;
                txtPhone.Text = _presenter.CurrentAppUser.Employee.Phone;
                txtCellPhone.Text = _presenter.CurrentAppUser.Employee.CellPhone;
                txtChaiEmail.Text = _presenter.CurrentAppUser.Employee.ChaiEMail;
                txtPersonalEmail.Text = _presenter.CurrentAppUser.Employee.PersonalEmail;
                txtCountry.Text = _presenter.CurrentAppUser.Employee.Country;
                txtCity.Text = _presenter.CurrentAppUser.Employee.City;
                txtAddress.Text = _presenter.CurrentAppUser.Employee.Address;
                imgProfilePic.ImageUrl = _presenter.CurrentAppUser.Employee.Photo;
                hfProfilePic.Value = _presenter.CurrentAppUser.Employee.Photo;
            }


        }
        private void BindFamilyDetails()
        {
            if (_presenter.CurrentAppUser.Employee != null)
            {
                grvFamilyDetails.DataSource = _presenter.CurrentAppUser.Employee.FamilyDetails;
                grvFamilyDetails.DataBind();
                if (grvFamilyDetails.Columns.Count > 0 && !_presenter.CurrentUser().EmployeePosition.PositionName.Equals("Head, Administration & HR"))
                    grvFamilyDetails.Columns[6].Visible = false;
            }
        }
        private void BindEmergencyContacts()
        {
            if (_presenter.CurrentAppUser.Employee != null)
            {
                grvEmergContacts.DataSource = _presenter.CurrentAppUser.Employee.EmergencyContacts;
                grvEmergContacts.DataBind();
            }
        }
        private void BindEducations()
        {
            if (_presenter.CurrentAppUser.Employee != null)
            {
                grvEducations.DataSource = _presenter.CurrentAppUser.Employee.Educations;
                grvEducations.DataBind();
                if (grvEducations.Columns.Count > 0 && !_presenter.CurrentUser().EmployeePosition.PositionName.Equals("Head, Administration & HR"))
                    grvEducations.Columns[8].Visible = false;
            }
        }
        private void BindWorkExperiences()
        {
            if (_presenter.CurrentAppUser.Employee != null)
            {
                grvWorkExperiences.DataSource = _presenter.CurrentAppUser.Employee.WorkExperiences;
                grvWorkExperiences.DataBind();
            }
        }
        private void clearFamilyDetails()
        {
            txtFamFirstName.Text = String.Empty;
            txtFamLastName.Text = String.Empty;
            txtFamCellPhone.Text = String.Empty;
            txtFamDateOfBirth.Text = String.Empty;
            txtFamDateOfMarriage.Text = String.Empty;
            ddlFamGender.SelectedValue = "";
            ddlFamRelationship.SelectedValue = "";
            btnFamDelete.Enabled = false;
            btnFamSave.Text = "Save & Add New";
        }
        private void clearEmergencyContacts()
        {
            txtEmergCellPhone.Text = String.Empty;
            txtEmergFullName.Text = String.Empty;
            txtEmergHouseNo.Text = String.Empty;
            txtEmergSubCity.Text = String.Empty;
            txtEmergTelephoneHome.Text = String.Empty;
            txtEmergTelephoneOffice.Text = String.Empty;
            txtEmergWoreda.Text = String.Empty;
            ddlEmergRelationship.SelectedValue = "";
            btnEmergDelete.Enabled = false;
            btnEmergSave.Text = "Save & Add New";
        }
        private void clearEducations()
        {
            txtEduGradYear.Text = String.Empty;
            txtEduInstLocation.Text = String.Empty;
            txtEduInstName.Text = String.Empty;
            txtEduMajor.Text = String.Empty;
            txtEduSpecialAward.Text = String.Empty;
            ddlEduInstType.SelectedValue = "";
            ddlEduLevel.SelectedValue = "";
            btnEduDelete.Enabled = false;
            btnEduSave.Text = "Save & Add New";
        }
        private void clearWorkExperiences()
        {
            txtWorkEndDate.Text = String.Empty;
            txtWorkJobTitle.Text = String.Empty;
            txtWorkOrgAddress.Text = String.Empty;
            txtWorkOrgName.Text = String.Empty;
            txtWorkStartDate.Text = String.Empty;
            ddlWorkTypeOfEmp.SelectedValue = String.Empty;
            btnWorkExpDelete.Enabled = false;
            btnWorkSave.Text = "Save & Add New";
        }

        protected void grvFamilyDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvFamilyDetails.PageIndex = e.NewPageIndex;
            BindFamilyDetails();
        }

        protected void grvFamilyDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            famId = Convert.ToInt32(grvFamilyDetails.SelectedDataKey[0]);
            Session["famId"] = Convert.ToInt32(grvFamilyDetails.SelectedDataKey[0]);
            FamilyDetail familyDetail = _presenter.CurrentAppUser.Employee.GetFamilyDetail(famId);
            txtFamFirstName.Text = familyDetail.FirstName;
            txtFamLastName.Text = familyDetail.LastName;
            txtFamCellPhone.Text = familyDetail.CellPhone;
            txtFamDateOfBirth.Text = Convert.ToDateTime(familyDetail.DateOfBirth).ToShortDateString();
            txtFamDateOfMarriage.Text = Convert.ToDateTime(familyDetail.DateOfMarriage).ToShortDateString();
            ddlFamGender.SelectedValue = familyDetail.Gender;
            ddlFamRelationship.SelectedValue = familyDetail.Relationship;
            btnFamSave.Text = "Update";
            btnFamDelete.Enabled = true;

            #region Relationship Logic
            //Handle the Relationship logic Regarding Dates being visible or not
            if (ddlFamRelationship.SelectedValue == "Spouse")
            {
                pnlFamDateOfMarriage.Visible = true;
                pnlFamCertificate.Visible = true;
                pnlFamDateOfBirth.Visible = false;
                txtFamDateOfBirth.Text = String.Empty;
            }
            else if (ddlFamRelationship.SelectedValue == "Child")
            {
                pnlFamDateOfBirth.Visible = true;
                pnlFamCertificate.Visible = true;
                pnlFamDateOfMarriage.Visible = false;
                txtFamDateOfMarriage.Text = String.Empty;
            }
            else if (ddlFamRelationship.SelectedValue == "Parent")
            {
                pnlFamDateOfMarriage.Visible = false;
                pnlFamDateOfBirth.Visible = false;
                pnlFamCertificate.Visible = false;
                txtFamDateOfMarriage.Text = String.Empty;
                txtFamDateOfBirth.Text = String.Empty;
            }
            #endregion

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
            Session["emergContId"] = Convert.ToInt32(grvEmergContacts.SelectedDataKey[0]);
            EmergencyContact emergencyContact = _presenter.CurrentAppUser.Employee.GetEmergencyContact(emergId);
            txtEmergFullName.Text = emergencyContact.FullName;
            txtEmergSubCity.Text = emergencyContact.SubCity;
            txtEmergWoreda.Text = emergencyContact.Woreda;
            txtEmergHouseNo.Text = emergencyContact.HouseNo;
            txtEmergTelephoneHome.Text = emergencyContact.TelephoneHome;
            txtEmergTelephoneOffice.Text = emergencyContact.TelephoneOffice;
            ddlEmergRelationship.SelectedValue = emergencyContact.Relationship;
            txtEmergCellPhone.Text = emergencyContact.CellPhone;
            ckIsPrimary.Checked = emergencyContact.IsPrimaryContact.Value;
            btnEmergDelete.Enabled = true;
            btnEmergSave.Text = "Update";
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
            Session["eduId"] = Convert.ToInt32(grvEducations.SelectedDataKey[0]);
            Education education = _presenter.CurrentAppUser.Employee.GetEducation(eduId);
            txtEduInstName.Text = education.InstitutionName;
            ddlEduInstType.SelectedValue = education.InstitutionType;
            txtEduInstLocation.Text = education.InstitutionLocation;
            txtEduMajor.Text = education.Major;
            ddlEduLevel.SelectedValue = education.EducationalLevel;
            txtEduGradYear.Text = Convert.ToDateTime(education.GraduationYear).ToShortDateString();
            txtEduSpecialAward.Text = education.SpecialAward;
            btnEduSave.Text = "Update";
            btnEduDelete.Enabled = true;

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
            Session["workExpId"] = Convert.ToInt32(grvWorkExperiences.SelectedDataKey[0]);
            WorkExperience workExperience = _presenter.CurrentAppUser.Employee.GetWorkExperience(workExpId);
            txtWorkOrgName.Text = workExperience.EmployerName;
            txtWorkOrgAddress.Text = workExperience.EmployerAddress;
            txtWorkStartDate.Text = Convert.ToDateTime(workExperience.StartDate).ToShortDateString();
            txtWorkEndDate.Text = Convert.ToDateTime(workExperience.EndDate).ToShortDateString();
            txtWorkJobTitle.Text = workExperience.JobTitle;
            ddlWorkTypeOfEmp.SelectedValue = workExperience.TypeOfEmployer;
            btnWorkSave.Text = "Update";
            btnWorkExpDelete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetowork();", true);
        }

        protected void ddlFamRelationship_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFamRelationship.SelectedValue == "Spouse")
            {
                pnlFamDateOfMarriage.Visible = true;
                pnlFamCertificate.Visible = true;
                pnlFamDateOfBirth.Visible = false;
                txtFamDateOfBirth.Text = String.Empty;
            }
            else if (ddlFamRelationship.SelectedValue == "Child")
            {
                pnlFamDateOfBirth.Visible = true;
                pnlFamCertificate.Visible = true;
                pnlFamDateOfMarriage.Visible = false;
                txtFamDateOfMarriage.Text = String.Empty;
            }
            else if (ddlFamRelationship.SelectedValue == "Parent")
            {
                pnlFamDateOfMarriage.Visible = false;
                pnlFamDateOfBirth.Visible = false;
                pnlFamCertificate.Visible = false;
                txtFamDateOfMarriage.Text = String.Empty;
                txtFamDateOfBirth.Text = String.Empty;
            }

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetofamily();", true);

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _presenter.SaveOrUpdateEmployee();
                BindEmployee();
                Master.ShowMessage(new AppMessage("You've Successfully Updated Your Profile!", RMessageType.Info));
                Log.Info(_presenter.CurrentUser().FullName + " has updated his/her Profile");
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: While Updating Employee Profile!", RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void btnFamSave_Click(object sender, EventArgs e)
        {
            try
            {
                FamilyDetail familyDetail = null;
                if (Session["famId"] != null)
                    familyDetail = _presenter.CurrentAppUser.Employee.GetFamilyDetail(Convert.ToInt32(Session["famId"]));
                else
                    familyDetail = new FamilyDetail();

                familyDetail.FirstName = txtFamFirstName.Text;
                familyDetail.LastName = txtFamLastName.Text;
                if (!String.IsNullOrEmpty(txtFamDateOfBirth.Text))
                    familyDetail.DateOfBirth = Convert.ToDateTime(txtFamDateOfBirth.Text);
                else
                    familyDetail.DateOfBirth = null;
                familyDetail.Gender = ddlFamGender.SelectedValue;
                familyDetail.Relationship = ddlFamRelationship.SelectedValue;
                familyDetail.CellPhone = txtFamCellPhone.Text;
                if (!String.IsNullOrEmpty(txtFamDateOfMarriage.Text))
                    familyDetail.DateOfMarriage = Convert.ToDateTime(txtFamDateOfMarriage.Text);
                else
                    familyDetail.DateOfMarriage = null;
                //Attached Certificates
                if (fuCertificate.HasFile)
                {
                    string fileName = "Fam" + _presenter.CurrentAppUser.UserName + Path.GetFileName(fuCertificate.PostedFile.FileName);
                    familyDetail.Certificate = "~/Certificates/" + fileName;
                    fuCertificate.PostedFile.SaveAs(Server.MapPath("~/Certificates/") + fileName);
                }

                if (Session["famId"] == null)
                    _presenter.CurrentAppUser.Employee.FamilyDetails.Add(familyDetail);
                _presenter.SaveOrUpdateEmployee(_presenter.CurrentAppUser);
                BindFamilyDetails();
                clearFamilyDetails();
                Session["famId"] = null;
                btnFamSave.Text = "Save & Add New";
                Master.ShowMessage(new AppMessage("You've Successfully Updated Your Family Information!", RMessageType.Info));
                Log.Info(_presenter.CurrentUser().FullName + " has updated his/her Family Information");
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetofamily();", true);
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: While Updating Family Information!", RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void btnEmergSave_Click(object sender, EventArgs e)
        {
            try
            {
                EmergencyContact emergencyContact = null;
                if (Session["emergContId"] != null)
                    emergencyContact = _presenter.CurrentAppUser.Employee.GetEmergencyContact(Convert.ToInt32(Session["emergContId"]));
                else
                    emergencyContact = new EmergencyContact();

                emergencyContact.FullName = txtEmergFullName.Text;
                emergencyContact.Relationship = ddlEmergRelationship.SelectedValue;
                emergencyContact.SubCity = txtEmergSubCity.Text;
                emergencyContact.Woreda = txtEmergWoreda.Text;
                emergencyContact.HouseNo = txtEmergHouseNo.Text;
                emergencyContact.TelephoneHome = txtEmergTelephoneHome.Text;
                emergencyContact.TelephoneOffice = txtEmergTelephoneOffice.Text;
                emergencyContact.CellPhone = txtEmergCellPhone.Text;
                emergencyContact.IsPrimaryContact = ckIsPrimary.Checked;

                if (Session["emergContId"] == null)
                    _presenter.CurrentAppUser.Employee.EmergencyContacts.Add(emergencyContact);
                _presenter.SaveOrUpdateEmployee(_presenter.CurrentAppUser);
                btnEmergSave.Text = "Save & Add New";
                BindEmergencyContacts();
                clearEmergencyContacts();
                Session["emergContId"] = null;
                Master.ShowMessage(new AppMessage("You've Successfully Updated Your Emergency Contacts!", RMessageType.Info));
                Log.Info(_presenter.CurrentUser().FullName + " has updated his/her Emergency Contacts");
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetoemergency();", true);
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: While Updating Emergency Contact Information!", RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void btnEduSave_Click(object sender, EventArgs e)
        {
            try
            {
                Education education = null;
                if (Session["eduId"] != null)
                    education = _presenter.CurrentAppUser.Employee.GetEducation(Convert.ToInt32(Session["eduId"]));
                else
                    education = new Education();

                education.InstitutionType = ddlEduInstType.SelectedValue;
                education.InstitutionName = txtEduInstName.Text;
                education.InstitutionLocation = txtEduInstLocation.Text;
                education.Major = txtEduMajor.Text;
                education.EducationalLevel = ddlEduLevel.SelectedValue;
                education.GraduationYear = Convert.ToDateTime(txtEduGradYear.Text);
                education.SpecialAward = txtEduSpecialAward.Text;
                //Attached Certificates

                if (fuEduCertificate.HasFile)
                {
                    string fileName = "Edu" + _presenter.CurrentAppUser.UserName + Path.GetFileName(fuEduCertificate.PostedFile.FileName);
                    education.Certificate = "~/Certificates/" + fileName;
                    fuEduCertificate.PostedFile.SaveAs(Server.MapPath("~/Certificates/") + fileName);
                }

                if (Session["eduId"] == null)
                    _presenter.CurrentAppUser.Employee.Educations.Add(education);
                _presenter.SaveOrUpdateEmployee(_presenter.CurrentAppUser);
                Session["eduId"] = null;
                BindEducations();
                clearEducations();
                btnEduSave.Text = "Save & Add New";
                Master.ShowMessage(new AppMessage("You've Successfully Updated Your Education Information!", RMessageType.Info));
                Log.Info(_presenter.CurrentUser().FullName + " has updated his/her Education Information");
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetoeducation();", true);
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: While Updating Education Information!", RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void btnWorkSave_Click(object sender, EventArgs e)
        {
            try
            {
                WorkExperience workExperience = null;
                if (Session["workExpId"] != null)
                    workExperience = _presenter.CurrentAppUser.Employee.GetWorkExperience(Convert.ToInt32(Session["workExpId"]));
                else
                    workExperience = new WorkExperience();

                workExperience.EmployerName = txtWorkOrgName.Text;
                workExperience.EmployerAddress = txtWorkOrgAddress.Text;
                workExperience.StartDate = Convert.ToDateTime(txtWorkStartDate.Text);
                workExperience.EndDate = Convert.ToDateTime(txtWorkEndDate.Text);
                workExperience.JobTitle = txtWorkJobTitle.Text;
                workExperience.TypeOfEmployer = ddlWorkTypeOfEmp.SelectedValue;

                if (Session["workExpId"] == null)
                    _presenter.CurrentAppUser.Employee.WorkExperiences.Add(workExperience);
                _presenter.SaveOrUpdateEmployee(_presenter.CurrentAppUser);
                BindWorkExperiences();
                clearWorkExperiences();
                Session["workExpId"] = null;
                btnWorkSave.Text = "Save & Add New";
                Master.ShowMessage(new AppMessage("You've Successfully Updated Your Work Experiences!", RMessageType.Info));
                Log.Info(_presenter.CurrentUser().FullName + " has updated his/her Work Experiences");
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetowork();", true);
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: While Updating Work Experience!", RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void lnkEduDownload_Clicked(object sender, EventArgs e)
        {
            string certificatePath = (sender as LinkButton).CommandArgument;
            imgCertPreview.ImageUrl = certificatePath;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetoeducation();previewImage('certPreview','edu');", true);
        }
        protected void lnkFamDownload_Clicked(object sender, EventArgs e)
        {
            string certificatePath = (sender as LinkButton).CommandArgument;
            imgCertPreview.ImageUrl = certificatePath;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "previewImage('certPreview','fam');movetofamily();", true);

        }
        protected void btnFamDelete_Click(object sender, EventArgs e)
        {
            try
            {
                _presenter.CurrentAppUser.Employee.RemoveFamilyDetail(Convert.ToInt32(Session["famId"]));
                FamilyDetail delFamilyDetail = _presenter.GetFamilyDetail(Convert.ToInt32(Session["famId"]));
                if (delFamilyDetail != null)
                    _presenter.DeleteFamilyDetail(delFamilyDetail);
                _presenter.SaveOrUpdateEmployee(_presenter.CurrentAppUser);
                Session["famId"] = null;
                //Clear the fields
                clearFamilyDetails();
                BindFamilyDetails();
                btnFamDelete.Enabled = false;
                Master.ShowMessage(new AppMessage("Family Information Is Successfully Deleted!", RMessageType.Info));
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetofamily();", true);
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: While Deleting Family Detail!", RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }

        }
        protected void btnEmergDelete_Click(object sender, EventArgs e)
        {
            try
            {
                _presenter.CurrentAppUser.Employee.RemoveEmergencyContact(Convert.ToInt32(Session["emergContId"]));
                EmergencyContact delEmergContact = _presenter.GetEmergencyContact(Convert.ToInt32(Session["emergContId"]));
                if (delEmergContact != null)
                    _presenter.DeleteEmergencyContact(delEmergContact);
                _presenter.SaveOrUpdateEmployee(_presenter.CurrentAppUser);
                Session["emergContId"] = null;
                //Clear the fields
                clearEmergencyContacts();
                BindEmergencyContacts();
                btnEmergDelete.Enabled = false;
                Master.ShowMessage(new AppMessage("Emergency Contact Information Is Successfully Deleted!", RMessageType.Info));
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetoemergency();", true);
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: While Deleting Emergency Contact!", RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void btnEduDelete_Click(object sender, EventArgs e)
        {
            try
            {
                _presenter.CurrentAppUser.Employee.RemoveEducation(Convert.ToInt32(Session["eduId"]));
                Education delEducation = _presenter.GetEducation(Convert.ToInt32(Session["eduId"]));
                if (delEducation != null)
                    _presenter.DeleteEducation(delEducation);
                _presenter.SaveOrUpdateEmployee(_presenter.CurrentAppUser);
                Session["eduId"] = null;
                //Clear the fields
                clearEducations();
                BindEducations();
                btnEduDelete.Enabled = false;
                Master.ShowMessage(new AppMessage("Education Information Is Successfully Deleted!", RMessageType.Info));
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetoeducation();", true);
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: While Deleting Education Information!", RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void btnWorkExpDelete_Click(object sender, EventArgs e)
        {
            try
            {
                _presenter.CurrentAppUser.Employee.RemoveWorkExperience(Convert.ToInt32(Session["workExpId"]));
                WorkExperience delWorkExp = _presenter.GetWorkExperience(Convert.ToInt32(Session["workExpId"]));
                if (delWorkExp != null)
                    _presenter.DeleteWorkExperience(delWorkExp);
                _presenter.SaveOrUpdateEmployee(_presenter.CurrentAppUser);
                Session["workExpId"] = null;
                //Clear the fields
                clearWorkExperiences();
                BindWorkExperiences();
                btnWorkExpDelete.Enabled = false;
                Master.ShowMessage(new AppMessage("Work Experience Is Successfully Deleted!", RMessageType.Info));
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetowork();", true);
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: While Deleting Work Experience!", RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
        protected void btnFamCancel_Click(object sender, EventArgs e)
        {
            clearFamilyDetails();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetofamily();", true);
        }
        protected void btnEmergCancel_Click(object sender, EventArgs e)
        {
            clearEmergencyContacts();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetoemergency();", true);
        }
        protected void btnEduCancel_Click(object sender, EventArgs e)
        {
            clearEducations();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetoeducation();", true);
        }
        protected void btnWorkCancel_Click(object sender, EventArgs e)
        {
            clearWorkExperiences();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetowork();", true);
        }
        protected void ckFamCertReview_Check(object sender, EventArgs e)
        {
            try
            {
                CheckBox ckBox = (CheckBox)sender;
                int rowIndex = Convert.ToInt32(ckBox.Attributes["RowIndex"]);
                HiddenField hfFamilyId = grvFamilyDetails.Rows[rowIndex].FindControl("hfFamId") as HiddenField;
                FamilyDetail familyDetail = _presenter.CurrentAppUser.Employee.GetFamilyDetail(Convert.ToInt32(hfFamilyId.Value));
                familyDetail.Reviewed = true;
                _presenter.SaveOrUpdateEmployee(_presenter.CurrentAppUser);
                Master.ShowMessage(new AppMessage("You've Successfully Reviewed this Certificate!", RMessageType.Info));
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetofamily();", true);
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: While Reviewing Certificate!", RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }

        }
        protected void ckEduCertReview_Check(object sender, EventArgs e)
        {
            try
            {
                CheckBox ckBox = (CheckBox)sender;
                int rowIndex = Convert.ToInt32(ckBox.Attributes["RowIndex"]);
                HiddenField hfEducationId = grvEducations.Rows[rowIndex].FindControl("hfEduId") as HiddenField;
                Education education = _presenter.CurrentAppUser.Employee.GetEducation(Convert.ToInt32(hfEducationId.Value));
                education.Reviewed = true;
                _presenter.SaveOrUpdateEmployee(_presenter.CurrentAppUser);
                Master.ShowMessage(new AppMessage("You've Successfully Reviewed this Certificate!", RMessageType.Info));
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "movetoeducation();", true);
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: While Reviewing Certificate!", RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }

        }

    }
}

