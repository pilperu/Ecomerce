<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/AdminWizard.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_SetupWizard_WizardPayment" Codebehind="WizardPayment.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>
        Store Setup - Step 2 of 4 - Payment Method</h1>
    <p class="flash-message-info">
        What type of payments will your store accept?</p>
    <p class="flash-message-minor">
        You can change these later if you're not sure.</p>
    <div style="padding: 20px 100px;">        
        <asp:Panel ID="pnlMain" runat="server">
            <table class="formtable big">
                <tr>
                    <td>
                        <asp:CheckBox ID="chkCreditCard" runat="server" Checked="true" />
                    </td>
                    <td>
                        <asp:Label ID="lblCreditCards" runat="server">Credit Cards</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkPurchaseOrder" runat="server" Checked="false" />
                    </td>
                    <td>
                        Purchase Orders
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkCheck" runat="server" Checked="false" />
                    </td>
                    <td>
                        Checks by Mail
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkTelephone" runat="server" Checked="false" />
                    </td>
                    <td>
                        Let Customers Call in Payment Information Later
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkCOD" runat="server" Checked="false" />
                    </td>
                    <td>
                        Cash on Delivery
                    </td>
                </tr>                              
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlPayPay" runat="server">
            <table class="formtable big">
                <tr>
                    <td>
                        <asp:CheckBox ID="chkPayPalCreditCards" runat="server" Checked="true" />
                    </td>
                    <td>
                        PayPal Website Payments Pro OR Payflow Pro
                    </td>
                </tr>
            </table>
        </asp:Panel>
        &nbsp;
        <div class="editorpanel">
            <asp:CheckBox ID="chkPayPalExpress" runat="server" Checked="true" /> <b>Accept PayPal Express Checkout</b>
            <div style="float:right"><img src="https://www.paypal.com/en_US/i/btn/btn_xpressCheckout.gif" alt="PayPal Express Checkout Logo" /></div>
            <p>PayPal: Proven to Increase Sales. According to Jupiter Research, 23% of online shoppers like to pay with PayPal.<sup>*</sup>
            If you offer your visitors the chance to pay with PayPal, they will be more likely to buy.                
            </p><br />
            <p class="tiny"><sup>*</sup>September 2007 Jupiter Research study of payment preferences online.</p>
            <hr />
            <asp:RadioButtonList runat="server" ID="ModeRadioButtonList">
                <asp:ListItem Selected="True" Text="Production Mode" Value="Live"></asp:ListItem>    
                <asp:ListItem Text="Sandbox Mode (for testing)" Value="Sandbox"></asp:ListItem>                
            </asp:RadioButtonList><br />            
            <asp:RadioButton ID="btnFastSignup" GroupName="FastSignup" runat="server" 
                AutoPostBack="True" Checked="True" 
                oncheckedchanged="btnFastSignup_CheckedChanged" /> E-mail address to receive PayPal payment: 
            <asp:TextBox ID="PayPalFastSignupEmail" runat="server" Columns="40" /><br />
            <asp:RadioButton ID="btnSlowSignup" GroupName="FastSignup" runat="server" 
                AutoPostBack="True" oncheckedchanged="btnSlowSignup_CheckedChanged" /> API Credentials for payments and post-checkout operations:<br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(can be setup later)
            <asp:Panel id="pnlApi" runat="server" Visible="false" CssClass="controlarea2" style="margin-left:20px;">
                <table class="formtable">
                <tr>
                    <td class="formlabel">API Username:</td>
                    <td class="formfield"><asp:TextBox id="APIUsername" runat="server" Columns="20"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="formlabel">API Password:</td>
                    <td class="formfield"><asp:TextBox id="APIPassword" runat="server" Columns="20"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="formlabel">API Signature:</td>
                    <td class="formfield"><asp:TextBox id="APISignature" runat="server" Columns="20"></asp:TextBox></td>
                </tr>
                </table>
            </asp:Panel>            
        </div>        
        <div style="padding: 20px 0 0 0;">
             <asp:ImageButton ID="btnSaveMain" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/SaveAndContinue.png"
                            OnClick="btnSaveMain_Click" />            
        </div>
    </div>
</asp:Content>
