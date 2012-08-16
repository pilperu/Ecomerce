<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Orders_OrderStatusDisplay" Codebehind="OrderStatusDisplay.ascx.cs" %>
<table border="0" cellspacing="0" cellpadding="3">
<tr>
    <td><asp:Literal ID="litPay" runat="server"></asp:Literal> / </td>
    <td><asp:Literal ID="litShip" runat="server"></asp:Literal> / </td>
    <td><asp:DropDownList ID="lstStatus" runat="server" AutoPostBack="true" 
            onselectedindexchanged="lstStatus_SelectedIndexChanged">
                <asp:ListItem Value= "A7FFDB90-C566-4cf2-93F4-D42367F359D5">Cancelled</asp:ListItem>
            <asp:ListItem Value="88B5B4BE-CA7B-41a9-9242-D96ED3CA3135">On Hold</asp:ListItem>
            <asp:ListItem Value="F37EC405-1EC6-4a91-9AC4-6836215FBBBC">Received</asp:ListItem>
            <asp:ListItem Value="e42f8c28-9078-47d6-89f8-032c9a6e1cce">Ready for Payment</asp:ListItem>
            <asp:ListItem Value="0c6d4b57-3e46-4c20-9361-6b0e5827db5a">Ready for Shipping</asp:ListItem>
            <asp:ListItem Value="09D7305D-BD95-48d2-A025-16ADC827582A">Complete</asp:ListItem>
</asp:DropDownList></td>
</tr>
</table>