using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    [Table("StoreRequestDetails")]
    public partial class StoreRequestDetail : IEntity
    {
        public int Id { get; set; }
        public string Item { get; set; }
       
        public int Qty { get; set; }
        public int QtyApproved { get; set; }
        public string UnitOfMeasurment { get; set; }
      
        public string Remark { get; set; }
     
      
        public virtual Project Project { get; set; }
        public virtual Grant Grant { get; set; }     
        public virtual StoreRequest StoreRequest { get; set; }

    }
}
