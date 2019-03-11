using System;

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
        public string EducationalLevel { get; set; }
        public Nullable<DateTime> GraduationYear { get; set; }
        public string SpecialAward { get; set; }
        public string Certificate { get; set; }
        public bool Reviewed { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
