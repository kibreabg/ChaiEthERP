using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class JobTitlePresenter : Presenter<IJobTitleView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private Chai.WorkflowManagment.Modules.Setting.SettingController _controller;
        public JobTitlePresenter([CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
           
        }

        public override void OnViewInitialized()
        {
            
        }
        public IList<JobTitle> GetJobTitles()
        {
            return _controller.GetJobTitles();
        }

       

        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }

       
        public JobTitle GetJobTitleById(int id)
        {
            return _controller.GetJobTitle(id);
        }

        public IList<JobTitle> ListJobTitles()
        {
            return _controller.ListJobTitles();
          
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}




