<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_CreditCardGateways_BV_PayFlowProNET_edit" Codebehind="edit.ascx.cs" %>
<h1>Payflow Pro Options</h1>
<asp:Panel ID="Panel1" DefaultButton="btnSave" runat="server">
<table border="0" cellspacing="0" cellpadding="3" style="width: 364px">
<tr>
    <td class="formlabel" colspan="2" align="center"></td>
</tr>
    <tr>
        <td class="formlabel">
            Partner:</td>
        <td class="formfield" style="width: 185px">
            <asp:TextBox ID="txtMerchantVendor" runat="server" ToolTip="Merchant Login is Required"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtMerchantVendor"
                ErrorMessage="Merchant Vendor Required">*</asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td class="formlabel">
            Vendor:</td>
        <td class="formfield" style="width: 185px">
            <asp:TextBox ID="txtMerchantLogin" runat="server" ToolTip="Merchant Login is Required"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtMerchantLogin"
                ErrorMessage="Merchant Login Required">*</asp:RequiredFieldValidator></td>
    </tr>
<tr>
    <td class="formlabel">
        User:</td>
    <td class="formfield" style="width: 185px">
        <asp:TextBox ID="txtMerchantUser" runat="server" ToolTip="Merchant User is sometimes Required and if often the same as your Login."></asp:TextBox>
    </td>
</tr>
<tr>
    <td class="formlabel" style="height: 30px">
        Merchant Password:</td>
    <td class="formfield" style="width: 185px; height: 30px;">
        <asp:TextBox ID="txtMerchantPassword" runat="server" ToolTip="Merchant Password is Required."></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMerchantPassword"
            ErrorMessage="Merchant Transaction Key Required">*</asp:RequiredFieldValidator></td>
    </tr>  
    <tr>
        <td class="formlabel" style="height: 30px">
            Currency Code:</td>
        <td class="formfield" style="width: 185px; height: 30px">
            <asp:DropDownList ID="CurrencyCodeDropDownList" runat="server">
            <asp:ListItem Text="Argentine Peso" Value="32"></asp:ListItem>
            <asp:ListItem Text="Australian Dollar" Value="36"></asp:ListItem>
            <asp:ListItem Text="Austrian Schilling" Value="40"></asp:ListItem>
            <asp:ListItem Text="Belgian Franc" Value="56"></asp:ListItem>
            <asp:ListItem Text="Canadian Dollar" Value="124"></asp:ListItem>
            <asp:ListItem Text="Chilean Peso" Value="152"></asp:ListItem>
            <asp:ListItem Text="Czech Koruna" Value="203"></asp:ListItem>
            <asp:ListItem Text="Danish Krone" Value="208"></asp:ListItem>
            <asp:ListItem Text="Dominican Peso" Value="214"></asp:ListItem>
            <asp:ListItem Text="Markka" Value="246"></asp:ListItem>
            <asp:ListItem Text="French Franc" Value="250"></asp:ListItem>
            <asp:ListItem Text="Deutsche Mark" Value="280"></asp:ListItem>
            <asp:ListItem Text="Drachma" Value="300"></asp:ListItem>
            <asp:ListItem Text="Hong Kong Dollar" Value="344"></asp:ListItem>
            <asp:ListItem Text="Indian Rupee" Value="356"></asp:ListItem>
            <asp:ListItem Text="Irish Punt" Value="372"></asp:ListItem>
            <asp:ListItem Text="Shekel" Value="376"></asp:ListItem>
            <asp:ListItem Text="Italian Lira" Value="380"></asp:ListItem>
            <asp:ListItem Text="Yen" Value="392"></asp:ListItem>
            <asp:ListItem Text="Won" Value="410"></asp:ListItem>
            <asp:ListItem Text="Luxembourg Franc" Value="442"></asp:ListItem>
            <asp:ListItem Text="Mexican Nuevo Peso" Value="484"></asp:ListItem>
            <asp:ListItem Text="Netherlands Guilder" Value="528"></asp:ListItem>
            <asp:ListItem Text="New Zealand Dollar" Value="554"></asp:ListItem>
            <asp:ListItem Text="Norwegian Frone" Value="578"></asp:ListItem>
            <asp:ListItem Text="Philippine Peso" Value="608"></asp:ListItem>
            <asp:ListItem Text="Portuguese Escudo" Value="620"></asp:ListItem>
            <asp:ListItem Text="Rand" Value="710"></asp:ListItem>
            <asp:ListItem Text="Spanish Peseta" Value="724"></asp:ListItem>
            <asp:ListItem Text="Swedish Krona" Value="752"></asp:ListItem>
            <asp:ListItem Text="Swiss Franc" Value="756"></asp:ListItem>
            <asp:ListItem Text="Thailand Baht" Value="764"></asp:ListItem>
            <asp:ListItem Text="Pound Sterling" Value="826"></asp:ListItem>
            <asp:ListItem Text="Russian Ruble" Value="810"></asp:ListItem>
            <asp:ListItem Text="U.S Dollar" Value="840"></asp:ListItem>
            <asp:ListItem Text="Bolivar" Value="862"></asp:ListItem>
            <asp:ListItem Text="New Taiwan Dollar" Value="901"></asp:ListItem>
            <asp:ListItem Text="Euro" Value="978"></asp:ListItem>
            <asp:ListItem Text="Polish New Zloty" Value="985"></asp:ListItem>
            <asp:ListItem Text="Brazilian Real" Value="986"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
<tr>
    <td class="formlabel">
        Test Mode:</td>
    <td class="formfield" style="width: 185px"><asp:CheckBox ID="chkTestMode" runat="server" ToolTip="Enable Gateways Test Mode" /></td>
</tr>
    <tr>
        <td class="formlabel" style="height: 25px">
            Debug Mode:</td>
        <td class="formfield" style="width: 185px; height: 25px">
            <asp:CheckBox ID="chkDebugMode" runat="server" ToolTip="Checking this will log the full gateway response in the Event Log" /></td>
    </tr>
    <tr>
        <td class="formlabel" style="height: 25px">
        </td>
        <td class="formfield" style="width: 185px; height: 25px;">
        </td>
    </tr>
    <tr>
        <td align="center" class="formlabel" style="height: 25px">
        <asp:ImageButton ID="btnCancel" CausesValidation="false" runat="server" 
                ImageUrl="~/BVAdmin/Images/Buttons/Cancel.png" onclick="btnCancel_Click" /></td>
        <td align="center" class="formfield" style="width: 185px; height: 25px">
        <asp:ImageButton ID="btnSave" runat="server" 
                ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" onclick="btnSave_Click" /></td>
    </tr>
    <tr>
        <td align="center" class="formlabel" colspan="2" style="height: 25px">
        </td>
    </tr>
    <tr>
        <td align="center" class="formlabel" colspan="2" style="height: 25px">
        </td>
    </tr>
    <tr>
        <td align="center" class="formlabel" colspan="2" style="height: 25px">
            .</td>
    </tr>
</table></asp:Panel>
