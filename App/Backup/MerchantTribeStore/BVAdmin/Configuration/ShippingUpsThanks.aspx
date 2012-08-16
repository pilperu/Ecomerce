<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="true" CodeBehind="ShippingUpsThanks.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.Configuration.ShippingUpsThanks" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        UPS OnLine<sup>®</sup> Tools Account Created!</h1>
    <table cellspacing="0" border="0" cellpadding="5">
        <tr>
            <td class="formlabel" valign="top" align="left">
                <br />
                &nbsp;<br />
                Your registration is complete!</td>
        </tr>
        <tr>
            <td class="formlabel" valign="top" align="left">
                <asp:ImageButton ID="btnContinue" runat="server" 
                    ImageUrl="~/BVAdmin/images/buttons/continue.png" onclick="btnContinue_Click">
                </asp:ImageButton></td>
        </tr>
        <tr>
            <td class="formlabel">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
