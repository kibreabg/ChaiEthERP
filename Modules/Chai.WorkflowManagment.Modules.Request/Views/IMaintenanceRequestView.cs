using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public interface IMaintenanceRequestView
    {
        MaintenanceRequest MaintenanceRequest { get; set; }
        string RequestNo { get; }
        string RequestDate { get; }
        int MaintenanceRequestId { get; }
        bool GetIsVehicle { get; }
        string GetPlateNo{ get; }
    }
}




