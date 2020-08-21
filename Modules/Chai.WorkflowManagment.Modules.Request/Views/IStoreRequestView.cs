using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public interface IStoreRequestView
    {
        StoreRequest StoreRequest { get; set; }
        string RequestNo { get; }
        string RequestDate { get; }
        int StoreRequestId { get; }
     
    }
}




