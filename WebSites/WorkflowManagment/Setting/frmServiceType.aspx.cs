using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using Microsoft.Practices.ObjectBuilder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public partial class frmServiceTypes : POCBasePage, IServiceTypeView
    {
        private ServiceTypePresenter _presenter;
        private IList<ServiceType> _serviceTypes;
        private ServiceType _serviceType;
        private int selectedServiceTypeId;
        private int ServiceTypeId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindServiceTypes();
            }

            this._presenter.OnViewLoaded();
            _serviceType = Session["ServiceType"] as ServiceType;
            //  ProgramId = (int)Session["ProgramId"] ;
        }

        [CreateNew]
        public ServiceTypePresenter Presenter
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
        #region Field Getters
        public string ServiceTypeName
        {
            get { return txtServiceTypeName.Text; }
        }
        public IList<ServiceType> ServiceTypes
        {
            get
            {
                return _serviceTypes;
            }
            set
            {
                _serviceTypes = value;
            }
        }
        #endregion
        public override string PageID
        {
            get
            {
                return "{A41DB11B-C51B-4277-9003-75FD320FBDE5}";
            }
        }
        private void BindServiceTypes()
        {
            dgServiceType.DataSource = _presenter.ListServiceTypes(txtServiceTypeName.Text);
            dgServiceType.DataBind();
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            _presenter.ListServiceTypes(ServiceTypeName);
            BindServiceTypes();
        }
        protected void dgServiceType_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgServiceType.EditItemIndex = -1;
            this.BindServiceTypes();
        }
        protected void dgServiceType_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgServiceType.DataKeys[e.Item.ItemIndex];
            ServiceType serviceType = _presenter.GetServiceType(id);
            try
            {
                _presenter.DeleteServiceType(serviceType);
                BindServiceTypes();

                Master.ShowMessage(new AppMessage("ServiceType was Removed Successfully", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Service Type. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgServiceType_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            ServiceType serviceType = new ServiceType();
            if (e.CommandName == "AddNew")
            {
                try
                {
                    TextBox txtFName = e.Item.FindControl("txtFName") as TextBox;
                    serviceType.Name = txtFName.Text;
                    TextBox txtFDesc = e.Item.FindControl("txtFDesc") as TextBox;
                    serviceType.Description = txtFDesc.Text;
                    TextBox txtFKmForService = e.Item.FindControl("txtFKmForService") as TextBox;
                    serviceType.KmForService = Convert.ToInt32(txtFKmForService.Text);

                    SaveServiceType(serviceType);
                    dgServiceType.EditItemIndex = -1;
                    BindServiceTypes();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to add Service Type " + ex.Message, RMessageType.Error));
                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
                }
            }
        }
        protected void dgServiceType_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgServiceType.EditItemIndex = e.Item.ItemIndex;
            BindServiceTypes();
        }
        protected void dgServiceType_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {

            }
            else
            {
                if (_serviceTypes != null)
                {
                    
                }
            }
        }
        protected void dgServiceType_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgServiceType.DataKeys[e.Item.ItemIndex];
            ServiceType serviceType = _presenter.GetServiceType(id);
            try
            {
                TextBox txtEdtName = e.Item.FindControl("txtEdtName") as TextBox;
                serviceType.Name = txtEdtName.Text;
                TextBox txtEdtDesc = e.Item.FindControl("txtEdtDesc") as TextBox;
                serviceType.Description = txtEdtDesc.Text;
                TextBox txtEdtKmForService = e.Item.FindControl("txtEdtKmForService") as TextBox;
                serviceType.KmForService = Convert.ToInt32(txtEdtKmForService.Text);

                _presenter.SaveOrUpdateServiceType(serviceType);
                Master.ShowMessage(new AppMessage("Service Type Updated Successfully.", RMessageType.Info));
                dgServiceType.EditItemIndex = -1;
                BindServiceTypes();
            }

            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Service Type Detail. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgServiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ServiceTypeId = (int)dgServiceType.DataKeys[dgServiceType.SelectedIndex];
            Session["ServiceTypeId"] = ServiceTypeId;
            dgServiceType.SelectedItemStyle.BackColor = System.Drawing.Color.BurlyWood;
            _serviceType = _presenter.GetServiceType(ServiceTypeId);
            Session["ServiceType"] = _serviceType;
            ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
            BindServiceTypeDetails();
        }
        private void SaveServiceType(ServiceType ServiceType)
        {
            try
            {
                if (ServiceType.Id <= 0)
                {
                    _presenter.SaveOrUpdateServiceType(ServiceType);
                    Master.ShowMessage(new AppMessage("Service Type saved", RMessageType.Info));
                }
                else
                {
                    _presenter.SaveOrUpdateServiceType(ServiceType);
                    Master.ShowMessage(new AppMessage("Service Type Updated", RMessageType.Info));
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        #region Service Type Detail
        private void BindServiceTypeDetails()
        {
            dgServiceTypeDetail.DataSource = _serviceType.ServiceTypeDetails;
            dgServiceTypeDetail.DataBind();
        }
        protected void dgServiceTypeDetail_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgServiceTypeDetail.EditItemIndex = -1;
            BindServiceTypeDetails();
        }
        protected void dgServiceTypeDetail_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgServiceTypeDetail.DataKeys[e.Item.ItemIndex];

            try
            {
                _presenter.DeleteServiceTypeDetail(_presenter.GetServiceTypeDetail(id));
                _presenter.SaveOrUpdateServiceType(_serviceType);

                BindServiceTypeDetails();
                ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
                Master.ShowMessage(new AppMessage("Service Type Detail was Removed Successfully", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Service Type Detail. " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        protected void dgServiceTypeDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgServiceTypeDetail.EditItemIndex = e.Item.ItemIndex;
            BindServiceTypeDetails();
            ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
        }
        protected void dgServiceTypeDetail_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "AddNew")
            {
                try
                {
                    ServiceTypeDetail stDetail = new ServiceTypeDetail();
                    TextBox txtFDesc = e.Item.FindControl("txtFDesc") as TextBox;
                    stDetail.Description = txtFDesc.Text;
                    CheckBox ckFStatus = e.Item.FindControl("ckFStatus") as CheckBox;
                    stDetail.Status = ckFStatus.Checked;
                    stDetail.ServiceType = _serviceType;

                    _serviceType.ServiceTypeDetails.Add(stDetail);
                    _presenter.SaveOrUpdateServiceType(_serviceType);
                    Master.ShowMessage(new AppMessage("Service Type Detail Added Successfully.", RMessageType.Info));
                    dgServiceTypeDetail.EditItemIndex = -1;
                    BindServiceTypeDetails();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Service Type Detail. " + ex.Message, RMessageType.Error));
                    ExceptionUtility.LogException(ex, ex.Source);
                    ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
                }
            }
        }
        protected void dgServiceTypeDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
            }
            else
            {
            }
        }
        protected void dgServiceTypeDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgServiceTypeDetail.DataKeys[e.Item.ItemIndex];
            ServiceTypeDetail stDetail = _serviceType.GetServiceTypeDetail(id);
            try
            {
                TextBox txtEdtDesc = e.Item.FindControl("txtEdtDesc") as TextBox;
                stDetail.Description = txtEdtDesc.Text;
                CheckBox ckEdtStatus = e.Item.FindControl("ckEdtStatus") as CheckBox;
                stDetail.Status = ckEdtStatus.Checked;

                _presenter.SaveOrUpdateServiceType(_serviceType);
                Master.ShowMessage(new AppMessage("Service Type Detail Updated Successfully.", RMessageType.Info));
                dgServiceTypeDetail.EditItemIndex = -1;
                BindServiceTypeDetails();
                ScriptManager.RegisterStartupScript(this, GetType(), "showDetailModal", "showDetailModal();", true);
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Service Type Detail . " + ex.Message, RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex, _presenter.CurrentUser().FullName);
            }
        }
        #endregion   
    }
}