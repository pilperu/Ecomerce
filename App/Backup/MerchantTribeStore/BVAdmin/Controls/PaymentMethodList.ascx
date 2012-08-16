<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Controls_PaymentMethodList" Codebehind="PaymentMethodList.ascx.cs" %>
<div class="PaymentMethodsList">
<asp:DropDownList AutoPostBack="true" ID="lstPaymentMethods" runat="server" 
        onselectedindexchanged="lstPaymentMethods_SelectedIndexChanged"></asp:DropDownList><br />
<asp:PlaceHolder runat="Server" ID="phOptions"></asp:PlaceHolder>
</div>