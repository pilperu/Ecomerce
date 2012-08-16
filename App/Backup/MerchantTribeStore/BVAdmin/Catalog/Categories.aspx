<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Catalog_Categories" Title="Categories" Codebehind="Categories.aspx.cs" %>

<asp:Content ID="header" ContentPlaceHolderID="headcontent" runat="server">
    <script src="Categories.js" language="javascript" type="text/javascript"></script>
</asp:Content>    
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>Categories / Pages</h1>
    <div class="controlarea2">
    <table class="formtable" cellpadding="3">
        <tr>
            <td><asp:ImageButton ID="btnNew" runat="server" AlternateText="Add New Category" ImageUrl="~/BVAdmin/Images/Buttons/new.png"
                    EnableViewState="False" onclick="btnNew_Click" /> </td>
            <td><asp:DropDownList id="lstType" runat="server">
                <asp:ListItem Value="0">Category Page</asp:ListItem>
                <asp:ListItem Value="4">Filtering Category</asp:ListItem>
                <asp:ListItem Value="3">HTML Page</asp:ListItem>
                <asp:ListItem Value="5">Web Site Page (BETA)</asp:ListItem>
                <asp:ListItem Value="2">Custom Link</asp:ListItem>
            </asp:DropDownList></td>
            <td>in</td>
            <td><asp:DropDownList ID="lstParents" runat="server"></asp:DropDownList></td>
        </tr>
    </table>
    </div><br />
    &nbsp;
    <asp:Literal ID="litMain" runat="server" EnableViewState="false"></asp:Literal>       
</asp:Content>
