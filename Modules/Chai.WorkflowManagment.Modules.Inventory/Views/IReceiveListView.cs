using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Inventory.Views
{
    public interface IReceiveListView
    {
        int GetReceiveId { get; }
        DateTime GetRecieveDate { get; }
        string GetReceiveNo { get; }
        string GetInvoiceNo { get; }
        string GetDeliveredBy { get; }
        int GetProgram { get; }
        int GetProject { get; }
        int GetGrant { get; }
        int GetSupplier { get; }
    }
}




