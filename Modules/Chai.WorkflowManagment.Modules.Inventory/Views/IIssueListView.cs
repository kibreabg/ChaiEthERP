using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Inventory.Views
{
    public interface IIssueListView
    {
        int GetIssueId { get; }
        DateTime GetIssueDate { get; }
        string GetPurpose { get; }
        string GetIssueNo { get; }
        int GetHandedOverBy { get; }
        int GetIssuedTo { get; }
    }
}




