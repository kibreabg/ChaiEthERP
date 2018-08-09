using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class Contract : IEntity
    {
        public int Id { get; set; }
        

        public DateTime ContractStartDate { get; set; }

        public DateTime ContractEndDate { get;  set;}

       



        public virtual HRM.Employee Employee { get; set; }
    
    }
}
