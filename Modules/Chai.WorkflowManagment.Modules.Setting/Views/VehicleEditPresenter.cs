using System;
using System.Collections.Generic;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.ObjectBuilder;

using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared.MailSender;
using Chai.WorkflowManagment.CoreDomain.Infrastructure;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class VehicleEditPresenter : Presenter<IVehicleEditView>
    {
        private SettingController _controller;

        private Vehicle _vehicle;

        public VehicleEditPresenter([CreateNew] SettingController controller)
        {
            _controller = controller;

        }

        public override void OnViewLoaded()
        {

        }

        public override void OnViewInitialized()
        {

        }

        public Vehicle CurrentVehicle
        {
            get
            {
                if (_vehicle == null)
                {
                    int id = View.GetVehicleId;
                    if (id > 0)
                        _vehicle = _controller.GetVehicle(id);
                    else
                        _vehicle = new Vehicle();
                }
                return _vehicle;
            }
        }

       

        public void SaveOrUpdateVehicle()
        {
            Vehicle vehicle = CurrentVehicle;

            if (vehicle.Id <= 0)
                vehicle.PlateNo = View.GetPlateNo;

            vehicle.Model = View.GetModel;
            vehicle.PurchaseYear = View.GetPurchaseYear;
            vehicle.MakeYear = View.GetMakeYear;
            vehicle.Brand = View.GetBrand;
            vehicle.BodyType = View.GetBodyType;
            vehicle.EngineCapacity = View.GetEngineCapacity;
            vehicle.EngineType = View.GetEngineType;
            vehicle.FrameNo = View.GetFrameNumber;
            vehicle.LastKmReading = View.GetLastKmReading;
            vehicle.Transmission = View.GetTransmission;
            vehicle.AppUser = View.AppUser;
            vehicle.Status = View.GetStatus;
            
                  

            _controller.SaveOrUpdateVehicle(vehicle);
        }

       
        public void DeleteVehicle()
        {
            _controller.SaveOrUpdateEntity(CurrentVehicle);
        }

        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/frmVehicles.aspx?{0}=0", AppConstants.TABID));
        }
        public void RedirectPage(string url)
        {
            _controller.Navigate(url);

        }
        public IList<Vehicle> GetVehicles()
        {

            return _controller.GetVehicles();

        }
        public Vehicle GetVehicle(int vehicleid)
        {
            return _controller.GetVehicle(vehicleid);
        }

       public IList<AppUser> GetDrivers()
        {
            return _controller.ListDrivers();
        }

        public AppUser GetDriver(int appId)
        {
            return _controller.GetAppuser(appId);
        }
        public AppUser GetCurrentUser()
        {
            return _controller.GetCurrentUser();
        }
    }
}




