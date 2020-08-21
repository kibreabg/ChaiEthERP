using System.ComponentModel.DataAnnotations;

namespace Chai.WorkflowManagment.CoreDomain.Inventory
{
    [Table("Shelves")]
    public partial class Shelf : IEntity
    {
        public int Id { get; set; }        
        public string Name { get; set; }
        public string Code { get; set; }        
        public string Status { get; set; }
        public virtual Section Section { get; set; }
    }
}
