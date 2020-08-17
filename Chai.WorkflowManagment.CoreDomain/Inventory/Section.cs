

namespace Chai.WorkflowManagment.CoreDomain.Inventory
{
    public partial class Section : IEntity
    {
        public int Id { get; set; }        
        public string Name { get; set; }
        public string Code { get; set; }        
        public bool Status { get; set; }
        public virtual Store Store { get; set; }
    }
}
