using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    [Table("MaintenanceSpareParts")]
    public partial class MaintenanceSparePart : IEntity
    {
        public int Id { get; set; }
      
        public virtual MaintenanceRequest MaintenanceRequest { get; set; }
        public virtual Item Item  { get; set; }

        
        public string Returned { get; set; }
        public DateTime ReturnedDate { get; set; }
        public string StoreKeeperRemark { get; set; }

    }
}
