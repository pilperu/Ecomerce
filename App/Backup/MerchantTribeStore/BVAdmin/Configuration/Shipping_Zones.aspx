<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_Shipping_Zones" Codebehind="Shipping_Zones.aspx.cs" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
<h1>Shipping Zones</h1>
    <uc1:MessageBox ID="MessageBox1" runat="server" />
    &nbsp;<br />

    <table width="100%">
    <asp:Literal ID="litMain" runat="server" EnableViewState="false"></asp:Literal>    
    </table><br />
    &nbsp;<br />    
        <div class="controlarea2">
        New Zone: <asp:TextBox ID="NewZoneField" runat="server" Columns="30" runat="server"></asp:TextBox>
        <asp:ImageButton ID="btnNew" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/New.png" onclick="btnNew_Click" />
        </div>    

</asp:Content>

