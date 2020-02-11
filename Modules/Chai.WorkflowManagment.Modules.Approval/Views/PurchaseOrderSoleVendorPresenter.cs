using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.Modules.Setting;
using Chai.WorkflowManagment.Modules.Admin;
using Chai.WorkflowManagment.CoreDomain.Approval;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public class PurchaseOrderSoleVendorPresenter : Presenter<IPurchaseOrderSoleVendorView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private ApprovalController _controller;
        private SettingController _settingcontroller;
        private AdminController _admincontroller;

        private SoleVendorRequest _soleVendorRequest;
        public PurchaseOrderSoleVendorPresenter([CreateNew] ApprovalController controller, [CreateNew] SettingController settingcontroller, [CreateNew] AdminController admincontroller)
        {
            _controller = controller;
            _settingcontroller = settingcontroller;
            _admincontroller = admincontroller;
        }

        public override void OnViewLoaded()
        {
            if (View.SoleVendorRequestId > 0)
            {
                _controller.CurrentObject = _controller.GetSoleVendorRequest(View.SoleVendorRequestId);
            }
            CurrentSoleVendorRequest = _controller.CurrentObject as SoleVendorRequest;

            if (CurrentSoleVendorRequest != null)
            {
                if (CurrentSoleVendorRequest.PurchaseOrderSoleVendor == null)
                {
                    CurrentSoleVendorRequest.PurchaseOrderSoleVendor = new PurchaseOrderSoleVendor();
                }
            }
        }
        public SoleVendorRequest CurrentSoleVendorRequest
        {
            get
            {
                if (_soleVendorRequest == null)
                {
                    int id = View.SoleVendorRequestId;
                    if (id > 0)
                        _soleVendorRequest = _controller.GetSoleVendorRequest(id);
                    else
                        _soleVendorRequest = new SoleVendorRequest();
                }
                return _soleVendorRequest;
            }
            set { _soleVendorRequest = value; }
        }
        public override void OnViewInitialized()
        {

            if (_soleVendorRequest == null)
            {
                int id = View.SoleVendorRequestId;
                if (id > 0)
                    _controller.CurrentObject = _controller.GetSoleVendorRequest(id);
                else
                    _controller.CurrentObject = new SoleVendorRequest();

            }
        }
        public ItemAccount GetItemAccount(int Id)
        {
            return _settingcontroller.GetItemAccount(Id);
        }
        public AppUser GetUser(int UserId)
        {
            return _admincontroller.GetUser(UserId);
        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public int GetLastPurchaseOrderSoleVendorId()
        {
            return _controller.GetLastPurchaseOrderSoleVendorId();
        }
        public void SaveOrUpdateSoleVendorRequest(SoleVendorRequest soleVendorRequest)
        {
            _controller.SaveOrUpdateEntity(soleVendorRequest);
        }

    }
}




