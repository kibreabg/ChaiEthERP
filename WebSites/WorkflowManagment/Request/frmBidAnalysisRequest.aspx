﻿<%@ Page Title=" Bid Analysis Request Form" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmBidAnalysisRequest.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Request.Views.frmBidAnalysisRequest" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script src="../js/libs/jquery-2.0.2.min.js"></script>
    <script type="text/javascript">

        function showSearch() {
            $(document).ready(function () {
                $('#searchModal').modal('show');
            });
        }

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
    </script>
    <asp:ValidationSummary ID="VSBidAnalysis" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="save" ForeColor="" />
    <%--  <asp:ValidationSummary ID="VSBid" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="Save" ForeColor="" />
    <asp:ValidationSummary ID="VSBidder" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="proadd" ForeColor="" />
    <asp:ValidationSummary ID="VSBidderEdit" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="proedit" ForeColor="" />--%>

    <div id="wid-id-0" class="jarviswidget" data-widget-custombutton="false" data-widget-editbutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>BID ANALYSIS WORKSHEET</h2>
        </header>
        <%--<div>
            <!-- widget edit box -->
            <div class="jarviswidget-editbox">
                <!-- This area used as dropdown edit box -->
            </div>
            <!-- end widget edit box -->
            <!-- widget content -->
            <div class="widget-body no-padding">--%>


        <div class="smart-form">
            <fieldset>
                <section class="col col-6">
                    <label id="lblPurchaseReq" runat="server" class="label" visible="true">
                        <b>Select Purchase Request to Prepare Bid</b>
                    </label>
                    <label class="select">
                        <asp:DropDownList ID="ddlPurchaseReq" AutoPostBack="true" runat="server" DataValueField="Id" DataTextField="RequestNo" OnSelectedIndexChanged="ddlPurchaseReq_SelectedIndexChanged" AppendDataBoundItems="True">
                        </asp:DropDownList><i></i>
                    </label>
                </section>
            </fieldset>
            <asp:Panel ID="pnlInfo" runat="server">
                <div class="alert alert-info fade in">
                    <button class="close" data-dismiss="alert">
                        ×
                    </button>
                    <i class="fa-fw fa fa-info"></i>
                    <strong>Info!</strong> Please select the Purchase Request Transaction to perform Bid Analysis
                </div>
            </asp:Panel>

            <asp:GridView ID="grvDetails" Visible="False"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover" OnSelectedIndexChanged="grvDetails_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvDetails_PageIndexChanging1" PageSize="15">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    <asp:BoundField DataField="PurchaseRequest.RequestNo" HeaderText="Purchase Request No" SortExpression="PurchaseRequest.RequestNo" />
                    <asp:BoundField DataField="ItemAccount.AccountName" HeaderText="AccountName" SortExpression="ItemAccount.AccountName" />
                    <asp:BoundField DataField="ItemAccount.AccountCode" HeaderText="Account Code" SortExpression="ItemAccount.AccountCode" />
                    <asp:BoundField DataField="ItemDescription" HeaderText="Item Description" SortExpression="ItemDescription"></asp:BoundField>
                    <asp:BoundField DataField="Qty" HeaderText="Quantity" SortExpression="Qty" />
                    <%--<asp:BoundField DataField="Project.ProjectCode" HeaderText="Project Code" />
                    <asp:BoundField DataField="Grant.GrantCode" HeaderText="Grant Code" />     --%>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
            <footer>
                <asp:Button ID="btnCreateBid" runat="server" CssClass="btn btn-primary" Text="Create Bid" OnClick="btnCreateBid_Click" />
            </footer>
        </div>

        <!-- widget div-->


        <!-- widget edit box -->
        <div class="jarviswidget-editbox">
            <!-- This area used as dropdown edit box -->

        </div>
        <!-- end widget edit box -->

        <!-- widget content -->
        <div id="wid-id-1" class="jarviswidget" data-widget-custombutton="false" data-widget-editbutton="false">
            <div class="smart-form">
                <div class="widget-body no-padding">
                    <div class="tab-content">
                        <div class="tab-pane active" id="hr2">

                            <ul class="nav nav-tabs">
                                <li class="active">
                                    <a href="#iss1" data-toggle="tab">Items Requested</a>
                                </li>
                                <li class="">
                                    <a href="#iss2" data-toggle="tab">Attachments</a>
                                </li>
                            </ul>
                            <div class="tab-content padding-10">
                                <div class="tab-pane active" id="iss1">
                                    <fieldset>
                                        <div class="row">

                                            <asp:DataGrid ID="dgItemDetail" runat="server" CellPadding="0"
                                                CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id" AutoGenerateColumns="False"
                                                GridLines="None" ShowFooter="True" OnSelectedIndexChanged="dgItemDetail_SelectedIndexChanged">

                                                <Columns>



                                                    <asp:BoundColumn DataField="ItemDescription" HeaderText="Item Description" SortExpression="ItemDescription"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Qty" HeaderText="Quantity" SortExpression="Qty" />


                                                    <asp:ButtonColumn CommandName="Select" Text="Bidders"></asp:ButtonColumn>

                                                </Columns>
                                                <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                            </asp:DataGrid>

                                        </div>


                                    </fieldset>
                                </div>

                                <div class="tab-pane" id="iss2">
                                    <fieldset>
                                        <div class="row">
                                            <section class="col col-6">
                                                <label class="label">Attach Quotation doc.</label>
                                                <asp:FileUpload ID="fuReciept" runat="server" />
                                                <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="btn btn-primary" OnClick="btnUpload_Click" />
                                            </section>
                                        </div>
                                    </fieldset>
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
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" Text="Delete" CommandArgument='<%# Eval("FilePath") %>' runat="server" OnClick="DeleteFile" />
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
                    <br />
                </div>
                <footer>
                    <asp:Button ID="btnSave" runat="server" Text="Request" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="save" CssClass="btn btn-primary"
                        UseSubmitBehavior="false" OnClientClick="this.disabled = true; this.value = 'Submitting...';"></asp:Button>
                    <%-- <asp:Button ID="btnRequest" runat="server" CssClass="btn btn-primary" OnClick="btnRequest_Click" Text="Request" ValidationGroup="Save" />--%>
                        &nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" Text="Back" />
                    <%--<asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" />--%>
                    <a data-toggle="modal" runat="server" id="searchLink" href="#searchModal" class="btn btn-primary"><i class="fa fa-circle-arrow-up fa-lg"></i>Search</a>
                    <asp:Button ID="btnPrintworksheet" runat="server" CssClass="btn btn-primary" Text="Print WorkSheet" OnClientClick="javascript:Clickheretoprint('divprint')" Enabled="False" />
                    <asp:Button ID="btnHiddenPopupp" runat="server" />
                    <asp:HiddenField ID="hfDetailId" runat="server" />
                </footer>

            </div>
        </div>
        <!-- end widget content -->
    </div>


    <!-- end widget div -->
    <div id="divprint" style="display: none;" visible="true">
        <fieldset>
            <table style="width: 100%;">
                <tr>
                    <td style="width: 17%; text-align: left;">
                        <img src="../img/CHAI%20Logo.png" width="70" height="50" /></td>
                    <td style="font-size: large; text-align: center;">
                        <strong>CHAI ETHIOPIA
                            <br />
                            BID ANALYSIS WORKSHEET</strong></td>
                </tr>
            </table>

            <table>
                <tr>
                    <td style="width: 629px; height: 18px; padding-left: 20%;">
                        <strong>
                            <asp:Label ID="Label1" runat="server" Text="Total Price:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblTot" runat="server" class="label" Text=""></asp:Label>
                    </td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 629px; height: 18px; padding-left: 20%;">
                        <strong>
                            <asp:Label ID="lblRequesterlable" runat="server" Text="Requester"></asp:Label>
                        </strong></td>
                    <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblRequester" runat="server" class="label"></asp:Label>
                    </td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 629px; height: 18px; padding-left: 20%;">
                        <strong>
                            <asp:Label ID="lblProposedSupplier" runat="server" Text="Reason For Selection:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblReasonForSelection" runat="server" class="label" Text=""></asp:Label>
                    </td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td>&nbsp;</td>
                    <tr>
                        <td style="width: 629px; height: 18px; padding-left: 20%;">
                            <strong>
                                <asp:Label ID="lblRequestDa" runat="server" Text="Request Date:"></asp:Label>
                            </strong></td>
                        <td style="width: 244px; height: 18px;">
                            <asp:Label ID="lblRequestDate0" runat="server" class="label" Text=""></asp:Label>
                        </td>
                        <td style="width: 389px;">&nbsp;</td>
                        <td style="width: 389px;">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>

            </table>





            <asp:GridView ID="grvprtBidders"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover">
                <RowStyle CssClass="rowstyle" />
                <Columns>

                    <asp:BoundField DataField="Supplier.SupplierNameType" HeaderText="Supplier" SortExpression="Supplier.SupplierNameType" />
                    <asp:BoundField DataField="Rank" HeaderText="Rank" SortExpression="Rank" />
                    <asp:BoundField DataField="ReasonForSelection" HeaderText="Reason For Selection" SortExpression="ReasonForSelection" />
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
            <br />
            <asp:GridView ID="grvprtBidderItemDetails"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover">
                <RowStyle CssClass="rowstyle" />
                <Columns>

                    <asp:BoundField DataField="Item" HeaderText="Item Description" SortExpression="Item" />
                    <asp:BoundField DataField="Qty" HeaderText="Quantity" SortExpression="Qty" />
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
            <br />
            <asp:GridView ID="grvStatuses" OnRowDataBound="grvStatuses_RowDataBound"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    <asp:BoundField DataField="ApprovalDate" HeaderText="Date" SortExpression="ApprovalDate" />
                    <asp:BoundField HeaderText="Approver" />
                    <asp:BoundField HeaderText="Approval Status" DataField="ApprovalStatus" />
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
            <br />
        </fieldset>
    </div>

    <div class="modal fade" id="searchModal" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel">Search Bid Analysis Requests</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtSrchRequestNo">Request Number</label>
                                <asp:TextBox ID="txtSrchRequestNo" CssClass="form-control" ToolTip="Request No" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtSrchRequestDate">Requested Date</label>
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
                                <asp:Button ID="btnCancelSearch" Text="Close" runat="server" class="btn btn-primary"></asp:Button>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <div class="well well-sm well-primary">
                                    <asp:GridView ID="grvBidAnalysisRequestList"
                                        runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                        AllowPaging="True"
                                        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="5" OnSelectedIndexChanged="grvBidAnlysisRequestList_SelectedIndexChanged" OnPageIndexChanging="grvBidAnalysisRequestList_PageIndexChanging">
                                        <RowStyle CssClass="rowstyle" />
                                        <Columns>
                                            <asp:BoundField DataField="RequestNo" HeaderText="Vourcher No" SortExpression="RequestNo" />
                                            <asp:BoundField DataField="RequestDate" HeaderText="Request Date" SortExpression="RequestDate" />

                                            <asp:BoundField DataField="TotalPrice" HeaderText="Total Price" SortExpression="TotalPrice" />

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

    <asp:Panel ID="pnlBidItem" runat="server" EnableViewState="False">
        <%--<ContentTemplate>--%>
        <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
            <header>
                <span class="widget-icon"><i class="fa fa-edit"></i></span>
                <h2>Bidders</h2>
            </header>
            <div>
                <div class="jarviswidget-editbox"></div>
                <div class="widget-body no-padding">
                    <div class="smart-form">

                        <asp:DataGrid ID="dgBidders" runat="server" AutoGenerateColumns="False" CellPadding="0"
                            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                            ShowFooter="True" GridLines="None" Height="30px" OnUpdateCommand="dgBidders_UpdateCommand1" OnCancelCommand="dgBidders_CancelCommand1" OnDeleteCommand="dgBidders_DeleteCommand1" OnItemCommand="dgBidders_ItemCommand" OnItemDataBound="dgBidders_ItemDataBound1" OnEditCommand="dgBidders_EditCommand">

                            <Columns>

                                <asp:TemplateColumn HeaderText="Supplier">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEdtSupplier" runat="server" CssClass="form-control"
                                            AppendDataBoundItems="True" DataTextField="SupplierNameType" DataValueField="Id"
                                            ValidationGroup="proedit">
                                            <asp:ListItem Value="0">Select Supplier</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RfvSupplier" runat="server" CssClass="validator"
                                            ControlToValidate="ddlEdtSupplier" ErrorMessage="Supplier Required"
                                            InitialValue="0" SetFocusOnError="True" ValidationGroup="proedit"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlFuSupplier" runat="server" CssClass="form-control"
                                            AppendDataBoundItems="True" DataTextField="SupplierNameType" DataValueField="Id"
                                            EnableViewState="true" ValidationGroup="proadd">
                                            <asp:ListItem Value="0">Select Supplier</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RfvFSupplier" runat="server" CssClass="validator"
                                            ControlToValidate="ddlFuSupplier" Display="Dynamic"
                                            ErrorMessage="Supplier Required" InitialValue="0" SetFocusOnError="True"
                                            ValidationGroup="proadd"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "Supplier.SupplierNameType")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>

                                <asp:TemplateColumn HeaderText="Contact Details">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "ContactDetails")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtContactDetails" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "ContactDetails")%>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RfvContactDetails" runat="server" CssClass="validator" ControlToValidate="txtContactDetails" ErrorMessage="Contact Details of Supplier Required" ValidationGroup="proedit">*</asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtFContactDetails" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RfvFContactDetails" runat="server" CssClass="validator" ControlToValidate="txtFContactDetails" ErrorMessage="Contact Details of Supplier Required" ValidationGroup="proadd">*</asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateColumn>

                                <%-- <asp:TemplateColumn HeaderText="Qty">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "Qty")%>
                                        <asp:HiddenField ID="hfqty" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Qty")%>'></asp:HiddenField>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtQty" Enabled="false" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "Qty")%>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtQty" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                 <asp:TemplateColumn HeaderText="Item">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "Item")%>
                                        <asp:HiddenField ID="hfItem" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Item")%>'></asp:HiddenField>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtItem" Enabled="false" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "Item")%>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtItem" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateColumn>--%>
                                <asp:TemplateColumn HeaderText="Rank">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "Rank")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRank" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "Rank")%>' OnTextChanged="txtRank_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RfvRank" runat="server" CssClass="validator" ControlToValidate="txtRank" ErrorMessage="Rank Required" ValidationGroup="proedit">*</asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtRank" ID="txtRank_FilteredTextBoxExtender" FilterType="numbers"></cc1:FilteredTextBoxExtender>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtFRank" runat="server" CssClass="form-control" OnTextChanged="txtFRank_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RfvFRank" runat="server" CssClass="validator" ControlToValidate="txtFRank" ErrorMessage="Rank Required" ValidationGroup="proadd">*</asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtFRank" ID="txtFRank_FilteredTextBoxExtender" FilterType="numbers"></cc1:FilteredTextBoxExtender>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Unit Cost">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "UnitCost")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtUnitCost" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "UnitCost")%>'></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtEdtUnitCost" ID="txtUnitCost_FilteredTextBoxExtender" FilterType="Custom, Numbers" ValidChars='"."'></cc1:FilteredTextBoxExtender>

                                        <asp:RequiredFieldValidator ID="rfvEdtUnitCost" runat="server" ControlToValidate="txtEdtUnitCost" CssClass="validator" Display="Dynamic" ErrorMessage="Unit Cost is required" SetFocusOnError="true" ValidationGroup="detailedit"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtUnitCost" runat="server" CssClass="form-control"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtUnitCost" ID="txtUnitCost_FilteredTextBoxExtender" FilterType="Custom, Numbers" ValidChars='"."'></cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="rfvUnitCost" runat="server" ControlToValidate="txtUnitCost" CssClass="validator" Display="Dynamic" ErrorMessage="Unit Cost is required" SetFocusOnError="true" ValidationGroup="detailadd"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Total Cost">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "TotalCost")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtTotalCost" runat="server" Enabled="false" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "TotalCost")%>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtTotalCost" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RfvTotalCost" runat="server" ControlToValidate="txtTotalCost" ErrorMessage="Total Cost Required" Enabled="false" ValidationGroup="detailadd">*</asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateColumn>

                                <asp:TemplateColumn HeaderText="Reason For Selection">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "ReasonForSelection")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" Visible="false" Text=' <%# DataBinder.Eval(Container.DataItem, "ReasonForSelection")%>'></asp:TextBox>

                                        <asp:RequiredFieldValidator ID="RfvReas" runat="server" CssClass="validator" ControlToValidate="txtReason" ErrorMessage="Reason For Selection Required" ValidationGroup="proedit">*</asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtFReason" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>

                                        <asp:RequiredFieldValidator ID="RfvFReas" runat="server" CssClass="validator" ControlToValidate="txtFReason" ErrorMessage="Reason For Selection Required" ValidationGroup="proadd">*</asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateColumn>

                                <asp:TemplateColumn HeaderText="Actions">
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" ValidationGroup="edit" CssClass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton ID="lnkAddNew" runat="server" CommandName="AddNew" ValidationGroup="save" CssClass="btn btn-sm btn-success"><i class="fa fa-save"></i></asp:LinkButton>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');"><i class="fa fa-times"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>

                            </Columns>
                            <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                        </asp:DataGrid>




                        <footer>
                            <asp:Button ID="btncancelCost" runat="server" CssClass="btn btn-primary" Text="Close" OnClick="btncancelCost_Click" />
                        </footer>


                    </div>
                </div>
            </div>

        </div>

    </asp:Panel>
    <cc1:ModalPopupExtender runat="server" Enabled="True" CancelControlID="btncancelCost"
        ID="pnlBidItem_ModalPopupExtender" TargetControlID="btnHiddenPopupp" BackgroundCssClass="modalBackground"
        PopupControlID="pnlBidItem">
    </cc1:ModalPopupExtender>


    <asp:Panel ID="pnlWarning" Visible="false" Style="position: absolute; top: 55px; left: 108px;" runat="server">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                </div>
                <div class="modal-body no-padding">
                    <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
                        <div>
                            <div class="jarviswidget-editbox"></div>
                            <div class="widget-body no-padding">
                                <div class="smart-form">
                                    <div class="alert alert-block alert-danger">
                                        <h4 class="alert-heading">Warning</h4>
                                        <p>
                                            The current Request has no Approval Settings defined. Please contact your administrator
                                        </p>
                                    </div>
                                    <footer>
                                        <asp:Button ID="btnCancelPopup" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnCancelPopup_Click"></asp:Button>
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


</asp:Content>
