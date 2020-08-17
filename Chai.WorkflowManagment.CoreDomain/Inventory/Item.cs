using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Inventory
{
    public partial class Item : IEntity
    {
        public int Id { get; set; }        
        public string ItemType { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ReOrderQuantity { get; set; }        
        public bool Status { get; set; }
        public string FilePath { get; set; }
        public virtual ItemSubCategory ItemSubCategory { get; set; }
        public virtual UnitOfMeasurement UnitOfMeasurement { get; set; }
    }
}
