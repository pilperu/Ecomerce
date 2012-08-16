<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_Reports_Shopping_Carts_Default" title="Reports - Shopping Carts" Codebehind="View.aspx.cs" %>
<%@ Register Src="~/BVAdmin/Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<h1>
        Current Shopping Carts</h1>
    <uc1:MessageBox ID="msg" runat="server" />    
    <div style="padding-right: 10px; padding-left: 10px; background: #ffffff; padding-bottom: 10px;
                    width: 700px; padding-top: 10px">
                    <asp:Label ID="lblResponse" Text="" runat="server" CssClass="BVSmallText" /><br />
                    &nbsp;<br />
                    <asp:DataGrid DataKeyField="bvin" CellPadding="3" BorderWidth="0px" CellSpacing="1"
                        ItemStyle-CssClass="row" ID="dgList" runat="server" AutoGenerateColumns="False"
                        Width="680px" ShowFooter="True" GridLines="none" 
                        oneditcommand="dgList_Edit" onitemdatabound="dgList_ItemDataBound">
                        <HeaderStyle CssClass="rowheader" />
                        <AlternatingItemStyle CssClass="alternaterow"></AlternatingItemStyle>
                        <ItemStyle CssClass="ItemStyle2"></ItemStyle>
                        <FooterStyle CssClass="HeaderStyle2"></FooterStyle>
                        <FooterStyle CssClass="HeaderStyle2"></FooterStyle>
                        <Columns>
                            <asp:BoundColumn DataField="timeoforder" HeaderText="Date" DataFormatString="{0:d}">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="bvin" HeaderText="ID #">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="SubTotal" HeaderText="SubTotal" DataFormatString="{0:C}">
                            </asp:BoundColumn>
                            <asp:TemplateColumn>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton ID="EditButton" runat="server" CommandName="Edit" ImageUrl="~/BVAdmin/Images/Buttons/View.png">
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle CssClass="FormLabel"></PagerStyle>
                    </asp:DataGrid>
                </div>
</asp:Content>

