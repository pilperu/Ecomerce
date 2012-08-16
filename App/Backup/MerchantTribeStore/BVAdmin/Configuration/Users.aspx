<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_Users" title="Untitled Page" Codebehind="Users.aspx.cs" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
 <h1>Customer Settings</h1>
        <uc1:MessageBox ID="MessageBox1" runat="server" />
        <asp:Label id="lblError" runat="server" CssClass="errormessage"></asp:Label>
        <asp:Panel ID="pnlMain" runat="server" DefaultButton="btnSave">
        <table border="0" cellspacing="0" cellpadding="3">
        <tr>
            <td class="formlabel">Private Store:</td>
            <td class="formfield">
                <asp:CheckBox ID="chkPrivateStore" runat="server" /></td>
        </tr>
        <tr>
            <td class="formlabel">Remember Customer Passwords:</td>
            <td class="formfield">
                <asp:CheckBox ID="chkRememberUserPasswords" runat="Server" /></td>
        </tr>
        <tr>
                <td class="formlabel">
                    <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="../images/buttons/Cancel.png"
                        CausesValidation="False" onclick="btnCancel_Click"></asp:ImageButton></td>
                <td class="formfield"><asp:ImageButton ID="btnSave" CausesValidation="true"
                            runat="server" ImageUrl="../images/buttons/SaveChanges.png" 
                        onclick="btnSave_Click"></asp:ImageButton></td>
            </tr>
        </table>
        </asp:Panel>
</asp:Content>

