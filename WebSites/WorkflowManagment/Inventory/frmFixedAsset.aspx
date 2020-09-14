<%@ Page Language="C#" Title="Fixed Assets" AutoEventWireup="true" MasterPageFile="~/Shared/ModuleMaster.master" CodeFile="frmFixedAsset.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Inventory.Views.frmFixedAsset" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script src="../js/libs/jquery-2.0.2.min.js"></script>
    <script type="text/javascript">
        function showUpdateFAModal() {
            $(document).ready(function () {
                $('#updateFAModal').modal('show');
            });
        }
        function showTransferFAModal() {
            $(document).ready(function () {
                $('#transferFAModal').modal('show');
            });
        }
        function showReturnFAModal() {
            $(document).ready(function () {
                $('#returnFAModal').modal('show');
            });
        }
    </script>

    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Filter Fixed Assets</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-6">
                                <label>Item</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlFilterItem" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" runat="server">
                                        <asp:ListItem Value="">Select Item</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label>Asset Status</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlFilterStatus" runat="server">
                                    </asp:DropDownList><i></i>
                                </label>
                            </section>
                        </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnFilter" OnClick="btnFilter_Click" runat="server" Text="Filter" CssClass="btn btn-primary"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>
    </div>
    <div class="jarviswidget jarviswidget-color-blueDark jarviswidget-sortable" id="wid-id-1" data-widget-editbutton="false" role="widget" style="">

        <header role="heading">
            <span class="widget-icon"><i class="fa fa-table"></i></span>
            <h2>Fixed Assets</h2>
            <span class="jarviswidget-loader"><i class="fa fa-refresh fa-spin"></i></span>
        </header>

        <!-- widget div-->
        <div role="content">

            <!-- widget edit box -->
            <div class="jarviswidget-editbox">
                <!-- This area used as dropdown edit box -->
            </div>
            <!-- end widget edit box -->

            <!-- widget content -->
            <div class="widget-body no-padding">
                <div id="datatable_fixed_column_wrapper" class="dataTables_wrapper form-inline no-footer">
                    <asp:DataGrid ID="dgFixedAsset" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                        CssClass="table table-striped table-bordered table-hover" OnPageIndexChanged="dgFixedAsset_PageIndexChanged" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                        GridLines="None" AllowPaging="true" AllowSorting="true" PageSize="20" ShowFooter="True">
                        <Columns>
                            <asp:TemplateColumn HeaderText="Item">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "Item.Name") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Supplier">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "Supplier.SupplierName") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Receive Date">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "ReceiveDate") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Receive No.">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "ReceiveNo") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Asset Code">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "AssetCode") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Serial No.">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "SerialNo") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Shelf">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "Shelf.Name") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Custodian">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "Custodian") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Asset Status">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "AssetStatus") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Operations">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlOperation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlOperation_SelectedIndexChanged" CssClass="btn btn-success dropdown-toggle" data-toggle="dropdown">
                                        <asp:ListItem Value="">Select Operation</asp:ListItem>
                                        <asp:ListItem Value="Update">Update</asp:ListItem>
                                        <asp:ListItem Value="Transfer">Transfer</asp:ListItem>
                                        <asp:ListItem Value="Return">Return</asp:ListItem>
                                        <asp:ListItem Value="Donate">Donate</asp:ListItem>
                                        <asp:ListItem Value="Dispose">Dispose</asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                    </asp:DataGrid>
                </div>
            </div>
            <!-- end widget content -->
        </div>
        <!-- end widget div -->

    </div>
    <div class="modal fade" id="updateFAModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" style="width: 800px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                    <h4 class="modal-title" id="myDetailModalLabel">Update Fixed Asset</h4>
                </div>
                <div class="modal-body">
                    <div class="jarviswidget-editbox"></div>
                    <div class="widget-body no-padding">
                        <div class="smart-form">
                            <fieldset>
                                <div class="row">
                                    <section class="col col-6">
                                        <label class="label">Asset Tag</label>
                                        <label class="input">
                                            <asp:TextBox ID="txtAssetCode" runat="server" Width="100%"></asp:TextBox>
                                        </label>
                                    </section>
                                    <section class="col col-6">
                                        <label class="label">Serial No</label>
                                        <label class="input">
                                            <asp:TextBox ID="txtSerialNo" runat="server" Width="100%"></asp:TextBox>
                                        </label>
                                    </section>
                                </div>
                                <div class="row">
                                    <section class="col col-6">
                                        <label class="label">Location</label>
                                        <label class="input">
                                            <asp:TextBox ID="txtLocation" runat="server" Width="100%"></asp:TextBox>
                                        </label>
                                    </section>
                                    <section class="col col-6">
                                        <label class="label">Condition</label>
                                        <label class="input">
                                            <asp:TextBox ID="txtCondition" runat="server" Width="100%"></asp:TextBox>
                                        </label>
                                    </section>
                                </div>
                                <div class="row">
                                    <section class="col col-6">
                                        <label class="label">Total Life</label>
                                        <label class="input">
                                            <asp:TextBox ID="txtTotalLife" runat="server" Width="100%"></asp:TextBox>
                                        </label>
                                    </section>
                                    <section class="col col-6">
                                        <label class="label">Remark</label>
                                        <label class="input">
                                            <asp:TextBox ID="txtRemark" runat="server" Width="100%"></asp:TextBox>
                                        </label>
                                    </section>
                                </div>
                            </fieldset>
                            <footer>
                                <asp:Button ID="btnUpdateFA" runat="server" OnClick="btnUpdateFA_Click" ValidationGroup="update" Text="Update" CssClass="btn btn-primary"></asp:Button>
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            </footer>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="transferFAModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" style="width: 800px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                    <h4 class="modal-title" id="transferModalLabel">Transfer Fixed Asset</h4>
                </div>
                <div class="modal-body">
                    <div class="jarviswidget-editbox"></div>
                    <div class="widget-body no-padding">
                        <div class="smart-form">
                            <fieldset>
                                <div class="row">
                                    <section class="col col-6">
                                        <label class="label">Current Custodian</label>
                                        <label class="input">
                                            <asp:Label ID="lblCurrentCustodian" CssClass="badge bg-color-red" Style="color: white; padding: 3px 7px;" runat="server"></asp:Label>
                                        </label>
                                    </section>
                                    <section class="col col-6">
                                        <label class="label">New Custodian</label>
                                        <label class="select">
                                            <asp:DropDownList ID="ddlNewCustodian" AppendDataBoundItems="true" DataValueField="FullName" DataTextField="FullName" runat="server">
                                                <asp:ListItem Value="">Select Custodian</asp:ListItem>
                                            </asp:DropDownList><i></i>
                                        </label>
                                    </section>
                                </div>
                            </fieldset>
                            <footer>
                                <asp:Button ID="btnTransferFA" runat="server" OnClick="btnTransferFA_Click" ValidationGroup="transfer" Text="Transfer" CssClass="btn btn-primary"></asp:Button>
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            </footer>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="returnFAModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" style="width: 800px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                    <h4 class="modal-title" id="returnModalLabel">Return Fixed Asset</h4>
                </div>
                <div class="modal-body">
                    <div class="jarviswidget-editbox"></div>
                    <div class="widget-body no-padding">
                        <div class="smart-form">
                            <fieldset>
                                <div class="row">
                                    <section class="col col-6">
                                        <label class="label">Current Custodian</label>
                                        <label class="input">
                                            <asp:Label ID="lblRtrnCrntCust" CssClass="badge bg-color-red" Style="color: white; padding: 3px 7px;" runat="server"></asp:Label>
                                        </label>
                                    </section>
                                    <section class="col col-6">
                                        <label class="label">Store</label>
                                        <label class="select">
                                            <asp:DropDownList ID="ddlStore" AutoPostBack="true" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" DataTextField="Name" DataValueField="Id" AppendDataBoundItems="true" runat="server">
                                                <asp:ListItem Value="0">Select Store</asp:ListItem>
                                            </asp:DropDownList><i></i>
                                        </label>
                                    </section>
                                </div>
                                <div class="row">
                                    <section class="col col-6">
                                        <label class="label">Section</label>
                                        <label class="select">
                                            <asp:DropDownList ID="ddlSection" AutoPostBack="true" OnSelectedIndexChanged="ddlSection_SelectedIndexChanged" DataTextField="Name" DataValueField="Id" AppendDataBoundItems="true" runat="server">
                                                <asp:ListItem Value="0">Select Section</asp:ListItem>
                                            </asp:DropDownList><i></i>
                                        </label>
                                    </section>
                                    <section class="col col-6">
                                        <label class="label">Shelf</label>
                                        <label class="select">
                                            <asp:DropDownList ID="ddlShelf" DataTextField="Name" DataValueField="Id" AppendDataBoundItems="true" runat="server">
                                                <asp:ListItem Value="0">Select Shelf</asp:ListItem>
                                            </asp:DropDownList><i></i>
                                            <asp:RequiredFieldValidator ID="rfvShelf" runat="server" ControlToValidate="ddlShelf" CssClass="validator" Display="Dynamic" InitialValue="0" ErrorMessage="Shelf is mandatory" SetFocusOnError="true" ValidationGroup="return"></asp:RequiredFieldValidator>
                                        </label>
                                    </section>
                                </div>
                            </fieldset>
                            <footer>
                                <asp:Button ID="btnReturn" runat="server" OnClick="btnReturn_Click" ValidationGroup="return" Text="Return" CssClass="btn btn-primary"></asp:Button>
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            </footer>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

