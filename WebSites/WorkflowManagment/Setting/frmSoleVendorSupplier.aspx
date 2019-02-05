<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmSoleVendorSupplier.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Setting.Views.frmSoleVendorSupplier" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>


<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
    <asp:ValidationSummary ID="NewValidationSummary1" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="2" ForeColor="" />
    <asp:ValidationSummary ID="EditValidationSummary2" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="1" ForeColor="" />
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Search Supplier</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-6">
                                <asp:Label ID="lblSupplierName" runat="server" Text="Supplier Name" CssClass="label"></asp:Label>

                                <label class="input">

                                    <asp:TextBox ID="txtSupplierName" runat="server"></asp:TextBox></label>
                            </section>
                        </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" CssClass="btn btn-primary"></asp:Button>
                          <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>


        <asp:DataGrid ID="dgSupplier" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
            GridLines="None"
            OnCancelCommand="dgSupplier_CancelCommand" OnDeleteCommand="dgSupplier_DeleteCommand" OnEditCommand="dgSupplier_EditCommand"
            OnItemCommand="dgSupplier_ItemCommand" OnItemDataBound="dgSupplier_ItemDataBound" OnUpdateCommand="dgSupplier_UpdateCommand"
            ShowFooter="True" OnSelectedIndexChanged="dgSupplier_SelectedIndexChanged">

            <Columns>
                <asp:TemplateColumn HeaderText="Supplier Type">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "SupplierType.SupplierTypeName")%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlEdtSupplierType" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                            <asp:ListItem Value="0">-Supplier Type-</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorType" runat="server" ControlToValidate="ddlEdtSupplierType" ErrorMessage="Supplier Type Required" ValidationGroup="1" InitialValue="0">*</asp:RequiredFieldValidator>
                        <i></i>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddlSupplierType" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                            <asp:ListItem Value="0">-Supplier Type-</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorFType" runat="server" CssClass="validator" ControlToValidate="ddlSupplierType" ErrorMessage="Supplier Type Required" ValidationGroup="2" InitialValue="0">*</asp:RequiredFieldValidator>
                        <i></i>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Supplier Name">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "SupplierName")%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSupplierName" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "SupplierName")%>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="validator" ControlToValidate="txtSupplierName" ErrorMessage="Supplier Name Required" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtFSupplierName" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="validator" ControlToValidate="txtFSupplierName" ErrorMessage="Supplier Name Required" ValidationGroup="2">*</asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Supplier Address">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "SupplierAddress")%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSupplierAddress" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "SupplierAddress")%>'></asp:TextBox>

                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtFSupplierAddress" runat="server" CssClass="form-control"></asp:TextBox>

                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Supplier Contact">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "SupplierContact")%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSupplierContact" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "SupplierContact")%>'></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtFSupplierContact" runat="server" CssClass="form-control"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Supplier Contact Phone No">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ContactPhone")%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSupplierphoneContact" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "ContactPhone")%>'></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtFSupplierphoneContact" runat="server" CssClass="form-control"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateColumn>
                 <asp:TemplateColumn HeaderText="Email">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Email")%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSupplierEmail" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "Email")%>'></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtFSupplierEmail" runat="server" CssClass="form-control"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateColumn>
                   <asp:TemplateColumn HeaderText="Starting Date">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "StartingDate")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEdtStartingDate" runat="server" CssClass="form-control datepicker"
                                                data-dateformat="mm/dd/yy" Text=' <%# DataBinder.Eval(Container.DataItem, "StartingDate","{0:M/d/yyyy}")%>'></asp:TextBox>
                                            <asp:RequiredFieldValidator
                                                ID="rfvEdtFromDate" runat="server" ErrorMessage="Starting date is required" Display="Dynamic"
                                                CssClass="validator" ValidationGroup="proedit" 
                                                SetFocusOnError="true" ControlToValidate="txtEdtStartingDate"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvEdtStartEndDates" runat="server" ErrorMessage="Starting Date must be less than End Date"
                                                ControlToCompare="txtEdtEndDate" ControlToValidate="txtEdtStartingDate"
                                                ValidationGroup="proedit" Type="Date" Operator="LessThan"></asp:CompareValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtStartingDate" runat="server" CssClass="form-control datepicker"
                                                data-dateformat="mm/dd/yy"></asp:TextBox>
                                            <asp:RequiredFieldValidator
                                                ID="rfvtxtFromDate" runat="server" ErrorMessage="Starting date is required" Display="Dynamic"
                                                CssClass="validator" ValidationGroup="proadd"
                                                SetFocusOnError="true" ControlToValidate="txtStartingDate"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvStartEndDates" runat="server" ErrorMessage="Starting Date must be less than End Date"
                                                ControlToCompare="txtEndDate" ControlToValidate="txtStartingDate"
                                                ValidationGroup="proadd" Type="Date" Operator="LessThan"></asp:CompareValidator>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="End Date">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "EndDate")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEdtEndDate" runat="server" CssClass="form-control datepicker"
                                                data-dateformat="mm/dd/yy" Text=' <%# DataBinder.Eval(Container.DataItem, "EndDate","{0:M/d/yyyy}")%>'></asp:TextBox>
                                            <asp:RequiredFieldValidator
                                                ID="rfvtxtEdtToDate" runat="server" ErrorMessage="End date is required" Display="Dynamic"
                                                CssClass="validator" ValidationGroup="proedit"
                                                SetFocusOnError="true" ControlToValidate="txtEdtEndDate"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control datepicker"
                                                data-dateformat="mm/dd/yy"></asp:TextBox>
                                            <asp:RequiredFieldValidator
                                                ID="rfvtxtEndDate" runat="server" ErrorMessage="End date is required" Display="Dynamic"
                                                CssClass="validator" ValidationGroup="proadd"
                                                SetFocusOnError="true" ControlToValidate="txtEndDate"></asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                 <asp:TemplateColumn HeaderText="Status">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlStatus" runat="server" Width="90px" CssClass="form-control"
                                                AppendDataBoundItems="True"
                                                ValidationGroup="3">
                                                <asp:ListItem Value="">Select Status</asp:ListItem>
                                                <asp:ListItem Value="Active">Active</asp:ListItem>
                                                <asp:ListItem Value="InActive">InActive</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RfvStatus" runat="server"
                                                ControlToValidate="ddlStatus" ErrorMessage="Status Required"
                                                InitialValue="" SetFocusOnError="True" ValidationGroup="proedit">*</asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlFStatus" runat="server" Width="90px" CssClass="form-control"
                                                AppendDataBoundItems="True" EnableViewState="true" ValidationGroup="proadd">
                                                <asp:ListItem Value="">Select Status</asp:ListItem>
                                                <asp:ListItem Value="Active">Active</asp:ListItem>
                                                <asp:ListItem Value="InActive">InActive</asp:ListItem>

                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RfvFStatus" runat="server"
                                                ControlToValidate="ddlFStatus" Display="Dynamic"
                                                ErrorMessage="Status Required" InitialValue="" SetFocusOnError="True"
                                                ValidationGroup="proadd">*</asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Status")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Actions">
                    <EditItemTemplate>
                        <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" ValidationGroup="1" CssClass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:LinkButton ID="lnkAddNew" runat="server" CommandName="AddNew" ValidationGroup="2" CssClass="btn btn-sm btn-success"><i class="fa fa-save"></i></asp:LinkButton>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');"><i class="fa fa-times"></i></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
            <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center"/>
        </asp:DataGrid>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="menuContent" runat="Server">
</asp:Content>

