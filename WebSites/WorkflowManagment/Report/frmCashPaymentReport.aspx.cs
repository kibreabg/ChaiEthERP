using System;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Report.Views
{
    public partial class frmCashPaymentReport : POCBasePage, IfrmCashPaymentReportView
    {
        private frmCashPaymentReportPresenter _presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
            }
            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public frmCashPaymentReportPresenter Presenter
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
                return "{4C647E6A-06EB-42A4-B531-D92E977EE8D2}";
            }
        }
        private void ViewCashPaymentReport()
        {

            var path = Server.MapPath("CashPaymentReport.rdlc");
            var datasource = _presenter.GetCashPaymentReport(txtDateFrom.Text, txtDateTo.Text);
            ReportDataSource s = new ReportDataSource("PaymentReqDataSet", datasource.Tables[0]);
            rvPayment.ProcessingMode = ProcessingMode.Local;
            rvPayment.LocalReport.DataSources.Clear();
            rvPayment.LocalReport.DataSources.Add(s);
            rvPayment.LocalReport.ReportPath = path;
            var DateFrom = txtDateFrom.Text != "" ? txtDateFrom.Text : " ";
            var DateTo = txtDateTo.Text != "" ? txtDateTo.Text : " ";
            var param4 = new ReportParameter("DateFrom", DateFrom);
            var param5 = new ReportParameter("DateTo", DateTo);
            var parameters = new List<ReportParameter>
            {
                param4,
                param5
            };
            rvPayment.LocalReport.SetParameters(parameters);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                pnlPaymentReport.Visible = true;
                ViewCashPaymentReport();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }

        }
    }
}

