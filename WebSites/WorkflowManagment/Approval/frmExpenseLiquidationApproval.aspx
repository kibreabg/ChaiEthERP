﻿<%@ Page Title="Expense Liquidation Request Approval Form" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmExpenseLiquidationApproval.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Approval.Views.frmExpenseLiquidationApproval" EnableEventValidation="false" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>


<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script src="../js/libs/jquery-2.0.2.min.js"></script>
    <script type="text/javascript">
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
            <h2>Search Expense Liquidation</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-3">
                                <asp:Label ID="lblTravelAdvanceNo" runat="server" Text="Travel Advance No" CssClass="label"></asp:Label>
                                <label class="input">
                                    <asp:TextBox ID="txtSrchTravelAdvanceNo" runat="server" Visible="true"></asp:TextBox>
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
                        <asp:Button ID="btnFind" runat="server" Text="Find" CssClass="btn btn-primary" OnClick="btnFind_Click"></asp:Button>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>
        <asp:GridView ID="grvExpenseLiquidationRequestList"
            runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="grvExpenseLiquidationRequestList_RowCommand"
            OnRowDataBound="grvExpenseLiquidationRequestList_RowDataBound" OnSelectedIndexChanged="grvExpenseLiquidationRequestList_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvExpenseLiquidationRequestList_PageIndexChanging"
            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="30">
            <RowStyle CssClass="rowstyle" />
            <Columns>
                <asp:BoundField DataField="TravelAdvanceRequest.TravelAdvanceNo" HeaderText="Travel Advance No" SortExpression="TravelAdvanceRequest.TravelAdvanceNo" />
                <asp:BoundField DataField="TravelAdvanceRequest.AppUser.FullName" HeaderText="Requester" SortExpression="TravelAdvanceRequest.AppUser.FullName" />
                <asp:BoundField DataField="ExpenseType" HeaderText="Expense Type" SortExpression="ExpenseType" />
                <asp:BoundField DataField="TotalActualExpenditure" HeaderText="Total Actual Expenditure" SortExpression="TotalActualExpenditure" />
                <asp:BoundField DataField="AdditionalComment" HeaderText="Comment" SortExpression="AdditionalComment" />
                <asp:BoundField DataField="ArrivalDateTime" HeaderText="Departure Time" SortExpression="ArrivalDateTime" />
                <asp:BoundField DataField="ReturnDateTime" HeaderText="Return Time" SortExpression="ReturnDateTime" />
                <asp:TemplateField HeaderText="Request Date">
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("RequestDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:ButtonField ButtonType="Button" CommandName="ViewItem" Text="View Item Detail" />
                <asp:CommandField ButtonType="Button" SelectText="Process Request" ShowSelectButton="True" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="btnStatus" Enabled="false" Text="" BorderStyle="None" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle CssClass="FooterStyle" />
            <HeaderStyle CssClass="headerstyle" />
            <PagerStyle CssClass="PagerStyle" />
            <RowStyle CssClass="rowstyle" />
        </asp:GridView>
        <div>
            <asp:Button runat="server" ID="btnInProgress" Enabled="false" Text="" BorderStyle="None" BackColor="#FFFF6C" />
            <b>In Progress</b><br />
            <asp:Button runat="server" ID="btnComplete" Enabled="false" Text="" BorderStyle="None" BackColor="#FF7251" />
            <b>Completed</b><br />
            <asp:Button runat="server" ID="btnAwaitBank" Enabled="false" Text="" BorderStyle="None" BackColor="Green" />
            <b>Awaiting Bank Payment</b>
        </div>
        <br />
    </div>
    <div class="modal fade" id="approvalModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" style="width: 800px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                    <h4 class="modal-title" id="myModalLabel">Process Expense Liquidation Request</h4>
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
                                            <asp:TextBox ID="txtRejectedReason" Visible="false" runat="server"></asp:TextBox>
                                        </label>
                                    </section>
                                </div>
                                <div class="row">
                                    <section class="col col-6">
                                        <asp:Label ID="lblAttachments" runat="server" Text="Attachments" CssClass="label"></asp:Label>
                                        <asp:GridView ID="grvAttachments"
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
                                    </section>
                                </div>
                            </fieldset>
                            <footer>
                                <asp:Button ID="btnApprove" runat="server" ValidationGroup="save" Text="Save" OnClick="btnApprove_Click" Enabled="false" CssClass="btn btn-primary"></asp:Button>
                                <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" Enabled="false" OnClientClick="javascript:Clickheretoprint('divprint');return false;"></asp:Button>
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                                <asp:Button ID="btnBankPayment" runat="server" CssClass="btn btn-default" OnClick="btnBankPayment_Click" Text="Bank Payment" Visible="False" />
                                <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="btn btn-primary" Visible="False" OnClick="btnExport_Click"></asp:Button>
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
                    <h4 class="modal-title">Expense Liquidation Detail</h4>
                </div>
                <div class="modal-body">
                    <div class="jarviswidget-editbox"></div>
                    <div class="widget-body no-padding">
                        <div class="smart-form">
                            <div style="overflow-x: auto;">
                                <asp:DataGrid ID="dgLiquidationRequestDetail" runat="server"
                                    AutoGenerateColumns="False" CellPadding="0" CssClass="table table-striped table-bordered table-hover"
                                    DataKeyField="Id" GridLines="None" PagerStyle-CssClass="paginate_button active"
                                    ShowFooter="True" OnItemDataBound="dgLiquidationRequestDetail_ItemDataBound" OnEditCommand="dgLiquidationRequestDetail_EditCommand" OnUpdateCommand="dgLiquidationRequestDetail_UpdateCommand">
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
                                <div style="text-align: center;">
                                    <asp:Label ID="lblLiquidationApprovalStatus" Font-Size="Large" Font-Bold="true" runat="server" Text="Approval Status"></asp:Label>
                                </div>
                                <asp:GridView ID="grvLiquidationRequestStatuses"
                                    runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                    CssClass="table table-striped table-bordered table-hover" OnRowDataBound="grvLiquidationRequestStatuses_RowDataBound">
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
                            </div>
                            <footer>
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
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
                        <strong>CHAI ETHIOPIA ERP
                            <br />
                            TRAVEL ADVANCE LIQUIDATION TRANSACTION FORM</strong></td>
                </tr>
            </table>
            <table style="width: 100%; border-spacing: 30px;">
                <tr>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblJournalVouchNo" runat="server" Text="Journal Voucher No:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 25%;">____________________
                    </td>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblCheckPayVouchNo" runat="server" Text="Check Payment Voucher No:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 25%;">____________________
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblRequestNo" runat="server" Text="Travel Advance No:"></asp:Label>
                        </strong></td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblRequestNoResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblRequester" runat="server" Text="Requester:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblRequesterResult" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblRequestedDate" runat="server" Text="Liquidation Date:"></asp:Label>
                        </strong></td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblRequestedDateResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblArrRetTime" runat="server" Text="Departure/Return Date & Time:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblArrRetTimeResult" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblCommentPrint" runat="server" Text="Purpose of Travel:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblPurposeofAdvanceResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblApprovalStatusPrint" runat="server" Text="Approval Status:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblApprovalStatusResult" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="grvDetails"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover" OnRowDataBound="grvDetails_RowDataBound" ShowFooter="True">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    <asp:BoundField DataField="ExpenseType.ExpenseTypeName" HeaderText="Expense Type" />
                    <asp:BoundField DataField="ItemAccount.AccountCode" HeaderText="Account Code" />
                    <asp:TemplateField HeaderText="Project ID">
                        <ItemTemplate>
                            <div style="text-align: right;">
                                <asp:Label ID="lblProject" runat="server" Text='<%# Eval("Project.ProjectCode") %>' />
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right;">
                                <asp:Label ID="lblTotal" Text="Total" runat="server" />
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Grant.GrantCode" HeaderText="Grant ID" />
                    <asp:TemplateField HeaderText="Amount Advanced">
                        <ItemTemplate>
                            <div style="text-align: right;">
                                <asp:Label ID="lblAmountAdvanced" runat="server" Text='<%# Eval("AmountAdvanced") %>' />
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right;">
                                <asp:Label ID="lblTotalAmountAdv" runat="server" />
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actual Expenditure">
                        <ItemTemplate>
                            <div style="text-align: right;">
                                <asp:Label ID="lblActualExpenditure" runat="server" Text='<%# Eval("ActualExpenditure") %>' />
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right;">
                                <asp:Label ID="lblTotalActualExp" runat="server" />
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Variance">
                        <ItemTemplate>
                            <div style="text-align: right;">
                                <asp:Label ID="lblVariance" runat="server" Text='<%# Eval("Variance") %>' />
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right;">
                                <asp:Label ID="lblTotalVariance" runat="server" />
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
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
                    <asp:BoundField DataField="Approver" HeaderText="Reviewer" SortExpression="Approver" />
                    <asp:BoundField DataField="AssignedBy" HeaderText="Assignee Approver" SortExpression="AssignedBy" />
                    <asp:BoundField HeaderText="Approval Status" DataField="ApprovalStatus" />
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
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
