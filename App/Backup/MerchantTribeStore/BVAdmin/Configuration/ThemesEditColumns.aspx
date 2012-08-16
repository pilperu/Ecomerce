<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_ThemesEditColumns" Codebehind="ThemesEditColumns.aspx.cs" %>
<%@ Register src="ThemesNav.ascx" tagname="ThemesNav" tagprefix="uc1" %>
<%@ Register src="../Controls/MessageBox.ascx" tagname="MessageBox" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavContent" Runat="Server">
    <uc1:ThemesNav ID="ThemesNav1" runat="server" />
    <div class="padded center"><a href="Themes.aspx" title="Back to Themes" class="btn">        
        <b>&laquo; Back to Themes</b></a></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">    
    <uc2:MessageBox ID="MessageBox1" runat="server" />    
    <h1>Theme Content Columns</h1>    
    <asp:LinkButton CssClass="btn" ID="btnCopyFromStore" 
        runat="server" 
        onclick="btnCopyFromStore_Click" ><b>Copy Columns &raquo; Theme</b></asp:LinkButton><br />
    &nbsp;<br />
    <asp:LinkButton CssClass="btn" ID="btnClear" 
        runat="server" onclick="btnClear_Click"><b>Clear Column Settings from Theme</b></asp:LinkButton><br />
    &nbsp;<br />
    <asp:LinkButton CssClass="btn" ID="btnCopyToStore" 
        runat="server" 
        onclick="btnCopyToStore_Click" ><b>Copy Theme Columns &raquo; Store</b></asp:LinkButton><br />
    &nbsp;<br />            
    <asp:HiddenField ID="themeidfield" ClientIDMode="Static" runat="server" />
</asp:Content>

