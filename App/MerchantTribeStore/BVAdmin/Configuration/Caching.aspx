<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="true" CodeBehind="Caching.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.Configuration.Caching" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <uc1:MessageBox ID="msg" runat="server" />
<h1>Caching</h1>    
<asp:LinkButton ID="btnClearStoreCache" runat="server" CssClass="btn" Text="<b>Clear Store Cache</b>" OnClick="btnClearStoreCache_Click"></asp:LinkButton>
</asp:Content>
