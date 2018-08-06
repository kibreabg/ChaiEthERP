using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class EmergencyContact : IEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string SubCity { get; set; }

        public string Woreda { get; set; }
        public string HouseNo { get; set; }
        public string TelephoneHome { get; set; }
        public string TelephoneOffice { get; set; }
        public virtual HRM.Employee Employee { get; set; }
    
    }
}
