<%@ Page Title="Payment Reimbursement Request Form" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmPaymentReimbursementRequest.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Request.Views.frmPaymentReimbursementRequest" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Payment Settlement</h2>
        </header>
        <div class="row">
            <div class="col-md-12">
                <div class="form-group">
                    <div class="well well-sm well-primary">
                        <asp:GridView ID="grvCashPayments"
                            runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                            OnRowDataBound="grvCashPayments_RowDataBound" OnSelectedIndexChanged="grvCashPayments_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvCashPayments_PageIndexChanging"
                            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active">
                            <RowStyle CssClass="rowstyle" />
                            <Columns>
                                <asp:BoundField DataField="RequestNo" HeaderText="RequestNo" />
                                <asp:BoundField DataField="RequestDate" HeaderText="Request Date" />
                                <asp:BoundField DataField="VoucherNo" HeaderText="Voucher No" />
                                <asp:BoundField DataField="Payee" HeaderText="Payee" />
                                <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" />
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
        <asp:Panel ID="pnlInfo" runat="server">
            <div class="alert alert-info fade in">
                <button class="close" data-dismiss="alert">
                    ×
                </button>
                <i class="fa-fw fa fa-info"></i>
                <strong>Info!</strong> Please select the Cash Payment Transaction to perform Payment Settlement for!
            </div>
        </asp:Panel>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
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
                                            <div class="tab-pane" id="BB">
                                            </div>
                                        </div>
                                        <ul class="nav nav-tabs">
                                            <li class="active">
                                                <a data-toggle="tab" href="#AA">Tab 1</a>
                                            </li>
                                            <li class="">
                                                <a data-toggle="tab" href="#BB">Tab 2</a>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="tab-pane active" id="hr2">

                                    <ul class="nav nav-tabs">
                                        <li class="active">
                                            <a href="#iss1" data-toggle="tab">Re-Imbursement Form</a>
                                        </li>
                                        <li class="">
                                            <a href="#iss2" data-toggle="tab">Attach Invoice</a>
                                        </li>
                                    </ul>
                                    <div class="tab-content padding-10">
                                        <div class="tab-pane active" id="iss1">
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
                                                        <label class="label">Comment</label>
                                                        <label class="input">
                                                            <asp:TextBox ID="txtComment" runat="server"></asp:TextBox>
                                                        </label>
                                                    </section>
                                                    </div>
                                                <div class="row">
                                                    <section class="col col-6">
                                                        <label class="label">Project</label>
                                                        <label class="input">
                                                             <asp:TextBox ID="txtProject" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </label>
                                                    </section>
                                                       <section class="col col-6">
                                                        <label class="label">Grant</label>
                                                        <label class="input">
                                                            <asp:TextBox ID="txtGrant" runat="server"></asp:TextBox>
                                                        </label>
                                                    </section>
                                                    </div>
                                                <div class="row">
                                                    <section class="col col-6">
                                                        <label class="label">Account Receivables</label>
                                                        <label class="input">
                                                            <i class="icon-append fa fa-calendar"></i>
                                                            <asp:TextBox ID="txtReceivables" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </label>
                                                    </section>
                                                       <section class="col col-6">
                                                        <label class="label">Total Re-Imbursement </label>
                                                        <label class="input">
                                                            <asp:TextBox ID="txtImbursement" runat="server"></asp:TextBox>
                                                        </label>
                                                    </section>
                                                    </div>
                                                <asp:DataGrid ID="dgPaymentReimbursementDetail" runat="server" AutoGenerateColumns="false"
                                                CellPadding="0" CssClass="table table-striped table-bordered table-hover" DataKeyField="Id"
                                                GridLines="None" OnCancelCommand="dgPaymentReimbursementDetail_CancelCommand" OnDeleteCommand="dgPaymentReimbursementDetail_DeleteCommand"
                                                OnEditCommand="dgPaymentReimbursementDetail_EditCommand" OnItemCommand="dgPaymentReimbursementDetail_ItemCommand"
                                                OnItemDataBound="dgPaymentReimbursementDetail_ItemDataBound" OnUpdateCommand="dgPaymentReimbursementDetail_UpdateCommand"
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
                                                    <asp:TemplateColumn HeaderText="Actual Expendture">
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "ActualExpendture")%>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEdtAmount" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "ActualExpendture")%>'></asp:TextBox>
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
                                                
                                                
                                            </fieldset>
                                        </div>
                                        <div class="tab-pane" id="iss2">
                                            <div class="jarviswidget-editbox"></div>
                                            <div class="widget-body no-padding">
                                                <div class="smart-form">
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
                            </div>
                        </div>
                        <!-- end widget content -->
                    </div>
                    <footer>
                        <asp:Button ID="btnSave" runat="server" CausesValidation="true" ValidationGroup="save" Enabled="false" Text="Request" OnClick="btnSave_Click" CssClass="btn btn-primary"></asp:Button>
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" />
                        <asp:Button ID="btnDelete" runat="server" CausesValidation="False" class="btn btn-primary"
                            Text="Delete" OnClick="btnDelete_Click" Visible="False"></asp:Button>
                        <cc1:ConfirmButtonExtender ID="btnDelete_ConfirmButtonExtender" runat="server"
                            ConfirmText="Are you sure" Enabled="True" TargetControlID="btnDelete">
                        </cc1:ConfirmButtonExtender>
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" Text="New" />
                          <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>
    </div>

    <%-- Modal --%>

    <asp:Panel ID="pnlSearch" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    &times;</button>
                <h4 class="modal-title" id="myModalLabel">Search Payment Reimbursement Requests</h4>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <div class="input-group">
                                <label for="txtSrchRequestDate">Requested Date</label>
                                <i class="icon-append fa fa-calendar"></i>
                                <asp:TextBox ID="txtSrchRequestDate" CssClass="datepicker form-control"
                                    data-dateformat="mm/dd/yy" ToolTip="Request Date" runat="server"></asp:TextBox>
                            </div>

                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="ddlSrchExpenseType">Expense Type</label><br />
                            <asp:DropDownList ID="ddlSrchExpenseType" CssClass="form-control" runat="server">
                                <asp:ListItem Value="Advance">Advance</asp:ListItem>
                                <asp:ListItem Value="Claim">Claim</asp:ListItem>
                            </asp:DropDownList><i></i>
                        </div>
                    </div>
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
                <div class="row" style="text-align: right;">
                    <div class="col-md-12">
                        <div class="form-group">
                            <asp:Button ID="btnFind" runat="server" OnClick="btnFind_Click" Text="Find" CssClass="btn btn-primary"></asp:Button>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <div class="well well-sm well-primary">
                                <asp:GridView ID="grvPaymentReimbursementRequestList"
                                    runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                    OnRowDataBound="grvPaymentReimbursementRequestList_RowDataBound" OnSelectedIndexChanged="grvPaymentReimbursementRequestList_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvPaymentReimbursementRequestList_PageIndexChanging"
                                    CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active">
                                    <RowStyle CssClass="rowstyle" />
                                    <Columns>
                                        <asp:BoundField DataField="RequestDate" HeaderText="Request Date" SortExpression="RequestDate" />
                                        <asp:BoundField DataField="ExpenseType" HeaderText="Expense Type" SortExpression="ExpenseType" />
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
            <div class="modal-footer">
                <asp:Button ID="btnCancelSearch" runat="server" CssClass="btn btn-primary" Text="Cancel" />
            </div>
        </div>
    </asp:Panel>

    <cc1:ModalPopupExtender runat="server" Enabled="True" TargetControlID="btnSearch" CancelControlID="btnCancelSearch"
        PopupControlID="pnlSearch" ID="pnlSearch_ModalPopupExtender" BackgroundCssClass="modalBackground">
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
