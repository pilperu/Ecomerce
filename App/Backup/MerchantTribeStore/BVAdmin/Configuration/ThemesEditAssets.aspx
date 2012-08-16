<%@ Page Title="Edit Theme | Assets" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_ThemesEditAssets" Codebehind="ThemesEditAssets.aspx.cs" %>
<%@ Register src="ThemesNav.ascx" tagname="ThemesNav" tagprefix="uc1" %>
<%@ Register src="../Controls/MessageBox.ascx" tagname="MessageBox" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" Runat="Server">
    <script src="ThemesEditAssets.js" language="javascript" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavContent" Runat="Server">
<uc1:ThemesNav ID="ThemesNav1" runat="server" />
    <div class="padded center"><a href="Themes.aspx" title="Back to Themes" class="btn">        
        <b>&laquo; Back to Themes</b></a></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
<uc2:MessageBox ID="MessageBox1" runat="server" />    
    <h1>Other Themes Images</h1>
    <div class="controlarea2 padded">
        <table>
            <tr>
                <td>Add Images:</td>
                <td><asp:FileUpload ID="fileupload1" runat="server" /></td>
                <td><asp:ImageButton ID="btnUpload" 
            runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Upload.png" 
            onclick="btnUpload_Click" /></td>
            </tr>
        </table>
     
    </div>
    &nbsp;<br />
    <asp:Literal ID="litMain" runat="server"></asp:Literal>  
    <asp:Literal ID="litIdField" runat="server" EnableViewState="false"></asp:Literal>  
</asp:Content>

