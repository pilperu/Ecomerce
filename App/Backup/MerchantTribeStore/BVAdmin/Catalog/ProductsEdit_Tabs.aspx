<%@ Page Title="Product Tabs" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Catalog_ProductsEdit_Tabs" Codebehind="ProductsEdit_Tabs.aspx.cs" %>
<%@ Register src="../Controls/ProductEditMenu.ascx" tagname="ProductEditMenu" tagprefix="uc5" %>
<%@ Register src="../Controls/ProductEditingDisplay.ascx" tagname="ProductEditing" tagprefix="uc5" %>
<%@ Register src="../Controls/MessageBox.ascx" tagname="MessageBox" tagprefix="uc1" %>

<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc5:ProductEditMenu ID="ProductNavigator" runat="server" />
    <uc5:ProductEditing ID="ProductEditing1" runat="server" />    
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" Runat="Server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <uc1:MessageBox ID="MessageBox1" runat="server" />
    <h1>Product Tabs</h1>        
    <asp:Literal id="litResults" ClientIDMode="Static" runat="server" EnableViewState="false"></asp:Literal>                    
    &nbsp;<br />
    <asp:ImageButton ID="NewTabButton" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/New.png" 
        onclick="NewTabButton_Click"/>

</asp:Content>


