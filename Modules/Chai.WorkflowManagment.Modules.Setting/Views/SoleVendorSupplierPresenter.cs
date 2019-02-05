using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class SoleVendorSupplierPresenter : Presenter<ISoleVendorSupplierView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private SettingController _controller;
        public SoleVendorSupplierPresenter([CreateNew] SettingController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            View.soleVendorSupplier = _controller.ListSoleVendorSuppliers(View.SupplierName);
        }

        public override void OnViewInitialized()
        {
            
        }
        public IList<SoleVendorSupplier> GetSoleVendorSuppliers()
        {
            return _controller.GetSoleVendorSuppliers();
        }

        public void SaveOrUpdateSoleVendorSupplier(SoleVendorSupplier SoleVendorSupplier)
        {
            _controller.SaveOrUpdateEntity(SoleVendorSupplier);
        }

        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }

        public void DeleteSoleVendorSupplier(SoleVendorSupplier SoleVendorSupplier)
        {
            _controller.DeleteEntity(SoleVendorSupplier);
        }
        public SoleVendorSupplier GetSoleVendorSupplierById(int id)
        {
            return _controller.GetSoleVendorSupplier(id);
        }

        public IList<SoleVendorSupplier> ListSoleVendorSuppliers(string SupplierName)
        {
            return _controller.ListSoleVendorSuppliers(SupplierName);          
        }
        public IList<SupplierType> GetSupplierTypes()
        {
            return _controller.GetSupplierTypes();
        }
        public SupplierType GetSupplierTypeById(int id)
        {
            return _controller.GetSupplierType(id);
        }
        public void Commit()
        {
            _controller.Commit();
        }        
    }
}




