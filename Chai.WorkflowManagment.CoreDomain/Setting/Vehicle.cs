using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Request;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public partial class Vehicle : IEntity
    {
        public int Id { get; set; }
        public string PlateNo { get; set; }
        
        public string Brand { get; set; }
        public string Model { get; set; }
        public int MakeYear { get; set; }
        public string FrameNumber { get; set; }
        public string EngineType { get; set; }
        public string Transmission { get; set; }
        public string BodyType { get; set; }
        public string EngineCapacity { get; set; }
        public int PurchaseYear { get; set; }
        public int LastKmReading { get; set; }
        public virtual AppUser AppUser { get; set; }
        public string Status { get; set; }

    }
}
