<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmLeaveApproverProReport.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Report.Views.frmLeaveApproverProReport"
    Title="Leave Report" MasterPageFile="~/Shared/ModuleMaster.master" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Leave Approval Progress Report</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                        </div>
                    </fieldset>
                    <footer>
                        
                    </footer>
                </div>
            </div>
        </div>
    </div>
    <asp:Panel ID="Panel1" runat="server" BackColor="White" Visible="true">

        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%"></rsweb:ReportViewer>
    </asp:Panel>
   
</asp:Content>
