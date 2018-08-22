using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.HRM
{
    public class WorkExperience :IEntity
    {

        public int Id { get; set; }
        public virtual HRM.Employee Employee { get; set; }
        public string EmployerName { get; set; }
        public string EmployerAddress { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
  
        public string JobTitle { get; set;  }
        public string TypeOfEmployer { get; set;  }

    }
}
