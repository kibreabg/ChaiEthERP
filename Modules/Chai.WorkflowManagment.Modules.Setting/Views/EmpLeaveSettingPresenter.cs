using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.CoreDomain.HRM;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class EmpLeaveSettingPresenter : Presenter<IEmpLeaveSettingView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private Chai.WorkflowManagment.Modules.Setting.SettingController _controller;
        public EmpLeaveSettingPresenter([CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            //View.Projects = _controller.GetProjects();
        }

        public override void OnViewInitialized()
        {

        }
        public IList<Employee> GetEmployees(string fullname)
        {
            return _controller.GetEmployees(fullname);
        }
        public Employee GetEmployee(int empId)
        {
            return _controller.GetEmployee(empId);
        }
        public void SaveOrUpdateEmpLeaveSetting(Employee employee)
        {
            _controller.SaveOrUpdateEntity(employee);
        }
        public decimal EmpLeaveTaken(int empid, DateTime LeaveSettingDate)
        {
            return _controller.TotalleaveTaken(empid, LeaveSettingDate);
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }

        public void Commit()
        {
            _controller.Commit();
        }
    }
}




