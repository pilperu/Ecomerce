<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Reports_Default" Title="Sales By Date" Codebehind="View.aspx.cs" %>

<%@ Register Src="~/BVAdmin/Controls/DateRangePicker.ascx" TagName="DateRangePicker"
    TagPrefix="uc2" %>
<%@ Register Src="~/BVAdmin/Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>
        Orders Received By Date</h1>
    <div class="flash-message-info">This report shows orders that arrived on a specific date. It does NOT represent the flow of payments for a given day. See the &quot;<a href="../Daily%20Sales/View.aspx">Daily Transaction Report</a>&quot; for actual payments by day.</div>
    <uc1:MessageBox ID="msg" runat="server" />
    &nbsp;
    <table cellspacing="0" cellpadding="0" border="0" id="container" width="100%">
        <tr>
            <td id="AdminContentWithNav">
                <table width="100%" border="0" cellspacing="0" cellpadding="5">
                    <tr>
                        <td align="left" valign="middle" class="FormLabel">
                            <uc2:DateRangePicker ID="DateRangeField" runat="server" RangeType="ThisMonth" />
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ImageButton ImageUrl="~/BVAdmin/Images/Buttons/OK.png" ID="btnShow" runat="server"
                                CausesValidation="False" onclick="btnShow_Click" />
                        </td>
                    </tr>
                </table>
                &nbsp; &nbsp;
                <div style="padding-right: 10px; padding-left: 10px; background: #ffffff; padding-bottom: 10px;
                     padding-top: 10px">
                    <asp:Label ID="lblResponse" Text="" runat="server" CssClass="BVSmallText" /><br>
                    &nbsp;<br>
                    <asp:DataGrid DataKeyField="timeoforderutc" CellPadding="3" BorderWidth="0px" CellSpacing="1"
                        ItemStyle-CssClass="row" ID="dgList" runat="server" AutoGenerateColumns="False"
                        Width="100%" ShowFooter="True" GridLines="None" 
                         onitemdatabound="dgList_ItemDataBound">
                        <HeaderStyle CssClass="rowheader" />
                        <AlternatingItemStyle CssClass="alternaterow"></AlternatingItemStyle>
                        <ItemStyle CssClass="ItemStyle2"></ItemStyle>
                        <FooterStyle CssClass="HeaderStyle2"></FooterStyle>
                        <FooterStyle CssClass="HeaderStyle2"></FooterStyle>
                        <Columns>
                            <asp:TemplateColumn HeaderText="Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblDate" runat="server" 
                                        Text=''></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="OrderNumber" HeaderText="Order #">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TotalOrderBeforeDiscounts" HeaderText="SubTotal" DataFormatString="{0:C}">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TotalOrderDiscounts" HeaderText="Discounts" DataFormatString="{0:C}">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TotalShippingBeforeDiscounts" HeaderText="Shipping" DataFormatString="{0:C}">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TotalShippingDiscounts" HeaderText="Ship Disc." DataFormatString="{0:C}">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TotalTax" HeaderText="Tax" DataFormatString="{0:C}"></asp:BoundColumn>
                            <asp:BoundColumn DataField="TotalGrand" HeaderText="Grand Total" DataFormatString="{0:C}">
                            </asp:BoundColumn>
                            <asp:TemplateColumn>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkViewOrder" runat="server" Text="View Order"></asp:HyperLink>                                    
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle CssClass="FormLabel"></PagerStyle>
                    </asp:DataGrid>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
