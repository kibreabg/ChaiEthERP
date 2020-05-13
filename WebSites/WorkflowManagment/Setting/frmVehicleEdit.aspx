<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmVehicleEdit.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Setting.Views.frmVehicleEdit"
    Title="VehicleEdit" MasterPageFile="~/Shared/ModuleMaster.master" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>




<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Vehicle</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-6">

                                <label class="label">Plate No</label>
                                <label class="input">
                                    <asp:TextBox ID="txtPlate" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvPlate" runat="server" ControlToValidate="txtPlate"
                                        Display="Dynamic" ErrorMessage="Plate Number is required" CssClass="validator"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <cc1:ValidatorCalloutExtender ID="rfvPlate_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="rfvPlate" Width="300px">
                                    </cc1:ValidatorCalloutExtender>
                                    <asp:Label ID="lblPlate" runat="server" Visible="False"></asp:Label></label>
                            </section>
                       
                            <section class="col col-6">
                                <label class="label">Brand</label>
                                <label class="input">

                                    <asp:TextBox ID="txtBrand" runat="server"></asp:TextBox>
                                  <asp:RequiredFieldValidator ID="rfvBrand" runat="server" ControlToValidate="txtBrand"
                                        Display="Dynamic" ErrorMessage="Brand is required" CssClass="validator"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <cc1:ValidatorCalloutExtender ID="rfvBrand_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="rfvBrand" Width="300px">
                                    </cc1:ValidatorCalloutExtender>
                                </label>
                            </section>
                        </div>
                        
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Model</label>
                                <label class="input">
                                     
                                    <asp:TextBox ID="txtModel" runat="server"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="rfvModel" runat="server" ControlToValidate="txtModel"
                                        Display="Dynamic" ErrorMessage="Model is required" CssClass="validator"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <cc1:ValidatorCalloutExtender ID="rfvModel_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="rfvModel" Width="300px">
                                    </cc1:ValidatorCalloutExtender>
                                </label>
                            </section>
                       
                            <section class="col col-6">
                                <label class="label">Make Year</label>
                                <label class="input">
                                    <asp:TextBox ID="txtMakeYear" runat="server"></asp:TextBox></label>
                              <asp:RequiredFieldValidator ID="rfvMakeYear" runat="server" ControlToValidate="txtMakeYear"
                                        Display="Dynamic" ErrorMessage="Make Year is required" CssClass="validator"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender1"
                                        runat="server" Enabled="True" TargetControlID="rfvMakeYear" Width="300px">
                                    </cc1:ValidatorCalloutExtender>
                            </section>
                        </div>
                              <div class="row">
                            <section class="col col-6">
                                <label class="label">Frame Number</label>
                               <label class="input">
                                     
                                    <asp:TextBox ID="txtFrameNumber" runat="server"></asp:TextBox></label>
                                
                            </section>
                        
                            <section class="col col-6">
                                <label class="label">Driver</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlDriver" runat="server" AppendDataBoundItems="True" DataTextField="FirstName" DataValueField="Id">
                                        <asp:ListItem Value="0">Select Driver</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                    <asp:RequiredFieldValidator ID="rfvDriver" runat="server" ControlToValidate="ddlDriver"
                                        Display="Dynamic" ErrorMessage="Driver is required" CssClass="validator"
                                        SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>
                                    <cc1:ValidatorCalloutExtender ID="rfvDriver_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="rfvDriver" Width="300px">
                                    </cc1:ValidatorCalloutExtender>
                                </label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Engine Type</label>
                                  <label class="input">
                                     
                                    <asp:TextBox ID="txtEngineType" runat="server"></asp:TextBox></label>
                               
                              
                            </section>
                       
                            <section class="col col-6">
                                <label class="label">Transmission</label>
                                <label class="input">
                                    <asp:TextBox ID="txtTransmission" runat="server"></asp:TextBox>
                                    
                                   
                                </label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">BodyType</label>
                                <label class="input">
                                    <asp:TextBox ID="txtBodyType" runat="server"></asp:TextBox>
                                  
                                </label>
                            </section>
                       
                            <section class="col col-6">
                                <label class="label">Engine Capacity</label>
                                <label class="input">
                                     
                                    <asp:TextBox ID="txtEngineCapacity" runat="server"></asp:TextBox></label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Purchase Year</label>
                                <label class="input">
                                    <asp:TextBox ID="txtPurchaseYear" runat="server"></asp:TextBox></label>
                            </section>
                     
                            <section class="col col-6">
                                <label class="label">Last Km Reading</label>
                                <label class="input">
                                     
                                    <asp:TextBox ID="txtLastKmReading" runat="server"></asp:TextBox></label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Status</label>
                                 <label class="select">
                                    <asp:DropDownList ID="ddlStatus" runat="server" AppendDataBoundItems="True" >
                                        <asp:ListItem>Active</asp:ListItem>
                                        <asp:ListItem>In Active</asp:ListItem>
                                        
                                    </asp:DropDownList><i></i>
                                    
                                </label>
                            </section>
                        </div>                      
                        
                    </fieldset>
                 
                    <footer>
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" class="btn btn-primary"></asp:Button>
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False"
                            OnClick="btnCancel_Click" class="btn btn-primary"></asp:Button>
                        <asp:Button ID="btnDelete" runat="server" Text="Deactivate" OnClick="btnDelete_Click" Visible="False" class="btn btn-danger"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
