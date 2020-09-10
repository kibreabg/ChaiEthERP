using System;

namespace Chai.WorkflowManagment.CoreDomain.Inventory
{
    public partial class Stock : IEntity
    {
        public int Id { get; set; }        
        public Nullable<int> Quantity { get; set; }
        public string Status { get; set; }
        public virtual Item Item { get; set; }
    }
}
