<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Content_StoreInfo" title="Untitled Page" Codebehind="StoreInfo.aspx.cs" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>

<%@ Register Src="../Controls/AddressEditor.ascx" TagName="AddressEditor" TagPrefix="uc1" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>    
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc2:MessageBox ID="MessageBox1" runat="server" />
<h1>Store's Address</h1>
<div>
<uc1:AddressEditor ID="ShipFromAddressField" runat="server" />
</div>
<div class="editorcontrols">
  <a href="default.aspx">Close</a>&nbsp;<asp:ImageButton ID="btnSave" 
        runat="server" imageurl="~/BVAdmin/Images/Buttons/SaveChanges.png" 
        AlternateText="Save Changes" onclick="btnSave_Click" />
</div>
</asp:Content>

