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
    public class CashPaymentApprovalPresenter : Presenter<ICashPaymentApprovalView>
    {
        private ApprovalController _controller;
        private RequestController _requestController;
        private CashPaymentRequest _cashPaymentRequest;
        private SettingController _settingController;
        private AdminController _adminController;

        public CashPaymentApprovalPresenter([CreateNew] ApprovalController controller, RequestController requestController, SettingController settingController, AdminController adminController)
        {
            _controller = controller;
            _requestController = requestController;
            _settingController = settingController;
            _adminController = adminController;
        }

        public override void OnViewLoaded()
        {
            if (View.GetCashPaymentRequestId > 0)
            {
                _controller.CurrentObject = _requestController.GetCashPaymentRequest(View.GetCashPaymentRequestId);
            }
            CurrentCashPaymentRequest = _controller.CurrentObject as CashPaymentRequest;
        }

        public override void OnViewInitialized()
        {
            if (_cashPaymentRequest == null)
            {
                _controller.CurrentObject = new CashPaymentRequest();
            }
        }
        public CashPaymentRequest CurrentCashPaymentRequest
        {
            get
            {
                if (_cashPaymentRequest == null)
                {
                    int id = View.GetCashPaymentRequestId;
                    if (id > 0)
                        _cashPaymentRequest = _requestController.GetCashPaymentRequest(id);
                    else
                        _cashPaymentRequest = new CashPaymentRequest();
                }
                return _cashPaymentRequest;
            }
            set { _cashPaymentRequest = value; }
        }
        public AppUser GetUser(int UserId)
        {
            return _adminController.GetUser(UserId);
        }
        public IList<AppUser> GetEmployeeList()
        {
            return _settingController.GetEmployeeList();
        }
        public void SaveOrUpdateCashPaymentRequest(CashPaymentRequest CashPaymentRequest)
        {
            _controller.SaveOrUpdateEntity(CashPaymentRequest);
            _controller.CurrentObject = null;
        }
        public CashPaymentRequest GetCashPaymentRequest(int reqId)
        {
            return _requestController.GetCashPaymentRequest(reqId);
        }
        public IList<CashPaymentRequest> ListCashPaymentRequests(string RequestNo, string RequestDate, string ProgressStatus, string Requester)
        {
            return _controller.ListCashPaymentRequests(RequestNo, RequestDate, ProgressStatus, Requester);
        }
        public OperationalControlRequest GetOperationalControlRequestByPaymentId(int paymentId)
        {
            return _requestController.GetOperationalControlRequestByPaymentId(paymentId);
        }
        public CPRAttachment GetAttachment(int attachmentId)
        {
            return _requestController.GetCPRAttachment(attachmentId);
        }
        public Account GetAccount(int AccountId)
        {
            return _settingController.GetAccount(AccountId);
        }
        public IList<Account> GetAccounts()
        {
            return _settingController.GetAccounts();
        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public IList<AppUser> GetAppUsersByEmployeePosition(int employeePosition)
        {
            return _settingController.GetAppUsersByEmployeePosition(employeePosition);
        }
        public IList<Program> GetPrograms()
        {
            return _settingController.GetPrograms();
        }
        public IList<Project> ListProjects(int programID)
        {
            return _settingController.GetProjectsByProgramId(programID);
        }
        public Project GetProject(int ProjectId)
        {
            return _settingController.GetProject(ProjectId);
        }
        public Grant GetGrant(int GrantId)
        {
            return _settingController.GetGrant(GrantId);
        }
        public IList<Grant> GetGrantbyprojectId(int projectId)
        {
            return _settingController.GetProjectGrantsByprojectId(projectId);
        }
        public AssignJob GetAssignedJobbycurrentuser()
        {
            return _controller.GetAssignedJobbycurrentuser();
        }
        public AssignJob GetAssignedJobbycurrentuser(int userId)
        {
            return _controller.GetAssignedJobbycurrentuser(userId);
        }
        public ItemAccount GetItemAccount(int ItemAccountId)
        {
            return _settingController.GetItemAccount(ItemAccountId);
        }
        public IList<ItemAccount> ListItemAccounts()
        {
            return _settingController.GetItemAccounts();
        }
        public void navigate(string url)
        {
            _controller.Navigate(url);
        }
        public ApprovalSetting GetApprovalSettingMedical()
        {
            return _settingController.GetApprovalSettingMedical();
        }
        public ApprovalSetting GetApprovalSettingforProcess(string Requesttype, decimal value)
        {
            return _settingController.GetApprovalSettingforProcess(Requesttype, value);
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




