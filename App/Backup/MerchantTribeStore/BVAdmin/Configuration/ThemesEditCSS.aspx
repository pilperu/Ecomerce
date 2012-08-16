<%@ Page Title="Edit Theme | CSS" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_ThemesEditCSS" Codebehind="ThemesEditCSS.aspx.cs" %>
<%@ Register src="ThemesNav.ascx" tagname="ThemesNav" tagprefix="uc1" %>
<%@ Register src="../Controls/MessageBox.ascx" tagname="MessageBox" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavContent" Runat="Server">
<uc1:ThemesNav ID="ThemesNav1" runat="server" />
    <div class="padded center"><a href="Themes.aspx" title="Back to Themes" class="btn">        
        <b>&laquo; Back to Themes</b></a></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <uc2:MessageBox ID="MessageBox1" runat="server" />    
    <h1>Edit Theme CSS</h1>    
    <asp:Literal ID="litEditor" runat="server" EnableViewState="false"></asp:Literal>
    <table border="0" cellspacing="0" cellpadding="3" width="100%">
    <tr>
        <td style="width:200px;text-align:left;">
            <asp:ImageButton ID="btnSave" runat="server" 
                ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" onclick="btnSave_Click" />
        </td>
        <td style="text-align:right">
          <asp:Panel ID="pnlTemp" Visible="false" runat="server"><div id="jsonoutput"></div>  
          <a href="#" id="updatecssonly"><img src="../images/buttons/QuickUpdate.png" alt="Update CSS" /></a></asp:Panel>
        </td>
    </tr>
    </table>        
<asp:HiddenField ID="themeidfield" ClientIDMode="Static" runat="server" />
</asp:Content>

