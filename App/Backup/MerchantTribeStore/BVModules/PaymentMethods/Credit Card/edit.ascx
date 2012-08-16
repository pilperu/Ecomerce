<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_PaymentMethods_Credit_Card_edit" Codebehind="edit.ascx.cs" %>
<h1>Credit Card Options</h1>
<asp:Panel ID="pnlMain" DefaultButton="btnSave" runat="server">
<table border="0" cellspacing="0" cellpadding="3">
<tr>
    <td class="formlabel">Capture Mode:</td>
    <td class="formfield"><asp:RadioButtonList runat="server" ID="lstCaptureMode">
    <asp:ListItem Text="Authorize at Checkout, Capture Funds Later" Value="1"></asp:ListItem>
    <asp:ListItem Selected="true" Text="Charge Full Amount at Checkout" Value="0"></asp:ListItem>
    </asp:RadioButtonList></td>
</tr>
<tr>
    <td class="formlabel">Security Code:</td>
    <td class="formfield"><asp:CheckBox ID="chkRequireCreditCardSecurityCode" runat="server" Text="Require CVV code during checkout?" /></td>
</tr>
<tr><td colspan="2">&nbsp;</td></tr>
<tr>
    <td class="formlabel">Accepted Cards:</td>
    <td class="formfield">
        <span class="cc-visa"></span>&nbsp;<asp:CheckBox ID="chkCardVisa" runat="server" Text="Visa" /><br />
        <span class="cc-mastercard"></span>&nbsp;<asp:CheckBox ID="chkCardMasterCard" runat="server" Text="MasterCard" /><br />
        <span class="cc-amex"></span>&nbsp;<asp:CheckBox ID="chkCardAmex" runat="server" Text="American Express (Amex)" /><br />
        <span class="cc-discover"></span>&nbsp;<asp:CheckBox ID="chkCardDiscover" runat="server" Text="Discover Card" /><br />
        <span class="cc-diners"></span>&nbsp;<asp:CheckBox ID="chkCardDiners" runat="server" Text="Diner's Club" /><br />
        <span class="cc-jcb"></span>&nbsp;<asp:CheckBox ID="chkCardJCB" runat="server" Text="JCB" /><br />
    </td>
</tr>
<tr>
    <td colspan="2">&nbsp;</td>
</tr>
<tr>
    <td class="formlabel">Gateway:</td>
    <td class="formfield"><asp:DropDownList ID="lstGateway" runat="server">
        <asp:ListItem>- Select a Gateway -</asp:ListItem>
    </asp:DropDownList>&nbsp;<asp:ImageButton ID="btnOptions" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/Edit.png" 
            AlternateText="Edit Gateway Options" onclick="btnOptions_Click" /></td>
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