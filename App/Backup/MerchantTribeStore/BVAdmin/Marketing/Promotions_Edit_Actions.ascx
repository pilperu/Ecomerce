<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Promotions_Edit_Actions.ascx.cs" Inherits="MerchantTribeStore.BVAdmin.Marketing.Promotions_Edit_Actions" %>
<div style="overflow:auto;height:500px;">
<%@ Register src="../Controls/MessageBox.ascx" tagname="MessageBox" tagprefix="uc1" %>
<uc1:MessageBox ID="MessageBox1" runat="server" />
<asp:MultiView ID="MultiView1" runat="server">
<asp:View ID="viewAdjustProductPrice" runat="server">
    <h1>Discount Product Price by...</h1>
    <table>
    <tr>
        <td class="formlabel">Adjust Price by +/-:</td>
        <td class="formfield"><asp:TextBox ID="AmountField" runat="server" Columns="10"></asp:TextBox> <asp:DropDownList ID="lstAdjustmentType" runat="server">
        <asp:ListItem Value="0">Amount</asp:ListItem>
        <asp:ListItem Value="1">Percent</asp:ListItem>
        </asp:DropDownList></td>
    </tr>
    </table>
</asp:View>
<asp:View ID="viewAdjustOrderTotal" runat="server">
    <h1>Discount Order Total by...</h1>
    <table>
    <tr>
        <td class="formlabel">Adjust Price by +/-:</td>
        <td class="formfield"><asp:TextBox ID="OrderTotalAmountField" runat="server" Columns="10"></asp:TextBox> <asp:DropDownList ID="lstOrderTotalAdjustmentType" runat="server">
        <asp:ListItem Value="0">Amount</asp:ListItem>
        <asp:ListItem Value="1">Percent</asp:ListItem>
        </asp:DropDownList></td>
    </tr>
    </table>
</asp:View>
<asp:View ID="viewOrderShippingAdjustment" runat="server">
    <h1>Discount Shipping by...</h1>
    <table>
    <tr>
        <td class="formlabel">Adjust Shipping by +/-:</td>
        <td class="formfield"><asp:TextBox ID="OrderShippingAdjustmentAmount" runat="server" Columns="10"></asp:TextBox> <asp:DropDownList ID="lstOrderShippingAdjustmentType" runat="server">
        <asp:ListItem Value="0">Amount</asp:ListItem>
        <asp:ListItem Value="1">Percent</asp:ListItem>
        </asp:DropDownList></td>
    </tr>
    </table>
</asp:View>
</asp:MultiView>
<asp:HiddenField ID="itemid" runat="server" />
<asp:HiddenField ID="promotionid" runat="server" />
</div>
