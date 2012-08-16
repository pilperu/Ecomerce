<%@ Page ValidateRequest="false" Title="Edit Product Tab" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Catalog_ProductsEdit_TabsEdit" Codebehind="ProductsEdit_TabsEdit.aspx.cs" %>
<%@ Register src="../Controls/ProductEditMenu.ascx" tagname="ProductEditMenu" tagprefix="uc5" %>
<%@ Register src="../Controls/ProductEditingDisplay.ascx" tagname="ProductEditing" tagprefix="uc5" %>
<%@ Register src="../Controls/OptionItemEditor.ascx" tagname="OptionItemEditor" tagprefix="uc3" %>
<%@ Register Src="../Controls/HtmlEditor.ascx" TagName="HtmlEditor" TagPrefix="uc1" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc5:ProductEditMenu ID="ProductNavigator" runat="server" />
    <uc5:ProductEditing ID="ProductEditing1" runat="server" />
</asp:Content>
    
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc2:MessageBox ID="MessageBox1" runat="server" />
    <h1>Edit Product Tab</h1>
    <div class="controlarea1">
    <table border="0" cellspacing="0" cellpadding="0" class="formtable">
    <tr>
        <td class="formlabel">Tab Title:</td>
        <td><asp:TextBox id="TabTitleField" runat="server" width="300px"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="formlabel">Html:</td>
        <td><asp:TextBox ID="HtmlDataField" runat="server" Width="500" Height="350" TextMode="MultiLine" Wrap="false"></asp:TextBox></td>
    </tr>  
    </table>
    </div>
    <div class="editorcontrols">        
        <a href="ProductsEdit_Tabs.aspx?id=<%=productBvin %>">Close</a> 
        &nbsp;<asp:ImageButton id="btnSaveOption" ClientIDMode="Static" runat="server" 
            ImageUrl="../images/buttons/SaveChanges.png" 
            onclick="btnSave_Click" /> 
        &nbsp;<asp:ImageButton id="btnSaveAndClose" ClientIDMode="Static" 
            runat="server" ImageUrl="../images/buttons/SaveAndClose.png" 
            onclick="btnSaveAndClose_Click" />
    </div>

</asp:Content>
