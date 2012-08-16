<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_PaymentMethods_Paypal_Express_Edit" Codebehind="Edit.ascx.cs" %>
<h1>Paypal Express Options</h1>
<asp:Panel ID="pnlMain" DefaultButton="btnSave" runat="server">
<table class="formtable">
<tr>
    <td class="formlabel">Paypal Mode:</td>
    <td class="formfield"><asp:RadioButtonList runat="server" ID="ModeRadioButtonList">
    <asp:ListItem Selected="True" Text="Production Mode" Value="Live"></asp:ListItem>  
    <asp:ListItem Text="Sandbox Mode (for testing)" Value="Sandbox"></asp:ListItem>      
    </asp:RadioButtonList></td>
</tr>
<tr>
    <td colspan="2">&nbsp;<br /><div class="editorpanel" style="text-align:left;">
        <asp:RadioButton ID="btnFastSignup" GroupName="FastSignup" runat="server" 
                 Checked="True" /> E-mail address to receive PayPal payment: 
            <asp:TextBox ID="PayPalFastSignupEmail" runat="server" Columns="40" /><br />
            <asp:RadioButton ID="btnSlowSignup" GroupName="FastSignup" runat="server" /> API Credentials for payments and post-checkout operations:<br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(can be setup later)    
           <asp:Panel id="pnlApi" runat="server" CssClass="controlarea2" style="margin-left:20px;">
                <table class="formtable">
                <tr>
                    <td class="formlabel">API Username:</td>
                    <td class="formfield"><asp:TextBox id="UsernameTextBox" runat="server" Columns="20"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="formlabel">API Password:</td>
                    <td class="formfield"><asp:TextBox id="PasswordTextBox" runat="server" Columns="20"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="formlabel">API Signature:</td>
                    <td class="formfield"><asp:TextBox id="SignatureTextBox" runat="server" Columns="20"></asp:TextBox></td>
                </tr>
                </table>
            </asp:Panel>        </div><br />&nbsp;
    </td>
</tr>
<tr>
    <td class="formlabel">Capture Mode:</td>
    <td class="formfield"><asp:RadioButtonList runat="server" ID="lstCaptureMode">
    <asp:ListItem Text="Authorize at Checkout, Capture Funds Later" Value="1"></asp:ListItem>
    <asp:ListItem Selected="true" Text="Charge Full Amount at Checkout" Value="0"></asp:ListItem>
    </asp:RadioButtonList></td>
</tr>
<tr>
    <td class="formlabel">Allow payments from unconfirmed addresses:</td>
    <td class="formfield">
        <asp:CheckBox ID="UnconfirmedAddressCheckBox" runat="server" />
    </td>
</tr>
<tr>
    <td class="formlabel">Paypal Monetary Format:<p class="tiny">(Does not apply to Website Payments Pro, and <br />needs to be in the same currency as your store.)</p></td>
    <td class="formfield"><asp:DropDownList runat="server" ID="PaypalMonetaryFormatRadioButtonList">
    <asp:ListItem Selected="true" Text="U.S. Dollars" Value="USD"></asp:ListItem>
    <asp:ListItem Text="British Pound" Value="GBP"></asp:ListItem>
    <asp:ListItem Text="New Taiwan Dollar" Value="TWD"></asp:ListItem>
    <asp:ListItem Text="Swedish Krona" Value="SEK"></asp:ListItem>
    <asp:ListItem Text="Singapore Dollar" Value="SGD"></asp:ListItem>
    <asp:ListItem Text="Euro" Value="EUR"></asp:ListItem>
    <asp:ListItem Text="Swiss Franc" Value="CHF"></asp:ListItem>
    <asp:ListItem Text="Australian Dollar" Value="AUD"></asp:ListItem>
    <asp:ListItem Text="Hong Kong Dollar" Value="HKD"></asp:ListItem>
    <asp:ListItem Text="Canadian Dollar" Value="CAD"></asp:ListItem>
    <asp:ListItem Text="Indian Rupee" Value="INR"></asp:ListItem>
    </asp:DropDownList></td>
</tr>
<tr>
    <td colspan="2">&nbsp;</td>
</tr>
<tr>
    <td class="formlabel">
        <asp:ImageButton ID="btnCancel" CausesValidation="false" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/Cancel.png" onclick="btnCancel_Click" /></td>
    <td class="formfield">
        <asp:ImageButton ID="btnSave" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" onclick="btnSave_Click" /></td>
</tr>
</table></asp:Panel>
