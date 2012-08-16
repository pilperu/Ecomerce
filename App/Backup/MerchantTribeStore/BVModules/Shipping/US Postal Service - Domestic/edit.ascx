<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_Shipping_US_Postal_Service_edit" Codebehind="edit.ascx.cs" %>
<h1>Edit Shipping Method - US Postal Service</h1>
<asp:Panel ID="pnlMain" DefaultButton="btnSave" runat="server">
<asp:HyperLink NavigateUrl="~/bvadmin/configuration/ShippingUSPSTester.aspx" Text="Test/Estimate Rates" runat="server" Target="_blank"></asp:HyperLink>
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
        <td class="formlabel">Package Type:</td>
        <td class="formfield"><asp:DropDownList ID="lstPackageType" runat="server">        
        <asp:ListItem Text="Auto-Selected Packaging" Value="-1"></asp:ListItem>
        <asp:ListItem Text="-------------------" Value="-1"></asp:ListItem>
        <asp:ListItem Text="First-Class Letter" Value="100"></asp:ListItem>
        <asp:ListItem Text="First-Class Large Envelope" Value="101"></asp:ListItem>
        <asp:ListItem Text="First-Class Parcel" Value="102"></asp:ListItem>
        <asp:ListItem Text="First-Class PostCard" Value="103"></asp:ListItem>
        <asp:ListItem Text="-------------------" Value="-1"></asp:ListItem>        
        <asp:ListItem Text="Flat Rate Box" Value="1"></asp:ListItem>
        <asp:ListItem Text="Flat Rate Box Small" Value="2"></asp:ListItem>
        <asp:ListItem Text="Flat Rate Box Medium" Value="3"></asp:ListItem>
        <asp:ListItem Text="Flat Rate Box Large" Value="4"></asp:ListItem>
        <asp:ListItem Text="Flat Rate Envelope" Value="5"></asp:ListItem>
        <asp:ListItem Text="Flat Rate Envelope Padded" Value="50"></asp:ListItem>
        <asp:ListItem Text="Flat Rate Envelope Legal" Value="51"></asp:ListItem>
        <asp:ListItem Text="Flat Rate Envelope Small" Value="52"></asp:ListItem>
        <asp:ListItem Text="Flat Rate Envelope Window" Value="53"></asp:ListItem>
        <asp:ListItem Text="Flat Rate Envelope Gift Card" Value="53"></asp:ListItem>
        <asp:ListItem Text="-------------------" Value="-1"></asp:ListItem>
        <asp:ListItem Text="Variable" Value="0"></asp:ListItem>
        <asp:ListItem Text="Rectangular" Value="6"></asp:ListItem>
        <asp:ListItem Text="Non-Rectangular" Value="7"></asp:ListItem>
        <asp:ListItem Text="Regional Box Rate A" Value="200"></asp:ListItem>
        <asp:ListItem Text="Regional Box Rate B" Value="201"></asp:ListItem>        
        </asp:DropDownList></td>
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