using System;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Report.Views
{
    public partial class frmTravelAdvanceReport : POCBasePage, IfrmTravelAdvanceReportView
    {
        private frmTravelAdvanceReportPresenter _presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
            }
            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public frmTravelAdvanceReportPresenter Presenter
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
        public override string PageID
        {
            get
            {
                return "{16099272-44CA-4DDC-A850-D964F417D036}";
            }
        }
        private void ViewTravelAdvanceReport()
        {
            var path = Server.MapPath("TravelAdvanceReport.rdlc");
            var datasource = _presenter.GetTravelAdvanceReport(txtDateFrom.Text, txtDateTo.Text);
            ReportDataSource s = new ReportDataSource("TravelDataSet", datasource.Tables[0]);
            rvTravel.ProcessingMode = ProcessingMode.Local;
            rvTravel.LocalReport.DataSources.Clear();
            rvTravel.LocalReport.DataSources.Add(s);
            rvTravel.LocalReport.ReportPath = path;
            var DateFrom = txtDateFrom.Text != "" ? txtDateFrom.Text : " ";
            var DateTo = txtDateTo.Text != "" ? txtDateTo.Text : " ";
            var param4 = new ReportParameter("DateFrom", DateFrom);
            var param5 = new ReportParameter("DateTo", DateTo);
            var parameters = new List<ReportParameter>
            {
                param4,
                param5
            };
            rvTravel.LocalReport.SetParameters(parameters);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                pnlTravelReport.Visible = true;
                ViewTravelAdvanceReport();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
    }
}

