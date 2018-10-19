using System;
using Microsoft.Practices.ObjectBuilder;
using System.Net.Mail;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Shared;
using System.Net.Configuration;
using System.Configuration;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Shell.Views
{
    public partial class UserLogin : Microsoft.Practices.CompositeWeb.Web.UI.Page, IUserLoginView
    {
        private UserLoginPresenter _presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.User != null)
            {
                if (Context.User.Identity.IsAuthenticated)
                    Context.Response.Redirect("~/");
            }

            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                txtUsername.Focus();
            }
            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public UserLoginPresenter Presenter
        {
            get
            {
                return this._presenter;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this._presenter = value;
                this._presenter.View = this;
            }
        }
        protected void btnLogin_Click(object sender, EventArgs e)   
        {
            if (this.txtUsername.Text.Trim().Length > 0 && this.txtPassword.Text.Trim().Length > 0)
            {
                try
                {
                    if (_presenter.AuthenticateUser())
                    {
                        //_presenter.RedirectToRowUrl();
                        Context.Response.Redirect("Default.aspx");
                    }
                    else
                    {
                        this.lblLoginError.Text = "User name or password incorrect";
                        this.lblLoginError.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    this.lblLoginError.Text = ex.Message + " The user may be not active user";
                    this.lblLoginError.Visible = true;
                }
            }
            else
            {
                this.lblLoginError.Text = "Please enter both a username and password";
                this.lblLoginError.Visible = true;
            }

        }
        protected void lnkForgotPassword_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text != "")
            {
                try
                {
                    AppUser user = _presenter.SearchUser(txtUsername.Text);
                    user.Password = Encryption.StringToMD5Hash("pass@123");
                    _presenter.SaveOrUpdateUser(user);

                    SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                    MailMessage Msg = new MailMessage();
                    // Sender e-mail address.
                    Msg.From = new MailAddress(section.From);
                    // Recipient e-mail address.
                    Msg.To.Add(user.Email);
                    Msg.Subject = "Your Password Details";
                    StringBuilder message = new StringBuilder();
                    string body = "Hi " + user.FullName + ", <br/>Please check your Login Details <br/><br/>Your Username: " + txtUsername.Text + "<br/><br/>Your Password: " + "pass@123" + "<br/><br/>";
                    message.AppendLine("<html><head><meta name='viewport' content='width=device-width' /><meta http-equiv='Content-Type' content='text/html; charset=UTF-8' /><title>Simple Transactional Email</title><style>img{border:none;-ms-interpolation-mode:bicubic;max-width:100%}body{background-color:#f6f6f6;font-family:sans-serif;-webkit-font-smoothing:antialiased;font-size:14px;line-height:1.4;margin:0;padding:0;-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%}table{border-collapse:separate;mso-table-lspace:0pt;mso-table-rspace:0pt;width:100%}table td{font-family:sans-serif;font-size:14px;vertical-align:top}.body{background-color:#f6f6f6;width:100%}.container{display:block;Margin:0 auto !important;max-width:580px;padding:10px;width:580px}.content{box-sizing:border-box;display:block;Margin:0 auto;max-width:580px;padding:10px}.main{background:#fff;border-radius:3px;width:100%}.wrapper{box-sizing:border-box;padding:20px}.content-block{padding-bottom:10px;padding-top:10px}.footer{clear:both;Margin-top:10px;text-align:center;width:100%}.footer td, .footer p, .footer span, .footer a{color:#999;font-size:12px;text-align:center}h1,h2,h3,h4{color:#000;font-family:sans-serif;font-weight:400;line-height:1.4;margin:0;Margin-bottom:30px}h1{font-size:35px;font-weight:300;text-align:center;text-transform:capitalize}p,ul,ol{font-family:sans-serif;font-size:14px;font-weight:normal;margin:0;Margin-bottom:15px}p li, ul li, ol li{list-style-position:inside;margin-left:5px}a{color:#3498db;text-decoration:underline}.btn{box-sizing:border-box;width:100%}.btn>tbody>tr>td{padding-bottom:15px}.btn table{width:auto}.btn table td{background-color:#fff;border-radius:5px;text-align:center}.btn a{background-color:#fff;border:solid 1px #3498db;border-radius:5px;box-sizing:border-box;color:#3498db;cursor:pointer;display:inline-block;font-size:14px;font-weight:bold;margin:0;padding:12px 25px;text-decoration:none;text-transform:capitalize}.btn-primary table td{background-color:#3498db}.btn-primary a{background-color:#3498db;border-color:#3498db;color:#fff}.last{margin-bottom:0}.first{margin-top:0}.align-center{text-align:center}.align-right{text-align:right}.align-left{text-align:left}.clear{clear:both}.mt0{margin-top:0}.mb0{margin-bottom:0}.preheader{color:transparent;display:none;height:0;max-height:0;max-width:0;opacity:0;overflow:hidden;mso-hide:all;visibility:hidden;width:0}.powered-by a{text-decoration:none}hr{border:0;border-bottom:1px solid #f6f6f6;Margin:20px 0}@media only screen and (max-width: 620px){table[class=body] h1{font-size:28px !important;margin-bottom:10px !important}table[class=body] p, table[class=body] ul, table[class=body] ol, table[class=body] td, table[class=body] span, table[class=body] a{font-size:16px !important}table[class=body] .wrapper, table[class=body] .article{padding:10px !important}table[class=body] .content{padding:0 !important}table[class=body] .container{padding:0 !important;width:100% !important}table[class=body] .main{border-left-width:0 !important;border-radius:0 !important;border-right-width:0 !important}table[class=body] .btn table{width:100% !important}table[class=body] .btn a{width:100% !important}table[class=body] .img-responsive{height:auto !important;max-width:100% !important;width:auto !important}}@media all{.ExternalClass{width:100%}.ExternalClass, .ExternalClass p, .ExternalClass span, .ExternalClass font, .ExternalClass td, .ExternalClass div{line-height:100%}.apple-link a{color:inherit !important;font-family:inherit !important;font-size:inherit !important;font-weight:inherit !important;line-height:inherit !important;text-decoration:none !important}.btn-primary table td:hover{background-color:#34495e !important}.btn-primary a:hover{background-color:#34495e !important;border-color:#34495e !important}}</style></head><body class=''><table border='0' cellpadding='0' cellspacing='0' class='body'><tr><td>&nbsp;</td><td class='container'><div class='content'><table class='main'><tr><td class='wrapper'><table border='0' cellpadding='0' cellspacing='0'><tr><td>");
                    message.AppendLine("<p>" + body + "</p>");
                    message.AppendLine("<table border='0' cellpadding='0' cellspacing='0' class='btn btn-primary'><tbody><tr><td align='left'><table border='0' cellpadding='0' cellspacing='0'><tbody><tr><td> <a href='http://10.143.1.25/CHAIETERP/' target='_blank'>Click here</a></td></tr></tbody></table></td></tr></tbody></table></td></tr></table></td></tr></table>");
                    Msg.Body = message.ToString();
                    Msg.IsBodyHtml = true;
                    // Send the email.
                    using (SmtpClient client = new SmtpClient(section.Network.Host, section.Network.Port))
                    {
                        client.EnableSsl = section.Network.EnableSsl;
                        client.Timeout = 2000000;
                        client.Credentials = new System.Net.NetworkCredential(section.Network.UserName, section.Network.Password);
                        client.Send(Msg);
                        client.Dispose();
                    }
                    //Msg = null;
                    lblForgotPassword.Text = "Your Password Details Sent to your Email, Please change your password after you login to the system";
                    // Clear the textbox valuess
                    //lblForgotPassword.Text = "";
                }
                catch (Exception ex)
                {
                    lblForgotPassword.Text = ex.Message;
                }
            }
            else
            {
                lblForgotPassword.Text = "The Username you entered does not exist.";
            }
        }

        #region IUserLoginView Members

        public string GetUserName
        {
            get { return txtUsername.Text; }
        }

        public string GetPassword
        {
            get { return txtPassword.Text; }
        }

        public bool PersistLogin
        {
            get { return chkPersistLogin.Checked; }
        }

        #endregion
    }
}

