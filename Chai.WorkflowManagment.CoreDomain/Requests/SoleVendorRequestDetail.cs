using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class SoleVendorRequestDetail : IEntity
    {

        public SoleVendorRequestDetail()
        { 
        
        }
        public int Id { get; set; }   
        public int PRDetailID { get; set; }
        public string ItemDescription { get; set; }
        public string ReasonForSelection { get; set; }        
        public string SoleVendorJustificationType { get; set; }
        public int Qty { get; set; }        
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }        
        public virtual Project Project { get; set; }
        public virtual Grant Grant { get; set; }
        public virtual SoleVendorSupplier SoleVendorSupplier { get; set; }
        public virtual SoleVendorRequest SoleVendorRequest { get; set; }
        public virtual ItemAccount ItemAccount { get; set; }

    }
}
