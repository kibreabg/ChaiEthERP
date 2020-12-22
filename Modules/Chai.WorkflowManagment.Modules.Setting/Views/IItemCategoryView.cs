using Chai.WorkflowManagment.CoreDomain.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public interface IItemCategoryView
    {
        IList<ItemCategory> GetItemCategories { get; set; }
        string GetCategoryName { get; }
        string GetCategoryCode { get; }
    }
}
