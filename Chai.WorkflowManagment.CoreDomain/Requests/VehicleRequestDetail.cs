using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class VehicleRequestDetail : IEntity
    {
        public int Id { get; set; }
        public string PlateNo { get; set; }
        public string AssignedVehicle { get; set; }
        public decimal Rate { get; set; }
        public string CarHiredLocation { get; set; }
        public string DriverPhoneNo { get; set; }
        public string RentalDriverName { get; set; }
        public string ReasonForHire { get; set; }
        public decimal StartKmReading { get; set; }
        public decimal PreEndKmReading { get; set; }
        public string AvailableWhileHired { get; set; }
        public virtual CarRental CarRental { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual CarModel CarModel { get; set; }
        public virtual VehicleRequest VehicleRequest { get; set; }
    }
}
