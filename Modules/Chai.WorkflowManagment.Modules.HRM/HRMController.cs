using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Utility;
using Microsoft.Practices.ObjectBuilder;

using Chai.WorkflowManagment.CoreDomain;
using Chai.WorkflowManagment.CoreDomain.DataAccess;
using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Services;
using Chai.WorkflowManagment.Shared.Navigation;


using System.Data;
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

        #region Employee
        public Employee GetEmployeeActivity(int Id)
        {
            return _workspace.Single<Employee>(x => x.Id == Id, y => y.Contracts, z => z.EmployeeDetails, a => a.Terminations, b => b.Warnings);
        }
        #endregion
        #region Contracts
        public IList<Contract> GetContracts()
        {
            return WorkspaceFactory.CreateReadOnly().Query<Contract>(null).ToList();
        }
        public Contract GetContract(int id)
        {
            return _workspace.Single<Contract>(x => x.Id == id);
        }
        public IList<Contract> ListContracts()
        {
            string filterExpression = "";

            filterExpression = "SELECT  *  FROM dbo.Contracts ";

            return _workspace.SqlQuery<Contract>(filterExpression).ToList();

        }
        public int GetLastContractId()
        {
            if (_workspace.Last<Contract>() != null)
            {
                return _workspace.Last<Contract>().Id;
            }
            else { return 0; }
        }

        #endregion
        #region Change
        public IList<EmployeeDetail> GetEmployeeDetails()
        {
            return WorkspaceFactory.CreateReadOnly().Query<EmployeeDetail>(null).ToList();
        }
        public EmployeeDetail GetEmployeeDetail(int id)
        {
            return _workspace.Single<EmployeeDetail>(x => x.Id == id);
        }
        public IList<EmployeeDetail> ListEmployeeDetails()
        {
            string filterExpression = "";

            filterExpression = "SELECT  *  FROM dbo.EmployeeDetails ";

            return _workspace.SqlQuery<EmployeeDetail>(filterExpression).ToList();

        }
        public int GetLastEmployeeDetailId()
        {
            if (_workspace.Last<EmployeeDetail>() != null)
            {
                return _workspace.Last<EmployeeDetail>().Id;
            }
            else { return 0; }
        }

        #endregion
        #region Warning
        public IList<Warning> GetWarnings()
        {
            return WorkspaceFactory.CreateReadOnly().Query<Warning>(null).ToList();
        }
        public Warning GetWarning(int id)
        {
            return _workspace.Single<Warning>(x => x.Id == id);
        }
        public IList<Warning> ListWarnings()
        {
            string filterExpression = "";

            filterExpression = "SELECT  *  FROM dbo.Warnings ";

            return _workspace.SqlQuery<Warning>(filterExpression).ToList();

        }
        public int GetLastWarningId()
        {
            if (_workspace.Last<Warning>() != null)
            {
                return _workspace.Last<Warning>().Id;
            }
            else { return 0; }
        }

        #endregion
        #region Terminations
        public IList<Termination> GetTerminations()
        {
            return WorkspaceFactory.CreateReadOnly().Query<Termination>(null).ToList();
        }
        public Termination GetTermination(int id)
        {
            return _workspace.Single<Termination>(x => x.Id == id);
        }
        public IList<Termination> ListTerminations()
        {
            string filterExpression = "";

            filterExpression = "SELECT  *  FROM dbo.Terminations ";

            return _workspace.SqlQuery<Termination>(filterExpression).ToList();

        }

        public IList<TerminationReason> GetTerminationReason(int terminationId)
        {
            string filterExpression = "";

            filterExpression = "SELECT  *  FROM dbo.TerminationReasons Where 1 = Case when '" + terminationId + "' =  0 Then 1 When TerminationReasons.TerminationId = '" + terminationId + "'  Then 1 END  ";

            return _workspace.SqlQuery<TerminationReason>(filterExpression).ToList();

        }
        public int GetLastTerminationId()
        {
            if (_workspace.Last<Termination>() != null)
            {
                return _workspace.Last<Termination>().Id;
            }
            else { return 0; }
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
