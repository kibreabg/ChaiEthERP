﻿using System;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Modules.Admin.Views;
using System.Web.Security;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Shared.MailSender;
using System.Web.UI;

namespace Chai.WorkflowManagment.Modules.Shell.MasterPages
{
    public partial class DefaultMaster : BaseMaster
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                base.Presenter.OnViewInitialized();
            }
            base.Presenter.OnViewLoaded();
            UserRole();
            UserApprover();
            txtFrom.Text = CurrentUser.Email;

        }
        protected void LoginStatus1_LoggedOut(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
        }
        private void UserRole()
        {
            foreach (AppUserRole userroles in CurrentUser.AppUserRoles)
            {
                if (userroles.Role.Name.Contains("Administrator"))
                {
                    lnkAdmin.Visible = true;
                    break;
                }
                else
                {
                    lnkAdmin.Visible = false;
                }

            }

        }
        private void UserApprover()
        {
            foreach (AppUserRole userroles in CurrentUser.AppUserRoles)
            {

                if (userroles.Role.Name.Contains("Approver") || userroles.Role.Name.Contains("Reviewer") || userroles.Role.Name.Contains("Preparer") || userroles.Role.Name.Contains("Authorizer") || userroles.Role.Name.Contains("Payer"))
                {
                    lnkassign.Visible = true;
                    break;
                }
                else
                {
                    lnkassign.Visible = false;
                }
            }

        }
        protected void lnkAdmin_Click(object sender, EventArgs e)
        {
            this.Page.Response.Redirect(string.Format("~/Admin/Default.aspx?{0}=0", Chai.WorkflowManagment.Shared.AppConstants.TABID));
        }
        protected void lnkEditProfile_Click(object sender, EventArgs e)
        {
            this.Page.Response.Redirect(string.Format("~/HRM/frmEmployeeProfile.aspx?{0}=6&EmpId={1}", Chai.WorkflowManagment.Shared.AppConstants.TABID, CurrentUser.Id));
        }
        protected void lnkassign_Click(object sender, EventArgs e)
        {
            this.Page.Response.Redirect(string.Format("~/Admin/AssignJob.aspx?{0}=0", Chai.WorkflowManagment.Shared.AppConstants.TABID));
        }
        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                EmailSender.SendEmails(txtFrom.Text, txtTo.Text, txtSubject.Text, txtMessage.Text);
                lblMessage.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
                clearControls();
            }
            catch (Exception ex)
            {

            }
        }
        private void clearControls()
        {
            txtFrom.Text = "";
            txtSubject.Text = "";
            txtMessage.Text = "";

        }
    }
}
