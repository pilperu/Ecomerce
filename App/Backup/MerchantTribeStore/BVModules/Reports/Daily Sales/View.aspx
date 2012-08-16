<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Reports_Sales_Day" Title="Daily Sales" Codebehind="View.aspx.cs" %>

<%@ Register Src="~/BVAdmin/Controls/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/BVAdmin/Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>
        Daily Transaction Report</h1>
    <asp:Label ID="Instructions" runat="server">Enter a date or select a date from the calendar.  Press the "OK" button after making your selection</asp:Label>
    <br />
    <br />
    <uc1:MessageBox ID="msg" runat="server" />
    <table width="100%" cellspacing="0" border="0" cellpadding="5">
        <tr>
            <td colspan="4" valign="top" class="FormLabel" align="left">
                <asp:ImageButton ID="btnLast" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/left.png"
                    ToolTip="Last" onclick="btnLast_Click" />
            </td>
            <td align="center">
                <uc2:DatePicker ID="DatePicker" runat="server" />
            </td>
                
            <td align="left" valign="top">
                <asp:ImageButton ID="btnShow" runat="server" CausesValidation="False" ImageUrl="~/BVAdmin/Images/Buttons/OK.png"
                    ToolTip="Ok" onclick="btnShow_Click" />
            </td>
            <td colspan="4" align="right" valign="top" class="FormLabel">
                <asp:ImageButton ID="btnNext" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/right.png"
                    ToolTip="Next" onclick="btnNext_Click" />
            </td>
        </tr>
    </table>
    &nbsp;
    <div style="padding-right: 10px; padding-left: 10px; background: #ffffff; padding-bottom: 10px;
        padding-top: 10px">
        <asp:Label ID="lblResponse" Text="" runat="server" CssClass="BVSmallText" /><br />
        &nbsp;
        <br>
        <asp:DataGrid Width="100%" DataKeyField="OrderId" CellPadding="3" 
            BorderWidth="0px" CellSpacing="1" ID="dgList" 
        runat="server" AutoGenerateColumns="False" ShowFooter="True" GridLines="none" 
            oneditcommand="dgList_Edit" onitemdatabound="dgList_ItemDataBound" 
            onpageindexchanged="dgList_PageIndexChanged"> 
            <HeaderStyle CssClass="rowheader" />
            <AlternatingItemStyle CssClass="alternaterow"></AlternatingItemStyle>
            <ItemStyle CssClass="ItemStyle2"></ItemStyle>
            <FooterStyle CssClass="HeaderStyle2"></FooterStyle>
            <Columns>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:Literal ID="litOrderNumber" runat="server"></asp:Literal> | <span class=\"tiny\"><asp:Literal ID="litTimeStamp" runat="server"></asp:Literal></span><br />
                        <asp:Literal ID="litCustomerName" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:Literal ID="litDescription" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="TempEstimatedItemPortion" HeaderText="SubTotal" DataFormatString="{0:C}">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TempEstimatedItemDiscount" HeaderText="Discounts" DataFormatString="{0:C}">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TempEstimatedShippingPortion" HeaderText="Shipping" DataFormatString="{0:C}">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TempEstimatedShippingDiscount" HeaderText="Ship Disc." DataFormatString="{0:C}">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TempEstimatedTaxPortion" HeaderText="Tax" DataFormatString="{0:C}"></asp:BoundColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:Literal ID="litAmount" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                    <ItemTemplate>
                        <asp:ImageButton ID="EditButton" runat="server" CommandName="Edit" ImageUrl="~/BVAdmin/Images/Buttons/OrderDetails.png">
                        </asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
            <PagerStyle CssClass="FormLabel" Mode="NumericPages"></PagerStyle>
        </asp:DataGrid>
    </div>
</asp:Content>
