<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="true" CodeBehind="Promotions.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.Marketing.Promotions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavContent" runat="server">
    <div class="controlarea1">
                        <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnGo">
                            Search:
                            <table>
                            <tr>
                                <td><asp:TextBox ID="FilterField" runat="server" Width="140px" style="margin-top:5px;"></asp:TextBox></td>
                                <td><asp:ImageButton ID="btnGo" runat="server" AlternateText="Filter Results" 
                                ImageUrl="~/BVAdmin/Images/Buttons/SmallRight.png" onclick="btnGo_Click" /></td>
                            </tr>
                            <tr>
                                <td colspan="2"><asp:CheckBox ID="chkShowDisabled" runat="server" Text="Show Disabled Items" /></td>
                            </tr>
                            </table>
                            </asp:Panel>
            </div>
            &nbsp;<br />
            <div class="controlarea1">
            Create New:<br />
            <asp:DropDownList ID="lstNewType" runat="server">
                <asp:ListItem Value="-1">Custom Sale</asp:ListItem>
                <asp:ListItem Value="-2">Custom Offer</asp:ListItem>
                <asp:ListItem Value="">----------------</asp:ListItem>
                <asp:ListItem Value="0">Sale: Storewide</asp:ListItem>
                <asp:ListItem Value="1">Sale: Products</asp:ListItem>
                <asp:ListItem Value="2">Sale: Categories</asp:ListItem>
                <asp:ListItem Value="3">Sale: Product Types</asp:ListItem>
                <asp:ListItem Value="4">Sale: By Price Group</asp:ListItem>
                <asp:ListItem Value="5">Sale: By User</asp:ListItem>
                <asp:ListItem Value="6">Offer: With a Coupon</asp:ListItem>                
                <asp:ListItem Value="8">Offer: By Price Group</asp:ListItem>
                <asp:ListItem Value="7">Offer: By User</asp:ListItem>
                <asp:ListItem Value="9">Offer: Free Shipping</asp:ListItem>
                <asp:ListItem Value="10">Offer: Shipping Discount</asp:ListItem>
            </asp:DropDownList><br />
            <asp:ImageButton ID="btnNew" runat="server" 
                            AlternateText="Create Promotion" 
                            ImageUrl="~/BVAdmin/Images/Buttons/New.png" onclick="btnNew_Click" />                
            </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Promotions</h1>    
            <h2><asp:label ID="lblResults" runat="server" EnableViewState="false"></asp:label></h2>
            <div id="resultsgrid">
                <asp:Literal id="litPager1" runat="server" EnableViewState="false"></asp:Literal>
                <asp:Literal id="litResults" ClientIDMode="Static" runat="server" EnableViewState="false"></asp:Literal>
                <div class="clear"></div>
                <asp:Literal id="litPager2" runat="server" EnableViewState="false"></asp:Literal>
            </div>                
        &nbsp;
</asp:Content>
