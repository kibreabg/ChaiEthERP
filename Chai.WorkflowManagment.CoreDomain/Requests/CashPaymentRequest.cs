﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class CashPaymentRequest : IEntity
    {
        public CashPaymentRequest()
        {

            this.CashPaymentRequestDetails = new List<CashPaymentRequestDetail>();
            this.CashPaymentRequestStatuses = new List<CashPaymentRequestStatus>();
        }
        public int Id { get; set; }
        public string RequestNo { get; set; }
        public Nullable<DateTime> RequestDate { get; set; }
        public string Payee { get; set; }
        public string Description { get; set; }
        public string VoucherNo { get; set; }
        public string ArrivalDateTime { get; set; }
        public string ReturnDateTime { get; set; }
        public int CurrentApprover { get; set; }
        public int CurrentApproverPosition { get; set; }
        public int CurrentLevel { get; set; }
        public string CurrentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalActualExpendture { get; set; }
        public string ProgressStatus { get; set; }
        public string PaymentReimbursementStatus { get; set; }
        public string ExportStatus { get; set; }
        public string AmountType { get; set; }
        public string RequestType { get; set; }
        public bool IsLiquidated { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual Program Program { get; set; }
        public virtual int SettlementRejectionCount { get; set; }
        public virtual string SettlementRejectionReasons { get; set; }
        public virtual string SettlementRejetcedby { get; set; }
        public virtual PaymentReimbursementRequest PaymentReimbursementRequest { get; set; }
        public virtual IList<CashPaymentRequestDetail> CashPaymentRequestDetails { get; set; }
        public virtual IList<CashPaymentRequestStatus> CashPaymentRequestStatuses { get; set; }

        #region CashPaymentRequestStatus
        public virtual CashPaymentRequestStatus GetCashPaymentRequestStatus(int Id)
        {
            foreach (CashPaymentRequestStatus CPRS in CashPaymentRequestStatuses)
            {
                if (CPRS.Id == Id)
                    return CPRS;
            }
            return null;
        }
        public virtual CashPaymentRequestStatus GetCashPaymentRequestStatusworkflowLevel(int workflowLevel)
        {
            foreach (CashPaymentRequestStatus CPRS in CashPaymentRequestStatuses)
            {
                if (CPRS.WorkflowLevel == workflowLevel)
                    return CPRS;
            }
            return null;
        }
        public virtual IList<CashPaymentRequestStatus> GetCashPaymentRequestStatusByRequestId(int RequestId)
        {
            IList<CashPaymentRequestStatus> LRS = new List<CashPaymentRequestStatus>();
            foreach (CashPaymentRequestStatus CPRS in CashPaymentRequestStatuses)
            {
                if (CPRS.CashPaymentRequest.Id == RequestId)
                    LRS.Add(CPRS);
            }
            return LRS;
        }
        public virtual void RemoveCashPaymentRequestStatus(int Id)
        {
            foreach (CashPaymentRequestStatus CPRS in CashPaymentRequestStatuses)
            {
                if (CPRS.Id == Id)
                {
                    CashPaymentRequestStatuses.Remove(CPRS);
                    break;
                }
            }
        }
        #endregion
        #region CashPaymentRequestDetail
        public virtual CashPaymentRequestDetail GetCashPaymentRequestDetail(int Id)
        {

            foreach (CashPaymentRequestDetail CPRD in CashPaymentRequestDetails)
            {
                if (CPRD.Id == Id)
                    return CPRD;
            }
            return null;
        }

        public virtual CashPaymentRequestDetail GetDetailByItemAccount(int itemAccountId)
        {
            foreach (CashPaymentRequestDetail CPRD in CashPaymentRequestDetails)
            {
                if (CPRD.ItemAccount.Id == itemAccountId)
                    return CPRD;
            }
            return null;
        }
        public virtual void RemoveCashPaymentRequestDetail(int Id)
        {
            foreach (CashPaymentRequestDetail CPRS in CashPaymentRequestDetails)
            {
                if (CPRS.Id == Id)
                {
                    CashPaymentRequestDetails.Remove(CPRS);
                    break;
                }
            }
        }
        #endregion

    }
}
