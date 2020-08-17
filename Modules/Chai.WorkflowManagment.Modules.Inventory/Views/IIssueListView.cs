using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Inventory.Views
{
    public interface IIssueListView
    {
        int GetId { get; }
        DateTime GetIssueDate { get; }
        string GetIssueNo { get; }
    }
}




