<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_People_PriceGroups" title="Untitled Page" Codebehind="PriceGroups.aspx.cs" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Price Groups</h1>
    <uc1:MessageBox ID="MessageBox1" runat="server" />
    <asp:GridView ID="PricingGroupsGridView" runat="server" 
        AutoGenerateColumns="False" DataKeyNames="bvin" GridLines="none" 
        CellPadding="3" onrowdatabound="PricingGroupsGridView_RowDataBound" 
        onrowdeleting="PricingGroupsGridView_RowDeleting">
        <Columns>
            <asp:TemplateField HeaderText="Name">
                <ItemTemplate>
                    <asp:TextBox ID="NameTextBox" runat="server"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Pricing Type">
                <ItemTemplate>
                    <asp:DropDownList ID="PricingTypeDropDownList" runat="server">
                        <asp:ListItem Text="Percentage Off Price" Value="4"></asp:ListItem>
                        <asp:ListItem Text="Amount Off Price" Value="5"></asp:ListItem>
                        <asp:ListItem Text="Percentage Off MSRP" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Amount Off MSRP" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Percentage Above Cost" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Amount Above Cost" Value="3"></asp:ListItem>                        
                    </asp:DropDownList>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText = "Adjustment Amount">
                <ItemTemplate>
                    <asp:TextBox ID="AdjustmentAmountTextBox" runat="server"></asp:TextBox>                
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton ID="DeleteImageButton" runat="server" AlternateText="Delete" CommandName="Delete" ImageUrl="~/BVAdmin/Images/Buttons/delete.png" />
                </ItemTemplate>
            </asp:TemplateField>            
        </Columns>
        <EmptyDataTemplate>
            There are currently no pricing groups.
        </EmptyDataTemplate>
        <RowStyle CssClass="row" />
        <HeaderStyle CssClass="rowheader" />
        <AlternatingRowStyle CssClass="alternaterow" />
    </asp:GridView>
    <br />
    <table>
    <tr>
        <td>
                <asp:ImageButton ID="AddNewImageButton" 
                    ImageUrl="~/BVAdmin/Images/Buttons/New.png" runat="server" AlternateText="Add" 
                    onclick="AddNewImageButton_Click" />   
        </td>
        <td>
            <asp:ImageButton ID="SaveImageButton" 
                ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" runat="server" 
                AlternateText="Save" onclick="SaveImageButton_Click" />
        </td>
        <td>
            <asp:ImageButton ID="CancelImageButton" 
                ImageUrl="~/BVAdmin/Images/Buttons/Cancel.png" runat="server" 
                AlternateText="Cancel" onclick="CancelImageButton_Click" />
        </td>
    </tr>
    </table>
</asp:Content>

