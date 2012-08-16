<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_ContentBlocks_Top_10_Products_adminview" Codebehind="adminview.ascx.cs" %>
<div class="sidemenu">
    <div class="decoratedblock">
        <h5>Top Sellers</h5>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="bvin"
            BorderColor="#CCCCCC" CellPadding="2" GridLines="None" Width="100%" >
            <Columns>
                <asp:BoundField DataField="ProductName" />
            </Columns>
        </asp:GridView>
    </div>
</div>
