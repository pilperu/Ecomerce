<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="true" CodeBehind="ChangeEmail.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.ChangeEmail" %>
<%@ Register src="Controls/MessageBox.ascx" tagname="MessageBox" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<uc1:MessageBox ID="MessageBox1" runat="server" />
<h1>Change Administrator Email</h1>
<table>
<tr>
    <td class="formlabel">Current Email:</td>
    <td class="formfield"><asp:Literal ID="litCurrentEmail" runat="server"></asp:Literal></td>
</tr>
<tr>
    <td class="formlabel">New Email:</td>
    <td class="formfield"><asp:TextBox ID="NewEmailField" runat="server" Columns="50"></asp:TextBox></td>
</tr>
<tr>
    <td class="formlabel">&nbsp;</td>
    <td class="formfield"><a href="Account.aspx">Close</a>
    &nbsp;<asp:ImageButton ID="btnSave" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" 
            AlternateText="Save Changes" onclick="btnSave_Click" /></td>
</tr>
</table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PreBodyCloseContent" runat="server">
</asp:Content>
