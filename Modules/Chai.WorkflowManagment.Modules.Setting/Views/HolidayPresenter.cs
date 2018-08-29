using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class HolidayPresenter : Presenter<IHolidayView>
    {

        
        private Chai.WorkflowManagment.Modules.Setting.SettingController _controller;
        public HolidayPresenter([CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            View.holidays = _controller.ListHolidays();
        }

        public override void OnViewInitialized()
        {
            
        }
        public IList<Holiday> GetHolidays()
        {
            return _controller.ListHolidays();
        }

        public void SaveOrUpdateHoliday(Holiday holiday)
        {
            _controller.SaveOrUpdateEntity(holiday);
        }

        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }

        public void Deleteholiday(Holiday holiday)
        {
            _controller.DeleteEntity(holiday);
        }
        public Holiday GetHolidayById(int id)
        {
            return _controller.GetHoliday(id);
        }
        public void DeleteHoliday(Holiday holiday)
        {
             _controller.DeleteEntity(holiday);
        }
        public IList<Holiday> ListHolidays()
        {
            return _controller.ListHolidays();
          
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}




