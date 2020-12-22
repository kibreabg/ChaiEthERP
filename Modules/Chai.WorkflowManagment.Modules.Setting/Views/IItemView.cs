using Chai.WorkflowManagment.CoreDomain.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public interface IItemView
    {
        IList<Item> GetItems { get; set; }
        string GetItemName { get; }
        string GetItemCode { get; }
    }
}
