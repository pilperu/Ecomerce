<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_Payment_Edit_Gateway" title="Untitled Page" Codebehind="Payment_Edit_Gateway.aspx.cs" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div style="text-align: center; margin: auto;">
        <asp:PlaceHolder ID="phEditor" runat="server"></asp:PlaceHolder>
    </div>
    <asp:HiddenField ID="GatewayIdField" runat="server" />
    <asp:HiddenField ID="MethodIdField" runat="server" />
</asp:Content>

