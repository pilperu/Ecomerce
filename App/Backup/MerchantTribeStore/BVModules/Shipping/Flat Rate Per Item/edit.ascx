<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_Shipping_Flat_Rate_Per_Item_edit" Codebehind="edit.ascx.cs" %>
<h1>
    Edit Shipping Method - 
    Flat Rate Per Item</h1>
<asp:Panel ID="pnlMain" DefaultButton="btnSave" runat="server">
    <table border="0" cellspacing="0" cellpadding="5" class="formtable">
        <tr>
            <td class="formlabel">Name:</td>
            <td class="formfield">                
                <asp:TextBox ID="NameField" runat="server" Width="300"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="formlabel">Per Item Charge:</td>
            <td class="formfield">                
                <asp:TextBox ID="AmountField" runat="server" Width="100px"></asp:TextBox></td>
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
                <asp:ImageButton ID="btnCancel" CausesValidation="false" runat="server" 
                    ImageUrl="~/BVAdmin/Images/Buttons/Cancel.png" onclick="btnCancel_Click" /></td>
            <td class="formfield">
                <asp:ImageButton ID="btnSave" runat="server" 
                    ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" onclick="btnSave_Click" /></td>
        </tr>
    </table>        
</asp:Panel>
