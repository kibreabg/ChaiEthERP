<%@ Page Title="Vehicles Form" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmVehicles.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Setting.Views.frmVehicles" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
     <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
                     <header>
					        <span class="widget-icon"> <i class="fa fa-edit"></i> </span>
					        <h2>Find Vehicles</h2>				
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
        <asp:TextBox ID="txtPlate" runat="server" ></asp:TextBox></label></section>
                </div>
                          </fieldset>
                          <footer>                     
         <asp:Button ID="btnNew" runat="server" Text="Add New Vehicle" onclick="btnNew_Click" Cssclass="btn btn-primary" ></asp:Button>
        <asp:Button ID="btnFind" runat="server" Text="Find" onclick="btnFind_Click" Cssclass="btn btn-primary"></asp:Button>
                              </footer>
  </div>
   </div>
                                </div>
 
<asp:GridView ID="grvVehicle" runat="server" AllowPaging="True" CssClass="table table-striped table-bordered table-hover"  PagerStyle-CssClass="paginate_button active" AlternatingRowStyle-CssClass=""
            AutoGenerateColumns="False" CellPadding="3" ForeColor="#333333" 
            GridLines="Horizontal" Width="100%"          
             onpageindexchanging="grvVehicle_PageIndexChanging" OnRowDataBound="grvVehicle_RowDataBound">
            <PagerSettings FirstPageImageUrl="~/Images/arrow_beg.gif" 
                LastPageImageUrl="~/Images/arrow_end.gif" 
                NextPageImageUrl="~/Images/arrow_right.gif" 
                PreviousPageImageUrl="~/Images/arrow_left.gif" />
           
            <Columns>
                <asp:TemplateField HeaderText="PlateNo">
                <ItemTemplate>                
                <asp:Label ID="lblPlateNo" runat ="server" Text ='<%# DataBinder.Eval(Container.DataItem,"PlateNo") %>'></asp:Label>
                </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Brand">
                <ItemTemplate>                
                <asp:Label ID="lblBrand" runat ="server" Text ='<%# DataBinder.Eval(Container.DataItem,"Brand") %>'></asp:Label>
                </ItemTemplate>                
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Model">
                <ItemTemplate>                
                <asp:Label ID="lblModel" runat ="server" Text ='<%# DataBinder.Eval(Container.DataItem,"Model") %>'></asp:Label>
                </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="MakeYear">
                <ItemTemplate>                
                <asp:Label ID="lblMakeYear" runat ="server" Text ='<%# DataBinder.Eval(Container.DataItem,"MakeYear") %>'></asp:Label>
                </ItemTemplate>                
                </asp:TemplateField>
              
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink ID="hplEdit" runat="server" Text="Edit"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle Cssclass="paginate_button active" HorizontalAlign="Center" />
        </asp:GridView>
   
   </div>
        
  
</asp:Content>
