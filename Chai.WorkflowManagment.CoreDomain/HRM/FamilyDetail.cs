using System;

namespace Chai.WorkflowManagment.CoreDomain.HRM
{
    public partial class FamilyDetail : IEntity
    {
        public FamilyDetail()
        {

        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<DateTime> DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Certificate { get; set; }
        public string Relationship { get; set; }
        public string CellPhone { get; set; }
        public Nullable<DateTime> DateOfMarriage { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
