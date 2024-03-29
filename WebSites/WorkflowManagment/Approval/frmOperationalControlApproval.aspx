﻿<%@ Page Title="Bank Payment Request Approval Form" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmOperationalControlApproval.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Approval.Views.frmOperationalControlApproval" EnableEventValidation="false" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script src="../js/libs/jquery-2.0.2.min.js"></script>
    <script type="text/javascript" language="javascript">
        function Clickheretoprint(theid) {
            var disp_setting = "toolbar=yes,location=no,directories=yes,menubar=yes,";
            disp_setting += "scrollbars=yes,width=750, height=600, left=100, top=25";
            var content_vlue = document.getElementById(theid).innerHTML;

            var docprint = window.open("", "", disp_setting);
            docprint.document.open();
            docprint.document.write('<html><head><title>CHAI Ethiopia ERP</title>');
            docprint.document.write('</head><body onLoad="self.print()"><center>');
            docprint.document.write(content_vlue);
            docprint.document.write('</center></body></html>');
            docprint.document.close();
            docprint.focus();
        }
        function showApprovalModal() {
            $(document).ready(function () {
                $('#approvalModal').modal('show');
            });
        }
        function showDetailModal() {
            $(document).ready(function () {
                $('#detailModal').modal('show');
            });
        }
    </script>
    <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Search Bank Payment Requests</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-3">
                                <asp:Label ID="lblSrchRequestNo" runat="server" Text="Voucher No" CssClass="label"></asp:Label>
                                <label class="input">
                                    <asp:TextBox ID="txtSrchRequestNo" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-3">
                                <asp:Label ID="lblSrchRequestDate" runat="server" Text="Request Date" CssClass="label"></asp:Label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtSrchRequestDate" CssClass="form-control datepicker" data-dateformat="mm/dd/yy" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-3">
                                <asp:Label ID="lblSrchProgressStatus" runat="server" Text="Status" CssClass="label"></asp:Label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlSrchProgressStatus" runat="server">
                                    </asp:DropDownList><i></i>
                                </label>
                            </section>
                            <section class="col col-3">
                                <asp:Label ID="lblSrchRequester" runat="server" Text="Requester" CssClass="label"></asp:Label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlSrchRequester" runat="server">
                                    </asp:DropDownList><i></i>
                                </label>
                            </section>
                        </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnPop" runat="server" />
                        <asp:Button ID="btnPop2" runat="server" />
                        <asp:Button ID="btnPop3" runat="server" />
                        <asp:Button ID="btnFind" runat="server" Text="Find" CssClass="btn btn-primary" OnClick="btnFind_Click"></asp:Button>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>
        <asp:GridView ID="grvOperationalControlRequestList"
            runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="grvOperationalControlRequestList_RowCommand"
            OnRowDataBound="grvOperationalControlRequestList_RowDataBound" OnSelectedIndexChanged="grvOperationalControlRequestList_SelectedIndexChanged"
            AllowPaging="True" OnPageIndexChanging="grvOperationalControlRequestList_PageIndexChanging"
            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="30">
            <RowStyle CssClass="rowstyle" />
            <Columns>
                <asp:BoundField DataField="RequestNo" HeaderText="Vourcher No" SortExpression="RequestNo" />
                <asp:BoundField DataField="RequestDate" HeaderText="Request Date" SortExpression="RequestDate" />
                <asp:BoundField DataField="AppUser.FullName" HeaderText="Bank Payment Initiator" SortExpression="AppUser.FullName" />
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                <asp:BoundField DataField="Supplier.SupplierName" HeaderText="Beneficiary" SortExpression="Supplier.SupplierName" />
                <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" SortExpression="TotalAmount" />
                <asp:ButtonField ButtonType="Button" CommandName="ViewItem" Text="View Item Detail" />
                <asp:CommandField ButtonType="Button" SelectText="Process Request" ShowSelectButton="True" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="btnStatus" Text="" Enabled="false" BorderStyle="None" />
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
            <FooterStyle CssClass="FooterStyle" />
            <HeaderStyle CssClass="headerstyle" />
            <PagerStyle CssClass="PagerStyle" />
            <RowStyle CssClass="rowstyle" />
        </asp:GridView>
        <div>
            <asp:Button runat="server" ID="btnInProgress" Text="" BorderStyle="None" BackColor="#FFFF6C" />
            <b>In Progress</b><br />
            <asp:Button runat="server" ID="btnComplete" Text="" BorderStyle="None" BackColor="#FF7251" />
            <b>Completed</b>
        </div>
        <br />
    </div>
    <div class="modal fade" id="approvalModal" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                    <h4 class="modal-title" id="myModalLabel">Process Bank Payment Request</h4>
                </div>
                <div class="modal-body">
                    <div class="jarviswidget-editbox"></div>
                    <div class="widget-body no-padding">
                        <div class="smart-form">
                            <fieldset>
                                <div class="row">
                                    <section class="col col-6">
                                        <asp:Label ID="lblApprovalStatus" runat="server" Text="Approval Status" CssClass="label"></asp:Label>
                                        <label class="select">
                                            <asp:DropDownList ID="ddlApprovalStatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlApprovalStatus_SelectedIndexChanged">
                                                <asp:ListItem Value="0">Select Status</asp:ListItem>
                                            </asp:DropDownList><i></i>
                                            <asp:RequiredFieldValidator ID="RfvApprovalStatus" CssClass="validator" runat="server" ValidationGroup="save" ErrorMessage="Approval Status Required" InitialValue="0" ControlToValidate="ddlApprovalStatus"></asp:RequiredFieldValidator>
                                        </label>
                                    </section>
                                    <section class="col col-6">
                                        <asp:Label ID="lblRejectedReason" runat="server" Text="Rejected Reason" Visible="false" CssClass="label"></asp:Label>
                                        <label class="input">
                                            <asp:TextBox ID="txtRejectedReason" CssClass="form-control" TextMode="MultiLine" Visible="false" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvRejectedReason" runat="server" Enabled="false" CssClass="validator" ValidationGroup="save" ErrorMessage="Must Enter Rejection Reason" ControlToValidate="txtRejectedReason"></asp:RequiredFieldValidator>
                                        </label>
                                    </section>
                                </div>
                            </fieldset>
                            <footer>
                                <asp:Button ID="btnApprove" runat="server" ValidationGroup="save" Text="Save" OnClick="btnApprove_Click" Enabled="false" CssClass="btn btn-primary"></asp:Button>
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                                <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" OnClientClick="javascript:Clickheretoprint('divprint')" Enabled="False"></asp:Button>
                            </footer>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="detailModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" style="width: 100%;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                    <h4 class="modal-title">Bank Payment Request Detail</h4>
                </div>
                <div class="modal-body">
                    <div class="jarviswidget-editbox"></div>
                    <div class="widget-body no-padding">
                        <div class="smart-form">
                            <div role="content">
                                <div class="jarviswidget-editbox">
                                </div>
                                <div class="widget-body">
                                    <div class="tab-content">
                                        <div class="tab-pane active" id="hr1">
                                            <ul class="nav nav-tabs">
                                                <li class="active">
                                                    <a href="#iss1" data-toggle="tab">Item Details</a>
                                                </li>
                                                <li>
                                                    <a href="#iss2" data-toggle="tab">Attachment</a>
                                                </li>
                                            </ul>
                                            <div class="tab-content padding-10">
                                                <div class="tab-pane active" id="iss1">
                                                    <div style="overflow-x: auto;">
                                                        <asp:DataGrid ID="dgOperationalControlRequestDetail" runat="server"
                                                            AutoGenerateColumns="False" CellPadding="0" CssClass="table table-striped table-bordered table-hover"
                                                            DataKeyField="Id" GridLines="None" PagerStyle-CssClass="paginate_button active" ShowFooter="True"
                                                            OnEditCommand="dgOperationalControlRequestDetail_EditCommand" OnItemDataBound="dgOperationalControlRequestDetail_ItemDataBound"
                                                            OnUpdateCommand="dgOperationalControlRequestDetail_UpdateCommand">
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText="Account Name">
                                                                    <ItemTemplate>
                                                                        <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountName")%>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlEdtAccountDescription" CssClass="form-control" OnSelectedIndexChanged="ddlEdtAccountDescription_SelectedIndexChanged" runat="server" AppendDataBoundItems="true" AutoPostBack="True">
                                                                            <asp:ListItem Value="0">Select Account</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <i></i>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Account Code">
                                                                    <ItemTemplate>
                                                                        <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountCode")%>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtEdtAccountCode" ReadOnly="true" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "AccountCode")%>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Amount">
                                                                    <ItemTemplate>
                                                                        <%# DataBinder.Eval(Container.DataItem, "Amount")%>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblSettelementTotalVariance" runat="server" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Project ID">
                                                                    <ItemTemplate>
                                                                        <%# DataBinder.Eval(Container.DataItem, "Project.ProjectCode")%>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlEdtProject" CssClass="form-control" runat="server" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlEdtProject_SelectedIndexChanged">
                                                                            <asp:ListItem Value="0">Select Project</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <i></i>
                                                                        <asp:RequiredFieldValidator ID="rfvddlEdtProject" runat="server" CssClass="validator" ControlToValidate="ddlEdtProject" Display="Dynamic" ErrorMessage="Project must be selected" InitialValue="0" SetFocusOnError="true" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Grant ID">
                                                                    <ItemTemplate>
                                                                        <%# DataBinder.Eval(Container.DataItem, "Grant.GrantCode")%>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlEdtGrant" runat="server" CssClass="form-control" DataTextField="GrantCode" DataValueField="Id">
                                                                            <asp:ListItem Value="0">Select Grant</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="RfvGrant" runat="server" ControlToValidate="ddlEdtGrant" ErrorMessage="Grant is required" InitialValue="0" SetFocusOnError="True" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Actions">
                                                                    <EditItemTemplate>
                                                                        <asp:LinkButton ID="lnkUpdate" runat="server" CausesValidation="true" CommandName="Update" CssClass="btn btn-xs btn-default" ValidationGroup="edit"><i class="fa fa-save"></i></asp:LinkButton>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                            </Columns>
                                                            <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                                        </asp:DataGrid>
                                                        <br />
                                                        <asp:GridView ID="grvOperationalControlStatuses"
                                                            runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                                            CssClass="table table-striped table-bordered table-hover" OnRowDataBound="grvStatuses_RowDataBound">
                                                            <RowStyle CssClass="rowstyle" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField HeaderText="Name" />
                                                                <asp:BoundField DataField="AssignedBy" HeaderText="Assignee Approver" SortExpression="AssignedBy" />
                                                                <asp:BoundField HeaderText="Approval Status" DataField="ApprovalStatus" />
                                                                <asp:BoundField HeaderText="Rejected Reason" DataField="RejectedReason" />
                                                            </Columns>
                                                            <FooterStyle CssClass="FooterStyle" />
                                                            <HeaderStyle CssClass="headerstyle" />
                                                            <PagerStyle CssClass="PagerStyle" />
                                                            <RowStyle CssClass="rowstyle" />
                                                        </asp:GridView>
                                                        <br />
                                                        <asp:Panel ID="pnlPaymentDetails" runat="server" Visible="false">
                                                            <div style="text-align: center;">
                                                                <asp:Label ID="lblPaymentReqDetail" Font-Size="Large" Font-Bold="true" runat="server" Text="Payment Detail"></asp:Label>
                                                            </div>
                                                            <br />
                                                            <span style="color: green; font-weight: bold;">Payment Requester : </span>
                                                            <asp:Label ID="lblPaymentRequester" runat="server"></asp:Label>
                                                            <asp:GridView ID="grvPaymentRequestStatuses"
                                                                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                                                CssClass="table table-striped table-bordered table-hover" OnRowDataBound="grvPaymentStatuses_RowDataBound">
                                                                <RowStyle CssClass="rowstyle" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="Name" />
                                                                    <asp:BoundField DataField="AssignedBy" HeaderText="Assignee Approver" SortExpression="AssignedBy" />
                                                                    <asp:BoundField HeaderText="Approval Status" DataField="ApprovalStatus" />
                                                                </Columns>
                                                                <FooterStyle CssClass="FooterStyle" />
                                                                <HeaderStyle CssClass="headerstyle" />
                                                                <PagerStyle CssClass="PagerStyle" />
                                                                <RowStyle CssClass="rowstyle" />
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlTravelDetails" runat="server" Visible="false">
                                                            <div style="text-align: center;">
                                                                <asp:Label ID="lblTravelDetail" Font-Size="Large" Font-Bold="true" runat="server" Text="Travel Advance Detail"></asp:Label>
                                                            </div>
                                                            <br />
                                                            <span style="color: green; font-weight: bold;">Travel Requester : </span>
                                                            <asp:Label ID="lblTravelRequester" runat="server"></asp:Label>
                                                            <asp:DataGrid ID="dgTravelAdvanceRequestDetail" runat="server" OnSelectedIndexChanged="dgTravelAdvanceRequestDetail_SelectedIndexChanged"
                                                                AutoGenerateColumns="False" CellPadding="0" CssClass="table table-striped table-bordered table-hover"
                                                                DataKeyField="Id" GridLines="None" PagerStyle-CssClass="paginate_button active" ShowFooter="True">
                                                                <Columns>
                                                                    <asp:TemplateColumn HeaderText="City From">
                                                                        <ItemTemplate>
                                                                            <%# DataBinder.Eval(Container.DataItem, "CityFrom")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="City To">
                                                                        <ItemTemplate>
                                                                            <%# DataBinder.Eval(Container.DataItem, "CityTo")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Hotel Booked">
                                                                        <ItemTemplate>
                                                                            <%# DataBinder.Eval(Container.DataItem, "HotelBooked")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="From Date">
                                                                        <ItemTemplate>
                                                                            <%# DataBinder.Eval(Container.DataItem, "FromDate","{0:dd/MM/yyyy}")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="To Date">
                                                                        <ItemTemplate>
                                                                            <%# DataBinder.Eval(Container.DataItem, "ToDate","{0:dd/MM/yyyy}")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Mode of Travel">
                                                                        <ItemTemplate>
                                                                            <%# DataBinder.Eval(Container.DataItem, "ModeOfTravel")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:ButtonColumn ButtonType="PushButton" CommandName="Select" Text="View Costs"></asp:ButtonColumn>
                                                                </Columns>
                                                                <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                                            </asp:DataGrid>
                                                            <br />
                                                            <asp:GridView ID="grvTravelAdvanceCosts"
                                                                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                                                AllowPaging="True" CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active">
                                                                <RowStyle CssClass="rowstyle" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="ItemAccount.AccountName" HeaderText="Account Name" />
                                                                    <asp:BoundField DataField="ItemAccount.AccountCode" HeaderText="Request Date" />
                                                                    <asp:BoundField DataField="ExpenseType.ExpenseTypeName" HeaderText="Expense Type" />
                                                                    <asp:BoundField DataField="Days" HeaderText="Days" />
                                                                    <asp:BoundField DataField="UnitCost" HeaderText="Unit Cost" />
                                                                    <asp:BoundField DataField="NoOfUnits" HeaderText="No Of Units" />
                                                                    <asp:BoundField DataField="Total" HeaderText="Total" />
                                                                </Columns>
                                                                <FooterStyle CssClass="FooterStyle" />
                                                                <HeaderStyle CssClass="headerstyle" />
                                                                <PagerStyle CssClass="PagerStyle" />
                                                                <RowStyle CssClass="rowstyle" />
                                                            </asp:GridView>
                                                            <br />
                                                            <asp:GridView ID="grvTravelAdvanceStatuses" OnRowDataBound="grvTravelStatuses_RowDataBound"
                                                                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                                                CssClass="table table-striped table-bordered table-hover">
                                                                <RowStyle CssClass="rowstyle" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="Name" />
                                                                    <asp:BoundField DataField="AssignedBy" HeaderText="Assignee Approver" SortExpression="AssignedBy" />
                                                                    <asp:BoundField HeaderText="Approval Status" DataField="ApprovalStatus" />
                                                                </Columns>
                                                                <FooterStyle CssClass="FooterStyle" />
                                                                <HeaderStyle CssClass="headerstyle" />
                                                                <PagerStyle CssClass="PagerStyle" />
                                                                <RowStyle CssClass="rowstyle" />
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlLiquidationDetails" runat="server" Visible="false">
                                                            <div style="text-align: center;">
                                                                <asp:Label ID="lblLiquidationDetail" Font-Size="Large" Font-Bold="true" runat="server" Text="Travel Expense Liquidation Detail"></asp:Label>
                                                            </div>
                                                            <br />
                                                            <span style="color: green; font-weight: bold;">Liquidation Requester : </span>
                                                            <asp:Label ID="lblLiquidationRequester" runat="server"></asp:Label>
                                                            <asp:DataGrid ID="dgLiquidationRequestDetail" runat="server"
                                                                AutoGenerateColumns="False" CellPadding="0" CssClass="table table-striped table-bordered table-hover"
                                                                DataKeyField="Id" GridLines="None" PagerStyle-CssClass="paginate_button active"
                                                                ShowFooter="True" OnItemDataBound="dgLiquidationRequestDetail_ItemDataBound">
                                                                <Columns>
                                                                    <asp:TemplateColumn HeaderText="Account Name">
                                                                        <EditItemTemplate>
                                                                            <asp:DropDownList ID="ddlEdtAccountDescription" CssClass="form-control" OnSelectedIndexChanged="ddlEdtAccountDescription_SelectedIndexChanged" runat="server" AppendDataBoundItems="true" AutoPostBack="True">
                                                                                <asp:ListItem Value="0">Select Account</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <i></i>
                                                                        </EditItemTemplate>
                                                                        <ItemTemplate>
                                                                            <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountName")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Account Code">
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtEdtAccountCode" ReadOnly="true" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountCode")%>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <ItemTemplate>
                                                                            <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountCode")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Amount Advanced">
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalAdvAmount" runat="server" />
                                                                        </FooterTemplate>
                                                                        <ItemTemplate>
                                                                            <%# DataBinder.Eval(Container.DataItem, "AmountAdvanced")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Actual Expenditure">
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalActualExp" runat="server" />
                                                                        </FooterTemplate>
                                                                        <ItemTemplate>
                                                                            <%# DataBinder.Eval(Container.DataItem, "ActualExpenditure")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Variance">
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalVariance" runat="server" />
                                                                        </FooterTemplate>
                                                                        <ItemTemplate>
                                                                            <%# DataBinder.Eval(Container.DataItem, "Variance")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Project ID">
                                                                        <EditItemTemplate>
                                                                            <asp:DropDownList ID="ddlEdtProject" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                                                                                <asp:ListItem Value="0">Select Project</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <i></i>
                                                                            <asp:RequiredFieldValidator ID="rfvddlEdtProject" runat="server" ControlToValidate="ddlEdtProject" CssClass="validator" Display="Dynamic" ErrorMessage="Project must be selected" InitialValue="0" SetFocusOnError="true" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                        <ItemTemplate>
                                                                            <%# DataBinder.Eval(Container.DataItem, "Project.ProjectCode")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                </Columns>
                                                                <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                                            </asp:DataGrid>
                                                            <br />
                                                            <asp:GridView ID="grvLiquidationStatuses"
                                                                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                                                CssClass="table table-striped table-bordered table-hover" OnRowDataBound="grvLiquidationStatuses_RowDataBound">
                                                                <RowStyle CssClass="rowstyle" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="Approver" HeaderText="Reviewer" SortExpression="Approver" />
                                                                    <asp:BoundField DataField="AssignedBy" HeaderText="Assignee Approver" SortExpression="AssignedBy" />
                                                                    <asp:BoundField HeaderText="Approval Status" DataField="ApprovalStatus" />
                                                                </Columns>
                                                                <FooterStyle CssClass="FooterStyle" />
                                                                <HeaderStyle CssClass="headerstyle" />
                                                                <PagerStyle CssClass="PagerStyle" />
                                                                <RowStyle CssClass="rowstyle" />
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlSettlmentDetails" runat="server" Visible="false">
                                                            <div style="text-align: center;">
                                                                <asp:Label ID="lblSettelementDetail" Font-Size="Large" Font-Bold="true" runat="server" Text="Cash Payment Settelement Detail"></asp:Label>
                                                            </div>
                                                            <br />
                                                            <asp:DataGrid ID="dgReimbursementDetail" runat="server"
                                                                AutoGenerateColumns="False" CellPadding="0" CssClass="table table-striped table-bordered table-hover"
                                                                DataKeyField="Id" GridLines="None" PagerStyle-CssClass="paginate_button active" ShowFooter="True" OnItemDataBound="dgReimbursementDetail_ItemDataBound">
                                                                <Columns>
                                                                    <asp:TemplateColumn HeaderText="Account Name">
                                                                        <ItemTemplate>
                                                                            <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountName")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Account Code">
                                                                        <ItemTemplate>
                                                                            <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountCode")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Actual Expenditure">
                                                                        <ItemTemplate>
                                                                            <%# DataBinder.Eval(Container.DataItem, "ActualExpenditure")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Project ID">
                                                                        <ItemTemplate>
                                                                            <%# DataBinder.Eval(Container.DataItem, "PaymentReimbursementRequest.Project.ProjectCode")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Amount Advanced">
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalAdvAmount" runat="server" />
                                                                        </FooterTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotalAdvAmount" runat="server" />
                                                                            <%--  <%# DataBinder.Eval(Container.DataItem, "AmountAdvanced")%>--%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Variance">
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalVariance" runat="server" />
                                                                        </FooterTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotalVariance" runat="server" />
                                                                            <%--<%# DataBinder.Eval(Container.DataItem, "Variance")%>--%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                </Columns>
                                                                <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                                            </asp:DataGrid>
                                                            <br />
                                                            <asp:GridView ID="dgReimbursementStatus"
                                                                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                                                CssClass="table table-striped table-bordered table-hover" OnRowDataBound="dgReimbursementStatus_RowDataBound">
                                                                <RowStyle CssClass="rowstyle" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="Approver" HeaderText="Reviewer" SortExpression="Approver" />
                                                                    <asp:BoundField DataField="AssignedBy" HeaderText="Assignee Approver" SortExpression="AssignedBy" />
                                                                    <asp:BoundField HeaderText="Approval Status" DataField="ApprovalStatus" />
                                                                </Columns>
                                                                <FooterStyle CssClass="FooterStyle" />
                                                                <HeaderStyle CssClass="headerstyle" />
                                                                <PagerStyle CssClass="PagerStyle" />
                                                                <RowStyle CssClass="rowstyle" />
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </div>
                                                </div>
                                                <div class="tab-pane" id="iss2">
                                                    <asp:GridView ID="grvdetailAttachments"
                                                        runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                                        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active">
                                                        <RowStyle CssClass="rowstyle" />
                                                        <Columns>
                                                            <asp:BoundField DataField="FilePath" HeaderText="File Name" SortExpression="FilePath" />
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDownload" Text="Download" CommandArgument='<%# Eval("FilePath") %>' runat="server" OnClick="DownloadFile"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
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
                            <footer>
                                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            </footer>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="divprint" style="display: none;">
        <fieldset>
            <table style="width: 100%;">
                <tr>
                    <td style="font-size: large; text-align: center;">
                        <img src="../img/CHAI%20Logo.png" width="130" height="80" />
                        <br />
                        <strong>CHAI ETHIOPIA
                            <br />
                            BANK PAYMENT REQUEST FORM</strong></td>
                </tr>
            </table>
            <table style="width: 100%; border-spacing: 10px;">
                <tr>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblBankPaymentNo" runat="server" Text="Bank Payment No:"></asp:Label>
                        </strong></td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblVoucherNoResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblCheckPayVouchNo" runat="server" Text="Check Payment Voucher No:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 25%;">__________
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblRequester" runat="server" Text="Bank Payment Initiator:"></asp:Label>
                        </strong></td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblRequesterResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 25%; text-align: right;"><strong>
                        <asp:Label ID="lblBankName" runat="server" Text="Beneficiary Bank:"></asp:Label>
                    </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblBankNameResult" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblRequestedDate" runat="server" Text="Requested Date:"></asp:Label>
                        </strong></td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblRequestedDateResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 25%; text-align: right;"><strong>
                        <asp:Label ID="lblBankAccountNo" runat="server" Text="Beneficiary Bank Account No:"></asp:Label>
                    </strong></td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblBankAccountNoResult" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblBeneficiaryName" runat="server" Text="Beneficiary Name:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblBeneficiaryNameResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 25%; text-align: right;"><strong>
                        <asp:Label ID="lblTotalAmount" runat="server" Text="Total Amount:"></asp:Label>
                    </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblTotalAmountResult" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblProjectCode" runat="server" Text="Project ID:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblProjectCodeResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblGrantCode" runat="server" Text="Grant ID:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblGrantCodeResult" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblDescription" runat="server" Text="Description:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblDescriptionResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 25%; text-align: right;"><strong>
                        <asp:Label ID="lblApprovalStatusPrint" runat="server" Text="Approval Status:"></asp:Label>
                    </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblApprovalStatusResult" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblChaiBank" runat="server" Text="CHAI Bank:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblChaiBankResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 25%; text-align: right;"><strong>
                        <asp:Label ID="lblChaiBankAcc" runat="server" Text="CHAI Bank Account No:"></asp:Label>
                    </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblChaiBankAccResult" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblReqNo" runat="server" Text="Payment/Travel/Liquidation/Settlement No:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblReqNoResult" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <asp:GridView ID="grvDetails"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    <asp:BoundField DataField="ItemAccount.AccountName" HeaderText="Account Name" SortExpression="ItemAccount.AccountName" />
                    <asp:BoundField DataField="ItemAccount.AccountCode" HeaderText="Account Code" SortExpression="ItemAccount.AccountCode" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                    <%--<asp:BoundField DataField="ActualExpendture" HeaderText="Actual Expendture" SortExpression="ActualExpendture" />--%>
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
            <br />
            <asp:GridView ID="grvStatuses"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover" OnRowDataBound="grvStatuses_RowDataBound">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date", "{0:dd/MM/yyyy}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Name" />
                    <asp:BoundField DataField="AssignedBy" HeaderText="Assignee Approver" SortExpression="AssignedBy" />
                    <asp:BoundField HeaderText="Approval Status" DataField="ApprovalStatus" />
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
            <br />
            <asp:Panel ID="pnlTravelDetail" Visible="false" runat="server">
                <div style="text-align: center;">
                    <asp:Label ID="lblTravelDetails" Text="Travel Advance Details" Font-Bold="true" Font-Size="Large" runat="server" Visible="false" />
                </div>
                <br />
                <table style="width: 100%; border-spacing: 10px;">
                    <tr>
                        <td style="width: 25%; text-align: right;">
                            <strong>
                                <asp:Label ID="lblTravelReqDate" runat="server" Text="Requested Date:"></asp:Label>
                            </strong></td>
                        <td style="width: 25%;">
                            <asp:Label ID="lblTravelReqDateResult" runat="server"></asp:Label>
                        </td>
                        <td style="width: 25%; text-align: right;">
                            <strong>
                                <asp:Label ID="lblOrigTravelReq" runat="server" Text="Requester:"></asp:Label>
                            </strong></td>
                        <td style="width: 25%;">
                            <asp:Label ID="lblOrigTravelReqResult" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="grvTravelDetails"
                    runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                    CssClass="table table-striped table-bordered table-hover">
                    <RowStyle CssClass="rowstyle" />
                    <Columns>
                        <asp:BoundField DataField="CityFrom" HeaderText="City From" SortExpression="CityFrom" />
                        <asp:BoundField DataField="CityTo" HeaderText="City To" SortExpression="CityTo" />
                        <asp:BoundField DataField="HotelBooked" HeaderText="Hotel Booked" SortExpression="HotelBooked" />
                        <asp:TemplateField HeaderText="From Date">
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("FromDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="To Date">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("ToDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="ModeOfTravel" HeaderText="Mode of Travel" SortExpression="ModeOfTravel" />
                    </Columns>
                    <FooterStyle CssClass="FooterStyle" />
                    <HeaderStyle CssClass="headerstyle" />
                    <PagerStyle CssClass="PagerStyle" />
                    <RowStyle CssClass="rowstyle" />
                </asp:GridView>
                <br />
                <asp:GridView ID="grvTravelCosts"
                    runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                    CssClass="table table-striped table-bordered table-hover">
                    <RowStyle CssClass="rowstyle" />
                    <Columns>
                        <asp:BoundField DataField="ExpenseType.ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseType.ExpenseTypeName" />
                        <asp:BoundField DataField="AccountCode" HeaderText="Account Code" SortExpression="AccountCode" />
                        <asp:BoundField DataField="Days" HeaderText="Days" SortExpression="Days" />
                        <asp:BoundField DataField="UnitCost" HeaderText="Unit Cost" SortExpression="UnitCost" />
                        <asp:BoundField DataField="NoOfUnits" HeaderText="No Of Units" SortExpression="NoOfUnits" />
                        <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
                    </Columns>
                    <FooterStyle CssClass="FooterStyle" />
                    <HeaderStyle CssClass="headerstyle" />
                    <PagerStyle CssClass="PagerStyle" />
                    <RowStyle CssClass="rowstyle" />
                </asp:GridView>
                <br />
                <asp:GridView ID="grvTravelStatuses" OnRowDataBound="grvTravelStatuses_RowDataBound"
                    runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                    CssClass="table table-striped table-bordered table-hover">
                    <RowStyle CssClass="rowstyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date", "{0:dd/MM/yyyy}")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Name" />
                        <asp:BoundField DataField="AssignedBy" HeaderText="Assignee Approver" SortExpression="AssignedBy" />
                        <asp:BoundField HeaderText="Approval Status" DataField="ApprovalStatus" />
                    </Columns>
                    <FooterStyle CssClass="FooterStyle" />
                    <HeaderStyle CssClass="headerstyle" />
                    <PagerStyle CssClass="PagerStyle" />
                    <RowStyle CssClass="rowstyle" />
                </asp:GridView>
            </asp:Panel>
            <asp:Panel ID="pnlLiquidationDetail" runat="server">
                <div style="text-align: center;">
                    <asp:Label ID="lblLiqDetail" Text="Travel Expense Liquidation Details" Font-Bold="true" Font-Size="Large" runat="server" />
                </div>
                <br />
                <asp:DataGrid ID="dgLiquidationDetail" runat="server"
                    AutoGenerateColumns="False" CellPadding="0" CssClass="table table-striped table-bordered table-hover"
                    DataKeyField="Id" GridLines="Both" PagerStyle-CssClass="paginate_button active"
                    ShowFooter="True" OnItemDataBound="dgLiquidationRequestDetail_ItemDataBound">
                    <Columns>
                        <asp:TemplateColumn HeaderText="Account Name">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlEdtAccountDescription" CssClass="form-control" OnSelectedIndexChanged="ddlEdtAccountDescription_SelectedIndexChanged" runat="server" AppendDataBoundItems="true" AutoPostBack="True">
                                    <asp:ListItem Value="0">Select Account</asp:ListItem>
                                </asp:DropDownList>
                                <i></i>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountName")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Account Code">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEdtAccountCode" ReadOnly="true" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountCode")%>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountCode")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Amount Advanced">
                            <FooterTemplate>
                                <asp:Label ID="lblTotalAdvAmount" runat="server" />
                            </FooterTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "AmountAdvanced")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Actual Expenditure">
                            <FooterTemplate>
                                <asp:Label ID="lblTotalActualExp" runat="server" />
                            </FooterTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "ActualExpenditure")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Variance">
                            <FooterTemplate>
                                <asp:Label ID="lblTotalVariance" runat="server" />
                            </FooterTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "Variance")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Project ID">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlEdtProject" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                                    <asp:ListItem Value="0">Select Project</asp:ListItem>
                                </asp:DropDownList>
                                <i></i>
                                <asp:RequiredFieldValidator ID="rfvddlEdtProject" runat="server" ControlToValidate="ddlEdtProject" CssClass="validator" Display="Dynamic" ErrorMessage="Project must be selected" InitialValue="0" SetFocusOnError="true" ValidationGroup="edit"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "Project.ProjectCode")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                </asp:DataGrid>
                <br />
                <asp:GridView ID="grvLiquidationPrintStatuses"
                    runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                    CssClass="table table-striped table-bordered table-hover" OnRowDataBound="grvLiquidationStatuses_RowDataBound">
                    <RowStyle CssClass="rowstyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date", "{0:dd/MM/yyyy}")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Approver" HeaderText="Reviewer" SortExpression="Approver" />
                        <asp:BoundField DataField="AssignedBy" HeaderText="Assignee Approver" SortExpression="AssignedBy" />
                        <asp:BoundField HeaderText="Approval Status" DataField="ApprovalStatus" />
                    </Columns>
                    <FooterStyle CssClass="FooterStyle" />
                    <HeaderStyle CssClass="headerstyle" />
                    <PagerStyle CssClass="PagerStyle" />
                    <RowStyle CssClass="rowstyle" />
                </asp:GridView>
            </asp:Panel>
            <asp:Panel ID="pnlPaymentDetail" runat="server">
                <div style="text-align: center;">
                    <asp:Label ID="lblPaymentDetail" Text="Payment Request Details" Font-Bold="true" Font-Size="Large" runat="server" />
                </div>
                <br />
                <table style="width: 100%; border-spacing: 10px;">
                    <tr>
                        <td style="width: 25%; text-align: right;">
                            <strong>
                                <asp:Label ID="lblPaymentReqDate" runat="server" Text="Requested Date:"></asp:Label>
                            </strong></td>
                        <td style="width: 25%;">
                            <asp:Label ID="lblPaymentReqDateResult" runat="server"></asp:Label>
                        </td>
                        <td style="width: 25%; text-align: right;">
                            <strong>
                                <asp:Label ID="lblOrigPaymentRequester" runat="server" Text="Requester:"></asp:Label>
                            </strong></td>
                        <td style="width: 25%;">
                            <asp:Label ID="lblOrigPaymentRequesterResult" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 25%; text-align: right;">
                            <strong>
                                <asp:Label ID="lblDepTime" runat="server" Text="Departure Date & time:"></asp:Label>
                            </strong></td>
                        <td style="width: 25%;">
                            <asp:Label ID="lblDepTimeResult" runat="server"></asp:Label>
                        </td>
                        <td style="width: 25%; text-align: right;">
                            <strong>
                                <asp:Label ID="lblRetTime" runat="server" Text="Return Date & time:"></asp:Label>
                            </strong></td>
                        <td style="width: 25%;">
                            <asp:Label ID="lblRetTimeResult" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="grvPaymentDetails"
                    runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                    CssClass="table table-striped table-bordered table-hover">
                    <RowStyle CssClass="rowstyle" />
                    <Columns>
                        <asp:BoundField DataField="ItemAccount.AccountName" HeaderText="Account Name" SortExpression="ItemAccount.AccountName" />
                        <asp:BoundField DataField="ItemAccount.AccountCode" HeaderText="Account Code" SortExpression="ItemAccount.AccountCode" />
                        <asp:BoundField DataField="ActualExpendture" HeaderText="Actual Expendture" SortExpression="ActualExpendture" />
                        <asp:BoundField DataField="Project.ProjectCode" HeaderText="Project Code" />
                        <asp:BoundField DataField="Grant.GrantCode" HeaderText="Grant Code" />
                    </Columns>
                    <FooterStyle CssClass="FooterStyle" />
                    <HeaderStyle CssClass="headerstyle" />
                    <PagerStyle CssClass="PagerStyle" />
                    <RowStyle CssClass="rowstyle" />
                </asp:GridView>
                <br />
                <asp:GridView ID="grvPaymentStatuses"
                    runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                    CssClass="table table-striped table-bordered table-hover" OnRowDataBound="grvPaymentStatuses_RowDataBound">
                    <RowStyle CssClass="rowstyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date", "{0:dd/MM/yyyy}")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Name" />
                        <asp:BoundField DataField="AssignedBy" HeaderText="Assignee Approver" SortExpression="AssignedBy" />
                        <asp:BoundField HeaderText="Approval Status" DataField="ApprovalStatus" />
                    </Columns>
                    <FooterStyle CssClass="FooterStyle" />
                    <HeaderStyle CssClass="headerstyle" />
                    <PagerStyle CssClass="PagerStyle" />
                    <RowStyle CssClass="rowstyle" />
                </asp:GridView>
            </asp:Panel>
            <asp:Panel ID="pnlSettelementDetail" runat="server" Visible="false">
                <div style="text-align: center;">
                    <asp:Label ID="lblSettelementDetails" Text="Settlement Details" Font-Bold="true" Font-Size="Large" runat="server" Visible="false" />
                </div>
                <br />
                <asp:GridView ID="grvReDetail"
                    runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                    CssClass="table table-striped table-bordered table-hover">
                    <RowStyle CssClass="rowstyle" />
                    <Columns>
                        <asp:BoundField DataField="ItemAccount.AccountName" HeaderText="Account Name" />
                        <asp:BoundField DataField="ActualExpenditure" HeaderText="Actual Expenditure" />
                        <asp:BoundField DataField="PaymentReimbursementRequest.Project.ProjectCode" HeaderText="Project" />
                    </Columns>
                    <FooterStyle CssClass="FooterStyle" />
                    <HeaderStyle CssClass="headerstyle" />
                    <PagerStyle CssClass="PagerStyle" />
                    <RowStyle CssClass="rowstyle" />
                </asp:GridView>
                <br />
                <asp:GridView ID="grvPRstatus"
                    runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                    CssClass="table table-striped table-bordered table-hover" OnRowDataBound="grvPRstatus_RowDataBound">
                    <RowStyle CssClass="rowstyle" />
                    <Columns>
                        <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                        <asp:BoundField DataField="Approver" HeaderText="Approver" SortExpression="Approver" />
                    </Columns>
                    <FooterStyle CssClass="FooterStyle" />
                    <HeaderStyle CssClass="headerstyle" />
                    <PagerStyle CssClass="PagerStyle" />
                    <RowStyle CssClass="rowstyle" />
                </asp:GridView>
            </asp:Panel>
            <br />
            <table style="width: 100%;">
                <tr>
                    <td></td>
                    <td>Signature</td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td>___________________</td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
        </fieldset>
    </div>

</asp:Content>
