<%@ Page Title="Travel Log Attach Form" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmTravelLogAttach.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Approval.Views.frmTravelLogAttach" EnableEventValidation="false" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
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
    </script>
    <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Search Vehicle Requests</h2>
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
                        </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnpop" runat="server" />
                        <asp:Button ID="btnFind" runat="server" Text="Find" CssClass="btn btn-primary" OnClick="btnFind_Click"></asp:Button>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>
        <asp:GridView ID="grvVehicleRequestList"
            runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="grvVehicleRequestList_RowCommand"
            OnRowDataBound="grvVehicleRequestList_RowDataBound" OnSelectedIndexChanged="grvVehicleRequestList_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvVehicleRequestList_PageIndexChanging"
            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="30">
            <RowStyle CssClass="rowstyle" />
            <Columns>
                <asp:BoundField DataField="RequestNo" HeaderText="Request No" SortExpression="RequestNo" />
                <asp:BoundField DataField="AppUser.FullName" HeaderText="Requester" SortExpression="AppUser.FullName" />
                <asp:TemplateField HeaderText="Request Date">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestDate" runat="server" Text='<%# Eval("RequestDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Departure Date">
                    <ItemTemplate>
                        <asp:Label ID="lblReportedDate" runat="server" Text='<%# Eval("DepartureDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DepartureTime" HeaderText="Departure Time" SortExpression="DepartureTime" />
                <asp:TemplateField HeaderText="Returning Date">
                    <ItemTemplate>
                        <asp:Label ID="lblReturningDate" runat="server" Text='<%# Eval("ReturningDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Destination" HeaderText="Destination" SortExpression="Destination" />

                <asp:BoundField DataField="PurposeOfTravel" HeaderText="Purpose Of Travel" SortExpression="PurposeOfTravel" />
                <asp:BoundField DataField="Comment" HeaderText="Comment" SortExpression="Comment" />
                <asp:CommandField ButtonType="Button" SelectText="Attach Travel Log" ShowSelectButton="True" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="btnStatus" Text="" BorderStyle="None" />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:ButtonField ButtonType="Button" Text="Travel Log" CommandName="TravelLog" />--%>
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
    <asp:Panel ID="pnlApproval" Style="position: absolute; top: 10%; left: 20%;" runat="server">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                    <h4 class="modal-title" id="myModalLabel">Attach Travel Log</h4>
                </div>
                <div class="modal-body">
                    <div class="jarviswidget-editbox"></div>
                    <div class="widget-body no-padding">
                        <div class="smart-form">
                            <fieldset>
                                <div class="row">
                                    <section class="col col-6">
                                        <asp:Label ID="lblActDaysTrav" runat="server" Text="Actual Days Travelled" CssClass="label"></asp:Label>
                                        <label class="input">
                                            <asp:TextBox ID="txtActualDaysTrav" runat="server" Width="100%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtActualDaysTrav" ID="txtActualDaysTrav_FilteredTextBoxExtender" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                        </label>
                                        <asp:RequiredFieldValidator
                                            ID="rfvActualDaysTrav" runat="server" ErrorMessage="Please enter actual days travelled" Display="Dynamic"
                                            CssClass="validator" ValidationGroup="approve" ControlToValidate="txtActualDaysTrav"></asp:RequiredFieldValidator>
                                    </section>
                                    <section class="col col-6">
                                        <label class="col-md-6 control-label">Attach Travel Log</label>
                                        <div class="form-group">
                                            <div class="smart-form">
                                                <div class="input input-file">
                                                    <asp:FileUpload ID="fuTravelLog" runat="server" />
                                                </div>
                                            </div>
                                            <asp:RequiredFieldValidator
                                            ID="rfvTravelLog" runat="server" ErrorMessage="Please attach Travel Log" Display="Dynamic"
                                            CssClass="validator" ValidationGroup="approve" ControlToValidate="fuTravelLog"></asp:RequiredFieldValidator>
                                        </div>
                                    </section>
                                </div>
                            </fieldset>
                            <footer>
                                <asp:Button ID="btnSave" runat="server" ValidationGroup="approve" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-primary"></asp:Button>
                                <asp:Button ID="btnCancelPopup" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnCancelPopup_Click"></asp:Button>
                                <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" Enabled="false" OnClientClick="javascript:Clickheretoprint('divprint')"></asp:Button>
                            </footer>
                        </div>
                    </div>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender runat="server" BackgroundCssClass="modalBackground" Enabled="True" PopupControlID="pnlApproval"
        TargetControlID="btnPop" CancelControlID="btnCancelPopup" ID="pnlApproval_ModalPopupExtender">
    </asp:ModalPopupExtender>
    <div id="divprint" style="display: none;">
        <table style="width: 100%;">
            <tr>
                <td style="width: 17%; text-align: left;">
                    <img src="../img/CHAI%20Logo.png" width="70" height="50" /></td>
                <td style="font-size: large; text-align: center;">
                    <strong>CHAI Ethiopia ERP
                            <br />
                        VEHICLE REQUEST FORM</strong></td>
            </tr>
        </table>
        <table style="width: 100%;">

            <tr>
                <td align="right" style="width: 576px">&nbsp;</td>
                <td align="right" style="width: 490px" class="modal-sm">&nbsp;</td>
                <td align="right" style="width: 280px" class="modal-sm">&nbsp;</td>
                <td align="right" style="width: 389px">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 576px; height: 18px; padding-left: 15%;">
                    <strong>
                        <asp:Label ID="lblRequestNo" runat="server" Text="Request No:"></asp:Label>
                    </strong></td>
                <td style="width: 490px" class="modal-sm">
                    <asp:Label ID="lblRequestNoResult" runat="server"></asp:Label>
                </td>
                <td style="width: 280px" class="modal-sm"><strong>
                    <asp:Label ID="lblPurposeTravel" runat="server" Text="Purpose Of Travel:"></asp:Label>
                </strong>
                </td>
                <td style="width: 389px">
                    <asp:Label ID="lblPurposeOfTravelResult" runat="server"></asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 576px; height: 18px; padding-left: 15%;">
                    <strong>
                        <asp:Label ID="lblRequestedDate" runat="server" Text="Requested Date:"></asp:Label>
                    </strong></td>
                <td style="width: 490px" class="modal-sm">
                    <asp:Label ID="lblRequestedDateResult" runat="server"></asp:Label>
                </td>
                <td style="width: 280px" class="modal-sm">
                    <strong>
                        <asp:Label ID="lblApprovalStatusPrint" runat="server" Text="Progress Status:"></asp:Label>
                    </strong>
                </td>
                <td style="width: 389px">
                    <asp:Label ID="lblApprovalStatusResult" runat="server"></asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 576px; height: 18px; padding-left: 15%;">
                    <strong>
                        <asp:Label ID="lblProjectId" runat="server" Text="ProjectID:"></asp:Label>
                    </strong></td>
                <td style="width: 490px; padding-left: 20%;">
                    <asp:Label ID="lblProjectIdResult" runat="server"></asp:Label>
                </td>
                <td style="width: 280px" class="modal-sm">
                    <strong>
                        <asp:Label ID="lblDepartureTime" runat="server" Text="Departure Time:"></asp:Label>
                    </strong>
                </td>
                <td style="width: 389px">
                    <asp:Label ID="lblDepartureTimeResult" runat="server"></asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 576px; height: 18px; padding-left: 15%;">
                    <strong>
                        <asp:Label ID="lblRequester" runat="server" Text="Requester:"></asp:Label>
                    </strong></td>
                <td style="width: 490px" class="modal-sm">
                    <asp:Label ID="lblRequesterResult" runat="server"></asp:Label>
                </td>
                <td style="width: 280px" class="modal-sm">&nbsp;</td>
                <td style="width: 389px"></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 576px; height: 18px; padding-left: 15%;">
                    <strong>
                        <asp:Label ID="lblDepartureDate" runat="server" Text="Departure Date:"></asp:Label>
                    </strong>
                </td>
                <td style="width: 490px; height: 18px;">
                    <asp:Label ID="lblDepartureDateResult" runat="server"></asp:Label>
                </td>
                <td style="width: 280px; height: 18px;">
                    <strong>
                        <asp:Label ID="lblReturningDate" runat="server" Text="Returning Date:"></asp:Label></strong></td>
                <td style="width: 389px; height: 18px;">
                    <asp:Label ID="lblReturningDateResult" runat="server"></asp:Label>
                </td>
                <td style="height: 18px">&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 576px; height: 18px; padding-left: 15%;">
                    <strong>
                        <asp:Label ID="lblDestination" runat="server" Text="Destination:"></asp:Label>
                    </strong></td>
                <td style="width: 490px" class="modal-sm">
                    <asp:Label ID="lblDestinationResult" runat="server"></asp:Label>
                </td>
                <td style="width: 280px" class="modal-sm">
                    <strong>
                        <asp:Label ID="lblNoOfPassengers" runat="server" Text="No of Passengers:"></asp:Label>
                    </strong>
                </td>
                <td style="width: 389px">
                    <asp:Label ID="lblNoOfPassengersResult" runat="server"></asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 576px; height: 18px; padding-left: 15%;">&nbsp;</td>
                <td style="width: 490px; height: 18px;">&nbsp;</td>
                <td style="width: 280px; height: 18px;">&nbsp;</td>
                <td style="width: 389px; height: 18px;">&nbsp;</td>
                <td style="height: 18px"></td>
            </tr>
            <tr>
                <td style="width: 576px; height: 18px; padding-left: 15%;">&nbsp;</td>
                <td style="width: 490px; height: 18px;">&nbsp;</td>
                <td style="height: 18px; width: 280px;">&nbsp;</td>
                <td></td>
                <td></td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="grvVehcles" CellPadding="5" CellSpacing="3"
            runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            CssClass="table table-striped table-bordered table-hover">
            <RowStyle CssClass="rowstyle" />
            <Columns>
                <asp:BoundField DataField="PlateNo" HeaderText="PlateNo" SortExpression="PlateNo" />
                <%-- <asp:BoundField DataField="AssignedVehicle" HeaderText="Assigned Vehicle" SortExpression="AssignedVehicle" />--%>
                <asp:BoundField DataField="AppUser.FullName" HeaderText="Allocated Driver" SortExpression="AppUser.FullName" />
                <asp:BoundField DataField="CarRental.Name" HeaderText="Hired Car" SortExpression="CarRental.Name" />
                <asp:BoundField DataField="FuelCardNumber" HeaderText="Fuel Card No" SortExpression="FuelCardNumber" />
            </Columns>
            <FooterStyle CssClass="FooterStyle" />
            <HeaderStyle CssClass="headerstyle" />
            <PagerStyle CssClass="PagerStyle" />
            <RowStyle CssClass="rowstyle" />
        </asp:GridView>
        <br />
        <asp:GridView ID="grvStatuses" CellPadding="5" CellSpacing="3"
            runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowDataBound="grvStatuses_RowDataBound"
            CssClass="table table-striped table-bordered table-hover">
            <RowStyle CssClass="rowstyle" />
            <Columns>
                <asp:TemplateField HeaderText="Date">
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date", "{0:dd/MM/yyyy}")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField HeaderText="Approver" />
                <asp:BoundField DataField="AssignedBy" HeaderText="Assignee Approver" SortExpression="AssignedBy" />
                <asp:BoundField DataField="ApprovalStatus" HeaderText="Approval Status" SortExpression="ApprovalStatus" />
                <asp:BoundField DataField="Comment" HeaderText="Comment" SortExpression="Comment" />
            </Columns>
            <FooterStyle CssClass="FooterStyle" />
            <HeaderStyle CssClass="headerstyle" />
            <PagerStyle CssClass="PagerStyle" />
            <RowStyle CssClass="rowstyle" />
        </asp:GridView>
    </div>

</asp:Content>
