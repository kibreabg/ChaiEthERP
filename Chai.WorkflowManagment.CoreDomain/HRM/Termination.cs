using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.HRM
{
    public  class Termination : IEntity
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime TerminationDate { get; set; }
        public DateTime LastDateOfEmployee { get; set; }
        public string RecommendationforRehire { get; set; }
        public string TerminationReasonId { get; set; }
       
    }
}
