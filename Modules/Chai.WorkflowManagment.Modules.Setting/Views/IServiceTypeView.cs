using Chai.WorkflowManagment.CoreDomain.Setting;
using System.Collections.Generic;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public interface IServiceTypeView
    {
        IList<ServiceType> ServiceTypes { get; set; }
        string ServiceTypeName { get; }
      
    }
}




