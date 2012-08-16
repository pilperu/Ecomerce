<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_Themes" title="Untitled Page" Codebehind="Themes.aspx.cs" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="headercontent" ContentPlaceHolderID="headcontent" runat="server">

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<div class="sidebyside1">
    <h1>Installed Themes</h1>
    <uc1:MessageBox ID="MessageBox1" runat="server" />
    <asp:Literal ID="litInstalled" runat="server" EnableViewState="false"></asp:Literal>
</div>
<div class="sidebyside2">
    <h1>Available Themes</h1>    
    <div class="controlarea1">
    <asp:Literal ID="litAvailable" runat="server" EnableViewState="false"></asp:Literal>
    </div>
</div>
<div class="clear"></div>

</asp:Content>

