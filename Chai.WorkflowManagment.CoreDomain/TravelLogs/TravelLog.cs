using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Requests;
using System.ComponentModel.DataAnnotations;

namespace Chai.WorkflowManagment.CoreDomain.TravelLogs
{
    public partial class TravelLog : IEntity
    {
        public int Id { get; set; }
        public string RequestNo { get; set; }
        public string DeparturePlace { get; set; }
        public string ArrivalPlace { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal StartKmReading { get; set; }
        public decimal EndKmReading { get; set; }
        public decimal FuelPrice { get; set; }
        public decimal FuelLitre { get; set; }
        public int KmReadingOnFuelRefill { get; set; }

        [Required]
     

        public virtual VehicleRequest VehicleRequest { get; set; }
    }
}
