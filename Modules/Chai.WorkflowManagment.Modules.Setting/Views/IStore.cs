using Chai.WorkflowManagment.CoreDomain.Inventory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public interface IStore
    {
        IList<Store> GetStores { get; set; }
        string GetName { get; }
        string GetLocation { get; }
      

    }
}




