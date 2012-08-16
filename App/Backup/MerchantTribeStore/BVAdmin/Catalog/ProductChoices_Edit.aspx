<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Catalog_ProductChoices_Edit" Codebehind="ProductChoices_Edit.aspx.cs" %>
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
    <h1>Edit Choice</h1>
    <div class="controlarea1">
    <table border="0" cellspacing="0" cellpadding="0" class="formtable">
    <tr>
        <td class="formlabel">Name:</td>
        <td><asp:TextBox id="NameField" runat="server" width="300px"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="formlabel">&nbsp;</td>
        <td><asp:CheckBox id="chkHideName" runat="server" /> Hide name on store</td>
    </tr>
    <tr id="trVariant" runat="server" visible="false">
        <td class="formlabel">&nbsp;</td>
        <td><asp:CheckBox id="chkVariant" runat="server" /> This choice changes <i>Inventory</i>, <i>Pictures</i>, <i>Prices</i> and/or <i>SKU</i></td>
    </tr>
    </table>
    </div>
    <asp:MultiView ID="viewMain" runat="server">
        <asp:View ID="viewHtml" runat="server">
            <div class="controlarea1">
                <uc1:HtmlEditor ID="HtmlEditor1" runat="server" EditorWidth="675" EditorHeight="220" EditorWrap="true" />
            </div>
        </asp:View>
        <asp:View ID="viewTextInput" runat="server">
            <div class="controlarea1">
        <table class="formtable">
        <tr>
            <td class="formlabel">Columns:</td>
            <td class="formfield"><asp:TextBox id="ColumnsField" runat="server" Columns="10" /></td>
        </tr>
        <tr>
            <td class="formlabel">Rows:</td>
            <td class="formfield"><asp:TextBox id="RowsField" runat="server" Columns="10" /></td>
        </tr>
        <tr>
            <td class="formlabel">Max Length:</td>
            <td class="formfield"><asp:TextBox id="MaxLengthField" runat="server" Columns="10" /></td>
        </tr>
        </table>
        </div>
        </asp:View>
        <asp:View ID="viewItems" runat="server">
            <div class="padded">
                <h2>Choice Items</h2>
                <uc3:OptionItemEditor ID="ItemsEditor" runat="server" />            
            </div>            
        </asp:View>        
    </asp:MultiView>
    <div class="editorcontrols">
        <a href="ProductChoices.aspx?id=<%=productBvin %>">Close</a> 
        &nbsp;<asp:ImageButton id="btnSaveOption" ClientIDMode="Static" runat="server" 
            ImageUrl="~/bvadmin/images/buttons/SaveChanges.png" 
            onclick="btnSaveOption_Click" /> 
        &nbsp;<asp:ImageButton id="btnSaveAndClose" ClientIDMode="Static" 
            runat="server" ImageUrl="~/bvadmin/images/buttons/SaveAndClose.png" 
            onclick="btnSaveAndClose_Click" />
    </div>

</asp:Content>
