<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmMaintenanceApproval.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Approval.Views.frmMaintenanceApproval" %>

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
            docprint.document.write('<html><head><title>CHAI Ethiopia</title>');
            docprint.document.write('</head><body onLoad="self.print()"><center>');
            docprint.document.write(content_vlue);
            docprint.document.write('</center></body></html>');
            docprint.document.close();
            docprint.focus();
        }
        function showMechanicDetail() {
            $(document).ready(function () {
                $('#mechanicModal').modal('show');
            });
        }

        function showApproverDetail() {
            $(document).ready(function () {
                $('#approverModal').modal('show');
            });
        }
    </script>
    <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Search Maintenance Request</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-3">
                                <asp:Label ID="lblRequestNosearch" runat="server" Text="Request No" CssClass="label"></asp:Label>

                                <label class="input">
                                    <asp:TextBox ID="txtRequestNosearch" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-3">
                                <asp:Label ID="lblRequestDatesearch" runat="server" Text="Request Date" CssClass="label"></asp:Label>

                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtRequestDatesearch" CssClass="form-control datepicker" data-dateformat="mm/dd/yy" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-3">
                                <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="label"></asp:Label>

                                <label class="select">
                                    <asp:DropDownList ID="ddlProgressStatus" runat="server"></asp:DropDownList><i></i>

                                </label>
                            </section>
                        </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnPop" runat="server" />
                        <asp:Button ID="btnPop2" runat="server" />
                        <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" CssClass="btn btn-primary"></asp:Button>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>

        <asp:GridView ID="grvMaintenanceRequestList"
            runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
            OnRowDataBound="grvMaintenanceRequestList_RowDataBound" OnRowDeleting="grvMaintenanceRequestList_RowDeleting"
            OnSelectedIndexChanged="grvMaintenanceRequestList_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvMaintenanceRequestList_PageIndexChanging"
            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="30" OnRowCommand="grvMaintenanceRequestList_RowCommand">
            <RowStyle CssClass="rowstyle" />
            <Columns>
                <asp:BoundField DataField="RequestNo" HeaderText="Request No" SortExpression="RequestNo" />
                 <asp:BoundField DataField="Requester" HeaderText="Requester" SortExpression="Requester" />
                <asp:TemplateField HeaderText="Request Date">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestedDate" runat="server" Text='<%# Eval("RequestDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="KmReading" HeaderText="Km Reading" SortExpression="KmReading" />
                <asp:BoundField DataField="ActionTaken" HeaderText="Action Taken" SortExpression="ActionTaken" />
                <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />
                <asp:ButtonField ButtonType="Button" CommandName="ViewItem" Text="Mechanic Review" />
                <asp:ButtonField ButtonType="Button" CommandName="ViewItemPrev" Text="Preview" />
                <asp:CommandField ShowSelectButton="True" SelectText="Process Request" ButtonType="Button" />
                <asp:ButtonField CommandName="Maintained" Text="Maintained"/>
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
    <asp:Panel ID="pnlApproval" runat="server">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                </div>
                <div class="modal-body no-padding">
                    <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
                        <header>
                            <span class="widget-icon"><i class="fa fa-edit"></i></span>
                            <h2>Process Request</h2>
                        </header>
                        <div>
                            <div class="jarviswidget-editbox"></div>
                            <div class="widget-body no-padding">
                                <div class="smart-form">
                                    <fieldset>
                                        <div class="row">
                                            <section class="col col-6">
                                                <asp:Label ID="lblApprovalStatus" runat="server" Text="Approval Status" CssClass="label"></asp:Label>
                                                <label class="select">
                                                    <asp:DropDownList ID="ddlApprovalStatus" runat="server" OnSelectedIndexChanged="ddlApprovalStatus_SelectedIndexChanged" ValidationGroup="Approve" AutoPostBack="True">
                                                    </asp:DropDownList><i></i>
                                                    <asp:RequiredFieldValidator ID="RfvApprovalStatus" CssClass="validator" runat="server" ErrorMessage="Approval Status Required" InitialValue="0" ControlToValidate="ddlApprovalStatus" ValidationGroup="Approve"></asp:RequiredFieldValidator>
                                                </label>
                                            </section>
                                            <section class="col col-6">
                                                <asp:Label ID="lblRejectedReason" runat="server" Text="Rejected Reason" Visible="false" CssClass="label"></asp:Label>
                                                <label class="textarea">
                                                    <asp:TextBox ID="txtRejectedReason" runat="server" TextMode="MultiLine" Visible="false"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvRejectedReason" runat="server" Enabled="false" CssClass="validator" ValidationGroup="save" ErrorMessage="Must Enter Rejection Reason" ControlToValidate="txtRejectedReason"></asp:RequiredFieldValidator>
                                                </label>
                                            </section>
                                        </div>
                                        <%--<div class="row">
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
                                        </div>--%>
                                    </fieldset>
                                    <footer>
                                        <asp:Button ID="btnApprove" runat="server" Text="Save" OnClick="btnApprove_Click" Enabled="false" CssClass="btn btn-primary" ValidationGroup="Approve"></asp:Button>
                                        <asp:Button ID="btnCancelPopup" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" OnClick="btnCancelPopup_Click"></asp:Button>
                                        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" OnClientClick="javascript:Clickheretoprint('divprint')" Enabled="False"></asp:Button>
                                        <%--  <asp:Button ID="btnPurchaseOrder" runat="server" CssClass="btn btn-primary" Enabled="False" OnClick="btnPurchaseOrder_Click" Text="Purchase Order" />--%>
                                    </footer>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- /.modal-content -->

    </asp:Panel>
    <asp:ModalPopupExtender runat="server" BackgroundCssClass="modalBackground"
        Enabled="True" TargetControlID="btnPop" PopupControlID="pnlApproval" CancelControlID="btnCancelPopup"
        ID="pnlApproval_ModalPopupExtender">
    </asp:ModalPopupExtender>
     
        <div class="modal fade" id="approverModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" style="width: 80%;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                    <h4 class="modal-title">Preview Section</h4>
                </div>
                <div class="modal-body">
                    <div class="jarviswidget-editbox"></div>
                    <div class="widget-body no-padding">
                        <div class="smart-form">
                            
                             <div class="tab-content">
                                <div class="tab-pane active" id="hr2">
                                    <ul class="nav nav-tabs">
                                        <li class="active">
                                            <a href="#iss3" data-toggle="tab"> ServiceTypes Requested and Recommended</a>
                                        </li>
                                        <li class="">
                                            <a href="#iss4" data-toggle="tab">SpareParts to be purchased?</a>
                                        </li>
                                    </ul>
                                    <div class="tab-content padding-10">
                                        <div class="tab-pane active" id="iss3">
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
                                            
                                        </div>
                                        <div class="tab-pane" id="iss4">
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
                                        </div>
                                    </div>
                                </div>
                                 <footer>
                                <asp:Button ID="btnCancelPopup2" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" OnClick="btnCancelPopup2_Click"></asp:Button>
                            </footer>
                            </div>






                                       
                                        
                    </div>
                </div>
            </div>
        </div>
    </div></div>
    <div class="modal fade" id="mechanicModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" style="width: 80%;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                    <h4 class="modal-title">Mechanic Review Section</h4>
                </div>
                <div class="modal-body">
                    <div class="jarviswidget-editbox"></div>
                    <div class="widget-body no-padding">
                        <div class="smart-form">
                            <div class="tab-content">
                                <div class="tab-pane active" id="hr2">
                                    <ul class="nav nav-tabs">
                                        <li class="active">
                                            <a href="#iss1" data-toggle="tab">Select Service Types</a>
                                        </li>
                                        <li class="">
                                            <a href="#iss2" data-toggle="tab">Spare Parts Needed?</a>
                                        </li>
                                    </ul>
                                    <div class="tab-content padding-10">
                                        <div class="tab-pane active" id="iss1">
                                            <asp:DataGrid ID="dgMaintenanceRequestDetail" runat="server"
                                                AutoGenerateColumns="False" CellPadding="0" CssClass="table table-striped table-bordered table-hover"
                                                DataKeyField="Id" GridLines="None" PagerStyle-CssClass="paginate_button active" ShowFooter="True" TextWrap="Wrap" OnEditCommand="dgMaintenanceRequestDetail_EditCommand" OnItemDataBound="dgMaintenanceRequestDetail_ItemDataBound" OnUpdateCommand="dgMaintenanceRequestDetail_UpdateCommand" OnItemCommand="dgMaintenanceRequestDetail_ItemCommand">
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="Service Type">
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "ServiceType.Name")%>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddlServiceTpe" runat="server" CssClass="form-control"
                                                                AppendDataBoundItems="True" DataTextField="Name" DataValueField="Id" AutoPostBack="True" EnableViewState="true"
                                                                ValidationGroup="proedit" OnSelectedIndexChanged="ddlServiceTpe_SelectedIndexChanged">
                                                                <asp:ListItem Value="0">Select Service Type</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RfvAccount" runat="server" CssClass="validator"
                                                                ControlToValidate="ddlServiceTpe" ErrorMessage="Service Type Required"
                                                                InitialValue="0" SetFocusOnError="True" ValidationGroup="proedit"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlFServiceTpe" runat="server" CssClass="form-control"
                                                                AppendDataBoundItems="True" DataTextField="Name" DataValueField="Id"
                                                                EnableViewState="true" ValidationGroup="proadd" AutoPostBack="True" OnSelectedIndexChanged="ddlFServiceTpe_SelectedIndexChanged">
                                                                <asp:ListItem Value="0">Select Service Type</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RfvFServiceType" runat="server" CssClass="validator"
                                                                ControlToValidate="ddlFServiceTpe" Display="Dynamic"
                                                                ErrorMessage="Service Type Required" InitialValue="0" SetFocusOnError="True"
                                                                ValidationGroup="proadd"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Service Type Detail (Driver)">
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "DriverServiceTypeDetail.Description")%>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "DriverServiceTypeDetail.Description")%>
                                                        </EditItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Service Type Detail (Mechanic)">
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "MechanicServiceTypeDetail.Description")%>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddlEdtMechanicServiceTypeDetail" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                                                                <asp:ListItem Value="0">Select Mechanic Service Type Detail</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <i></i>
                                                            <asp:RequiredFieldValidator ID="rfvddlEdtMechanicServiceTypeDetail" runat="server" CssClass="validator" ControlToValidate="ddlEdtMechanicServiceTypeDetail" Display="Dynamic" ErrorMessage="Mechanic Service Type must be selected" InitialValue="0" SetFocusOnError="true" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlMecServiceTypeDet" runat="server" CssClass="form-control" Enabled="true" DataValueField="Id" DataTextField="Description" AppendDataBoundItems="True">
                                                                <asp:ListItem Value="0">--Select Service Detail--</asp:ListItem>
                                                            </asp:DropDownList><i></i>
                                                        </FooterTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Mechanic Remark">
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "TechnicianRemark")%>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEdtTechnicianRemark" ReadOnly="false" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "TechnicianRemark")%>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFRemark" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RfvFRemark" CssClass="validator" runat="server" ControlToValidate="txtFRemark" ErrorMessage="Remark Required" ValidationGroup="proadd"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Actions">
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" ValidationGroup="proedit" CssClass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:LinkButton ID="lnkAddNew" runat="server" CommandName="AddNew" ValidationGroup="proadd" CssClass="btn btn-sm btn-success"><i class="fa fa-save"></i></asp:LinkButton>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');"><i class="fa fa-times"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                                <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                            </asp:DataGrid>
                                        </div>
                                        <div class="tab-pane" id="iss2">
                                            <asp:DataGrid ID="dgSparepart" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                                                CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                                                GridLines="None" ShowFooter="True" OnCancelCommand="dgSparepart_CancelCommand" OnDeleteCommand="dgSparepart_DeleteCommand" OnEditCommand="dgSparepart_EditCommand" OnItemCommand="dgSparepart_ItemCommand" OnItemDataBound="dgSparepart_ItemDataBound" OnUpdateCommand="dgSparepart_UpdateCommand">

                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="Spare Parts">
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddlItem" runat="server" CssClass="form-control"
                                                                AppendDataBoundItems="True" DataTextField="Name" DataValueField="Id"
                                                                ValidationGroup="proeditspare">
                                                                <asp:ListItem Value="0">Select Item</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RfvName" runat="server" CssClass="validator"
                                                                ControlToValidate="ddlItem" ErrorMessage="Item Required"
                                                                InitialValue="0" SetFocusOnError="True" ValidationGroup="proeditspare"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlFItem" runat="server" CssClass="form-control"
                                                                AppendDataBoundItems="True" DataTextField="Name" DataValueField="Id"
                                                                EnableViewState="true" ValidationGroup="proaddspare">
                                                                <asp:ListItem Value="0">Select Item</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RfvFItem" runat="server" CssClass="validator"
                                                                ControlToValidate="ddlFItem" Display="Dynamic"
                                                                ErrorMessage="Item Required" InitialValue="0" SetFocusOnError="True"
                                                                ValidationGroup="proaddspare"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "Item.Name")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Actions">
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" ValidationGroup="proeditspare" CssClass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:LinkButton ID="lnkAddNew" runat="server" CommandName="AddNew" ValidationGroup="proaddspare" CssClass="btn btn-sm btn-success"><i class="fa fa-save"></i></asp:LinkButton>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');"><i class="fa fa-times"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                                <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                            </asp:DataGrid>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="divprint" style="display: none; width: 942px;">
        <fieldset>
            <table style="width: 100%;">
                <tr>
                    <td style="width: 17%; text-align: left;">
                        <img src="../img/CHAI%20Logo.png" width="70" height="50" /></td>
                    <td style="font-size: large; text-align: center;">
                        <strong>CHAI ETHIOPIA
                           
                            <br />
                            VEHICLE MAINTENANCE REQUEST FORM</strong></td>
                </tr>
            </table>
            <table style="width: 100%">

                <tr>
                    <td align="right" style="">&nbsp;</td>
                    <td align="right" style="width: 244px" class="inbox-data-from">&nbsp;</td>
                    <td align="right" style="width: 271px">&nbsp;</td>
                    <td align="right" style="width: 389px">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 629px; height: 18px; padding-left: 20%;">
                        <strong>
                            <asp:Label ID="lblRequestNo" runat="server" Text="Request No.:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblRequestNoresult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 629px; height: 18px; padding-left: 20%;">
                        <strong>
                            <asp:Label ID="lblRequestedDate" runat="server" Text="Requested Date:"></asp:Label>
                        </strong></td>
                    <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblRequestedDateresult" runat="server"></asp:Label>
                    </td>
                    <td align="right" style="width: 334px">&nbsp;</td>
                    <td align="right" style="width: 335px">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 629px; height: 18px; padding-left: 20%;">
                        <strong>
                            <asp:Label ID="lblRequester" runat="server" Text="Requester:"></asp:Label>
                        </strong></td>
                    <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblRequesterres" runat="server"></asp:Label>
                    </td>
                    <td style="width: 334px">&nbsp;</td>
                    <td style="width: 335px">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 682px; height: 18px;">&nbsp;</td>
                    <td style="width: 244px; height: 18px;">&nbsp;</td>
                    <td style="width: 334px; height: 18px;">&nbsp;</td>
                    <td style="width: 335px; height: 18px;">&nbsp;</td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
            </table>
            <br />
            <asp:GridView ID="grvSoleDetailsPrint" CellPadding="5" CellSpacing="3"
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
                    <asp:BoundField HeaderText="Approval Status" DataField="ApprovalStatus" />
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
        </fieldset>
         <asp:GridView ID="grvSpare" CellPadding="5" CellSpacing="3"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id" 
                CssClass="table table-striped table-bordered table-hover">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    

                   
                    <asp:BoundField DataField="Name" HeaderText="Item" SortExpression="Name" />
                   
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="menuContent" runat="Server">
</asp:Content>

