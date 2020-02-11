<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmEmpLeaveSetting.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Setting.Views.frmEmpLeaveSetting" %>
 <%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>


<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">

    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
         <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Employee Leave Setting</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                                <section class="col col-4">
                                <asp:Label ID="lblSrchFullName" runat="server" Text="Full Name" CssClass="label"></asp:Label>
                                <label class="input">
                                    <asp:TextBox ID="txtSrchSrchFullName" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                        </div>
                     </fieldset>
                    <footer>
                        <asp:Button ID="btnFind" runat="server" Text="Find" CssClass="btn btn-primary" OnClick="btnFind_Click"></asp:Button>
                       <asp:Button ID="btnEnd" runat="server" CssClass="btn btn-default" Text="Set Ending Balance" ValidationGroup="Savedetail" OnClick="btnEnd_Click" Enabled="False"   />
                       <asp:Button ID="btnOpen" runat="server" CssClass="btn btn-primary" Text="Set Opening Balance" ValidationGroup="Savedetail" OnClick="btnOpen_Click"  />
                       <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                   
                                <div class="inline-group">
                                      <label class="checkbox">
                                    <asp:CheckBox ID="chkItems" runat="server" AutoPostBack="True" OnCheckedChanged="chkItems_CheckedChanged"/>
                                    <i></i>Select All</label>
                                                    </div>
                                              
                    </footer>
                </div>
            </div>
        </div>
         <asp:DataGrid ID="dgItemDetail" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                                        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                                        GridLines="None"  ShowFooter="True" OnItemDataBound="dgItemDetail_ItemDataBound" OnSelectedIndexChanged="dgItemDetail_SelectedIndexChanged">

                                        <Columns>
                                            <asp:TemplateColumn HeaderText="First Name">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "FirstName")%>
                                         
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                              <asp:TemplateColumn HeaderText="Last Name">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "LastName")%>
                                                </ItemTemplate>

                                            </asp:TemplateColumn>
                                           
                                            <asp:TemplateColumn HeaderText="Opening Leave Balance">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtOpeningLeavebalance" runat="server" CssClass="form-control" Text=' <%# String.Format("{0:F2}",DataBinder.Eval(Container.DataItem,"SDLeaveBalance"))%>' Height="20px" Width="104px"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtOpeningLeavebalance" ID="txtOpeningLeavebalance_FilteredTextBoxExtender" FilterType="Numbers,Custom" ValidChars="."></asp:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="RfvOpeningLeavebalance" runat="server" CssClass="validator" ControlToValidate="txtOpeningLeavebalance" ErrorMessage="Opening Leave balance Required" ValidationGroup="Savedetail" InitialValue="">*</asp:RequiredFieldValidator>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Opening Leave Balance Date">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtOpeningLeavebalancedate" runat="server" CssClass="form-control datepicker" data-dateformat="mm/dd/yy" Text=' <%# DataBinder.Eval(Container.DataItem,"LeaveSettingDate")%>' Height="20px" Width="104px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RfvOpeningLeavebalancedate" runat="server" CssClass="validator" ControlToValidate="txtOpeningLeavebalancedate" ErrorMessage="Opening Leave Balance Date Required" ValidationGroup="Savedetail" InitialValue="">*</asp:RequiredFieldValidator>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkselect" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                    </asp:DataGrid>
    
</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="menuContent" Runat="Server">
</asp:Content>

