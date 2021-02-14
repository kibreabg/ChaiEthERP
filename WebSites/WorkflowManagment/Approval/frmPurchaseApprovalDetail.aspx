<%@ Page Title="Purchase Request Approval Form" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmPurchaseApprovalDetail.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Approval.Views.frmPurchaseApprovalDetail" EnableEventValidation="false" %>

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
            docprint.document.write('<html><head><title>CHAI Ethiopia</title>');
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
            <h2>Search Purchase Requests</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-3">
                                <asp:Label ID="lblSrchRequestNo" runat="server" Text="Request No" CssClass="label"></asp:Label>
                                <label class="input">
                                    <asp:TextBox ID="txtSrchRequestNo" runat="server"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-3">
                                <asp:Label ID="lblSrchRequestDate" runat="server" Text="Request Date" CssClass="label"></asp:Label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtSrchRequestDate" CssClass="form-control datepicker" data-dateformat="mm/dd/yy" runat="server"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-3">
                                <asp:Label ID="lblSrchProgressStatus" runat="server" Text="Status" CssClass="label"></asp:Label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlSrchProgressStatus" runat="server">
                                    </asp:DropDownList><i></i>
                                </label>
                            </section>
                        </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnpop" runat="server" />
                        <asp:Button ID="btnPop2" runat="server" />
                        <asp:Button ID="btnFind" runat="server" Text="Find" CssClass="btn btn-primary" OnClick="btnFind_Click"></asp:Button>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>

                    </footer>
                </div>
            </div>
        </div>
        <div class="table-responsive">
            <asp:GridView ID="grvPurchaseRequestList" Width="100%"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="grvPurchaseRequestList_RowCommand"
                OnRowDataBound="grvPurchaseRequestList_RowDataBound" OnSelectedIndexChanged="grvPurchaseRequestList_SelectedIndexChanged"
                AllowPaging="True" OnPageIndexChanging="grvPurchaseRequestList_PageIndexChanging"
                CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="30">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    <asp:BoundField DataField="RequestNo" HeaderText="Request No" SortExpression="RequestNo" />
                    <asp:BoundField DataField="Requester" HeaderText="Requester" SortExpression="Requester" />
                    <asp:TemplateField HeaderText="Request Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("RequestedDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Required Date of Delivery">
                        <ItemTemplate>
                            <asp:Label ID="lblDateDelivery" runat="server" Text='<%# Eval("Requireddateofdelivery", "{0:dd/MM/yyyy}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Maint Req No - Plate No">
                        <ItemTemplate>
                            <asp:Label ID="lblMaintReqPlateNo" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="SuggestedSupplier" HeaderText="Suggested Supplier" SortExpression="PurposeOfTravel" />
                    <asp:ButtonField ButtonType="Button" CommandName="ViewItem" Text="View Item Detail" />
                    <asp:CommandField ButtonType="Button" SelectText="Process Request" ShowSelectButton="True" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button runat="server" ID="btnStatus" Text="" BorderStyle="None" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
        </div>
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
                    <h4 class="modal-title" id="myModalLabel">Process Purchase Request</h4>
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
                                        <asp:Label ID="lblRejectedReason" runat="server" Text="Rejected/Canceled Reason" Visible="false" CssClass="label"></asp:Label>
                                        <label class="input">
                                            <asp:TextBox ID="txtRejectedReason" Visible="false" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvRejectedReason" runat="server" Enabled="false" CssClass="validator" ValidationGroup="save" ErrorMessage="Must Enter Rejection Reason" ControlToValidate="txtRejectedReason"></asp:RequiredFieldValidator>
                                        </label>
                                    </section>
                                </div>
                                <div class="row">
                                    <section class="col col-6">
                                        <asp:LinkButton runat="server" ID="lnkBidRequest" Visible="true" Text="Prepare Bid Analysis" OnClick="lnkBidRequest_Click" CssClass="btn btn-primary"></asp:LinkButton><br />
                                        <br />
                                        <asp:LinkButton runat="server" ID="lnkSoleVendor" Visible="true" Text="Prepare Sole Vendor Verification" OnClick="lnkSoleVendor_Click" CssClass="btn btn-primary"></asp:LinkButton>
                                    </section>
                                </div>
                            </fieldset>
                            <footer>
                                <asp:Button ID="btnApprove" runat="server" ValidationGroup="save" Text="Save" OnClick="btnApprove_Click" Enabled="false" CssClass="btn btn-primary"></asp:Button>
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
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
                    <h4 class="modal-title">Purchase Request Details</h4>
                </div>
                <div class="modal-body">
                    <div class="jarviswidget-editbox"></div>
                    <div class="widget-body no-padding">
                        <div class="smart-form">
                            <div style="overflow-x: auto;">
                                <asp:DataGrid ID="dgPurchaseRequestDetail" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0" TextWrap="Wrap"
                                    CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                                    GridLines="None" OnCancelCommand="dgPurchaseRequestDetail_CancelCommand" OnEditCommand="dgPurchaseRequestDetail_EditCommand"
                                    OnItemDataBound="dgPurchaseRequestDetail_ItemDataBound" OnUpdateCommand="dgPurchaseRequestDetail_UpdateCommand"
                                    ShowFooter="True">

                                    <Columns>
                                        <asp:TemplateColumn HeaderText="Account Name">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountName")%>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlAccount" runat="server" CssClass="form-control"
                                                    AppendDataBoundItems="True" DataTextField="AccountName" DataValueField="Id" AutoPostBack="True" OnSelectedIndexChanged="ddlAccount_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">Select Account</asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Account Code">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "AccountCode")%>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtAccountCode" runat="server" CssClass="form-control" Enabled="false" Text=' <%# DataBinder.Eval(Container.DataItem, "AccountCode")%>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Item">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "ItemDescription")%>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtItem" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "ItemDescription")%>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Requested Quantity">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Qty")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Approved Quantity">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "ApprovedQuantity")%>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtApprovedQuantity" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "ApprovedQuantity")%>'></asp:TextBox>
                                            </EditItemTemplate>
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
                                                    <asp:ListItem Value="Other">Other</asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "UnitOfMeasurment")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Purpose of Purchase">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlPurposeOfPurchase" runat="server" CssClass="form-control"
                                                    AppendDataBoundItems="True" ValidationGroup="proedit">
                                                    <asp:ListItem Value="0">Select Purpose</asp:ListItem>
                                                    <asp:ListItem Value="Office Use">Office Use</asp:ListItem>
                                                    <asp:ListItem Value="Training">Training</asp:ListItem>
                                                    <asp:ListItem Value="Donation">Donation</asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "PurposeOfPurchase")%>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlPurposeOfPurchase" runat="server" CssClass="form-control"
                                                    AppendDataBoundItems="True" ValidationGroup="proedit">
                                                    <asp:ListItem Value="0">Select Purpose</asp:ListItem>
                                                    <asp:ListItem Value="Office Use">Office Use</asp:ListItem>
                                                    <asp:ListItem Value="Training">Training</asp:ListItem>
                                                    <asp:ListItem Value="Donation">Donation</asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Remark">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Remark")%>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "Remark")%>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Project ID">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Project.ProjectCode")%>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control"
                                                    AppendDataBoundItems="True" DataTextField="ProjectCode" DataValueField="Id"
                                                    ValidationGroup="proedit" AutoPostBack="True" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">Select Project</asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Grant ID">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Grant.GrantCode")%>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtGrant" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "Grant.GrantCode")%>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Actions">
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" ValidationGroup="proedit" CssClass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                    <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                </asp:DataGrid>
                                <div style="text-align: center;">
                                    <asp:Label ID="lblMainDetail" Font-Size="Large" Font-Bold="true" runat="server" Visible="false" Text="Maintenance Request Review Detail"></asp:Label>
                                </div>
                                <asp:GridView ID="grvPreviewDetail" CellPadding="5" CellSpacing="3"
                                    runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                    CssClass="table table-striped table-bordered table-hover">
                                    <Columns>
                                        <asp:BoundField DataField="ServiceType.Name" HeaderText="Service Type" SortExpression="ServiceType.Name" />
                                        <asp:BoundField DataField="DriverServiceTypeDetail.Description" HeaderText="Driver's Service Type Request" SortExpression="DriverServiceTypeDetail.Description" />
                                        <asp:BoundField DataField="MechanicServiceTypeDetail.Description" HeaderText="Mechanic's Service Type Reccomendation" SortExpression="MechanicServiceTypeDetail.Description" />
                                        <asp:BoundField DataField="TechnicianRemark" HeaderText="Mechanic's Remark" SortExpression="TechnicianRemark" />

                                    </Columns>
                                    <FooterStyle CssClass="FooterStyle" />
                                    <HeaderStyle CssClass="headerstyle" />
                                    <PagerStyle CssClass="PagerStyle" />
                                    <RowStyle CssClass="rowstyle" />
                                </asp:GridView>

                                <div style="text-align: center;">
                                    <asp:Label ID="lblReqItems" Font-Size="Large" Font-Bold="true" runat="server" Visible="false" Text="Requested Items During Maintenance Review"></asp:Label>
                                </div>
                                <asp:GridView ID="grvSparepart" CellPadding="5" CellSpacing="3"
                                    runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                    CssClass="table table-striped table-bordered table-hover">
                                    <Columns>
                                        <asp:BoundField DataField="Item.Name" HeaderText="Item" SortExpression="Item.Name" />


                                    </Columns>
                                    <FooterStyle CssClass="FooterStyle" />
                                    <HeaderStyle CssClass="headerstyle" />
                                    <PagerStyle CssClass="PagerStyle" />
                                    <RowStyle CssClass="rowstyle" />
                                </asp:GridView>
                                <div style="text-align: center;">
                                    <asp:Label ID="lblApprovalDet" Font-Size="Large" Font-Bold="true" runat="server" Visible="false" Text="Maintenance Approval Detail"></asp:Label>
                                </div>
                                <asp:GridView ID="grvMaintenanceStatuses" OnRowDataBound="grvMaintenanceStatuses_RowDataBound"
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
    <div id="divprint" style="display: none; text-align: center;">
        <table style="width: 100%;">
            <tr>
                <td style="font-size: large; text-align: center;">
                    <img src="../img/CHAI%20Logo.png" width="130" height="80" />
                    <br />
                    <strong>CHAI ETHIOPIA
                            <br />
                        PURCHASE REQUEST FORM</strong></td>
            </tr>
        </table>
        <table style="width: 75%;">
            <tr>
                <td style="width: 25%;">
                    <strong>
                        <asp:Label ID="lblRequestNo" runat="server" Text="Request No:"></asp:Label>
                    </strong></td>
                <td style="width: 25%;">
                    <asp:Label ID="lblRequestNoResult" runat="server"></asp:Label>
                </td>
                <td style="width: 25%;">
                    <strong>
                        <asp:Label ID="lblReqDateofDelivery" runat="server" Text="Required Date of Delivery:"></asp:Label>
                    </strong>
                </td>
                <td style="width: 25%;">
                    <asp:Label ID="lblReqDateofDeliveryResult" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 25%;">
                    <strong>
                        <asp:Label ID="lblRequestedDate" runat="server" Text="Requested Date:"></asp:Label>
                    </strong></td>
                <td style="width: 25%;">
                    <asp:Label ID="lblRequestedDateResult" runat="server"></asp:Label>
                </td>
                <td style="width: 25%;">
                    <strong>
                        <asp:Label ID="lblSuggestedSupplier" runat="server" Text="Suggested Supplier:"></asp:Label>
                    </strong>
                </td>
                <td style="width: 25%;">
                    <asp:Label ID="lblSuggestedSupplierResult" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 25%;">
                    <strong>
                        <asp:Label ID="lblSpecialNeed" runat="server" Text="Comment:"></asp:Label>
                    </strong></td>
                <td style="width: 25%;">
                    <asp:Label ID="lblRemarkResult" runat="server"></asp:Label>
                </td>
                <td style="width: 25%;">
                    <strong>
                        <asp:Label ID="Label1" runat="server" Text="Deliver To:"></asp:Label>
                    </strong>
                </td>
                <td style="width: 25%;">
                    <asp:Label ID="lblDelivertoResult" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 25%;">
                    <strong>
                        <asp:Label ID="lblRequester" runat="server" Text="Requester:"></asp:Label>
                    </strong></td>
                <td style="width: 25%;">
                    <asp:Label ID="lblRequesterResult" runat="server"></asp:Label>
                </td>
                <td style="width: 25%;">
                <td style="width: 25%;"></td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="grvDetails"
            runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            CssClass="table table-striped table-bordered table-hover">
            <RowStyle CssClass="rowstyle" />
            <Columns>
                <asp:BoundField DataField="ItemAccount.AccountName" HeaderText="AccountName" SortExpression="ItemAccount.AccountName" />
                <asp:BoundField DataField="ItemAccount.AccountCode" HeaderText="Account Code" SortExpression="ItemAccount.AccountCode" />
                <asp:BoundField DataField="Qty" HeaderText="Quantity" SortExpression="Qty" />
                <asp:BoundField DataField="PurposeOfPurchase" HeaderText="Purpose of Purchase" SortExpression="PurposeOfPurchase" />
                <asp:BoundField DataField="ItemDescription" HeaderText="Item" SortExpression="ItemDescription" />
                <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />
                <asp:BoundField DataField="Project.ProjectCode" HeaderText="Project Code" />
                <asp:BoundField DataField="Grant.GrantCode" HeaderText="Grant Code" />
            </Columns>
            <FooterStyle CssClass="FooterStyle" />
            <HeaderStyle CssClass="headerstyle" />
            <PagerStyle CssClass="PagerStyle" />
            <RowStyle CssClass="rowstyle" />
        </asp:GridView>

        <div style="text-align: center;">
            <asp:Label ID="Label2" Font-Bold="true" runat="server" Text="Maintenance Request Review Detail"></asp:Label>
        </div>
        <asp:GridView ID="grvMaintenaceDet" CellPadding="5" CellSpacing="3"
            runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            CssClass="table table-striped table-bordered table-hover">
            <Columns>
                <asp:BoundField DataField="ServiceType.Name" HeaderText="Service Type" SortExpression="ServiceType.Name" />
                <asp:BoundField DataField="DriverServiceTypeDetail.Description" HeaderText="Driver's Service Type Request" SortExpression="DriverServiceTypeDetail.Description" />
                <asp:BoundField DataField="MechanicServiceTypeDetail.Description" HeaderText="Mechanic's Service Type Reccomendation" SortExpression="MechanicServiceTypeDetail.Description" />
                <asp:BoundField DataField="TechnicianRemark" HeaderText="Mechanic's Remark" SortExpression="TechnicianRemark" />

            </Columns>
            <FooterStyle CssClass="FooterStyle" />
            <HeaderStyle CssClass="headerstyle" />
            <PagerStyle CssClass="PagerStyle" />
            <RowStyle CssClass="rowstyle" />
        </asp:GridView>


        <div style="text-align: center;">
            <asp:Label ID="Label3" runat="server" Text="Maintenance Approval Detail"></asp:Label>
        </div>
        <asp:GridView ID="grvMainSta" OnRowDataBound="grvMaintenanceStatuses_RowDataBound"
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
        <br />
        <br />
        <asp:GridView ID="grvStatuses" CellPadding="5" CellSpacing="3"
            runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            CssClass="table table-striped table-bordered table-hover" OnRowDataBound="grvStatuses_RowDataBound">
            <RowStyle CssClass="rowstyle" />
            <Columns>
                <asp:TemplateField HeaderText="Date">
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("ApprovalDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField HeaderText="Approver" />
                <asp:BoundField DataField="AssignedBy" HeaderText="Assignee Approver" SortExpression="AssignedBy" />
                <asp:BoundField DataField="ApprovalStatus" HeaderText="Approval Status" SortExpression="ApprovalStatus" />

            </Columns>
            <FooterStyle CssClass="FooterStyle" />
            <HeaderStyle CssClass="headerstyle" />
            <PagerStyle CssClass="PagerStyle" />
            <RowStyle CssClass="rowstyle" />
        </asp:GridView>
    </div>

</asp:Content>
