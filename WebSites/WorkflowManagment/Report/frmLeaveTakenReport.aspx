<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmLeaveTakenReport.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Report.Views.frmLeaveTakenReport"
    Title="Leave Taken Report" MasterPageFile="~/Shared/ModuleMaster.master" %>




<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script lang="javascript" type="text/javascript">
    function CallPrint(strid) {
        var disp_setting = "toolbar=yes,location=no,directories=yes,menubar=yes,";
        disp_setting += "scrollbars=yes,width=650, height=600, left=100, top=25";
        var content_vlue = document.getElementById(strid).innerHTML;

        var docprint = window.open("", "", disp_setting);
        docprint.document.open();
        docprint.document.write('<html><head><title>Ethio ERP</title>');
        docprint.document.write('</head><body onLoad="self.print()"><center>');
        docprint.document.write(content_vlue);
        docprint.document.write('</center></body></html>');
        docprint.document.close();
        docprint.focus();
    
     
    }
   
    </script>
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Employee Leave Taken Report</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                         <section class="col col-6">
                                <label class="label">Employee Name</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlEmployeeName" runat="server" AppendDataBoundItems="True" DataTextField="FullName" DataValueField="FullName">
                                        <asp:ListItem Value=" ">Select Employee</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                </label>
                            </section>
                                  <section class="col col-3">
                                <asp:Label ID="lblSrchProgram" runat="server" Text="Program" CssClass="label"></asp:Label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlSrchSrchProgram" runat="server" AppendDataBoundItems="True" DataTextField="ProgramName" DataValueField="Id">
                                        <asp:ListItem Value="0">Select Program</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                </label>
                            </section>
                        </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnView" runat="server" Text="View" CssClass="btn btn-primary" OnClick="btnView_Click"></asp:Button>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" OnClientClick="CallPrint('divPrint')"></asp:Button>
                        

                    </footer>
                </div>
            </div>
        </div>
         <div id ="divPrint">
        <asp:GridView ID="GRVEmployeeList" runat="server" AutoGenerateColumns="False" CellPadding="3"
                DataKeyNames="Id" EnableModelValidation="True" ForeColor="#333333" GridLines="Horizontal"
                CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active"
                AlternatingRowStyle-CssClass="" OnRowDataBound="GRVEmployeeList_RowDataBound" Width="100%"
                Style="text-align: left" AllowPaging="True" OnPageIndexChanging="GRVEmployeeList_PageIndexChanging"
                Visible="True" PageSize="40">
                <%--    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />--%>
                <Columns>
                    <asp:BoundField DataField="FirstName" HeaderText="First Name" />    
                    <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                    <asp:BoundField HeaderText="Total Leave Taken" />
                    <asp:BoundField HeaderText="Leave Balance " />
                    <asp:BoundField HeaderText="Leave Balance as of CED" />
                    <asp:BoundField HeaderText="Leave Balance as of YE" />
                    
            
                    
                </Columns>
                <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
            </asp:GridView>
             </div>
    </div>
  
    
</asp:Content>
