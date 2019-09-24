using Chai.WorkflowManagment.CoreDomain.Setting;
using System.Collections.Generic;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public interface IProgramView
    {
        IList<Program> Program { get; set; }
        string ProgramName { get; }
        string ProgramCode { get; }
     
    }
}




