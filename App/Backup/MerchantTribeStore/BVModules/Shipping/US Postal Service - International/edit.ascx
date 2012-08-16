<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="edit.ascx.cs" Inherits="MerchantTribeStore.BVModules.Shipping.US_Postal_Service___International.edit" %>
<h1>Edit Shipping Method - US Postal Service International</h1>
<asp:Panel ID="pnlMain" DefaultButton="btnSave" runat="server">
<asp:HyperLink ID="HyperLink1" NavigateUrl="~/bvadmin/configuration/ShippingUSPSInternationalTester.aspx" Text="Test/Estimate International Rates" runat="server" Target="_blank"></asp:HyperLink>
<table border="0" cellspacing="0" cellpadding="3">
    <tr>
        <td class="formlabel">Name:</td>
        <td class="formfield"><asp:TextBox ID="NameField" runat="server" Width="300"></asp:TextBox></td>
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
        <td class="formlabel">Services:</td>
        <td class="formfield">
            <asp:Panel ID="pnlFilter" runat="server">
                    <asp:CheckBoxList ID="ShippingTypesCheckBoxList" runat="server">                        
                    </asp:CheckBoxList>
            </asp:Panel>
        </td>
    </tr>    
    <tr>
            <td class="formlabel">Diagnostics Mode:</td>
            <td class="formfield"><asp:CheckBox ID="Diagnostics" runat="server" /></td>
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
                <asp:ListItem Selected="True" Value="1">Amount</asp:ListItem>
                <asp:ListItem Value="2">Percentage</asp:ListItem>
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
</table></asp:Panel>