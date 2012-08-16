<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_Email" title="Untitled Page" Codebehind="Email.aspx.cs" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Email Addresses</h1>    
    <uc1:MessageBox ID="MessageBox1" runat="server" />
    <asp:Panel ID="pnlMain" runat="server" DefaultButton="btnSave">
    <table border="0" cellspacing="0" cellpadding="3">        
        <tr>
            <td class="formlabel">Send General Email To:</td>
            <td class="formfield">
                <asp:TextBox ID="ContactEmailField" Columns="40" Width="300px" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="EmailAddressRequiredFieldValidator" runat="server" ControlToValidate="ContactEmailField" EnableClientScript="True" ErrorMessage="E-mail Address Is Required." ForeColor=" " CssClass="errormessage" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="BVRegularExpressionValidator1" ForeColor=" " CssClass="errormessage"
                    runat="server" ControlToValidate="ContactEmailField" Display="Dynamic" ErrorMessage="Please enter a valid email address"
                    ValidationExpression="^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$"></asp:RegularExpressionValidator>    
            </td>
        </tr>
        <tr>
            <td class="formlabel">Send New Order Notices To:</td>
            <td class="formfield">
                <asp:TextBox Columns="40" runat="server" ID="OrderNotificationEmailField" 
                    Width="300px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="BVRequiredFieldValidator3" runat="server" ControlToValidate="OrderNotificationEmailField" EnableClientScript="True" ErrorMessage="E-mail Address Is Required." ForeColor=" " CssClass="errormessage" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="BVRegularExpressionValidator4" ForeColor=" " CssClass="errormessage"
                    runat="server" ControlToValidate="OrderNotificationEmailField" Display="Dynamic" ErrorMessage="Please enter a valid email address"
                    ValidationExpression="^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$"></asp:RegularExpressionValidator>    
            </td>
        </tr>        
        <tr>
                <td class="formfield"><asp:ImageButton ID="btnSave" CausesValidation="true"
                            runat="server" ImageUrl="../images/buttons/SaveChanges.png" 
                        onclick="btnSave_Click"></asp:ImageButton></td>
        </tr>
     </table>
     </asp:Panel>
</asp:Content>

