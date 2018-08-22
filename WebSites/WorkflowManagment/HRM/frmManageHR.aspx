<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmManageHR.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.HRM.Views.frmManageHR"
    Title="Manage HR" MasterPageFile="~/Shared/ModuleMaster.master" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div class="jarviswidget" id="wid-id-5" data-widget-colorbutton="false" data-widget-editbutton="false" data-widget-fullscreenbutton="false" data-widget-custombutton="false" data-widget-sortable="false" role="widget" style="">
        <header role="heading">
            <div class="jarviswidget-ctrls" role="menu">
            </div>
            <h2>Manage HR</h2>
        </header>
        <script src="../js/libs/jquery-2.0.2.min.js"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                // fuelux wizard
                var wizard = $('.wizard').wizard();

                wizard.on('finished', function (e, data) {
                    //$("#fuelux-wizard").submit();
                    //console.log("submitted!");
                    $.smallBox({
                        title: "Congratulations! Your form was submitted",
                        content: "<i class='fa fa-clock-o'></i> <i>1 seconds ago...</i>",
                        color: "#5F895F",
                        iconSmall: "fa fa-check bounce animated",
                        timeout: 4000
                    });

                });
            });
        </script>
        <!-- widget div-->
        <div role="content">

            <!-- widget edit box -->
            <div class="jarviswidget-editbox">
                <!-- This area used as dropdown edit box -->

            </div>
            <!-- end widget edit box -->

            <!-- widget content -->
            <div class="widget-body">

                <div class="tabs-left">
                    <ul class="nav nav-tabs tabs-left" id="demo-pill-nav">
                        <li class="active">
                            <a href="#tab-r1" data-toggle="tab"><span class="badge bg-color-blue txt-color-white">12</span> Contract </a>
                        </li>
                        <li class="">
                            <a href="#tab-r2" data-toggle="tab"><span class="badge bg-color-blueDark txt-color-white">3</span> Change </a>
                        </li>
                        <li class="">
                            <a href="#tab-r3" data-toggle="tab"><span class="badge bg-color-greenLight txt-color-white">0</span> Warning</a>
                        </li>
                        <li class="">
                            <a href="#tab-r4" data-toggle="tab"><span class="badge bg-color-greenLight txt-color-white">0</span> Termination</a>
                        </li>
                    </ul>
                    <div class="tab-content">
                        <div class="tab-pane active " id="tab-r1">
                            <fieldset>

                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="icon-append fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control datepicker" placeholder="Contract Start Date"></asp:TextBox>



                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="icon-append fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control datepicker" placeholder="Contract End Date" data-dateformat="dd/mm/yy"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-flag fa-fw"></i></span>
                                                <asp:DropDownList ID="ddlReason" runat="server" CssClass="form-control" placeholder="Status" AppendDataBoundItems="True">
                                                    <asp:ListItem Value="New Hire">New Hire</asp:ListItem>
                                                    <asp:ListItem Value="Change">Change</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-user fa-fw"></i></span>
                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" placeholder="Status" AppendDataBoundItems="True">
                                                    <asp:ListItem Value="Active">Active</asp:ListItem>
                                                    <asp:ListItem Value="In Active">In Active</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </fieldset>
                            <div class="form-actions">
                                <div class="row">
                                    <div class="col-md-12">
                                        <button class="btn btn-default" type="submit">
                                            Cancel
                                        </button>
                                        <asp:Button ID="btnAddcontract" runat="server" CssClass="btn btn-primary" Text="Add Contract" ValidationGroup="Savedetail" OnClick="btnAddcontract_Click" />
                                          <button class="btn btn-default" type="submit">
                                            Generate PAF
                                        </button>
                                    </div>
                                </div>
                            </div>
                            <fieldset>
                                <legend></legend>
                            </fieldset>
                            <fieldset>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <h1>Employee Contracts</h1>
                                        <fieldset>
                                            <asp:GridView ID="dgContractDetail" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                                DataKeyNames="Id" ForeColor="#333333"
                                                GridLines="Horizontal" CssClass="table table-striped table-bordered table-hover"
                                                PagerStyle-CssClass="paginate_button active" AlternatingRowStyle-CssClass=""
                                                Width="100%" Style="text-align: left"
                                                AllowPaging="True" PageSize="20" OnSelectedIndexChanged="dgContractDetail_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:BoundField DataField="ContractStartDate" HeaderText="Contract Start Date" />
                                                    <asp:BoundField DataField="ContractEndDate" HeaderText="Contract End Date" />
                                                    <asp:BoundField DataField="Reason" HeaderText="Reason" />
                                                    <asp:BoundField DataField="Status" HeaderText="Status" />




                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                            <div class="btn-group open">
                                                               <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');"><i class="fa fa-times"></i></asp:LinkButton>
                                                            
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                            </asp:GridView>

                                        </fieldset>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                        <div class="tab-pane " id="tab-r2">
                            <fieldset>

                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-envelope fa-fw"></i></span>
                                                <asp:DropDownList ID="ddlJobTitle" runat="server" CssClass="form-control" placeholder="Job Title" AppendDataBoundItems="True"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-envelope fa-fw"></i></span>
                                                <asp:DropDownList ID="ddlPosition" runat="server" CssClass="form-control" placeholder="Position" AppendDataBoundItems="True"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-flag fa-fw"></i></span>
                                                <asp:DropDownList ID="ddlProgram" runat="server" CssClass="form-control" placeholder="Program" AppendDataBoundItems="True"></asp:DropDownList>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-user fa-fw"></i></span>
                                                <asp:DropDownList ID="ddlDutyStation" runat="server" CssClass="form-control" placeholder="Duty Station" AppendDataBoundItems="True">
                                                    <asp:ListItem Value="0">Select DutyStation </asp:ListItem>
                                                    <asp:ListItem Value="1">Addis Ababa</asp:ListItem>
                                                    <asp:ListItem Value="2">SNNPR</asp:ListItem>
                                                    <asp:ListItem Value="2">Tigray</asp:ListItem>
                                                    <asp:ListItem Value="4">Oromia</asp:ListItem>
                                                    <asp:ListItem Value="5">Amhara</asp:ListItem>
                                                    <asp:ListItem Value="6">Afar</asp:ListItem>
                                                    <asp:ListItem Value="7">Somali</asp:ListItem>
                                                    <asp:ListItem Value="8">Harari</asp:ListItem>
                                                    <asp:ListItem Value="9">Dire Dawa</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-flag fa-fw"></i></span>
                                                <asp:TextBox ID="txtSalary" runat="server" CssClass="form-control" placeholder="Salary"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-user fa-fw"></i></span>
                                                <asp:DropDownList ID="txtEmployeeStatus" runat="server" CssClass="form-control" placeholder="Employement Status">
                                                    <asp:ListItem Value="0">Select Employement Status</asp:ListItem>
                                                    <asp:ListItem Value="1">Fulltime</asp:ListItem>
                                                    <asp:ListItem Value="2">Temporary</asp:ListItem>
                                                    <asp:ListItem Value="3">Volunteer</asp:ListItem>
                                                    <asp:ListItem Value="4">Intern</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-flag fa-fw"></i></span>

                                                <asp:DropDownList ID="txtClass" runat="server" CssClass="form-control" placeholder="Class">
                                                    <asp:ListItem Value="0">Select Class</asp:ListItem>
                                                    <asp:ListItem Value="1">Local National</asp:ListItem>
                                                    <asp:ListItem Value="2">Expat</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-user fa-fw"></i></span>
                                                <asp:TextBox ID="txtHoursPerWeek" runat="server" CssClass="form-control" placeholder="Hours Per Week"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-flag fa-fw"></i></span>
                                                <asp:TextBox ID="txtBaseCount" runat="server" CssClass="form-control" placeholder="Base Country"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-user fa-fw"></i></span>
                                                <asp:TextBox ID="txtBaseCity" runat="server" CssClass="form-control" placeholder="Base City"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-flag fa-fw"></i></span>
                                                <asp:TextBox ID="txtBaseState" runat="server" CssClass="form-control" placeholder="Base State"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-user fa-fw"></i></span>
                                                <asp:TextBox ID="txtCountryTeam" runat="server" CssClass="form-control" placeholder="Country Team"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-flag fa-fw"></i></span>
                                                <asp:TextBox ID="txtDescJT" runat="server" CssClass="form-control" placeholder="Descriptive Job Title"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-user fa-fw"></i></span>
                                                <asp:DropDownList ID="ddlSuperVisor" runat="server" CssClass="form-control" placeholder="Supervisor" AppendDataBoundItems="True"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-flag fa-fw"></i></span>
                                                <asp:DropDownList ID="ddlReportsTo" runat="server" CssClass="form-control" placeholder="Reports To" AppendDataBoundItems="True"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </fieldset>
                            <div class="form-actions">
                                <div class="row">
                                    <div class="col-md-12">
                                        <button class="btn btn-default" type="submit">
                                            Cancel
                                        </button>
                                        <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="Add Change" ValidationGroup="Savedetail" OnClick="btnAddChange_Click" />
                                        <button class="btn btn-default" type="submit">
                                            Generate PAF
                                        </button>
                                    </div>
                                </div>
                            </div>
                            <fieldset>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <h1>Employee History Detail</h1>
                                        <fieldset>
                                            <asp:DataGrid ID="dgChange" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                                                CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                                                GridLines="None"
                                                ShowFooter="True" OnEditCommand="dgChange_EditCommand" OnSelectedIndexChanged="dgChange_SelectedIndexChanged">

                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="Job Title">

                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "JobTitle.JobTitleName")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>

                                                    <asp:TemplateColumn HeaderText="Position">
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "Position.PositionName")%>
                                                        </ItemTemplate>

                                                    </asp:TemplateColumn>

                                                    <asp:TemplateColumn HeaderText="Program">
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "Program.ProgramName")%>
                                                        </ItemTemplate>

                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Duty Station">
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "DutyStationId")%>
                                                        </ItemTemplate>

                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Salary">

                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "Salary")%>
                                                        </ItemTemplate>
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
                                        </fieldset>

                                    </div>
                                </div>
                            </fieldset>

                        </div>
                        <div class="tab-pane" id="tab-r3">
                            <fieldset>

                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-envelope fa-fw"></i></span>
                                                <asp:TextBox ID="txtWarDesc" runat="server" CssClass="form-control" placeholder="Warning Description"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="icon-append fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtWarningDate" runat="server" CssClass="form-control datepicker" placeholder="Warning Date"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                            </fieldset>
                            <div class="form-actions">
                                <div class="row">
                                    <div class="col-md-12">
                                        <button class="btn btn-default" type="submit">
                                            Cancel
                                        </button>
                                        <asp:Button ID="btnAddWarning" runat="server" CssClass="btn btn-primary" Text="Add Warning" ValidationGroup="Savedetail" OnClick="btnAddWarning_Click" />
                                        <button class="btn btn-default" type="submit">
                                            Generate PAF
                                        </button>
                                    </div>
                                </div>
                            </div>


                            <fieldset>
                                <legend></legend>
                            </fieldset>
                            <fieldset>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <h1>Warnings</h1>
                                        <fieldset>
                                            <asp:DataGrid ID="dgWarning" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                                                CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                                                GridLines="None"
                                                ShowFooter="True">

                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="Job Title">

                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "WarningDescription")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>

                                                    <asp:TemplateColumn HeaderText="Position">
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "WarningDate")%>
                                                        </ItemTemplate>

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
                                        </fieldset>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                        <div class="tab-pane" id="tab-r4">
                            <fieldset>

                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="icon-append fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtTerminationDate" runat="server" CssClass="form-control datepicker" placeholder="Termination Date"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="icon-append fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtLastDate" runat="server" CssClass="form-control datepicker" placeholder="Last Date at Work"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-flag fa-fw"></i></span>
                                                <asp:TextBox ID="txtReccomendation" runat="server" CssClass="form-control" placeholder="Reccomendation For Rehire"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-user fa-fw"></i></span>
                                                <asp:TextBox ID="txtTerminationReason" runat="server" CssClass="form-control" placeholder="Termination Reason"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </fieldset>
                            <div class="form-actions">
                                <div class="row">
                                    <div class="col-md-12">
                                        <button class="btn btn-default" type="submit">
                                            Cancel
                                        </button>
                                        <asp:Button ID="btnAddTerm" runat="server" CssClass="btn btn-primary" Text="Add Termination" ValidationGroup="Savedetail" OnClick="btnAddTerm_Click" />
                                        <button class="btn btn-default" type="submit">
                                            Generate PAF
                                        </button>
                                    </div>
                                </div>
                            </div>
                            <fieldset>
                                <legend></legend>
                            </fieldset>
                            <fieldset>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <h1>Terminations</h1>

                                        <fieldset>
                                            <asp:GridView ID="dgTermination" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                                DataKeyNames="Id" ForeColor="#333333"
                                                GridLines="Horizontal" CssClass="table table-striped table-bordered table-hover"
                                                PagerStyle-CssClass="paginate_button active" AlternatingRowStyle-CssClass=""
                                                Width="100%" Style="text-align: left"
                                                AllowPaging="True" PageSize="20">
                                                <Columns>
                                                    <asp:BoundField DataField="TerminationDate" HeaderText="Termination Date" />
                                                    <asp:BoundField DataField="LastDateofEmployee" HeaderText="Last Date of Employee at Office" />
                                                    <asp:BoundField DataField="ReccomendationForRehire" HeaderText="Reccomendation For Rehire" />
                                                    <asp:BoundField DataField="TerminationReason" HeaderText="Termination Reason" />



                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                            <div class="btn-group open">
                                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');"><i class="fa fa-times"></i></asp:LinkButton>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>

                                                <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </fieldset>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>
                <!-- end widget content -->

            </div>

            <!-- end widget div -->
        </div>
    </div>
</asp:Content>
