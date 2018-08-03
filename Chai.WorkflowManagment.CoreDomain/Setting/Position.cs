using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public class Position :IEntity
    {
        public int Id { get; set; }

        public string PositionName { get; set; }

        public string Status { get; set; }
    }
}
