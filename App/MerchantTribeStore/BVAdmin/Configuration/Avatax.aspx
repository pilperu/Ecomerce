<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="true" CodeBehind="Avatax.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.Configuration.Avatax" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <uc1:MessageBox ID="msg" runat="server" />
    <h1>Avalara Avatax Configuration</h1>    
                
    <asp:Panel ID="pnlMain" runat="server" DefaultButton="btnSave">
    <table border="0" cellspacing="0" cellpadding="3">
    <tr>
        <td class="formlabel">Enable Avalara:</td>
        <td class="formfield">
            <asp:CheckBox ID="EnableCheckBox" runat="server" />   
        </td>
    </tr>
    <tr>
        <td class="formlabel">Account:</td>
        <td class="formfield">
            <asp:TextBox ID="AccountTextBox" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="formlabel">License Key:</td>
        <td class="formfield">
            <asp:TextBox ID="LicenseKeyTextBox" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="formlabel">UserName:</td>
        <td class="formfield">
            <asp:TextBox ID="UserNameField" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="formlabel">Password:</td>
        <td class="formfield">
            <asp:TextBox TextMode="Password" ID="PasswordField" runat="server"></asp:TextBox>
        </td>
    </tr>
    
    <tr>
        <td class="formlabel">Company Code (leave blank if you just have one default company):</td>
        <td class="formfield">
            <asp:TextBox ID="CompanyCodeTextBox" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="formlabel">Url:</td>
        <td class="formfield">
            <asp:TextBox ID="UrlTextBox" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="formlabel">Debug Mode:</td>
        <td class="formfield">
            <asp:CheckBox ID="DebugCheckBox" runat="server" />
        </td>
    </tr>
    <tr>
            <td class="formlabel">
                <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="../images/buttons/Cancel.png"
                    CausesValidation="False" OnClick="btnCancel_Click"></asp:ImageButton></td>
            <td class="formfield"><asp:ImageButton ID="btnSave" CausesValidation="true"
                        runat="server" ImageUrl="../images/buttons/SaveChanges.png" OnClick="btnSave_Click"></asp:ImageButton></td>
        </tr>
    </table>
    </asp:Panel>
</asp:Content>
