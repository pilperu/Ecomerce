<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_Fraud" title="Untitled Page" Codebehind="Fraud.aspx.cs" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Fraud Screening</h1>
    <p>Any order that matches this information with 5 points or more will be automatically placed on hold.</p>
    <table width="700" border="0" cellspacing="0" cellpadding="3">
        <tr>
            <td class="HeaderStyle2" >
                Emails<br />5 point</td>
            <td class="HeaderStyle2">
                IP Addresses<br />1 point</td>
            <td class="HeaderStyle2">
                Domain Names<br />3 points</td>
            <td class="HeaderStyle2">
                Phone Numbers<br />3 points</td>
            <td class="HeaderStyle2">
                Credit Card Numbers<br />7 points</td>
        </tr>
        <tr>
            <td align="left" valign="top" class="ItemStyle2" style="width: 140px"><asp:Panel ID="pnlEmail" runat="server" DefaultButton="btnNewEmail">
                <asp:TextBox ID="EmailField" runat="server" Columns="16"></asp:TextBox>&nbsp;
                <asp:ImageButton ID="btnNewEmail" runat="server" 
                    ImageUrl="../images/buttons/new.png" onclick="btnNewEmail_Click">
                </asp:ImageButton></asp:Panel></td>
            <td align="left" valign="top" class="ItemStyle2" style="width: 140px"><asp:Panel ID="Panel1" runat="server" DefaultButton="btnNewIP">
                <asp:TextBox ID="IPField" runat="server" Columns="16"></asp:TextBox>&nbsp;
                <asp:ImageButton ID="btnNewIP" runat="server" 
                    ImageUrl="../images/buttons/new.png" onclick="btnNewIP_Click"></asp:ImageButton></asp:Panel></td>
            <td align="left" valign="top" class="ItemStyle2" style="width: 140px"><asp:Panel ID="Panel2" runat="server" DefaultButton="btnNewDomain">
                <asp:TextBox ID="DomainField" runat="server" Columns="16"></asp:TextBox>&nbsp;
                <asp:ImageButton ID="btnNewDomain" runat="server" 
                    ImageUrl="../images/buttons/new.png" onclick="btnNewDomain_Click">
                </asp:ImageButton></asp:Panel></td>
            <td align="left" valign="top" class="ItemStyle2" style="width: 140px"><asp:Panel ID="Panel3" runat="server" DefaultButton="btnNewPhoneNumber">
                <asp:TextBox ID="PhoneNumberField" runat="server" Columns="16"></asp:TextBox>&nbsp;
                <asp:ImageButton ID="btnNewPhoneNumber" runat="server" 
                    ImageUrl="../images/buttons/new.png" onclick="btnNewPhoneNumber_Click">
                </asp:ImageButton></asp:Panel></td>
            <td align="left" valign="top" class="ItemStyle2" style="width: 140px"><asp:Panel ID="Panel4" runat="server" DefaultButton="btnNewCCNumber">
                <asp:TextBox ID="CreditCardField" runat="server" Columns="16"></asp:TextBox>&nbsp;
                <asp:ImageButton ID="btnNewCCNumber" runat="server" 
                    ImageUrl="../images/buttons/new.png" onclick="btnNewCCNumber_Click">
                </asp:ImageButton></asp:Panel></td>
        </tr>
        <tr>
            <td align="center" valign="top" class="AlternateItemStyle2" style="width: 140px">
                <asp:ListBox ID="lstEmail" runat="server" Rows="16" Width="140px" SelectionMode="Multiple">
                </asp:ListBox></td>
            <td align="center" valign="top" class="AlternateItemStyle2" style="width: 140px">
                <asp:ListBox ID="lstIP" runat="server" Rows="16" Width="140px" SelectionMode="Multiple">
                </asp:ListBox></td>
            <td align="center" valign="top" class="AlternateItemStyle2" style="width: 140px">
                <asp:ListBox ID="lstDomain" runat="server" Rows="16" Width="140px" SelectionMode="Multiple">
                </asp:ListBox></td>
            <td align="center" valign="top" class="AlternateItemStyle2" style="width: 140px">
                <asp:ListBox ID="lstPhoneNumber" runat="server" Rows="16" Width="140px" SelectionMode="Multiple">
                </asp:ListBox></td>
            <td align="center" valign="top" class="AlternateItemStyle2" style="width: 140px">
                <asp:ListBox ID="lstCreditCard" runat="server" Rows="16" Width="140px" SelectionMode="Multiple">
                </asp:ListBox></td>
        </tr>
        <tr>
            <td align="center" valign="top" class="ItemStyle2" style="width: 140px">
                <asp:ImageButton ID="btnDeleteEmail" runat="server" 
                    ImageUrl="../images/buttons/delete.png" onclick="btnDeleteEmail_Click">
                </asp:ImageButton></td>
            <td align="center" valign="top" class="ItemStyle2" style="width: 140px">
                <asp:ImageButton ID="btnDeleteIP" runat="server" 
                    ImageUrl="../images/buttons/delete.png" onclick="btnDeleteIP_Click">
                </asp:ImageButton></td>
            <td align="center" valign="top" class="ItemStyle2" style="width: 140px">
                <asp:ImageButton ID="btnDeleteDomain" runat="server" 
                    ImageUrl="../images/buttons/delete.png" onclick="btnDeleteDomain_Click">
                </asp:ImageButton></td>
            <td align="center" valign="top" class="ItemStyle2" style="width: 140px">
                <asp:ImageButton ID="btnDeletePhoneNumber" runat="server" 
                    ImageUrl="../images/buttons/delete.png" onclick="btnDeletePhoneNumber_Click">
                </asp:ImageButton></td>
            <td align="center" valign="top" class="ItemStyle2" style="width: 140px">
                <asp:ImageButton ID="btnDeleteCCNumber" runat="server" 
                    ImageUrl="../images/buttons/delete.png" onclick="btnDeleteCCNumber_Click">
                </asp:ImageButton></td>
        </tr>
    </table>
</asp:Content>

