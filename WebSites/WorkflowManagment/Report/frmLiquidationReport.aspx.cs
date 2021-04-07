using System;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Report.Views
{
    public partial class frmLiquidationReport : POCBasePage, IfrmLiquidationReportView
	{
        private frmLiquidationReportPresenter _presenter;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this._presenter.OnViewInitialized();
			}
			this._presenter.OnViewLoaded();
		}

		[CreateNew]
        public frmLiquidationReportPresenter Presenter
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
                return "{8FD3CC34-9213-497B-A316-F59D40EADB44}";
            }
        }
        private void ViewLiquidationReport()
        {

            var path = Server.MapPath("LiquidationReport.rdlc");
            var datasource = _presenter.GetLiquidationReport(txtDateFrom.Text, txtDateTo.Text);
            ReportDataSource s = new ReportDataSource("LiquidationDataSet", datasource.Tables[0]);
            rvLiquidation.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            rvLiquidation.LocalReport.DataSources.Clear();
            rvLiquidation.LocalReport.DataSources.Add(s);
            rvLiquidation.LocalReport.ReportPath = path;
            var DateFrom = txtDateFrom.Text != "" ? txtDateFrom.Text : " ";
            var DateTo = txtDateTo.Text != "" ? txtDateTo.Text : " ";
            var param4 = new ReportParameter("DateFrom", DateFrom);
            var param5 = new ReportParameter("DateTo", DateTo);
            var parameters = new List<ReportParameter>
            {
                param4,
                param5
            };
            rvLiquidation.LocalReport.SetParameters(parameters);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
        protected void btnView_Click(object sender, EventArgs e)
        {            
            try
            {
                pnlLiquidationReport.Visible = true;
                ViewLiquidationReport();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
}
}

