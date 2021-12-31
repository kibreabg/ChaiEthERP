<%@ Page Title="Expense Liquidation Request Form" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmExpenseLiquidationRequest.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Request.Views.frmExpenseLiquidationRequest" %>

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
        function printLiquidationForm(theid) {
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

        function showLiquidationSearch() {
            $('#searchModal').modal('show');
        }

        function IsOneDecimalPoint(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode; // restrict user to type only one . point in number
            var parts = evt.srcElement.value.split('.');
            if (parts.length > 1 && charCode == 46)
                return false;
            return true;
        }

        function setArrivalReturnTimeVal() {
            if ($('#DefaultContent_lnkArrivalReturnTime').text() != 'Choose Time')
                $('#DefaultContent_txtArrivalReturnTime').val($('#DefaultContent_lnkArrivalReturnTime').text());
        }

        $(document).ready(function () {
            $('#DefaultContent_lnkArrivalReturnTime').editable({
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
            <h2>Expense Liquidation</h2>
        </header>
        <div class="row">
            <div class="col-md-12">
                <div class="form-group">
                    <div class="well well-sm well-primary">
                        <asp:GridView ID="grvTravelAdvances"
                            runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                            OnRowDataBound="grvTravelAdvances_RowDataBound" OnSelectedIndexChanged="grvTravelAdvances_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvTravelAdvances_PageIndexChanging"
                            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active">
                            <RowStyle CssClass="rowstyle" />
                            <Columns>
                                <asp:BoundField DataField="TravelAdvanceNo" HeaderText="Travel Advance No" SortExpression="TravelAdvanceNo" />
                                <asp:BoundField DataField="RequestDate" HeaderText="Request Date" SortExpression="RequestDate" />
                                <asp:BoundField DataField="PurposeOfTravel" HeaderText="Purpose Of Travel" SortExpression="PurposeOfTravel" />
                                <asp:BoundField DataField="TotalTravelAdvance" HeaderText="Total Travel Advance" SortExpression="TotalTravelAdvance" />
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
                <strong>Info!</strong> Please select the Travel Advance Transaction to perform Expense Liquidation for
            </div>
        </asp:Panel>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <div role="content">
                        <div class="jarviswidget-editbox">
                        </div>
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
                                            <a href="#iss1" data-toggle="tab">Expense Form</a>
                                        </li>
                                        <li class="">
                                            <a href="#iss2" data-toggle="tab">Attachments</a>
                                        </li>
                                    </ul>
                                    <div class="tab-content padding-10">
                                        <div class="tab-pane active" id="iss1">
                                            <fieldset>
                                                <div class="row">
                                                    <section class="col col-6">
                                                        <label class="label">Expense Type</label>
                                                        <label class="select">
                                                            <asp:DropDownList ID="ddlPExpenseType" runat="server">
                                                                <asp:ListItem Value="Advance">Advance</asp:ListItem>
                                                            </asp:DropDownList><i></i>
                                                        </label>
                                                    </section>
                                                    <section class="col col-6">
                                                        <label class="label">Purpose of Advance </label>
                                                        <label class="input">
                                                            <asp:TextBox ID="txtComment" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvPurpose" runat="server" ErrorMessage="Purpose of Advance Required" ForeColor="Red" ControlToValidate="txtComment" ValidationGroup="request"></asp:RequiredFieldValidator>
                                                        </label>
                                                    </section>
                                                </div>
                                                <div class="row">
                                                    <section class="col col-6">
                                                        <label class="label">Total Actual Expenditure </label>
                                                        <label class="input">
                                                            <asp:TextBox ID="txtTotActual" Text="0" runat="server" ReadOnly="true"></asp:TextBox>
                                                        </label>
                                                    </section>
                                                    <section class="col col-6">
                                                        <label class="label">Total Travel Advance</label>
                                                        <label class="input">
                                                            <asp:TextBox ID="txtTotalAdvance" runat="server" ReadOnly="true"></asp:TextBox>
                                                        </label>
                                                    </section>
                                                </div>
                                                <div class="row">
                                                    <section class="col col-6">
                                                        <label class="label">Travel Advance Request Date </label>
                                                        <label class="input">
                                                            <i class="icon-append fa fa-calendar"></i>
                                                            <asp:TextBox ID="txtTravelAdvReqDate" runat="server" ReadOnly="true"></asp:TextBox>
                                                        </label>
                                                    </section>
                                                    <section class="col col-6">
                                                        <label class="label">Additional Comment <span style="color: red;">(Please specify reason for OVERSPENT expenses!)</span></label>
                                                        <label class="input">
                                                            <asp:TextBox ID="txtAdditionalComment" runat="server"></asp:TextBox>
                                                        </label>
                                                    </section>
                                                </div>
                                                <div class="row">
                                                    <section class="col" style="width: 100%;">
                                                        <label class="label">
                                                            Arrival Time / Return Time 
                                                            <span style="color: red;">(If you're requesting Per Diems Please specify your Arrival Date & Time and Return Date & Time!)</span>
                                                        </label>
                                                        <label class="input">
                                                            <asp:LinkButton ID="lnkArrivalReturnTime" runat="server" Text="Choose Time"
                                                                CssClass="btn btn-success" data-type="combodate" data-format="DD-MM-YYYY h:mm a"
                                                                data-template="DD / MM / YYYY hh : mm a" data-viewformat="MMM D YYYY hh:mm a"
                                                                data-pk="1" data-original-title="Setup event date and time"
                                                                Style="padding: 6px 12px;"></asp:LinkButton>
                                                            <asp:LinkButton ID="lnkSetArrivalReturnTime" CssClass="btn btn-primary" runat="server"
                                                                OnClientClick="setArrivalReturnTimeVal(); return false;"
                                                                Style="padding: 6px 12px;" Text="Set Arrival/Return Time"></asp:LinkButton>
                                                            <asp:TextBox ID="txtArrivalReturnTime" runat="server"></asp:TextBox>
                                                        </label>
                                                    </section>
                                                </div>
                                                <br />
                                                <asp:Label ID="lblAdvancedDetails" runat="server" Font-Bold="true" ForeColor="Red" Text="Please specify values for the following Advanced expenses!"></asp:Label>
                                                <asp:GridView ID="grvAdvancedDetails"
                                                    runat="server" AutoGenerateColumns="False"
                                                    CssClass="table table-striped table-bordered table-hover">
                                                    <RowStyle CssClass="rowstyle" />
                                                    <Columns>
                                                        <asp:BoundField DataField="ExpenseType" HeaderText="Expense Type" />
                                                        <asp:BoundField DataField="AmountAdvanced" HeaderText="Amount Advanced" />
                                                    </Columns>
                                                    <FooterStyle CssClass="FooterStyle" />
                                                    <HeaderStyle CssClass="headerstyle" />
                                                    <PagerStyle CssClass="PagerStyle" />
                                                    <RowStyle CssClass="rowstyle" />
                                                </asp:GridView>
                                                <br />
                                                <asp:DataGrid ID="dgExpenseLiquidationDetail" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                                                    CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                                                    OnEditCommand="dgExpenseLiquidationDetail_EditCommand" OnDeleteCommand="dgExpenseLiquidationDetail_DeleteCommand" OnUpdateCommand="dgExpenseLiquidationDetail_UpdateCommand"
                                                    GridLines="None" OnItemDataBound="dgExpenseLiquidationDetail_ItemDataBound" ShowFooter="True" OnItemCommand="dgExpenseLiquidationDetail_ItemCommand">

                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="Account Name">
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountName")%>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:DropDownList ID="ddlAccountDescription" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddlAccountDescription_SelectedIndexChanged">
                                                                    <asp:ListItem Value="0">Select Account</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvddlAccountDescription" runat="server" ControlToValidate="ddlAccountDescription" CssClass="validator" Display="Dynamic" ErrorMessage="Account must be selected" InitialValue="0" SetFocusOnError="true" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                                                <i></i>
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlFAccountDescription" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddlAccountDescription_SelectedIndexChanged">
                                                                    <asp:ListItem Value="0">Select Account</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvFddlAccountDescription" runat="server" ControlToValidate="ddlFAccountDescription" CssClass="validator" Display="Dynamic" ErrorMessage="Account must be selected" InitialValue="0" SetFocusOnError="true" ValidationGroup="save"></asp:RequiredFieldValidator>
                                                                <i></i>
                                                            </FooterTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Account Code">
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountCode")%>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtAccountCode" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountCode")%>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtAccountCode" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </FooterTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Expense Type">
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container.DataItem, "ExpenseType.ExpenseTypeName")%>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:DropDownList ID="ddlEdtExpenseType" CssClass="form-control" runat="server" Enabled="true" DataValueField="Id" DataTextField="ExpenseTypeName" AppendDataBoundItems="True">
                                                                    <asp:ListItem Value="0">--Select Expense Type--</asp:ListItem>
                                                                </asp:DropDownList><i></i>
                                                                <asp:RequiredFieldValidator ID="rfvddlEdtExpenseType" runat="server" ControlToValidate="ddlEdtExpenseType" CssClass="validator" Display="Dynamic" ErrorMessage="Expense Type must be selected" InitialValue="0" SetFocusOnError="true" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlExpenseType" CssClass="form-control" runat="server" Enabled="true" DataValueField="Id" DataTextField="ExpenseTypeName" AppendDataBoundItems="True">
                                                                    <asp:ListItem Value="0">--Select Expense Type--</asp:ListItem>
                                                                </asp:DropDownList><i></i>
                                                                <asp:RequiredFieldValidator ID="rfvddlExpenseType" runat="server" ControlToValidate="ddlExpenseType" CssClass="validator" Display="Dynamic" ErrorMessage="Expense Type must be selected" InitialValue="0" SetFocusOnError="true" ValidationGroup="save"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Project ID">
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container.DataItem, "Project.ProjectCode")%>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:DropDownList ID="ddlProject" runat="server" Enabled="false" AutoPostBack="true" CssClass="form-control" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                                                                    <asp:ListItem Value="0">Select Project</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvddlProject" runat="server" ControlToValidate="ddlProject" CssClass="validator" Display="Dynamic" ErrorMessage="Project must be selected" InitialValue="0" SetFocusOnError="true" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                                                <i></i>
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlFProject" runat="server" Enabled="false" AutoPostBack="true" CssClass="form-control" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlFProject_SelectedIndexChanged">
                                                                    <asp:ListItem Value="0">Select Project</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvddlFProject" runat="server" ControlToValidate="ddlFProject" CssClass="validator" Display="Dynamic" ErrorMessage="Project must be selected" InitialValue="0" SetFocusOnError="true" ValidationGroup="save"></asp:RequiredFieldValidator>
                                                                <i></i>
                                                            </FooterTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Grant ID">
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container.DataItem, "Grant.GrantCode")%>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:DropDownList ID="ddlGrant" runat="server" Enabled="false" AppendDataBoundItems="True" CssClass="form-control" DataTextField="GrantCode" DataValueField="Id">
                                                                    <asp:ListItem Value="0">Select Grant</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="RfvGrant" runat="server" CssClass="validator" ControlToValidate="ddlGrant" ErrorMessage="Grant is required" InitialValue="0" SetFocusOnError="True" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlFGrant" runat="server" Enabled="false" AppendDataBoundItems="True" CssClass="form-control" DataTextField="GrantCode" DataValueField="Id" EnableViewState="true">
                                                                    <asp:ListItem Value="0">Select Grant</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="RfvFGrantCode" runat="server" CssClass="validator" ControlToValidate="ddlFGrant" Display="Dynamic" ErrorMessage="Grant is required" InitialValue="0" SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Amount Advanced">
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container.DataItem, "AmountAdvanced")%>
                                                                <asp:HiddenField ID="hfAmountAdvanced" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "AmountAdvanced")%>'></asp:HiddenField>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "AmountAdvanced")%>'></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvtxtAmount" runat="server" ControlToValidate="txtAmount" CssClass="validator" ErrorMessage="Amount advanced is required" ValidationGroup="edit" InitialValue=""></asp:RequiredFieldValidator>
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtFAmount" runat="server" CssClass="form-control">0</asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvtxtFAmount" runat="server" CssClass="validator" ControlToValidate="txtFAmount" ErrorMessage="Amount advanced is required" ValidationGroup="save" InitialValue=""></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Actual Expenditure">
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container.DataItem, "ActualExpenditure")%>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtActualExpenditure" runat="server" CssClass="form-control"
                                                                    onkeypress="return IsOneDecimalPoint(event);"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem, "ActualExpenditure")%>'></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtActualExpenditure" ID="txtActualExpenditure_FilteredTextBoxExtender" FilterType="Custom,Numbers" ValidChars="."></cc1:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="rfvActualExpenditure" runat="server" ControlToValidate="txtActualExpenditure" CssClass="validator" ErrorMessage="Actual Expenditure is required" ValidationGroup="edit" InitialValue=""></asp:RequiredFieldValidator>
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtFActualExpenditure" onkeypress="return IsOneDecimalPoint(event);" runat="server" CssClass="form-control"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtFActualExpenditure" ID="txtFActualExpenditure_FilteredTextBoxExtender" FilterType="Custom,Numbers" ValidChars="."></cc1:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="rfvtxtFActualExpenditure" runat="server" CssClass="validator" ControlToValidate="txtFActualExpenditure" ErrorMessage="Actual Expenditure is required" ValidationGroup="save" InitialValue=""></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Variance">
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container.DataItem, "Variance")%>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtVariance" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "Variance")%>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtFVariance" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </FooterTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Actions">
                                                            <EditItemTemplate>
                                                                <asp:LinkButton ID="lnkUpdate" runat="server" CausesValidation="true" CommandName="Update" CssClass="btn btn-xs btn-default" ValidationGroup="edit"><i class="fa fa-save"></i></asp:LinkButton>
                                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:LinkButton ID="lnkAddNew" runat="server" CausesValidation="true" CommandName="AddNew" CssClass="btn btn-sm btn-success" ValidationGroup="save"><i class="fa fa-save"></i></asp:LinkButton>
                                                            </FooterTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                    </Columns>
                                                    <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                                </asp:DataGrid>
                                            </fieldset>
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
                    </div>
                    <footer>
                        <asp:Button ID="btnSave" runat="server" CausesValidation="true" ValidationGroup="request" Visible="false" Text="Request" OnClick="btnSave_Click" class="btn btn-primary"
                            UseSubmitBehavior="false" OnClientClick="this.disabled = true; this.value = 'Submitting...';"></asp:Button>
                        <asp:Button ID="btnSearch" runat="server" OnClientClick="showLiquidationSearch(); return false;" Text="Search" class="btn btn-default" />
                        <%--<a data-toggle="modal" runat="server" id="searchLink" href="#searchModal" class="btn btn-default"><i class="fa fa-circle-arrow-up fa-lg"></i>Search</a>--%>
                        <asp:Button ID="btnDelete" runat="server" CausesValidation="False" class="btn btn-default"
                            Text="Delete" OnClick="btnDelete_Click" Visible="False"></asp:Button>
                        <cc1:ConfirmButtonExtender ID="btnDelete_ConfirmButtonExtender" runat="server"
                            ConfirmText="Are you sure" Enabled="True" TargetControlID="btnDelete">
                        </cc1:ConfirmButtonExtender>
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-default" OnClick="btnCancel_Click" Text="New" />
                        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-default" Enabled="false" OnClientClick="javascript:printLiquidationForm('divprint')"></asp:Button>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" CssClass="btn btn-default" PostBackUrl="../Default.aspx"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="searchModal" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                    <h4 class="modal-title" id="myModalLabel">Search Expense Liquidation Requests</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
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
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlSrchExpenseType">Expense Type</label><br />
                                <asp:DropDownList ID="ddlSrchExpenseType" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="Advance">Advance</asp:ListItem>
                                </asp:DropDownList><i></i>
                            </div>
                        </div>

                    </div>
                    <div class="row" style="text-align: right;">
                        <div class="col-md-12">
                            <div class="form-group">
                                <asp:Button ID="btnFind" runat="server" OnClick="btnFind_Click" Text="Find" CssClass="btn btn-primary"></asp:Button>
                                <asp:Button Text="Close" ID="Button1" runat="server" class="btn btn-primary"></asp:Button>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <div class="well well-sm well-primary">
                                    <asp:GridView ID="grvExpenseLiquidationRequestList"
                                        runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                        OnRowDataBound="grvExpenseLiquidationRequestList_RowDataBound" OnSelectedIndexChanged="grvExpenseLiquidationRequestList_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvExpenseLiquidationRequestList_PageIndexChanging"
                                        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active">
                                        <RowStyle CssClass="rowstyle" />
                                        <Columns>
                                            <asp:BoundField HeaderText="Travel Advance No" SortExpression="TravelAdvanceNo" />
                                            <asp:BoundField DataField="RequestDate" HeaderText="Request Date" SortExpression="RequestDate" />
                                            <asp:BoundField DataField="ExpenseType" HeaderText="Expense Type" SortExpression="ExpenseType" />
                                            <asp:CommandField ShowSelectButton="True" />
                                        </Columns>
                                        <FooterStyle CssClass="FooterStyle" />
                                        <HeaderStyle CssClass="headerstyle" />
                                        <PagerStyle CssClass="PagerStyle" />
                                        <RowStyle CssClass="rowstyle" />
                                    </asp:GridView>
                                    <div class="alert alert-info fade in">
                                        <button class="close" data-dismiss="alert">
                                            ×
                                        </button>
                                        <i class="fa-fw fa fa-info"></i>
                                        <strong>Info!</strong> Rejected Expense Liquidations are highlighted in <span style="color: red">Red Color</span>
                                    </div>
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
    </asp:Panel>
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
                            <asp:Label ID="lblArrRetTime" runat="server" Text="Arrival/Return Date & Time:"></asp:Label>
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
