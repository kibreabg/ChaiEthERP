using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public interface IOperationalControlRequestView
    {
        int GetOperationalControlRequestId { get; }
        string GetRequestNo { get; }
        int GetBankAccountId { get; }
        int GetBeneficiaryId { get; }
        string GetDescription { get; }
        string GetTelephoneNo { get; }
        string GetBankName { get; }
        string GetVoucherNo { get; }
        string GetPaymentType { get; }
    }
}




