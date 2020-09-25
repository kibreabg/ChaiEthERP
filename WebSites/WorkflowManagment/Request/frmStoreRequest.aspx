<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmStoreRequest.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Request.Views.frmStoreRequest" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script src="../js/libs/jquery-2.0.2.min.js"></script>
    <script type="text/javascript">
        function showSearch() {
            $(document).ready(function () {
                $('#searchModal').modal('show');
            });
        }
    </script>
    <asp:ValidationSummary ID="VSPurchaseRequest" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="Save" ForeColor="" />
    <asp:ValidationSummary ID="VSDetailAdd" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="proadd" ForeColor="" />
    <asp:ValidationSummary ID="VSEdit" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="proedit" ForeColor="" />

    <asp:Panel ID="pnlPurchaseSpareParts" Visible="false" runat="server">
        <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
            <header>
                <span class="widget-icon"><i class="fa fa-edit"></i></span>
                <h2>Spare Part related Purchases</h2>
            </header>
            <div>
                <div class="jarviswidget-editbox"></div>
                <div class="widget-body no-padding">
                    <div class="smart-form">
                        <fieldset>
                            <div class="row">
                                <section class="col col-4">
                                    <label id="lblPurchaseReq" runat="server" class="label" visible="true">
                                        Purchase Requests
                                    </label>
                                    <label class="select">
                                        <asp:DropDownList ID="ddlPurchaseReq" AppendDataBoundItems="true" AutoPostBack="true" runat="server" DataValueField="Id" DataTextField="RequestNo" OnSelectedIndexChanged="ddlPurchaseReq_SelectedIndexChanged">
                                        </asp:DropDownList><i></i>
                                    </label>
                                </section>
                            </div>
                        </fieldset>
                        <asp:Panel ID="pnlInfo" runat="server">
                            <div class="alert alert-info fade in">
                                <button class="close" data-dismiss="alert">
                                    ×
                                </button>
                                <i class="fa-fw fa fa-info"></i>
                                <strong>Info!</strong> Please select the Purchase Request Transaction you used to buy the Spare Parts
                            </div>
                        </asp:Panel>
                        <asp:GridView ID="grvDetails" Visible="False"
                            runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                            CssClass="table table-striped table-bordered table-hover" AllowPaging="True" OnPageIndexChanging="grvDetails_PageIndexChanging" PageSize="15">
                            <RowStyle CssClass="rowstyle" />
                            <Columns>
                                <asp:BoundField DataField="PurchaseRequest.RequestNo" HeaderText="Purchase Request No" SortExpression="PurchaseRequest.RequestNo" />
                                <asp:BoundField DataField="ItemAccount.AccountName" HeaderText="AccountName" SortExpression="ItemAccount.AccountName" />
                                <asp:BoundField DataField="ItemAccount.AccountCode" HeaderText="Account Code" SortExpression="ItemAccount.AccountCode" />
                                <asp:BoundField DataField="Item.Name" HeaderText="Item Description" SortExpression="Item.Name"></asp:BoundField>
                                <asp:BoundField DataField="Qty" HeaderText="Quantity" SortExpression="Qty" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle CssClass="FooterStyle" />
                            <HeaderStyle CssClass="headerstyle" />
                            <PagerStyle CssClass="PagerStyle" />
                            <RowStyle CssClass="rowstyle" />
                        </asp:GridView>

                        <footer>
                            <asp:Button ID="btnCreateStoreItem" runat="server" CssClass="btn btn-primary" Text="Prepare Store Requisition" OnClick="btnCreateStoreItem_Click" />
                        </footer>

                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <div id="wid-id-0" class="jarviswidget" data-widget-custombutton="false" data-widget-editbutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Store Request</h2>
        </header>


        <!-- widget div-->
        <div>

            <!-- widget edit box -->
            <div class="jarviswidget-editbox">
                <!-- This area used as dropdown edit box -->

            </div>
            <!-- end widget edit box -->

            <!-- widget content -->
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-6">
                                <div class="smart-form">
                                    <div class="inline-group">
                                        <label class="checkbox">
                                            <asp:CheckBox ID="ckIsSparePart" runat="server" OnCheckedChanged="ckIsSparePart_CheckedChanged" AutoPostBack="True" />
                                            <i></i>Is Vehicle Spare Part?</label>
                                    </div>
                                </div>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">
                                    Requester</label>
                                <label class="input">
                                    <asp:TextBox ID="txtRequester" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label id="lblRequestDate" runat="server" class="label" visible="true">
                                    Request Date</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtRequestDate" ReadOnly="true" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">
                                    Deliver to</label>
                                <label class="input">
                                    <asp:TextBox ID="txtDeliverto" runat="server" Visible="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfvDeliverto" CssClass="validator" runat="server" ControlToValidate="txtDeliverto" ErrorMessage="Deliver To Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label class="label">Program</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlProgram" AutoPostBack="true" DataTextField="ProgramName" DataValueField="Id" AppendDataBoundItems="true" runat="server" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select Program</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                </label>
                                <asp:RequiredFieldValidator ID="rfvProgram" runat="server" ControlToValidate="ddlProgram" CssClass="validator" Display="Dynamic" ErrorMessage="Select Program" SetFocusOnError="true" ValidationGroup="request"></asp:RequiredFieldValidator>
                            </section>
                        </div>
                        <asp:DataGrid ID="dgStoreRequestDetail" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                            GridLines="None" OnCancelCommand="dgStoreRequestDetail_CancelCommand" OnDeleteCommand="dgStoreRequestDetail_DeleteCommand" OnEditCommand="dgStoreRequestDetail_EditCommand"
                            OnItemCommand="dgStoreRequestDetail_ItemCommand" OnItemDataBound="dgStoreRequestDetail_ItemDataBound" OnUpdateCommand="dgStoreRequestDetail_UpdateCommand"
                            ShowFooter="True">

                            <Columns>
                                <asp:TemplateColumn HeaderText="Item Description">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "Item.Name")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlItem" DataTextField="Name" DataValueField="Id" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlFItem" DataTextField="Name" DataValueField="Id" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Requested Quantity">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "Qty")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "Qty")%>'></asp:TextBox>
                                        <asp:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtQty" ID="txtQty_FilteredTextBoxExtender" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RfvQty" CssClass="validator" runat="server" ControlToValidate="txtQty" ErrorMessage="Qty Required" ValidationGroup="proedit"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtFQty" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtFQty" ID="txtFQty_FilteredTextBoxExtender" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RfvFQty" CssClass="validator" runat="server" ControlToValidate="txtFQty" ErrorMessage="Qty Required" ValidationGroup="proadd"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Unit of Measurment">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlUnitOfMeasurment" runat="server" CssClass="form-control"
                                            AppendDataBoundItems="True"
                                            ValidationGroup="proedit">
                                            <asp:ListItem Value="0">Select Unit</asp:ListItem>
                                            <asp:ListItem Value="Pieces (Pcs)">Pieces (Pcs)</asp:ListItem>
                                            <asp:ListItem Value="Ream">Ream</asp:ListItem>
                                            <asp:ListItem Value="Grams (g)">Grams (g)</asp:ListItem>
                                            <asp:ListItem Value="Kilogram (kg)">Kilogram (kg)</asp:ListItem>
                                            <asp:ListItem Value="Tons (ton)">Tons (ton)</asp:ListItem>
                                            <asp:ListItem Value="Meter (m)">Meter (m)</asp:ListItem>
                                            <asp:ListItem Value="Square Meter (m2)">Square Meter (m2)</asp:ListItem>
                                            <asp:ListItem Value="Cubic meter (m3)">Cubic meter (m3)</asp:ListItem>
                                            <asp:ListItem Value="Liter (L)">Liter (L)</asp:ListItem>
                                            <asp:ListItem Value="Day (D)">Day (D)</asp:ListItem>
                                            <asp:ListItem Value="Hour (Hr)">Hour (Hr)</asp:ListItem>
                                            <asp:ListItem Value="Pack (Pk)">Pack (Pk)</asp:ListItem>
                                            <asp:ListItem Value="Set (St)">Set (St)</asp:ListItem>
                                            <asp:ListItem Value="Other">Other</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RfvUoM" runat="server" CssClass="validator"
                                            ControlToValidate="ddlUnitOfMeasurment" ErrorMessage="Unit of Measurement is Required"
                                            InitialValue="0" SetFocusOnError="True" ValidationGroup="proedit"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlFUnitOfMeasurment" runat="server" CssClass="form-control"
                                            AppendDataBoundItems="True" DataTextField="AccountName" DataValueField="Id"
                                            EnableViewState="true" ValidationGroup="proadd">
                                            <asp:ListItem Value="0">Select Unit</asp:ListItem>
                                            <asp:ListItem Value="Pieces (Pcs)">Pieces (Pcs)</asp:ListItem>
                                            <asp:ListItem Value="Ream">Ream</asp:ListItem>
                                            <asp:ListItem Value="Grams (g)">Grams (g)</asp:ListItem>
                                            <asp:ListItem Value="Kilogram (kg)">Kilogram (kg)</asp:ListItem>
                                            <asp:ListItem Value="Tons (ton)">Tons (ton)</asp:ListItem>
                                            <asp:ListItem Value="Meter (m)">Meter (m)</asp:ListItem>
                                            <asp:ListItem Value="Square Meter (m2)">Square Meter (m2)</asp:ListItem>
                                            <asp:ListItem Value="Cubic meter (m3)">Cubic meter (m3)</asp:ListItem>
                                            <asp:ListItem Value="Liter (L)">Liter (L)</asp:ListItem>
                                            <asp:ListItem Value="Day (D)">Day (D)</asp:ListItem>
                                            <asp:ListItem Value="Hour (Hr)">Hour (Hr)</asp:ListItem>
                                            <asp:ListItem Value="Pack (Pk)">Pack (Pk)</asp:ListItem>
                                            <asp:ListItem Value="Set (St)">Set (St)</asp:ListItem>
                                            <asp:ListItem Value="Other">Other</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RfvFUoM" runat="server" CssClass="validator"
                                            ControlToValidate="ddlFUnitOfMeasurment" Display="Dynamic"
                                            ErrorMessage="Unit of Measurement is Required" InitialValue="0" SetFocusOnError="True"
                                            ValidationGroup="proadd"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "UnitOfMeasurment")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Remark">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "Remark")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "Remark")%>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RfvRemark" CssClass="validator" runat="server" ControlToValidate="txtRemark" ErrorMessage="Remark Required" ValidationGroup="proedit"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtFRemark" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RfvFRemark" CssClass="validator" runat="server" ControlToValidate="txtFQty" ErrorMessage="Remark Required" ValidationGroup="proadd"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Project ID">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control"
                                            AppendDataBoundItems="True" DataTextField="ProjectCode" DataValueField="Id"
                                            ValidationGroup="proedit" AutoPostBack="True" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Select Project</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RfvProject" runat="server" CssClass="validator"
                                            ControlToValidate="ddlProject" ErrorMessage="Project Required"
                                            InitialValue="0" SetFocusOnError="True" ValidationGroup="proedit"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlFProject" runat="server" CssClass="form-control"
                                            AppendDataBoundItems="True" DataTextField="ProjectCode" DataValueField="Id"
                                            EnableViewState="true" ValidationGroup="proadd" AutoPostBack="True" OnSelectedIndexChanged="ddlFProject_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Select Project</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RfvFProjectCode" runat="server" CssClass="validator"
                                            ControlToValidate="ddlFProject" Display="Dynamic"
                                            ErrorMessage="Project Required" InitialValue="0" SetFocusOnError="True"
                                            ValidationGroup="proadd"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "Project.ProjectCode")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Grant ID">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlGrant" runat="server" CssClass="form-control"
                                            AppendDataBoundItems="True" DataTextField="GrantCode" DataValueField="Id"
                                            ValidationGroup="proedit" OnSelectedIndexChanged="ddlGrant_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Select Grant</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RfvGrant" runat="server" CssClass="validator"
                                            ControlToValidate="ddlGrant" ErrorMessage="Grant Required"
                                            InitialValue="0" SetFocusOnError="True" ValidationGroup="proedit"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlFGrant" runat="server" CssClass="form-control"
                                            AppendDataBoundItems="True" DataTextField="GrantCode" DataValueField="Id"
                                            EnableViewState="true" ValidationGroup="proadd">
                                            <asp:ListItem Value="0">Select Grant</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RfvFGrantCode" runat="server" CssClass="validator"
                                            ControlToValidate="ddlFGrant" Display="Dynamic"
                                            ErrorMessage="Grant Required" InitialValue="0" SetFocusOnError="True"
                                            ValidationGroup="proadd"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "Grant.GrantCode")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Actions">
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" ValidationGroup="proedit" CssClass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton ID="lnkAddNew" runat="server" CommandName="AddNew" ValidationGroup="proadd" CssClass="btn btn-sm btn-success"><i class="fa fa-save"></i></asp:LinkButton>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');"><i class="fa fa-times"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                            <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                        </asp:DataGrid>
                    </fieldset>

                    <footer>
                        <asp:Button ID="btnRequest" runat="server" CssClass="btn btn-primary" OnClick="btnRequest_Click" Text="Request" ValidationGroup="Save" />
                        <a data-toggle="modal" runat="server" id="searchLink" href="#searchModal" class="btn btn-default"><i class="fa fa-circle-arrow-up fa-lg"></i>Search</a>
                        <%--<asp:Button ID="btnsearch2" runat="server" CssClass="btn btn-primary" Text="Search" />--%>
                        <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-default" Text="Delete" OnClick="btnDelete_Click" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');" TabIndex="9" Visible="False" />

                        <%--<asp:Button ID="btnSearch" data-toggle="modal" runat="server" OnClientClick="#myModal" Text="Search" ></asp:Button>--%>
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-default" OnClick="btnCancel_Click" Text="New" />
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-default" PostBackUrl="../Default.aspx"></asp:Button>

                    </footer>

                </div>
            </div>
            <!-- end widget content -->

        </div>
        <!-- end widget div -->
    </div>
    <div class="modal fade" id="searchModal" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body no-padding">
                    <div class="jarviswidget" data-widget-colorbutton="false"
                        data-widget-editbutton="false"
                        data-widget-togglebutton="false"
                        data-widget-deletebutton="false"
                        data-widget-fullscreenbutton="false"
                        data-widget-custombutton="false"
                        data-widget-sortable="false">
                        <header>
                            <span class="widget-icon"><i class="fa fa-edit"></i></span>
                            <h2>Search Store Request</h2>
                        </header>
                        <div>
                            <div class="jarviswidget-editbox"></div>
                            <div class="widget-body no-padding">
                                <div class="smart-form">
                                    <fieldset>
                                        <div class="row">
                                            <section class="col col-6">
                                                <asp:Label ID="lblRequestNosearch" runat="server" Text="Request No" CssClass="label"></asp:Label>

                                                <label class="input">
                                                    <asp:TextBox ID="txtRequestNosearch" runat="server" Visible="true"></asp:TextBox>
                                                </label>
                                            </section>
                                            <section class="col col-6">
                                                <asp:Label ID="lblRequestDatesearch" runat="server" Text="Request Date" CssClass="label"></asp:Label>

                                                <label class="input">
                                                    <i class="icon-append fa fa-calendar"></i>
                                                    <asp:TextBox ID="txtRequestDatesearch" CssClass="form-control datepicker"
                                                        data-dateformat="mm/dd/yy" runat="server" Visible="true"></asp:TextBox>
                                                </label>
                                            </section>
                                        </div>
                                    </fieldset>

                                    <footer>
                                        <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" CssClass="btn btn-primary"></asp:Button>
                                        <asp:Button ID="Button1" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary"></asp:Button>
                                    </footer>
                                </div>
                            </div>
                        </div>
                        <asp:GridView ID="grvStoreRequestList"
                            runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                            OnRowDataBound="grvStoreRequestList_RowDataBound" OnRowDeleting="grvStoreRequestList_RowDeleting"
                            OnSelectedIndexChanged="grvStoreRequestList_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvStoreRequestList_PageIndexChanging"
                            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="5">
                            <RowStyle CssClass="rowstyle" />
                            <Columns>
                                <asp:BoundField DataField="RequestNo" HeaderText="Request No" SortExpression="RequestNo" />
                                <asp:TemplateField HeaderText="Request Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("RequestedDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DeliverTo" HeaderText="Deliver To" SortExpression="DeliverTo" />
                                <asp:CommandField ShowSelectButton="True" />
                            </Columns>
                            <FooterStyle CssClass="FooterStyle" />
                            <HeaderStyle CssClass="headerstyle" />
                            <PagerStyle CssClass="PagerStyle" />
                            <RowStyle CssClass="rowstyle" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:Panel ID="pnlWarning" Visible="false" Style="position: absolute; top: 370px; left: 84px;" runat="server">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                </div>
                <div class="modal-body no-padding">
                    <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
                        <div>
                            <div class="jarviswidget-editbox"></div>
                            <div class="widget-body no-padding">
                                <div class="smart-form">
                                    <div class="alert alert-block alert-danger">
                                        <h4 class="alert-heading">Warning</h4>
                                        <p>
                                            The current Request has no Approval Settings defined. Please contact your administrator!
                                       
                                        </p>
                                    </div>
                                    <footer>
                                        <asp:Button ID="btnCancelPopup" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnCancelPopup_Click"></asp:Button>
                                    </footer>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="menuContent" runat="Server">
</asp:Content>

