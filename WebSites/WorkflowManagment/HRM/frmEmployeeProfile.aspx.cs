using System;
using Microsoft.Practices.ObjectBuilder;

namespace Chai.WorkflowManagment.Modules.HRM.Views
{
	public partial class EmployeeProfile : POCBasePage, IEmployeeProfileView
    {
		private EmployeeProfilePresenter _presenter;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this._presenter.OnViewInitialized();
			}
			this._presenter.OnViewLoaded();
		}

		[CreateNew]
		public EmployeeProfilePresenter Presenter
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
                return "{F5FE9AB4-0AF8-432F-92B4-DFA2EEECE42B}";
            }
        }

    }
}

