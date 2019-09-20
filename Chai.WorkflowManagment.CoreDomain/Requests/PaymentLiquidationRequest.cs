using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class PaymentLiquidationRequest : IEntity
    {
        public PaymentLiquidationRequest()
        {
            this.PaymentLiquidationRequestStatuses = new List<PaymentLiquidationRequestStatus>();
            this.PaymentLiquidationRequestDetails = new List<PaymentLiquidationRequestDetail>();
            this.PLRAttachments = new List<PLRAttachment>();
        }
        public int Id { get; set; }
        public Nullable<DateTime> RequestDate { get; set; }
        public Nullable<DateTime> PaymentRequestDate { get; set; }
        public string ExpenseType { get; set; }        
        public string Comment { get; set; }
        public string ExpenseReimbersmentType { get; set; }
        public string ReimbersmentNo { get; set; }
        public string ExportStatus { get; set; }
        public int CurrentApprover { get; set; }
        public int CurrentLevel { get; set; }
        public string CurrentStatus { get; set; }
        public string ProgressStatus { get; set; }
        public decimal TotalActualExpenditure { get; set; }
        public decimal TotalTravelAdvance { get; set; }
        public string AdditionalComment { get; set; }

        [Required]
        public virtual CashPaymentRequest CashPaymentRequest { get; set; }
        public virtual IList<PaymentLiquidationRequestDetail> PaymentLiquidationRequestDetails { get; set; }
        public virtual IList<PaymentLiquidationRequestStatus> PaymentLiquidationRequestStatuses { get; set; }
        public virtual IList<PLRAttachment> PLRAttachments { get; set; }

        #region LiquidationDetails
        public virtual PaymentLiquidationRequestDetail GetPaymentLiquidationRequestDetail(int Id)
        {

            foreach (PaymentLiquidationRequestDetail PaymentLiquidationRequestDetail in PaymentLiquidationRequestDetails)
            {
                if (PaymentLiquidationRequestDetail.Id == Id)
                    return PaymentLiquidationRequestDetail;
            }
            return null;
        }

        public virtual IList<PaymentLiquidationRequestDetail> GetPaymentLiquidationRequestDetailByLiquidationId(int liquidationId)
        {
            IList<PaymentLiquidationRequestDetail> LiquidationDetails = new List<PaymentLiquidationRequestDetail>();
            foreach (PaymentLiquidationRequestDetail PaymentLiquidationRequestDetail in PaymentLiquidationRequestDetails)
            {
                if (PaymentLiquidationRequestDetail.PaymentLiquidationRequest.Id == liquidationId)
                    LiquidationDetails.Add(PaymentLiquidationRequestDetail);

            }
            return LiquidationDetails;
        }
        public virtual void RemovePaymentLiquidationRequestDetail(int Id)
        {

            foreach (PaymentLiquidationRequestDetail PaymentLiquidationRequestDetail in PaymentLiquidationRequestDetails)
            {
                if (PaymentLiquidationRequestDetail.Id == Id)
                    PaymentLiquidationRequestDetails.Remove(PaymentLiquidationRequestDetail);
                break;
            }

        }

        #endregion
        #region PLRAttachments

        public virtual void RemovePLRAttachment(string FilePath)
        {
            foreach (PLRAttachment cpa in PLRAttachments)
            {
                if (cpa.FilePath == FilePath)
                {
                    PLRAttachments.Remove(cpa);
                    break;
                }
            }
        }
        #endregion
    }
}
