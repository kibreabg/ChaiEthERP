using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Utility;
using Microsoft.Practices.ObjectBuilder;

using Chai.WorkflowManagment.CoreDomain;
using Chai.WorkflowManagment.CoreDomain.DataAccess;
using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Services;
using Chai.WorkflowManagment.Shared.Navigation;


using System.Data;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.TravelLogs;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.CoreDomain.Request;
using Chai.WorkflowManagment.CoreDomain.HRM;

namespace Chai.WorkflowManagment.Modules.Request
{
    public class RequestController : ControllerBase
    {
        private IWorkspace _workspace;

        [InjectionConstructor]
        public RequestController([ServiceDependency] IHttpContextLocatorService httpContextLocatorService, [ServiceDependency]INavigationService navigationService)
            : base(httpContextLocatorService, navigationService)
        {
            _workspace = ZadsServices.Workspace;
        }
        public AppUser GetSuperviser(int superviser)
        {
            return _workspace.Single<AppUser>(x => x.Id == superviser);
        }
        public AppUser CurrentUser()
        {
            return GetCurrentUser();
        }
        public AppUser Approver(int position)
        {
            return _workspace.SqlQuery<AppUser>("SELECT * FROM AppUsers WHERE EmployeePosition_Id = " + position).ToList().Last<AppUser>();
        }

        #region CurrenrObject
        public object CurrentObject
        {
            get
            {
                return GetCurrentContext().Session["CurrentObject"];
            }
            set
            {
                GetCurrentContext().Session["CurrentObject"] = value;
            }
        }
        #endregion
        #region Travel Log
        public IList<TravelLog> GetTravelLogs()
        {
            return WorkspaceFactory.CreateReadOnly().Query<TravelLog>(null).ToList();
        }
        public TravelLog GetTravelLog(int TravelLogId)
        {
            return _workspace.Single<TravelLog>(x => x.Id == TravelLogId);
        }
        public IList<TravelLog> ListTravelLogs(int RequestId)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM TravelLogs Where 1 = Case when '" + RequestId + "' = '' Then 1 When TravelLogs.VehicleRequest_Id = '" + RequestId + "' Then 1 END ";

            return _workspace.SqlQuery<TravelLog>(filterExpression).ToList();

        }
        #endregion
        #region Vehicle Requests
        public IList<VehicleRequest> GetVehicleRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<VehicleRequest>(null).ToList();
        }
        public VehicleRequest GetVehicleRequest(int id)
        {
            return _workspace.Single<VehicleRequest>(x => x.Id == id);
        }
        public IList<VehicleRequest> ListVehicleRequests(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM VehicleRequests Where 1 = Case when '" + RequestNo + "' = '' Then 1 When VehicleRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When VehicleRequests.RequestDate = '" + RequestDate + "'  Then 1 END And VehicleRequests.AppUser_Id='" + GetCurrentUser().Id + "' order by VehicleRequests.Id Desc ";

            return _workspace.SqlQuery<VehicleRequest>(filterExpression).ToList();

        }
        public int GetLastVehicleRequestId()
        {
            if (_workspace.Last<VehicleRequest>() != null)
            {
                return _workspace.Last<VehicleRequest>().Id;
            }
            else { return 0; }
        }
        public VehicleRequestDetail GetAssignedVehicleById(int id)
        {
            return _workspace.Single<VehicleRequestDetail>(x => x.Id == id);
        }
        public IList<VehicleRequest> GetExtVehicleRequest()
        {
            int currentUserId = GetCurrentUser().Id;
            return WorkspaceFactory.CreateReadOnly().Query<VehicleRequest>(x => x.AppUser.Id == currentUserId && x.CurrentStatus != "Rejected").ToList();
        }

        #endregion
        #region Cash Payment
        public CashPaymentRequest GetCashPaymentRequest(int RequestId)
        {
            return _workspace.Single<CashPaymentRequest>(x => x.Id == RequestId);
        }
        public IList<CashPaymentRequest> ListCashPaymentRequests(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM CashPaymentRequests LEFT JOIN Suppliers on CashPaymentRequests.Supplier_Id = Suppliers.Id Where 1 = Case when '" + RequestNo + "' = '' Then 1 When CashPaymentRequests.VoucherNo = '" + RequestNo + "' Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When CashPaymentRequests.RequestDate = '" + RequestDate + "'  Then 1 END And CashPaymentRequests.AppUser_Id='" + GetCurrentUser().Id + "' ORDER BY CashPaymentRequests.Id Desc";
            // return WorkspaceFactory.CreateReadOnly().Queryable<CashPaymentRequest>(filterExpression).ToList();
            return _workspace.SqlQuery<CashPaymentRequest>(filterExpression).ToList();
        }
        public IList<CashPaymentRequest> GetCashPaymentRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<CashPaymentRequest>(null).ToList();
        }
        public IList<CashPaymentRequest> ListCashPaymentsNotExpensed()
        {
            int currentUserId = GetCurrentUser().Id;
            return WorkspaceFactory.CreateReadOnly().Query<CashPaymentRequest>(x => x.IsLiquidated == false && x.AmountType == "Advanced" && x.ProgressStatus == "Completed" && currentUserId == x.AppUser.Id).ToList();
        }
        public IList<CashPaymentRequest> GetAllOutPatMedCPReqsThisYear()
        {
            int currentUserId = GetCurrentUser().Id;
            return WorkspaceFactory.CreateReadOnly().Query<CashPaymentRequest>(x => x.RequestType == "Medical Expense (Out-Patient)" && x.AppUser.Id == currentUserId && x.RequestDate.Value.Year == DateTime.Now.Year && (x.CurrentStatus != "Rejected" || x.CurrentStatus == null)).ToList();
        }
        public IList<CashPaymentRequest> GetAllInPatMedCPReqsThisYear()
        {
            int currentUserId = GetCurrentUser().Id;
            return WorkspaceFactory.CreateReadOnly().Query<CashPaymentRequest>(x => x.RequestType == "Medical Expense (In-Patient)" && x.AppUser.Id == currentUserId && x.RequestDate.Value.Year == DateTime.Now.Year && (x.CurrentStatus != "Rejected" || x.CurrentStatus == null)).ToList();
        }
        public CashPaymentRequestDetail GetCashPaymentRequestDetail(int CPRDId)
        {
            return _workspace.Single<CashPaymentRequestDetail>(x => x.Id == CPRDId);
        }
        public int GetLastCashPaymentRequestId()
        {
            if (_workspace.Last<CashPaymentRequest>() != null)
            {
                return _workspace.Last<CashPaymentRequest>().Id;
            }
            else { return 0; }
        }

        public CPRAttachment GetCPRAttachment(int attachmentId)
        {
            return _workspace.Single<CPRAttachment>(x => x.Id == attachmentId);
        }
        #endregion
        #region Cost Sharing
        public CostSharingRequest GetCostSharingRequest(int RequestId)
        {
            return _workspace.Single<CostSharingRequest>(x => x.Id == RequestId);
        }
        public IList<CostSharingRequest> ListCostSharingRequests(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM CostSharingRequests Where 1 = Case when '" + RequestNo + "' = '' Then 1 When CostSharingRequests.VoucherNo = '" + RequestNo + "' Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When CostSharingRequests.RequestDate = '" + RequestDate + "'  Then 1 END And CostSharingRequests.AppUser_Id='" + GetCurrentUser().Id + "' ORDER BY CostSharingRequests.Id Desc";

            return _workspace.SqlQuery<CostSharingRequest>(filterExpression).ToList();
        }
        public IList<CostSharingRequest> GetCostSharingRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<CostSharingRequest>(null).ToList();
        }
        public IList<CostSharingRequest> ListCostSharingRequestsNotExpensed()
        {
            int currentUserId = GetCurrentUser().Id;
            return WorkspaceFactory.CreateReadOnly().Query<CostSharingRequest>(x => x.PaymentReimbursementStatus == "Completed" && currentUserId == x.AppUser.Id).ToList();
        }
        public CostSharingRequestDetail GetCostSharingRequestDetail(int CSRDId)
        {
            return _workspace.Single<CostSharingRequestDetail>(x => x.Id == CSRDId);
        }
        public int GetLastCostSharingRequestId()
        {
            if (_workspace.Last<CostSharingRequest>() != null)
            {
                return _workspace.Last<CostSharingRequest>().Id;
            }
            else { return 0; }
        }
        public CSRAttachment GetCSRAttachment(int attachmentId)
        {
            return _workspace.Single<CSRAttachment>(x => x.Id == attachmentId);
        }
        #endregion
        #region Payment Reimbursement
        public IList<PaymentReimbursementRequest> ListPaymentReimbursementRequests(string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM PaymentReimbursementRequests Where 1 = Case when '" + RequestDate + "' = '' Then 1 When PaymentReimbursementRequests.RequestDate = '" + RequestDate + "'  Then 1 END ORDER BY PaymentReimbursementRequests.Id Desc ";

            return _workspace.SqlQuery<PaymentReimbursementRequest>(filterExpression).ToList();
        }
        public PaymentReimbursementRequest GetPaymentReimbursementRequest(int RequestId)
        {
            return _workspace.Single<PaymentReimbursementRequest>(x => x.Id == RequestId);
        }
        public IList<PaymentReimbursementRequest> GetPaymentReimbursementRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<PaymentReimbursementRequest>(null).ToList();
        }
        //public ELRAttachment GetELRAttachment(int attachmentId)
        //{
        //    return _workspace.Single<ELRAttachment>(x => x.Id == attachmentId);
        //}
        #endregion
        #region Bank Payment
        public BankPaymentRequest GetBankPaymentRequest(int RequestId)
        {
            return _workspace.Single<BankPaymentRequest>(x => x.Id == RequestId);
        }
        public IList<BankPaymentRequest> GetBankPaymentRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<BankPaymentRequest>(null).ToList();
        }
        public int GetLastBankPaymentRequestId()
        {
            if (_workspace.Last<BankPaymentRequest>() != null)
            {
                return _workspace.Last<BankPaymentRequest>().Id;
            }
            else { return 1; }
        }
        public IList<BankPaymentRequest> ListBankPaymentRequests(string RequestNo, string ProcessDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM BankPaymentRequests Where 1 = Case when '" + RequestNo + "' = '' Then 1 When BankPaymentRequests.RequestNo = '" + RequestNo + "' Then 1 END And  1 = Case when '" + ProcessDate + "' = '' Then 1 When BankPaymentRequests.ProcessDate = '" + ProcessDate + "'  Then 1 END ORDER BY BankPaymentRequests.Id Desc";

            return _workspace.SqlQuery<BankPaymentRequest>(filterExpression).ToList();
        }
        public BankPaymentRequestDetail GetBankPaymentRequestDetail(int BPRDId)
        {
            return _workspace.Single<BankPaymentRequestDetail>(x => x.Id == BPRDId);
        }
        #endregion
        #region Operational Control
        public OperationalControlRequest GetOperationalControlRequest(int RequestId)
        {
            return _workspace.Single<OperationalControlRequest>(x => x.Id == RequestId);
        }
        public IList<OperationalControlRequest> ListOperationalControlRequests(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM OperationalControlRequests Where 1 = Case when '" + RequestNo + "' = '' Then 1 When OperationalControlRequests.VoucherNo = '" + RequestNo + "' Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When OperationalControlRequests.RequestDate = '" + RequestDate + "'  Then 1 END ORDER BY OperationalControlRequests.Id Desc";

            return _workspace.SqlQuery<OperationalControlRequest>(filterExpression).ToList();
        }
        public IList<OperationalControlRequest> GetOperationalControlRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<OperationalControlRequest>(null).ToList();
        }
        public IList<OperationalControlRequest> ListOperationalControlsNotExpensed()
        {
            //int currentUserId = GetCurrentUser().Id;
            //return WorkspaceFactory.CreateReadOnly().Query<OperationalControlRequest>(x => x.PaymentReimbursementStatus == "Completed" && x.PaymentReimbursementRequest == null && currentUserId == x.AppUser.Id).ToList();
            return null;
        }
        public OperationalControlRequestDetail GetOperationalControlRequestDetail(int OCRDId)
        {
            return _workspace.Single<OperationalControlRequestDetail>(x => x.Id == OCRDId);
        }
        public int GetLastOperationalControlRequestId()
        {
            if (_workspace.Last<OperationalControlRequest>() != null)
            {
                return _workspace.Last<OperationalControlRequest>().Id;
            }
            else { return 0; }
        }
        public OCRAttachment GetOCRAttachment(int attachmentId)
        {
            return _workspace.Single<OCRAttachment>(x => x.Id == attachmentId);
        }
        #endregion
        #region Travel Advance Request
        public TravelAdvanceRequest GetTravelAdvanceRequest(int TravelAdvanceRequestId)
        {
            return _workspace.Single<TravelAdvanceRequest>(x => x.Id == TravelAdvanceRequestId);
        }
        public IList<TravelAdvanceRequest> GetTravelAdvanceRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<TravelAdvanceRequest>(null).ToList();
        }
        public int GetLastTravelAdvanceRequestId()
        {
            if (_workspace.Last<TravelAdvanceRequest>() != null)
            {
                return _workspace.Last<TravelAdvanceRequest>().Id;
            }
            else { return 0; }
        }
        public IList<TravelAdvanceRequest> ListTravelAdvanceRequests(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM TravelAdvanceRequests Where 1 = Case when '" + RequestNo + "' = '' Then 1 When TravelAdvanceRequests.TravelAdvanceNo = '" + RequestNo + "' Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When TravelAdvanceRequests.RequestDate = '" + RequestDate + "'  Then 1 END And TravelAdvanceRequests.AppUser_Id='" + GetCurrentUser().Id + "' ORDER BY TravelAdvanceRequests.Id Desc ";

            return _workspace.SqlQuery<TravelAdvanceRequest>(filterExpression).ToList();
        }
        public IList<TravelAdvanceRequest> ListTravelAdvancesNotExpensed()
        {
            int currentUserId = GetCurrentUser().Id;
            return WorkspaceFactory.CreateReadOnly().Query<TravelAdvanceRequest>(x => x.ExpenseLiquidationStatus == "Completed" && x.ExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses.Count == 0 && x.AppUser.Id == currentUserId).ToList();
        }
        public TravelAdvanceRequestDetail GetTravelAdvanceRequestDetail(int id)
        {
            return _workspace.Single<TravelAdvanceRequestDetail>(x => x.Id == id);
        }
        public TravelAdvanceCost GetTravelAdvanceCost(int id)
        {
            return _workspace.Single<TravelAdvanceCost>(x => x.Id == id);
        }
        #endregion
        #region Expense Liquidation
        public IList<ExpenseLiquidationRequest> ListExpenseLiquidationRequests(string ExpenseType, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM ExpenseLiquidationRequests INNER JOIN TravelAdvanceRequests ON TravelAdvanceRequests.Id = ExpenseLiquidationRequests.Id Where 1 = Case when '" + ExpenseType + "' = '' Then 1 When ExpenseLiquidationRequests.ExpenseType = '" + ExpenseType + "' Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When ExpenseLiquidationRequests.RequestDate = '" + RequestDate + "'  Then 1 END AND TravelAdvanceRequests.AppUser_Id='" + GetCurrentUser().Id + "' ORDER BY ExpenseLiquidationRequests.Id Desc ";

            return _workspace.SqlQuery<ExpenseLiquidationRequest>(filterExpression).ToList();
        }
        public ExpenseLiquidationRequest GetExpenseLiquidationRequest(int RequestId)
        {
            return _workspace.Single<ExpenseLiquidationRequest>(x => x.Id == RequestId);
        }
        public IList<ExpenseLiquidationRequest> GetExpenseLiquidationRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<ExpenseLiquidationRequest>(null).ToList();
        }
        public PRAttachment GetPRAttachment(int attachmentId)
        {
            return _workspace.Single<PRAttachment>(x => x.Id == attachmentId);
        }
        public ELRAttachment GetELRAttachment(int attachmentId)
        {
            return _workspace.Single<ELRAttachment>(x => x.Id == attachmentId);
        }
        #endregion
        #region LeaveRequest

        public IList<LeaveRequest> GetLeaveRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<LeaveRequest>(null).ToList();
        }
        public LeaveRequest GetLeaveRequest(int LeaveRequestId)
        {
            return _workspace.Single<LeaveRequest>(x => x.Id == LeaveRequestId);
        }
        public decimal getTotalSickLeaveTaken(int EmpId)
        {
            string filterExpression = "";

            filterExpression = "SELECT *  FROM LeaveRequests Inner Join LeaveTypes on LeaveRequests.LeaveType_Id = LeaveTypes.Id "
                               + " Where LeaveTypes.LeaveTypeName = 'Sick Leave' and LeaveRequests.CurrentStatus= 'Issued' and LeaveRequests.Requester = '" + EmpId + "'and Year(LeaveRequests.RequestedDate) = '" + DateTime.Today.Year + "'";
            // return WorkspaceFactory.CreateReadOnly().Queryable<CashPaymentRequest>(filterExpression).ToList();
            IList<LeaveRequest> EmpLeaverequest = _workspace.SqlQuery<LeaveRequest>(filterExpression).ToList();
            return EmpLeaverequest.Sum(x => x.RequestedDays);
        }
        public IList<LeaveRequest> ListLeaveRequests(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT  *  FROM LeaveRequests Where 1 = Case when '" + RequestNo + "' = '' Then 1 When LeaveRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When LeaveRequests.RequestedDate = '" + RequestDate + "'  Then 1 END And LeaveRequests.Requester='" + GetCurrentUser().Id + "'order by LeaveRequests.Id DESC ";

            return _workspace.SqlQuery<LeaveRequest>(filterExpression).ToList();

        }
        public int GetLastLeaveRequestId()
        {
            if (_workspace.Last<LeaveRequest>() != null)
            {
                return _workspace.Last<LeaveRequest>().Id;
            }
            else { return 0; }
        }
        #endregion
        #region AssignJob
        public AssignJob GetAssignedJobbycurrentuser()
        {
            int userId = GetCurrentUser().Id;
            return _workspace.Single<AssignJob>(x => x.AppUser.Id == userId && x.Status == true);
        }
        public AssignJob GetAssignedJobbycurrentuser(int UserId)
        {
            //int userId = GetCurrentUser().Id;
            return _workspace.Single<AssignJob>(x => x.AppUser.Id == UserId && x.Status == true);
        }
        public int GetAssignedUserbycurrentuser()
        {
            int userId = GetCurrentUser().Id;
            IList<AssignJob> AJ = _workspace.All<AssignJob>(x => x.AssignedTo == userId && x.Status == true).ToList();
            if (AJ.Count != 0)
            { return AJ[0].AssignedTo; }
            else
                return 0;
        }
        #endregion
        #region PurchaseRequest

        public IList<PurchaseRequest> GetPurchaseRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<PurchaseRequest>(null).ToList();
        }
        public IList<PurchaseRequest> GetDistinctCompletedPurchaseReqs()
        {
            return WorkspaceFactory.CreateReadOnly().Query<PurchaseRequest>(x => x.ProgressStatus == "Completed" && x.IsVehicle == true).Distinct().ToList();
        }
      
        public IList<PurchaseRequest> GetPurchaseRequestsInProgress()
        {
            string filterExpression = "";
            filterExpression = "SELECT DISTINCT PurchaseRequests.Id,RequestNo,Requester,RequestedDate,Requireddateofdelivery,TotalPrice,SpecialNeed,NeededFor, " +
                                      " DeliverTo,Comment,SuggestedSupplier,IsVehicle,MaintenanceRequestNo,CurrentApprover,CurrentLevel,ProgressStatus,CurrentStatus FROM " +
                                      " PurchaseRequests INNER JOIN PurchaseRequestDetails ON dbo.PurchaseRequestDetails.PurchaseRequest_Id = PurchaseRequests.Id" +
                                       " WHERE PurchaseRequestDetails.BidAnalysisRequestStatus = 'InProgress' AND PurchaseRequests.ProgressStatus = 'Completed' ORDER BY PurchaseRequests.Id DESC ";

            return _workspace.SqlQuery<PurchaseRequest>(filterExpression).ToList();
        }
        public IList<PurchaseRequest> GetPurchaseRequestsCompleted()
        {
            string filterExpression = "";
            filterExpression = "SELECT DISTINCT PurchaseRequests.Id,RequestNo,Requester,RequestedDate,Requireddateofdelivery,TotalPrice,SpecialNeed,NeededFor, " +
                                      " DeliverTo,Comment,SuggestedSupplier,IsVehicle,MaintenanceRequestNo,CurrentApprover,CurrentLevel,ProgressStatus,CurrentStatus FROM " +
                                      " PurchaseRequests INNER JOIN PurchaseRequestDetails ON dbo.PurchaseRequestDetails.PurchaseRequest_Id = PurchaseRequests.Id" +
                                       " WHERE PurchaseRequestDetails.BidAnalysisRequestStatus = 'InProgress' AND PurchaseRequests.ProgressStatus = 'Completed' ORDER BY PurchaseRequests.Id DESC ";

            return _workspace.SqlQuery<PurchaseRequest>(filterExpression).ToList();
        }
        public IList<PurchaseRequestDetail> ListPurchaseReqInProgress()
        {
            string filterExpression = "";

            filterExpression = "SELECT  *  FROM PurchaseRequestDetails INNER JOIN PurchaseRequests on dbo.PurchaseRequestDetails.PurchaseRequest_Id = PurchaseRequests.Id  Where PurchaseRequestDetails.BidAnalysisRequestStatus = 'InProgress'  order by PurchaseRequests.Id Desc ";

            return _workspace.SqlQuery<PurchaseRequestDetail>(filterExpression).ToList();

        }
        public IList<PurchaseRequestDetail> ListPurchaseReqCompleted()
        {
            string filterExpression = "";

            filterExpression = "SELECT  *  FROM PurchaseRequestDetails INNER JOIN PurchaseRequests on dbo.PurchaseRequestDetails.PurchaseRequest_Id = PurchaseRequests.Id  Where PurchaseRequestDetails.BidAnalysisRequestStatus = 'Completed'  order by PurchaseRequests.Id Desc ";

            return _workspace.SqlQuery<PurchaseRequestDetail>(filterExpression).ToList();

        }
        public IList<PurchaseRequestDetail> ListPurchaseReqInProgressById(int ReqId)
        {
            string filterExpression = "";

            filterExpression = "SELECT  *  FROM PurchaseRequestDetails INNER JOIN PurchaseRequests on PurchaseRequestDetails.PurchaseRequest_Id = PurchaseRequests.Id  Where  PurchaseRequests.Id = '" + ReqId + "' AND PurchaseRequestDetails.BidAnalysisRequestStatus='InProgress' order by PurchaseRequests.Id Desc ";

            return _workspace.SqlQuery<PurchaseRequestDetail>(filterExpression).ToList();

        }
        public IList<PurchaseRequestDetail> ListPRDetailsInProgressById(int ReqId)
        {
            string filterExpression = "";

            filterExpression = "SELECT  *  FROM PurchaseRequestDetails INNER JOIN PurchaseRequests ON PurchaseRequestDetails.PurchaseRequest_Id = PurchaseRequests.Id WHERE PurchaseRequestDetails.BidAnalysisRequestStatus = 'InProgress' AND PurchaseRequests.Id = '" + ReqId + "'  ORDER BY PurchaseRequests.Id DESC";

            return _workspace.SqlQuery<PurchaseRequestDetail>(filterExpression).ToList();

        }

        public IList<PurchaseRequestDetail> ListPRDetailsCompletedById(int ReqId)
        {
            string filterExpression = "";

            filterExpression = "SELECT  *  FROM PurchaseRequestDetails INNER JOIN PurchaseRequests ON PurchaseRequestDetails.PurchaseRequest_Id = PurchaseRequests.Id WHERE PurchaseRequestDetails.BidAnalysisRequestStatus = 'Completed' AND PurchaseRequests.Id = '" + ReqId + "'  ORDER BY PurchaseRequests.Id DESC";

            return _workspace.SqlQuery<PurchaseRequestDetail>(filterExpression).ToList();

        }
        
        public IList<PurchaseRequestDetail> ListPurchaseReqById(int Id)
        {
            string filterExpression = "";

            filterExpression = "SELECT  *  FROM PurchaseRequestDetails INNER JOIN PurchaseRequests on PurchaseRequestDetails.PurchaseRequest_Id = PurchaseRequests.Id  Where 1 = Case when '" + Id + "' = '' Then 1 When PurchaseRequestDetails.Id = '" + Id + "'  Then 1 END  order by PurchaseRequestDetails.Id Desc ";

            return _workspace.SqlQuery<PurchaseRequestDetail>(filterExpression).ToList();

        }
        public PurchaseRequest GetPurchaseRequest(int PurchaseRequestId)
        {
            return _workspace.Single<PurchaseRequest>(x => x.Id == PurchaseRequestId, x => x.PurchaseRequestDetails.Select(y => y.ItemAccount), x => x.PurchaseRequestDetails.Select(z => z.Project));
        }

        public PurchaseRequestDetail GetPurchaseRequestbyPuID(int Id)
        {
            return _workspace.Single<PurchaseRequestDetail>(x => x.Id == Id, y => y.PurchaseRequest);
        }
        public PurchaseRequestDetail GetPurchaseRequestDetail(int PurchaseRequestDetailId)
        {
            return _workspace.Single<PurchaseRequestDetail>(x => x.Id == PurchaseRequestDetailId);
        }
        public IList<PurchaseRequest> ListPurchaseRequests(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT  *  FROM PurchaseRequests Where 1 = Case when '" + RequestNo + "' = '' Then 1 When PurchaseRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When PurchaseRequests.RequestedDate = '" + RequestDate + "'  Then 1 END and PurchaseRequests.Requester='" + GetCurrentUser().Id + "' order by PurchaseRequests.Id Desc ";

            return _workspace.SqlQuery<PurchaseRequest>(filterExpression).ToList();

        }
        public IList<PurchaseRequest> ListPurchaseRequestForBids(string RequestNo, string RequestDate, string ProgressStatus)
        {
            string filterExpression = "";

            if (ProgressStatus != "Completed")
            {
                filterExpression = " SELECT  *  FROM PurchaseRequests INNER JOIN AppUsers on AppUsers.Id=PurchaseRequests.CurrentApprover  Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When PurchaseRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When PurchaseRequests.RequestedDate = '" + RequestDate + "'  Then 1 END AND PurchaseRequests.ProgressStatus='" + ProgressStatus + "' " +
                                       " AND  ((PurchaseRequests.CurrentApprover = '" + CurrentUser().Id + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by PurchaseRequests.Id DESC";
            }
            else
            {
                filterExpression = " SELECT  *  FROM PurchaseRequests INNER JOIN AppUsers on AppUsers.Id=PurchaseRequests.CurrentApprover INNER JOIN PurchaseRequestStatuses on PurchaseRequestStatuses.PurchaseRequest_Id = PurchaseRequests.Id Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When PurchaseRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When PurchaseRequests.RequestedDate = '" + RequestDate + "'  Then 1 END AND PurchaseRequests.ProgressStatus='" + ProgressStatus + "' AND " +
                                           "   (PurchaseRequestStatuses.ApprovalStatus Is not null AND (PurchaseRequestStatuses.Approver = '" + CurrentUser().Id + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by PurchaseRequests.Id DESC ";
            }
            return _workspace.SqlQuery<PurchaseRequest>(filterExpression).ToList();

        }
        public IList<MaintenanceRequest> GetMaintenanceRequestsCompleted()
        {
            return WorkspaceFactory.CreateReadOnly().Query<MaintenanceRequest>(x => x.ProgressStatus == "Completed").ToList();
        }
        //public IList<MaintenanceRequest> GetMaintenanceRequestsCompleted()
        //{
        //    string filterExpression = "";
        //    filterExpression = "SELECT DISTINCT RequestNo FROM MaintenanceRequests " +
        //                               " WHERE MaintenanceRequests.ProgressStatus = 'Completed' Group BY RequestNo";

        //    return _workspace.SqlQuery<MaintenanceRequest>(filterExpression).ToList();
        //}
        public int GetLastPurchaseRequestId()
        {
            if (_workspace.Last<PurchaseRequest>() != null)
            {
                return _workspace.Last<PurchaseRequest>().Id;
            }
            else
            { return 0; }
        }

        #endregion
        #region Sole Vendor Requests
        public IList<SoleVendorRequest> GetSoleVendorRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<SoleVendorRequest>(null).ToList();
        }
        public SoleVendorRequest GetSoleVendorRequest(int id)
        {
            return _workspace.Single<SoleVendorRequest>(x => x.Id == id);
        }
        public IList<SoleVendorRequest> ListSoleVendorRequests(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM SoleVendorRequests Where 1 = Case when '" + RequestNo + "' = '' Then 1 When SoleVendorRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When SoleVendorRequests.RequestDate = '" + RequestDate + "'  Then 1 END And SoleVendorRequests.AppUser_Id='" + GetCurrentUser().Id + "' order by SoleVendorRequests.Id Desc ";

            return _workspace.SqlQuery<SoleVendorRequest>(filterExpression).ToList();

        }
        public SoleVendorRequestDetail GetSoleVendorRequestDetail(int id)
        {
            return _workspace.Single<SoleVendorRequestDetail>(x => x.Id == id);
        }
        public int GetLastSoleVendorRequestId()
        {
            if (_workspace.Last<SoleVendorRequest>() != null)
            {
                return _workspace.Last<SoleVendorRequest>().Id;
            }
            else { return 0; }
        }

        #endregion
        #region Bid Analysis Requests
        public IList<BidAnalysisRequest> GetBidAnalysisRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<BidAnalysisRequest>(null).ToList();
        }
        public BidAnalysisRequest GetBidAnalysisRequest(int id)
        {
            return _workspace.Single<BidAnalysisRequest>(x => x.Id == id);
        }
        public IList<BidAnalysisRequest> ListBidAnalysisRequests(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM BidAnalysisRequests Where 1 = Case when '" + RequestNo + "' = '' Then 1 When BidAnalysisRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When BidAnalysisRequests.RequestDate = '" + RequestDate + "'  Then 1 END And BidAnalysisRequests.AppUser_Id='" + GetCurrentUser().Id + "' order by BidAnalysisRequests.Id Desc ";

            return _workspace.SqlQuery<BidAnalysisRequest>(filterExpression).ToList();

        }
        public BidderItemDetail GetBiderItem(int id)
        {
            return _workspace.Single<BidderItemDetail>(x => x.Id == id);
        }
        public int GetLastBidAnalysisRequestId()
        {
            if (_workspace.Last<BidAnalysisRequest>() != null)
            {
                return _workspace.Last<BidAnalysisRequest>().Id;
            }
            else { return 0; }
        }

        public Bidder GetBidder(int id)
        {
            return _workspace.Single<Bidder>(x => x.Id == id);
        }



        #endregion
        #region MaintenanceRequest

        public IList<MaintenanceRequest> GetMaintenanceRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<MaintenanceRequest>(null).ToList();

        }
       
        public IList<MaintenanceRequest> GetMaintenanceRequestsInProgress()
        {
            string filterExpression = "";
            filterExpression = "SELECT DISTINCT MaintenanceRequests.Id,RequestNo,Requester,RequestedDate,Requireddateofdelivery,TotalPrice,SpecialNeed,NeededFor, " +
                                      " DeliverTo,Comment,SuggestedSupplier,IsVehicle,PlateNo,CurrentApprover,CurrentLevel,ProgressStatus,CurrentStatus FROM " +
                                      " MaintenanceRequests INNER JOIN MaintenanceRequestDetails ON dbo.MaintenanceRequestDetails.MaintenanceRequest_Id = MaintenanceRequests.Id" +
                                       " WHERE MaintenanceRequestDetails.BidAnalysisRequestStatus = 'InProgress' AND MaintenanceRequests.ProgressStatus = 'Completed' ORDER BY MaintenanceRequests.Id DESC ";

            return _workspace.SqlQuery<MaintenanceRequest>(filterExpression).ToList();
        }
        public IList<MaintenanceRequestDetail> ListMaintenanceReqInProgress()
        {
            string filterExpression = "";

            filterExpression = "SELECT  *  FROM MaintenanceRequestDetails INNER JOIN MaintenanceRequests on dbo.MaintenanceRequestDetails.MaintenanceRequest_Id = MaintenanceRequests.Id  Where MaintenanceRequestDetails.BidAnalysisRequestStatus = 'InProgress'  order by MaintenanceRequests.Id Desc ";

            return _workspace.SqlQuery<MaintenanceRequestDetail>(filterExpression).ToList();

        }
        public IList<MaintenanceRequestDetail> ListMaintenanceReqInProgressById(int ReqId)
        {
            string filterExpression = "";

            filterExpression = "SELECT  *  FROM MaintenanceRequestDetails INNER JOIN MaintenanceRequests on MaintenanceRequestDetails.MaintenanceRequest_Id = MaintenanceRequests.Id  Where  MaintenanceRequests.Id = '" + ReqId + "' AND MaintenanceRequestDetails.BidAnalysisRequestStatus='InProgress' order by MaintenanceRequests.Id Desc ";

            return _workspace.SqlQuery<MaintenanceRequestDetail>(filterExpression).ToList();

        }
        public AppUser GetMechanic()
        {
            return _workspace.Single<AppUser>(x => x.EmployeePosition.PositionName == "Driver/Mechanic");
        }
        public IList<MaintenanceRequestDetail> ListMaintenanceReqById(int Id)
        {
            string filterExpression = "";

            filterExpression = "SELECT  *  FROM MaintenanceRequestDetails INNER JOIN MaintenanceRequests on MaintenanceRequestDetails.MaintenanceRequest_Id = MaintenanceRequests.Id  Where 1 = Case when '" + Id + "' = '' Then 1 When MaintenanceRequestDetails.Id = '" + Id + "'  Then 1 END  order by MaintenanceRequestDetails.Id Desc ";

            return _workspace.SqlQuery<MaintenanceRequestDetail>(filterExpression).ToList();

        }
        public MaintenanceRequest GetMaintenanceRequest(int MaintenanceRequestId)
        {
            return _workspace.Single<MaintenanceRequest>(x => x.Id == MaintenanceRequestId);
        }

        public MaintenanceRequestDetail GetMaintenanceRequestbyPuID(int Id)
        {
            return _workspace.Single<MaintenanceRequestDetail>(x => x.Id == Id, y => y.MaintenanceRequest);
        }
        public MaintenanceRequestDetail GetMaintenanceRequestDetail(int MaintenanceRequestDetailId)
        {
            return _workspace.Single<MaintenanceRequestDetail>(x => x.Id == MaintenanceRequestDetailId);
        }
        public MaintenanceSparePart GetMaintenanceSparePart(int MaintenanceSparePartId)
        {
            return _workspace.Single<MaintenanceSparePart>(x => x.Id == MaintenanceSparePartId);
        }
        public IList<MaintenanceRequest> ListMaintenanceRequests(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT  *  FROM MaintenanceRequests Where 1 = Case when '" + RequestNo + "' = '' Then 1 When MaintenanceRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When MaintenanceRequests.RequestDate = '" + RequestDate + "'  Then 1 END and MaintenanceRequests.Requester='" + GetCurrentUser().Id + "' order by MaintenanceRequests.Id Desc ";

            return _workspace.SqlQuery<MaintenanceRequest>(filterExpression).ToList();

        }
        public IList<MaintenanceRequest> ListMaintenanceRequestForBids(string RequestNo, string RequestDate, string ProgressStatus)
        {
            string filterExpression = "";

            if (ProgressStatus != "Completed")
            {
                filterExpression = " SELECT  *  FROM MaintenanceRequests INNER JOIN AppUsers on AppUsers.Id=MaintenanceRequests.CurrentApprover  Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When MaintenanceRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When MaintenanceRequests.RequestedDate = '" + RequestDate + "'  Then 1 END AND MaintenanceRequests.ProgressStatus='" + ProgressStatus + "' " +
                                       " AND  ((MaintenanceRequests.CurrentApprover = '" + CurrentUser().Id + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by MaintenanceRequests.Id DESC";
            }
            else
            {
                filterExpression = " SELECT  *  FROM MaintenanceRequests INNER JOIN AppUsers on AppUsers.Id=MaintenanceRequests.CurrentApprover INNER JOIN MaintenanceRequestStatuses on MaintenanceRequestStatuses.MaintenanceRequest_Id = MaintenanceRequests.Id Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When MaintenanceRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When MaintenanceRequests.RequestedDate = '" + RequestDate + "'  Then 1 END AND MaintenanceRequests.ProgressStatus='" + ProgressStatus + "' AND " +
                                           "   (MaintenanceRequestStatuses.ApprovalStatus Is not null AND (MaintenanceRequestStatuses.Approver = '" + CurrentUser().Id + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by MaintenanceRequests.Id DESC ";
            }
            return _workspace.SqlQuery<MaintenanceRequest>(filterExpression).ToList();

        }
        public int GetLastMaintenanceRequestId()
        {
            if (_workspace.Last<MaintenanceRequest>() != null)
            {
                return _workspace.Last<MaintenanceRequest>().Id;
            }
            else
            { return 0; }
        }



        #endregion
        #region StoreRequest

        public IList<StoreRequest> GetStoreRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<StoreRequest>(null).ToList();

        }

        public IList<StoreRequest> GetStoreRequestsInProgress()
        {
            string filterExpression = "";
            filterExpression = "SELECT DISTINCT StoreRequests.Id,RequestNo,Requester,RequestedDate,TotalPrice, " +
                                      " DeliverTo,Comment,CurrentApprover,CurrentLevel,ProgressStatus,CurrentStatus FROM " +
                                      " StoreRequests INNER JOIN StoreRequestDetails ON dbo.StoreRequestDetails.StoreRequest_Id = StoreRequests.Id" +
                                       " WHERE  StoreRequests.ProgressStatus = 'Completed' ORDER BY StoreRequests.Id DESC ";

            return _workspace.SqlQuery<StoreRequest>(filterExpression).ToList();
        }
      
      
        public StoreRequest GetStoreRequest(int StoreRequestId)
        {
            return _workspace.Single<StoreRequest>(x => x.Id == StoreRequestId, x => x.StoreRequestDetails.Select(z => z.Project));
        }
        public StoreRequest GetStoreRequestByPurchaseId(int purchaseId)
        {
            return _workspace.First<StoreRequest>(x => x.purchaseId == purchaseId);
        }
        public StoreRequestDetail GetStoreRequestbyPuID(int Id)
        {
            return _workspace.Single<StoreRequestDetail>(x => x.Id == Id, y => y.StoreRequest);
        }
        public StoreRequestDetail GetStoreRequestDetail(int StoreRequestDetailId)
        {
            return _workspace.Single<StoreRequestDetail>(x => x.Id == StoreRequestDetailId);
        }
        public IList<StoreRequest> ListStoreRequests(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT  *  FROM StoreRequests Where 1 = Case when '" + RequestNo + "' = '' Then 1 When StoreRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When StoreRequests.RequestedDate = '" + RequestDate + "'  Then 1 END and StoreRequests.Requester='" + GetCurrentUser().Id + "' order by StoreRequests.Id Desc ";

            return _workspace.SqlQuery<StoreRequest>(filterExpression).ToList();

        }
        public IList<StoreRequest> ListStoreRequestForBids(string RequestNo, string RequestDate, string ProgressStatus)
        {
            string filterExpression = "";

            if (ProgressStatus != "Completed")
            {
                filterExpression = " SELECT  *  FROM StoreRequests INNER JOIN AppUsers on AppUsers.Id=StoreRequests.CurrentApprover  Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When StoreRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When StoreRequests.RequestedDate = '" + RequestDate + "'  Then 1 END AND StoreRequests.ProgressStatus='" + ProgressStatus + "' " +
                                       " AND  ((StoreRequests.CurrentApprover = '" + CurrentUser().Id + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by StoreRequests.Id DESC";
            }
            else
            {
                filterExpression = " SELECT  *  FROM StoreRequests INNER JOIN AppUsers on AppUsers.Id=StoreRequests.CurrentApprover INNER JOIN StoreRequestStatuses on StoreRequestStatuses.StoreRequest_Id = StoreRequests.Id Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When StoreRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When StoreRequests.RequestedDate = '" + RequestDate + "'  Then 1 END AND StoreRequests.ProgressStatus='" + ProgressStatus + "' AND " +
                                           "   (StoreRequestStatuses.ApprovalStatus Is not null AND (StoreRequestStatuses.Approver = '" + CurrentUser().Id + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by StoreRequests.Id DESC ";
            }
            return _workspace.SqlQuery<StoreRequest>(filterExpression).ToList();

        }
        public int GetLastStoreRequestId()
        {
            if (_workspace.Last<StoreRequest>() != null)
            {
                return _workspace.Last<StoreRequest>().Id;
            }
            else
            { return 0; }
        }

        #endregion
        #region Employee
        public Employee GetEmployee(int empid)
        {
            return _workspace.Single<Employee>(x => x.Id == empid);
        }
        #endregion
        #region Entity Manipulation
        public void SaveOrUpdateEntity<T>(T item) where T : class
        {
          
            IEntity entity = (IEntity)item;
            if (entity.Id == 0)
                _workspace.Add<T>(item);
            else
                _workspace.Update<T>(item);
            try { 
            _workspace.CommitChanges();
        }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
        // raise a new exception nesting  
        // the current instance as InnerException  
        raise = new InvalidOperationException(message, raise);
    }
}
                throw raise;
            }
            _workspace.Refresh(item);
           
        }
        public void DeleteEntity<T>(T item) where T : class
        {
            _workspace.Delete<T>(item);
            _workspace.CommitChanges();
            _workspace.Refresh(item);
        }

        public void Commit()
        {
            _workspace.CommitChanges();
        }
        #endregion               
    }
}
