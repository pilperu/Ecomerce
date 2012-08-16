<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Catalog_ProductChoices" Codebehind="ProductChoices.aspx.cs" %>
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
    <h1>Choices - Edit</h1>        
    <asp:Literal id="litResults" ClientIDMode="Static" runat="server" EnableViewState="false"></asp:Literal>                    
    &nbsp;<br />
    <table class="formtable controlarea2 padded">
    <tr>
        <td><asp:DropDownList ID="ChoiceTypes" runat="server">
        <asp:ListItem Value="100">Drop Down List</asp:ListItem>
        <asp:ListItem Value="200">Radio Button List</asp:ListItem>
        <asp:ListItem Value="300">Checkboxes</asp:ListItem>
        <asp:ListItem Value="400">Html Description</asp:ListItem>
        <asp:ListItem Value="500">Text Input</asp:ListItem>    
    </asp:DropDownList></td>
    <td><asp:ImageButton ID="NewChoiceButton" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/New.png" onclick="NewChoiceButton_Click" /></td>
    </tr>
    </table>
        
</asp:Content>

