using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public class JobTitle :IEntity
    {
        public int Id { get; set; }

        public string JobTitleName { get; set; }

        public string Description { get; set; }
    }
}
