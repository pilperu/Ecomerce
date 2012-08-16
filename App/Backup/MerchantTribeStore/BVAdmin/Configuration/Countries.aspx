<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_Countries" Title="Untitled Page" Codebehind="Countries.aspx.cs" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<%@ Register src="../Controls/MessageBox.ascx" tagname="MessageBox" tagprefix="uc1" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <uc1:MessageBox ID="MessageBox1" runat="server" />
    <h1>
        Active Countries</h1>   
    <asp:DropDownList id="lstActive" runat="server"></asp:DropDownList> 
    <br />
    <asp:LinkButton ID="btnDisabled" runat="server" CssClass="btn" 
        Text="<b>Disable Country</b>" onclick="btnDisabled_Click"></asp:LinkButton>
    <br />&nbsp;
    <br />&nbsp;
    <br />&nbsp;
    <br />
    <h1>Disabled Countries</h1>
    <asp:ListBox ID="lstDisabled" runat="server" SelectionMode="Single" Rows="10"></asp:ListBox> 
    <br />
    <asp:LinkButton ID="btnEnable" runat="server" CssClass="btn" 
        Text="<b>Enable Country</b>" onclick="btnEnable_Click"></asp:LinkButton>
</asp:Content>
