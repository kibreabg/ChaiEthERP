using Chai.WorkflowManagment.CoreDomain.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public interface ISectionView
    {
        IList<Section> GetSections { get; set; }
        string GetSectionName { get; }
        string GetSectionCode { get; }
    }
}
