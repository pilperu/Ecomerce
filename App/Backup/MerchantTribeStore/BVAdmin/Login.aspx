<%@ Page Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Login" Codebehind="Login.aspx.cs" %>

<%@ Register Src="Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="<%=Page.ResolveUrl("~/bvadmin/styles.css") %>" rel="stylesheet" type="text/css" />
     <script type="text/javascript" src="../Bvc.js" language="javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="wrapper"><div class="topper"></div>
        <asp:Panel runat="server" ID="PanelMain" DefaultButton="btnLogin">
            <uc1:MessageBox ID="MessageBox1" runat="server" />
            <div id="login">                   
                   <table cellspacing="0" cellpadding="3">
                    <tr>
                        <td class="formlabel">
                            Email:
                        </td>
                        <td class="formfield">
                            <asp:TextBox ID="UsernameField" runat="server" ToolTip="Username" Width="200px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="formlabel">
                            Password:
                        </td>
                        <td class="formfield">
                            <asp:TextBox ID="PasswordField" runat="server" TextMode="Password" ToolTip="Username"
                                Width="200px"></asp:TextBox><br />
                                <asp:HyperLink ID="lnkForgot" runat="server" EnableViewState="false" NavigateUrl="~/adminaccount/resetpassword">Forgot Password? Need to Reset?</asp:HyperLink></td>
                    </tr>
                    <tr>
                        <td class="formlabel">&nbsp;</td>
                        <td class="formfield" colspan="2">
                            <asp:ImageButton ID="btnLogin" runat="server" AlternateText="Login" ImageUrl="~/BVAdmin/Images/Buttons/Login.png"
                                ToolTip="Login" onclick="btnLogin_Click" /></td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <div class="bottom"></div>
        </div>
        <asp:HiddenField ID="RedirectToField" runat="server" />        
    </form>
</body>
</html>
