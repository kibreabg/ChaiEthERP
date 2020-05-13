using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;

using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Setting;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class VehiclesPresenter : Presenter<IVehiclesView>
    {
        private SettingController _controller;
 
        public VehiclesPresenter([CreateNew] SettingController controller)
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

        public SettingController Controller { get { return _controller; } }

        public void AddNewVehicle()
        {
            string url = String.Format("~/Setting/frmVehicleEdit.aspx");
            _controller.Navigate(url);
            // Response.Redirect("frmCashPaymentRequest.aspx");
        }

        public IList<Vehicle> SearchVehicle(string PlateNo)
        {
            return _controller.ListVehicles(PlateNo);
        }
    }
}




