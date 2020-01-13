using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public interface IPurchaseOrderSoleVendorView
    {
       
        SoleVendorRequest SoleVendorRequest { get; set; }
        string RequestType { get;  }
        int SoleVendorRequestId { get; }
    }
}




