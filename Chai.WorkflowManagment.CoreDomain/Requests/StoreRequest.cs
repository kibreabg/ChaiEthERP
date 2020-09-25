using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class StoreRequest : IEntity
    {

        public StoreRequest()
        {
            this.StoreRequestStatuses = new List<StoreRequestStatus>();
            this.StoreRequestDetails = new List<StoreRequestDetail>();
        }
        public int Id { get; set; }
        public string RequestNo { get; set; }
        public int Requester { get; set; }
        public int purchaseId { get; set; }
        public DateTime RequestedDate { get; set; }
        public string DeliverTo { get; set; }
        public string Comment { get; set; }
        public int CurrentApprover { get; set; }
        public int CurrentLevel { get; set; }
        public string CurrentStatus { get; set; }
        public string ProgressStatus { get; set; }
        public virtual IList<StoreRequestStatus> StoreRequestStatuses { get; set; }
        public virtual IList<StoreRequestDetail> StoreRequestDetails { get; set; }
        #region StoreRequestStatus
        public virtual StoreRequestStatus GetStoreRequestStatus(int Id)
        {

            foreach (StoreRequestStatus PRS in StoreRequestStatuses)
            {
                if (PRS.Id == Id)
                    return PRS;

            }
            return null;
        }
        public virtual StoreRequestStatus GetStoreRequestStatusworkflowLevel(int workflowLevel)
        {

            foreach (StoreRequestStatus PRS in StoreRequestStatuses)
            {
                if (PRS.WorkflowLevel == workflowLevel)
                    return PRS;

            }
            return null;
        }
        public virtual IList<StoreRequestStatus> GetStoreRequestStatusByRequestId(int RequestId)
        {
            IList<StoreRequestStatus> LRS = new List<StoreRequestStatus>();
            foreach (StoreRequestStatus AR in StoreRequestStatuses)
            {
                if (AR.StoreRequest.Id == RequestId)
                    LRS.Add(AR);

            }
            return LRS;
        }
        public virtual void RemoveStoreRequestStatus(int Id)
        {

            foreach (StoreRequestStatus PRS in StoreRequestStatuses)
            {
                if (PRS.Id == Id)
                    StoreRequestStatuses.Remove(PRS);
                break;
            }

        }

        #endregion
        #region StoreRequestDetail
        public virtual StoreRequestDetail GetStoreRequestDetail(int Id)
        {

            foreach (StoreRequestDetail PRS in StoreRequestDetails)
            {
                if (PRS.Id == Id)
                    return PRS;

            }
            return null;
        }
        public virtual IList<StoreRequestDetail> GetStoreRequestDetailByStoreId(int StoreId)
        {
            IList<StoreRequestDetail> LRS = new List<StoreRequestDetail>();
            foreach (StoreRequestDetail AR in StoreRequestDetails)
            {
                if (AR.StoreRequest.Id == StoreId)
                    LRS.Add(AR);

            }
            return LRS;
        }
        public virtual IList<StoreRequestDetail> GetPurchaseReqDetails(int StoreRequestId)
        {
            IList<StoreRequestDetail> LRS = new List<StoreRequestDetail>();

            foreach (StoreRequestDetail AR in StoreRequestDetails)
            {
                if (AR.StoreRequest.Id == StoreRequestId)
                    LRS.Add(AR);

            }
            return LRS;

        }
        public virtual void RemoveStoreRequestDetail(int Id)
        {

            foreach (StoreRequestDetail PRS in StoreRequestDetails)
            {
                if (PRS.Id == Id)
                    StoreRequestDetails.Remove(PRS);
                break;
            }

        }
        #endregion
        #region Public Methods
        public virtual bool IsItemAlreadyRequested(int itemId)
        {
            foreach (StoreRequestDetail SRD in StoreRequestDetails)
            {
                if (SRD.Item.Id == itemId)
                    return true;
            }
            return false;
        }
        #endregion

    }
}
