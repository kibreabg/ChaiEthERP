<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmEmployeeProfile.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.HRM.Views.EmployeeProfile"
    Title="Employee Profile" MasterPageFile="~/Shared/ModuleMaster.master" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
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

            $('#datetimepicker1').datetimepicker({
                format: 'MM/DD/YYYY'
            });
            $('#datetimepicker2').datetimepicker({
                format: 'MM/DD/YYYY'
            });
            $('#datetimepicker3').datetimepicker({
                format: 'MM/DD/YYYY'
            });
            $('#datetimepicker4').datetimepicker({
                format: 'MM/DD/YYYY'
            });
            $('#datetimepicker5').datetimepicker({
                format: 'MM/DD/YYYY'
            });
            $('#datetimepicker6').datetimepicker({
                format: 'MM/DD/YYYY'
            });
        });

        function previewImage(divId, source) {
            var disp_setting = "toolbar=no, location=no, menubar=no,";
            disp_setting += "scrollbars=no, left=400, top=400";
            var content_value = document.getElementById(divId).innerHTML;
            var docprint = window.open("", "_blank", disp_setting);
            docprint.document.open();
            docprint.document.write('<html><head><title>CHAI Ethiopia ERP</title>');
            if (source == 'edu') {
                docprint.document.write('</head><body onload="movetoeducation();">');
            } else if (source == 'fam') {
                docprint.document.write('</head><body onload="movetofamily();">');
            }

            docprint.document.write(content_value);
            docprint.document.write('</body></html>');
            docprint.document.close();
            docprint.focus();
        }

        function newTab() {
            document.forms[0].target = "_blank";
        }

        function movetofamily() {
            $(document).ready(function () {
                var wizard = $('.wizard').wizard();

                wizard.wizard('selectedItem', {
                    step: 2
                });
            });
        }

        function movetoemergency() {
            $(document).ready(function () {
                var wizard = $('.wizard').wizard();

                wizard.wizard('selectedItem', {
                    step: 3
                });
            });
        }

        function movetoeducation() {
            $(document).ready(function () {
                var wizard = $('.wizard').wizard();

                wizard.wizard('selectedItem', {
                    step: 4
                });
            });
        }

        function movetowork() {
            $(document).ready(function () {
                var wizard = $('.wizard').wizard();

                wizard.wizard('selectedItem', {
                    step: 5
                });
            });
        }

    </script>

    <section id="widget-grid" class="">

        <!-- row -->
        <div class="row">

            <!-- NEW WIDGET START -->
            <article class="col-sm-12 sortable-grid ui-sortable">

                <!-- Widget ID (each widget will need unique ID)-->

                <!-- end widget -->

                <div class="jarviswidget jarviswidget-sortable" id="wid-id-2" data-widget-editbutton="false" data-widget-deletebutton="false" role="widget" style="">

                    <header role="heading">
                        <div class="jarviswidget-ctrls" role="menu"><a href="javascript:void(0);" class="button-icon jarviswidget-toggle-btn" rel="tooltip" title="" data-placement="bottom" data-original-title="Collapse"><i class="fa fa-minus "></i></a><a href="javascript:void(0);" class="button-icon jarviswidget-fullscreen-btn" rel="tooltip" title="" data-placement="bottom" data-original-title="Fullscreen"><i class="fa fa-expand "></i></a></div>
                        <div class="widget-toolbar" role="menu">
                            <a data-toggle="dropdown" class="dropdown-toggle color-box selector" href="javascript:void(0);"></a>
                            <ul class="dropdown-menu arrow-box-up-right color-select pull-right">
                                <li><span class="bg-color-green" data-widget-setstyle="jarviswidget-color-green" rel="tooltip" data-placement="left" data-original-title="Green Grass"></span></li>
                                <li><span class="bg-color-greenDark" data-widget-setstyle="jarviswidget-color-greenDark" rel="tooltip" data-placement="top" data-original-title="Dark Green"></span></li>
                                <li><span class="bg-color-greenLight" data-widget-setstyle="jarviswidget-color-greenLight" rel="tooltip" data-placement="top" data-original-title="Light Green"></span></li>
                                <li><span class="bg-color-purple" data-widget-setstyle="jarviswidget-color-purple" rel="tooltip" data-placement="top" data-original-title="Purple"></span></li>
                                <li><span class="bg-color-magenta" data-widget-setstyle="jarviswidget-color-magenta" rel="tooltip" data-placement="top" data-original-title="Magenta"></span></li>
                                <li><span class="bg-color-pink" data-widget-setstyle="jarviswidget-color-pink" rel="tooltip" data-placement="right" data-original-title="Pink"></span></li>
                                <li><span class="bg-color-pinkDark" data-widget-setstyle="jarviswidget-color-pinkDark" rel="tooltip" data-placement="left" data-original-title="Fade Pink"></span></li>
                                <li><span class="bg-color-blueLight" data-widget-setstyle="jarviswidget-color-blueLight" rel="tooltip" data-placement="top" data-original-title="Light Blue"></span></li>
                                <li><span class="bg-color-teal" data-widget-setstyle="jarviswidget-color-teal" rel="tooltip" data-placement="top" data-original-title="Teal"></span></li>
                                <li><span class="bg-color-blue" data-widget-setstyle="jarviswidget-color-blue" rel="tooltip" data-placement="top" data-original-title="Ocean Blue"></span></li>
                                <li><span class="bg-color-blueDark" data-widget-setstyle="jarviswidget-color-blueDark" rel="tooltip" data-placement="top" data-original-title="Night Sky"></span></li>
                                <li><span class="bg-color-darken" data-widget-setstyle="jarviswidget-color-darken" rel="tooltip" data-placement="right" data-original-title="Night"></span></li>
                                <li><span class="bg-color-yellow" data-widget-setstyle="jarviswidget-color-yellow" rel="tooltip" data-placement="left" data-original-title="Day Light"></span></li>
                                <li><span class="bg-color-orange" data-widget-setstyle="jarviswidget-color-orange" rel="tooltip" data-placement="bottom" data-original-title="Orange"></span></li>
                                <li><span class="bg-color-orangeDark" data-widget-setstyle="jarviswidget-color-orangeDark" rel="tooltip" data-placement="bottom" data-original-title="Dark Orange"></span></li>
                                <li><span class="bg-color-red" data-widget-setstyle="jarviswidget-color-red" rel="tooltip" data-placement="bottom" data-original-title="Red Rose"></span></li>
                                <li><span class="bg-color-redLight" data-widget-setstyle="jarviswidget-color-redLight" rel="tooltip" data-placement="bottom" data-original-title="Light Red"></span></li>
                                <li><span class="bg-color-white" data-widget-setstyle="jarviswidget-color-white" rel="tooltip" data-placement="right" data-original-title="Purity"></span></li>
                                <li><a href="javascript:void(0);" class="jarviswidget-remove-colors" data-widget-setstyle="" rel="tooltip" data-placement="bottom" data-original-title="Reset widget color to default">Remove</a></li>
                            </ul>
                        </div>
                        <h2>Employee Profile</h2>

                        <span class="jarviswidget-loader"><i class="fa fa-refresh fa-spin"></i></span>
                    </header>

                    <!-- widget div-->
                    <div role="content">

                        <!-- widget edit box -->
                        <div class="jarviswidget-editbox">
                            <!-- This area used as dropdown edit box -->

                        </div>
                        <!-- end widget edit box -->

                        <!-- widget content -->
                        <div class="widget-body fuelux">

                            <div class="wizard">
                                <ul class="steps" style="margin-left: 0">
                                    <li data-target="#step1" class="active">
                                        <span class="badge badge-info">1</span>Basic Information<span class="chevron"></span>
                                    </li>
                                    <li data-target="#step2" class="">
                                        <span class="badge">2</span>Family Detail<span class="chevron"></span>
                                    </li>
                                    <li data-target="#step3" class="">
                                        <span class="badge">3</span>Emergency Contact<span class="chevron"></span>
                                    </li>
                                    <li data-target="#step4" class="">
                                        <span class="badge">4</span>Education<span class="chevron"></span>
                                    </li>
                                    <li data-target="#step5" class="">
                                        <span class="badge">5</span>Work Experience<span class="chevron"></span>
                                    </li>
                                    <li data-target="#step6" class="">
                                        <span class="badge">6</span>Finish<span class="chevron"></span>
                                    </li>
                                </ul>
                                <div class="actions">
                                    <button type="button" class="btn btn-sm btn-primary btn-prev" disabled="disabled">
                                        <i class="fa fa-arrow-left"></i>Prev
                                    </button>
                                    <button type="button" class="btn btn-sm btn-success btn-next" data-last="Finish">Next<i class="fa fa-arrow-right"></i></button>
                                </div>
                            </div>
                            <div class="step-content">
                                <div class="step-pane active" id="step1">
                                    <h3><strong>Step 1 </strong>- Basic Information</h3>
                                    <!-- wizard form starts here -->
                                    <fieldset>
                                        <div class="row">
                                            <div style="padding-left: 13px; float: left; position: relative; width: 16.66666667%; height: 150px; border: 3px solid #fff;">
                                                <asp:Image ID="imgProfilePic" Width="100%" Height="100%" runat="server" />
                                                <asp:HiddenField ID="hfProfilePic" runat="server" />
                                                <div class="form-group">
                                                    <div class="smart-form">
                                                        <div class="input input-file">
                                                            <asp:FileUpload ID="fuProfilePic" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-user fa-fw"></i></span>
                                                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" placeholder="First Name"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-user fa-fw"></i></span>
                                                        <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" placeholder="Last Name"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-user fa-fw"></i></span>
                                                        <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                                                            <asp:ListItem Value="" Text="Select Gender"></asp:ListItem>
                                                            <asp:ListItem Value="Male" Text="Male"></asp:ListItem>
                                                            <asp:ListItem Value="Female" Text="Female"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group date" id='datetimepicker1'>

                                                        <asp:TextBox ID="txtDateOfBirth" runat="server" CssClass="form-control" data-dateformat="mm/dd/yy" placeholder="Date of Birth"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar fa-fw"></i></span>
                                                    </div>
                                                    <asp:RequiredFieldValidator
                                                        ID="rfvtxtDateOfBirth" runat="server" ErrorMessage="Date of Birth is required" Display="Dynamic"
                                                        CssClass="validator" ValidationGroup="saveBasicInfo" ControlToValidate="txtDateOfBirth"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-user fa-fw"></i></span>
                                                        <asp:DropDownList ID="ddlMaritalStatus" runat="server" CssClass="form-control">
                                                            <asp:ListItem Value="" Text="Select Marital Status"></asp:ListItem>
                                                            <asp:ListItem Value="Single" Text="Single"></asp:ListItem>
                                                            <asp:ListItem Value="Married" Text="Married"></asp:ListItem>
                                                            <asp:ListItem Value="Divorced" Text="Divorced"></asp:ListItem>
                                                            <asp:ListItem Value="Widowed" Text="Widowed"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-flag fa-fw"></i></span>
                                                        <asp:TextBox ID="txtNationality" runat="server" CssClass="form-control" placeholder="Nationality"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-2">
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-phone fa-fw"></i></span>
                                                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" data-mask="+(999) 999-99-99-99" data-mask-placeholder="X" placeholder="Phone"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-mobile fa-fw"></i></span>
                                                        <asp:TextBox ID="txtCellPhone" runat="server" CssClass="form-control" data-mask="+(999) 999-99-99-99" data-mask-placeholder="X" placeholder="Cell Phone"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-envelope fa-fw"></i></span>
                                                        <asp:TextBox ID="txtChaiEmail" runat="server" CssClass="form-control" placeholder="CHAI Email"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-envelope fa-fw"></i></span>
                                                        <asp:TextBox ID="txtPersonalEmail" runat="server" CssClass="form-control" placeholder="Personal Email"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-flag fa-fw"></i></span>
                                                        <asp:TextBox ID="txtCountry" runat="server" CssClass="form-control" placeholder="Country"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-map-marker fa-fw"></i></span>
                                                        <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" placeholder="City"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-map-marker fa-fw"></i></span>
                                                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Address"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </fieldset>
                                    <div class="form-actions">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:Button ID="btnCancel" runat="server" Text="Clear" OnClick="btnCancel_Click" CssClass="btn btn-default"></asp:Button>
                                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="saveBasicInfo" CssClass="btn btn-primary"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="step-pane" id="step2">
                                    <h3><strong>Step 2 </strong>- Family Details</h3>
                                    <fieldset>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-user fa-fw"></i></span>
                                                        <asp:TextBox ID="txtFamFirstName" runat="server" CssClass="form-control" placeholder="First Name"></asp:TextBox>
                                                    </div>
                                                    <asp:RequiredFieldValidator
                                                        ID="rfvFamFirstName" runat="server" ErrorMessage="First Name is Required" Display="Dynamic"
                                                        CssClass="validator" ValidationGroup="saveFam" ControlToValidate="txtFamFirstName"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-user fa-fw"></i></span>
                                                        <asp:TextBox ID="txtFamLastName" runat="server" CssClass="form-control" placeholder="Last Name"></asp:TextBox>
                                                    </div>
                                                    <asp:RequiredFieldValidator
                                                        ID="rfvFamLastName" runat="server" ErrorMessage="Last Name is Required" Display="Dynamic"
                                                        CssClass="validator" ValidationGroup="saveFam" ControlToValidate="txtFamLastName"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-user fa-fw"></i></span>
                                                        <asp:DropDownList ID="ddlFamGender" runat="server" CssClass="form-control">
                                                            <asp:ListItem Value="" Text="Select Gender"></asp:ListItem>
                                                            <asp:ListItem Value="Male" Text="Male"></asp:ListItem>
                                                            <asp:ListItem Value="Female" Text="Female"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <asp:RequiredFieldValidator
                                                        ID="rfvFamGender" runat="server" ErrorMessage="Gender is Required" Display="Dynamic"
                                                        CssClass="validator" ValidationGroup="saveFam" ControlToValidate="ddlFamGender"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-mobile fa-fw"></i></span>
                                                        <asp:DropDownList ID="ddlFamRelationship" AutoPostBack="true" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFamRelationship_SelectedIndexChanged">
                                                            <asp:ListItem Value="" Text="Select Relationship"></asp:ListItem>
                                                            <asp:ListItem Value="Child" Text="Child"></asp:ListItem>
                                                            <asp:ListItem Value="Spouse" Text="Spouse"></asp:ListItem>
                                                            <asp:ListItem Value="Parent" Text="Parent"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <asp:RequiredFieldValidator
                                                        ID="rfvFamRelationship" runat="server" ErrorMessage="Relationship is Required" Display="Dynamic"
                                                        CssClass="validator" ValidationGroup="saveFam" ControlToValidate="ddlFamRelationship"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <asp:Panel ID="pnlFamDateOfBirth" Visible="false" runat="server">
                                                    <div class="form-group">

                                                        <div class="input-group date" id='datetimepicker2'>
                                                            <asp:TextBox ID="txtFamDateOfBirth" runat="server" CssClass="form-control" data-dateformat="mm/dd/yy" placeholder="Date of Birth"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar fa-fw"></i></span>
                                                        </div>
                                                    </div>

                                                </asp:Panel>
                                            </div>
                                            <asp:Panel ID="pnlFamCertificate" Visible="false" runat="server">
                                                <label class="col-md-1 control-label">Attach Certificate</label>
                                                <div class="col-sm-5">
                                                    <div class="form-group">
                                                        <div class="smart-form">
                                                            <div class="input input-file">
                                                                <asp:FileUpload ID="fuCertificate" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <asp:Panel ID="pnlFamDateOfMarriage" Visible="false" runat="server">
                                                    <div class="form-group">
                                                        <div class="input-group date" id="datetimepicker6">

                                                            <asp:TextBox ID="txtFamDateOfMarriage" runat="server" CssClass="form-control" data-dateformat="mm/dd/yy" placeholder="Date of Marriage"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar fa-fw"></i></span>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-mobile fa-fw"></i></span>
                                                        <asp:TextBox ID="txtFamCellPhone" runat="server" CssClass="form-control" data-mask="+(999) 999-99-99-99" data-mask-placeholder="X" placeholder="Cell Phone"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </fieldset>
                                    <div id="certPreview" style="display: none;">
                                        <asp:Image ID="imgCertPreview" Width="100%" Height="100%" runat="server" />
                                    </div>
                                    <div class="form-actions">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:Button ID="btnFamCancel" runat="server" Text="Clear" OnClick="btnFamCancel_Click" CssClass="btn btn-default"></asp:Button>
                                                <asp:Button ID="btnFamDelete" runat="server" Text="Delete" OnClick="btnFamDelete_Click" OnClientClick="javascript:return confirm('Are you sure you want to delete this family information?');" Enabled="false" CssClass="btn btn-primary"></asp:Button>
                                                <asp:Button ID="btnFamSave" runat="server" Text="Save & Add New" CausesValidation="true" ValidationGroup="saveFam" OnClick="btnFamSave_Click" CssClass="btn btn-primary"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                    <fieldset>
                                        <legend></legend>
                                    </fieldset>
                                    <fieldset>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <h1>Family Members</h1>
                                                <asp:GridView ID="grvFamilyDetails"
                                                    runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnSelectedIndexChanged="grvFamilyDetails_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvFamilyDetails_PageIndexChanging"
                                                    CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active">
                                                    <RowStyle CssClass="rowstyle" />
                                                    <Columns>
                                                        <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" />
                                                        <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName" />
                                                        <asp:BoundField DataField="Gender" HeaderText="Gender" SortExpression="Gender" />
                                                        <asp:BoundField DataField="Relationship" HeaderText="Relationship" SortExpression="Relationship" />
                                                        <asp:TemplateField HeaderText="Certificate">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkFamDownload" Text="Preview" CommandArgument='<%# Eval("Certificate") %>' runat="server" OnClick="lnkFamDownload_Clicked"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:CommandField ShowSelectButton="True" SelectText="Edit" />
                                                    </Columns>
                                                    <FooterStyle CssClass="FooterStyle" />
                                                    <HeaderStyle CssClass="headerstyle" />
                                                    <PagerStyle CssClass="PagerStyle" />
                                                    <RowStyle CssClass="rowstyle" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>

                                <div class="step-pane" id="step3">
                                    <h3><strong>Step 3 </strong>- Emergency Contact</h3>
                                    <fieldset>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-user fa-fw"></i></span>
                                                        <asp:TextBox ID="txtEmergFullName" runat="server" CssClass="form-control" placeholder="Full Name"></asp:TextBox>
                                                    </div>
                                                    <asp:RequiredFieldValidator
                                                        ID="rfvEmergFullName" runat="server" ErrorMessage="Full Name is Required" Display="Dynamic"
                                                        CssClass="validator" ValidationGroup="saveEmerg" ControlToValidate="txtEmergFullName"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-mobile fa-fw"></i></span>
                                                        <asp:DropDownList ID="ddlEmergRelationship" runat="server" CssClass="form-control">
                                                            <asp:ListItem Value="" Text="Select Relationship"></asp:ListItem>
                                                            <asp:ListItem Value="Child" Text="Child"></asp:ListItem>
                                                            <asp:ListItem Value="Spouse" Text="Spouse"></asp:ListItem>
                                                            <asp:ListItem Value="Parent" Text="Parent"></asp:ListItem>
                                                            <asp:ListItem Value="Other" Text="Other"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <asp:RequiredFieldValidator
                                                        ID="rfvEmergRelationship" runat="server" ErrorMessage="Relationship is Required" Display="Dynamic"
                                                        CssClass="validator" ValidationGroup="saveEmerg" ControlToValidate="ddlEmergRelationship"></asp:RequiredFieldValidator>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-map-marker fa-fw"></i></span>
                                                        <asp:TextBox ID="txtEmergSubCity" runat="server" CssClass="form-control" placeholder="Sub-City"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-map-marker fa-fw"></i></span>
                                                        <asp:TextBox ID="txtEmergWoreda" runat="server" CssClass="form-control" placeholder="Woreda"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-map-marker fa-fw"></i></span>
                                                        <asp:TextBox ID="txtEmergHouseNo" runat="server" CssClass="form-control" placeholder="House No."></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-mobile fa-fw"></i></span>
                                                        <asp:TextBox ID="txtEmergCellPhone" runat="server" CssClass="form-control" data-mask="+(999) 999-99-99-99" data-mask-placeholder="X" placeholder="Cell Phone"></asp:TextBox>
                                                    </div>
                                                    <asp:RequiredFieldValidator
                                                        ID="rfvEmergCellPhone" runat="server" ErrorMessage="Cellphone is Required" Display="Dynamic"
                                                        CssClass="validator" ValidationGroup="saveEmerg" ControlToValidate="txtEmergCellPhone"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-phone fa-fw"></i></span>
                                                        <asp:TextBox ID="txtEmergTelephoneHome" runat="server" CssClass="form-control" data-mask="+(999) 999-99-99-99" data-mask-placeholder="X" placeholder="Telephone Home"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-phone fa-fw"></i></span>
                                                        <asp:TextBox ID="txtEmergTelephoneOffice" runat="server" CssClass="form-control" data-mask="+(999) 999-99-99-99" data-mask-placeholder="X" placeholder="Telephone Office"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="smart-form">
                                                    <div class="inline-group">
                                                        <label class="checkbox">
                                                            <asp:CheckBox ID="ckIsPrimary" runat="server" />
                                                            <i></i>Is Primary Contact</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </fieldset>
                                    <div class="form-actions">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:Button ID="btnEmergCancel" runat="server" Text="Clear" OnClick="btnEmergCancel_Click" CssClass="btn btn-default"></asp:Button>
                                                <asp:Button ID="btnEmergDelete" runat="server" Text="Delete" OnClick="btnEmergDelete_Click" OnClientClick="javascript:return confirm('Are you sure you want to delete this emergency contact?');" Enabled="false" CssClass="btn btn-primary"></asp:Button>
                                                <asp:Button ID="btnEmergSave" runat="server" Text="Save & Add New" CausesValidation="true" ValidationGroup="saveEmerg" OnClick="btnEmergSave_Click" CssClass="btn btn-primary"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                    <fieldset>
                                        <legend></legend>
                                    </fieldset>
                                    <fieldset>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <h1>Emergency Contacts</h1>
                                                <asp:GridView ID="grvEmergContacts"
                                                    runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnSelectedIndexChanged="grvEmergContacts_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvEmergContacts_PageIndexChanging"
                                                    CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active">
                                                    <RowStyle CssClass="rowstyle" />
                                                    <Columns>
                                                        <asp:BoundField DataField="FullName" HeaderText="Full Name" SortExpression="FullName" />
                                                        <asp:BoundField DataField="SubCity" HeaderText="Sub City" SortExpression="SubCity" />
                                                        <asp:BoundField DataField="Woreda" HeaderText="Woreda" SortExpression="Woreda" />
                                                        <asp:BoundField DataField="HouseNo" HeaderText="House No." SortExpression="HouseNo" />
                                                        <asp:BoundField DataField="CellPhone" HeaderText="Cell Phone" SortExpression="CellPhone" />
                                                        <asp:BoundField DataField="TelephoneOffice" HeaderText="Telephone Office" SortExpression="TelephoneOffice" />
                                                        <asp:CommandField ShowSelectButton="True" SelectText="Edit" />
                                                    </Columns>
                                                    <FooterStyle CssClass="FooterStyle" />
                                                    <HeaderStyle CssClass="headerstyle" />
                                                    <PagerStyle CssClass="PagerStyle" />
                                                    <RowStyle CssClass="rowstyle" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>

                                <div class="step-pane" id="step4">
                                    <h3><strong>Step 4 </strong>- Education</h3>
                                    <fieldset>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-institution fa-fw"></i></span>
                                                        <asp:DropDownList ID="ddlEduInstType" CssClass="form-control" runat="server">
                                                            <asp:ListItem Value="" Text="Select Institution Type"></asp:ListItem>
                                                            <asp:ListItem Value="Government" Text="Government"></asp:ListItem>
                                                            <asp:ListItem Value="Private" Text="Private"></asp:ListItem>
                                                            <asp:ListItem Value="International" Text="International"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-institution fa-fw"></i></span>
                                                        <asp:TextBox ID="txtEduInstName" runat="server" CssClass="form-control" placeholder="Institution Name"></asp:TextBox>
                                                    </div>
                                                    <asp:RequiredFieldValidator
                                                        ID="rfvEduInstName" runat="server" ErrorMessage="Institution name is Required" Display="Dynamic"
                                                        CssClass="validator" ValidationGroup="saveEdu" ControlToValidate="txtEduInstName"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-map-marker fa-fw"></i></span>
                                                        <asp:TextBox ID="txtEduInstLocation" runat="server" CssClass="form-control" placeholder="Institution Location"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-pencil-square-o fa-fw"></i></span>
                                                        <asp:TextBox ID="txtEduMajor" runat="server" CssClass="form-control" placeholder="Major"></asp:TextBox>
                                                    </div>
                                                    <asp:RequiredFieldValidator
                                                        ID="rfvEduMajor" runat="server" ErrorMessage="Major is Required" Display="Dynamic"
                                                        CssClass="validator" ValidationGroup="saveEdu" ControlToValidate="txtEduMajor"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-graduation-cap fa-fw"></i></span>
                                                        <asp:DropDownList ID="ddlEduLevel" CssClass="form-control" runat="server">
                                                            <asp:ListItem Value="" Text="Select Educational Level"></asp:ListItem>
                                                            <asp:ListItem Value="PHD" Text="PHD"></asp:ListItem>
                                                            <asp:ListItem Value="Masters" Text="Masters"></asp:ListItem>
                                                            <asp:ListItem Value="Doctor of Medicine" Text="Doctor of Medicine"></asp:ListItem>
                                                            <asp:ListItem Value="Post-Graduate Diploma" Text="Post-Graduate Diploma"></asp:ListItem>
                                                            <asp:ListItem Value="Bachelor" Text="Bachelor"></asp:ListItem>
                                                            <asp:ListItem Value="Advanced Diploma" Text="Advanced Diploma"></asp:ListItem>
                                                            <asp:ListItem Value="Diploma" Text="Diploma"></asp:ListItem>
                                                            <asp:ListItem Value="Certificate" Text="Certificate"></asp:ListItem>
                                                            <asp:ListItem Value="High-School Graduate" Text="High-School Graduate"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <asp:RequiredFieldValidator
                                                        ID="rfvEduLevel" runat="server" ErrorMessage="Educational level is required" Display="Dynamic"
                                                        CssClass="validator" ValidationGroup="saveEdu" ControlToValidate="ddlEduLevel"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group date" id='datetimepicker3'>
                                                        <asp:TextBox ID="txtEduGradYear" runat="server" CssClass="form-control" data-dateformat="mm/dd/yy" placeholder="Graduation Year"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar fa-fw"></i></span>
                                                    </div>
                                                    <asp:RequiredFieldValidator
                                                        ID="rfvtxtEduGradYear" runat="server" ErrorMessage="Graduation year is required" Display="Dynamic"
                                                        CssClass="validator" ValidationGroup="saveEdu" ControlToValidate="txtEduGradYear"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-trophy fa-fw"></i></span>
                                                        <asp:TextBox ID="txtEduSpecialAward" runat="server" CssClass="form-control" placeholder="Special Award"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <label class="col-md-1 control-label">Attach Certificate</label>
                                            <div class="col-sm-5">
                                                <div class="form-group">
                                                    <div class="smart-form">
                                                        <div class="input input-file">
                                                            <asp:FileUpload ID="fuEduCertificate" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </fieldset>
                                    <div class="form-actions">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:Button ID="btnEduCancel" runat="server" Text="Clear" OnClick="btnEduCancel_Click" CssClass="btn btn-default"></asp:Button>
                                                <asp:Button ID="btnEduDelete" runat="server" Text="Delete" OnClick="btnEduDelete_Click" OnClientClick="javascript:return confirm('Are you sure you want to delete this education information?');" Enabled="false" CssClass="btn btn-primary"></asp:Button>
                                                <asp:Button ID="btnEduSave" runat="server" Text="Save & Add New" CausesValidation="true" ValidationGroup="saveEdu" OnClick="btnEduSave_Click" CssClass="btn btn-primary"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                    <fieldset>
                                        <legend></legend>
                                    </fieldset>
                                    <fieldset>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <h1>Educational Background</h1>
                                                <asp:GridView ID="grvEducations"
                                                    runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnSelectedIndexChanged="grvEducations_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvEducations_PageIndexChanging"
                                                    CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active">
                                                    <RowStyle CssClass="rowstyle" />
                                                    <Columns>
                                                        <asp:BoundField DataField="InstitutionType" HeaderText="Institution Type" SortExpression="InstitutionType" />
                                                        <asp:BoundField DataField="InstitutionName" HeaderText="Institution Name" SortExpression="InstitutionName" />
                                                        <asp:BoundField DataField="InstitutionLocation" HeaderText="Institution Location" SortExpression="InstitutionLocation" />
                                                        <asp:BoundField DataField="Major" HeaderText="Major" SortExpression="Major" />
                                                        <asp:BoundField DataField="EducationalLevel" HeaderText="Educational Level" SortExpression="EducationalLevel" />
                                                        <asp:BoundField DataField="GraduationYear" HeaderText="Graduation Year" SortExpression="GraduationYear" />
                                                        <asp:TemplateField HeaderText="Certificate">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkEduDownload" Text="Preview" CommandArgument='<%# Eval("Certificate") %>' runat="server" OnClientClick="newTab();" OnClick="lnkEduDownload_Clicked"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:CommandField ShowSelectButton="True" SelectText="Edit" />
                                                    </Columns>
                                                    <FooterStyle CssClass="FooterStyle" />
                                                    <HeaderStyle CssClass="headerstyle" />
                                                    <PagerStyle CssClass="PagerStyle" />
                                                    <RowStyle CssClass="rowstyle" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>

                                <div class="step-pane" id="step5">
                                    <h3><strong>Step 5 </strong>- Work Experience</h3>
                                    <fieldset>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-institution fa-fw"></i></span>
                                                        <asp:TextBox ID="txtWorkOrgName" runat="server" CssClass="form-control" placeholder="Organization Name"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-map-marker fa-fw"></i></span>
                                                        <asp:TextBox ID="txtWorkOrgAddress" runat="server" CssClass="form-control" placeholder="Organization Address"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group date" id='datetimepicker4'>

                                                        <asp:TextBox ID="txtWorkStartDate" runat="server" CssClass="form-control " data-dateformat="mm/dd/yy" placeholder="Start Date"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar fa-fw"></i></span>
                                                    </div>
                                                    <asp:RequiredFieldValidator
                                                        ID="rfvWorkStartDate" runat="server" ErrorMessage="Start Date is required" Display="Dynamic"
                                                        CssClass="validator" ValidationGroup="workSave" ControlToValidate="txtWorkStartDate"></asp:RequiredFieldValidator>
                                                    <asp:CompareValidator ID="cvtxtWorkStartDate" CssClass="validator" runat="server" ErrorMessage="Work Start Date must be less than Work End Date" ControlToCompare="txtWorkEndDate" ControlToValidate="txtWorkStartDate" ValidationGroup="workSave" Type="Date" Operator="LessThanEqual"></asp:CompareValidator>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group date" id='datetimepicker5'>

                                                        <asp:TextBox ID="txtWorkEndDate" runat="server" CssClass="form-control" data-dateformat="mm/dd/yy" placeholder="End Date"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar fa-fw"></i></span>
                                                    </div>
                                                    <asp:RequiredFieldValidator
                                                        ID="rfvWorkEndDate" runat="server" ErrorMessage="End Date is required" Display="Dynamic"
                                                        CssClass="validator" ValidationGroup="workSave" ControlToValidate="txtWorkEndDate"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-briefcase fa-fw"></i></span>
                                                        <asp:TextBox ID="txtWorkJobTitle" runat="server" CssClass="form-control" placeholder="Job Title"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-building fa-fw"></i></span>
                                                        <asp:DropDownList ID="ddlWorkTypeOfEmp" runat="server" CssClass="form-control">
                                                            <asp:ListItem Value="" Text="Select Type of Employer"></asp:ListItem>
                                                            <asp:ListItem Value="NGO" Text="NGO"></asp:ListItem>
                                                            <asp:ListItem Value="Private" Text="Private"></asp:ListItem>
                                                            <asp:ListItem Value="Government" Text="Government"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </fieldset>
                                    <div class="form-actions">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:Button ID="btnWorkCancel" runat="server" Text="Clear" OnClick="btnWorkCancel_Click" CssClass="btn btn-default"></asp:Button>
                                                <asp:Button ID="btnWorkExpDelete" runat="server" Text="Delete" OnClick="btnWorkExpDelete_Click" OnClientClick="javascript:return confirm('Are you sure you want to delete this work experience?');" Enabled="false" CssClass="btn btn-primary"></asp:Button>
                                                <asp:Button ID="btnWorkSave" runat="server" Text="Save & Add New" OnClick="btnWorkSave_Click" CausesValidation="true" ValidationGroup="workSave" CssClass="btn btn-primary"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                    <fieldset>
                                        <legend></legend>
                                    </fieldset>
                                    <fieldset>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <h1>Work Experiences</h1>
                                                <asp:GridView ID="grvWorkExperiences"
                                                    runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnSelectedIndexChanged="grvWorkExperiences_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvWorkExperiences_PageIndexChanging"
                                                    CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active">
                                                    <RowStyle CssClass="rowstyle" />
                                                    <Columns>
                                                        <asp:BoundField DataField="EmployerName" HeaderText="Employee Name" SortExpression="EmployerName" />
                                                        <asp:BoundField DataField="EmployerAddress" HeaderText="Employee Address" SortExpression="EmployerAddress" />
                                                        <asp:BoundField DataField="StartDate" HeaderText="Start Date" SortExpression="StartDate" />
                                                        <asp:BoundField DataField="EndDate" HeaderText="End Date" SortExpression="EndDate" />
                                                        <asp:BoundField DataField="JobTitle" HeaderText="Job Title" SortExpression="JobTitle" />
                                                        <asp:CommandField ShowSelectButton="True" SelectText="Edit" />
                                                    </Columns>
                                                    <FooterStyle CssClass="FooterStyle" />
                                                    <HeaderStyle CssClass="headerstyle" />
                                                    <PagerStyle CssClass="PagerStyle" />
                                                    <RowStyle CssClass="rowstyle" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>
                                <div class="step-pane" id="step6">
                                    <h3><strong>Step 5 </strong>- Almost Done!</h3>
                                    <br>
                                    <br>
                                    <h1 class="text-center text-success"><i class="fa fa-check"></i>You have completed updating your profile! Please submit all required certificates to HR!
													<br>
                                        <small>Click Finish to end updating your profile!</small></h1>
                                    <br>
                                    <br>
                                    <br>
                                    <br>
                                </div>

                            </div>

                        </div>
                        <!-- end widget content -->

                    </div>
                    <!-- end widget div -->

                </div>
            </article>
            <!-- WIDGET END -->

        </div>
        <!-- end row -->

    </section>
</asp:Content>
