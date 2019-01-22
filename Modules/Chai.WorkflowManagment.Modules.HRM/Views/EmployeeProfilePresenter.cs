using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.CoreDomain.HRM;
using Chai.WorkflowManagment.Modules.Admin;

namespace Chai.WorkflowManagment.Modules.HRM.Views
{
    public class EmployeeProfilePresenter : Presenter<IEmployeeProfileView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private HRMController _controller;
        private AppUser _appUser;
        private AdminController _adminController;
        public EmployeeProfilePresenter([CreateNew] HRMController controller, AdminController adminController)
        {
            _controller = controller;
            _adminController = adminController;
        }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
            if (View.GetEmployeeId > 0)
            {
                _controller.CurrentObject = _adminController.GetUser(View.GetEmployeeId);
            }
            CurrentAppUser = _controller.CurrentObject as AppUser;
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            if (_appUser == null)
            {
                if (View.GetEmployeeId > 0)
                    _controller.CurrentObject = _adminController.GetUser(View.GetEmployeeId);
                else
                    _controller.CurrentObject = new AppUser();
            }
        }

        public AppUser CurrentAppUser
        {
            get
            {
                if (_appUser == null)
                {
                    int id = CurrentUser().Id;
                    if (id > 0)
                        _appUser = _adminController.GetUser(id);
                    else
                        _appUser = new AppUser();
                }
                return _appUser;
            }
            set
            {
                _appUser = value;
            }
        }

        // TODO: Handle other view events and set state in the view
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }

        public void SaveOrUpdateEmployee()
        {
            Employee employee = null;
            if (CurrentAppUser.Employee == null)
                employee = new Employee();
            else
                employee = CurrentAppUser.Employee;
            employee.FirstName = View.GetFirstName;
            employee.LastName = View.GetLastName;
            employee.Gender = View.GetGender;
            employee.DateOfBirth = View.GetDateOfBirth;
            employee.MaritalStatus = View.GetMaritalStatus;
            employee.Nationality = View.GetNationality;
            employee.Address = View.GetAddress;
            employee.City = View.GetCity;
            employee.Country = View.GetCountry;
            employee.Phone = View.GetPhone;
            employee.CellPhone = View.GetCellPhone;
            employee.PersonalEmail = View.GetPersonalEmail;
            employee.ChaiEMail = View.GetChaiEmail;
            if (employee.Id <= 0)
            {
                employee.SDLeaveBalance = 0;
                employee.LeaveSettingDate = DateTime.Now.Date;
            }
            employee.Photo = View.GetPhoto;
            CurrentAppUser.Employee = employee;
            CurrentAppUser.Employee.AppUser = CurrentAppUser;

            _controller.SaveOrUpdateEntity(CurrentAppUser);
            _controller.CurrentObject = null;
        }
        public void SaveOrUpdateEmployee(AppUser currentAppUser)
        {
            _controller.SaveOrUpdateEntity(currentAppUser);
            _controller.CurrentObject = null;
        }
        public FamilyDetail GetFamilyDetail(int famDetId)
        {
            return _controller.GetFamilyDetail(famDetId);
        }
        public EmergencyContact GetEmergencyContact(int emergId)
        {
            return _controller.GetEmergencyContact(emergId);
        }
        public Education GetEducation(int eduId)
        {
            return _controller.GetEducation(eduId);
        }
        public WorkExperience GetWorkExperience(int workExpId)
        {
            return _controller.GetWorkExperience(workExpId);
        }
        public void DeleteFamilyDetail(FamilyDetail familyDetail)
        {
            _controller.DeleteEntity(familyDetail);
        }
        public void DeleteEmergencyContact(EmergencyContact emergContact)
        {
            _controller.DeleteEntity(emergContact);
        }
        public void DeleteEducation(Education education)
        {
            _controller.DeleteEntity(education);
        }
        public void DeleteWorkExperience(WorkExperience workExperience)
        {
            _controller.DeleteEntity(workExperience);
        }


    }
}




