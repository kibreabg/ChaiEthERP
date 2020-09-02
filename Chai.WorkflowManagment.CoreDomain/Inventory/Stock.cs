using System;

namespace Chai.WorkflowManagment.CoreDomain.Inventory
{
    public partial class Stock : IEntity
    {
        public int Id { get; set; }
        public Item Item { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Status { get; set; }
    }
}
