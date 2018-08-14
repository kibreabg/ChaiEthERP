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
        });
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
                                <form class="form-horizontal" id="fuelux-wizard" method="post">

                                    <div class="step-pane active" id="step1">
                                        <h3><strong>Step 1 </strong>- Basic Information</h3>

                                        <!-- wizard form starts here -->
                                        <fieldset>

                                            <div class="row">
                                                <div class="col-sm-6">
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
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <div class="input-group">
                                                            <span class="input-group-addon"><i class="fa fa-user fa-fw"></i></span>
                                                            <asp:TextBox ID="txtGender" runat="server" CssClass="form-control" placeholder="Gender"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <div class="input-group">
                                                            <span class="input-group-addon"><i class="fa fa-calendar fa-fw"></i></span>
                                                            <asp:TextBox ID="txtDateOfBirth" runat="server" CssClass="form-control" placeholder="Date of Birth"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <div class="input-group">
                                                            <span class="input-group-addon"><i class="fa fa-user fa-fw"></i></span>
                                                            <asp:TextBox ID="txtMaritalStatus" runat="server" CssClass="form-control" placeholder="Marital Status"></asp:TextBox>
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
                                                <div class="col-sm-6">
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
                                                            <asp:TextBox ID="txtCellPhone" runat="server" CssClass="form-control" placeholder="Cell Phone"></asp:TextBox>
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
                                                            <span class="input-group-addon"><i class="fa fa-user fa-fw"></i></span>
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
                                                    <button class="btn btn-default" type="submit">
                                                        Cancel
                                                    </button>
                                                    <button class="btn btn-primary" type="submit">
                                                        <i class="fa fa-save"></i>
                                                        Submit
                                                    </button>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="step-pane" id="step2">
                                        <h3><strong>Step 2 </strong>- Alerts</h3>

                                        <div class="alert alert-warning fade in">
                                            <button class="close" data-dismiss="alert">
                                                ×
                                            </button>
                                            <i class="fa-fw fa fa-warning"></i>
                                            <strong>Warning</strong> Your monthly traffic is reaching limit.
                                        </div>

                                        <div class="alert alert-success fade in">
                                            <button class="close" data-dismiss="alert">
                                                ×
                                            </button>
                                            <i class="fa-fw fa fa-check"></i>
                                            <strong>Success</strong> The page has been added.
                                        </div>

                                        <div class="alert alert-info fade in">
                                            <button class="close" data-dismiss="alert">
                                                ×
                                            </button>
                                            <i class="fa-fw fa fa-info"></i>
                                            <strong>Info!</strong> You have 198 unread messages.
                                        </div>

                                        <div class="alert alert-danger fade in">
                                            <button class="close" data-dismiss="alert">
                                                ×
                                            </button>
                                            <i class="fa-fw fa fa-times"></i>
                                            <strong>Error!</strong> The daily cronjob has failed.
                                        </div>

                                    </div>

                                    <div class="step-pane" id="step3">
                                        <h3><strong>Step 3 </strong>- Wizard continued</h3>
                                        <br>
                                        <br>
                                        <h1 class="text-center text-primary">This will be your Step 3 </h1>
                                        <br>
                                        <br>
                                        <br>
                                        <br>
                                    </div>

                                    <div class="step-pane" id="step4">
                                        <h3><strong>Step 4 </strong>- Wizard continued...</h3>
                                        <br>
                                        <br>
                                        <h1 class="text-center text-danger">This will be your Step 4 </h1>
                                        <br>
                                        <br>
                                        <br>
                                        <br>
                                    </div>

                                    <div class="step-pane" id="step5">
                                        <h3><strong>Step 5 </strong>- Work Experience</h3>
                                        <br>
                                        <br>
                                        <h1 class="text-center text-success"><i class="fa fa-check"></i>Congratulations!
													<br>
                                            <small>Click finish to end wizard</small></h1>
                                        <br>
                                        <br>
                                        <br>
                                        <br>
                                    </div>
                                    <div class="step-pane" id="step6">
                                        <h3><strong>Step 5 </strong>- Finished!</h3>
                                        <br>
                                        <br>
                                        <h1 class="text-center text-success"><i class="fa fa-check"></i>Congratulations!
													<br>
                                            <small>Click finish to end wizard</small></h1>
                                        <br>
                                        <br>
                                        <br>
                                        <br>
                                    </div>

                                </form>
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
