<%@ Page Language="C#" Title="Fixed Assets" AutoEventWireup="true" MasterPageFile="~/Shared/ModuleMaster.master" CodeFile="frmFixedAssetHistories.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Inventory.Views.frmFixedAssetHistories" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script src="../js/libs/jquery-2.0.2.min.js"></script>
    <script type="text/javascript">
    </script>

    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Filter Fixed Asset Transactions</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-3">
                                <label>Item</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlFilterAssetCode" AppendDataBoundItems="true" DataTextField="AssetCode" DataValueField="AssetCode" runat="server">
                                    </asp:DropDownList><i></i>
                                </label>
                            </section>
                            <section class="col col-3">
                                <label>Asset Status</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlFilterSerialNo" AppendDataBoundItems="true" DataTextField="SerialNo" DataValueField="SerialNo" runat="server">
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
            <h2>Fixed Asset Transactions</h2>
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
                    <asp:DataGrid ID="dgFixedAssetHistory" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                        CssClass="table table-striped table-bordered table-hover" OnPageIndexChanged="dgFixedAssetHistory_PageIndexChanged" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                        GridLines="None" AllowPaging="true" AllowSorting="true" PageSize="20" ShowFooter="True">
                        <Columns>
                            <asp:TemplateColumn HeaderText="Item">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "FixedAsset.Item.Name") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Serial No">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "FixedAsset.SerialNo") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Asset Code">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "FixedAsset.AssetCode") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Transaction Date">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "TransactionDate") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Custodian">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "Custodian") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Operation">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "Operation") %>
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
</asp:Content>

