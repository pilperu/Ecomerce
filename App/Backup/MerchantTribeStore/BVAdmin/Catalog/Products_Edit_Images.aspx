<%@ Page MasterPageFile="~/BVAdmin/BVAdminNav.master" ValidateRequest="False"
    Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.products_products_edit_images" Codebehind="Products_Edit_Images.aspx.cs" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register src="../Controls/ProductEditMenu.ascx" tagname="ProductEditMenu" tagprefix="uc5" %>
<%@ Register src="../Controls/ProductEditingDisplay.ascx" tagname="ProductEditing" tagprefix="uc5" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc5:ProductEditMenu ID="ProductNavigator" runat="server" />
    <uc5:ProductEditing ID="ProductEditing1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">    
    <h1>Additional Images</h1>
    <uc1:MessageBox ID="MessageBox1" runat="server" />
    <asp:Panel ID="pnlMain" runat="server" Visible="True">
        <div class="padded">
            <asp:FileUpload ID="imgupload" runat="server" ClientIDMode="Static" Columns="40" /> 
            <asp:LinkButton CssClass="btn" ID="btnAdd" runat="server" 
                Text="<b>Add New Image</b>" AlternateText="Add New Image" 
                onclick="btnAdd_Click"  />
        </div>&nbsp;
        <asp:Literal ID="litImages" runat="server" EnableViewState="false" ></asp:Literal>
    </asp:Panel>
    <asp:HiddenField ID="ProductIdField" runat="server" />
</asp:Content>
