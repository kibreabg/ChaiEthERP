<%@ Page Title="Store Request Approval Form" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmStoreApproval.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Approval.Views.frmStoreApproval" EnableEventValidation="false" %>

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
    </script>
    <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Search Store Requests</h2>
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
            <asp:GridView ID="grvStoreRequestList" Width="100%"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="grvStoreRequestList_RowCommand"
                OnRowDataBound="grvStoreRequestList_RowDataBound" OnSelectedIndexChanged="grvStoreRequestList_SelectedIndexChanged"
                AllowPaging="True" OnPageIndexChanging="grvStoreRequestList_PageIndexChanging"
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
                    <asp:ButtonField ButtonType="Button" CommandName="ViewItem" Text="View Item Detail" />
                    <asp:CommandField ButtonType="Button" SelectText="Process Request" ShowSelectButton="True" />
                    <asp:ButtonField ButtonType="Button" CommandName="Issue" Text="Issue" />
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
                    <h4 class="modal-title" id="myModalLabel">Process Store Request</h4>
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
    <asp:Panel ID="pnlDetail" runat="server">
        <div class="modal-body no-padding">
            <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
                <header>
                    <span class="widget-icon"><i class="fa fa-edit"></i></span>
                    <h2>Store Request Details</h2>
                </header>
                <div>
                    <div class="jarviswidget-editbox"></div>
                    <div class="widget-body no-padding">
                        <div class="smart-form">
                            <asp:DataGrid ID="dgStoreRequestDetail" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0" TextWrap="Wrap"
                                CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                                GridLines="None" OnCancelCommand="dgStoreRequestDetail_CancelCommand" OnEditCommand="dgStoreRequestDetail_EditCommand"
                                OnItemDataBound="dgStoreRequestDetail_ItemDataBound" OnUpdateCommand="dgStoreRequestDetail_UpdateCommand"
                                ShowFooter="True">

                                <Columns>

                                    <asp:TemplateColumn HeaderText="Item">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Item.Name")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlItem" DataTextField="Name" DataValueField="Id" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Requested Quantity">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Qty")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Approved Quantity">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "QtyApproved")%>
                                        </ItemTemplate>
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
                            <footer>
                                <asp:Button ID="btnCancelPopup2" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary"></asp:Button>
                            </footer>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender runat="server" BackgroundCssClass="modalBackground"
        Enabled="True" TargetControlID="btnPop2" PopupControlID="pnlDetail" CancelControlID="btnCancelPopup2"
        ID="pnlDetail_ModalPopupExtender">
    </asp:ModalPopupExtender>
    <div id="divprint" style="display: none;">
        <table style="width: 100%;">
            <tr>
                <td style="width: 17%; text-align: left;">
                    <img src="../img/CHAI%20Logo.png" width="70" height="50" /></td>
                <td style="font-size: large; text-align: center;">
                    <strong>CHAI ETHIOPIA
                           
                        <br />
                        STORE REQUEST FORM</strong></td>
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

            </tr>
            <tr>
                <td style="width: 576px; height: 18px; padding-left: 15%;">
                    <strong>
                        <asp:Label ID="lblSpecialNeed" runat="server" Text="Comment:"></asp:Label>
                    </strong></td>
                <td style="width: 490px" class="modal-sm">
                    <asp:Label ID="lblRemarkResult" runat="server"></asp:Label>
                </td>

                <td style="width: 389px">
                    <asp:Label ID="lblDelivertoResult" runat="server"></asp:Label>
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
        <asp:GridView ID="grvDetails"
            runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            CssClass="table table-striped table-bordered table-hover">
            <RowStyle CssClass="rowstyle" />
            <Columns>
                <asp:BoundField DataField="Item.Name" HeaderText="Item" SortExpression="Item.Name" />
                <asp:BoundField DataField="Qty" HeaderText="Requested Quantity" SortExpression="Qty" />
                <asp:BoundField DataField="QtyApproved" HeaderText="Approved Quantity" SortExpression="QtyApproved" />
                <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />
                <asp:BoundField DataField="Project.ProjectCode" HeaderText="Project Code" />
                <asp:BoundField DataField="Grant.GrantCode" HeaderText="Grant Code" />
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
