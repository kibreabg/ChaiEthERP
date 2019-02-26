using System;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Chai.WorkflowManagment.Modules.Report.Views
{
    public partial class frmLeaveApproverProReport : POCBasePage, IfrmLeaveApproverProReportView
    {
		private frmLeaveApproverProReportPresenter _presenter;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this._presenter.OnViewInitialized();
                ViewLeaveReport();

            }
			this._presenter.OnViewLoaded();
           

        }

		[CreateNew]
        public frmLeaveApproverProReportPresenter Presenter
		{
			get
			{
				return this._presenter;
			}
			set
			{
				if(value == null)
					throw new ArgumentNullException("value");

				this._presenter = value;
				this._presenter.View = this;
			}
		}
        public override string PageID
        {
            get
            {
                return "{8FD3CC34-9213-BBCB-A316-F59D40EADB44}";
            }
        }
       
        private void ViewLeaveReport()
        {

            var path = Server.MapPath("Report.rdlc");
            var datasource = _presenter.LeaveApproverProgressReport();
            ReportDataSource s = new ReportDataSource("DataSet1", datasource.Tables[0]);
            ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(s);
            ReportViewer1.LocalReport.ReportPath = path;
          


        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            Panel1.Visible = true;
            ViewLeaveReport();
        }
}
}

