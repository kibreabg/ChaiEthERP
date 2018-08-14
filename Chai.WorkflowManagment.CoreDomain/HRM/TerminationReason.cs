using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.HRM
{
   public class TerminationReason :IEntity
    {
       
        public int Id { get; set; }
        public int TerminationReasonId { get; set; }
        public string Reason { get; set; }
       
    }
}
