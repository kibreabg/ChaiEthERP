using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.Modules.Setting;
using Chai.WorkflowManagment.CoreDomain.HRM;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.Modules.HRM.Views
{
    public class ManageHRPresenter : Presenter<IManageHRView>
    {

        private HRMController _controller;
        private SettingController _settingController;
        private Employee _employee;
        private EmployeeDetail _employeedetail;
        private Contract _contract;
        private Termination _termination;
        private Warning _warning;
        public ManageHRPresenter([CreateNew] HRMController controller,  SettingController settingController)
        {
            _controller = controller;
            _settingController = settingController;
        }
       
        public override void OnViewLoaded()
        {
            if (View.GetId > 0)
            {
                _controller.CurrentObject = _controller.GetEmployee(View.GetId);
            }
            CurrentEmployee = _controller.CurrentObject as Employee;
        }

        public override void OnViewInitialized()
        {
            if (View.GetId == 0)
            {
                int id = View.GetId;
                if (id > 0)
                    _controller.CurrentObject = _controller.GetEmployee(View.GetId);
                else
                    _controller.CurrentObject = new Employee();
            }
        }

       
        
        #region Employee

        public Employee CurrentEmployee
        {

            get
            {
                if (_employee == null)
                {
                    int id = View.GetId;
                    if (id > 0)
                        _employee = _controller.GetEmployee(View.GetId);
                    else
                        _employee = new Employee();
                }
                return _employee;
            }
            set { _employee = value; }



        }
        public void SaveOrUpdateEmployeeActivity(Employee employeeactivity)
        {
            _controller.SaveOrUpdateEntity(employeeactivity);
        }


        #endregion

        #region Contract
     

        public IList<Contract> GetContracts()
        {
            return _controller.GetContracts();
        }
        public IList<Contract> ListContracts()
        {
            return _controller.ListContracts();
        }
        public int GetLastContrcatId()
        {
            return _controller.GetLastContractId();

        }

        public Contract GetEmpContract(int id)
        {
            return _controller.GetEmpContract(id);
        }

        public EmployeePosition GetEmployeePosition(int id)
        {
            return _settingController.GetEmployeePosition(id);

        }
        public Program GetProgram(int id)
        {
            return _settingController.GetProgram(id);

        }


        public IList<Program> GetPrograms()
        {
            return _settingController.GetPrograms();

        }
        
        public IList<EmployeePosition> GetEmployeePositions()
        {
            return _settingController.GetEmployeePositions();
        }
        public IList<AppUser> GetEmployees()
        {
            return _settingController.GetEmployeeList();

        }
       
        public void SaveOrUpdateContract(Contract Contract)
           {
            _controller.SaveOrUpdateEntity(Contract);
           }

        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/HRM/Default.aspx?{0}=3", AppConstants.TABID));
        }
        public void DeleteContracts(Contract Contract)
        {
            _controller.DeleteEntity(Contract);
        }
        public Contract GetContract(int id)
        {
            return _controller.GetContract(id);
        }

        #endregion

        #region EmployeeDetail

       

        public IList<EmployeeDetail> GetEmployeeDetails()
        {
            return _controller.GetEmployeeDetails();
        }
        public IList<EmployeeDetail> ListEmployeeDetails()
        {
            return _controller.ListEmployeeDetails();
        }
        public int GetLastEmployeeDetailId()
        {
            return _controller.GetLastEmployeeDetailId();

        }
        
     
        public void SaveOrUpdateEmployeeDetail(EmployeeDetail EmployeeDetail)
        {
            _controller.SaveOrUpdateEntity(EmployeeDetail);
        }

       
     
        public void DeleteEmployeeDetails(EmployeeDetail EmployeeDetail)
        {
            _controller.DeleteEntity(EmployeeDetail);
        }
        public EmployeeDetail GetEmployeeDetail(int id)
        {
            return _controller.GetEmployeeDetail(id);
        }

        #endregion

        #region Warning
       

        public IList<Warning> GetWarnings()
        {
            return _controller.GetWarnings();
        }
        public IList<Warning> ListWarnings()
        {
            return _controller.ListWarnings();
        }
        public int GetLastWarningId()
        {
            return _controller.GetLastWarningId();

        }

        public Warning GetWarning(int id)
        {
            return _controller.GetWarning(id);

        }


        public void SaveOrUpdateWarning(Warning warning)
        {
            _controller.SaveOrUpdateEntity(warning);
        }




        #endregion

        #region Termination
       

        public IList<Termination> GetTerminations()
        {
            return _controller.GetTerminations();
        }
        public IList<Termination> ListTerminations()
        {
            return _controller.ListTerminations();
        }
        public int GetLastTerminationId()
        {
            return _controller.GetLastTerminationId();

        }

        public Termination GetTermination(int id)
        {
            return _controller.GetTermination(id);

        }


        public void SaveOrUpdateTermination(Termination termination)
        {
            _controller.SaveOrUpdateEntity(termination);
        }

        public IList<TerminationReason> GetTerminationReason(int terminationId)
        {
            return _controller.GetTerminationReason(terminationId);

        }


        #endregion
        // TODO: Handle other view events and set state in the view
    }
}




