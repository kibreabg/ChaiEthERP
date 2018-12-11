using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.HRM;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Modules.Setting;

namespace Chai.WorkflowManagment.Modules.HRM.Views
{
    public class EmployeeListPresenter : Presenter<IEmployeeListView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private Chai.WorkflowManagment.Modules.HRM.HRMController _controller;
        private SettingController _settingController;
        public EmployeeListPresenter([CreateNew] Chai.WorkflowManagment.Modules.HRM.HRMController controller, SettingController settingController)
        {
            _controller = controller;
            _settingController = settingController;
        }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }
        public IList<Employee> ListEmployees(string EmpNo, string FullName, int project)
        {
            return _controller.ListEmployees(EmpNo, FullName, project);
        }
        public decimal EmpLeaveTaken(int empid, DateTime LeaveSettingDate)
        {
            return _controller.TotalleaveTaken(empid, LeaveSettingDate);
        }
        public IList<Program> GetPrograms()
        {
           return _settingController.GetPrograms();
        }
        // TODO: Handle other view events and set state in the view
    }
}




