<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_TaxClasses" Title="Untitled Page" Codebehind="TaxClasses.aspx.cs" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    
    <h1>Tax Schedules</h1>
    
    <uc1:MessageBox ID="msg" runat="server" />
        
    <table cellspacing="0" cellpadding="0" border="0" id="container">
        <tr>
            <td>
                <table cellpadding="0">
                        <tr>
                            <td class="FormLabel" valign="middle" align="right">
                                New Tax Schedule&nbsp;
                            </td>
                            <td class="FormLabel" valign="middle" align="left">
                                <asp:TextBox ID="DisplayNameField" runat="server" CssClass="FormInput" Columns="10"></asp:TextBox></td>
                            <td class="FormLabel" valign="middle" align="center">
                                <asp:ImageButton ID="btnAddNewRegion" runat="server" 
                                    ImageUrl="../../bvadmin/images/buttons/New.png" onclick="btnAddNewRegion_Click">
                                </asp:ImageButton></td>
                        </tr>
                    </table>
                    &nbsp;
                    <table cellspacing="0" cellpadding="3">
                        <tr>
                            <td align="center" colspan="2">
                                <asp:DataGrid ID="dgTaxClasses" DataKeyField="Id" Width="100%" 
                                    AutoGenerateColumns="False" runat="server"
                                    BorderWidth="0px" CellPadding="3" GridLines="none" 
                                    ondeletecommand="dgTaxClasses_Delete" >
                                    <AlternatingItemStyle CssClass="AlternateItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="Item"></ItemStyle>
                                    <HeaderStyle CssClass="Header"></HeaderStyle>
                                    <Columns>
                                        <asp:TemplateColumn HeaderText="Schedule Name" HeaderStyle-CssClass="rowheader">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Name") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn>
                                            <ItemTemplate>
                                                <a href="Taxes_Edit.aspx?id=<%# DataBinder.Eval(Container, "DataItem.Id") %>"><img src="../../bvadmin/images/buttons/Edit.png" alt="Edit" /></a>                                                
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="DeleteButton" runat="server" ImageUrl="../../bvadmin/images/buttons/Delete.png"
                                                    Message="Are you sure you want to delete this tax schedule?" CommandName="Delete"></asp:ImageButton>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                <AlternatingItemStyle CssClass="alternaterow" />
                                <HeaderStyle CssClass="rowheader" />
                                <ItemStyle CssClass="row" />
                                </asp:DataGrid></td>
                        </tr>
                    </table>
                </td>
        </tr>
    </table>
</asp:Content>
