<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Catalog_Products_Edit_Inventory" title="Untitled Page" Codebehind="Products_Edit_Inventory.aspx.cs" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register src="../Controls/ProductEditMenu.ascx" tagname="ProductEditMenu" tagprefix="uc5" %>
<%@ Register src="../Controls/ProductEditingDisplay.ascx" tagname="ProductEditing" tagprefix="uc5" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc5:ProductEditMenu ID="ProductNavigator" runat="server" />
    <uc5:ProductEditing ID="ProductEditing1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">    
    <uc1:MessageBox ID="MessageBox1" runat="server" />
    <h1>Inventory</h1>
    <table border="0" cellspacing="0" cellpadding="3">   
    <tr>
       <td colspan="2">
        Inventory Mode: <asp:DropDownList id="OutOfStockModeField" runat="server">
        <asp:ListItem Text="Always In Stock (Ignore Inventory)" Value="100"></asp:ListItem>
        <asp:ListItem Text="When out of stock, remove from store" Value="101"></asp:ListItem>
        <asp:ListItem Text="When out of stock, show but don't allow purchases" Value="102"></asp:ListItem>
        <asp:ListItem Text="When out of stock, allow back orders" Value="103"></asp:ListItem>
        </asp:DropDownList><br />                
        &nbsp;<br />
        <asp:Label ID="lblIsAvailable" runat="server"></asp:Label><br />
        &nbsp;<br />
        <asp:GridView ID="EditsGridView" runat="server" DataKeyNames="bvin" 
               GridLines="None" AutoGenerateColumns="False" Width="690" 
               onrowdatabound="EditsGridView_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="SKU">                 
                    <ItemTemplate>
                        <asp:Label CssClass="smalltext" ID="Label1" runat="server" Text=''></asp:Label><br />
                        <asp:Label CssClass="smalltext" ID="Label2" runat="server" Text='<%# Bind("variantId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="On Hand">
                    <ItemTemplate><asp:Label ID="lblQuantityOnHand" runat="server" Text="0"></asp:Label></ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Reserved">
                    <ItemTemplate><asp:Label ID="lblQuantityReserved" runat="server" Text="0"></asp:Label></ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Avail. For Sale">
                    <ItemTemplate><asp:Label ID="lblQuantityAvailableForSale" runat="server" Text="0"></asp:Label></ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Low Stock Point">
                    <ItemTemplate><asp:TextBox ID="LowPointField" runat="server" Columns="5" Text="0"></asp:TextBox></ItemTemplate>
                </asp:TemplateField>                
                <asp:TemplateField>
                    <ItemTemplate><asp:DropDownList ID="AdjustmentModeField" runat="Server">
                    <asp:ListItem Value="1" Text="Add"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Subtract"></asp:ListItem>
                    <asp:ListItem Value="3" Text="Set To"></asp:ListItem>
                    </asp:DropDownList></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate><asp:TextBox ID="AdjustmentField" runat="server" Columns="5" Text="0"></asp:TextBox></ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle CssClass="row" />
            <HeaderStyle CssClass="rowheader" />
            <AlternatingRowStyle CssClass="alternaterow" />
        </asp:GridView>
       </td>
    </tr>
    <tr>
        <td class="formlabel"><asp:ImageButton ID="btnCancel" runat="server" 
                ImageUrl="~/BVAdmin/Images/Buttons/Cancel.png" AlternateText="Cancel" 
                CausesValidation="false" onclick="btnCancel_Click" /></td>
        <td class="formfield"><asp:ImageButton ID="btnSaveChanges" runat="server" 
                ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" 
                AlternateText="Save Changes" onclick="btnSaveChanges_Click" /></td>
    </tr>
    </table>     
    <asp:HiddenField ID="bvinfield" runat="server" />   
</asp:Content>

