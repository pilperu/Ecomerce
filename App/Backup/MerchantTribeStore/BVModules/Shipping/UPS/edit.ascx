<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_Shipping_UPS_edit" Codebehind="edit.ascx.cs" %>
<h1>
    Edit Shipping Method - UPS</h1>
<asp:Panel ID="pnlMain" DefaultButton="btnSave" runat="server">
    <asp:Literal ID="litMessage" runat="server"></asp:Literal>
    <table border="0" cellspacing="0" cellpadding="3">
        <tr>
            <td colspan="2">
                <h2>
                    Methods Specific Settings</h2>
            </td>
        </tr>    
         <tr>
            <td class="formlabel">Name:</td>
            <td class="formfield"><asp:TextBox ID="NameField" runat="server" /></td>
        </tr>    
        <tr>
            <td class="formlabel">
                Shipping Zone:
            </td>
            <td class="formfield">                
                <asp:DropDownList ID="lstZones" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="formlabel">
                &nbsp;
            </td>
            <td class="formfield">
                <asp:RadioButtonList ID="rbFilterMode" runat="server" AutoPostBack="true" 
                    onselectedindexchanged="rbFilterMode_SelectedIndexChanged">
                    <asp:ListItem Text="Try all available UPS Services" Value="1" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Try only the UPS Services I select" Value="0"></asp:ListItem>
                </asp:RadioButtonList>
                <asp:Panel ID="pnlFilter" runat="server">
                    <asp:CheckBoxList ID="ShippingTypesCheckBoxList" runat="server">                        
                    </asp:CheckBoxList>
                </asp:Panel>
                
            </td>
        </tr>        
        <tr>
            <td colspan="2">
                <h2>
                    Global UPS Settings</h2>
            </td>
        </tr>
        <tr>
            <td class="formfield">Registration Status:</td>
            <td class="forminput"><asp:HyperLink ID="lnkRegister" runat="server" NavigateUrl="~/BVAdmin/Configuration/ShippingUpsLicense.aspx"></asp:HyperLink></td>
        </tr>
        <tr>
            <td class="formlabel">
                Account Number:</td>
            <td class="formfield">
                <asp:TextBox ID="AccountNumberField" runat="server" Width="300"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="formlabel">
                Force Residential Addresses:
            </td>
            <td class="formfield">
                <asp:CheckBox ID="ResidentialAddressCheckBox" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="formlabel">
                Don't use package dimensions, only weight:
            </td>
            <td class="formfield">
                <asp:CheckBox ID="SkipDimensionsCheckBox" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="formlabel">
                Pickup Type:
            </td>
            <td class="formfield">
                <asp:RadioButtonList ID="PickupTypeRadioButtonList" runat="server">
                    <asp:ListItem Enabled="true" Selected="true" Text="Daily Pickup" Value="1"></asp:ListItem>
                    <asp:ListItem Enabled="true" Selected="false" Text="Customer Counter" Value="3"></asp:ListItem>
                    <asp:ListItem Enabled="true" Selected="false" Text="Suggested Retail Rate" Value="11"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="formlabel">
                Default Service:
            </td>
            <td class="formfield">
                <asp:DropDownList runat="server" ID="DefaultServiceField">
                    <asp:ListItem Value="3">UPS Ground</asp:ListItem>
                    <asp:ListItem Value="1">UPS Next Day Air&#174;</asp:ListItem>
                    <asp:ListItem Value="13">UPS Next Day Air Saver&#174;</asp:ListItem>
                    <asp:ListItem Value="14">UPS Next Day Air&#174; Early A.M.&#174;</asp:ListItem>
                    <asp:ListItem Value="2">UPS 2nd Day Air&#174;</asp:ListItem>
                    <asp:ListItem Value="59">UPS 2nd Day Air A.M.&#174;</asp:ListItem>
                    <asp:ListItem Value="12">UPS 3 Day Select (sm)</asp:ListItem>
                    <asp:ListItem Value="65">UPS Express Saver (sm)</asp:ListItem>
                    <asp:ListItem Value="7">UPS Worldwide Express (sm)</asp:ListItem>
                    <asp:ListItem Value="54">UPS Worldwide Express Plus (sm)</asp:ListItem>
                    <asp:ListItem Value="8">UPS Worldwide Expedited (sm)</asp:ListItem>
                    <asp:ListItem Value="11">UPS Standard</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="formlabel">
                Default Packaging:
            </td>
            <td class="formfield">
                <asp:DropDownList runat="server" ID="DefaultPackagingField">
                    <asp:ListItem Value="2">Customer Supplied</asp:ListItem>
                    <asp:ListItem Value="1">UPS Letter</asp:ListItem>
                    <asp:ListItem Value="3">UPS Tube</asp:ListItem>
                    <asp:ListItem Value="4">UPS Pak</asp:ListItem>
                    <asp:ListItem Value="21">UPS Express Box</asp:ListItem>
                    <asp:ListItem Value="25">UPS 10Kg Box</asp:ListItem>
                    <asp:ListItem Value="24">UPS 25Kg Box</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>        
        <tr>
            <td class="formlabel">Diagnostics Mode:</td>
            <td class="formfield"><asp:CheckBox ID="chkDiagnostics" runat="server" /></td>
        </tr>
        <tr>
            <td colspan="2">
                <h2>
                    Adjustments</h2>
            </td>
        </tr>
        <tr>
            <td class="formlabel">
                Adjust price by:
            </td>
            <td class="formfield">
                <asp:TextBox ID="AdjustmentTextBox" runat="server" Columns="5"></asp:TextBox>
                &nbsp;<asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="AdjustmentTextBox"
                    ErrorMessage="Adjustment is not in the correct format." 
                    onservervalidate="CustomValidator1_ServerValidate">*</asp:CustomValidator>
                <asp:DropDownList ID="AdjustmentDropDownList" runat="server">
                    <asp:ListItem Value="1">Amount</asp:ListItem>
                    <asp:ListItem Selected="True" Value="2">Percentage</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="formlabel">
                <asp:ImageButton ID="btnCancel" CausesValidation="false" runat="server" 
                    ImageUrl="~/BVAdmin/Images/Buttons/Cancel.png" onclick="btnCancel_Click" /></td>
            <td class="formfield">
                <asp:ImageButton ID="btnSave" runat="server" 
                    ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" onclick="btnSave_Click" /></td>
        </tr>
    </table>
</asp:Panel>
