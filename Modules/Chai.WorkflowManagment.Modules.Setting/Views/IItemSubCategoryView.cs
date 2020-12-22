using Chai.WorkflowManagment.CoreDomain.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public interface IItemSubCategoryView
    {
        IList<ItemSubCategory> GetItemSubCategories { get; set; }
        string GetSubCategoryName { get; }
        string GetSubCategoryCode { get; }
    }
}
