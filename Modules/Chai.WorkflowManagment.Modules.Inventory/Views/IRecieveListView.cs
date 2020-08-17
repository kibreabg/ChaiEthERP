using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Inventory.Views
{
    public interface IRecieveListView
    {
        int GetId { get; }
        DateTime GetRecieveDate { get; }
        string GetRecieveNo { get; }
    }
}




