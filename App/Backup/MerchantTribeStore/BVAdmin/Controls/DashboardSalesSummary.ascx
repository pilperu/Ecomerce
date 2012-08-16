<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Controls_DashboardSalesSummary" Codebehind="DashboardSalesSummary.ascx.cs" %>
<img src="WeeklySalesChart.aspx" width="600" height="200" alt="weekly sales chart" />
<table class="salessummary">
<tr>
    <td class="axis">&nbsp;</td>
    <td class="axis">This Week</td>
    <td class="axis">Last Week</td>
    <td class="axis">Change</td>
</tr>
<tr class="alternaterow">
    <td class="axis">Sunday</td>
    <td><asp:Literal ID="litY" runat="server" EnableViewState="false" /></td>
    <td><asp:Literal ID="litYL" runat="server" EnableViewState="false" /></td>
    <td><asp:Literal ID="litYC" runat="server" EnableViewState="false" /></td>
</tr>
<tr class="row">
    <td class="axis">Monday</td>
    <td><asp:Literal ID="litM" runat="server" EnableViewState="false" /></td>
    <td><asp:Literal ID="litML" runat="server" EnableViewState="false" /></td>
    <td><asp:Literal ID="litMC" runat="server" EnableViewState="false" /></td>
</tr>
<tr class="alternaterow">
    <td class="axis">Tuesday</td>
    <td><asp:Literal ID="litT" runat="server" EnableViewState="false" /></td>
    <td><asp:Literal ID="litTL" runat="server" EnableViewState="false" /></td>
    <td><asp:Literal ID="litTC" runat="server" EnableViewState="false" /></td>
</tr>
<tr class="row">
    <td class="axis">Wednesday</td>
    <td><asp:Literal ID="litW" runat="server" EnableViewState="false" /></td>
    <td><asp:Literal ID="litWL" runat="server" EnableViewState="false" /></td>
    <td><asp:Literal ID="litWC" runat="server" EnableViewState="false" /></td>
</tr>
<tr class="alternaterow">
    <td class="axis">Thursday</td>
    <td><asp:Literal ID="litR" runat="server" EnableViewState="false" /></td>
    <td><asp:Literal ID="litRL" runat="server" EnableViewState="false" /></td>
    <td><asp:Literal ID="litRC" runat="server" EnableViewState="false" /></td>
</tr>
<tr class="row">
    <td class="axis">Friday</td>
    <td><asp:Literal ID="litF" runat="server" EnableViewState="false" /></td>
    <td><asp:Literal ID="litFL" runat="server" EnableViewState="false" /></td>
    <td><asp:Literal ID="litFC" runat="server" EnableViewState="false" /></td>
</tr>
<tr class="alternaterow">
    <td class="axis">Saturday</td>
    <td><asp:Literal ID="litS" runat="server" EnableViewState="false" /></td>
    <td><asp:Literal ID="litSL" runat="server" EnableViewState="false" /></td>
    <td><asp:Literal ID="litSC" runat="server" EnableViewState="false" /></td>
</tr>
<tr>                                
    <td class="axis">Weekly Total</td>
    <td class="totals"><asp:Literal ID="litWeek" runat="server" EnableViewState="false" /></td>
    <td class="totals"><asp:Literal ID="litWeekL" runat="server" EnableViewState="false" /></td>
    <td class="totals"><asp:Literal ID="litWeekC" runat="server" EnableViewState="false" /></td>
</tr>
</table>

