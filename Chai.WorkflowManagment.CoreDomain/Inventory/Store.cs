

namespace Chai.WorkflowManagment.CoreDomain.Inventory
{
    public partial class Store : IEntity
    {
        public int Id { get; set; }        
        public string Name { get; set; }
        public string Location { get; set; }        
        public string Status { get; set; }
    }
}
