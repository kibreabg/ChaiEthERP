using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public interface IPaymentLiquidationRequestView
    {
        int GetPayRequestId { get; }
        string GetExpenseType { get; }
        string GetComment { get; }
        string GetAdditionalComment { get; }
        string GetCashPayReqDate { get; }


    }
}




