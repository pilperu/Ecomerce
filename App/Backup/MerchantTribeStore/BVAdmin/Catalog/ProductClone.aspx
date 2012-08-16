<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Catalog_ProductClone" title="Clone Product" Codebehind="ProductClone.aspx.cs" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

    <h1>Product Clone</h1>
        <uc1:MessageBox ID="MessageBox1" runat="server" />    
    <table>
        <tr>
            <td>Name:</td>
            <td><asp:TextBox runat="server" ID="NameTextBox"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Sku:</td>
            <td><asp:TextBox runat="server" ID="SkuTextBox"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Clone Product Choices?</td>
            <td><asp:CheckBox ID="ProductChoicesCheckBox" runat="server" /></td>
        </tr>
        <tr>
            <td>Clone Product Images?</td>
            <td><asp:CheckBox ID="ImagesCheckBox" runat="server" /></td>
        </tr>
        <tr>
            <td>Clone Category Placement?</td>
            <td><asp:CheckBox ID="CategoryPlacementCheckBox" runat="server" /></td>
        </tr>
        <tr>
            <td>Create As Inactive?
            </td>
            <td><asp:CheckBox ID="InactiveCheckBox" runat="server" /></td>        
        </tr>
        <tr>
            <td colspan="2">
                <asp:ImageButton ID="CancelButton" runat="server" AlternateText="Cancel" 
                    ImageUrl="~/BVAdmin/Images/Buttons/Cancel.png" onclick="CancelButton_Click" />
                <asp:ImageButton ID="CloneButton" runat="server" AlternateText="Clone" 
                    ImageUrl="~/BVAdmin/Images/Buttons/CloneProduct.png" 
                    onclick="CloneButton_Click" />                
            </td>
        </tr>
    </table>
</asp:Content>

