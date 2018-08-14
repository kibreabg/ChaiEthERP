using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.HRM
{
    public partial class Warning : IEntity
    {
        public int Id { get; set; }

        public string WarningDescription { get; set; }
        public DateTime WarningDate { get; set; }
          
        public virtual HRM.Employee Employee { get; set; }

    }
}
