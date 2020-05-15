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
           
        public virtual PurchaseRequest PurchaseRequest { get; set; }
        public virtual ServiceTypeDetail DriverServiceType { get; set; }

        public virtual ServiceTypeDetail MechanicServiceType { get; set; }
    }
}
