﻿<%@ Page Title="Payment Request" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmCashPaymentRequest.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Request.Views.frmCashPaymentRequest" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <style>
        .popover-title {
            padding: 8px 14px;
        }

        .popover-content {
            padding: 9px 14px;
        }

        .editable-buttons {
            margin-left: 7px;
        }
    </style>
    <script src="../js/libs/jquery-2.0.2.min.js"></script>
    <script src="../js/bootstrap/bootstrap.min.js"></script>
    <script src="../js/plugin/x-editable/moment.min.js"></script>
    <script src="../js/plugin/x-editable/jquery.mockjax.min.js"></script>
    <script src="../js/plugin/x-editable/x-editable.min.js"></script>
    <script type="text/javascript">
        function printPaymentDetail(id) {
            var disp_setting = "toolbar=yes,location=no,directories=yes,menubar=yes,";
            disp_setting += "scrollbars=yes,width=750, height=600, left=100, top=25";
            var content_value = document.getElementById(id).innerHTML;
            var docprint = window.open("", "", disp_setting);
            docprint.document.open();
            docprint.document.write('<html><head><title>CHAI Ethiopia ERP</title>');
            docprint.document.write('</head><body onLoad="self.print()"><center>');
            docprint.document.write(content_value);
            docprint.document.write('</center></body></html>');
            docprint.document.close();
            docprint.focus();
        }
        function showPaymentSearch() {
            $('#paymentSearchModal').modal('show');
        }

        function setArrivalTimeVal() {
            if ($('#DefaultContent_lnkArrivalTime').text() != 'Choose Time')
                $('#DefaultContent_txtArrivalTime').val($('#DefaultContent_lnkArrivalTime').text());
        }

        function setReturnTimeVal() {
            if ($('#DefaultContent_lnkReturnTime').text() != 'Choose Time')
                $('#DefaultContent_txtReturnTime').val($('#DefaultContent_lnkReturnTime').text());
        }

        $(document).ready(function () {
            $('#DefaultContent_lnkArrivalTime').editable({
                placement: 'right',
                combodate: {
                    firstItem: 'name',
                    minYear: 2015,
                    maxYear: 2040
                }
            });
            $('#DefaultContent_lnkReturnTime').editable({
                placement: 'right',
                combodate: {
                    firstItem: 'name',
                    minYear: 2015,
                    maxYear: 2040
                }
            });
        });

    </script>
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Payment Request</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Request Date</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtRequestDate" ReadOnly="true" runat="server"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label class="label">Description</label>
                                <label class="input">
                                    <asp:TextBox ID="txtDescription" TextMode="MultiLine" Width="100%" runat="server"></asp:TextBox>
                                </label>
                                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription" CssClass="validator" Display="Dynamic" ErrorMessage="Description is Required" SetFocusOnError="true" ValidationGroup="request"></asp:RequiredFieldValidator>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Request Type</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlRequestType" runat="server">
                                        <asp:ListItem Value="">Select Request Type</asp:ListItem>
                                        <asp:ListItem Value="Medical Expense (Out-Patient)">Medical Expense (Out-Patient)</asp:ListItem>
                                        <asp:ListItem Value="Medical Expense (In-Patient)">Medical Expense (In-Patient)</asp:ListItem>
                                        <asp:ListItem Value="Other">Other Payment</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                </label>
                                <asp:RequiredFieldValidator ID="rfvPaymentType" runat="server" ControlToValidate="ddlRequestType" CssClass="validator" Display="Dynamic" ErrorMessage="Select Request Type" SetFocusOnError="true" ValidationGroup="request"></asp:RequiredFieldValidator>
                            </section>
                            <section class="col col-6">
                                <label class="label">Payee</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlPayee" runat="server" DataTextField="SupplierName" AppendDataBoundItems="true" DataValueField="Id">
                                        <asp:ListItem Value="0">Select Payee</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                </label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Amount Type</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlAmountType" AutoPostBack="true" OnSelectedIndexChanged="ddlAmountType_SelectedIndexChanged" runat="server">
                                        <asp:ListItem Value="">Select Amount Type</asp:ListItem>
                                        <asp:ListItem>Advanced</asp:ListItem>
                                        <asp:ListItem>Actual Amount</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                </label>
                                <asp:RequiredFieldValidator ID="RfvAmountType" runat="server" ControlToValidate="ddlAmountType" CssClass="validator" Display="Dynamic" ErrorMessage="Amount Type Required" SetFocusOnError="true" ValidationGroup="request"></asp:RequiredFieldValidator>
                            </section>
                            <section class="col col-6">
                                <label class="label">Program</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlProgram" AutoPostBack="true" DataTextField="ProgramName" DataValueField="Id" AppendDataBoundItems="true" runat="server" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select Program</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                </label>
                                <asp:RequiredFieldValidator ID="rfvProgram" runat="server" ControlToValidate="ddlProgram" CssClass="validator" Display="Dynamic" ErrorMessage="Select Program" SetFocusOnError="true" ValidationGroup="request"></asp:RequiredFieldValidator>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">
                                    Departure Date/Time 
                                    <span style="color: red;">(For Per Diems Please specify your Departure Date & Time!)</span>
                                </label>
                                <label class="input">
                                    <asp:LinkButton ID="lnkArrivalTime" runat="server" Text="Choose Time"
                                        CssClass="btn btn-success" data-type="combodate" data-format="DD-MM-YYYY h:mm a"
                                        data-template="DD / MM / YYYY hh : mm a" data-viewformat="MMM D YYYY hh:mm a"
                                        data-pk="1" data-original-title="Setup event date and time"
                                        Style="padding: 6px 12px;"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkSetArrivalTime" CssClass="btn btn-primary" runat="server"
                                        OnClientClick="setArrivalTimeVal(); return false;"
                                        Style="padding: 6px 12px;" Text="Set Departure Time"></asp:LinkButton>
                                    <asp:TextBox ID="txtArrivalTime" runat="server"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label class="label">
                                    Return Date/Time 
                                    <span style="color: red;">(For Per Diems Please specify your Return Date & Time!)</span>
                                </label>
                                <label class="input">
                                    <asp:LinkButton ID="lnkReturnTime" runat="server" Text="Choose Time"
                                        CssClass="btn btn-success" data-type="combodate" data-format="DD-MM-YYYY h:mm a"
                                        data-template="DD / MM / YYYY hh : mm a" data-viewformat="MMM D YYYY hh:mm a"
                                        data-pk="1" data-original-title="Setup event date and time"
                                        Style="padding: 6px 12px;"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkSetReturnTime" Style="padding: 6px 12px;"
                                        CssClass="btn btn-primary" runat="server" OnClientClick="setReturnTimeVal(); return false;"
                                        Text="Set Return Time"></asp:LinkButton>
                                    <asp:TextBox ID="txtReturnTime" runat="server"></asp:TextBox>
                                </label>
                            </section>
                        </div>
                    </fieldset>
                    <div role="content">
                        <div class="jarviswidget-editbox">
                        </div>
                        <div class="widget-body">
                            <div class="tab-content">
                                <div class="tab-pane active" id="hr1">
                                    <ul class="nav nav-tabs">
                                        <li class="active">
                                            <a href="#iss1" data-toggle="tab">Add Details</a>
                                        </li>
                                        <li class="">
                                            <a href="#iss2" data-toggle="tab">Attach Receipt</a>
                                        </li>
                                        <li></li>
                                    </ul>
                                    <div class="tab-content padding-10">
                                        <div class="tab-pane active" id="iss1">
                                            <div style="overflow-x: auto;">
                                                <asp:DataGrid ID="dgCashPaymentDetail" runat="server" AutoGenerateColumns="false"
                                                    CellPadding="0" CssClass="table table-striped table-bordered table-hover" DataKeyField="Id"
                                                    GridLines="None" OnCancelCommand="dgCashPaymentDetail_CancelCommand" OnDeleteCommand="dgCashPaymentDetail_DeleteCommand"
                                                    OnEditCommand="dgCashPaymentDetail_EditCommand" OnItemCommand="dgCashPaymentDetail_ItemCommand"
                                                    OnItemDataBound="dgCashPaymentDetail_ItemDataBound" OnUpdateCommand="dgCashPaymentDetail_UpdateCommand"
                                                    PagerStyle-CssClass="paginate_button active" ShowFooter="True" Visible="true">
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="Account Name">
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountName")%>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:DropDownList ID="ddlEdtAccountDescription" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddlEdtAccountDescription_SelectedIndexChanged">
                                                                    <asp:ListItem Value="0">Select Account</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <i></i>
                                                                <asp:RequiredFieldValidator ID="rfvdddlEdtAccountDescription" runat="server" ControlToValidate="ddlEdtAccountDescription" CssClass="validator" Display="Dynamic" ErrorMessage="Account Name must be selected" InitialValue="0" SetFocusOnError="true" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlAccountDescription" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddlAccountDescription_SelectedIndexChanged">
                                                                    <asp:ListItem Value="0">Select Account</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <i></i>
                                                                <asp:RequiredFieldValidator ID="rfvddlddlAccountDescription" runat="server" ControlToValidate="ddlAccountDescription" CssClass="validator" Display="Dynamic" ErrorMessage="Account Name must be selected" InitialValue="0" SetFocusOnError="true" ValidationGroup="save"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Account Code">
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container.DataItem, "AccountCode")%>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtEdtAccountCode" Enabled="false" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "AccountCode")%>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtAccountCode" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>

                                                            </FooterTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Amount">
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container.DataItem, "Amount")%>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtEdtAmount" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "Amount")%>'></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="txtEdtAmount_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtEdtAmount" ValidChars="&quot;.&quot;">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="rfvtxtEdtAmount" runat="server" ControlToValidate="txtEdtAmount" CssClass="validator" Display="Dynamic" ErrorMessage="Amount is required" SetFocusOnError="true" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="txtAmount_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtAmount" ValidChars="&quot;.&quot;">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="rfvtxtAmount" runat="server" ControlToValidate="txtAmount" CssClass="validator" Display="Dynamic" ErrorMessage="Amount is required" SetFocusOnError="true" ValidationGroup="save"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Project ID">
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container.DataItem, "Project.ProjectCode")%>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:DropDownList ID="ddlEdtProject" runat="server" AutoPostBack="true" CssClass="form-control" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlEdtProject_SelectedIndexChanged">
                                                                    <asp:ListItem Value="0">Select Project</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <i></i>
                                                                <asp:RequiredFieldValidator ID="rfvddlEdtProject" runat="server" ControlToValidate="ddlEdtProject" CssClass="validator" Display="Dynamic" ErrorMessage="Project must be selected" InitialValue="0" SetFocusOnError="true" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlProject" runat="server" AutoPostBack="true" CssClass="form-control" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                                                                    <asp:ListItem Value="0">Select Project</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <i></i>
                                                                <asp:RequiredFieldValidator ID="rfvddlProject" runat="server" ControlToValidate="ddlProject" CssClass="validator" Display="Dynamic" ErrorMessage="Project must be selected" InitialValue="0" SetFocusOnError="true" ValidationGroup="save"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Grant ID">
                                                            <EditItemTemplate>
                                                                <asp:DropDownList ID="ddlEdtGrant" runat="server" AppendDataBoundItems="True" CssClass="form-control" DataTextField="GrantCode" DataValueField="Id">
                                                                    <asp:ListItem Value="0">Select Grant</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="RfvGrant" runat="server" CssClass="validator" ControlToValidate="ddlEdtGrant" ErrorMessage="Grant is required" InitialValue="0" SetFocusOnError="True" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlGrant" runat="server" AppendDataBoundItems="True" CssClass="form-control" DataTextField="GrantCode" DataValueField="Id" EnableViewState="true">
                                                                    <asp:ListItem Value="0">Select Grant</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="RfvFGrantCode" runat="server" CssClass="validator" ControlToValidate="ddlGrant" Display="Dynamic" ErrorMessage="Grant is required" InitialValue="0" SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container.DataItem, "Grant.GrantCode")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Supporting Doc Attached">
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container.DataItem, "SupportDocAttached")%>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <label class="checkbox">
                                                                    <asp:CheckBox runat="server" ID="ckSupDocAttached" /><i></i>
                                                                </label>
                                                            </FooterTemplate>
                                                            <EditItemTemplate>
                                                                <label class="checkbox">
                                                                    <asp:CheckBox runat="server" ID="ckEdtSupDocAttached" /><i></i>
                                                                </label>
                                                            </EditItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Actions">
                                                            <EditItemTemplate>
                                                                <asp:LinkButton ID="lnkUpdate" runat="server" CausesValidation="true" CommandName="Update" CssClass="btn btn-xs btn-default" ValidationGroup="edit"><i class="fa fa-save"></i></asp:LinkButton>
                                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:LinkButton ID="lnkAddNew" runat="server" CausesValidation="true" CommandName="AddNew" CssClass="btn btn-sm btn-success" ValidationGroup="save"><i class="fa fa-save"></i></asp:LinkButton>
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
                                        <div class="tab-pane" id="iss2">
                                            <fieldset>
                                                <div class="row">
                                                    <section class="col col-6">
                                                        <label class="label">Attach Reciepts</label>
                                                    </section>
                                                </div>
                                            </fieldset>
                                            <asp:GridView ID="grvAttachments"
                                                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                                CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active">
                                                <RowStyle CssClass="rowstyle" />
                                                <Columns>
                                                    <asp:BoundField DataField="ItemAccountChecklists[0].ItemAccount.AccountName" HeaderText="Account Name" />
                                                    <asp:BoundField DataField="FilePath" HeaderText="File Name" SortExpression="FilePath" />
                                                    <asp:BoundField DataField="ItemAccountChecklists[0].ChecklistName" HeaderText="Checklist Name" SortExpression="ItemAccountChecklists[0].ChecklistName" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <div class="input input-file">
                                                                <asp:FileUpload ID="fuReciept" runat="server" />
                                                                <asp:Button ID="btnUpload" Text="Upload" CssClass="btn btn-primary" CommandArgument='<%# Eval("FilePath") %>' runat="server" OnClick="btnUpload_Click"></asp:Button>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
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
                        <!-- end widget content -->

                    </div>
                    <footer>
                        <asp:Button ID="btnSave" runat="server" Text="Request" OnClick="btnSave_Click" class="btn btn-primary"
                            UseSubmitBehavior="false" OnClientClick="this.disabled = true; this.value = 'Submitting...';"
                            ValidationGroup="request"></asp:Button>

                        <asp:Button ID="btnSearch" runat="server" OnClientClick="showPaymentSearch(); return false;" Text="Search" class="btn btn-default" />
                        <%--<a runat="server" id="searchLink" onclientclick="javascript:showSearch(); return false;" class="btn btn-default"><i class="fa fa-circle-arrow-up fa-lg"></i>Search</a>--%>
                        <asp:Button ID="btnDelete" runat="server" CausesValidation="False" class="btn btn-default"
                            Text="Delete" OnClick="btnDelete_Click" Visible="false"></asp:Button>
                        <asp:Button ID="btnPrint" runat="server" class="btn btn-default"
                            Text="Print" OnClientClick="javascript:printPaymentDetail('divprint'); return false;" Visible="false"></asp:Button>
                        <cc1:ConfirmButtonExtender ID="btnDelete_ConfirmButtonExtender" runat="server"
                            ConfirmText="Are you sure you want to delete this record?" Enabled="True" TargetControlID="btnDelete">
                        </cc1:ConfirmButtonExtender>
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-default" OnClick="btnCancel_Click" Text="New" />
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-default" PostBackUrl="../Default.aspx"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>

    </div>
    <div class="modal fade" id="paymentSearchModal" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel">Search Payment Requests</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtSrchRequestNo">Voucher No.</label>
                                <asp:TextBox ID="txtSrchRequestNo" CssClass="form-control" ToolTip="Voucher No" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="ddlSrchSupplier">Supplier</label>
                                <asp:DropDownList ID="ddlSrchSupplier" CssClass="form-control" runat="server" DataTextField="SupplierName" AppendDataBoundItems="true" DataValueField="Id">
                                </asp:DropDownList><i></i>
                            </div>
                        </div>
                        <div class="col-md-4">
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
                                <asp:Button Text="Close" ID="btnCancelSearch" runat="server" class="btn btn-primary"></asp:Button>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <div class="well well-sm well-primary">
                                    <asp:GridView ID="grvCashPaymentRequestList"
                                        runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                        OnSelectedIndexChanged="grvCashPaymentRequestList_SelectedIndexChanged"
                                        AllowPaging="True" OnPageIndexChanging="grvCashPaymentRequestList_PageIndexChanging"
                                        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="5" OnRowDataBound="grvCashPaymentRequestList_RowDataBound">
                                        <RowStyle CssClass="rowstyle" />
                                        <Columns>
                                            <asp:BoundField DataField="RequestNo" HeaderText="Vourcher No" SortExpression="RequestNo" />
                                            <asp:TemplateField HeaderText="Request Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDate" runat="server" Text='<%# Eval("RequestDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Supplier.SupplierName" HeaderText="Supplier" SortExpression="SupplierName" />
                                            <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" SortExpression="TotalAmount" />
                                            <asp:BoundField DataField="CurrentStatus" HeaderText="Status" SortExpression="CurrentStatus" />
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
                                            The current Request has no Approval Settings defined. Please contact your administrator!
                                        </p>
                                    </div>
                                    <footer>
                                        <asp:Button ID="btnCancelPopup" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnCancelPopup_Click"></asp:Button>
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
    <div id="divprint" style="display: none;">
        <fieldset>
            <table style="width: 100%;">
                <tr>
                    <td style="font-size: large; text-align: center;">
                        <img src="../img/CHAI%20Logo.png" width="130" height="80" />
                        <br />
                        <strong>CHAI ETHIOPIA
                            <br />
                            PAYMENT REQUEST FORM</strong></td>
                </tr>
            </table>
            <table style="width: 100%; border-spacing: 30px;">
                <tr>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblVoucherNo" runat="server" Text="Voucher No:"></asp:Label>
                        </strong></td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblVoucherNoResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblPostingRef" runat="server" Text="Posting Ref:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 25%;">__________
                    </td>
                    <td>&nbsp;</td>
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
                            <asp:Label ID="lblRequestedDate" runat="server" Text="Requested Date:"></asp:Label>
                        </strong></td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblRequestedDateResult" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblPayee" runat="server" Text="Payee:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblPayeeResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblSupplier" runat="server" Text="Supplier:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblSupplierRes" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblDescriptionP" runat="server" Text="Description :"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblDescResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 25%; text-align: right;">
                        <strong>
                            <asp:Label ID="lblTotalAmount" runat="server" Text="Total Amount Paid:"></asp:Label>
                        </strong></td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblTotalAmountResult" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%; text-align: right;"><strong>
                        <asp:Label ID="lblApprovalStatusPrint" runat="server" Text="Approval Status:"></asp:Label>
                    </strong>
                    </td>
                    <td style="width: 25%;">
                        <asp:Label ID="lblApprovalStatusResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 25%;"><strong></strong>
                    </td>
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
                    <asp:BoundField DataField="Amount" HeaderText="Amount Requested" SortExpression="Amount" />
                    <asp:BoundField DataField="Project.ProjectCode" HeaderText="Project Code" />
                    <asp:BoundField DataField="Grant.GrantCode" HeaderText="Grant Code" />
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
            <table style="width: 100%;">
                <tr>
                    <td></td>
                    <td>Signature</td>
                    <td></td>
                    <td></td>
                    <td style="text-align: right; padding-right: 6%;">Recieved By </td>
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
