﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmPurchaseOrder.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Approval.Views.frmPurchaseOrder" %>
<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MenuContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script type="text/javascript" language="javascript">
        function Clickheretoprint(theid) {
            var disp_setting = "toolbar=yes,location=no,directories=yes,menubar=yes,";
            disp_setting += "scrollbars=yes,width=750, height=600, left=100, top=25";
            var content_vlue = document.getElementById(theid).innerHTML;

            var docprint = window.open("", "", disp_setting);
            docprint.document.open();
            docprint.document.write('<html><head><title>CHAI Ethiopia</title>');
            docprint.document.write('</head><body onLoad="self.print()"><center>');
            docprint.document.write(content_vlue);
            docprint.document.write('</center></body></html>');
            docprint.document.close();
            docprint.focus();
        }
    </script>
    <asp:ValidationSummary ID="VSBid" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="Save" ForeColor="" />
    <div id="wid-id-0" class="jarviswidget" data-widget-custombutton="false" data-widget-editbutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Purchase Order</h2>
        </header>


        <!-- widget div-->
        <div>

            <!-- widget edit box -->
            <div class="jarviswidget-editbox">
                <!-- This area used as dropdown edit box -->

            </div>
            <!-- end widget edit box -->

            <!-- widget content -->
            <div class="smart-form">

              
                  <asp:Panel ID="pnlInfo" runat="server">
                <div class="alert alert-info fade in">
                    <button class="close" data-dismiss="alert">
                        ×
                    </button>
                    <i class="fa-fw fa fa-info"></i>
                    <strong>Info!</strong> Please select the Bid Analysis Request Transaction to perform Purchase Order
                </div>
            </asp:Panel>
             <asp:GridView ID="grvBidforPO" Visible="True"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover" AllowPaging="True"  PageSize="15">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                      
                    <%-- <asp:BoundField DataField="RequestNo" HeaderText="Bid Analysis Request No" SortExpression="RequestNo" />--%>
                    
                        <asp:BoundField DataField="Item" HeaderText="Item" SortExpression="Item" />
                     
                     <asp:BoundField DataField="Qty" HeaderText="Quantity" SortExpression="Qty" />
                       <asp:BoundField DataField="Supplier.SupplierNameType" HeaderText="Supplier" SortExpression="Supplier.SupplierNameType" />
                       <asp:BoundField DataField="Rank" HeaderText="Rank" SortExpression="Rank" />
                      <asp:BoundField DataField="TotalCost" HeaderText="Total Cost" SortExpression="TotalCost" />                     
                        <asp:BoundField DataField="ReasonForSelection" HeaderText="Reason For Selection" SortExpression="ReasonForSelection" />
                      <asp:TemplateField>
                         
                          <ItemTemplate>
                              <asp:CheckBox ID="chkSelect" runat="server" />
                          </ItemTemplate>
                      </asp:TemplateField>
                      
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
                  <footer>
           <asp:Button ID="btnCreatePO" runat="server" CssClass="btn btn-primary" Text="Create Purchase Order" OnClick="btnCreatePO_Click"/>
          </footer>
            </div>

              <asp:GridView ID="grvRankedBidders" Visible="True"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover" AllowPaging="True"  PageSize="15" OnSelectedIndexChanged="grvRankedBidders_SelectedIndexChanged">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                      
                    
                    <asp:BoundField DataField="Supplier.SupplierNameType" HeaderText="Supplier" SortExpression="Supplier.SupplierNameType" />
                     <asp:BoundField DataField="Rank" HeaderText="Rank" SortExpression="Rank" />
                     <asp:BoundField DataField="Qty" HeaderText="Quantity" SortExpression="Qty" />
                     <asp:BoundField DataField="UnitCost" HeaderText="Total Cost" SortExpression="UnitCost" />    
                     <asp:BoundField DataField="TotalCost" HeaderText="Total Cost" SortExpression="TotalCost" />                                        
                    <asp:BoundField DataField="ReasonForSelection" HeaderText="Reason For Selection" SortExpression="ReasonForSelection" />
                     
                      
                      <asp:TemplateField>
                         
                          <ItemTemplate>
                                 <asp:Button runat="server" CommandName="Select" Text="Prepare Purchase Order"></asp:Button>
                          </ItemTemplate>
                      </asp:TemplateField>
                      
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>

                        <div class="row">
                            <section class="col col-4">
                                <label class="label">
                                    Purchase Order No.</label>
                                <label class="input">
                                    <asp:TextBox ID="txtPONo" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-4">
                                <label class="label">
                                    Requester</label>
                                <label class="input">
                                    <asp:TextBox ID="txtRequester" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-4">
                                <label id="lblDate" runat="server" class="label" visible="true">
                                    Date</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtDate" runat="server" Visible="true" CssClass="form-control datepicker"></asp:TextBox>
                                </label>
                            </section>

                        </div>

                        <div class="row">
                            <section class="col col-6">
                                <label class="label">
                                    Bill to</label>
                                <label class="input">
                                    <asp:TextBox ID="txtBillto" runat="server" Visible="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfvBillTo" runat="server" CssClass="validator" ControlToValidate="txtBillto" ErrorMessage="Bill To Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label class="label">
                                    Ship To</label>
                                <label class="input">
                                    <asp:TextBox ID="txtShipTo" runat="server" Visible="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfvShipTo" runat="server" CssClass="validator" ControlToValidate="txtShipTo" ErrorMessage="Ship To Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                </label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">
                                    Delivery Fees</label>
                                <label class="input">
                                    <asp:TextBox ID="txtDeliveeryFees" runat="server" Visible="true"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtDeliveeryFees" ID="txtDeliveeryFees_FilteredTextBoxExtender" FilterType="Numbers,Custom" ValidChars="."></asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RfvtxtDeliveeryFees" runat="server" CssClass="validator" ControlToValidate="txtDeliveeryFees" ErrorMessage="Delivery Fees Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                                        </label>
                                                    </section>
                                                     <section class="col col-6">
                                                        <label class="label">
                                                       Payment Terms</label>
                                                        <label class="input">
                                                        <asp:TextBox ID="txtPaymentTerms" runat="server" Visible="true"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RfvPaymentTerms" runat="server" CssClass="validator" ControlToValidate="txtPaymentTerms" ErrorMessage="Payment Terms Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                                        </label>
                                                    </section>
                                                </div>
                                                <div class="row">
                                                 <section class="col col-4">
                                                        <label class="label">
                                                       Supplier Name</label>
                                                        <label class="input">
                                                        <asp:TextBox ID="txtSupplierName" runat="server" Enabled="true"></asp:TextBox>
                                                            
                                                        </label>
                                                    </section>
                                                    <section class="col col-4">
                                                        <label class="label">
                                                       Supplier Address</label>
                                                        <label class="input">
                                                        <asp:TextBox ID="txtSupplierAddress" runat="server" Enabled="true"></asp:TextBox>
                                                            
                                                        </label>
                                                    </section>
                                                  
                                                                                                      
                                                    
                                                </div>
                        <div class="row">
                                                 <section class="col col-4">
                                                        <label class="label">
                                                       Delivery Location:</label>
                                                        <label class="input">
                                                        <asp:TextBox ID="txtDelLoc" runat="server" Enabled="true"></asp:TextBox>
                                                            
                                                        </label>
                                                    </section>
                                                    <section class="col col-4">
                                                        <label class="label">
                                                       Delivery By</label>
                                                        <label class="input">
                                                        <asp:TextBox ID="txtDeliveryBy" runat="server" Enabled="true"></asp:TextBox>
                                                            
                                                        </label>
                                                    </section>
                                                  
                                                                                                      
                                                    
                                                </div>
                                                 <div class="row">
                                                 <section class="col col-4">
                                                        <label class="label">
                                                       Delivery Date:</label>
                                                       <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtDeliveryDate" runat="server" Visible="true" CssClass="form-control datepicker"></asp:TextBox>                            

                                                       </label>
                                                       
                                                         
                                                    </section>
                                                    
                                                  </div>
                                                                                                      
                                             
                                            </fieldset>  
                                            <asp:DataGrid ID="dgPODetail" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0" 
        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id" 
         GridLines="None" ShowFooter="True">
   
        <Columns>
          
            <asp:TemplateColumn HeaderText="Qty">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Qty")%>
                </ItemTemplate>
               
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Unit Cost">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "UnitCost")%>
                </ItemTemplate>
              
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Total Cost">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "TotalCost")%>
                </ItemTemplate>
               
            </asp:TemplateColumn>
               <asp:TemplateColumn HeaderText="Rank">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Rank")%>
                </ItemTemplate>
               
            </asp:TemplateColumn>
        </Columns>
           <PagerStyle  Cssclass="paginate_button active" HorizontalAlign="Center" />
                 </asp:DataGrid>
     


                                            <br />
                                            <footer>
                                                <asp:Button ID="btnRequest" runat="server" Cssclass="btn btn-primary" onclick="btnRequest_Click" Text="Save" ValidationGroup="Save" />
                                                &nbsp;<asp:Button ID="btnCancel" runat="server" Cssclass="btn btn-primary" onclick="btnCancel_Click" Text="Back"  />
												 <asp:Button ID="btnPrintPurchaseForm" runat="server" Cssclass="btn btn-primary"  Text="Print Purchase Order" OnClientClick="javascript:Clickheretoprint('printtran')" Enabled="False" />										
                                               <%-- <asp:Button ID="btnPrintPurchaseOrder" runat="server" Cssclass="btn btn-primary"  Text="Print Purchase Order" OnClientClick="javascript:Clickheretoprint('divprint')" Enabled="False" />--%>
																					
											</footer>
                                      
                                    </div></div>
									<!-- end widget content -->
				
								</div>
        
								<!-- end widget div -->
     
      <div id="printtran" style="display: none;">
            <fieldset>
                <table style="width: 100%;">
                    <tr>
                        <td style="font-size: large; text-align: center;">
                            <img src="../img/CHAI%20Logo.png" width="100" height="100" />
                            <br />
                            <strong>CHAI ETHIOPIA
                                <br />
                                PURCHASE ORDER</strong>
                        </td>
                    </tr>
                </table>

                <table style="width: 100%; border-style: solid;">
                    <tr>
                        <td style="width: 585px">Purchase Order No</td>
                        <td style="width: 617px">
                            <asp:Label ID="lblPurchaseOrderNo" runat="server"></asp:Label></td>
                        <td style="width: 25%;">Are the service being delivered by Supplier or picked up by CHAI?</td>
                        <td style="width: 25%;">
                            <asp:Label ID="lblDeliveryBy" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 25%;">PO Created Date:</td>
                        <td style="width: 25%;">
                            <asp:Label ID="lblPOCreatedDate" runat="server"></asp:Label>
                        </td>
                      <td style="width: 25%;">Delivery Location: </td>
                        <td style="width: 25%;">
                            <asp:Label ID="lblDeliverLocation" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 25%;">Payment Terms</td>
                        <td style="width: 25%;">
                            <asp:Label ID="lblPaymentTerms" runat="server"></asp:Label>
                        </td>
                        <td style="width: 25%;">Delivery Date: </td>
                        <td style="width: 25%;">
                            <asp:Label ID="lblDeliveryDate" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 25%;">Ship To</td>
                        <td style="width: 25%;">
                            <asp:Label ID="lblShipTo" runat="server"></asp:Label>
                        </td>
                      <td style="width: 25%;">Total Cost</td>
                        <td style="width: 25%;">
                            <asp:Label ID="lblTotCost" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 585px">Bill To</td>
                        <td style="width: 617px">
                            <asp:Label ID="lblBillToResult" runat="server"></asp:Label></td>
                        
                    </tr>
                    <tr>
                        <td style="width: 585px">Delivery Fees
                        </td>
                        <td style="width: 617px">
                            <asp:Label ID="lblDeliveryFees" runat="server"></asp:Label>
                        </td>
                      
                    </tr>
                </table>
                <br />
                <asp:GridView ID="grvDetails"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                   
                    
                    <asp:BoundField DataField="Rank" HeaderText="Rank" SortExpression="Rank" />
                    <asp:BoundField DataField="ItemDescription" HeaderText="Item" />
                    <asp:BoundField DataField="Qty" HeaderText="Qty" />
                    <asp:BoundField DataField="UnitCost" HeaderText="UnitCost" />
                    <asp:BoundField DataField="TotalCost" HeaderText="TotalCost" />
                    <asp:BoundField DataField="Vat" HeaderText="Vat" />
                    
                    <asp:BoundField DataField="Supplier.SupplierName" HeaderText="Supplier Name" />
                    <asp:BoundField DataField="Supplier.SupplierAddress" HeaderText="Supplier Address" />
                  
                    
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
                <br />
                <p style="text-align: left;">
                    <b>Supplier:  Please follow steps 1-4 in fulfilling this Purchase Order:</b><br />
                </p>
                <ol style="text-align: left;">
                    <li>By signing this Purchase Order or supplying Goods, you agree that the specifications of this Purchase Order and the attached Terms and Conditions apply to all Goods delivered under this Purchase Order and invoices pertaining thereto.</li>
                    <li>You agree to process and fill this order in accordance with the price and specifications above, the attached Terms and Conditions and any additional delivery information communicated to you from time-to-time.</li>
                    <li>Email all invoices to the e-mail address indicated above for approval and processing.  Send all other correspondence to the above local address.</li>
                    <li>Notify us immediately if you are unable to deliver as specified</li>
                </ol>

                <table style="width: 100%;">
                    <tr>
                        <td>Clinton Health Access Initiative</td>
                        <td style="width: 50%;"><asp:Label ID="lblSupp" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Signature:___________________</td>
                        <td style="width: 50%;">Signature:___________________</td>
                    </tr>
                    <tr>
                        <td>Date:</td>
                        <td style="width: 50%;">Date:</td>
                    </tr>
                </table>
            </fieldset>
        </div>
          
							</div>
 	 	 	 
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    	 
 	 	 	 	 	 
 	 	 	 	 	 

</asp:Content>

