<%@ Page MasterPageFile="~/BVAdmin/BVAdmin.Master" ValidateRequest="False" Language="C#"
    AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Catalog_ProductTypeProperties" Codebehind="ProductTypeProperties.aspx.cs" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Type Properties</h1>
    <uc1:MessageBox ID="msg" runat="server" />
    <table>
        <tr>
            <td valign="bottom" align="left">
                <asp:DropDownList ID="lstProductType" runat="server">
                    <asp:ListItem Value="1" Selected="True">Text</asp:ListItem>
                    <asp:ListItem Value="2">Multiple Choice</asp:ListItem>
                    <asp:ListItem Value="3">Currency</asp:ListItem>
                    <asp:ListItem Value="4">Date</asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td>
                <asp:ImageButton ID="btnNew" runat="server" CausesValidation="False" 
                    ImageUrl="~/BVAdmin/images/buttons/New.png" onclick="btnNew_Click">
                </asp:ImageButton>&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgList" CellPadding="3" BorderWidth="0px" AutoGenerateColumns="False"
                    DataKeyField="id" runat="server" GridLines="none" 
                    ondeletecommand="dgList_DeleteCommand" oneditcommand="dgList_EditCommand">
                    <Columns>
                        <asp:BoundColumn DataField="PropertyName" HeaderText="Property Name"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TypeCodeDisplayName" HeaderText="Type"></asp:BoundColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="EditButton" ImageUrl="~/BVAdmin/images/buttons/Edit.png"
                                    AlternateText="Edit" CommandName="Edit"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton ID="DeleteImageButton" runat="server" ImageUrl="~/BVAdmin/images/buttons/Delete.png"
                                    CommandName="Delete" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <AlternatingItemStyle CssClass="alternaterow" />
                    <ItemStyle CssClass="row" />
                    <HeaderStyle CssClass="rowheader" />
                </asp:DataGrid>
            </td>
        </tr>
    </table>
</asp:Content>
