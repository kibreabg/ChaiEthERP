using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class CarModelPresenter : Presenter<ICarModelView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private SettingController _controller;
        public CarModelPresenter([CreateNew] SettingController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            View.CarModels = _controller.ListCarModels(View.GetName);
        }

        public override void OnViewInitialized()
        {
            
        }
        public IList<CarModel> GetCarModels()
        {
            return _controller.GetCarModels();
        }

        public void SaveOrUpdateCarModel(CarModel CarModel)
        {
            _controller.SaveOrUpdateEntity(CarModel);
        }

        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }

        public void DeleteCarModel(CarModel CarModel)
        {
            _controller.DeleteEntity(CarModel);
        }
        public CarModel GetCarModelById(int id)
        {
            return _controller.GetCarModel(id);
        }

        public IList<CarModel> ListCarModels(string CarModelName)
        {
            return _controller.ListCarModels(CarModelName);
          
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}




