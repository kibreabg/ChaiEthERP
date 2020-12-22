using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public interface ISoleVendorRequestView
    {
        IList<SoleVendorRequest> SoleVendorRequests { get; set; }
        int GetSoleVendorRequestId { get; }
        string GetRequestNo { get; }
        string GetComment { get; }
        int GetPurchaseRequestId { get; }
    }
}




