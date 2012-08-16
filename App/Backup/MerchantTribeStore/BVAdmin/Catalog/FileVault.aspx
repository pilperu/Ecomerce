<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Catalog_FileVault" title="File Vault" Codebehind="FileVault.aspx.cs" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>

<%@ Register Src="../Controls/FilePicker.ascx" TagName="FilePicker" TagPrefix="uc1" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc2:MessageBox ID="MessageBox1" runat="server" />
    <h1>File Vault</h1>
    <div style="float: right;">
        <asp:LinkButton ID="ImportLinkButton" runat="server" CausesValidation="False" 
            onclick="ImportLinkButton_Click">Import Files Already In "Files" Folder</asp:LinkButton>
    </div>
    <div style="clear: both;">
        <table width="100%">
        <thead>
            <tr>
                <th>File Name</th>
                <th>Description</th>
                <th>Product Count</th>
                <th>&nbsp;</th>
                <th>&nbsp;</th>
            </tr>            
        </thead>
        <asp:Literal ID="litFiles" runat="server" EnableViewState="false"></asp:Literal>
        </table>
    </div>
    <div style="margin-top: 20px;">        
        <uc1:FilePicker ID="FilePicker" runat="server" />
        <asp:ImageButton ID="AddNewImageButton" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/AddFile.png" 
            onclick="AddNewImageButton_Click" />        
    </div>
</asp:Content>