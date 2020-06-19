using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;
using System;
using System.Collections.Generic;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class MaintenanceRequest : IEntity
    {

        public MaintenanceRequest()
        {
            this.MaintenanceRequestStatuses = new List<MaintenanceRequestStatus>();
            this.MaintenanceRequestDetails = new List<MaintenanceRequestDetail>();
            
        }
        public int Id { get; set; }
        public string RequestNo { get; set; }
        
        public DateTime RequestedDate { get; set; }
        public string MaintenanceType { get; set; }

        public int Requester { get; set; }
        public bool IsVehicle { get; set; }
        public string PlateNo { get; set; }
        public string KmReading { get; set; }
        public string Comment { get; set; }
        public int CurrentApprover { get; set; }
        public int CurrentLevel { get; set; }
        public string CurrentStatus { get; set; }
        public string ProgressStatus { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual Vehicle Vehicles { get; set; }

        public virtual ServiceType ServiceTpes { get; set; }
       
        public virtual IList<MaintenanceRequestStatus> MaintenanceRequestStatuses { get; set; }
        public virtual IList<MaintenanceRequestDetail> MaintenanceRequestDetails { get; set; }
        // public virtual PurchaseOrder PurchaseOrders { get; set; }
        #region MaintenanceRequestStatus
        public virtual MaintenanceRequestStatus GetMaintenanceRequestStatus(int Id)
        {

            foreach (MaintenanceRequestStatus MRS in MaintenanceRequestStatuses)
            {
                if (MRS.Id == Id)
                    return MRS;

            }
            return null;
        }
        public virtual MaintenanceRequestStatus GetMaintenanceRequestStatusworkflowLevel(int workflowLevel)
        {

            foreach (MaintenanceRequestStatus MRS in MaintenanceRequestStatuses)
            {
                if (MRS.WorkflowLevel == workflowLevel)
                    return MRS;

            }
            return null;
        }
        public virtual IList<MaintenanceRequestStatus> GetMaintenanceRequestStatusByRequestId(int RequestId)
        {
            IList<MaintenanceRequestStatus> MRS = new List<MaintenanceRequestStatus>();
            foreach (MaintenanceRequestStatus MR in MaintenanceRequestStatuses)
            {
                if (MR.MaintenanceRequest.Id == RequestId)
                    MRS.Add(MR);

            }
            return MRS;
        }
        public virtual void RemoveMaintenanceRequestStatus(int Id)
        {

            foreach (MaintenanceRequestStatus MRS in MaintenanceRequestStatuses)
            {
                if (MRS.Id == Id)
                    MaintenanceRequestStatuses.Remove(MRS);
                break;
            }

        }

        #endregion
        #region MaintenanceRequestDetail
        public virtual MaintenanceRequestDetail GetMaintenanceRequestDetail(int Id)
        {

            foreach (MaintenanceRequestDetail MRS in MaintenanceRequestDetails)
            {
                if (MRS.Id == Id)
                    return MRS;

            }
            return null;
        }
        public virtual IList<MaintenanceRequestDetail> GetMaintenanceRequestDetailByMaintenanceId(int MaintenanceId)
        {
            IList<MaintenanceRequestDetail> MRS = new List<MaintenanceRequestDetail>();
            foreach (MaintenanceRequestDetail MR in MaintenanceRequestDetails)
            {
                if (MR.MaintenanceRequest.Id == MaintenanceId)
                    MRS.Add(MR);

            }
            return MRS;
        }
     
        public virtual void RemoveMaintenanceRequestDetail(int Id)
        {

            foreach (MaintenanceRequestDetail MRS in MaintenanceRequestDetails)
            {
                if (MRS.Id == Id)
                    MaintenanceRequestDetails.Remove(MRS);
                break;
            }

        }
        #endregion
       


    }
}
