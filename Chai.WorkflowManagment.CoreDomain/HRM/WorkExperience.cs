using System;

namespace Chai.WorkflowManagment.CoreDomain.HRM
{
    public class WorkExperience : IEntity
    {
        public WorkExperience()
        {
        }

        public int Id { get; set; }
        public string EmployerName { get; set; }
        public string EmployerAddress { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string JobTitle { get; set; }
        public string TypeOfEmployer { get; set; }
        public virtual Employee Employee { get; set; }

    }
}
