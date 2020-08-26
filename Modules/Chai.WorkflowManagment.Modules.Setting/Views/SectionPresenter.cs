using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.CoreDomain.Inventory;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class SectionPresenter : Presenter<ISectionView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private SettingController _controller;
        public SectionPresenter([CreateNew] SettingController controller)
        {
            _controller = controller;
        }
        public override void OnViewLoaded()
        {
            View.GetSections = _controller.ListSections(View.GetSectionName, View.GetSectionCode);
        }
        public override void OnViewInitialized()
        {

        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public IList<Section> GetSections()
        {
            return _controller.GetSections();
        }
        public Section GetSectionById(int id)
        {
            return _controller.GetSection(id);
        }
        public IList<Store> GetStores()
        {
            return _controller.GetStores();
        }
        public Store GetStore(int storeId)
        {
            return _controller.GetStore(storeId);
        }
        public Section GetSectById(int id)
        {
            return _controller.GetSec(id);
        }
        public IList<Section> ListSections(string SectionName, string SectionCode)
        {
            return _controller.ListSections(SectionName, SectionCode);
        }
        public void DeleteSection(Section Section)
        {
            _controller.DeleteEntity(Section);
        }
        public void SaveOrUpdateSection(Section Section)
        {
            _controller.SaveOrUpdateEntity(Section);
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




