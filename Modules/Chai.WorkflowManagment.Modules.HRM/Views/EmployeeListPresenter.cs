using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.HRM;

namespace Chai.WorkflowManagment.Modules.HRM.Views
{
    public class EmployeeListPresenter : Presenter<IEmployeeListView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private Chai.WorkflowManagment.Modules.HRM.HRMController _controller;
        public EmployeeListPresenter([CreateNew] Chai.WorkflowManagment.Modules.HRM.HRMController controller)
        {
            _controller = controller;
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
        // TODO: Handle other view events and set state in the view
    }
}




