<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Catalog_ProductChoices_Variants" Codebehind="ProductChoices_Variants.aspx.cs" %>

<%@ Register Src="../Controls/ProductEditMenu.ascx" TagName="ProductEditMenu" TagPrefix="uc5" %>
<%@ Register Src="../Controls/ProductEditingDisplay.ascx" TagName="ProductEditing"
    TagPrefix="uc5" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc5:ProductEditMenu ID="ProductNavigator" runat="server" />
    <uc5:ProductEditing ID="ProductEditing1" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <uc1:MessageBox ID="MessageBox1" runat="server" />
    <h1>Choices - Variants</h1>
    <div class="controlarea1">
        <table>
            <tr>
                <td><asp:PlaceHolder ID="phLists" runat="server"></asp:PlaceHolder></td>
                <td><asp:ImageButton ID="btnNew" runat="server" 
                        ImageUrl="~/bvadmin/images/buttons/New.png" AlternateText="Add New Variant" 
                        onclick="btnNew_Click" /></td>
                <td><asp:ImageButton ID="btnGenerateAll" runat="server" 
                        ImageUrl="~/bvadmin/images/buttons/CreateAllVariants.png" 
                        AlternateText="Generate All Possible Variants" onclick="btnGenerateAll_Click" /></td>
            </tr>
        </table>
    </div>
    &nbsp;    
    <div id="litVariants"></div>
    <div class="modal controlarea1">
        <h1>Edit Variant</h1>
        <table class="formtable">
        <tr>
            <td class="formfield" colspan="2" id="dialogdescription"></td>    
        </tr>
        <tr>
            <td class="formlabel">Sku:</td>
            <td class="formfield"><asp:TextBox ID="dialogsku" ClientIDMode="Static" runat="server" Width="220" /></td>
        </tr>
        <tr>
            <td class="formlabel">Price:</td>
            <td class="formfield"><asp:TextBox ID="dialogprice" ClientIDMode="Static" runat="server" Width="220" /></td>
        </tr>
        <tr>
            <td class="formlabel">Picture:</td>
            <td class="formfield"><img id="dialogimg" src="" width="220" border="0" /><br />
            <asp:FileUpload ID="dialognewFile" ClientIDMode="Static" runat="server" />
        </td>
        </tr>
        </table>
        <div class="editorcontrols">
            <a href="#" id="dialogclose">Close</a> <asp:ImageButton ID="btnSave" 
                runat="server" ImageUrl="~/bvadmin/images/buttons/SaveChanges.png" 
                AlternateText="Save Changes" onclick="btnSave_Click" />
        </div>
        <asp:HiddenField ID="dialogbvin" ClientIDMode="Static" runat="server" />        
</div>

</asp:Content>
