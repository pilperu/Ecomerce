<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Catalog_ProductVolumeDiscounts" title="Untitled Page" Codebehind="ProductVolumeDiscounts.aspx.cs" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register src="../Controls/ProductEditMenu.ascx" tagname="ProductEditMenu" tagprefix="uc5" %>
<%@ Register src="../Controls/ProductEditingDisplay.ascx" tagname="ProductEditing" tagprefix="uc5" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc5:ProductEditMenu ID="ProductNavigator" runat="server" />
    <uc5:ProductEditing ID="ProductEditing1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">        
    <uc1:MessageBox ID="MessageBox1" runat="server" />
    <h1>Volume Discounts</h1>    
    <div>
        <asp:GridView CssClass="formtable" ID="VolumeDiscountsGridView" runat="server" 
            DataKeyNames="bvin" AutoGenerateColumns="False" GridLines="none" 
            onrowdeleting="VolumeDiscountsGridView_RowDeleting">
            <Columns>
                <asp:BoundField HeaderText="Quantity" DataField="Qty" />                
                <asp:BoundField HeaderText="Price" DataField="Amount" DataFormatString="{0:c}" HtmlEncode="False" />                
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="DeleteImageButton" runat="server" CommandName="Delete" ImageUrl="~/BVAdmin/Images/Buttons/Delete.png" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>        
    </div>
    <table>
        <tr>
            <td>Qty</td>
            <td>Price</td>
            <td></td>
        </tr>
        <tr>
            <td><asp:TextBox ID="QuantityTextBox" runat="server"></asp:TextBox></td>
            <td><asp:TextBox ID="PriceTextBox" runat="server"></asp:TextBox></td>
            <td><asp:ImageButton ID="NewLevelImageButton" runat="server" 
                    AlternateText="New Level" ImageUrl="~/BVAdmin/Images/Buttons/New.png" 
                    onclick="NewLevelImageButton_Click" /></td>
        </tr>
    </table>
</asp:Content>

