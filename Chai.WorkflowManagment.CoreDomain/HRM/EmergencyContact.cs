using System;

namespace Chai.WorkflowManagment.CoreDomain.HRM
{
    public partial class EmergencyContact : IEntity
    {
        public EmergencyContact()
        {

        }
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Relationship { get; set; }
        public string SubCity { get; set; }
        public string Woreda { get; set; }
        public string HouseNo { get; set; }
        public string TelephoneHome { get; set; }
        public string TelephoneOffice { get; set; }
        public string CellPhone { get; set; }
        public Nullable<Boolean> IsPrimaryContact { get; set; }
        public virtual Employee Employee { get; set; }
    
    }
}
