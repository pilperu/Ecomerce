<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_PaymentWizard" Codebehind="PaymentWizard.aspx.cs" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Payment Setup Wizard</h1>
    <uc1:MessageBox ID="MessageBox1" runat="server" />
    <asp:MultiView ID="multiview1" ActiveViewIndex="0" runat="server">
        <asp:View ID="step1" runat="server">
            <h3>Step 1 of 4</h3>
            &nbsp;                                   
                    <div class="controlarea2" style="padding:10px;"><asp:LinkButton ID="lnkWithPayPal" 
                            runat="server" onclick="lnkWithPayPal_Click">
                        <strong>Accept Credit Cards + PayPal</strong><br />
                        <img src="../Images/PaymentWithPayPal.png" alt="Credit Cards + PayPal" border="0" />
                        </asp:LinkButton></div>
                        &nbsp;<br />
                        <div class="controlarea2" style="padding:10px;">
                        <asp:LinkButton ID="lnkNoPayPal" runat="server" onclick="lnkNoPayPal_Click">
                        <strong>Accept Credit Cards Only</strong><br />
                        <img src="../Images/PaymentNoPayPal.png" alt="Credit Cards Only" border="0" />
                        </asp:LinkButton></div>
                
                       
            <p>* Other payment options will be available in a moment.</p>        
        </asp:View>
        <asp:View ID="step2" runat="server">
            <h3>Step 2 of 4</h3>
            &nbsp;
          
             <table border="0" cellspacing="0" cellpadding="0" width="500">
            <tr>
                <td width="46%">
                    <div class="controlarea2" style="padding:10px;height:250px;">
                        <strong>PayPal Website Payments Standard</strong><br />
                        <em>Easy to get started. No Monthly Fees<br /><a href="#">Quick Demo</a></em><br />&nbsp;
                        <ul>
                            <li>Description here</li>
                            <li>Description here</li>
                            <li>Description here</li>
                        </ul>
                        Pricing
                        <ul>
                            <li>Description here</li>
                            <li>Description here</li>
                            <li>Description here</li>
                        </ul><div style="text-align:right">
                        <asp:Button ID="btnPayPalStandard" runat="server" Text="Select Standard" 
                                onclick="btnPayPalStandard_Click" /></div>
                    </div>
                </td>
                <td>&nbsp;</td>
                <td width="46%">
                    <div class="controlarea2" style="padding:10px;height:250px;">
                        <strong>PayPal Website Payments Pro</strong><br />
                        <em>Advanced e-commerce solution for established businesses<br /><a href="#">Quick Demo</a></em>                        
                        <ul>
                            <li>Description here</li>
                            <li>Description here</li>
                            <li>Description here</li>
                        </ul>
                        Pricing
                        <ul>
                            <li>Description here</li>
                            <li>Description here</li>
                            <li>Description here</li>
                        </ul><div style="text-align:right">
                        <asp:Button ID="btnPayPayPro" runat="server" Text="Select Pro" 
                                onclick="btnPayPayPro_Click" /></div>
                    </div>
                </td>
            </tr>
            </table>
            &nbsp;<br />
            <asp:LinkButton ID="lnkBackFrom2a" runat="server" onclick="lnkBackFrom2a_Click">&laquo; Back</asp:LinkButton>
        </asp:View>
        <asp:View ID="step2b" runat="server">
            <h3>Step 2 of 4</h3>
            &nbsp;
            <div class="controlarea2" style="padding:10px;">
                <strong>Select a Credit Card Gateway</strong>
                <asp:RadioButtonList runat="server" ID="lstGateways">
                    <asp:ListItem>Authorize.Net</asp:ListItem>
                    <asp:ListItem>LinkPoint</asp:ListItem>
                    <asp:ListItem>PayPal PayFlow Pro</asp:ListItem>
                    <asp:ListItem>PayPal Website Payments Standard</asp:ListItem>
                    <asp:ListItem>PayPal Website Payments Pro</asp:ListItem>
                    <asp:ListItem>WorldPay</asp:ListItem>                    
                </asp:RadioButtonList>                                
                <div style="text-align:right;">
                    <asp:Button ID="btnSaveGateway" runat="server" Text="Next >>" 
                        onclick="btnSaveGateway_Click" />
                    </div>
            </div>
            &nbsp;<br />
            <asp:LinkButton ID="lnkBackFrom2" runat="server" onclick="lnkBackFrom2_Click">&laquo; Back</asp:LinkButton>
        </asp:View>
         <asp:View ID="step3a" runat="server">
            <h3>Step 3 of 4</h3>
            &nbsp;
             <div class="controlarea2" style="padding:10px;">
                PAY PAL CONFIG GOES HERE
                <div style="text-align:right;">
                    <asp:Button ID="btnPayPalConfig" runat="server" Text="Next >>" 
                        onclick="btnPayPalConfig_Click" />
                    </div>
            </div>            
            &nbsp;<br />
            <asp:LinkButton ID="lnkBackFrom3a" runat="server" onclick="lnkBackFrom3a_Click">&laquo; Back</asp:LinkButton>
        </asp:View>
        <asp:View ID="step3b" runat="server">
            <h3>Step 3 of 4</h3>
            &nbsp;
            <div class="controlarea2" style="padding:10px;">
                CREDIT CARD CONFIG GOES HERE
                <div style="text-align:right;">
                    <asp:Button ID="btnCCConfig" runat="server" Text="Next >>" 
                        onclick="btnCCConfig_Click" />
                    </div>
            </div>
            &nbsp;<br />
            <asp:LinkButton ID="lnkBackFrom3b" runat="server" onclick="lnkBackFrom3b_Click">&laquo; Back</asp:LinkButton>
        </asp:View>
         <asp:View ID="step4" runat="server">
            <h3>Step 4 of 4</h3>
            &nbsp;
            <div class="controlarea2" style="padding:10px;">
                <strong>Other Payment Options</strong>
                
                <asp:CheckBoxList ID="lstOther" runat="server">
                    <asp:ListItem><b>PayPal Express</b><br />
                                    <blockquote>
                                  <em>According to Jupiter Research, 23% of online shoppers consider PayPal one of their favorite ways to pay online. * Accepting PayPal in addition to credit cards is proven to increase your sales **</em>
                                  <br /><a href="#">See Quick Demo</a>
                                  </blockquote>                  </asp:ListItem>                    
                    <asp:ListItem><b>Cash on Delivery</b></asp:ListItem>
                    <asp:ListItem><b>eCheck</b></asp:ListItem>
                </asp:CheckBoxList>
                
           
           
                <div style="text-align:right;">
                <asp:Button ID="btnFinish" runat="server" Text="Finish" />
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
    
</asp:Content>

