<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmItem.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Setting.Views.frmItem" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Search Items</h2>
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
                        <h2>Items</h2>
                    </header>
                    <div role="content">
                        <div class="widget-body">
                            <asp:DataGrid ID="dgItem" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                                CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                                GridLines="None"
                                OnCancelCommand="dgItem_CancelCommand" OnDeleteCommand="dgItem_DeleteCommand" OnEditCommand="dgItem_EditCommand"
                                OnItemCommand="dgItem_ItemCommand" OnItemDataBound="dgItem_ItemDataBound" OnUpdateCommand="dgItem_UpdateCommand"
                                ShowFooter="True" OnSelectedIndexChanged="dgItem_SelectedIndexChanged">

                                <Columns>
                                    <asp:TemplateColumn HeaderText="Item Sub Category">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "ItemSubCategory.Name") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlEdtItemSubCategory" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                                <asp:ListItem Value="0">Select Item Sub Category</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvEdtItem" runat="server" CssClass="validator" ControlToValidate="ddlEdtItemSubCategory" ErrorMessage="Item Sub Category Required" ValidationGroup="edit" InitialValue="0"></asp:RequiredFieldValidator>
                                            <i></i>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlItemSubCategory" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                                <asp:ListItem Value="0">Select Item Sub Category</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvItem" runat="server" CssClass="validator" ControlToValidate="ddlItemSubCategory" ErrorMessage="Item Sub Category Required" ValidationGroup="save" InitialValue="0"></asp:RequiredFieldValidator>
                                            <i></i>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Item Name">
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
                                    <asp:TemplateColumn HeaderText="Item Code">
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
                                    <asp:TemplateColumn HeaderText="Item Type">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "ItemType") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlEdtItemType" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                                <asp:ListItem Value="">Select Item Type</asp:ListItem>
                                                <asp:ListItem Value="Fixed Asset">Fixed Asset</asp:ListItem>
                                                <asp:ListItem Value="Current Asset">Current Asset</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvEdtItemType" runat="server" CssClass="validator" ControlToValidate="ddlEdtItemType" ErrorMessage="Item Type Required" ValidationGroup="edit" InitialValue=""></asp:RequiredFieldValidator>
                                            <i></i>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlItemType" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                                <asp:ListItem Value="">Select Item Type</asp:ListItem>
                                                <asp:ListItem Value="Fixed Asset">Fixed Asset</asp:ListItem>
                                                <asp:ListItem Value="Current Asset">Current Asset</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvItemType" runat="server" CssClass="validator" ControlToValidate="ddlItemType" ErrorMessage="Item Type Required" ValidationGroup="save" InitialValue=""></asp:RequiredFieldValidator>
                                            <i></i>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Is Spare Part?" FooterStyle-Width="100px">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "IsSparePart") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="ckEdtIsSparePart" runat="server" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox ID="ckIsSparePart" runat="server" />
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Re-Order Quantity">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "ReOrderQuantity") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEdtReOrderQty" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "ReOrderQuantity")%>'></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="txtEdtReOrderQty_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtEdtReOrderQty" ValidChars="&quot;.&quot;"></cc1:FilteredTextBoxExtender>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFReOrderQty" runat="server" CssClass="form-control"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="txtFReOrderQty_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtFReOrderQty" ValidChars="&quot;.&quot;"></cc1:FilteredTextBoxExtender>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Unit Of Measurement">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "UnitOfMeasurement.Name") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlEdtUnitOfMeas" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                                <asp:ListItem Value="0">Select Measurement</asp:ListItem>
                                            </asp:DropDownList>
                                            <i></i>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlUnitOfMeas" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                                <asp:ListItem Value="0">Select Measurement</asp:ListItem>
                                            </asp:DropDownList>
                                            <i></i>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Status">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"
                                                AppendDataBoundItems="True"
                                                ValidationGroup="edit">
                                                <asp:ListItem Value="">Select Status</asp:ListItem>
                                                <asp:ListItem Value="Active">Active</asp:ListItem>
                                                <asp:ListItem Value="InActive">InActive</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RfvStatus" runat="server" CssClass="validator"
                                                ControlToValidate="ddlStatus" ErrorMessage="Status Required"
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

