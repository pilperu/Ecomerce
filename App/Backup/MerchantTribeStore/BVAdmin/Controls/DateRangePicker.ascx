<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Controls_DateRangePicker" Codebehind="DateRangePicker.ascx.cs" %>
<%@ Register Src="DropDownDate.ascx" TagName="DropDownDate" TagPrefix="uc1" %>
<div class="DateRangePicker">
<asp:DropDownList runat="server" ID="lstRangeType" AutoPostBack="True" 
        onselectedindexchanged="lstRangeType_SelectedIndexChanged">
    <asp:ListItem Value="9">All Dates</asp:ListItem>
    <asp:ListItem Value="1">Today</asp:ListItem>
    <asp:ListItem Value="12">Yesterday</asp:ListItem>
    <asp:ListItem Value="2">This Week</asp:ListItem>
    <asp:ListItem Value="3">Last Week</asp:ListItem>
    <asp:ListItem Value="10">This Month</asp:ListItem>
    <asp:ListItem Value="11">Last Month</asp:ListItem>
    <asp:ListItem Value="4">Last 31 Days</asp:ListItem>
    <asp:ListItem Value="5">Last 60 Days</asp:ListItem>
    <asp:ListItem Value="6">Last 120 Days</asp:ListItem>
    <asp:ListItem Value="7">Year To Date</asp:ListItem>
    <asp:ListItem Value="8">Last Year</asp:ListItem>
    <asp:ListItem Value="99">Custom Date</asp:ListItem>
</asp:DropDownList>
<asp:Panel runat="server" ID="pnlCustom" Visible="false">
<div class="DateRangePickerStartDate">Start: <uc1:DropDownDate ID="StartDateField" runat="server" />
</div>
<div class="DateRangePickerEndDate">End: <uc1:DropDownDate ID="EndDateField" runat="server" />
</div>
</asp:Panel>
</div>