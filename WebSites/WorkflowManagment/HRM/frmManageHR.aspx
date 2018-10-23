<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmManageHR.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.HRM.Views.frmManageHR"
    Title="Manage HR" MasterPageFile="~/Shared/ModuleMaster.master" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
    <script type="text/javascript" language="javascript">
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
    </script>
    <div class="alert alert-info fade in">
        <button class="close" data-dismiss="alert">
            ×
        </button>
       <h3>
<span class="badge badge-info"></span>Employee Information<span class="chevron"></span></h3>
       
        
          <div class="row"> 
            <div class="form-group">
             <div  style="padding-left: 13px; float: left; position: relative; width: 16.66666667%; height: 150px; border: 3px solid #fff;">
                                                
                                                <asp:Image ID="imgProfilePic" Width="100%" Height="100%" runat="server" />
                                                <div class="form-group">
                                                    <div class="smart-form">
                                                       
                                                    </div>
                                                </div>
                                              
                                            </div>
                 </div>
             
                                                 <div style= "padding-left: 50px; position: center;">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                       
                                                      <strong> <asp:Label ID="txtFirstNamelbl" runat="server" Text="First Name:"></asp:Label></strong>
                                                      <asp:Label ID="txtFirstName" runat="server"></asp:Label>  
                                                    </div>
                                                </div>
                                          
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <strong>  <asp:Label ID="txtLastNamelbl" runat="server"  Text="Last Name:"></asp:Label></strong>
                                                       <asp:Label ID="txtLastName" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                          
                                          
                                                <div class="form-group">
                                                    <div class="input-group">
                                                       
                                                        <strong><asp:Label ID="lblempId" runat="server"  Text="Employee ID:"></asp:Label></strong>
                                                       <asp:Label ID="txtempid" runat="server"></asp:Label>
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
                            <a href="#tab-r1" data-toggle="tab"><span class="badge bg-color-blue txt-color-white"></span> Contract </a>
                        </li>
                      

                        <li class="">
                            <a href="#tab-r2" data-toggle="tab"><span class="badge bg-color-greenLight txt-color-white"></span> Exit Management</a>
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
                                             <asp:CompareValidator id="cvtxtStartDate" runat="server" ValidationGroup="Savecont" ControlToCompare="txtStartDate" cultureinvariantvalues="true" display="Dynamic" enableclientscript="true" ControlToValidate="txtEndDate" 
                                             ErrorMessage="End Date must be Greater than Start date" type="Date" setfocusonerror="true" Operator="GreaterThanEqual"  text="End Date must be Greater than Start date" ForeColor="Red"></asp:CompareValidator>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <div class="select">
                                            
                                            <asp:DropDownList ID="ddlReason" runat="server" CssClass="form-control" placeholder="Status" AppendDataBoundItems="True">
                                                <asp:ListItem Value=" ">Select Contract Type</asp:ListItem>
                                                <asp:ListItem Value="New Hire">New Hire</asp:ListItem>
                                                <asp:ListItem Value="Renewal">Renewal</asp:ListItem>
                                                <asp:ListItem Value="Rehire">Rehire</asp:ListItem>
                                            </asp:DropDownList>
                                             
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidatorContType" runat="server" Display="Dynamic" ValidationGroup="Savecont" ErrorMessage="Contract Type Required" InitialValue="0" ControlToValidate="ddlReason" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <div class="select">
                                           
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" placeholder="Status" AppendDataBoundItems="True">
                                                <asp:ListItem Value="0">Select Status</asp:ListItem>
                                                <asp:ListItem Value="Active">Active</asp:ListItem>
                                                <asp:ListItem Value="In Active">In Active</asp:ListItem>
                                            </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorStatus" Display="Dynamic" runat="server" ValidationGroup="Savecont" ErrorMessage="Status Required" InitialValue="0" ControlToValidate="ddlStatus" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="form-actions">
                                <div class="row">
                                    <div class="col-md-12">
                                        <button class="btn btn-default" type="submit">
                                            Cancel</button>
                                        <asp:Button ID="btnAddcontract" runat="server"  CssClass="btn btn-primary" Text="Add Contract" ValidationGroup="Savecont" OnClick="btnAddcontract_Click" />

                                    </div>
                                </div>
                            </div>


                            <div class="row">
                                <div class="col-sm-12">
                                    <h1>Employee Contracts</h1>
                                    <fieldset>
                                        <asp:GridView ID="dgContractDetail" OnRowCommand="dgContractDetail_RowCommand1" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                            DataKeyNames="Id" ForeColor="#333333"
                                            GridLines="Horizontal" CssClass="table table-striped table-bordered table-hover"
                                            PagerStyle-CssClass="paginate_button active" AlternatingRowStyle-CssClass=""
                                            Width="100%" Style="text-align: left"
                                            AllowPaging="True" PageSize="20" OnSelectedIndexChanged="dgContractDetail_SelectedIndexChanged" OnRowDeleting="dgContractDetail_RowDeleting" OnRowDataBound="dgContractDetail_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="ContractStartDate" HeaderText="Contract Start Date" />
                                                <asp:BoundField DataField="ContractEndDate" HeaderText="Contract End Date" />
                                                <asp:BoundField DataField="Reason" HeaderText="Reason" />
                                                <asp:BoundField DataField="Status" HeaderText="Status" />




                                                <asp:CommandField SelectText="Edit" ShowSelectButton="True" >
                                                <ItemStyle Font-Underline="True" ForeColor="#000099" />
                                                </asp:CommandField>
                                                 
                                                <asp:CommandField ShowDeleteButton="True" >
                                                 
                                                <ItemStyle ForeColor="#000099" />
                                                </asp:CommandField>
                                                 
                                                <asp:ButtonField CommandName="History" Text="History" >
                                                 
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

                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-append fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtLastDate" runat="server" CssClass="form-control datepicker" placeholder="Last Date at Work" data-dateformat="mm/dd/yy"></asp:TextBox>
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

                            <div class="row">
                                <div class="col-sm-12">
                                    <h1>Terminations</h1>

                                    <fieldset>
                                        <asp:GridView ID="dgTermination" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                            DataKeyNames="Id" ForeColor="#333333"
                                            GridLines="Horizontal" CssClass="table table-striped table-bordered table-hover"
                                            PagerStyle-CssClass="paginate_button active" AlternatingRowStyle-CssClass=""
                                            Width="100%" Style="text-align: left"
                                            AllowPaging="True" PageSize="20" OnSelectedIndexChanged="dgTermination_SelectedIndexChanged" OnRowDeleting="dgTermination_RowDeleting">
                                            <Columns>
                                                <asp:BoundField DataField="TerminationDate" HeaderText="Termination Date" />
                                                <asp:BoundField DataField="LastDateofEmployee" HeaderText="Last Date of Employee at Office" />
                                                <asp:BoundField DataField="ReccomendationForRehire" HeaderText="Reccomendation For Rehire" />
                                                <asp:BoundField DataField="TerminationReason" HeaderText="Termination Reason" />



                                                <asp:CommandField SelectText="Edit" ShowSelectButton="True" >
                                                <ItemStyle Font-Underline="True" ForeColor="#000099" />
                                                </asp:CommandField>
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');" Font-Underline="True" ForeColor="#000099" Text="Delete"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Underline="True" ForeColor="#000099" />
                                                </asp:TemplateField>

                                            </Columns>

                                            <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                        </asp:GridView>
                                    </fieldset>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    
                    <asp:Panel ID="pnlEMPHIST" Visible="true"  runat="server" style = "min-height:300px">
                      
                      
        <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false" style="overflow: auto;max-height:400px; width: 80%;" >
            <header>
                <span class="widget-icon"><i class="fa fa-edit"></i></span>
                <h2>Employee History</h2>
            </header>
                       

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
                                                <asp:ListItem Value="0">Select Duty Station</asp:ListItem>
                                                <asp:ListItem Value="Addis Ababa">Addis Ababa</asp:ListItem>
                                                <asp:ListItem Value="SNNPR">SNNPR</asp:ListItem>
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
                                                <asp:ListItem Value="0">Select Employement Status</asp:ListItem>
                                                <asp:ListItem Value="1">Fulltime</asp:ListItem>
                                                <asp:ListItem Value="2">Temporary</asp:ListItem>
                                                <asp:ListItem Value="3">Volunteer</asp:ListItem>
                                                <asp:ListItem Value="4">Intern</asp:ListItem>
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
                                                <asp:ListItem Value="0">Select Class</asp:ListItem>
                                                <asp:ListItem Value="1">Local National</asp:ListItem>
                                                <asp:ListItem Value="2">Expat</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorClass" runat="server" Display="Dynamic" ValidationGroup="Savedetail" ErrorMessage="Class Required" InitialValue="0" ControlToValidate="txtClass" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div></div>
                              <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon"></span>
                                            <asp:TextBox ID="txtHoursPerWeek" runat="server" CssClass="form-control" placeholder="Hours Per Week"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidatorhour" runat="server" ControlToValidate="txtHoursPerWeek" CssClass="validator" Display="Dynamic" ErrorMessage="Hours Per Week is required" SetFocusOnError="true" ValidationGroup="Savedetail" ForeColor="Red"></asp:RequiredFieldValidator>

                                        </div>
                                    </div>
                                </div>
                            
                          
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon"></span>
                                            <asp:TextBox ID="txtBaseCount" runat="server" CssClass="form-control" placeholder="Base Country"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidatorbasecount" runat="server" ControlToValidate="txtBaseCount" CssClass="validator" Display="Dynamic" ErrorMessage="Base Country is required" SetFocusOnError="true" ValidationGroup="Savedetail" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div></div>
                              <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon"></span>
                                            <asp:TextBox ID="txtBaseCity" runat="server" CssClass="form-control" placeholder="Base City"></asp:TextBox>
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
                                </div></div>
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
                                            <span class="input-group-addon"></span>
                                            <asp:TextBox ID="txtDescJT" runat="server" CssClass="form-control" placeholder="Descriptive Job Title"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidatorjt" runat="server" ControlToValidate="txtDescJT" CssClass="validator" Display="Dynamic" ErrorMessage="Descriptive Job Title is required" SetFocusOnError="true" ValidationGroup="Savedetail" ForeColor="Red"></asp:RequiredFieldValidator>

                                        </div>
                                    </div>
                                </div></div>
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
                            
               
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon"></span>
                                            <asp:DropDownList ID="ddlReportsTo" runat="server" CssClass="form-control" placeholder="Reports To" AppendDataBoundItems="True"></asp:DropDownList>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidatorRepto" runat="server" Display="Dynamic" ValidationGroup="Savedetail" ErrorMessage="Reports To Required" InitialValue="0" ControlToValidate="ddlReportsTo" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="form-actions">
                                <div class="row">
                                    <div class="col-md-12">
                                        <button class="btn btn-default" type="submit">
                                            Cancel</button>
                                        <asp:Button ID="btnAddChange" runat="server" CssClass="btn btn-primary" Text="Add Change" ValidationGroup="Savedetail" OnClick="btnAddChange_Click" />
                                        <asp:Button ID="btnPAFNew" runat="server" CssClass="btn btn-primary" Text="Generate PAF New Hire" OnClientClick="javascript:Clickheretoprint('divprint2')" Visible="False" OnClick="btnPAFNew_Click" />
                                        <asp:Button ID="btnPAFChange" runat="server" CssClass="btn btn-primary" Text="Generate PAF Change" OnClientClick="javascript:Clickheretoprint('divprint')" Visible="False" OnClick="btnPAFChange_Click" />
                                    </div>
                                </div>
                            </div>

                          
                       
          
            <div>
                  <div class="row">
                                <div class="col-sm-12">
                                    <h1>Employee History Detail</h1>
                                                                  





                                </div>
                            </div>
                <div class="jarviswidget-editbox"></div>
                <div class="widget-body no-padding">
                    <div class="smart-form">
                        <asp:GridView ID="dgChange" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                        DataKeyNames="Id" ForeColor="#333333"
                                        GridLines="Horizontal" CssClass="table table-striped table-bordered table-hover"
                                        PagerStyle-CssClass="paginate_button active" AlternatingRowStyle-CssClass=""
                                        Width="100%" Style="text-align: left"
                                        AllowPaging="True" PageSize="20" OnSelectedIndexChanged="dgChange_SelectedIndexChanged1" OnRowDeleting="dgChange_RowDeleting" OnRowCommand="dgChange_RowCommand" OnRowDataBound="dgChange_RowDataBound">
                                        <Columns>
                                            
                                            <asp:BoundField DataField="Position.PositionName" HeaderText="Position" />
                                            <asp:BoundField DataField="Program.ProgramName" HeaderText="Program" />
                                            <asp:BoundField DataField="DutyStation" HeaderText="DutyStation" />
                                            <asp:BoundField DataField="Salary" HeaderText="Salary" />


                                            <asp:CommandField SelectText="Edit" ShowSelectButton="True" >


                                            <ItemStyle ForeColor="#000099" />
                                            </asp:CommandField>
                                            <asp:CommandField ShowDeleteButton="True">
                                            <ItemStyle ForeColor="#0000CC" />
                                            </asp:CommandField>
                                        </Columns>
                                        <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                    </asp:GridView>
                        <footer>
                            <asp:Button ID="btnCancelCost" runat="server" Text="Close" class="btn btn-primary"></asp:Button>
                            <asp:Button ID="btnHiddenPopupp" runat="server" />
                        <asp:HiddenField ID="hfDetailId" runat="server" />
                        </footer>
                    </div>
                </div>
            </div>
                            
        </div>    
    </asp:Panel>

                </div>

                
                <cc1:modalpopupextender runat="server" enabled="True" cancelcontrolid="btnCancelCost"
                    id="pnlEMPHIST_ModalPopupExtender" targetcontrolid="btnHiddenPopupp" backgroundcssclass="PopupPageBackground" RepositionMode="RepositionOnWindowResize"   popupcontrolid="pnlEMPHIST">
    </cc1:modalpopupextender>



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
                                <td align="right" style="">&nbsp;</td>
                                <td align="right" style="width: 244px" class="inbox-data-from">&nbsp;</td>
                                <td align="right" style="width: 271px">&nbsp;</td>
                                <td align="right" style="width: 389px">&nbsp;</td>
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
    </div>
</asp:Content>
