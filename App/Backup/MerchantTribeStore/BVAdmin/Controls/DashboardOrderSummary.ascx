<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Controls_DashboardOrderSummary" Codebehind="DashboardOrderSummary.ascx.cs" %>
<a class="ordersummary" id="ordernew" href="orders/default.aspx?p=1&mode=1"><span><asp:Literal ID="litNewCount" runat="server" EnableViewState="false" /></span></a>
<a class="ordersummary" id="orderhold" href="orders/default.aspx?p=1&mode=5"><span><asp:Literal ID="litHoldCount" runat="server" EnableViewState="false" /></span></a>
<a class="ordersummary" id="orderpayment" href="orders/default.aspx?p=1&mode=2"><span><asp:Literal ID="litPaymentCount" runat="server" EnableViewState="false" /></span></a>
<a class="ordersummary" id="ordershipping" href="orders/default.aspx?p=1&mode=3"><span><asp:Literal ID="litShippingCount" runat="server" EnableViewState="false" /></span></a>
