<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmManageHR.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.HRM.Views.frmManageHR"
    Title="Manage HR" MasterPageFile="~/Shared/ModuleMaster.master" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script src="../js/libs/jquery-2.0.2.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            vv();
        });
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
        function Clickheretoprint(theid) {
            var disp_setting = "toolbar=yes,location=no,directories=yes,menubar=yes,";
            disp_setting += "scrollbars=yes,width=750, height=600, left=100, top=25";
            var content_vlue = document.getElementById(theid).innerHTML;

            var docprint = window.open("", "", disp_setting);
            docprint.document.open();
            docprint.document.write('<html><head><title>CHAI ETHIOPIA</title>');
            docprint.document.write('</head><body onLoad="self.print()"><center>');
            docprint.document.write(content_vlue);
            docprint.document.write('</center></body></html>');
            docprint.document.close();
            docprint.focus();
        }
        function showEmpHistoryModal() {
            $(document).ready(function () {
                $('#empHistory').modal('show');
            });
        }
        function onshowing() {
            $find('pnlEMPHIST_ModalPopupExtender').set_X(document.documentElement.clientWidth / 2);
            $find('pnlEMPHIST_ModalPopupExtender').set_Y(document.documentElement.clientHeight / 2);
        }
        function vv() {
            //for bootstrap 3 use 'shown.bs.tab' instead of 'shown' in the next line

            $('a[data-toggle="tab"]').on('shown.bs.tab', function () {
                //save the latest tab; use cookies if you like 'em better:
                localStorage.setItem('lastTab', $(this).attr('href'));
            });

            //go to the latest tab, if it exists:
            var lastTab = localStorage.getItem('lastTab');
            if (lastTab) {
                $('a[href=' + lastTab + ']').tab('show');
            }
            else {
                // Set the first tab if cookie do not exist
                $('a[data-toggle="tab"]:first').tab('show');
            }
        }
    </script>

    <div class="jarviswidget jarviswidget-color-blueDark" id="wid-id-x" data-widget-colorbutton="false" data-widget-editbutton="false" data-widget-togglebutton="false" data-widget-deletebutton="false" data-widget-fullscreenbutton="false" data-widget-custombutton="false" data-widget-sortable="false" role="widget" style="">
        <header role="heading">
            <span class="widget-icon"><i class="fa fa-align-justify"></i></span>
            <h2>Employee Information</h2>

            <span class="jarviswidget-loader"><i class="fa fa-refresh fa-spin"></i></span>
        </header>
        <div class="row" style="background-color: lightgrey;">
            <div class="col-sm-3">
                <div style="height: 200px; border: 3px solid #e6e9ec;">
                    <asp:Image ID="imgProfilePic" Width="100%" Height="100%" runat="server" />
                </div>
            </div>

            <div class="col-sm-9">
                <div class="row">
                    <div class="col-sm-6">
                        <h1>
                            <asp:Label ID="txtFullName" runat="server"></asp:Label>
                            <br>
                            <small>
                                <asp:Label ID="txtProgram" runat="server"></asp:Label>,
                                <asp:Label ID="txtPosition" runat="server"></asp:Label></small></h1>

                        <ul class="list-unstyled">
                            <li>
                                <p class="text-muted">
                                    <i class="fa fa-phone"></i>&nbsp;&nbsp;<span class="txt-color-darken"><asp:Label ID="txtPhoneNo" runat="server"></asp:Label></span>
                                </p>
                            </li>
                            <li>
                                <p class="text-muted">
                                    <i class="fa fa-envelope"></i>&nbsp;&nbsp;<a runat="server" id="lnkEmail"><asp:Label ID="txtEmail" runat="server"></asp:Label></a>
                                </p>
                            </li>
                            <li>
                                <p class="text-muted">
                                    <i class="fa fa-credit-card"></i>&nbsp;&nbsp;<span class="txt-color-darken"><asp:Label ID="txtEmpId" runat="server"></asp:Label></span>
                                </p>
                            </li>
                            <li>
                                <p class="text-muted">
                                    <i class="fa fa-calendar"></i>&nbsp;&nbsp;<span class="txt-color-darken"><asp:Label ID="txtHiredDate" runat="server"></asp:Label></span>
                                </p>
                            </li>
                        </ul>
                    </div>
                    <div class="col-sm-6">
                        <h1><small>Leave Information</small></h1>
                        <ul class="list-unstyled">
                            <li>
                                <p class="text-muted">
                                    <span class="txt-color-darken">Leave Balance as of Today</span> <span class="badge bg-color-red">
                                        <asp:Label ID="txtLeaveAsOfToday" runat="server"></asp:Label></span>
                                </p>
                            </li>
                            <li>
                                <p class="text-muted">
                                    <span class="txt-color-darken">Leave Balance as of Calendar End Date</span> <span class="badge bg-color-yellow">
                                        <asp:Label ID="txtLeaveAsOfCalEndDate" runat="server"></asp:Label></span>
                                </p>
                            </li>
                            <li>
                                <p class="text-muted">
                                    <span class="txt-color-darken">Leave Balance as of Contract End Date</span> <span class="badge bg-color-green">
                                        <asp:Label ID="txtLeaveAsOfContractEndDate" runat="server"></asp:Label></span>
                                </p>
                            </li>
                            <li>
                                <p class="text-muted">
                                    <span class="txt-color-darken">Total Leave Taken this Year</span> <span class="badge bg-color-green">
                                        <asp:Label ID="txttoalleavetaken" runat="server"></asp:Label></span>
                                </p>
                            </li>
                            <li>
                                <fieldset>
                                    <div class="form-group">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtthisdate" CssClass="form-control datepicker" placeholder="Leave Balance as of this date" data-dateformat="mm/dd/yy" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="input-group-append">
                                            <asp:Button ID="btnGet" runat="server" Text="Get" class="btn btn-outline-secondary" OnClick="btnGet_Click" />
                                        </div>
                                        <span class="badge bg-color-yellow">
                                            <asp:Label ID="lbllastdayleave" runat="server"></asp:Label></span>
                                    </div>
                                </fieldset>


                            </li>
                        </ul>
                    </div>

                </div>

            </div>
        </div>
    </div>

    <div class="jarviswidget" id="wid-id-5" data-widget-colorbutton="false" data-widget-editbutton="false" data-widget-fullscreenbutton="false" data-widget-custombutton="false" data-widget-sortable="false" role="widget" style="">
        <header role="heading">
            <div class="jarviswidget-ctrls" role="menu">
            </div>
            <h2>Manage HR</h2>

        </header>

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
                            <a href="#tab-r1" runat="server" data-toggle="tab"><span class="badge bg-color-blue txt-color-white"></span>Contract </a>
                        </li>

                        <li class="">
                            <a href="#tab-r2" runat="server" data-toggle="tab"><span class="badge bg-color-greenLight txt-color-white"></span>Exit Management</a>
                        </li>
                        <li class=""></li>
                        <li class=""></li>

                        <li class=""></li>
                        <li class=""></li>
                        <li class=""></li>
                        <li class=""></li>
                        <li class=""></li>
                        <li class=""></li>
                    </ul>
                </div>
                <div class="tab-content">
                    <div class="tab-pane active" id="tab-r1">
                        <fieldset>
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-append fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control datepicker" placeholder="Contract Start Date" data-dateformat="mm/dd/yy"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-append fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control datepicker" placeholder="Contract End Date" data-dateformat="mm/dd/yy"></asp:TextBox>
                                            <asp:CompareValidator ID="cvtxtStartDate" runat="server" ValidationGroup="Savecont" ControlToCompare="txtStartDate" CultureInvariantValues="true" Display="Dynamic" EnableClientScript="true" ControlToValidate="txtEndDate"
                                                ErrorMessage="End Date must be Greater than Start date" Type="Date" SetFocusOnError="true" Operator="GreaterThanEqual" Text="End Date must be Greater than Start date" ForeColor="Red"></asp:CompareValidator>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <div class="select">

                                            <asp:DropDownList ID="ddlReason" runat="server" CssClass="form-control" placeholder="Status" AppendDataBoundItems="True">
                                                <asp:ListItem Value="">Select Contract Type</asp:ListItem>
                                                <asp:ListItem Value="New Hire">New Hire</asp:ListItem>
                                                <asp:ListItem Value="Renewal">Renewal</asp:ListItem>
                                                <asp:ListItem Value="Rehire">Rehire</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorContType" runat="server" Display="Dynamic" ValidationGroup="Savecont" ErrorMessage="Contract Type Required" InitialValue="" ControlToValidate="ddlReason" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <div class="select">

                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" placeholder="Status" AppendDataBoundItems="True">
                                                <asp:ListItem Value="">Select Status</asp:ListItem>
                                                <asp:ListItem Value="Active">Active</asp:ListItem>
                                                <asp:ListItem Value="In Active">In Active</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorStatus" Display="Dynamic" runat="server" ValidationGroup="Savecont" ErrorMessage="Status Required" InitialValue="" ControlToValidate="ddlStatus" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions">
                                <div class="row">
                                    <div class="col-md-12">
                                        <button class="btn btn-default" type="submit">
                                            Cancel</button>
                                        <asp:Button ID="btnAddcontract" runat="server" CssClass="btn btn-primary" Text="Add Contract" ValidationGroup="Savecont" OnClick="btnAddcontract_Click" />
                                    </div>
                                </div>
                            </div>
                            <fieldset>
                                <legend></legend>
                            </fieldset>
                            <div class="row">
                                <div class="col-sm-12">
                                    <h2>Employee Contracts</h2>
                                    <fieldset>
                                        <asp:GridView ID="dgContractDetail" OnRowCommand="dgContractDetail_RowCommand1" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                            DataKeyNames="Id" ForeColor="#333333"
                                            GridLines="Horizontal" CssClass="table table-striped table-bordered table-hover"
                                            PagerStyle-CssClass="paginate_button active" AlternatingRowStyle-CssClass=""
                                            Width="100%" Style="text-align: left"
                                            AllowPaging="True" PageSize="20" OnSelectedIndexChanged="dgContractDetail_SelectedIndexChanged" OnRowDataBound="dgContractDetail_RowDataBound" OnRowDeleting="dgContractDetail_RowDeleting1">
                                            <Columns>
                                                <asp:BoundField DataField="ContractStartDate" HeaderText="Contract Start Date" />
                                                <asp:BoundField DataField="ContractEndDate" HeaderText="Contract End Date" />
                                                <asp:BoundField DataField="Reason" HeaderText="Reason" />
                                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                                <asp:CommandField SelectText="Edit" ShowSelectButton="True">
                                                    <ItemStyle Font-Underline="True" ForeColor="#000099" />
                                                </asp:CommandField>
                                                <asp:CommandField ShowDeleteButton="True">
                                                    <ItemStyle ForeColor="#000099" />
                                                </asp:CommandField>

                                                <asp:ButtonField CommandName="History" Text="History">

                                                    <ItemStyle ForeColor="#0000CC" />
                                                </asp:ButtonField>

                                            </Columns>
                                            <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                        </asp:GridView>

                                    </fieldset>
                                </div>
                            </div>
                        </fieldset>
                    </div>

                    <div class="tab-pane" id="tab-r2">
                        <fieldset>

                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-append fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtTerminationDate" runat="server" CssClass="form-control datepicker" placeholder="Termination Confirmation Date" data-dateformat="mm/dd/yy"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RfvTerminationDate" runat="server" ControlToValidate="txtTerminationDate" CssClass="validator" ErrorMessage="Termination Date Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Saveterm"></asp:RequiredFieldValidator>

                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-append fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtLastDate" runat="server" CssClass="form-control datepicker" placeholder="Last Date at Work" data-dateformat="mm/dd/yy"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvLastDate" runat="server" ControlToValidate="txtLastDate" CssClass="validator" ErrorMessage="Last Date Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Saveterm"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon"></span>

                                            <asp:DropDownList ID="ddlRecommendation" runat="server" CssClass="form-control" placeholder="Reccomendation For Rehire">
                                                <asp:ListItem Value="">Select Recommendation For Rehire</asp:ListItem>
                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                <asp:ListItem Value="No">No</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon"></span>
                                            <asp:TextBox ID="txtTerminationReason" runat="server" CssClass="form-control" placeholder="Termination Reason"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RfvTermReason" runat="server" ControlToValidate="txtTerminationReason" CssClass="validator" ErrorMessage="Termination reason Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Saveterm"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions">
                                <div class="row">
                                    <div class="col-md-12">
                                        <button class="btn btn-default" type="submit">
                                            Cancel</button>
                                        <asp:Button ID="btnAddTerm" runat="server" CssClass="btn btn-primary" Text="Add Termination" ValidationGroup="Saveterm" OnClick="btnAddTerm_Click" />
                                        <asp:Button ID="btnGenPAF" runat="server" CssClass="btn btn-primary" Text="Generate PAF" OnClientClick="javascript:Clickheretoprint('divprint1')" />
                                    </div>
                                </div>
                            </div>
                            <fieldset>
                                <legend></legend>
                            </fieldset>
                            <div class="row">
                                <div class="col-sm-12">
                                    <h2>Terminations</h2>
                                    <fieldset>
                                        <asp:GridView ID="dgTermination" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                            DataKeyNames="Id" ForeColor="#333333"
                                            GridLines="Horizontal" CssClass="table table-striped table-bordered table-hover"
                                            PagerStyle-CssClass="paginate_button active" AlternatingRowStyle-CssClass=""
                                            Width="100%" Style="text-align: left"
                                            AllowPaging="True" PageSize="20" OnSelectedIndexChanged="dgTermination_SelectedIndexChanged" OnRowDeleting="dgTermination_RowDeleting1" OnRowCommand="dgTermination_RowCommand">
                                            <Columns>
                                                <asp:BoundField DataField="TerminationDate" HeaderText="Termination Date" />
                                                <asp:BoundField DataField="LastDateofEmployee" HeaderText="Last Date of Employee at Office" />
                                                <asp:BoundField DataField="ReccomendationForRehire" HeaderText="Reccomendation For Rehire" />
                                                <asp:BoundField DataField="TerminationReason" HeaderText="Termination Reason" />
                                                <asp:CommandField SelectText="Edit" ShowSelectButton="True">
                                                    <ItemStyle Font-Underline="True" ForeColor="#3333FF" />
                                                </asp:CommandField>
                                                <asp:CommandField ShowDeleteButton="True">
                                                    <ItemStyle ForeColor="Blue" />
                                                </asp:CommandField>

                                            </Columns>

                                            <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                        </asp:GridView>
                                    </fieldset>
                                </div>
                            </div>
                        </fieldset>
                    </div>

                    <div class="modal fade" id="empHistory" tabindex="-1" role="dialog">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                                        &times;</button>
                                    <h4 class="modal-title">Employee Change History</h4>
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <span class="input-group-addon"></span>
                                                    <asp:DropDownList ID="ddlPosition" runat="server" CssClass="form-control" placeholder="Position" AppendDataBoundItems="True"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorPosition" runat="server" Display="Dynamic" ValidationGroup="Savedetail" ErrorMessage="Position Required" InitialValue="0" ControlToValidate="ddlPosition" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <span class="input-group-addon"></span>
                                                    <asp:DropDownList ID="ddlProgram" runat="server" CssClass="form-control" placeholder="Program" AppendDataBoundItems="True"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorProgram" runat="server" Display="Dynamic" ValidationGroup="Savedetail" ErrorMessage="Program Required" InitialValue="0" ControlToValidate="ddlProgram" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <span class="input-group-addon"></span>
                                                    <asp:DropDownList ID="ddlDutyStation" runat="server" CssClass="form-control" placeholder="Duty Station" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="Addis Ababa">Addis Ababa</asp:ListItem>
                                                        <asp:ListItem Value="SNNPR">SNNPR</asp:ListItem>
                                                        <asp:ListItem Value="Sidama">Sidama</asp:ListItem>
                                                        <asp:ListItem Value="Tigray">Tigray</asp:ListItem>
                                                        <asp:ListItem Value="Oromia">Oromia</asp:ListItem>
                                                        <asp:ListItem Value="Amhara">Amhara</asp:ListItem>
                                                        <asp:ListItem Value="Afar">Afar</asp:ListItem>
                                                        <asp:ListItem Value="Benshangul Gumuz">Benshangul Gumuz</asp:ListItem>
                                                        <asp:ListItem Value="Somali">Somali</asp:ListItem>
                                                        <asp:ListItem Value="Harari">Harari</asp:ListItem>
                                                        <asp:ListItem Value="Dire Dawa">Dire Dawa</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorDuty" runat="server" Display="Dynamic" ValidationGroup="Savedetail" ErrorMessage="Duty Station Required" InitialValue="0" ControlToValidate="ddlDutyStation" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <span class="input-group-addon"></span>
                                                    <asp:TextBox ID="txtSalary" runat="server" CssClass="form-control" placeholder="Salary"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="txtSalary_FilteredTextBoxExtender" runat="server" FilterType="Custom,Numbers" TargetControlID="txtSalary" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvtSal" runat="server" ControlToValidate="txtSalary" CssClass="validator" Display="Dynamic" ErrorMessage="Salary is required" SetFocusOnError="true" ValidationGroup="Savedetail" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <span class="input-group-addon"></span>
                                                    <asp:DropDownList ID="txtEmployeeStatus" runat="server" CssClass="form-control" placeholder="Employement Status">
                                                        <asp:ListItem Value="Fulltime">Fulltime</asp:ListItem>
                                                        <asp:ListItem Value="Temporary">Temporary</asp:ListItem>
                                                        <asp:ListItem Value="Volunteer">Volunteer</asp:ListItem>
                                                        <asp:ListItem Value="Intern">Intern</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmpSta" runat="server" Display="Dynamic" ValidationGroup="Savedetail" ErrorMessage="Employement Status Required" InitialValue="0" ControlToValidate="txtEmployeeStatus" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <span class="input-group-addon"></span>

                                                    <asp:DropDownList ID="txtClass" runat="server" CssClass="form-control" placeholder="Class">
                                                        <asp:ListItem Value="">Select Class</asp:ListItem>
                                                        <asp:ListItem Value="Local National">Local National</asp:ListItem>
                                                        <asp:ListItem Value="Expat">Expat</asp:ListItem>
                                                        <asp:ListItem Value="Secondee">Secondee</asp:ListItem>
                                                        <asp:ListItem Value="Volunteer">Volunteer</asp:ListItem>
                                                        <asp:ListItem Value="TCN">TCN</asp:ListItem>
                                                        <asp:ListItem Value="Independent Contractor">Independent Contractor</asp:ListItem>

                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorClass" runat="server" Display="Dynamic" ValidationGroup="Savedetail" ErrorMessage="Class Required" InitialValue="0" ControlToValidate="txtClass" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <span class="input-group-addon"></span>
                                                    <asp:TextBox ID="txtHoursPerWeek" runat="server" CssClass="form-control" placeholder="Hours Per Week">40</asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="txtHoursPerWeek_FilteredTextBoxExtender" runat="server" FilterType="Numbers" TargetControlID="txtHoursPerWeek" ValidChars="">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorhour" runat="server" ControlToValidate="txtHoursPerWeek" CssClass="validator" Display="Dynamic" ErrorMessage="Hours Per Week is required" SetFocusOnError="true" ValidationGroup="Savedetail" ForeColor="Red"></asp:RequiredFieldValidator>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <span class="input-group-addon"></span>
                                                    <asp:TextBox ID="txtBaseCount" runat="server" CssClass="form-control" placeholder="Base Country">Ethiopia</asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorbasecount" runat="server" ControlToValidate="txtBaseCount" CssClass="validator" Display="Dynamic" ErrorMessage="Base Country is required" SetFocusOnError="true" ValidationGroup="Savedetail" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <span class="input-group-addon"></span>
                                                    <asp:TextBox ID="txtBaseCity" runat="server" CssClass="form-control" placeholder="Base City">Addis Ababa</asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorcity" runat="server" ControlToValidate="txtBaseCity" CssClass="validator" Display="Dynamic" ErrorMessage="Base City is required" SetFocusOnError="true" ValidationGroup="Savedetail" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <span class="input-group-addon"></span>
                                                    <asp:TextBox ID="txtBaseState" runat="server" CssClass="form-control" placeholder="Base State"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorstate" runat="server" ControlToValidate="txtBaseState" CssClass="validator" Display="Dynamic" ErrorMessage="Base State is required" SetFocusOnError="true" ValidationGroup="Savedetail" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <span class="input-group-addon"></span>
                                                    <asp:TextBox ID="txtCountryTeam" runat="server" CssClass="form-control" placeholder="Country Team"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorcounteam" runat="server" ControlToValidate="txtCountryTeam" CssClass="validator" Display="Dynamic" ErrorMessage="Country Team is required" SetFocusOnError="true" ValidationGroup="Savedetail" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="icon-append fa fa-calendar"></i></span>
                                                    <asp:TextBox ID="txtEffectDate" runat="server" CssClass="form-control datepicker" placeholder="Change Effective Date"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="ceEffectiveDate" CssClass="MyCalendar" TargetControlID="txtEffectDate" runat="server"></cc1:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <span class="input-group-addon"></span>
                                                    <asp:DropDownList ID="ddlSuperVisor" runat="server" CssClass="form-control" placeholder="Supervisor" AppendDataBoundItems="True"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorsup" runat="server" Display="Dynamic" ValidationGroup="Savedetail" ErrorMessage="Supervisor Required" InitialValue="0" ControlToValidate="ddlSuperVisor" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-actions">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                                <asp:Button ID="btnAddChange" runat="server" CssClass="btn btn-primary" Text="Add Change" ValidationGroup="Savedetail" OnClick="btnAddChange_Click" />

                                                <asp:HiddenField ID="btnHiddenPopupp" runat="server" />
                                                <asp:HiddenField ID="hfDetailId" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="widget-body no-padding">
                                        <div class="smart-form">
                                            <asp:GridView ID="dgChange" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                                DataKeyNames="Id" ForeColor="#333333" GridLines="Horizontal" CssClass="table table-striped table-bordered table-hover"
                                                PagerStyle-CssClass="paginate_button active" AlternatingRowStyle-CssClass=""
                                                Width="100%" Style="text-align: left" AllowPaging="True" PageSize="20" OnRowCommand="dgChange_RowCommand"
                                                OnRowDataBound="dgChange_RowDataBound" OnRowDeleting="dgChange_RowDeleting1">
                                                <Columns>

                                                    <asp:BoundField DataField="Position.PositionName" HeaderText="Position" />
                                                    <asp:BoundField DataField="Program.ProgramName" HeaderText="Program" />
                                                    <asp:BoundField DataField="DutyStation" HeaderText="Duty Station" />
                                                    <asp:BoundField DataField="Salary" HeaderText="Salary" />
                                                    <asp:BoundField DataField="EffectiveDateOfChange" HeaderText="Effective Date of Change" />
                                                    <asp:CommandField SelectText="Edit" ShowSelectButton="True">
                                                        <ItemStyle ForeColor="#000099" />
                                                    </asp:CommandField>
                                                    <asp:CommandField ShowDeleteButton="True">
                                                        <ItemStyle ForeColor="#0000CC" />
                                                    </asp:CommandField>
                                                </Columns>
                                                <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- end widget content -->
            <div id="divprint" style="display: none;">
                <fieldset>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 17%; text-align: left;">
                                <img src="../img/CHAI%20Logo.png" width="70" height="50" /></td>
                            <td style="font-size: large; text-align: center;">
                                <strong>CHAI ETHIOPIA
                            <br />
                                    PERSONNEL ACTION FORM(PAF) CHANGE</strong></td>
                        </tr>
                    </table>

                    <table style="width: 100%;">
                        <tr>
                            <td style="">&nbsp;</td>
                            <td style="width: 244px" class="inbox-data-from">&nbsp;</td>
                            <td style="width: 271px">&nbsp;</td>
                            <td style="width: 389px">&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>

                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblEmployeeID" runat="server" Text="Employee ID:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblEmployeeIDResult" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px;">
                                <strong>
                                    <asp:Label ID="lblFirstName" runat="server" Text="First Name:"></asp:Label>
                                </strong>
                            </td>
                            <td style="width: 244px; height: 18px;">
                                <strong>
                                    <asp:Label ID="lblFirstNameResult" runat="server"></asp:Label>
                                </strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblMiddleName" runat="server" Text="Middle Name:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lbliddleNameResult" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblLastName" runat="server" Text="Last Name:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblLastNameResult" runat="server"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblEmailResult" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblEffectiveDate" runat="server" Text="Effective Date For Change:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblEffectiveDateRes" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblReason" runat="server" Text="Reason For Change:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblReasonRes" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>

                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblClass" runat="server" Text="Class:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblClassCurr" runat="server"></asp:Label>
                            </td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblClassChange" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblEmpStat" runat="server" Text="Employment Status:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblEmpStatCurr" runat="server"></asp:Label>
                            </td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblEmpStatChange" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblHrPerWeek" runat="server" Text="Class:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblHrPerWeekCurr" runat="server"></asp:Label>
                            </td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblHrPerWeekChage" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblDurationOfCont" runat="server" Text="Duration Of Contract :"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblStartDurcur" runat="server"></asp:Label>
                                <asp:Label ID="lblEndDurcur" runat="server"></asp:Label>
                            </td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblStartDurChange" runat="server"></asp:Label>
                                <asp:Label ID="lblEndDurChange" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblBasCountry" runat="server" Text="Base Country:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblBaseCountryCurr" runat="server"></asp:Label>
                            </td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblBaseCountryChange" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblBaseCity" runat="server" Text="Base City:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblBaseCityCurr" runat="server"></asp:Label>
                            </td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblBaseCityChange" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblBaseState" runat="server" Text="Base State:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblBaseStateCurr" runat="server"></asp:Label>
                            </td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblBaseStateChange" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblProgram" runat="server" Text="Program/Division:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblProgramCurr" runat="server"></asp:Label>
                            </td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblProgramChange" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblCountryTeam" runat="server" Text="Country Team (If Applicable):"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblCountryTeamCurr" runat="server"></asp:Label>
                            </td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblCountryTeamChange" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblOffJob" runat="server" Text="Official Job Title:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblOffJobCurr" runat="server"></asp:Label>
                            </td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblOffJobChange" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblDescJob" runat="server" Text="Descriptive Job Title:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblDescJobCurr" runat="server"></asp:Label>
                            </td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblDescJobChange" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblEmpMan" runat="server" Text="Employee Manager(First,Last Name):"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblEmpManCurr" runat="server"></asp:Label>
                            </td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblEmpManChange" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblReportto" runat="server" Text="Employee Direct Reports To(If applicable):"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblReporttoCurr" runat="server"></asp:Label>
                            </td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblReporttoChange" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>

                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblAnnualBaseSalary" runat="server" Text="Annual Base Salary:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblAnnualBaseSalaryCurr" runat="server"></asp:Label>
                            </td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblAnnualBaseSalaryChange" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblAddCompensation" runat="server" Text="Additional/Other Compensation(please explain in notes):"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblAddCompensationCurr" runat="server"></asp:Label>
                            </td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblAddCompensationChange" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblPayrollAdmin" runat="server" Text="Payroll Administered By:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblPayrollAdminCurr" runat="server"></asp:Label>
                            </td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblPayrollAdminChange" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>

                </fieldset>
            </div>

            <div id="divprint2" style="display: none;">
                <fieldset>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 17%; text-align: left;">
                                <img src="../img/CHAI%20Logo.png" width="70" height="50" /></td>
                            <td style="font-size: large; text-align: center;">
                                <strong>CHAI ETHIOPIA
                            <br />
                                    PERSONNEL ACTION FORM(PAF) New Hire</strong></td>
                        </tr>
                    </table>

                    <table style="width: 100%;">
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
                                    <asp:Label ID="lblEmployeeIDnew" runat="server" Text="Employee ID:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblEmployeeIDResultnew" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 271px; height: 18px;">
                                <strong>
                                    <asp:Label ID="lblFirstNamenew" runat="server" Text="First Name:"></asp:Label>
                                </strong>
                            </td>
                            <td style="width: 271px; height: 18px;">
                                <strong>
                                    <asp:Label ID="lblFirstNameResultnew" runat="server"></asp:Label>
                                </strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblMiddleNamenew" runat="server" Text="Middle Name:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lbliddleNameResultnew" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblLastNamenew" runat="server" Text="Last Name:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblLastNameResultnew" runat="server"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblEmailnew" runat="server" Text="Email:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblEmailResultnew" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblEffectiveDatenew" runat="server" Text="Effective Date For Change:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblEffectiveDateResnew" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblReasonnew" runat="server" Text="Reason For Change:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblReasonResnew" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>

                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblClassnew" runat="server" Text="Class:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblClassres" runat="server"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblEmpStatnew" runat="server" Text="Employment Status:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblEmpStatRes" runat="server"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblHrPerWeeknew" runat="server" Text="Class:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblHrPerWeekRes" runat="server"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblDurationOfContnew" runat="server" Text="Duration Of Contract :"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblStartDurnew" runat="server"></asp:Label>
                                <asp:Label ID="lblEndDurnew" runat="server"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblBasCountrynew" runat="server" Text="Base Country:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblBaseCountryres" runat="server"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblBaseCitynew" runat="server" Text="Base City:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblBaseCityres" runat="server"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblBaseStatenew" runat="server" Text="Base State:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblBaseStateres" runat="server"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblProgramnew" runat="server" Text="Program/Division:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblProgramres" runat="server"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblCountryTeamnew" runat="server" Text="Country Team (If Applicable):"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblCountryTeamres" runat="server"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblOffJobnew" runat="server" Text="Official Job Title:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblOffJobres" runat="server"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblDescJobnew" runat="server" Text="Descriptive Job Title:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblDescJobres" runat="server"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblEmpMannew" runat="server" Text="Employee Manager(First,Last Name):"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblEmpManres" runat="server"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblReporttonew" runat="server" Text="Employee Direct Reports To(If applicable):"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblReporttores" runat="server"></asp:Label>
                            </td>

                        </tr>
                    </table>
                    <table>

                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblAnnualBaseSalarynew" runat="server" Text="Annual Base Salary:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblAnnualBaseSalaryres" runat="server"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblAddCompensationnew" runat="server" Text="Additional/Other Compensation(please explain in notes):"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblAddCompensationres" runat="server"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblPayrollAdminnew" runat="server" Text="Payroll Administered By:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblPayrollAdminres" runat="server"></asp:Label>
                            </td>

                        </tr>
                    </table>

                </fieldset>
            </div>

            <div id="divprint1" style="display: none;">
                <fieldset>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 17%; text-align: left;">
                                <img src="../img/CHAI%20Logo.png" width="70" height="50" /></td>
                            <td style="font-size: large; text-align: center;">
                                <strong>CHAI ETHIOPIA
                            <br />
                                    PERSONNEL ACTION FORM(PAF) Termination</strong></td>
                        </tr>
                    </table>

                    <table style="width: 100%;">
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
                                    <asp:Label ID="lblEmployeeIDTer" runat="server" Text="Voucher No:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblEmployeeIDResultTer" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 271px; height: 18px;">
                                <strong>
                                    <asp:Label ID="lblFirstNameTer" runat="server" Text="First Name:"></asp:Label>
                                </strong>
                            </td>
                            <td style="width: 271px; height: 18px;">
                                <strong>
                                    <asp:Label ID="lblFirstNameResultTer" runat="server"></asp:Label>
                                </strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblMiddleNameTer" runat="server" Text="Middle Name:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lbliddleNameResultTer" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblLastNameTer" runat="server" Text="Last Name:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblLastNameResultTer" runat="server"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblEmailTer" runat="server" Text="Email:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblEmailResultTer" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblBasCountryTer" runat="server" Text="Class:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblBasCountryResTer" runat="server"></asp:Label>
                            </td>

                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblClassTer" runat="server" Text="Class:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblClassResTer" runat="server"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblReasonTer" runat="server" Text="Reason For Termination:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblReasonResTer" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>


                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lbllistDirectRepoto" runat="server" Text="List Direct Reports, if applicable:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lbllistDirectRepotoRes" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lbllastDay" runat="server" Text="Last Day of Employment:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lbllastDayRes" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblRecommRehire" runat="server" Text="Recommendation for Rehire :"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblRecommRehireRes" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 629px; height: 18px; padding-left: 20%;">
                                <strong>
                                    <asp:Label ID="lblWhoManage" runat="server" Text="Who Manages Direct Reports Upon Exit?:"></asp:Label>
                                </strong></td>
                            <td style="width: 244px" class="inbox-data-from">
                                <asp:Label ID="lblWhoManageRes" runat="server"></asp:Label>
                            </td>
                        </tr>



                    </table>

                </fieldset>
            </div>
        </div>

        <!-- end widget div -->
    </div>

</asp:Content>
