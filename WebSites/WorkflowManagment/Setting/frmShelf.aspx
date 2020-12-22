<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmShelf.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Setting.Views.frmShelf" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Search Shelf</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-6">
                                <asp:Label ID="lblName" runat="server" Text="Name" CssClass="label"></asp:Label>
                                <label class="input">
                                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox></label>
                            </section>
                            <section class="col col-6">
                                <asp:Label ID="lblCode" runat="server" Text="Code" CssClass="label"></asp:Label>
                                <label class="input">
                                    <asp:TextBox ID="txtCode" runat="server"></asp:TextBox></label>
                            </section>
                        </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" CssClass="btn btn-primary"></asp:Button>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>
        <div runat="server" id="shelfDiv" class="row" visible="true">
            <article class="col-xs-12 col-sm-12 col-md-12 col-lg-12 sortable-grid ui-sortable">
                <div class="jarviswidget jarviswidget-color-blueDark jarviswidget-sortable" id="wid-id-0" data-widget-editbutton="false" role="widget" style="">
                    <header role="heading">
                        <h2>Shelf</h2>
                    </header>
                    <div role="content">
                        <div class="widget-body">
                            <asp:DataGrid ID="dgShelf" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                                CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                                GridLines="None"
                                OnCancelCommand="dgShelf_CancelCommand" OnDeleteCommand="dgShelf_DeleteCommand" OnEditCommand="dgShelf_EditCommand"
                                OnItemCommand="dgShelf_ItemCommand" OnItemDataBound="dgShelf_ItemDataBound" OnUpdateCommand="dgShelf_UpdateCommand"
                                ShowFooter="True" OnSelectedIndexChanged="dgShelf_SelectedIndexChanged">

                                <Columns>
                                     <asp:TemplateColumn HeaderText="Store">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlStore" runat="server" CssClass="form-control"
                                            AppendDataBoundItems="True" DataTextField="Name" DataValueField="Id"
                                            ValidationGroup="edit" AutoPostBack="True" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Select Store</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RfvStore" runat="server" CssClass="validator"
                                            ControlToValidate="ddlStore" ErrorMessage="Store Required"
                                            InitialValue="0" SetFocusOnError="True" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlFStore" runat="server" CssClass="form-control"
                                            AppendDataBoundItems="True" DataTextField="Name" DataValueField="Id"
                                            EnableViewState="true" ValidationGroup="save" AutoPostBack="True" OnSelectedIndexChanged="ddlFStore_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Select Store</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RfvFStore" runat="server" CssClass="validator"
                                            ControlToValidate="ddlFStore" Display="Dynamic"
                                            ErrorMessage="Store Required" InitialValue="0" SetFocusOnError="True"
                                            ValidationGroup="save"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "Section.Store.Name")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Section">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Section.Name") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlEdtSection" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                                <asp:ListItem Value="0">Select Section</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvEdtSection" runat="server" CssClass="validator" ControlToValidate="ddlEdtSection" ErrorMessage="Section Required" ValidationGroup="edit" InitialValue="0"></asp:RequiredFieldValidator>
                                            <i></i>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlSection" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                                <asp:ListItem Value="0">Select Section</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvSection" runat="server" CssClass="validator" ControlToValidate="ddlSection" ErrorMessage="Section Required" ValidationGroup="save" InitialValue="0"></asp:RequiredFieldValidator>
                                            <i></i>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Shelf Name">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Name") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEdtName" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "Name")%>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEdtName" runat="server" ControlToValidate="txtEdtName" ErrorMessage="Category Name is required" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFName" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFName" runat="server" CssClass="validator" ControlToValidate="txtFName" ErrorMessage="Category Name is required" ValidationGroup="save"></asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Code">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Code") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEdtCode" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "Code")%>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEdtCode" runat="server" CssClass="validator" ControlToValidate="txtEdtCode" ErrorMessage="Category Code is required" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFCode" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFCode" runat="server" CssClass="validator" ControlToValidate="txtFCode" ErrorMessage="Category Code is required" ValidationGroup="save"></asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                  
                                    <asp:TemplateColumn HeaderText="Status">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlEdtStatus" runat="server" CssClass="form-control"
                                                AppendDataBoundItems="True"
                                                ValidationGroup="edit">
                                                <asp:ListItem Value="">Select Status</asp:ListItem>
                                                <asp:ListItem Value="Active">Active</asp:ListItem>
                                                <asp:ListItem Value="InActive">InActive</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RfvStatus" runat="server" CssClass="validator"
                                                ControlToValidate="ddlEdtStatus" ErrorMessage="Status Required"
                                                InitialValue="" SetFocusOnError="True" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlFStatus" runat="server" CssClass="form-control"
                                                AppendDataBoundItems="True" EnableViewState="true" ValidationGroup="save">
                                                <asp:ListItem Value="">Select Status</asp:ListItem>
                                                <asp:ListItem Value="Active">Active</asp:ListItem>
                                                <asp:ListItem Value="InActive">InActive</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RfvFStatus" runat="server" CssClass="validator"
                                                ControlToValidate="ddlFStatus" Display="Dynamic"
                                                ErrorMessage="Status Required" InitialValue="" SetFocusOnError="True"
                                                ValidationGroup="save"></asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Status")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Actions">
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" ValidationGroup="edit" CssClass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton ID="lnkAddNew" runat="server" CommandName="AddNew" ValidationGroup="save" CssClass="btn btn-sm btn-success"><i class="fa fa-save"></i></asp:LinkButton>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');"><i class="fa fa-times"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                            </asp:DataGrid>
                        </div>
                    </div>
                </div>
            </article>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="menuContent" runat="Server">
</asp:Content>

