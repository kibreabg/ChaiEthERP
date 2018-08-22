using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.CoreDomain.HRM;

namespace Chai.WorkflowManagment.Modules.HRM.Views
{
    public class EmployeeProfilePresenter : Presenter<IEmployeeProfileView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private HRMController _controller;
        private Employee _employee;
        public EmployeeProfilePresenter([CreateNew] HRMController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
            if (View.GetEmployeeId > 0)
            {
                _controller.CurrentObject = _controller.GetEmployee(View.GetEmployeeId);
            }
            CurrentEmployee = _controller.CurrentObject as Employee;
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            if (_employee == null)
            {
                int id = View.GetEmployeeId;
                if (id > 0)
                    _controller.CurrentObject = _controller.GetEmployee(id);
                else
                    _controller.CurrentObject = new Employee();
            }
        }

        public Employee CurrentEmployee
        {
            get
            {
                if (_employee == null)
                {
                    int id = View.GetEmployeeId;
                    if (id > 0)
                        _employee = _controller.GetEmployee(id);
                    else
                        _employee = new Employee();
                }
                return _employee;
            }
            set
            {
                _employee = value;
            }
        }

        // TODO: Handle other view events and set state in the view
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }

        public FamilyDetail GetFamilyDetail(int famId)
        {
            return _controller.GetFamilyDetail(famId);
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

        public void SaveOrUpdateEmployee()
        {
            Employee Employee = CurrentEmployee;
            Employee.FirstName = View.GetFirstName;
            Employee.LastName = View.GetLastName;
            Employee.Gender = View.GetGender;
            Employee.DateOfBirth = View.GetDateOfBirth;
            Employee.MaritalStatus = View.GetMaritalStatus;
            Employee.Nationality = View.GetNationality;
            Employee.Address = View.GetAddress;
            Employee.City = View.GetCity;
            Employee.Country = View.GetCountry;
            Employee.Phone = View.GetPhone;
            Employee.CellPhone = View.GetCellPhone;
            Employee.PersonalEmail = View.GetPersonalEmail;
            Employee.ChaiEMail = View.GetChaiEmail;
            Employee.Status = true;

            _controller.SaveOrUpdateEntity(Employee);
            _controller.CurrentObject = null;
        }

        public void SaveOrUpdateEmployee(Employee currentEmployee)
        {
            _controller.SaveOrUpdateEntity(currentEmployee);
            _controller.CurrentObject = null;
        }        
    }
}




