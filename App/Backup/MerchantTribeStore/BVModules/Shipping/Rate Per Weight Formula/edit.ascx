<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="edit.ascx.cs" Inherits="MerchantTribeStore.BVModules.Shipping.Rate_Per_Weight_Formula.edit" %>
<h1>
    Edit Shipping Method - 
    Rate Per Weight Formula</h1>
<asp:Panel ID="pnlMain" DefaultButton="btnSave" runat="server">
    <table border="0" cellspacing="0" cellpadding="5" class="formtable">
        <tr>
            <td class="formlabel">Name:</td>
            <td class="formfield">                
                <asp:TextBox ID="NameField" runat="server" Width="300"></asp:TextBox></td>
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
            <td class="formlabel">When Weight is greater than</td>
            <td class="formfield">                
                <asp:TextBox ID="MinWeightField" runat="server" Width="50px"></asp:TextBox> Pounds/Kilos</td>
        </tr>
        <tr>
            <td class="formlabel">And Weight less than</td>
            <td class="formfield">                
                <asp:TextBox ID="MaxWeightField" runat="server" Width="50px"></asp:TextBox> Pounds/Kilos</td>
        </tr>
        <tr>
            <td class="formlabel">Charge:</td>
            <td class="formfield">                
                <asp:TextBox ID="BaseAmountField" runat="server" Width="50px"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="formlabel">and Charge:</td>
            <td class="formfield">                
                <asp:TextBox ID="AdditionalWeightChargeField" runat="server" Width="50px"></asp:TextBox> for each Pound/Kilo over <asp:TextBox ID="BaseWeightField" runat="server" Width="50px"></asp:TextBox> Pounds/Kilo</td>
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
