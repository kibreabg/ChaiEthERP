using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.CoreDomain;
using Chai.WorkflowManagment.CoreDomain.DataAccess;
using Chai.WorkflowManagment.Services;
using Chai.WorkflowManagment.Shared.Navigation;


using System.Data;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.HRM;

namespace Chai.WorkflowManagment.Modules.HRM
{
    public class HRMController : ControllerBase
    {
        private IWorkspace _workspace;

        [InjectionConstructor]
        public HRMController([ServiceDependency] IHttpContextLocatorService httpContextLocatorService, [ServiceDependency]INavigationService navigationService)
            : base(httpContextLocatorService, navigationService)
        {
            _workspace = ZadsServices.Workspace;
        }

        #region Employee
        public IList<Employee> GetEmployees()
        {
            return WorkspaceFactory.CreateReadOnly().Query<Employee>(null).ToList();
        }
        public Employee GetEmployee(int id)
        {
            return _workspace.Single<Employee>(x => x.Id == id);
        }
        public IList<Employee> ListEmployees(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM Employees Where 1 = Case when '" + RequestNo + "' = '' Then 1 When Employees.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When Employees.RequestDate = '" + RequestDate + "'  Then 1 END And Employees.AppUser_Id='" + GetCurrentUser().Id + "' order by Employees.Id Desc ";

            return _workspace.SqlQuery<Employee>(filterExpression).ToList();

        }
        public int GetLastEmployeeId()
        {
            if (_workspace.Last<Employee>() != null)
            {
                return _workspace.Last<Employee>().Id;
            }
            else { return 0; }
        }
        #endregion
        #region Family Detail
        public IList<FamilyDetail> GetFamilyDetails()
        {
            return WorkspaceFactory.CreateReadOnly().Query<FamilyDetail>(null).ToList();
        }
        public FamilyDetail GetFamilyDetail(int id)
        {
            return _workspace.Single<FamilyDetail>(x => x.Id == id);
        }
        public IList<FamilyDetail> ListFamilyDetails()
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM FamilyDetails ORDER BY FamilyDetails.Id Desc ";

            return _workspace.SqlQuery<FamilyDetail>(filterExpression).ToList();

        }      

        public int GetLastFamilyDetailId()
        {
            if (_workspace.Last<FamilyDetail>() != null)
            {
                return _workspace.Last<FamilyDetail>().Id;
            }
            else { return 0; }
        }        
        #endregion
        #region Emergency Contact
        public IList<EmergencyContact> GetEmergencyContacts()
        {
            return WorkspaceFactory.CreateReadOnly().Query<EmergencyContact>(null).ToList();
        }
        public EmergencyContact GetEmergencyContact(int id)
        {
            return _workspace.Single<EmergencyContact>(x => x.Id == id);
        }
        public IList<EmergencyContact> ListEmergencyContacts()
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM EmergencyContacts ORDER BY EmergencyContacts.Id Desc ";

            return _workspace.SqlQuery<EmergencyContact>(filterExpression).ToList();

        }
        public int GetLastEmergencyContactId()
        {
            if (_workspace.Last<EmergencyContact>() != null)
            {
                return _workspace.Last<EmergencyContact>().Id;
            }
            else { return 0; }
        }
        #endregion
        #region Education
        public IList<Education> GetEducations()
        {
            return WorkspaceFactory.CreateReadOnly().Query<Education>(null).ToList();
        }
        public Education GetEducation(int id)
        {
            return _workspace.Single<Education>(x => x.Id == id);
        }
        public IList<Education> ListEducations()
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM Educations ORDER BY Educations.Id Desc ";

            return _workspace.SqlQuery<Education>(filterExpression).ToList();

        }
        public int GetLastEducationId()
        {
            if (_workspace.Last<Education>() != null)
            {
                return _workspace.Last<Education>().Id;
            }
            else { return 0; }
        }
        #endregion
        #region Work Experience
        public IList<WorkExperience> GetWorkExperiences()
        {
            return WorkspaceFactory.CreateReadOnly().Query<WorkExperience>(null).ToList();
        }
        public WorkExperience GetWorkExperience(int id)
        {
            return _workspace.Single<WorkExperience>(x => x.Id == id);
        }
        public IList<WorkExperience> ListWorkExperiences()
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM WorkExperiences ORDER BY WorkExperiences.Id Desc ";

            return _workspace.SqlQuery<WorkExperience>(filterExpression).ToList();

        }
        public int GetLastWorkExperienceId()
        {
            if (_workspace.Last<WorkExperience>() != null)
            {
                return _workspace.Last<WorkExperience>().Id;
            }
            else { return 0; }
        }
        #endregion

        #region CurrentObject
        public object CurrentObject
        {
            get
            {
                return GetCurrentContext().Session["CurrentObject"];
            }
            set
            {
                GetCurrentContext().Session["CurrentObject"] = value;
            }
        }
        #endregion
        #region Employee Search 
        public IList<Employee> ListEmployees(string EmpNo, string FullName,int project)
        {
            string filterExpression = "";
            
           filterExpression = "SELECT * FROM Employees Where 1 = Case when '" + FullName + "' = '' Then 1 When (Employees.FirstName + ' ' + Employees.lastName) = '" + FullName + "' Then 1 END ";
            // return WorkspaceFactory.CreateReadOnly().Queryable<CashPaymentRequest>(filterExpression).ToList();
            return _workspace.SqlQuery<Employee>(filterExpression).ToList();
        }
        #endregion
        #region Entity Manipulation
        public void SaveOrUpdateEntity<T>(T item) where T : class
        {
            IEntity entity = (IEntity)item;
            if (entity.Id == 0)
                _workspace.Add<T>(item);
            else
                _workspace.Update<T>(item);

            _workspace.CommitChanges();
            _workspace.Refresh(item);
        }

        public void DeleteEntity<T>(T item) where T : class
        {
            _workspace.Delete<T>(item);
            _workspace.CommitChanges();
            _workspace.Refresh(item);
        }

        public void Commit()
        {
            _workspace.CommitChanges();
        }
        #endregion               
    }
}
