using System;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class ProgramPresenter : Presenter<IProgramView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private SettingController _controller;
        public ProgramPresenter([CreateNew] SettingController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            View.Program = _controller.ListPrograms(View.ProgramName,View.ProgramCode);
        }

        public override void OnViewInitialized()
        {
            
        }
        public IList<Program> GetPrograms()
        {
            return _controller.GetPrograms();
        }

        public void SaveOrUpdateProgram(Program Program)
        {
            _controller.SaveOrUpdateEntity(Program);
        }

        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }

        public void DeleteProgram(Program Program)
        {
            _controller.DeleteEntity(Program);
        }
        public Program GetProgramById(int id)
        {
            return _controller.GetProgram(id);
        }

        public IList<Program> ListPrograms(string ProgramName, string ProgramCode)
        {
            return _controller.ListPrograms(ProgramName, ProgramCode);          
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}




