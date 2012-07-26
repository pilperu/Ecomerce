<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Catalog_ProductUpSellCrossSell" title="Untitled Page" MaintainScrollPositionOnPostback="true" Codebehind="ProductUpSellCrossSell.aspx.cs" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc3" %>
<%@ Register Src="../Controls/ProductPicker.ascx" TagName="ProductPicker" TagPrefix="uc1" %>
<%@ Register src="../Controls/ProductEditMenu.ascx" tagname="ProductEditMenu" tagprefix="uc5" %>
<%@ Register src="../Controls/ProductEditingDisplay.ascx" tagname="ProductEditing" tagprefix="uc5" %>

<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc5:ProductEditMenu ID="ProductNavigator" runat="server" />
    <uc5:ProductEditing ID="ProductEditing1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<uc3:MessageBox ID="MessageBox1" runat="server" />
<table border="0" width="100%">
<tr>
    <td>
        <h1>Related Items</h1>
        <div class="padded">
            <asp:Literal ID="litItems" runat="server" EnableViewState="false" />
        </div>
    </td>
    <td>
        <h1>Add...</h1>
        <uc1:ProductPicker ID="UpSellsProductPicker" runat="server" />
        <asp:LinkButton ID="btnAdd" runat="server" CssClass="btn" OnClick="btnAdd_Click" Text="<b>&laquo; Add Selected Products</b>"></asp:LinkButton>
    </td>
</tr>
</table>
</asp:Content>

