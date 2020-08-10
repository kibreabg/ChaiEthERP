<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmMaintenanceRequest.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Request.Views.frmMaintenanceRequest" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script src="../js/libs/jquery-2.0.2.min.js"></script>
    <script type="text/javascript">
        function showSearch() {
            $(document).ready(function () {
                $('#searchModal').modal('show');
            });
        }
    </script>
    <asp:ValidationSummary ID="VSPurchaseRequest" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="Save" ForeColor="" />
    <asp:ValidationSummary ID="VSDetailAdd" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="proadd" ForeColor="" />
    <asp:ValidationSummary ID="VSEdit" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="proedit" ForeColor="" />
    <div id="wid-id-0" class="jarviswidget" data-widget-custombutton="false" data-widget-editbutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Maintenance Request</h2>
        </header>


        <!-- widget div-->
        <div>

            <!-- widget edit box -->
            <div class="jarviswidget-editbox">
                <!-- This area used as dropdown edit box -->

            </div>
            <!-- end widget edit box -->

            <!-- widget content -->
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                         <div class="row">
                            <section class="col col-6">
                                <label class="label">
                                    Requester</label>
                                <label class="input">
                                    <asp:TextBox ID="txtRequester" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label class="label">
                                    Plate No</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlPlate" AutoPostBack="true" runat="server" DataValueField="Id" DataTextField="PlateNo">
                                    </asp:DropDownList><i id="i1" runat="server"></i>
                                </label>
                            </section>

                              
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">
                                    Km Reading</label>
                                <label class="input">
                                    <asp:TextBox ID="txtKMReading" runat="server" Visible="true"></asp:TextBox>
                                  <asp:RequiredFieldValidator ID="RfvKM" CssClass="validator" runat="server" ControlToValidate="txtKMReading" ErrorMessage="KM Reading To Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </label>
                            </section>
                           
                            <section class="col col-6">
                                <label id="lblRequestDate" runat="server" class="label" visible="true">
                                    Request Date</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtRequestDate" ReadOnly="true" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>

                        </div>

                        <div class="row">

                           
                            <section class="col col-6">
                                <label class="label">
                                    Action Taken</label>
                                <label class="input">
                                    <asp:TextBox ID="ActionTaken" runat="server" Visible="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfvActionTaken" CssClass="validator" runat="server" ControlToValidate="ActionTaken" ErrorMessage="Action Taken To Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </label>
                            </section>
                        
                     
                            <section class="col col-6">
                                <label class="label">
                                   Remark</label>
                                <label class="input">
                                    <asp:TextBox ID="txtRemark" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section></div>
                              <div class="row">                            
                            <section class="col col-6">
                                <label class="label">Project</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlProject" AutoPostBack="true" runat="server" DataValueField="Id" DataTextField="ProjectCode" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                                    </asp:DropDownList><i></i>
                                    <asp:RequiredFieldValidator
                                        ID="rfvddlProject" runat="server" ErrorMessage="Project is required" Display="Dynamic"
                                        CssClass="validator" ValidationGroup="saveMain" InitialValue="0"
                                        SetFocusOnError="true" ControlToValidate="ddlProject"></asp:RequiredFieldValidator>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label class="label">Grant</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlGrant" runat="server" DataValueField="Id" DataTextField="GrantCode">
                                    </asp:DropDownList><i></i>
                                    <asp:RequiredFieldValidator
                                        ID="rfvGrant" runat="server" ErrorMessage="Grant is required" Display="Dynamic"
                                        CssClass="validator" ValidationGroup="saveMain" InitialValue="0"
                                        SetFocusOnError="true" ControlToValidate="ddlGrant"></asp:RequiredFieldValidator>
                                </label>
                            </section>
                        </div>
                           


                       
                        <asp:DataGrid ID="dgMaintenanceRequestDetail" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                            GridLines="None"
                            OnCancelCommand="dgMaintenanceRequestDetail_CancelCommand" OnDeleteCommand="dgMaintenanceRequestDetail_DeleteCommand" OnEditCommand="dgMaintenanceRequestDetail_EditCommand"
                            OnItemCommand="dgMaintenanceRequestDetail_ItemCommand" OnItemDataBound="dgMaintenanceRequestDetail_ItemDataBound" OnUpdateCommand="dgMaintenanceRequestDetail_UpdateCommand"
                            ShowFooter="True">

                            <Columns>
                                <asp:TemplateColumn HeaderText="Service Type">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlServiceTpe" runat="server" CssClass="form-control"
                                            AppendDataBoundItems="True" DataTextField="Name" DataValueField="Id"
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
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "ServiceType.Name")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                 <asp:TemplateColumn HeaderText="Driver Service Type Detail">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "DriverServiceTypeDetail.Description")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                          <asp:DropDownList ID="ddlEdtServiceTypeDet" runat="server"  CssClass="form-control" Enabled="true" DataValueField="Id" DataTextField="Description" AppendDataBoundItems="True">
                                            <asp:ListItem Value="0">--Select Service Detail--</asp:ListItem>
                                        </asp:DropDownList><i></i>
                                     
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                         <asp:DropDownList ID="ddlServiceTypeDet" runat="server"  CssClass="form-control" Enabled="true" DataValueField="Id" DataTextField="Description" AppendDataBoundItems="True">
                                            <asp:ListItem Value="0">--Select Service Detail--</asp:ListItem>
                                        </asp:DropDownList><i></i>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                               <%--<asp:TemplateColumn HeaderText=" Mechanic Service Type Detail">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "ServiceTypeDetail.Description")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                          <asp:DropDownList ID="ddlEdtMecServiceTypeDet" runat="server" Enabled="true" DataValueField="Id" DataTextField="Description" AppendDataBoundItems="True">
                                            <asp:ListItem Value="0">--Select Service Detail--</asp:ListItem>
                                        </asp:DropDownList><i></i>
                                     
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                         <asp:DropDownList ID="ddlMecServiceTypeDet" runat="server" Enabled="true" DataValueField="Id" DataTextField="Description" AppendDataBoundItems="True">
                                            <asp:ListItem Value="0">--Select Service Detail--</asp:ListItem>
                                        </asp:DropDownList><i></i>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Mechanic Remark">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "TechnicianRemark")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "TechnicianRemark")%>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RfvRemark" CssClass="validator" runat="server" ControlToValidate="txtRemark" ErrorMessage="Remark Required" ValidationGroup="proedit"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtFRemark" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RfvFRemark" CssClass="validator" runat="server" ControlToValidate="txtFRemark" ErrorMessage="Remark Required" ValidationGroup="proadd"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateColumn>--%>
                               
                               
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
                    </fieldset>

                    <footer>
                        <asp:Button ID="btnRequest" runat="server" CssClass="btn btn-primary" OnClick="btnRequest_Click" Text="Request" ValidationGroup="Save" />
                        <a data-toggle="modal" runat="server" id="searchLink" href="#searchModal" class="btn btn-primary"><i class="fa fa-circle-arrow-up fa-lg"></i>Search</a>
                        <%--<asp:Button ID="btnsearch2" runat="server" CssClass="btn btn-primary" Text="Search" />--%>
                        <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-primary" Text="Delete" OnClick="btnDelete_Click" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');" TabIndex="9" Visible="False" />

                        <%--<asp:Button ID="btnSearch" data-toggle="modal" runat="server" OnClientClick="#myModal" Text="Search" ></asp:Button>--%>
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" Text="New" />
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>

                    </footer>

                </div>
            </div>
            <!-- end widget content -->

        </div>
        <!-- end widget div -->

    </div>

    <div class="modal fade" id="searchModal" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body no-padding">
                    <div class="jarviswidget" data-widget-colorbutton="false"
                        data-widget-editbutton="false"
                        data-widget-togglebutton="false"
                        data-widget-deletebutton="false"
                        data-widget-fullscreenbutton="false"
                        data-widget-custombutton="false"
                        data-widget-sortable="false">
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
                                            <section class="col col-6">
                                                <asp:Label ID="lblRequestNosearch" runat="server" Text="Request No" CssClass="label"></asp:Label>

                                                <label class="input">
                                                    <asp:TextBox ID="txtRequestNosearch" runat="server" Visible="true"></asp:TextBox>
                                                </label>
                                            </section>
                                            <section class="col col-6">
                                                <asp:Label ID="lblRequestDatesearch" runat="server" Text="Request Date" CssClass="label"></asp:Label>

                                                <label class="input">
                                                    <i class="icon-append fa fa-calendar"></i>
                                                    <asp:TextBox ID="txtRequestDatesearch" CssClass="form-control datepicker"
                                                        data-dateformat="mm/dd/yy" runat="server" Visible="true"></asp:TextBox>
                                                </label>
                                            </section>
                                        </div>
                                    </fieldset>

                                    <footer>
                                        <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" CssClass="btn btn-primary"></asp:Button>
                                        <asp:Button ID="Button1" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary"></asp:Button>
                                    </footer>
                                </div>
                            </div>
                        </div>
                        <asp:GridView ID="grvMaintenanceRequestList"
                            runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                            OnRowDataBound="grvMaintenanceRequestList_RowDataBound" OnRowDeleting="grvMaintenanceRequestList_RowDeleting"
                            OnSelectedIndexChanged="grvMaintenanceRequestList_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvMaintenanceRequestList_PageIndexChanging"
                            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="5">
                            <RowStyle CssClass="rowstyle" />
                            <Columns>
                                <asp:BoundField DataField="RequestNo" HeaderText="Request No" SortExpression="RequestNo" />
                                <asp:TemplateField HeaderText="Request Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("RequestDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                              

                                <asp:BoundField DataField="PlateNo" HeaderText="Plate No" SortExpression="PlateNo" />
                              

                                <asp:CommandField ShowSelectButton="True" />
                                <%-- <asp:CommandField ShowDeleteButton="True" />--%>
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
    <!-- /.modal-content -->
    <%--<asp:ModalPopupExtender runat="server" Enabled="True" PopupControlID="pnlSearch"
        TargetControlID="btnsearch2" CancelControlID="Button1" BackgroundCssClass="modalBackground" ID="pnlSearch_ModalPopupExtender">
    </asp:ModalPopupExtender>--%>

    <asp:Panel ID="pnlWarning" Visible="false" Style="position: absolute; top: 370px; left: 84px;" runat="server">
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
<asp:Content ID="Content3" ContentPlaceHolderID="menuContent" runat="Server">
</asp:Content>

