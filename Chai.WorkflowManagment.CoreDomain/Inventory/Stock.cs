using System;

namespace Chai.WorkflowManagment.CoreDomain.Inventory
{
    public partial class Stock : IEntity
    {
        public int Id { get; set; }
        public Item Item { get; set; }
        public Nullable<decimal> UnitCost { get; set; }
        public Nullable<int> Qty { get; set; }
        public string Status { get; set; }
    }
}
