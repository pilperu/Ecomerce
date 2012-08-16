<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_Affiliates" title="Untitled Page" Codebehind="Affiliates.aspx.cs" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Affiliates</h1>        
        <uc1:MessageBox ID="MessageBox1" runat="server" />
        <asp:Label id="lblError" runat="server" CssClass="errormessage"></asp:Label>
        <asp:Panel ID="pnlMain" runat="server" DefaultButton="btnSave">
        <table border="0" cellspacing="0" cellpadding="3">
        <tr>
            <td class="formlabel">Default Commission:</td>
            <td class="formfield">
                 <asp:DropDownList ID="lstCommissionType" runat="server">
                    <asp:ListItem Value="1">Percentage of Sale</asp:ListItem>
                    <asp:ListItem Value="2">Flat Rate Commission</asp:ListItem>
                </asp:DropDownList>&nbsp;
                <asp:TextBox ID="AffiliateCommissionAmountField" Columns="30" Width="200px" runat="server"></asp:TextBox></td>
        </tr>     
        <tr>
            <td class="formlabel">Default Referral Days:</td>
            <td class="formfield">
                <asp:TextBox ID="AffiliateReferralDays" Columns="5" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="formlabel">Conflict Resolution Mode:</td>
            <td class="formfield"><asp:DropDownList ID="AffiliateConflictModeField" runat="server">
                <asp:ListItem Value="1">Favor Older Affiliate</asp:ListItem>
                <asp:ListItem Value="2">Favor Newer Affiliate</asp:ListItem>
            </asp:DropDownList></td>
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

