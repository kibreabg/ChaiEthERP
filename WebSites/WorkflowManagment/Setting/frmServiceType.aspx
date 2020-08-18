<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmServiceType.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Setting.Views.frmServiceTypes" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script src="../js/libs/jquery-2.0.2.min.js"></script>
    <script type="text/javascript">

        function showDetailModal() {
            $(document).ready(function () {
                $('#detailModal').modal('show');
            });
        }
    </script>
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Search Service Types</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-6">
                                <asp:Label ID="lblServiceTypeName" runat="server" Text="Service Type Name" CssClass="label"></asp:Label>
                                <label class="input">
                                    <asp:TextBox ID="txtServiceTypeName" runat="server"></asp:TextBox></label>
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

        <div class="row">
            <article class="col-xs-12 col-sm-12 col-md-12 col-lg-12 sortable-grid ui-sortable">
                <div class="jarviswidget jarviswidget-color-blueDark jarviswidget-sortable" id="wid-id-0" data-widget-editbutton="false" role="widget" style="">
                    <header role="heading">
                        <h2>Service Types</h2>
                    </header>
                    <div role="content">
                        <div class="widget-body">
                            <asp:DataGrid ID="dgServiceType" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                                CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                                GridLines="None"
                                OnCancelCommand="dgServiceType_CancelCommand" OnDeleteCommand="dgServiceType_DeleteCommand" OnEditCommand="dgServiceType_EditCommand"
                                OnItemCommand="dgServiceType_ItemCommand" OnItemDataBound="dgServiceType_ItemDataBound" OnUpdateCommand="dgServiceType_UpdateCommand"
                                ShowFooter="True" OnSelectedIndexChanged="dgServiceType_SelectedIndexChanged">

                                <Columns>
                                    <asp:TemplateColumn HeaderText="Name">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Name")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEdtName" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "Name")%>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEdtName" runat="server" ControlToValidate="txtEdtName" ErrorMessage="Name is required" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFName" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFName" runat="server" CssClass="validator" ControlToValidate="txtFName" ErrorMessage="Name is required" ValidationGroup="save"></asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Description">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Description")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEdtDesc" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "Description")%>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEdtDesc" runat="server" CssClass="validator" ControlToValidate="txtEdtDesc" ErrorMessage="Description is required" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFDesc" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFDesc" runat="server" CssClass="validator" ControlToValidate="txtFDesc" ErrorMessage="Description is required" ValidationGroup="save"></asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="KM For Service">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "KmForService")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEdtKmForService" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "KmForService")%>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEdtKmForService" runat="server" CssClass="validator" ControlToValidate="txtEdtKmForService" ErrorMessage="Please enter KM" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                            <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtEdtKmForService" ID="ftbeEdtKmForService" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFKmForService" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFKmForService" runat="server" CssClass="validator" ControlToValidate="txtFKmForService" ErrorMessage="Please enter KM" ValidationGroup="save"></asp:RequiredFieldValidator>
                                            <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtFKmForService" ID="ftbeKmForService" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
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
                                    <asp:ButtonColumn ButtonType="PushButton" CommandName="Select" Text="Service Type Detail"></asp:ButtonColumn>
                                </Columns>
                                <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                            </asp:DataGrid>
                        </div>
                    </div>
                </div>
            </article>
        </div>
    </div>
    <div class="modal fade" id="detailModal" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                    <h4 class="modal-title" id="myModalLabel">Process Service Type Details</h4>
                </div>
                <div class="modal-body">
                    <div class="jarviswidget-editbox"></div>
                    <div class="widget-body no-padding">
                        <div class="smart-form">
                            <asp:DataGrid ID="dgServiceTypeDetail" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                                CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                                GridLines="None"
                                OnCancelCommand="dgServiceTypeDetail_CancelCommand" OnDeleteCommand="dgServiceTypeDetail_DeleteCommand" OnEditCommand="dgServiceTypeDetail_EditCommand"
                                OnItemCommand="dgServiceTypeDetail_ItemCommand" OnItemDataBound="dgServiceTypeDetail_ItemDataBound" OnUpdateCommand="dgServiceTypeDetail_UpdateCommand"
                                ShowFooter="True">

                                <Columns>
                                    <asp:TemplateColumn HeaderText="Description">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Description")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEdtDesc" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "Description")%>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEdtDesc" runat="server" CssClass="validator" ControlToValidate="txtEdtDesc" ErrorMessage="Description is Required" ValidationGroup="editDet"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFDesc" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFDesc" runat="server" CssClass="validator" ControlToValidate="txtFDesc" ErrorMessage="Description is Required" ValidationGroup="saveDet"></asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Status">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Status")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <div class="row" style="margin: 0 0px">
                                                <label class="checkbox">
                                                    <asp:CheckBox ID="ckEdtStatus" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "Status")%>' />
                                                    <i></i>
                                                </label>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <div class="row" style="margin: 0 0px">
                                                <label class="checkbox">
                                                    <asp:CheckBox ID="ckFStatus" runat="server" />
                                                    <i></i>
                                                </label>
                                            </div>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Actions">
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" ValidationGroup="editDet" CssClass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton ID="lnkAddNew" runat="server" CommandName="AddNew" ValidationGroup="saveDet" CssClass="btn btn-sm btn-success"><i class="fa fa-save"></i></asp:LinkButton>
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
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            </footer>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="menuContent" runat="Server">
</asp:Content>

