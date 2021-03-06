﻿using System;
using Microsoft.Practices.ObjectBuilder;

namespace Chai.WorkflowManagment.Modules.HRM.Views
{
	public partial class Default : POCBasePage, IDefaultView
	{
		private DefaultPresenter _presenter;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this._presenter.OnViewInitialized();
			}
			this._presenter.OnViewLoaded();
		}

		[CreateNew]
		public DefaultPresenter Presenter
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

	
	}
}

