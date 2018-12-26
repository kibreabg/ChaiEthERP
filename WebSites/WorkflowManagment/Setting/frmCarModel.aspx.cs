using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using Microsoft.Practices.ObjectBuilder;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public partial class frmCarModel : POCBasePage, ICarModelView
    {
        private CarModelPresenter _presenter;
        private IList<CarModel> _CarModels;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindCarModels();
            }

            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public CarModelPresenter Presenter
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
                return "{94C874DF-15C7-4978-A8C7-80221EE83AAD}";
            }
        }

        #region Field Getters
        public string GetName
        {
            get { return txtSrchCarModelName.Text; }
        }
        public IList<CarModel> CarModels
        {
            get
            {
                return _CarModels;
            }
            set
            {
                _CarModels = value;
            }
        }
        #endregion
        void BindCarModels()
        {
            dgCarRental.DataSource = _presenter.ListCarModels(GetName);
            dgCarRental.DataBind();
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            //_presenter.ListCarRentals(GetName);
            BindCarModels();
        }
        protected void dgCarRental_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgCarRental.EditItemIndex = -1;
        }
        protected void dgCarRental_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgCarRental.DataKeys[e.Item.ItemIndex];
            CarModel CarModel = _presenter.GetCarModelById(id);
            try
            {
               
                _presenter.SaveOrUpdateCarModel(CarModel);
                
                BindCarModels();

                Master.ShowMessage(new AppMessage("Car Model was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Car Model. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgCarRental_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            CarModel CarModel = new CarModel();
            if (e.CommandName == "AddNew")
            {
                try
                {

                    TextBox txtCarModelName = e.Item.FindControl("txtCarModelName") as TextBox;
                    CarModel.ModelName = txtCarModelName.Text;
                    TextBox txtYear = e.Item.FindControl("txtYear") as TextBox;
                    CarModel.ManufacturedYear = Convert.ToInt32(txtYear.Text);
                    TextBox txtDescription = e.Item.FindControl("txtDescription") as TextBox;
                    CarModel.Description = txtDescription.Text;
                    SaveCarModel(CarModel);
                    dgCarRental.EditItemIndex = -1;
                    BindCarModels();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Car Model " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }
        private void SaveCarModel(CarModel CarModel)
        {
            try
            {
                if (CarModel.Id <= 0)
                {
                    _presenter.SaveOrUpdateCarModel(CarModel);
                    Master.ShowMessage(new AppMessage("Car Model saved", RMessageType.Info));
                    //_presenter.CancelPage();
                }
                else
                {
                    _presenter.SaveOrUpdateCarModel(CarModel);
                    Master.ShowMessage(new AppMessage("Car Model Updated", RMessageType.Info));
                    // _presenter.CancelPage();
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
            }
        }
        protected void dgCarRental_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgCarRental.EditItemIndex = e.Item.ItemIndex;

            BindCarModels();
        }
        protected void dgCarRental_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void dgCarRental_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int id = (int)dgCarRental.DataKeys[e.Item.ItemIndex];
            CarModel CarModel = _presenter.GetCarModelById(id);

            try
            {
                TextBox txtCarModelName = e.Item.FindControl("txtEdtCarModelName") as TextBox;
                CarModel.ModelName = txtCarModelName.Text;
                TextBox txtYear = e.Item.FindControl("txtEdtYear") as TextBox;
                CarModel.ManufacturedYear = Convert.ToInt32(txtYear.Text);
                TextBox txtDescription = e.Item.FindControl("txtEdtDescription") as TextBox;
                CarModel.Description = txtDescription.Text;
                SaveCarModel(CarModel);
                dgCarRental.EditItemIndex = -1;
                BindCarModels();


               
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Car Rental. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }        
    }
}