<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="true" CodeBehind="FileDownloads.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.Catalog.FileDownloads" %>
<%@ Register src="../Controls/ProductEditMenu.ascx" tagname="ProductEditMenu" tagprefix="uc5" %>
<%@ Register src="../Controls/ProductEditingDisplay.ascx" tagname="ProductEditing" tagprefix="uc5" %>
<%@ Register src="../Controls/MessageBox.ascx" tagname="MessageBox" tagprefix="uc1" %>
<%@ Register Src="../Controls/FilePicker.ascx" TagName="FilePicker" TagPrefix="uc2" %>

<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc5:ProductEditMenu ID="ProductNavigator" runat="server" />
    <uc5:ProductEditing ID="ProductEditing1" runat="server" />    
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<uc1:MessageBox ID="MessageBox1" runat="server" />
<h1>File Downloads</h1>
<uc2:FilePicker ID="FilePicker1" runat="server" /> 
    <asp:ImageButton ID="AddFileButton" runat="server" 
        ImageUrl="~/BVAdmin/Images/Buttons/AddFile.png" onclick="AddFileButton_Click" /><br />
    &nbsp;<br />
   &nbsp;<br />
   <h3>Files Associated With this Product</h3>
    <asp:GridView ID="FileGrid" runat="server" AutoGenerateColumns="False" 
        ShowHeader="False" GridLines="none" Borderwidth="0px" DataKeyNames="Bvin" 
        onrowdeleting="FileGrid_RowDeleting" 
        onrowediting="FileGrid_RowEditing">
        <Columns>
            <asp:BoundField DataField="ShortDescription" />
            <asp:ButtonField ButtonType="Image" ImageUrl="~/BVAdmin/Images/Buttons/Delete.png"
                Text="Button" CommandName="Delete" />
            <asp:ButtonField ButtonType="Image" ImageUrl="~/BVAdmin/Images/Buttons/Download.png"
                Text="Button" CommandName="Edit"  />
        </Columns>
        <RowStyle CssClass="row" />
        <HeaderStyle CssClass="rowheader" />
        <AlternatingRowStyle CssClass="alternaterow" />
    </asp:GridView>
</asp:Content>
