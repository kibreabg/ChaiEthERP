<%@ Page Title="Payment Settlement Request Approval Form" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmPaymentReimbursementApproval.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Approval.Views.frmPaymentReimbursementApproval" EnableEventValidation="false" %>

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
            <h2>Search Payment Settlement</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-3">
                                <asp:Label ID="lblExpenseType" runat="server" Text="Expense Type" CssClass="label"></asp:Label>
                                <label class="input">
                                    <asp:TextBox ID="txtSrchExpenseType" runat="server" Visible="true"></asp:TextBox>
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
                        <asp:Button ID="btnFind" runat="server" Text="Find" CssClass="btn btn-primary" OnClick="btnFind_Click"></asp:Button>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>
        <asp:GridView ID="grvPaymentReimbursementRequestList"
            runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="grvPaymentReimbursementRequestList_RowCommand"
            OnRowDataBound="grvPaymentReimbursementRequestList_RowDataBound" OnSelectedIndexChanged="grvPaymentReimbursementRequestList_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvPaymentReimbursementRequestList_PageIndexChanging"
            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="30">
            <RowStyle CssClass="rowstyle" />
            <Columns>
                <asp:BoundField DataField="CashPaymentRequest.RequestNo" HeaderText="Cash Payment Request No." SortExpression="Cash Payment Request No" />
                <asp:BoundField DataField="CashPaymentRequest.AppUser.FullName" HeaderText="Requester" />
                <asp:BoundField DataField="CashPaymentRequest.Supplier.SupplierName" HeaderText="Payable To" />
                <asp:BoundField DataField="RequestDate" HeaderText="Request Date" SortExpression="RequestDate" />
                <asp:BoundField DataField="ReceivableAmount" HeaderText="Amount Advanced Taken" SortExpression="ReceivableAmount" />
                <asp:BoundField DataField="TotalAmount" HeaderText="Total Expenditure" SortExpression="TotalAmount" />
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
                    <h4 class="modal-title" id="myModalLabel">Process Payment Settlement Request</h4>
                    <asp:Label ID="lblOverSpendrefund" ForeColor="Red" runat="server" Font-Size="Medium" Text="Over Spend Refund :- " Visible="true"></asp:Label>
                    <asp:Label ID="lblOverSpend" runat="server" ForeColor="Red" Font-Size="Medium" Text="-" Visible="true"></asp:Label>
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
                                            <asp:RequiredFieldValidator ID="RfvApprovalStatus" runat="server" ValidationGroup="save" ErrorMessage="Approval Status Required" InitialValue="0" ControlToValidate="ddlApprovalStatus"></asp:RequiredFieldValidator>
                                        </label>
                                    </section>
                                    <section class="col col-6">
                                        <asp:Label ID="lblRejectedReason" runat="server" Text="Rejected Reason" Visible="false" CssClass="label"></asp:Label>
                                        <label class="input">
                                            <asp:TextBox ID="txtRejectedReason" Visible="false" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvRejectedReason" runat="server" Enabled="false" CssClass="validator" ValidationGroup="save" ErrorMessage="Must Enter Rejection Reason" ControlToValidate="txtRejectedReason"></asp:RequiredFieldValidator>
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
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                                <asp:Button ID="btnBankPayment" runat="server" CssClass="btn btn-default" OnClick="btnBankPayment_Click" Text="Bank Payment" Visible="False" />
                                <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" Enabled="false" OnClientClick="javascript:Clickheretoprint('divprint')"></asp:Button>
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
                    <h4 class="modal-title">Advance Settlement Detail</h4>
                </div>
                <div class="modal-body">
                    <div class="jarviswidget-editbox"></div>
                    <div class="widget-body no-padding">
                        <div class="smart-form">
                            <div style="overflow-x: auto;">
                                <asp:DataGrid ID="dgReimbursementDetail" runat="server"
                                    AutoGenerateColumns="False" CellPadding="0" CssClass="table table-striped table-bordered table-hover"
                                    DataKeyField="Id" GridLines="None" PagerStyle-CssClass="paginate_button active" ShowFooter="True"
                                    OnItemDataBound="dgReimbursementDetail_ItemDataBound" OnEditCommand="dgReimbursementDetail_EditCommand"
                                    OnUpdateCommand="dgReimbursementDetail_UpdateCommand">
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
                                                <asp:TextBox ID="txtEdtAccountCode" ReadOnly="true" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountCode")%>'></asp:TextBox>
                                            </EditItemTemplate>
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
                                    <asp:Label ID="lblSettlmentApprovalStatus" Font-Size="Large" Font-Bold="true" runat="server" Text="Approval Status"></asp:Label>
                                </div>
                                <asp:GridView ID="grvPaymentReimbursementRequestStatuses"
                                    runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                    CssClass="table table-striped table-bordered table-hover" OnRowDataBound="grvPaymentReimbursementRequestStatuses_RowDataBound">
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
                            CASH PAYMENT SETTLEMENT TRANSACTION FORM</strong></td>
                </tr>
            </table>
            <table style="width: 100%; border-spacing: 30px;">
                <tr>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblRequestNo" runat="server" Text="Payment Request No:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblRequestNoResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblRequestedDate" runat="server" Text="Requested Date:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblRequestedDateResult" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 25%; text-align: right;"><strong>
                        <asp:Label ID="lblAdvanceTaken" runat="server" Text="Total Advance Taken:"></asp:Label>
                    </strong></td>
                    <td style="width: 25%;">
                        <asp:Label ID="lbladvancetakenresult" runat="server"></asp:Label></td>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblActualExpenditure" runat="server" Text="Total Expenditure:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblActualExpenditureresult" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblRequester" runat="server" Text="Requester:"></asp:Label>
                        </strong></td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblRequesterResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblEmployeeNo" runat="server" Text="Employee No:"></asp:Label>
                        </strong></td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblEmpNoResult" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblCommentPrint" runat="server" Text="Comment:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblCommentResult" runat="server"></asp:Label>
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
            <asp:GridView ID="grvStatuses"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover" OnRowDataBound="grvStatuses_RowDataBound">
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
            <br />
            <table style="width: 100%;">
                <tr>
                    <td></td>
                    <td>Signature</td>
                    <td></td>
                    <td></td>
                    <td style="text-align: right; padding-right: 6%;">Received By </td>
                </tr>
                <tr>
                    <td></td>
                    <td>___________________</td>
                    <td></td>
                    <td></td>
                    <td style="text-align: right;">___________________</td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
