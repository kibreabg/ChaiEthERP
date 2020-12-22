using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;

namespace Chai.WorkflowManagment.CoreDomain.Inventory
{
    public partial class UnitOfMeasurement : IEntity
    {
        public int Id { get; set; }        
        public string Name { get; set; }
        public string ShortCode { get; set; }
    }
}
