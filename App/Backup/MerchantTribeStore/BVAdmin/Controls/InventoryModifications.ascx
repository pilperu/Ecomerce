<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Controls_InventoryModifications" Codebehind="InventoryModifications.ascx.cs" %>
<%@ Register Src="EnumeratedValueModifierField.ascx" TagName="EnumeratedValueModifierField"
    TagPrefix="uc5" %>
<%@ Register Src="IntegerModifierField.ascx" TagName="IntegerModifierField" TagPrefix="uc3" %>
<asp:Panel ID="InventoryModificationsPanel" runat="server">
    <table>        
        <tr id="Tr9" runat="server">
            <td><asp:CheckBox ID="QuantityAvailableCheckBox" runat="server" CssClass="modificationSelected"/>Quantity Available</td>
            <td>
                <uc3:IntegerModifierField ID="QuantityAvailableIntegerModifierField" runat="server" />
            </td>
        </tr>
        <tr id="Tr10" runat="server">
            <td><asp:CheckBox ID="QuantityOutOfStockPointCheckBox" runat="server" CssClass="modificationSelected"/>Quantity Out Of Stock Point</td>
            <td>
                <uc3:IntegerModifierField ID="QuantityOutOfStockPointIntegerModifierField" runat="server" />
            </td>
        </tr>     
        <tr id="Tr12" runat="server">
            <td><asp:CheckBox ID="QuantityReservedCheckBox" runat="server" CssClass="modificationSelected"/>Quantity Reserved</td>
            <td>
                <uc3:IntegerModifierField ID="QuantityReserveIntegerModifierField" runat="server" />
            </td>
        </tr>     
        <%-- 
        <tr id="Tr14" runat="server">
            <td><asp:CheckBox ID="StatusCheckBox" runat="server" CssClass="modificationSelected"/>Status</td>
            <td>
                <uc5:EnumeratedValueModifierField ID="StatusEnumeratedValueModifierField" runat="server" />
            </td>
        </tr>--%>      
    </table>
</asp:Panel>    
    