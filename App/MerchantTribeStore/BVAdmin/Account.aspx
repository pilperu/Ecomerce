<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Account" Codebehind="Account.aspx.cs" %>

<%@ Register src="../BVModules/Controls/CreditCardInput.ascx" tagname="CreditCardInput" tagprefix="uc1" %>

<%@ Register src="Controls/MessageBox.ascx" tagname="MessageBox" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div style="width:700px;margin:10px auto;">
    <h1>My Account</h1>
    <uc2:MessageBox ID="MessageBox1" runat="server" />
    <div class="editorpanel">
        <h3>Login Information</h3>&nbsp;
        <table class="formtable">
        <tr>
            <td class="formlabel">Email:</td>
            <td class="formfield"><asp:Label ID="lblEmail" runat="server" /></td>
        </tr>
        <tr>
            <td class="formlabel">&nbsp;</td>
            <td class="formfield"><a href="ChangeEmail.aspx" class="btn" ><b>Change Email</b></a>
            &nbsp;<a href="ChangePassword.aspx" class="btn" ><b>Change Password</b></a>
            </td>
        </tr>
        <tr>
            <td class="formlabel">Member Since:</td>
            <td class="formfield"><asp:Label ID="lblMemberSince" runat="server" /></td>
        </tr>
        </table>
    </div>
    &nbsp;
    <div class="editorpanel">
    <h3>Subscription Information</h3>&nbsp;        
    <asp:Literal ID="litStores" runat="server" EnableViewState="false" />        
    </div>
    &nbsp;
    <asp:Panel ID="pnlBilling" runat="server" Visible="true">
    <div class="editorpanel">
    <h3>Update Credit Card</h3>&nbsp;
    <uc1:CreditCardInput ID="CreditCardInput1" runat="server" /><br />
    Billing Zip Code: <asp:textbox ID="txtZipCode" runat="server" Columns="10"></asp:textbox><br />&nbsp;<br />
    <asp:LinkButton ID="btnUpdateCreditCard" runat="server" CssClass="btn" 
            Text="<b>Update Credit Card Information</b>" 
            onclick="btnUpdateCreditCard_Click"></asp:LinkButton>
    </div>
    </asp:Panel>
    <div class="clear"></div>
</div>
</asp:Content>

