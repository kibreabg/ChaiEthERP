using System;
using System.Collections.Generic;
using System.Text;
using Chai.WorkflowManagment.CoreDomain;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public interface IVehicleEditView
    {
        int GetVehicleId { get; }
        string GetPlateNo { get; }
        string GetBrand { get; }
        string GetModel { get; }
        int GetMakeYear { get; }
        string GetFrameNumber { get; }
       
        string GetEngineType { get; }
        string GetTransmission { get; }
        string GetBodyType { get; }

        string GetEngineCapacity { get; }
        int GetPurchaseYear { get;}
        int GetLastKmReading { get; }
        AppUser AppUser { get; }
        string GetStatus { get; }


        //IList<Role> Roles { set; }

    }
}




