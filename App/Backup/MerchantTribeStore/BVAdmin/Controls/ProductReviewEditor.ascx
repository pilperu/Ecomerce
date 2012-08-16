<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Controls_ProductReviewEditor" Codebehind="ProductReviewEditor.ascx.cs" %>
        
<table class="formtable" border="0" cellspacing="0" cellpadding="3">
    <tr>
        <td align="right" valign="top" class="formlabel">
            Product:</td>
        <td align="left" valign="top" class="formfield">
            <asp:Label ID="lblProductName" runat="server" CssClass="BVSmallText">Product Name</asp:Label></td>
    </tr>
    <tr>
        <td align="right" valign="top" class="formlabel">
            User:</td>
        <td align="left" valign="top" class="formfield">
            <asp:Label ID="lblUserName" runat="server" CssClass="BVSmallText">User name</asp:Label></td>
    </tr>
    <tr>
        <td align="right" valign="top" class="formlabel">
            Review Date:</td>
        <td align="left" valign="top" class="formfield">
            <asp:Label ID="lblReviewDate" runat="server" CssClass="BVSmallText">01/01/2004</asp:Label></td>
    </tr>
    <tr>
        <td align="right" valign="top" class="formlabel">
            Rating:</td>
        <td align="left" valign="top" class="formfield">
            <asp:DropDownList ID="lstRating" runat="server">
                <asp:ListItem Value="5">5 Stars</asp:ListItem>
                <asp:ListItem Value="4">4 Stars</asp:ListItem>
                <asp:ListItem Value="3">3 Stars</asp:ListItem>
                <asp:ListItem Value="2">2 Stars</asp:ListItem>
                <asp:ListItem Value="1">1 Stars</asp:ListItem>
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td align="right" valign="top" class="formlabel">
            Karma Score:</td>
        <td align="left" valign="top" class="formfield">
            <asp:TextBox ID="KarmaField" runat="server" CssClass="FormInput" Columns="5">0</asp:TextBox></td>
    </tr>
    <tr>
        <td align="right" valign="top" class="formlabel">
            Approved:</td>
        <td align="left" valign="top" class="formfield">
            <asp:CheckBox ID="chkApproved" runat="server"></asp:CheckBox></td>
    </tr>
    <tr>
        <td align="right" valign="top" class="formlabel">
            Review:</td>
        <td align="left" valign="top" class="formfield">
            <asp:TextBox ID="DescriptionField" runat="server" CssClass="FormInput" Columns="40"
                MaxLength="6000" Rows="6" TextMode="MultiLine"></asp:TextBox></td>
    </tr>
    <tr>
        <td align="right" valign="top" class="formlabel">
            <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="../images/buttons/cancel.png"
                AlternateText="Cancel" CausesValidation="False" onclick="btnCancel_Click"></asp:ImageButton></td>
        <td align="left" valign="top" class="formfield">
            <asp:ImageButton ID="btnOK" runat="server" ImageUrl="../images/buttons/ok.png" 
                AlternateText="OK" onclick="btnOK_Click" CausesValidation="False">
            </asp:ImageButton></td>
    </tr>
</table>
