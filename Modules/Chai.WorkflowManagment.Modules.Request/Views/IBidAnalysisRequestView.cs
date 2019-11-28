using Chai.WorkflowManagment.CoreDomain.Request;
using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public interface IBidAnalysisRequestView
    {
        IList<BidAnalysisRequest> BidAnalysisRequests { get; set; }
        int GetBidAnalysisRequestId { get; }

        int GetBARequestId { get; }
        int GetPurchaseRequestId { get; }
        string GetRequestNo { get; }
        DateTime GetRequestDate { get; }
      
      //  string GetNeededFor { get; }
        
        decimal GetTotalPrice { get; }
        //string GetProject { get; }
        //string GetGrant { get; }
        int GetProjectId { get; }
        int GetGrantId { get; }
        string GetReasonForSelection { get; }  
    
    }
}




