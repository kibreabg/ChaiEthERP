<%@ Page Title="Item Issue" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmIssue.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Inventory.Views.frmIssue" %>

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
                $('#IssueDetailModal').modal('show');
            });
        }

    </script>
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Item Issue</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Issue Date</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtIssueDate" ReadOnly="true" runat="server"></asp:TextBox>
                                </label>
                            </section>      
                            <section class="col col-6">
                                <label class="label">Purpose of Issue</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlPurpose" runat="server" AppendDataBoundItems="True" DataTextField="FullName" DataValueField="Id">
                                        <asp:ListItem Value="Office Use">Office Use</asp:ListItem>
                                        <asp:ListItem Value="Donation">Donation</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                </label>
                            </section>                      
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Issued To</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlIssuedTo" runat="server" AppendDataBoundItems="True" DataTextField="FullName" DataValueField="Id">
                                        <asp:ListItem Value="0">Select Employee</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label class="label">Handed Over By</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlHandedOverBy" runat="server" AppendDataBoundItems="True" DataTextField="FullName" DataValueField="Id">
                                        <asp:ListItem Value="0">Select Employee</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                </label>
                            </section>
                        </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnNewDetail" Style="float: left;" runat="server" Text="Add Detail" OnClick="btnNewDetail_Click" CssClass="btn btn-default"></asp:Button>
                    </footer>
                    <div role="content">

                        <!-- widget edit box -->
                        <div class="jarviswidget-editbox">
                            <!-- This area used as dropdown edit box -->

                        </div>
                        <!-- end widget edit box -->

                        <!-- widget content -->
                        <div class="widget-body">
                            <div class="tab-content">
                                <div class="tab-pane active" id="hr1">
                                    <ul class="nav nav-tabs">
                                        <li class="active">
                                            <a href="#iss1" data-toggle="tab">Issue Details</a>
                                        </li>
                                    </ul>
                                    <div class="tab-content padding-10">
                                        <div class="tab-pane active" id="iss1">
                                            <asp:GridView ID="grvIssueDetails"
                                                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                                OnSelectedIndexChanged="grvIssueDetails_SelectedIndexChanged"
                                                AllowPaging="True" OnPageIndexChanging="grvIssueDetails_PageIndexChanging"
                                                CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="5" OnRowDataBound="grvIssueDetails_RowDataBound">
                                                <RowStyle CssClass="rowstyle" />
                                                <Columns>
                                                    <asp:BoundField DataField="Item.Name" HeaderText="Item" SortExpression="Item.Name" />
                                                    <asp:BoundField DataField="Shelf.Name" HeaderText="Shelf" SortExpression="Shelf.Name" />
                                                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" />
                                                    <asp:BoundField DataField="UnitCost" HeaderText="UnitCost" SortExpression="UnitCost" />
                                                    <asp:BoundField DataField="TotalQuantity" HeaderText="TotalQuantity" SortExpression="TotalQuantity" />
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
                        <!-- end widget content -->

                    </div>
                    <footer>
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" class="btn btn-primary" ValidationGroup="save"></asp:Button>
                        <a data-toggle="modal" runat="server" id="searchLink" href="#searchModal" class="btn btn-default"><i class="fa fa-circle-arrow-up fa-lg"></i>Search</a>
                        <asp:Button ID="btnDelete" runat="server" CausesValidation="False" class="btn btn-default"
                            Text="Delete" OnClick="btnDelete_Click" Visible="false"></asp:Button>
                        <cc1:ConfirmButtonExtender ID="btnDelete_ConfirmButtonExtender" runat="server"
                            ConfirmText="Are you sure you want to delete this record?" Enabled="True" TargetControlID="btnDelete">
                        </cc1:ConfirmButtonExtender>
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-default" OnClick="btnCancel_Click" Text="New" />
                    </footer>
                </div>
            </div>
        </div>

    </div>
    <div class="modal fade" id="IssueDetailModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" style="width: 800px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                    <h4 class="modal-title" id="myDetailModalLabel">Add Item Issue Detail</h4>
                </div>
                <div class="modal-body">
                    <div class="jarviswidget-editbox"></div>
                    <div class="widget-body no-padding">
                        <div class="smart-form">
                            <fieldset>
                                <div class="row">
                                    <section class="col col-6">
                                        <label class="label">Item Category</label>
                                        <label class="select">
                                            <asp:DropDownList ID="ddlCategory" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" DataTextField="Name" DataValueField="Id" AppendDataBoundItems="true" runat="server">
                                                <asp:ListItem Value="0">Select Item Category</asp:ListItem>
                                            </asp:DropDownList><i></i>
                                        </label>
                                    </section>
                                    <section class="col col-6">
                                        <label class="label">Item Sub-Category</label>
                                        <label class="select">
                                            <asp:DropDownList ID="ddlSubCategory" AutoPostBack="true" OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged" DataTextField="Name" DataValueField="Id" AppendDataBoundItems="true" runat="server">
                                                <asp:ListItem Value="0">Select Item Sub-Category</asp:ListItem>
                                            </asp:DropDownList><i></i>
                                        </label>
                                    </section>
                                </div>
                                <div class="row">
                                    <section class="col col-6">
                                        <label class="label">Item</label>
                                        <label class="select">
                                            <asp:DropDownList ID="ddlItem" DataTextField="Name" DataValueField="Id" AppendDataBoundItems="true" runat="server">
                                                <asp:ListItem Value="0">Select Item</asp:ListItem>
                                            </asp:DropDownList><i></i>
                                            <asp:RequiredFieldValidator ID="rfvItem" runat="server" ControlToValidate="ddlItem" CssClass="validator" Display="Dynamic" InitialValue="0" ErrorMessage="Item is mandatory" SetFocusOnError="true" ValidationGroup="detail"></asp:RequiredFieldValidator>
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
                                            <asp:RequiredFieldValidator ID="rfvShelf" runat="server" ControlToValidate="ddlShelf" CssClass="validator" Display="Dynamic" InitialValue="0" ErrorMessage="Shelf is mandatory" SetFocusOnError="true" ValidationGroup="detail"></asp:RequiredFieldValidator>
                                        </label>
                                    </section>
                                </div>
                                <div class="row">
                                    <section class="col col-6">
                                        <label class="label">Quantity</label>
                                        <label class="input">
                                            <asp:TextBox ID="txtQuantity" runat="server" Width="100%"></asp:TextBox>
                                        </label>
                                        <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity" CssClass="validator" Display="Dynamic" InitialValue="" ErrorMessage="Quantity is mandatory" SetFocusOnError="true" ValidationGroup="detail"></asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="ftbQuantity" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtQuantity" ValidChars="&quot;.&quot;">
                                        </cc1:FilteredTextBoxExtender>
                                    </section>
                                    <section class="col col-6">
                                        <asp:Label ID="Label7" runat="server" Text="Expiry Date" CssClass="label"></asp:Label>
                                        <label class="input">
                                            <i class="icon-append fa fa-calendar"></i>
                                            <asp:TextBox ID="txtExpiryDate" CssClass="form-control datepicker" data-dateformat="mm/dd/yy" runat="server"></asp:TextBox>
                                        </label>
                                    </section>
                                </div>
                                <div class="row">
                                    <section class="col col-6">
                                        <label class="label">Unit Cost</label>
                                        <label class="input">
                                            <asp:TextBox ID="txtUnitCost" runat="server" Width="100%"></asp:TextBox>
                                        </label>
                                        <cc1:FilteredTextBoxExtender ID="ftbUnitCost" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtUnitCost" ValidChars="&quot;.&quot;">
                                        </cc1:FilteredTextBoxExtender>
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
                                <asp:Button ID="btnSaveDetail" runat="server" ValidationGroup="detail" Text="Save Detail" CssClass="btn btn-primary" OnClick="btnSaveDetail_Click"></asp:Button>
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            </footer>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="searchModal" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel">Search Item Issues</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtSrchRequestNo">Issue No.</label>
                                <asp:TextBox ID="txtSrchRequestNo" CssClass="form-control" ToolTip="Issue No" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtSrchRequestDate">Request Date</label>
                                <label class="input" style="position: relative; display: block; font-weight: 400;">
                                    <i class="icon-append fa fa-calendar" style="position: absolute; top: 5px; width: 22px; height: 22px; font-size: 14px; line-height: 22px; text-align: center; right: 5px; padding-left: 3px; border-left-width: 1px; border-left-style: solid; color: #A2A2A2;"></i>
                                    <asp:TextBox ID="txtSrchRequestDate" CssClass="form-control datepicker"
                                        data-dateformat="mm/dd/yy" ToolTip="Request Date" runat="server"></asp:TextBox>
                                </label>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="text-align: right;">
                        <div class="col-md-12">
                            <div class="form-group">
                                <asp:Button ID="btnFind" runat="server" OnClick="btnFind_Click" Text="Find" CssClass="btn btn-primary"></asp:Button>
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <div class="well well-sm well-primary">
                                    <asp:GridView ID="grvIssueList"
                                        runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                        OnSelectedIndexChanged="grvIssueList_SelectedIndexChanged"
                                        AllowPaging="True" OnPageIndexChanging="grvIssueList_PageIndexChanging"
                                        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="5" OnRowDataBound="grvIssueList_RowDataBound">
                                        <RowStyle CssClass="rowstyle" />
                                        <Columns>
                                            <asp:BoundField DataField="IssueNo" HeaderText="Issue No" SortExpression="IssueNo" />
                                            <asp:TemplateField HeaderText="Issue Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDate" runat="server" Text='<%# Eval("IssueDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Purpose" HeaderText="Purpose of Issue" SortExpression="Purpose" />
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
            </div>
        </div>
    </div>
</asp:Content>
