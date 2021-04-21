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
using Chai.WorkflowManagment.CoreDomain.Request;
using Chai.WorkflowManagment.CoreDomain.Approval;
using Chai.WorkflowManagment.CoreDomain.HRM;
using System.Data.Entity.Validation;

namespace Chai.WorkflowManagment.Modules.Approval
{
    public class ApprovalController : ControllerBase
    {
        private IWorkspace _workspace;

        [InjectionConstructor]
        public ApprovalController([ServiceDependency] IHttpContextLocatorService httpContextLocatorService, [ServiceDependency] INavigationService navigationService)
            : base(httpContextLocatorService, navigationService)
        {
            _workspace = ZadsServices.Workspace;
        }
        public AppUser CurrentUser()
        {
            return GetCurrentUser();
        }
        public AppUser Approver(int position)
        {
            return _workspace.Single<AppUser>(x => x.EmployeePosition.Id == position);
        }
        public AppUser GetSuperviser(int superviser)
        {
            return _workspace.Single<AppUser>(x => x.Id == superviser);
        }
        #region CurrentObject
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
        #region Vehicle Approval
        public IList<VehicleRequest> ListVehicleRequests(string RequestNo, string RequestDate, string ProgressStatus, string Requester)
        {
            string filterExpression = "";

            filterExpression = " SELECT  *  FROM VehicleRequests INNER JOIN AppUsers on AppUsers.Id=VehicleRequests.CurrentApprover Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id  AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When VehicleRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When VehicleRequests.RequestDate = '" + RequestDate + "'  Then 1 END AND VehicleRequests.ProgressStatus='" + ProgressStatus + "' " +
                                   " AND  1 = CASE WHEN '" + Requester + "' = '0' THEN 1 WHEN VehicleRequests.AppUser_Id = '" + Requester + "'  THEN 1 END " +
                                   " AND  ((VehicleRequests.CurrentApprover = '" + CurrentUser().Id + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by VehicleRequests.Id Desc";

            return _workspace.SqlQuery<VehicleRequest>(filterExpression).ToList();
        }
        #endregion
        #region Travel Log Attach
        public IList<VehicleRequest> ListPendingVehicleRequests(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = " SELECT * FROM VehicleRequests Where 1 = Case when '" + RequestNo + "' = '' Then 1 When VehicleRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When VehicleRequests.RequestDate = '" + RequestDate + "'  Then 1 END AND VehicleRequests.ProgressStatus='Completed' AND VehicleRequests.TravelLogStatus='Pending' AND VehicleRequests.IsExtension='FALSE' " +
                                   " ORDER BY VehicleRequests.Id Desc";

            return _workspace.SqlQuery<VehicleRequest>(filterExpression).ToList();
        }
        public VehicleRequest GetExtVehicleRequest(int requestId)
        {
            return _workspace.Single<VehicleRequest>(x => x.ExtRefRequest_Id == requestId && x.IsExtension == true);
        }
        #endregion
        #region Cash Payment Approval
        public IList<CashPaymentRequest> ListCashPaymentRequests(string RequestNo, string RequestDate, string ProgressStatus, string Requester)
        {
            string filterExpression = "";
            if (ProgressStatus == "InProgress")
            {
                filterExpression = " SELECT * FROM CashPaymentRequests " +
                                   " INNER JOIN AppUsers ON (AppUsers.Id = CashPaymentRequests.CurrentApprover) OR (AppUsers.EmployeePosition_Id = CashPaymentRequests.CurrentApproverPosition AND AppUsers.Id = '" + CurrentUser().Id + "') " +
                                   " LEFT JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 " +
                                   " WHERE 1 = Case when '" + RequestNo + "' = '' Then 1 When CashPaymentRequests.VoucherNo = '" + RequestNo + "'  Then 1 END " +
                                   " AND 1 = CASE WHEN '" + RequestDate + "' = '' Then 1 When CashPaymentRequests.RequestDate = '" + RequestDate + "'  Then 1 END " +
                                   " AND 1 = CASE WHEN '" + Requester + "' = '0' THEN 1 WHEN CashPaymentRequests.AppUser_Id = '" + Requester + "'  THEN 1 END " +
                                   " AND CashPaymentRequests.ProgressStatus='" + ProgressStatus + "' " +
                                   " AND ((CashPaymentRequests.CurrentApprover = '" + CurrentUser().Id + "') OR (CashPaymentRequests.CurrentApproverPosition = '" + CurrentUser().EmployeePosition.Id + "') OR (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) " +
                                   " ORDER BY CashPaymentRequests.Id DESC";
            }
            else if (ProgressStatus == "Reviewed")
            {
                filterExpression = " SELECT * FROM CashPaymentRequests " +
                                   " INNER JOIN AppUsers ON (AppUsers.Id = CashPaymentRequests.CurrentApprover) OR (AppUsers.EmployeePosition_Id = CashPaymentRequests.CurrentApproverPosition AND AppUsers.Id = '" + CurrentUser().Id + "') " +
                                   " LEFT JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 " +
                                   " WHERE 1 = CASE WHEN '" + RequestNo + "' = '' Then 1 When CashPaymentRequests.VoucherNo = '" + RequestNo + "'  Then 1 END " +
                                   " AND 1 = CASE WHEN '" + RequestDate + "' = '' Then 1 When CashPaymentRequests.RequestDate = '" + RequestDate + "'  Then 1 END " +
                                   " AND 1 = CASE WHEN '" + Requester + "' = '0' THEN 1 WHEN CashPaymentRequests.AppUser_Id = '" + Requester + "'  THEN 1 END " +
                                   " AND CashPaymentRequests.CurrentLevel > 1 " +
                                   " ORDER BY CashPaymentRequests.Id DESC";
            }
            else if (ProgressStatus == "Not Retired" || ProgressStatus == "Retired")
            {
                filterExpression = " SELECT * FROM CashPaymentRequests " +
                                   " INNER JOIN AppUsers ON  (AppUsers.Id = CashPaymentRequests.CurrentApprover) OR (AppUsers.EmployeePosition_Id = CashPaymentRequests.CurrentApproverPosition AND AppUsers.Id = '" + CurrentUser().Id + "') " +
                                   " LEFT JOIN AssignJobs ON AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When CashPaymentRequests.VoucherNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When CashPaymentRequests.RequestDate = '" + RequestDate + "'  Then 1 END " +
                                   " AND CashPaymentRequests.ProgressStatus='Completed' AND CashPaymentRequests.PaymentReimbursementStatus = '" + ProgressStatus + "' " +
                                   " AND (CashPaymentRequests.CurrentApprover = '" + CurrentUser().Id + "') OR (CashPaymentRequests.CurrentApproverPosition = '" + CurrentUser().EmployeePosition.Id + "') OR (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "') " +
                                   " ORDER BY CashPaymentRequests.Id DESC ";
            }
            else if (ProgressStatus == "Completed")
            {
                filterExpression = " SELECT DISTINCT(CashPaymentRequests.RequestNo), CashPaymentRequests.* FROM CashPaymentRequests " +
                                   " INNER JOIN CashPaymentRequestStatuses ON CashPaymentRequestStatuses.CashPaymentRequest_Id = CashPaymentRequests.Id " +
                                   " INNER JOIN AppUsers ON AppUsers.Id = CashPaymentRequestStatuses.Approver" +
                                   " LEFT JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 " +
                                   " WHERE 1 = CASE WHEN '" + RequestNo + "' = '' Then 1 When CashPaymentRequests.VoucherNo = '" + RequestNo + "'  Then 1 END " +
                                   " AND  1 = CASE WHEN '" + RequestDate + "' = '' Then 1 When CashPaymentRequests.RequestDate = '" + RequestDate + "'  Then 1 END " +
                                   " AND 1 = CASE WHEN '" + Requester + "' = '0' THEN 1 WHEN CashPaymentRequests.AppUser_Id = '" + Requester + "'  THEN 1 END " +
                                   " AND CashPaymentRequests.ProgressStatus='" + ProgressStatus + "' " +
                                   " AND CashPaymentRequestStatuses.ApprovalStatus IS NOT NULL AND ((CashPaymentRequestStatuses.Approver = '" + CurrentUser().Id + "') OR (CashPaymentRequests.CurrentApproverPosition = '" + CurrentUser().EmployeePosition.Id + "') OR (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) " +
                                   " AND (CashPaymentRequests.CurrentStatus != 'Rejected' OR CashPaymentRequests.CurrentStatus IS NULL) " +
                                   " ORDER BY CashPaymentRequests.Id DESC";

            }
            return _workspace.SqlQuery<CashPaymentRequest>(filterExpression).ToList();
        }
        #endregion
        #region Cost Sharing Approval
        public IList<CostSharingRequest> ListCostSharingRequests(string RequestNo, string RequestDate, string ProgressStatus)
        {
            string filterExpression = "";
            if (ProgressStatus == "InProgress")
            {
                filterExpression = " SELECT * FROM CostSharingRequests INNER JOIN AppUsers ON (AppUsers.Id = CostSharingRequests.CurrentApprover) OR (AppUsers.EmployeePosition_Id = CostSharingRequests.CurrentApproverPosition AND AppUsers.Id = '" + CurrentUser().Id + "') Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When CostSharingRequests.VoucherNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When CostSharingRequests.RequestDate = '" + RequestDate + "'  Then 1 END AND CostSharingRequests.ProgressStatus='" + ProgressStatus + "' " +
                                       " AND  ((CostSharingRequests.CurrentApprover = '" + CurrentUser().Id + "') or (CostSharingRequests.CurrentApproverPosition = '" + CurrentUser().EmployeePosition.Id + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by CostSharingRequests.Id DESC ";
            }
            else if (ProgressStatus == "Not Retired" || ProgressStatus == "Retired")

                filterExpression = " SELECT * FROM CostSharingRequests INNER JOIN AppUsers ON (AppUsers.Id = CostSharingRequests.CurrentApprover) OR (AppUsers.EmployeePosition_Id = CostSharingRequests.CurrentApproverPosition AND AppUsers.Id = '" + CurrentUser().Id + "') Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When CostSharingRequests.VoucherNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When CostSharingRequests.RequestDate = '" + RequestDate + "'  Then 1 END AND CostSharingRequests.ProgressStatus='Completed' AND CostSharingRequests.PaymentReimbursementStatus = '" + ProgressStatus + "'" +
                                       " AND  (CostSharingRequests.CurrentApprover = '" + CurrentUser().Id + "') or (CostSharingRequests.CurrentApproverPosition = '" + CurrentUser().EmployeePosition.Id + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "') order by CostSharingRequests.Id DESC ";
            else if (ProgressStatus == "Completed")
            {
                filterExpression = " SELECT * FROM CostSharingRequests INNER JOIN AppUsers ON (AppUsers.Id = CostSharingRequests.CurrentApprover) OR (AppUsers.EmployeePosition_Id = CostSharingRequests.CurrentApproverPosition AND AppUsers.Id = '" + CurrentUser().Id + "') INNER JOIN CostSharingRequestStatuses on CostSharingRequestStatuses.CostSharingRequest_Id = CostSharingRequests.Id  Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When CostSharingRequests.VoucherNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When CostSharingRequests.RequestDate = '" + RequestDate + "'  Then 1 END AND CostSharingRequests.ProgressStatus='" + ProgressStatus + "' " +
                                           " AND  (CostSharingRequestStatuses.ApprovalStatus Is not null  AND (CostSharingRequestStatuses.Approver = '" + CurrentUser().Id + "') or (CostSharingRequests.CurrentApproverPosition = '" + CurrentUser().EmployeePosition.Id + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by CostSharingRequests.Id DESC";
            }
            return _workspace.SqlQuery<CostSharingRequest>(filterExpression).ToList();
        }
        #endregion
        #region Bank Payment Approval
        public IList<BankPaymentRequest> ListBankPaymentRequests(string RequestNo, string RequestDate, string ProgressStatus)
        {
            string filterExpression = "";

            filterExpression = " SELECT * FROM BankPaymentRequests INNER JOIN AppUsers on AppUsers.Id = BankPaymentRequests.CurrentApprover Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When BankPaymentRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When BankPaymentRequests.ProcessDate = '" + RequestDate + "'  Then 1 END AND BankPaymentRequests.ProgressStatus='" + ProgressStatus + "' " +
                                   " AND  ((BankPaymentRequests.CurrentApprover = '" + CurrentUser().Id + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by BankPaymentRequests.Id DESC";

            return _workspace.SqlQuery<BankPaymentRequest>(filterExpression).ToList();
        }
        #endregion
        #region Operational Control Approval
        public IList<OperationalControlRequest> ListOperationalControlRequests(string RequestNo, string RequestDate, string ProgressStatus, string Requester)
        {
            string filterExpression = "";
            if (ProgressStatus == "InProgress")
            {
                filterExpression = " SELECT * FROM OperationalControlRequests " +
                                   " INNER JOIN AppUsers ON (AppUsers.Id = OperationalControlRequests.CurrentApprover) OR (AppUsers.EmployeePosition_Id = OperationalControlRequests.CurrentApproverPosition AND AppUsers.Id = '" + CurrentUser().Id + "') " +
                                   " LEFT JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 " +
                                   " WHERE 1 = CASE WHEN '" + RequestNo + "' = '' Then 1 When OperationalControlRequests.VoucherNo = '" + RequestNo + "'  Then 1 END " +
                                   " AND 1 = CASE WHEN '" + RequestDate + "' = '' Then 1 When OperationalControlRequests.RequestDate = '" + RequestDate + "'  THEN 1 END " +
                                   " AND 1 = CASE WHEN '" + Requester + "' = '0' THEN 1 WHEN OperationalControlRequests.Supplier_Id = '" + Requester + "'  THEN 1 END " +
                                   " AND OperationalControlRequests.ProgressStatus='" + ProgressStatus + "' " +
                                   " AND ((OperationalControlRequests.CurrentApprover = '" + CurrentUser().Id + "') OR (OperationalControlRequests.CurrentApproverPosition = '" + CurrentUser().EmployeePosition.Id + "') OR (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) ORDER BY OperationalControlRequests.Id DESC";

            }
            else if (ProgressStatus == "Reviewed")
            {
                filterExpression = " SELECT * FROM OperationalControlRequests " +
                                   " INNER JOIN AppUsers ON (AppUsers.Id = OperationalControlRequests.CurrentApprover) OR (AppUsers.EmployeePosition_Id = OperationalControlRequests.CurrentApproverPosition AND AppUsers.Id = '" + CurrentUser().Id + "') " +
                                   " LEFT JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 " +
                                   " WHERE 1 = CASE WHEN '" + RequestNo + "' = '' Then 1 When OperationalControlRequests.VoucherNo = '" + RequestNo + "'  Then 1 END " +
                                   " AND 1 = CASE WHEN '" + RequestDate + "' = '' Then 1 When OperationalControlRequests.RequestDate = '" + RequestDate + "'  THEN 1 END " +
                                   " AND 1 = CASE WHEN '" + Requester + "' = '0' THEN 1 WHEN OperationalControlRequests.Supplier_Id = '" + Requester + "'  THEN 1 END " +
                                   " AND OperationalControlRequests.CurrentLevel > 1 " +
                                   " ORDER BY OperationalControlRequests.Id DESC";

            }
            else if (ProgressStatus == "Not Retired" || ProgressStatus == "Retired")
            {
                filterExpression = " SELECT * FROM OperationalControlRequests " +
                                   " INNER JOIN AppUsers ON  (AppUsers.Id = OperationalControlRequests.CurrentApprover) OR (AppUsers.EmployeePosition_Id = OperationalControlRequests.CurrentApproverPosition AND AppUsers.Id = '" + CurrentUser().Id + "') " +
                                   " LEFT JOIN AssignJobs ON AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 " +
                                   " WHERE 1 = CASE WHEN '" + RequestNo + "' = '' THEN 1 WHEN OperationalControlRequests.VoucherNo = '" + RequestNo + "'  Then 1 END " +
                                   " AND  1 = CASE WHEN '" + RequestDate + "' = '' Then 1 When OperationalControlRequests.RequestDate = '" + RequestDate + "'  Then 1 END " +
                                   " AND 1 = CASE WHEN '" + Requester + "' = '0' THEN 1 WHEN OperationalControlRequests.Supplier_Id = '" + Requester + "'  THEN 1 END " +
                                   " AND OperationalControlRequests.ProgressStatus='Completed' AND OperationalControlRequests.PaymentReimbursementStatus = '" + ProgressStatus + "' " +
                                   " AND (OperationalControlRequests.CurrentApprover = '" + CurrentUser().Id + "') OR (OperationalControlRequests.CurrentApproverPosition = '" + CurrentUser().EmployeePosition.Id + "') OR (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "') " +
                                   " ORDER BY OperationalControlRequests.Id DESC ";
            }
            else if (ProgressStatus == "Completed")
            {
                filterExpression = " SELECT DISTINCT(OperationalControlRequests.VoucherNo), OperationalControlRequests.* FROM OperationalControlRequests " +
                                   " INNER JOIN OperationalControlRequestStatuses ON OperationalControlRequestStatuses.OperationalControlRequest_Id = OperationalControlRequests.Id " +
                                   " INNER JOIN AppUsers ON AppUsers.Id = OperationalControlRequestStatuses.Approver" +
                                   " LEFT JOIN AssignJobs ON AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 " +
                                   " WHERE 1 = CASE WHEN '" + RequestNo + "' = '' THEN 1 WHEN OperationalControlRequests.VoucherNo = '" + RequestNo + "' THEN 1 END " +
                                   " AND 1 = CASE WHEN '" + RequestDate + "' = '' THEN 1 WHEN OperationalControlRequests.RequestDate = '" + RequestDate + "' THEN 1 END " +
                                   " AND 1 = CASE WHEN '" + Requester + "' = '0' THEN 1 WHEN OperationalControlRequests.Supplier_Id = '" + Requester + "'  THEN 1 END " +
                                   " AND OperationalControlRequests.ProgressStatus='" + ProgressStatus + "' " +
                                   " AND OperationalControlRequestStatuses.ApprovalStatus IS NOT NULL AND ((OperationalControlRequestStatuses.Approver = '" + CurrentUser().Id + "') OR (OperationalControlRequests.CurrentApproverPosition = '" + CurrentUser().EmployeePosition.Id + "') OR (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) " +
                                   " AND (OperationalControlRequests.CurrentStatus != 'Rejected' OR OperationalControlRequests.CurrentStatus IS NULL)" +
                                   " ORDER BY OperationalControlRequests.Id DESC ";
            }
            return _workspace.SqlQuery<OperationalControlRequest>(filterExpression).ToList();
        }
        #endregion
        #region Payment Reimbursement Approval
        public IList<PaymentReimbursementRequest> ListPaymentReimbursementRequests(string RequestDate, string ProgressStatus, string Requester)
        {
            string filterExpression = "";

            filterExpression = " SELECT * FROM PaymentReimbursementRequests " +
                               " INNER JOIN CashPaymentRequests ON CashPaymentRequests.Id = PaymentReimbursementRequests.Id " +
                               " LEFT JOIN AppUsers ON AppUsers.Id = PaymentReimbursementRequests.CurrentApprover " +
                               " LEFT JOIN AssignJobs ON AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 " +
                               " WHERE 1 = CASE WHEN '" + RequestDate + "' = '' THEN 1 WHEN PaymentReimbursementRequests.RequestDate = '" + RequestDate + "'  THEN 1 END " +
                               " AND 1 = CASE WHEN '" + Requester + "' = '0' THEN 1 WHEN CashPaymentRequests.AppUser_Id = '" + Requester + "'  THEN 1 END " +
                               " AND PaymentReimbursementRequests.ProgressStatus='" + ProgressStatus + "' " +
                               " AND ((PaymentReimbursementRequests.CurrentApprover = '" + CurrentUser().Id + "') " +
                               " OR (PaymentReimbursementRequests.CurrentApproverPosition = '" + CurrentUser().EmployeePosition.Id + "') " +
                               " OR (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) " +
                               " ORDER BY PaymentReimbursementRequests.Id DESC";

            return _workspace.SqlQuery<PaymentReimbursementRequest>(filterExpression).ToList();
        }
        #endregion
        #region Travel Advance Approval
        public IList<TravelAdvanceRequest> ListTravelAdvanceRequests(string RequestNo, string RequestDate, string ProgressStatus, string Requester)
        {
            string filterExpression = "";
            if (ProgressStatus.Equals("InProgress"))
            {

                filterExpression = " SELECT * FROM TravelAdvanceRequests " +
                                   " INNER JOIN AppUsers ON (AppUsers.Id = TravelAdvanceRequests.CurrentApprover) OR (AppUsers.EmployeePosition_Id = TravelAdvanceRequests.CurrentApproverPosition AND AppUsers.Id = '" + CurrentUser().Id + "') " +
                                   " LEFT JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 " +
                                   " WHERE 1 = CASE WHEN '" + RequestNo + "' = '' Then 1 When TravelAdvanceRequests.TravelAdvanceNo = '" + RequestNo + "'  Then 1 END " +
                                   " AND 1 = CASE WHEN '" + RequestDate + "' = '' Then 1 When TravelAdvanceRequests.RequestDate = '" + RequestDate + "'  Then 1 END " +
                                   " AND 1 = CASE WHEN '" + Requester + "' = '0' THEN 1 WHEN TravelAdvanceRequests.AppUser_Id = '" + Requester + "'  THEN 1 END " +
                                   " AND TravelAdvanceRequests.ProgressStatus='" + ProgressStatus + "' " +
                                   " AND ((TravelAdvanceRequests.CurrentApprover = '" + CurrentUser().Id + "') OR (TravelAdvanceRequests.CurrentApproverPosition = '" + CurrentUser().EmployeePosition.Id + "') OR (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) " +
                                   " ORDER BY TravelAdvanceRequests.Id DESC";
            }
            else if (ProgressStatus.Equals("Reviewed"))
            {
                filterExpression = " SELECT * FROM TravelAdvanceRequests " +
                                   " INNER JOIN AppUsers ON (AppUsers.Id = TravelAdvanceRequests.CurrentApprover) OR (AppUsers.EmployeePosition_Id = TravelAdvanceRequests.CurrentApproverPosition AND AppUsers.Id = '" + CurrentUser().Id + "') " +
                                   " LEFT JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 " +
                                   " WHERE 1 = CASE WHEN '" + RequestNo + "' = '' Then 1 When TravelAdvanceRequests.TravelAdvanceNo = '" + RequestNo + "'  Then 1 END " +
                                   " AND 1 = CASE WHEN '" + RequestDate + "' = '' Then 1 When TravelAdvanceRequests.RequestDate = '" + RequestDate + "'  Then 1 END " +
                                   " AND 1 = CASE WHEN '" + Requester + "' = '0' THEN 1 WHEN TravelAdvanceRequests.AppUser_Id = '" + Requester + "'  THEN 1 END " +
                                   " AND TravelAdvanceRequests.CurrentLevel > 1" +
                                   " ORDER BY TravelAdvanceRequests.Id DESC";
            }
            else if (ProgressStatus.Equals("Completed"))
            {
                filterExpression = " SELECT DISTINCT(TravelAdvanceRequests.TravelAdvanceNo), TravelAdvanceRequests.* FROM TravelAdvanceRequests " +
                                   " INNER JOIN TravelAdvanceRequestStatuses ON TravelAdvanceRequestStatuses.TravelAdvanceRequest_Id = TravelAdvanceRequests.Id " +
                                   " INNER JOIN AppUsers ON AppUsers.Id = TravelAdvanceRequestStatuses.Approver" +
                                   " LEFT JOIN AssignJobs ON AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 " +
                                   " WHERE 1 = CASE WHEN '" + RequestNo + "' = '' THEN 1 WHEN TravelAdvanceRequests.TravelAdvanceNo = '" + RequestNo + "'  Then 1 END " +
                                   " AND 1 = CASE WHEN '" + RequestDate + "' = '' THEN 1 WHEN TravelAdvanceRequests.RequestDate = '" + RequestDate + "'  Then 1 END " +
                                   " AND 1 = CASE WHEN '" + Requester + "' = '0' THEN 1 WHEN TravelAdvanceRequests.AppUser_Id = '" + Requester + "'  THEN 1 END " +
                                   " AND TravelAdvanceRequests.ProgressStatus='" + ProgressStatus + "'" +
                                   " AND (TravelAdvanceRequests.CurrentStatus != 'Rejected' OR TravelAdvanceRequests.CurrentStatus IS NULL)" +
                                   " AND TravelAdvanceRequestStatuses.ApprovalStatus IS NOT NULL " +
                                   " AND ((TravelAdvanceRequestStatuses.Approver = '" + CurrentUser().Id + "') OR (TravelAdvanceRequests.CurrentApproverPosition = '" + CurrentUser().EmployeePosition.Id + "') OR (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) " +
                                   " ORDER BY TravelAdvanceRequests.Id DESC ";

            }
            return _workspace.SqlQuery<TravelAdvanceRequest>(filterExpression).ToList();
        }
        #endregion
        #region Expense Liquidation Approval
        public IList<ExpenseLiquidationRequest> ListExpenseLiquidationRequests(string ExpenseType, string RequestDate, string ProgressStatus, string Requester)
        {
            string filterExpression = "";

            filterExpression = " SELECT * FROM ExpenseLiquidationRequests " +
                                   " INNER JOIN TravelAdvanceRequests ON TravelAdvanceRequests.Id = ExpenseLiquidationRequests.Id " +
                                   " INNER JOIN AppUsers ON (AppUsers.Id = ExpenseLiquidationRequests.CurrentApprover) OR (AppUsers.EmployeePosition_Id = ExpenseLiquidationRequests.CurrentApproverPosition AND AppUsers.Id = '" + CurrentUser().Id + "') " +
                                   " LEFT JOIN AssignJobs ON AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 " +
                                   " WHERE 1 = CASE WHEN '" + ExpenseType + "' = '' THEN 1 WHEN ExpenseLiquidationRequests.ExpenseType = '" + ExpenseType + "' THEN 1 END " +
                                   " AND 1 = CASE WHEN '" + RequestDate + "' = '' THEN 1 WHEN ExpenseLiquidationRequests.RequestDate = '" + RequestDate + "' THEN 1 END " +
                                   " AND 1 = CASE WHEN '" + Requester + "' = '0' THEN 1 WHEN TravelAdvanceRequests.AppUser_Id = '" + Requester + "'  THEN 1 END " +
                                   " AND ExpenseLiquidationRequests.ProgressStatus='" + ProgressStatus + "' " +
                                   " AND ((ExpenseLiquidationRequests.CurrentApprover = '" + CurrentUser().Id + "') OR (ExpenseLiquidationRequests.CurrentApproverPosition = '" + CurrentUser().EmployeePosition.Id + "') OR (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) " +
                                   " ORDER BY ExpenseLiquidationRequests.Id DESC ";

            return _workspace.SqlQuery<ExpenseLiquidationRequest>(filterExpression).ToList();
        }
        public DataSet ExportTravelAdvance(int LiquidationId)
        {
            ReportDao re = new ReportDao();
            return re.ExportLiquidationReport(LiquidationId);
        }
        #endregion
        #region LeaveApproval
        public LeaveRequest GetLeaveRequest(int LeaveRequestId)
        {
            return _workspace.Single<LeaveRequest>(x => x.Id == LeaveRequestId);
        }
        public IList<LeaveRequest> ListLeaveRequests(string RequestNo, string RequestDate, string ProgressStatus)
        {
            string filterExpression = "";
            if (ProgressStatus != "Completed")
            {
                filterExpression = " SELECT  *  FROM LeaveRequests INNER JOIN AppUsers on AppUsers.Id=LeaveRequests.CurrentApprover  Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When LeaveRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When LeaveRequests.RequestedDate = '" + RequestDate + "'  Then 1 END AND LeaveRequests.ProgressStatus='" + ProgressStatus + "' " +
                                       " AND  ((LeaveRequests.CurrentApprover = '" + CurrentUser().Id + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by LeaveRequests.Id DESC ";
            }
            else
            {
                filterExpression = " SELECT  *  FROM LeaveRequests INNER JOIN AppUsers on AppUsers.Id=LeaveRequests.CurrentApprover INNER JOIN LeaveRequestStatuses on LeaveRequestStatuses.LeaveRequest_Id = LeaveRequests.Id Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When LeaveRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When LeaveRequests.RequestedDate = '" + RequestDate + "'  Then 1 END AND LeaveRequests.ProgressStatus='" + ProgressStatus + "'  " +
                                          " AND  ( LeaveRequestStatuses.ApprovalStatus Is not null AND (LeaveRequestStatuses.Approver = '" + CurrentUser().Id + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by LeaveRequests.Id DESC ";
            }

            return _workspace.SqlQuery<LeaveRequest>(filterExpression).ToList();
        }
        #endregion
        #region AssignJob
        public int GetAssignedUserbycurrentuser()
        {
            int userId = GetCurrentUser().Id;
            IList<AssignJob> AJ = _workspace.All<AssignJob>(x => x.AssignedTo == userId && x.Status == true).ToList();
            if (AJ.Count != 0)
            { return AJ[0].AssignedTo; }
            else
                return 0;
        }
        public int GetAssignedUserbycurrentuser(int userId)
        {
            IList<AssignJob> AJ = _workspace.All<AssignJob>(x => x.AssignedTo == userId && x.Status == true).ToList();
            if (AJ.Count != 0)
            { return AJ[0].AssignedTo; }
            else
                return 0;
        }
        public AssignJob GetAssignedJobbycurrentuser()
        {
            int userId = GetCurrentUser().Id;
            IList<AssignJob> AJ = _workspace.All<AssignJob>(x => x.AssignedTo == userId && x.Status == true).ToList();
            if (AJ.Count != 0)
            {
                return AJ[0];
            }
            else
            { return null; }

        }
        public AssignJob GetAssignedJobbycurrentuser(int UserId)
        {
            // int userId = GetCurrentUser().Id;
            IList<AssignJob> AJ = _workspace.All<AssignJob>(x => x.AppUser.Id == UserId && x.Status == true).ToList();
            if (AJ.Count != 0)
            {
                return AJ[0];
            }
            else
            { return null; }
        }
        #endregion
        #region BidAnalysisApproval
        public BidAnalysisRequest GetBidAnalysisRequest(int BidAnalysisId)
        {
            return _workspace.Single<BidAnalysisRequest>(x => x.Id == BidAnalysisId);
            //x => x.Bidders.Select(y => y.ItemAccount), x => x.PurchaseRequestDetails.Select(z => z.project), x => x.Bidders.Select(z => z.Supplier), x => x.BidAnalysises.Bidders.Select(z => z.BidderItemDetails.Select(y => y.ItemAccount)), x => x.PurchaseOrders.PurchaseOrderDetails);
        }
        public IList<BidAnalysisRequest> ListBidAnalysisRequests(string RequestNo, string RequestDate, string ProgressStatus)
        {
            string filterExpression = "";

            if (ProgressStatus != "Completed")
            {
                filterExpression = " SELECT  *  FROM BidAnalysisRequests INNER JOIN AppUsers on AppUsers.Id=BidAnalysisRequests.CurrentApprover  Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When BidAnalysisRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When BidAnalysisRequests.RequestDate = '" + RequestDate + "'  Then 1 END AND BidAnalysisRequests.ProgressStatus='" + ProgressStatus + "' " +
                                       " AND  ((BidAnalysisRequests.CurrentApprover = '" + CurrentUser().Id + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by BidAnalysisRequests.Id DESC";
            }
            else
            {
                filterExpression = " SELECT  *  FROM BidAnalysisRequests INNER JOIN AppUsers on AppUsers.Id=BidAnalysisRequests.CurrentApprover INNER JOIN BidAnalysisRequestStatuses on BidAnalysisRequestStatuses.BidAnalysisRequest_Id = BidAnalysisRequests.Id Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When BidAnalysisRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When BidAnalysisRequests.RequestDate = '" + RequestDate + "'  Then 1 END AND BidAnalysisRequests.ProgressStatus='" + ProgressStatus + "' AND " +
                                           "   (BidAnalysisRequestStatuses.ApprovalStatus Is not null AND (BidAnalysisRequestStatuses.Approver = '" + CurrentUser().Id + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by BidAnalysisRequests.Id DESC ";
            }
            return _workspace.SqlQuery<BidAnalysisRequest>(filterExpression).ToList();

        }
        public Bidder GetBidderbyId(int Id)
        {
            return _workspace.Single<Bidder>(x => x.Id == Id);
        }
        public IList<BidAnalysisRequest> ListBidReqInProgressById(int ReqId)
        {
            string filterExpression = "";

            filterExpression = "SELECT  * " +
                       " FROM   dbo.BidAnalysisRequests INNER JOIN" +
                      " dbo.BidderItemDetails ON dbo.BidAnalysisRequests.Id = dbo.BidderItemDetails.BidAnalysisRequest_Id INNER JOIN" +
                      " dbo.Bidders ON dbo.BidderItemDetails.Id = dbo.Bidders.BidderItemDetail_Id INNER JOIN" +
                      " dbo.PurchaseRequests ON dbo.BidAnalysisRequests.PurchaseRequest_Id = dbo.PurchaseRequests.Id INNER JOIN" +
                      " dbo.Suppliers ON dbo.Bidders.Supplier_Id = dbo.Suppliers.Id  Where dbo.BidAnalysisRequests.Id = '" + ReqId + "' And dbo.Bidders.Rank = 1  order by BidAnalysisRequests.Id Desc ";
            return _workspace.SqlQuery<BidAnalysisRequest>(filterExpression).ToList();

        }
        public IList<Bidder> GetBiddersByBidReq(int ReqId)
        {
            string filterExpression = "";

            filterExpression = "SELECT  *" +
                                " FROM dbo.Bidders INNER JOIN " +
                                " dbo.BidderItemDetails ON dbo.Bidders.BidderItemDetail_Id = dbo.BidderItemDetails.Id INNER JOIN" +
                                " dbo.BidAnalysisRequests ON dbo.BidderItemDetails.BidAnalysisRequest_Id = dbo.BidAnalysisRequests.Id INNER JOIN" +
                                " dbo.Suppliers ON dbo.Bidders.Supplier_Id = dbo.Suppliers.Id  Where dbo.BidAnalysisRequests.Id = '" + ReqId + "' And dbo.Bidders.Rank = 1  order by BidAnalysisRequests.Id Desc ";
            return _workspace.SqlQuery<Bidder>(filterExpression).ToList();

        }
        public PurchaseRequestDetail GetPurchaseRequestbyPuID(int Id)
        {
            return _workspace.Single<PurchaseRequestDetail>(x => x.Id == Id, y => y.PurchaseRequest);
        }
        public BAAttachment GetBAAttachment(int attachmentId)
        {
            return _workspace.Single<BAAttachment>(x => x.Id == attachmentId);
        }
        public int GetLastPurchaseOrderId()
        {
            if (_workspace.Last<PurchaseOrder>() != null)
            {
                return _workspace.Last<PurchaseOrder>().Id;
            }
            else { return 0; }


        }
        #endregion
        #region PurchaseApproval
        public PurchaseRequest GetPurchaseRequest(int PurchaseId)
        {
            return _workspace.Single<PurchaseRequest>(x => x.Id == PurchaseId);
            //x => x.Bidders.Select(y => y.ItemAccount), x => x.PurchaseRequestDetails.Select(z => z.project), x => x.Bidders.Select(z => z.Supplier), x => x.BidAnalysises.Bidders.Select(z => z.BidderItemDetails.Select(y => y.ItemAccount)), x => x.PurchaseOrders.PurchaseOrderDetails);
        }
        public IList<PurchaseRequest> ListPurchaseRequests(string RequestNo, string RequestDate, string ProgressStatus)
        {
            string filterExpression = "";

            if (ProgressStatus != "Completed")
            {
                filterExpression = " SELECT  *  FROM PurchaseRequests INNER JOIN AppUsers on AppUsers.Id=PurchaseRequests.CurrentApprover  Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When PurchaseRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When PurchaseRequests.RequestedDate = '" + RequestDate + "'  Then 1 END AND PurchaseRequests.ProgressStatus='" + ProgressStatus + "' " +
                                       " AND  ((PurchaseRequests.CurrentApprover = '" + CurrentUser().Id + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by PurchaseRequests.Id DESC";
            }
            else
            {
                filterExpression = " SELECT * FROM PurchaseRequests " +
                                   " INNER JOIN PurchaseRequestStatuses ON PurchaseRequestStatuses.PurchaseRequest_Id = PurchaseRequests.Id " +
                                   " INNER JOIN AppUsers ON AppUsers.Id = PurchaseRequestStatuses.Approver" +
                                   " LEFT JOIN AssignJobs ON AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 " +
                                   " WHERE 1 = CASE WHEN '" + RequestNo + "' = '' THEN 1 WHEN PurchaseRequests.RequestNo = '" + RequestNo + "'  Then 1 END " +
                                   " AND 1 = CASE WHEN '" + RequestDate + "' = '' THEN 1 WHEN PurchaseRequests.RequestedDate = '" + RequestDate + "'  Then 1 END " +
                                   " AND PurchaseRequests.ProgressStatus='" + ProgressStatus + "' AND PurchaseRequests.CurrentStatus != 'Rejected'" +
                                   " AND PurchaseRequestStatuses.ApprovalStatus Is NOT NULL " +
                                   " AND (PurchaseRequestStatuses.Approver = '" + CurrentUser().Id + "' OR AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "') " +
                                   " ORDER BY PurchaseRequests.Id DESC ";
            }
            return _workspace.SqlQuery<PurchaseRequest>(filterExpression).ToList();

        }
        public IList<PurchaseRequest> GetPurchaseRequestsInProgressPO()
        {
            string filterExpression = "SELECT  *  FROM BidAnalysisRequests INNER JOIN PurchaseRequests on PurchaseRequests.Id = BidAnalysisRequests.PurchaseRequest_Id  Where BidAnalysisRequests.ProgressStatus = 'Completed'  order by BidAnalysisRequests.Id Desc ";

            return _workspace.SqlQuery<PurchaseRequest>(filterExpression).ToList();
        }
        #endregion
        #region SoleVendorApproval
        public SoleVendorRequest GetSoleVendorRequest(int SoleVendorRequestId)
        {
            return _workspace.Single<SoleVendorRequest>(x => x.Id == SoleVendorRequestId);
        }
        public IList<SoleVendorRequest> ListSoleVendorRequests(string RequestNo, string RequestDate, string ProgressStatus)
        {
            string filterExpression = "";
            if (ProgressStatus != "Completed")
            {
                filterExpression = " SELECT * FROM SoleVendorRequests INNER JOIN AppUsers ON AppUsers.Id = SoleVendorRequests.CurrentApprover Left JOIN AssignJobs ON AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When SoleVendorRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When SoleVendorRequests.RequestDate = '" + RequestDate + "'  Then 1 END AND SoleVendorRequests.ProgressStatus='" + ProgressStatus + "' " +
                                       " AND ((SoleVendorRequests.CurrentApprover = '" + CurrentUser().Id + "') OR (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by SoleVendorRequests.Id DESC ";
            }
            else
            {
                filterExpression = " SELECT * FROM SoleVendorRequests INNER JOIN AppUsers ON AppUsers.Id = SoleVendorRequests.CurrentApprover INNER JOIN SoleVendorRequestStatuses ON SoleVendorRequestStatuses.SoleVendorRequest_Id = SoleVendorRequests.Id LEFT JOIN AssignJobs ON AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When SoleVendorRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When SoleVendorRequests.RequestDate = '" + RequestDate + "'  Then 1 END AND SoleVendorRequests.ProgressStatus='" + ProgressStatus + "'  " +
                                          " AND (SoleVendorRequestStatuses.ApprovalStatus IS NOT NULL AND (SoleVendorRequestStatuses.Approver = '" + CurrentUser().Id + "') OR (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) ORDER BY SoleVendorRequests.Id DESC ";
            }

            return _workspace.SqlQuery<SoleVendorRequest>(filterExpression).ToList();
        }
        public int GetLastPurchaseOrderSoleVendorId()
        {
            if (_workspace.Last<PurchaseOrderSoleVendor>() != null)
            {
                return _workspace.Last<PurchaseOrderSoleVendor>().Id;
            }
            else { return 0; }
        }
        public PurchaseOrderSoleVendor GetPurchaseOrderSoleVendor(int purchaseOrderSoleVendorId)
        {
            return _workspace.Single<PurchaseOrderSoleVendor>(x => x.Id == purchaseOrderSoleVendorId);
        }
        #endregion
        #region MaintenanceApproval
        public MaintenanceRequest GetMaintenanceRequest(int MaintenanceRequestId)
        {
            return _workspace.Single<MaintenanceRequest>(x => x.Id == MaintenanceRequestId);
        }
        public IList<MaintenanceRequest> ListMaintenanceRequests(string RequestNo, string RequestDate, string ProgressStatus)
        {
            string filterExpression = "";
            if (ProgressStatus != "Completed")
            {
                filterExpression = " SELECT * FROM MaintenanceRequests INNER JOIN AppUsers ON AppUsers.Id = MaintenanceRequests.CurrentApprover Left JOIN AssignJobs ON AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When MaintenanceRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When MaintenanceRequests.RequestDate = '" + RequestDate + "'  Then 1 END AND MaintenanceRequests.ProgressStatus='" + ProgressStatus + "' " +
                                       " AND ((MaintenanceRequests.CurrentApprover = '" + CurrentUser().Id + "') OR (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by MaintenanceRequests.Id DESC ";
            }
            else
            {
                filterExpression = " SELECT * FROM MaintenanceRequests INNER JOIN AppUsers ON AppUsers.Id = MaintenanceRequests.CurrentApprover INNER JOIN MaintenanceRequestStatuses ON MaintenanceRequestStatuses.MaintenanceRequest_Id = MaintenanceRequests.Id LEFT JOIN AssignJobs ON AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When MaintenanceRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When MaintenanceRequests.RequestDate = '" + RequestDate + "'  Then 1 END AND MaintenanceRequests.ProgressStatus='" + ProgressStatus + "'  " +
                                          " AND (MaintenanceRequestStatuses.ApprovalStatus IS NOT NULL AND (MaintenanceRequestStatuses.Approver = '" + CurrentUser().Id + "') OR (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) ORDER BY MaintenanceRequests.Id DESC ";
            }

            return _workspace.SqlQuery<MaintenanceRequest>(filterExpression).ToList();
        }
        public MaintenanceSparePart GetMaintenanceSparePart(int MaintenanceSparePartId)
        {
            return _workspace.Single<MaintenanceSparePart>(x => x.Id == MaintenanceSparePartId);
        }
        public AppUser GetMechanic()
        {
            return _workspace.Single<AppUser>(x => x.EmployeePosition.PositionName == "Driver/Mechanic");
        }
        #endregion
        #region StoreApproval
        public StoreRequest GetStoreRequest(int StoreId)
        {
            return _workspace.Single<StoreRequest>(x => x.Id == StoreId);
            //x => x.Bidders.Select(y => y.ItemAccount), x => x.StoreRequestDetails.Select(z => z.project), x => x.Bidders.Select(z => z.Supplier), x => x.BidAnalysises.Bidders.Select(z => z.BidderItemDetails.Select(y => y.ItemAccount)), x => x.StoreOrders.StoreOrderDetails);
        }
        public IList<StoreRequest> ListStoreRequests(string RequestNo, string RequestDate, string ProgressStatus)
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
        public IList<StoreRequest> GetStoreRequestsInProgressPO()
        {
            string filterExpression = "SELECT  *  FROM BidAnalysisRequests INNER JOIN StoreRequests on StoreRequests.Id = BidAnalysisRequests.StoreRequest_Id  Where BidAnalysisRequests.ProgressStatus = 'Completed'  order by BidAnalysisRequests.Id Desc ";

            return _workspace.SqlQuery<StoreRequest>(filterExpression).ToList();
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

            try
            {
                _workspace.CommitChanges();
            }
            catch (DbEntityValidationException dbEx)
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
        }
        public void Commit()
        {
            _workspace.CommitChanges();
        }
        #endregion        
    }
}
