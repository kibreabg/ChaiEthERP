using Chai.WorkflowManagment.CoreDomain.Setting;

namespace Chai.WorkflowManagment.CoreDomain.Approval
{
    public partial class PurchaseOrderSoleVendorDetail : IEntity
    {

        public PurchaseOrderSoleVendorDetail()
        { 
        
        }

        public int Id { get; set; }
        public virtual PurchaseOrderSoleVendor PurchaseOrderSoleVendor { get; set; }
        public virtual ItemAccount ItemAccount { get; set; }
        public string Item { get; set; }
        public int Qty { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal Vat { get; set; }
    }
}
