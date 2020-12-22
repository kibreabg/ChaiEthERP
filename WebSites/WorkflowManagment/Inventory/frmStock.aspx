<%@ Page Title="Item Stock" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmStock.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Inventory.Views.frmStock" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script src="../js/libs/jquery-2.0.2.min.js"></script>
    <script type="text/javascript">
        function showSearch() {
            $(document).ready(function () {
                $('#searchModal').modal('show');
            });
        }

        function showDetailModal() {
            $(document).ready(function () {
                $('#StockDetailModal').modal('show');
            });
        }

    </script>
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Item Stock</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Item</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlItem" DataTextField="Name" DataValueField="Id" AppendDataBoundItems="true" runat="server">
                                        <asp:ListItem Value="">Select Item</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label class="label">Quantity</label>
                                <label class="input">
                                    <asp:TextBox ID="txtQuantity" Width="100%" runat="server"></asp:TextBox>
                                </label>
                            </section>
                        </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" class="btn btn-primary" ValidationGroup="save"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>
    </div>
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Filter Stock</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Item</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlSrchItem" DataTextField="Name" DataValueField="Id" AppendDataBoundItems="true" runat="server">
                                        <asp:ListItem Value="">Select Item</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                </label>
                            </section>
                        </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnFind" runat="server" OnClick="btnFind_Click" Text="Find" CssClass="btn btn-default"></asp:Button>
                    </footer>
                </div>
                <div class="row">
                    <article class="col-xs-12 col-sm-12 col-md-12 col-lg-12 sortable-grid ui-sortable">
                        <div class="jarviswidget jarviswidget-color-blueDark jarviswidget-sortable" id="wid-id-0" data-widget-editbutton="false" role="widget" style="">
                            <header role="heading">
                                <h2>Item Stock Information</h2>
                            </header>
                            <div role="content">
                                <div class="widget-body">
                                    <asp:GridView ID="grvStockList"
                                        runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                        OnSelectedIndexChanged="grvStockList_SelectedIndexChanged"
                                        AllowPaging="True" OnPageIndexChanging="grvStockList_PageIndexChanging"
                                        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="5" OnRowDataBound="grvStockList_RowDataBound">
                                        <RowStyle CssClass="rowstyle" />
                                        <Columns>
                                            <asp:BoundField DataField="Item.Name" HeaderText="Item" SortExpression="Item.Name" />
                                            <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" />
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
                    </article>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
