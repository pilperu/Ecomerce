<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="true" CodeBehind="SocialMedia.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.Configuration.SocialMedia" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<h1>Social Media Settings</h1>
<uc1:MessageBox ID="MessageBox1" runat="server" />
        <asp:Label id="lblError" runat="server" CssClass="errormessage"></asp:Label>
        <asp:Panel ID="pnlMain" runat="server" DefaultButton="btnSave">
        <table border="0" cellspacing="0" cellpadding="3">
        <tr>
            <td class="formlabel">Use FaceBook:</td>
            <td class="formfield">
                <asp:CheckBox ID="chkUseFaceBook" runat="server" /></td>
        </tr>
        <tr>
            <td class="formlabel">FaceBook Application Id:</td>
            <td class="formfield">
                <asp:TextBox ID="FaceBookAppIdField" Columns="50" Width="300px" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="formlabel">FaceBook Admins:</td>
            <td class="formfield">
                <asp:TextBox ID="FaceBookAdminsField" Columns="50" Width="300px" runat="server"></asp:TextBox> (USER1,USER2,etc...)</td>
        </tr>
        <tr>
                        <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td class="formlabel">Use Twitter</td>
            <td class="formfield">
                <asp:CheckBox ID="chkUseTwitter" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="formlabel">Twitter Handle (username):</td>
            <td class="formfield">
                @<asp:TextBox ID="TwitterHandleField" Columns="50" Width="300px" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="formlabel">Default Tweet Text:</td>
            <td class="formfield">                
                <asp:TextBox ID="DefaultTweetTextField" TextMode="MultiLine" Columns="50" Rows="3" MaxLength="140" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
                     <td colspan="2">&nbsp;</td>
        </tr>         
        <tr>
            <td class="formlabel">Use Google Plus:</td>
            <td class="formfield">
                <asp:CheckBox ID="chkUseGooglePlus" runat="server" /></td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>  <td>&nbsp;</td>
                <td class="formfield"><asp:ImageButton ID="btnSave" CausesValidation="true"
                            runat="server" ImageUrl="../images/buttons/SaveChanges.png" 
                        onclick="btnSave_Click"></asp:ImageButton></td>
        </tr>
        </table>
        </asp:Panel>
</asp:Content>


