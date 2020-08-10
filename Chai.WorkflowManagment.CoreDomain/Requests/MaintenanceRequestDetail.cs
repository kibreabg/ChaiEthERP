using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    [Table("MaintenanceRequestDetails")]
    public partial class MaintenanceRequestDetail : IEntity
    {
        public int Id { get; set; }
        public virtual ServiceType ServiceType { get; set; }
        public virtual MaintenanceRequest MaintenanceRequest { get; set; }
        public virtual ServiceTypeDetail DriverServiceTypeDetail  { get; set; }

        public virtual ServiceTypeDetail MechanicServiceTypeDetail { get; set; }
        public string  TechnicianRemark { get; set; }
       
    }
}
