using Chai.WorkflowManagment.CoreDomain.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public interface IShelfView
    {
        IList<Shelf> GetShelfs { get; set; }
        string GetShelfName { get; }
        string GetShelfCode { get; }
    }
}
