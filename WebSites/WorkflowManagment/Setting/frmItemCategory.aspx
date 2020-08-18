<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmItemCategory.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Setting.Views.frmItemCategory" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>


<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
    <asp:ValidationSummary ID="vsEdit" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="edit" ForeColor="" />
    <asp:ValidationSummary ID="vsSave" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="save" ForeColor="" />
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Search Item Categories</h2>
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
        <div runat="server" id="categoryDiv" class="row" visible="true">
            <article class="col-xs-12 col-sm-12 col-md-12 col-lg-12 sortable-grid ui-sortable">
                <div class="jarviswidget jarviswidget-color-blueDark jarviswidget-sortable" id="wid-id-0" data-widget-editbutton="false" role="widget" style="">
                    <header role="heading">
                        <h2>Item Categories</h2>
                    </header>
                    <div role="content">
                        <div class="widget-body">
                            <asp:DataGrid ID="dgItemCategory" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                                CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                                GridLines="None"
                                OnCancelCommand="dgItemCategory_CancelCommand" OnDeleteCommand="dgItemCategory_DeleteCommand" OnEditCommand="dgItemCategory_EditCommand"
                                OnItemCommand="dgItemCategory_ItemCommand" OnItemDataBound="dgItemCategory_ItemDataBound" OnUpdateCommand="dgItemCategory_UpdateCommand"
                                ShowFooter="True" OnSelectedIndexChanged="dgItemCategory_SelectedIndexChanged">

                                <Columns>
                                    <asp:TemplateColumn HeaderText="Category Name">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Name") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEdtName" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "Name")%>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEdtName" runat="server" ControlToValidate="txtEdtName" ErrorMessage="Category Name is required" ValidationGroup="edit">*</asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFName" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFName" runat="server" CssClass="validator" ControlToValidate="txtFName" ErrorMessage="Category Name is required" ValidationGroup="save">*</asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Category Code">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Code") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEdtCode" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "Code")%>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEdtCode" runat="server" CssClass="validator" ControlToValidate="txtEdtCode" ErrorMessage="Category Code is required" ValidationGroup="edit">*</asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFCode" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFCode" runat="server" CssClass="validator" ControlToValidate="txtFCode" ErrorMessage="Category Code is required" ValidationGroup="save">*</asp:RequiredFieldValidator>
                                        </FooterTemplate>
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

