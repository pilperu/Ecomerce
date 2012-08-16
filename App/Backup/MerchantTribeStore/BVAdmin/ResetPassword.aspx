<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.ResetPassword" %>

<%@ Register Src="Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Login</title>
    <link href="<%=Page.ResolveUrl("~/bvadmin/styles.css") %>" rel="stylesheet" type="text/css" />     
</head>
<body>
    <form id="form1" runat="server">
        <div id="wrapper"><div class="topper"></div>
        <asp:Panel runat="server" ID="PanelMain">
            <uc1:MessageBox ID="MessageBox1" runat="server" />
            <div id="login">                   
                    <h1>Reset Password</h1>
                   <table cellspacing="0" cellpadding="3">
                    <tr>
                        <td class="formlabel">
                            Email:
                        </td>
                        <td class="formfield">
                            <asp:TextBox ID="UsernameField" runat="server" ToolTip="Username" Width="200px"></asp:TextBox></td>
                    </tr>                    
                    <tr>
                        <td class="formlabel">&nbsp;</td>
                        <td class="formfield">
                            <asp:LinkButton ID="lnkReset" runat="server" Text="<b>Reset Password</b>" 
                                CssClass="btn" onclick="lnkReset_Click"></asp:LinkButton>
                        </td>
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
