using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public class CarModel : IEntity
    {
        public int Id { get; set; }

        public string ModelName { get; set; }

        public int ManufacturedYear { get; set; }

        public string Description { get; set; }
      
    }
}
