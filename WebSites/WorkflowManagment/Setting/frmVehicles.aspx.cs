using System;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.CoreDomain.Setting;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public partial class frmVehicles : POCBasePage, IVehiclesView
    {
        private VehiclesPresenter _presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
            }
            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public VehiclesPresenter Presenter
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
                return "{7B51A5EE-B7D6-449B-ABE6-BD6DBE0C337E}";
            }
        }
        public void BindVehicless()
        {
            
            this.grvVehicle.DataSource = _presenter.SearchVehicle(txtPlate.Text.Trim());
            this.grvVehicle.DataBind();
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindVehicless();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            _presenter.AddNewVehicle();
        }
        
      

      


        protected void grvVehicle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvVehicle.PageIndex = e.NewPageIndex;
            BindVehicless();
        }



   


     

        protected void grvVehicle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           Vehicle vec = e.Row.DataItem as Vehicle;
            if (vec != null)
            {
                HyperLink hpl = e.Row.FindControl("hplEdit") as HyperLink;
              
                

                string url = string.Format(String.Format("../Setting/frmVehicleEdit.aspx?GetId={0}", vec.Id));
                hpl.NavigateUrl = this.ResolveUrl(url);
                // Do something with grid view row here
            }
        }
    }
}

