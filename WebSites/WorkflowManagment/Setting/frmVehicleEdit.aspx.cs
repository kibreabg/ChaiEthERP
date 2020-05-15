using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Shared.Settings;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public partial class frmVehicleEdit : POCBasePage, IVehicleEditView
    {
        private VehicleEditPresenter _presenter;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();

                //PopProgram();
                PopDriver();
                BindVehicleControls();
                
            }
            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public VehicleEditPresenter Presenter
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
        private void PopDriver()
        {
           ddlDriver.Items.Add(new ListItem("None", "0"));
            ddlDriver.DataSource = _presenter.GetDrivers();
            ddlDriver.DataBind();
        }
       
      
        private void BindVehicleControls()
        {
            this.txtModel.Text = _presenter.CurrentVehicle.Model;
            this.txtPlate.Text = _presenter.CurrentVehicle.PlateNo;
            this.txtBrand.Text = _presenter.CurrentVehicle.Brand;
            this.txtMakeYear.Text = Convert.ToInt32(_presenter.CurrentVehicle.MakeYear).ToString();
            this.txtPurchaseYear.Text = Convert.ToInt32(_presenter.CurrentVehicle.PurchaseYear).ToString();
            this.txtTransmission.Text = _presenter.CurrentVehicle.Transmission;
            this.txtEngineType.Text = _presenter.CurrentVehicle.EngineType;
            this.txtEngineCapacity.Text = _presenter.CurrentVehicle.EngineCapacity;
            this.txtLastKmReading.Text = Convert.ToInt32(_presenter.CurrentVehicle.LastKmReading).ToString();
            this.txtBodyType.Text = _presenter.CurrentVehicle.BodyType;
            txtFrameNumber.Text = _presenter.CurrentVehicle.FrameNo;
            this.ddlDriver.SelectedValue = _presenter.CurrentVehicle.AppUser != null ? _presenter.CurrentVehicle.AppUser.Id.ToString():"0";
           
            this.ddlStatus.SelectedValue = _presenter.CurrentVehicle.Status!= null? _presenter.CurrentVehicle.Status.ToString():"";
           
            this.btnDelete.Visible = (_presenter.CurrentVehicle.Id > 0);
            this.btnDelete.Attributes.Add("onclick", "return confirm(\"Are you sure you want to delete this Vehicle?\")");
        }

      

      
      


        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                   
                    if (_presenter.CurrentVehicle.Id == 0)
                    {
                        _presenter.SaveOrUpdateVehicle();
                        Master.TransferMessage(new AppMessage("Vehicle  is Saved successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
                        _presenter.RedirectPage(String.Format("~/Setting/frmVehicleEdit.aspx?{0}=0&{1}={2}"));
                    }
                    else
                    {
                        _presenter.SaveOrUpdateVehicle();
                        Master.ShowMessage(new AppMessage("Vehicle saved", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    }
                }
                catch (Exception ex)
                {
                    
                    Master.ShowMessage(new AppMessage("Error: " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }

        #region IUserEditView Members

       
        public int GetVehicleId
        {
           

            get
            {
                if (Request.QueryString["GetId"] != "")

                    return Convert.ToInt32(Request.QueryString["GetId"]);
                else
                    return 0;
            }
        }

        public string GetPlateNo
        {
            get
            {
                return this.txtPlate.Text;
            }
        }

        public string GetBrand
        {
            get
            {
                return this.txtBrand.Text;
            }
        }

        public string GetModel
        {
            get
            {
                return this.txtModel.Text;
            }
        }

        public int GetMakeYear
        {
            get
            {
                return Convert.ToInt32(this.txtMakeYear.Text);
            }
        }

        public string GetFrameNumber
        {
            get
            {
                return this.txtFrameNumber.Text;
            }
        }

        public string GetEngineType
        {
            get
            {
                return this.txtEngineType.Text;
            }
        }

        public string GetTransmission
        {
            get
            {
                return this.txtTransmission.Text;
            }
        }

        public string GetBodyType
        {
            get
            {
                return this.txtBodyType.Text;
            }
        }

        public string GetEngineCapacity
        {
            get
            {
                return this.txtEngineCapacity.Text;
            }
        }

        public int GetPurchaseYear
        {
            get
            {
                return Convert.ToInt32(this.txtPurchaseYear.Text);
            }
        }

        public int GetLastKmReading
        {
            get
            {
                return Convert.ToInt32(this.txtLastKmReading.Text);
            }
        }

        public AppUser AppUser
        {
            get
            {
                return _presenter.GetDriver(int.Parse(ddlDriver.SelectedValue));
            }
        }

        public string GetStatus
        {
            get
            {
                return this.ddlStatus.Text;
            }
        }
        #endregion

     
                
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            _presenter.CancelPage();
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {


            
            try
            {
                _presenter.CurrentVehicle.Status = "In Active";
                _presenter.DeleteVehicle();
                Master.TransferMessage(new AppMessage("Vehicle was Deactivate", Chai.WorkflowManagment.Enums.RMessageType.Info));
                _presenter.CancelPage();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }


      
    }
}

