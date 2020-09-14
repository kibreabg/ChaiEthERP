using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Inventory.Views
{
    public interface IStockListView
    {
        int GetStockId { get; }
        string GetItem { get; }
        string GetQuantity { get; }
    }
}




