<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_ContentBlocks_Top_Weekly_Sellers_adminview" Codebehind="adminview.ascx.cs" %>
<h4><asp:Label runat="server">Top Weekly Sellers</asp:Label></h4>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="bvin"
        BorderColor="#CCCCCC" CellPadding="3" GridLines="None" Width="100%" AllowSorting="true">
        <Columns>
            <asp:HyperLinkField DataTextField="ProductName"/>
        </Columns>
    </asp:GridView>
