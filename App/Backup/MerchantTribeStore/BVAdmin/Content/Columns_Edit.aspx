<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Content_Columns_Edit" Title="Untitled Page" Codebehind="Columns_Edit.aspx.cs" %>

<%@ Register Src="../Controls/ContentColumnEditor.ascx" TagName="ContentColumnEditor"
    TagPrefix="uc2" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>
        Edit Column </h1>
    <uc1:MessageBox ID="msg" runat="server" EnableViewState="false" />    
    <uc2:ContentColumnEditor ID="ContentColumnEditor" runat="server" />
    &nbsp;
    <div style="text-align: right;">
        <asp:ImageButton ID="btnOk" runat="server" AlternateText="Back to Column List" 
            ImageUrl="~/BVAdmin/Images/Buttons/OK.png" onclick="btnOk_Click" />
    </div>
    
    <div style="margin: 50px 0px 10px 0px; text-align: left; width: 475px;">
        <h2>
            Advanced Options</h2><asp:Panel ID="pnlAdvanced" runat="server" DefaultButton="btnClone">        
            Copy To:&nbsp;<asp:DropDownList ID="CopyToList" runat="server" Width="350px">
            </asp:DropDownList>
            <asp:ImageButton ID="btnCopyBlocks" runat="server" 
                ImageUrl="~/BVAdmin/Images/Buttons/Go.png" onclick="btnCopyBlocks_Click" /><br />
            Clone As:
            <asp:TextBox ID="CloneNameField" runat="server" Columns="20" Width="345px"></asp:TextBox>&nbsp;<asp:ImageButton
                ID="btnClone" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Go.png" 
                onclick="btnClone_Click" />
        </asp:Panel>
    </div>
</asp:Content>
