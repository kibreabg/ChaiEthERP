﻿<%@ Page Title="Bank Payment Request" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmOperationalControlRequest.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Request.Views.frmOperationalControlRequest" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script src="../js/libs/jquery-2.0.2.min.js"></script>
    <script type="text/javascript">
        function showSearch() {
            $(document).ready(function () {
                $('#searchModal').modal('show');
            });
        }
    </script>
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Bank Payment Request</h2>
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
                                    <asp:RequiredFieldValidator
                                        ID="rfvDescription" runat="server" ErrorMessage="Description must be provided" Display="Dynamic"
                                        CssClass="validator" ValidationGroup="saveMain" InitialValue=""
                                        SetFocusOnError="true" ControlToValidate="txtDescription"></asp:RequiredFieldValidator>
                                </label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Bank Account</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlBankAccount" AutoPostBack="true" AppendDataBoundItems="true"
                                        runat="server" DataValueField="Id" DataTextField="Name" CssClass="form-control"
                                        OnSelectedIndexChanged="ddlBankAccount_SelectedIndexChanged">
                                        <asp:ListItem Text="--Select Bank Account--" Value="0"></asp:ListItem>
                                    </asp:DropDownList><i></i>
                                    <asp:RequiredFieldValidator
                                        ID="rfvddlBankAccount" runat="server" ErrorMessage="Bank must be selected" Display="Dynamic"
                                        CssClass="validator" ValidationGroup="saveMain" InitialValue="0"
                                        SetFocusOnError="true" ControlToValidate="ddlBankAccount"></asp:RequiredFieldValidator>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label class="label">Bank Account No</label>
                                <label class="input">
                                    <asp:TextBox ID="txtBankAccountNo" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                                </label>
                            </section>
                        </div>
                    </fieldset>
                    <fieldset>
                        <legend>Beneficiary Details</legend>
                        <div>
                            <section>
                                <div class="inline-group">
                                    <label class="radio">
                                        <asp:RadioButton ID="rbAccount" Checked="true" GroupName="PaymentType" runat="server" />
                                        <i></i>Transfer to Account</label>
                                    <label class="radio">
                                        <asp:RadioButton ID="rbCheque" GroupName="PaymentType" runat="server" />
                                        <i></i>Cheque Payment</label>
                                    <label class="radio">
                                        <asp:RadioButton ID="rbTelegraphic" GroupName="PaymentType" runat="server" />
                                        <i></i>Telegraphic Transfer</label>
                                </div>
                            </section>
                        </div>
                        <asp:Panel ID="pnlBeneficiaries" runat="server">
                            <div class="row">
                                <section class="col col-6">
                                    <label class="label">Beneficiary Name</label>
                                    <label class="select">
                                        <asp:DropDownList ID="ddlBeneficiary" runat="server" AppendDataBoundItems="True" AutoPostBack="True" DataTextField="SupplierName" DataValueField="Id" OnSelectedIndexChanged="ddlBeneficiary_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Select Beneficiary</asp:ListItem>
                                        </asp:DropDownList><i></i>
                                    </label>
                                    <asp:RequiredFieldValidator ID="rfvddlBeneficiary" runat="server" ControlToValidate="ddlBeneficiary" CssClass="validator" Display="Dynamic" ErrorMessage="Beneficiary is required" SetFocusOnError="true" InitialValue="0" ValidationGroup="saveMain"></asp:RequiredFieldValidator>
                                </section>
                                <section class="col col-6">
                                    <label class="label">Bank Name</label>
                                    <label class="input">
                                        <asp:TextBox ID="txtBankName" Enabled="false" runat="server"></asp:TextBox>
                                    </label>
                                </section>
                            </div>
                            <div class="row">
                                <section class="col col-6">
                                    <label class="label">Account No.</label>
                                    <label class="input">
                                        <asp:TextBox ID="txtBenAccountNo" Enabled="false" runat="server"></asp:TextBox>
                                    </label>
                                </section>
                                <section class="col col-6">
                                    <label class="label">Original Requester</label>
                                    <label class="input">
                                        <asp:TextBox ID="txtOriginalRequester" Enabled="false" runat="server"></asp:TextBox>
                                    </label>
                                </section>
                            </div>
                            <div class="row">
                                <section class="col col-6">
                                    <label class="label">Telephone Number</label>
                                    <label class="input">
                                        <asp:TextBox ID="txtTelephoneNo" runat="server"></asp:TextBox>
                                    </label>
                                </section>
                            </div>
                        </asp:Panel>
                    </fieldset>
                    <div role="content">

                        <!-- widget edit box -->
                        <div class="jarviswidget-editbox">
                            <!-- This area used as dropdown edit box -->

                        </div>
                        <!-- end widget edit box -->

                        <!-- widget content -->
                        <div class="widget-body">
                            <div class="tab-content">
                                <div class="tab-pane" id="hr1">
                                    <div class="tabbable tabs-below">
                                        <div class="tab-content padding-10">
                                            <div class="tab-pane" id="AA">
                                            </div>
                                        </div>
                                        <ul class="nav nav-tabs">
                                            <li class="active">
                                                <a data-toggle="tab" href="#AA">Tab 1</a>
                                            </li>
                                        </ul>
                                    </div>

                                </div>
                                <div class="tab-pane active" id="hr2">

                                    <ul class="nav nav-tabs">
                                        <li class="active">
                                            <a href="#iss1" data-toggle="tab">Add Details</a>
                                        </li>
                                        <li>
                                            <a href="#iss2" data-toggle="tab">Attach Receipt</a>
                                        </li>

                                    </ul>
                                    <div class="tab-content padding-10">
                                        <div class="tab-pane active" id="iss1">
                                            <div style="overflow-x: auto;">
                                                <asp:DataGrid ID="dgOperationalControlDetail" runat="server" AutoGenerateColumns="false"
                                                    CellPadding="0" CssClass="table table-striped table-bordered table-hover" DataKeyField="Id"
                                                    GridLines="None" OnCancelCommand="dgOperationalControlDetail_CancelCommand" OnDeleteCommand="dgOperationalControlDetail_DeleteCommand"
                                                    OnEditCommand="dgOperationalControlDetail_EditCommand" OnItemCommand="dgOperationalControlDetail_ItemCommand"
                                                    OnItemDataBound="dgOperationalControlDetail_ItemDataBound" OnUpdateCommand="dgOperationalControlDetail_UpdateCommand"
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
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlAccountDescription" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddlAccountDescription_SelectedIndexChanged">
                                                                    <asp:ListItem Value="0">Select Account</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <i></i>
                                                            </FooterTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Account Code">
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountCode")%>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtEdtAccountCode" Enabled="false" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountCode")%>'></asp:TextBox>
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
                                                                <cc1:FilteredTextBoxExtender ID="txtEdtAmount_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtEdtAmount" ValidChars="&quot;.&quot;-">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="rfvtxtEdtAmount" runat="server" ControlToValidate="txtEdtAmount" CssClass="validator" Display="Dynamic" ErrorMessage="Amount is required" SetFocusOnError="true" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="txtAmount_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtAmount" ValidChars="&quot;.&quot;-">
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
                                                                <asp:RequiredFieldValidator ID="RfvGrant" runat="server" ControlToValidate="ddlEdtGrant" CssClass="validator" ErrorMessage="Grant is required" InitialValue="0" SetFocusOnError="True" ValidationGroup="edit"></asp:RequiredFieldValidator>
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

                        </div>
                        <!-- end widget content -->

                    </div>
                    <footer>
                        <asp:Button ID="btnSave" runat="server" Text="Request" OnClick="btnSave_Click" CssClass="btn btn-primary" CausesValidation="true" ValidationGroup="saveMain"
                            UseSubmitBehavior="false" OnClientClick="this.disabled = true; this.value = 'Submitting...';"></asp:Button>
                        <%--<asp:Button ID="btnSearch" runat="server" Text="Search" class="btn btn-primary" />--%>
                        <a data-toggle="modal" runat="server" id="searchLink" href="#searchModal" class="btn btn-default"><i class="fa fa-circle-arrow-up fa-lg"></i>Search</a>
                        <asp:Button ID="btnDelete" runat="server" CausesValidation="False" class="btn btn-default"
                            Text="Delete" OnClick="btnDelete_Click" Visible="False"></asp:Button>
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
    <div class="modal fade" id="searchModal" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel">Search Operational Control Requests</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtSrchRequestNo">Voucher No.</label>
                                <asp:TextBox ID="txtSrchRequestNo" CssClass="form-control" ToolTip="Voucher No" runat="server"></asp:TextBox>
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
                                    <asp:GridView ID="grvOperationalControlRequestList"
                                        runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                        OnSelectedIndexChanged="grvOperationalControlRequestList_SelectedIndexChanged"
                                        AllowPaging="True" OnPageIndexChanging="grvOperationalControlRequestList_PageIndexChanging"
                                        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="5">
                                        <RowStyle CssClass="rowstyle" />
                                        <Columns>
                                            <asp:BoundField DataField="RequestNo" HeaderText="Vourcher No" SortExpression="RequestNo" />
                                            <asp:BoundField DataField="RequestDate" HeaderText="Request Date" SortExpression="RequestDate" />
                                            <asp:BoundField DataField="Supplier.SupplierName" HeaderText="Account Transfer Made To" SortExpression="Supplier.SupplierName" />
                                            <asp:BoundField DataField="Payee" HeaderText="Cheque/Letter Made Out To" SortExpression="Payee" />
                                            <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" SortExpression="TotalAmount" />
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
</asp:Content>
