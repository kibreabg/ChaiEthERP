using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Approval
{
    public partial class PurchaseOrderDetail : IEntity
    {

        public PurchaseOrderDetail()
        { 
        
        }

        public int Id { get; set; }
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public virtual ItemAccount ItemAccount { get; set; }
        public int Qty { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal Vat { get; set; }
        public virtual Bidder Bidder { get; set; }
        public virtual Supplier Supplier { get; set; }
        public int Rank { get; set; }
        public string ItemDescription { get; set; }

        public string Status { get; set; }
       
       
    }
}
