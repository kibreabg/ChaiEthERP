using System;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;

namespace Chai.WorkflowManagment.Modules.Report.Views
{
    public partial class frmEmployeebirth : POCBasePage, IfrmEmployeebirthView
	{
		private frmEmployeebirthPresenter _presenter;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this._presenter.OnViewInitialized();
			}
			this._presenter.OnViewLoaded();
            
           // ViewEmployeeBirthReport();

        }

		[CreateNew]
        public frmEmployeebirthPresenter Presenter
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
                return "{A91F2C96-C8E9-493B-AE47-033940D3BA93}}";
            }
        }
        private void ViewEmployeeBirthReport()
        {

            var path = Server.MapPath("Employeebirth.rdlc");
            var datasource = _presenter.EmployeeBirthReport(ddlmonth.SelectedValue);
            ReportDataSource s = new ReportDataSource("DataSet1", datasource.Tables[0]);
            ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(s);
            ReportViewer1.LocalReport.ReportPath = path;
            var Month = ddlmonth.SelectedValue;
           
            var param4 = new ReportParameter("Month", Month);
          
            var parameters = new List<ReportParameter>();

            parameters.Add(param4);
           
            ReportViewer1.LocalReport.SetParameters(parameters);




        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            
            ViewEmployeeBirthReport();
        }
        
}
}

