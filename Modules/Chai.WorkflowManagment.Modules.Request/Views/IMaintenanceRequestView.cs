using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public interface IMaintenanceRequestView
    {
        MaintenanceRequest MaintenanceRequest { get; set; }
        int GetMaintenancetRequestId { get; }
        string RequestNo { get; }
        DateTime RequestDate { get; }
        int MaintenanceRequestId { get; }
       
        string GetPlateNo{ get; }
        int GetKmReading { get; }
        string GetActionTaken { get; }

        string GetRemark { get; }
       
        int GetProjectId { get; }
        int GetGrantId { get; }
    }
}




