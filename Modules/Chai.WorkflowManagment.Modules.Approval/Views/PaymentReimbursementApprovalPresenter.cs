﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.Modules.Admin;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Modules.Setting;
using Chai.WorkflowManagment.Modules.Request;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public class PaymentReimbursementApprovalPresenter : Presenter<IPaymentReimbursementApprovalView>
    {
        private ApprovalController _controller;
        private RequestController _requestController;
        private PaymentReimbursementRequest _PaymentReimbursementRequest;
        private SettingController _settingController;
        private AdminController _adminController;

        public PaymentReimbursementApprovalPresenter([CreateNew] ApprovalController controller, RequestController requestController, SettingController settingController, AdminController adminController)
        {
            _controller = controller;
            _requestController = requestController;
            _settingController = settingController;
            _adminController = adminController;
        }

        public override void OnViewLoaded()
        {
            if (View.GetPaymentReimbursementRequestId > 0)
            {
                _controller.CurrentObject = _requestController.GetPaymentReimbursementRequest(View.GetPaymentReimbursementRequestId);
            }
            CurrentPaymentReimbursementRequest = _controller.CurrentObject as PaymentReimbursementRequest;
        }

        public override void OnViewInitialized()
        {

        }
        public OperationalControlRequest GetOperationalControlRequestByLiquidationId(int liquidationId)
        {
            return _requestController.GetOperationalControlRequestByLiquidationId(liquidationId);
        }
        public PaymentReimbursementRequest CurrentPaymentReimbursementRequest
        {
            get
            {
                if (_PaymentReimbursementRequest == null)
                {
                    int id = View.GetPaymentReimbursementRequestId;
                    if (id > 0)
                        _PaymentReimbursementRequest = _requestController.GetPaymentReimbursementRequest(id);
                    else
                        _PaymentReimbursementRequest = new PaymentReimbursementRequest();
                }
                return _PaymentReimbursementRequest;
            }
            set { _PaymentReimbursementRequest = value; }
        }
        public PaymentReimbursementRequest GetPaymentReimbursementRequest(int Id)
        {
            return _requestController.GetPaymentReimbursementRequest(Id);
        }
        public IList<AppUser> GetEmployeeList()
        {
            return _settingController.GetEmployeeList();
        }
        public IList<AppUser> GetDrivers()
        {
            return _adminController.GetDrivers();
        }
        public IList<AppUser> GetAppUsersByEmployeePosition(int employeePosition)
        {
            return _settingController.GetAppUsersByEmployeePosition(employeePosition);
        }
        public IList<CarRental> GetCarRentals()
        {
            return _settingController.GetCarRentals();
        }
        public CarRental GetCarRental(int Id)
        {
            return _settingController.GetCarRental(Id);
        }
        public ItemAccount GetItemAccount(int id)
        {
            return _settingController.GetItemAccount(id);
        }
        public IList<ItemAccount> GetItemAccountList()
        {
            return _settingController.GetItemAccounts();
        }
        public PRAttachment GetAttachment(int attachmentId)
        {
            return _requestController.GetPRAttachment(attachmentId);
        }
        public AppUser GetUser(int UserId)
        {
            return _adminController.GetUser(UserId);
        }
        public void SaveOrUpdatePaymentReimbursementRequest(PaymentReimbursementRequest PaymentReimbursementRequest)
        {
            _controller.SaveOrUpdateEntity(PaymentReimbursementRequest);
        }
        public void DeletePaymentReimbursementRequest(PaymentReimbursementRequest PaymentReimbursementRequest)
        {
            _controller.DeleteEntity(PaymentReimbursementRequest);
        }
        public IList<PaymentReimbursementRequest> ListPaymentReimbursementRequests(string RequestNo, string RequestDate, string ProgressStatus, string Requester)
        {
            return _controller.ListPaymentReimbursementRequests(RequestNo, RequestDate, ProgressStatus, Requester);
        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public AssignJob GetAssignedJobbycurrentuser()
        {
            return _controller.GetAssignedJobbycurrentuser();
        }
        public ApprovalSetting GetApprovalSettingforProcess(string Requesttype, decimal value)
        {
            return _settingController.GetApprovalSettingforProcess(Requesttype, value);
        }
        public void navigate(string url)
        {
            _controller.Navigate(url);
        }
        public AssignJob GetAssignedJobbycurrentuser(int userId)
        {
            return _controller.GetAssignedJobbycurrentuser(userId);
        }
        public int GetAssignedUserbycurrentuser()
        {
            return _controller.GetAssignedUserbycurrentuser();
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}




