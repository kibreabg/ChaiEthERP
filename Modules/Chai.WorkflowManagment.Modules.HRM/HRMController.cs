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
using Chai.WorkflowManagment.CoreDomain.Request;

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
        public FamilyDetail GetFamilyDetail(int famDetId)
        {
            return _workspace.Single<FamilyDetail>(x => x.Id == famDetId);
        }
        public EmergencyContact GetEmergencyContact(int emergId)
        {
            return _workspace.Single<EmergencyContact>(x => x.Id == emergId);
        }
        public Education GetEducation(int eduId)
        {
            return _workspace.Single<Education>(x => x.Id == eduId);
        }      
        public WorkExperience GetWorkExperience(int workExpId)
        {
            return _workspace.Single<WorkExperience>(x => x.Id == workExpId);
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
        public IList<Employee> ListEmployees(string EmpNo, string FullName, int project)
        {
            string filterExpression = "";
            
           filterExpression = "SELECT * FROM Employees Where 1 = Case when '" + FullName + "' = '' Then 1 When (Employees.FirstName + ' ' + Employees.lastName) = '" + FullName + "' Then 1 END ";
            // return WorkspaceFactory.CreateReadOnly().Queryable<CashPaymentRequest>(filterExpression).ToList();
            return _workspace.SqlQuery<Employee>(filterExpression).ToList();
        }
        public decimal TotalleaveTaken(int EmpId, DateTime Leavedatesetting)
        {
            string filterExpression = "";

            filterExpression = "SELECT *  FROM LeaveRequests Inner Join LeaveTypes on LeaveRequests.LeaveType_Id = LeaveTypes.Id "
                               + " Where LeaveTypes.LeaveTypeName = 'Annual Leave' and LeaveRequests.CurrentStatus= 'Issued' and LeaveRequests.Requester = '" + EmpId + "' and LeaveRequests.RequestedDate >= '" + Leavedatesetting + "'";
            // return WorkspaceFactory.CreateReadOnly().Queryable<CashPaymentRequest>(filterExpression).ToList();
            IList<LeaveRequest> EmpLeaverequest = _workspace.SqlQuery<LeaveRequest>(filterExpression).ToList();
            return EmpLeaverequest.Sum(x => x.RequestedDays);



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
