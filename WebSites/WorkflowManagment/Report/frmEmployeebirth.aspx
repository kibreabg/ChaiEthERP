<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmEmployeebirth.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Report.Views.frmEmployeebirth"
    Title="Employee Birthdate Report" MasterPageFile="~/Shared/ModuleMaster.master" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Employee Birthdate Report</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Month</label>
                                <label class="select">
                        <asp:DropDownList ID="ddlmonth" runat="server">
                            <asp:ListItem Value=" ">Select Month</asp:ListItem>
                            <asp:ListItem>January</asp:ListItem>
                            <asp:ListItem>February</asp:ListItem>
                            <asp:ListItem>March</asp:ListItem>
                            <asp:ListItem>April</asp:ListItem>
                            <asp:ListItem>May</asp:ListItem>
                            <asp:ListItem>June</asp:ListItem>
                            <asp:ListItem>July</asp:ListItem>
                            <asp:ListItem>August</asp:ListItem>
                            <asp:ListItem>September</asp:ListItem>
                            <asp:ListItem>October</asp:ListItem>
                            <asp:ListItem>November</asp:ListItem>
                            <asp:ListItem>December</asp:ListItem>
                                </asp:DropDownList><i></i>
                                </label>
                            </section>
                            </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnView" runat="server" Text="View" CssClass="btn btn-primary" OnClick="btnView_Click"></asp:Button>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>


                    </footer>
                </div>
            </div>
        </div>
    </div>
    <asp:Panel ID="Panel1" runat="server" BackColor="White" Visible="true">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%"></rsweb:ReportViewer>
    </asp:Panel>
   
</asp:Content>
