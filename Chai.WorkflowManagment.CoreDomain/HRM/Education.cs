using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.HRM
{
    public partial class Education : IEntity
    {
        public Education()
        {

        }
        public int Id { get; set; }
        public string InstitutionType { get; set; }
        public string InstitutionName { get; set; }
        public string InstitutionLocation { get; set; }
        public string Major { get; set; }
        public string EducationLevel { get; set; }
        public Nullable<DateTime> GraduationYear { get; set; }
        public string SpecialAward { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
