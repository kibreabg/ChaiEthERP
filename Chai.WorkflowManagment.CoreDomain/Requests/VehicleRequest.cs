using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.CoreDomain.TravelLogs;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class VehicleRequest : IEntity
    {
        public VehicleRequest()
        {
            this.VehicleRequestStatuses = new List<VehicleRequestStatus>();
            this.VehicleRequestDetails = new List<VehicleRequestDetail>();  
        }
        public int Id { get; set; }
        public string RequestNo { get; set; }
        public Nullable<DateTime> RequestDate { get; set; }
        public string PurposeOfTravel { get; set; }
        public string Destination { get; set; }
        public string Comment { get; set; }
        public int NoOfPassengers { get; set; }
        public int CurrentApprover { get; set; }
        public Nullable<int> CurrentLevel { get; set; }
        public string CurrentStatus { get; set; }
        public string ProgressStatus { get; set; }
        public Nullable<DateTime> DepartureDate { get; set; }
        public Nullable<DateTime> ReturningDate { get; set; }
        public string DeparturePlace { get; set; }
        public string TravelLogStatus { get; set; }
        public bool IsExtension { get; set; }
        public int ExtRefRequest_Id { get; set; }
        public string TravelLogAttachment { get; set; }
        public int ActualDaysTravelled { get; set; }
        public string DepartureTime { get; set; }
        public string PassengerDetails { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual Project Project { get; set; }
        public virtual Grant Grant { get; set; }
        public virtual int LastKmReading { get; set; }
        public virtual TravelLog TravelLog { get; set; }
        public virtual IList<VehicleRequestStatus> VehicleRequestStatuses { get; set; }
        public virtual IList<VehicleRequestDetail> VehicleRequestDetails { get; set; }

        #region VehicleRequestStatus
        public virtual VehicleRequestStatus GetVehicleRequestStatus(int Id)
        {

            foreach (VehicleRequestStatus VRS in VehicleRequestStatuses)
            {
                if (VRS.Id == Id)
                    return VRS;

            }
            return null;
        }
        public virtual VehicleRequestStatus GetVehicleRequestStatusworkflowLevel(int workflowLevel)
        {

            foreach (VehicleRequestStatus LRS in VehicleRequestStatuses)
            {
                if (LRS.WorkflowLevel == workflowLevel)
                    return LRS;

            }
            return null;
        }
        public virtual IList<VehicleRequestStatus> GetVehicleRequestStatusByRequestId(int RequestId)
        {
            IList<VehicleRequestStatus> VRS = new List<VehicleRequestStatus>();
            foreach (VehicleRequestStatus VR in VehicleRequestStatuses)
            {
                if (VR.VehicleRequest.Id == RequestId)
                    VRS.Add(VR);

            }
            return VRS;
        }
        public virtual void RemoveVehicleRequestStatus(int Id)
        {

            foreach (VehicleRequestStatus VRS in VehicleRequestStatuses)
            {
                if (VRS.Id == Id)
                    VehicleRequestStatuses.Remove(VRS);
                break;
            }

        }
        #endregion
        #region Vehicle
        public virtual VehicleRequestDetail GetVehicle(int Id)
        {
            foreach (VehicleRequestDetail V in VehicleRequestDetails)
            {
                if (V.Id == Id)
                    return V;
            }
            return null;
        }
        public virtual VehicleRequestDetail GetVehiclebypalte(string plateno)
        {
            foreach (VehicleRequestDetail V in VehicleRequestDetails)
            {
                if (V.PlateNo == plateno)
                    return V;
            }
            return null;
        }
        public virtual IList<VehicleRequestDetail> GetVehiclebypaltes()
        {
            IList<VehicleRequestDetail> detail = new List<VehicleRequestDetail>();
            foreach (VehicleRequestDetail V in VehicleRequestDetails)
            {
                    detail.Add(V);
                   
            }
            return detail;
        }
        public virtual void RemoveVehicle(int Id)
        {

            foreach (VehicleRequestDetail V in VehicleRequestDetails)
            {
                if (V.Id == Id)
                    VehicleRequestDetails.Remove(V);
                break;
            }

        }
        #endregion
    }
}
