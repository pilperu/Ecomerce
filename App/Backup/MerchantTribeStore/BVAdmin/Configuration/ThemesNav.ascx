<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_ThemesNav" Codebehind="ThemesNav.ascx.cs" %>
<ul class="navmenu">
    <li><asp:Hyperlink ID="lnkEdit" runat="server" NavigateUrl="ThemesEdit.aspx">Theme Info</asp:Hyperlink></li>
    <li><asp:Hyperlink ID="lnkCss" runat="server" NavigateUrl="ThemesEditCSS.aspx">Style Sheet (CSS)</asp:Hyperlink></li>
    <li><asp:Hyperlink ID="lnkHeader" runat="server" NavigateUrl="ThemesEditHeaderFooter.aspx">Header/Footer</asp:Hyperlink></li>
    <li><asp:Hyperlink ID="lnkButtons" runat="server" NavigateUrl="ThemesEditButtons.aspx">Images: Buttons</asp:Hyperlink></li>
    <li><asp:Hyperlink ID="lnkAssets" runat="server" NavigateUrl="ThemesEditAssets.aspx">Images: Other</asp:Hyperlink></li>
    <li><asp:Hyperlink ID="lnkColumns" runat="server" NavigateUrl="ThemesEditColumns.aspx">Content Columns</asp:Hyperlink></li>
</ul>