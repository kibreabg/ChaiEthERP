using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Inventory
{
    public partial class ItemCategory : IEntity
    {
        public int Id { get; set; }        
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
