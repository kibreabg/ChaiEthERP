<%@ Page Title="Holiday Setting" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmHoliday.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Setting.Views.frmHoliday" %>
 <%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>


<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" Runat="Server">
      <asp:ValidationSummary ID="NewValidationSummary1" runat="server" 
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph" 
        ValidationGroup="2" ForeColor="" />
        <asp:ValidationSummary ID="EditValidationSummary2" runat="server" 
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph" 
        ValidationGroup="1" ForeColor="" />
   <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
          
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Holiday Setting</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    
                    <footer>
                        
                     <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    
                    </footer>
                </div>
            </div>
        </div>
		

    <asp:DataGrid ID="dgHoliday" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0" 
        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id" 
         GridLines="None"
        oncancelcommand="dgHoliday_CancelCommand" ondeletecommand="dgHoliday_DeleteCommand" oneditcommand="dgHoliday_EditCommand" 
        onitemcommand="dgHoliday_ItemCommand" onitemdatabound="dgHoliday_ItemDataBound" onupdatecommand="dgHoliday_UpdateCommand" 
         ShowFooter="True">
   
        <Columns>
            <asp:TemplateColumn HeaderText="Holiday">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "HolidayName")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtHolidayName" runat="server" Cssclass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "HolidayName")%>'></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="validator" ControlToValidate="txtHolidayName" ErrorMessage="Holiday Required" ValidationGroup="1">*</asp:RequiredFieldValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="txtFHolidayName" runat="server" Cssclass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="validator" ControlToValidate="txtFHolidayName" ErrorMessage="Holiday Required" ValidationGroup="2">*</asp:RequiredFieldValidator>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Date">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Date")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control datepicker" data-dateformat="mm/dd/yy" Text=' <%# DataBinder.Eval(Container.DataItem, "Date")%>'></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="validator" ControlToValidate="txtDate" ErrorMessage="Date Required" ValidationGroup="1">*</asp:RequiredFieldValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="txtFDate" runat="server" CssClass="form-control datepicker" data-dateformat="mm/dd/yy"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="validator" ControlToValidate="txtFDate" ErrorMessage="Date Required" ValidationGroup="2">*</asp:RequiredFieldValidator>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn  HeaderText="Actions">
                <EditItemTemplate>
                    <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" ValidationGroup="1" Cssclass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                   <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Cssclass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:LinkButton ID="lnkAddNew" runat="server" CommandName="AddNew" ValidationGroup="2" Cssclass="btn btn-sm btn-success"><i class="fa fa-save"></i></asp:LinkButton>
                </FooterTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Cssclass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Cssclass="btn btn-xs btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');" ><i class="fa fa-times"></i></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
           <PagerStyle  Cssclass="paginate_button active" HorizontalAlign="Center" />
    </asp:DataGrid>
    </div>
		
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="menuContent" Runat="Server">
</asp:Content>

