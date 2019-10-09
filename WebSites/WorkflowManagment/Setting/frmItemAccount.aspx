<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmItemAccount.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Setting.Views.frmItemAccount" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>


<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
    <asp:ValidationSummary ID="NewValidationSummary1" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="2" ForeColor="" />
    <asp:ValidationSummary ID="EditValidationSummary2" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="1" ForeColor="" />
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Search Chart of Accounts</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-6">
                                <asp:Label ID="lblItemAccountName" runat="server" Text="Account Name" CssClass="label"></asp:Label>
                                <label class="input">
                                    <asp:TextBox ID="txtItemAccountName" runat="server"></asp:TextBox></label>
                            </section>
                            <section class="col col-6">
                                <asp:Label ID="lblItemAccountCode" runat="server" Text="Account Code" CssClass="label"></asp:Label>
                                <label class="input">
                                    <asp:TextBox ID="txtItemAccountCode" runat="server"></asp:TextBox></label>
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
        <div runat="server" id="accountDiv" class="row" visible="true">
            <article class="col-xs-12 col-sm-12 col-md-12 col-lg-12 sortable-grid ui-sortable">
                <div class="jarviswidget jarviswidget-color-blueDark jarviswidget-sortable" id="wid-id-0" data-widget-editbutton="false" role="widget" style="">
                    <header role="heading">
                        <h2>Projects</h2>
                    </header>
                    <div role="content">
                        <div class="widget-body">
                            <asp:DataGrid ID="dgItemAccount" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                                CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                                GridLines="None"
                                OnCancelCommand="dgItemAccount_CancelCommand" OnDeleteCommand="dgItemAccount_DeleteCommand" OnEditCommand="dgItemAccount_EditCommand"
                                OnItemCommand="dgItemAccount_ItemCommand" OnItemDataBound="dgItemAccount_ItemDataBound" OnUpdateCommand="dgItemAccount_UpdateCommand"
                                ShowFooter="True" OnSelectedIndexChanged="dgItemAccount_SelectedIndexChanged">

                                <Columns>
                                    <asp:TemplateColumn HeaderText="Account Name">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "AccountName")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtItemAccountName" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "AccountName")%>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtItemAccountName" ErrorMessage="ItemAccount Name Required" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFItemAccountName" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="validator" ControlToValidate="txtFItemAccountName" ErrorMessage="ItemAccount Name Required" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Account Code">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "AccountCode")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtItemAccountCode" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "AccountCode")%>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="validator" ControlToValidate="txtItemAccountCode" ErrorMessage="ItemAccount Code Required" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFItemAccountCode" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="validator" ControlToValidate="txtFItemAccountCode" ErrorMessage="ItemAccount Code Required" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Actions">
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" ValidationGroup="1" CssClass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton ID="lnkAddNew" runat="server" CommandName="AddNew" ValidationGroup="2" CssClass="btn btn-sm btn-success"><i class="fa fa-save"></i></asp:LinkButton>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');"><i class="fa fa-times"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:ButtonColumn ButtonType="PushButton" CommandName="Select" Text="Checklists"></asp:ButtonColumn>
                                </Columns>
                                <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                            </asp:DataGrid>
                        </div>
                    </div>
                </div>
            </article>
        </div>
    </div>

    <asp:Panel ID="PnlChecklists" runat="server" Style="position: absolute; top: 10%; left: 20%;" Visible="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                </div>
                <div class="modal-body no-padding">
                    <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
                        <header>
                            <span class="widget-icon"><i class="fa fa-edit"></i></span>
                            <h2>Checklists</h2>
                        </header>
                        <div>
                            <div class="jarviswidget-editbox"></div>
                            <div class="widget-body no-padding">
                                <div class="smart-form">
                                    <asp:DataGrid ID="dgChecklists" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                                        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                                        GridLines="None"
                                        OnCancelCommand="dgChecklists_CancelCommand" OnDeleteCommand="dgChecklists_DeleteCommand" OnEditCommand="dgChecklists_EditCommand"
                                        OnItemCommand="dgChecklists_ItemCommand" OnItemDataBound="dgChecklists_ItemDataBound" OnUpdateCommand="dgChecklists_UpdateCommand"
                                        ShowFooter="True">

                                        <Columns>                                            
                                            <asp:TemplateColumn HeaderText="Checklist Name">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "ChecklistName")%>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtChecklistName" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "ChecklistName")%>'></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtChecklistName" ErrorMessage="Checklist Name is Required" ValidationGroup="ckleditg">*</asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtFChecklistName" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtFChecklistName" ErrorMessage="Checklist Name is Required" ValidationGroup="ckladdg">*</asp:RequiredFieldValidator>
                                                </FooterTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Actions">
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" ValidationGroup="ckleditg" CssClass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:LinkButton ID="lnkAddNew" runat="server" CommandName="AddNew" ValidationGroup="ckladdg" CssClass="btn btn-sm btn-success"><i class="fa fa-save"></i></asp:LinkButton>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');"><i class="fa fa-times"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                    </asp:DataGrid>

                                    <footer>
                                        <asp:Button ID="btnCancelChecklist" runat="server" CssClass="btn btn-primary" Text="Close" OnClick="btnCancelChecklist_Click" />
                                    </footer>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <!-- /.modal-content -->
    </asp:Panel>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="menuContent" runat="Server">
</asp:Content>

