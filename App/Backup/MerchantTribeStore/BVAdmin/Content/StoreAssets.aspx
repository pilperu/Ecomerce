<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Content_StoreAssets" Codebehind="StoreAssets.aspx.cs" %>

<%@ Register src="../Controls/MessageBox.ascx" tagname="MessageBox" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" Runat="Server">
    <script src="StoreAssets.js" language="javascript" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavContent" Runat="Server">
    <div class="padded center"><a href="../Catalog/Categories.aspx" title="Back to Pages" class="btn">        
        <b>&laquo; Back to Pages</b></a></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
<uc2:MessageBox ID="MessageBox1" runat="server" />    
    <h1>Page Images</h1>
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
</asp:Content>


