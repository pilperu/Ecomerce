<%@ Page MasterPageFile="~/BVAdmin/BVAdminNav.Master" Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.products_edit_categories" Codebehind="Products_Edit_Categories.aspx.cs" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>
<%@ Register src="../Controls/ProductEditMenu.ascx" tagname="ProductEditMenu" tagprefix="uc5" %>
<%@ Register src="../Controls/ProductEditingDisplay.ascx" tagname="ProductEditing" tagprefix="uc5" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc5:ProductEditMenu ID="ProductNavigator" runat="server" />
    <uc5:ProductEditing ID="ProductEditing1" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">    
    <h1>Categories</h1>
    <table class="onecolumn">
        <tr>
            <td id="AdminContentWithNav">
                <uc2:MessageBox ID="msg" runat="server" />
                <uc2:MessageBox ID="WebPageMessage1" runat="server" />              
                <table class="FormTable" cellpadding="5">
                    <tr>
                    <td>
                        <asp:CheckBoxList ID="chkCategories" runat="server">
						</asp:CheckBoxList>
                    </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <asp:ImageButton ID="CancelButton" runat="server" 
                                ImageUrl="../images/buttons/Cancel.png" onclick="CancelButton_Click">
                            </asp:ImageButton></td>
                        <td align="right" valign="top">
                            <asp:ImageButton ID="SaveButton" runat="server" 
                                ImageUrl="../images/buttons/SaveChanges.png" onclick="SaveButton_Click">
                            </asp:ImageButton></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
