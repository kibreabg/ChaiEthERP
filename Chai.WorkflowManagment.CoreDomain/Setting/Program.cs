using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public class Program :IEntity
    {
        public int Id { get; set; }

        public string ProgramName { get; set; }

        public string Status { get; set; }
    }
}
