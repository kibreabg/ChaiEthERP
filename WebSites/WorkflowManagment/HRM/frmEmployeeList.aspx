
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmEmployeeList.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.HRM.Views.EmployeeList"
    Title="Default" MasterPageFile="~/Shared/ModuleMaster.master" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" Runat="Server">
		 <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
       <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Employee List</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-4">
                                <asp:Label ID="lblSrchEmpNo" runat="server" Text="Employee No" CssClass="label"></asp:Label>
                                <label class="input">
                                    <asp:TextBox ID="txtSrchEmpNo" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-4">
                                <asp:Label ID="lblSrchFullName" runat="server" Text="Full Name" CssClass="label"></asp:Label>
                                <label class="input">
                                     <asp:TextBox ID="txtSrchSrchFullName"  runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-4">
                                <asp:Label ID="lblSrchProgram" runat="server" Text="Project" CssClass="label"></asp:Label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlSrchSrchProgram" runat="server">
                                        <asp:ListItem Value="0">Select Project</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                </label>
                            </section>
                        </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnFind" runat="server" Text="Find" CssClass="btn btn-primary" OnClick="btnFind_Click"></asp:Button>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" CssClass="btn btn-primary"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>          
      
            <asp:GridView ID="GRVEmployeeList" runat="server" AutoGenerateColumns="False" CellPadding="3"
                DataKeyNames="Id" EnableModelValidation="True" ForeColor="#333333" GridLines="Horizontal"
                CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active"
                AlternatingRowStyle-CssClass="" OnRowDataBound="GRVEmployeeList_RowDataBound" Width="100%"
                Style="text-align: left" AllowPaging="True" OnPageIndexChanging="GRVEmployeeList_PageIndexChanging"
                Visible="True">
                <%--    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />--%>
                <Columns>
                    <asp:BoundField DataField="FirstName" HeaderText="First Name" />    
                    <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                    <asp:BoundField DataField="Gender" HeaderText="Gender" />
                     <asp:BoundField DataField="DateOfBirth" HeaderText="Date Of Birth" />
                    <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <div class="btn-group open">
                                        <asp:DropDownList ID="ddlAction" runat="server" class="btn dropdown-toggle btn-xs btn-success"
                                            data-toggle="dropdown" AppendDataBoundItems="False" AutoPostBack="True" OnSelectedIndexChanged="ddlAction_SelectedIndexChanged1">
                                            <asp:ListItem Value="0">Select Action</asp:ListItem>
                                            <asp:ListItem Value="1">Manage HR</asp:ListItem>
                                            <asp:ListItem Value="2">Preview</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
            </asp:GridView>
        </div>
</asp:Content>
