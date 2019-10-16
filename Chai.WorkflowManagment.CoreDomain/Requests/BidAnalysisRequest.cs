using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;
using System.ComponentModel.DataAnnotations;
using Chai.WorkflowManagment.CoreDomain.Approval;


namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class BidAnalysisRequest : IEntity
    {
        public BidAnalysisRequest()
        {
            this.BidAnalysisRequestStatuses = new List<BidAnalysisRequestStatus>();
            this.BAAttachments = new List<BAAttachment>();
            this.BidderItemDetails = new List<BidderItemDetail>();
            this.BidAnalysisRequestDetails = new List<BidAnalysisRequestDetail>();
    
        }
        public int Id { get; set; }
    
     
      
      
        public string RequestNo { get; set; }
        public Nullable<DateTime> RequestDate { get; set; }
        public DateTime AnalyzedDate { get; set; }
  
        public string SpecialNeed { get; set; }
        
        public virtual Supplier Supplier { get; set; }

      
        public decimal TotalPrice { get; set; }
        public string ReasonforSelection { get; set; }
        public int SelectedBy { get; set; }
        public string Status { get; set; }
        public int CurrentApprover { get; set; }
        public Nullable<int> CurrentLevel { get; set; }
        public string  CurrentStatus { get; set; }
        public string ProgressStatus { get; set; }        
        public virtual AppUser AppUser { get; set; }
        public virtual Project Project { get; set; }
        public virtual Grant Grant { get; set; }
   
        public virtual PurchaseRequest PurchaseRequest { get; set; }
      
        public virtual IList<BidAnalysisRequestStatus> BidAnalysisRequestStatuses { get; set; }
        public virtual PurchaseOrder PurchaseOrders { get; set; }        
        public virtual IList<BidderItemDetail> BidderItemDetails { get; set; }
        public virtual IList<BAAttachment> BAAttachments { get; set; }
        public virtual IList<BidAnalysisRequestDetail> BidAnalysisRequestDetails { get; set; }
        #region Bidder
        public virtual BidderItemDetail GetBidderItemDetail(int Id)
        {

            foreach (BidderItemDetail bidderItemDetail in BidderItemDetails)
            {
                if (bidderItemDetail.Id == Id)
                    return bidderItemDetail;

            }
            return null;
        }
       
        public virtual IList<BidderItemDetail> GetBidderItemDetailByBidAnalysisId(int AnalisisId)
        {
            IList<BidderItemDetail> BidderItemDetails = new List<BidderItemDetail>();
            foreach (BidderItemDetail bidderItemDetail in BidderItemDetails)
            {
                if (bidderItemDetail.BidAnalysisRequest.Id == AnalisisId)
                    BidderItemDetails.Add(bidderItemDetail);

            }
            return BidderItemDetails;
        }
        public virtual void RemoveBidderItemDetail(int Id)
        {

            foreach (BidderItemDetail bidderItemDetail in BidderItemDetails)
            {
                if (bidderItemDetail.Id == Id)
                    BidderItemDetails.Remove(bidderItemDetail);
                break;
            }

        }

        public IList<Bidder> GetAllBidders()
        {
            IList<Bidder> details = new List<Bidder>();
            foreach (BidderItemDetail b in BidderItemDetails)
            {
                foreach (Bidder det in b.Bidders)
                {
                    details.Add(det);
                }
            }
            return details;
        }
        #endregion
        #region BidAnalysisRequestDetail
        public virtual BidAnalysisRequestDetail GetBidAnalysisRequestDetail(int Id)
        {

            foreach (BidAnalysisRequestDetail PRS in BidAnalysisRequestDetails)
            {
                if (PRS.Id == Id)
                    return PRS;

            }
            return null;
        }

        public virtual IList<BidAnalysisRequestDetail> GetBidAnalysisRequestDetailByPurchaseId(int PurchaseId)
        {
            IList<BidAnalysisRequestDetail> LRS = new List<BidAnalysisRequestDetail>();
            foreach (BidAnalysisRequestDetail AR in BidAnalysisRequestDetails)
            {
                if (AR.BidAnalysisRequest.Id == PurchaseId)
                    LRS.Add(AR);

            }
            return LRS;
        }
        public virtual void RemoveBidAnalysisRequestDetail(int Id)
        {

            foreach (BidAnalysisRequestDetail PRS in BidAnalysisRequestDetails)
            {
                if (PRS.Id == Id)
                    BidAnalysisRequestDetails.Remove(PRS);
                break;
            }

        }

        #endregion
        #region BidAnalysisRequestStatus
        public virtual BidAnalysisRequestStatus GetBidAnalysisRequestStatus(int Id)
        {

            foreach (BidAnalysisRequestStatus SVRS in BidAnalysisRequestStatuses)
            {
                if (SVRS.Id == Id)
                    return SVRS;

            }
            return null;
        }
        public virtual BidAnalysisRequestStatus GetBidAnalysisRequestStatusworkflowLevel(int workflowLevel)
        {

            foreach (BidAnalysisRequestStatus LRS in BidAnalysisRequestStatuses)
            {
                if (LRS.WorkflowLevel == workflowLevel)
                    return LRS;

            }
            return null;
        }
        public virtual IList<BidAnalysisRequestStatus> GetBidAnalysisRequestStatusByRequestId(int RequestId)
        {
            IList<BidAnalysisRequestStatus> VRS = new List<BidAnalysisRequestStatus>();
            foreach (BidAnalysisRequestStatus VR in BidAnalysisRequestStatuses)
            {
                if (VR.BidAnalysisRequest.Id == RequestId)
                    VRS.Add(VR);

            }
            return VRS;
        }
        public virtual void RemoveBidAnalysisRequestStatus(int Id)
        {

            foreach (BidAnalysisRequestStatus VRS in BidAnalysisRequestStatuses)
            {
                if (VRS.Id == Id)
                    BidAnalysisRequestStatuses.Remove(VRS);
                break;
            }

        }
        #endregion
        #region BAAttachment
        public virtual void RemoveBAAttachment(string FilePath)
        {
            foreach (BAAttachment cpa in BAAttachments)
            {
                if (cpa.FilePath == FilePath)
                {
                    BAAttachments.Remove(cpa);
                    break;
                }
            }
        }
        #endregion
    }
}
