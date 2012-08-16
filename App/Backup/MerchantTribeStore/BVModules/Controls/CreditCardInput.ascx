<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_Controls_CreditCardInput" Codebehind="CreditCardInput.ascx.cs" %>
<div class="creditcardinput">
    <table border="0" cellspacing="0" cellpadding="2">
        <tr>
            <td>&nbsp;</td>
            <td class="formfield">
            <asp:Literal ID="litCardsAccepted" runat="server" EnableViewState="false"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="formlabel">
                Card Number</td>
            <td class="formfield">
                <span class="creditcardnumber">
                    <asp:TextBox ID="cccardnumber" ClientIDMode="Static" runat="server" Columns="20" MaxLength="20"></asp:TextBox>                    
                </span>
            </td>
        </tr>
        <tr>
            <td class="formlabel">
                Exp. Date</td>
            <td class="formfield">
                <asp:DropDownList ID="ccexpmonth" ClientIDMode="Static" runat="server">
                </asp:DropDownList>                
                &nbsp;/&nbsp;
                <asp:DropDownList ID="ccexpyear" ClientIDMode="Static" runat="server">
                </asp:DropDownList>                
            </td>
        </tr>
        <tr id="issueNumberRow" runat="server" visible="false">
            <td class="formlabel">
                Issue Number</td>
            <td class="formfield">
                <asp:TextBox ID="ccissuenumber" ClientIDMode="Static" runat="server" Columns="5" MaxLength="4"></asp:TextBox>&nbsp;                
            </td>
        </tr>
        <tr>
            <td class="formlabel">
                Security Code</td>
            <td class="formfield">
                <asp:TextBox ID="ccsecuritycode" ClientIDMode="Static" runat="server" Columns="5" MaxLength="4"></asp:TextBox>                
                </td>
        </tr>
        <tr>
            <td class="formlabel">
                Name On Card</td>
            <td class="formfield">
                <asp:TextBox ID="cccardholder" ClientIDMode="Static" runat="server" Columns="20"></asp:TextBox>                
            </td>
        </tr>
    </table>
</div>
