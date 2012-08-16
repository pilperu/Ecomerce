<%@ Page ValidateRequest="false" Title="Edit Theme | Header and Footer" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_ThemesEditHeaderFooter" Codebehind="ThemesEditHeaderFooter.aspx.cs" %>
<%@ Register src="ThemesNav.ascx" tagname="ThemesNav" tagprefix="uc1" %>
<%@ Register src="../Controls/MessageBox.ascx" tagname="MessageBox" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavContent" Runat="Server">
<uc1:ThemesNav ID="ThemesNav1" runat="server" />
    <div class="padded center"><a href="Themes.aspx" title="Back to Themes" class="btn">        
        <b>&laquo; Back to Themes</b></a></div>
        &nbsp;<br /> 
        &nbsp;<br /> 
        &nbsp;<br />                
        <div class="padded">
        <h2>Available Tags</h2>        
    <table>
        <tr>
            <td class="formlabel">{{logo}}</td>
            <td>Logo and Link</td>
        </tr>
        <tr>
            <td class="formlabel">{{headerlinks}}</td>
            <td>Contact Us, etc.</td>
        </tr>
        <tr>
            <td class="formlabel">{{cartlink}}</td>
            <td>View Cart Link</td>
        </tr>
        <tr>
            <td class="formlabel">{{headermenu}}</td>
            <td>Category Links</td>
        </tr>
        <tr>
            <td class="formlabel">{{searchform}}</td>
            <td>Search Form</td>
        </tr>
        <tr>
            <td class="formlabel">{{copyright}}</td>
            <td>Copyright Notice</td>
        </tr>
    </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <uc2:MessageBox ID="MessageBox1" runat="server" />    
    <h1>Edit Theme Header &amp; Footer</h1>
    <h2>Header</h2>
    <asp:Literal ID="litheader" runat="server" EnableViewState="false"></asp:Literal>    
    <h2>Footer</h2>
    <asp:Literal ID="litFooter" runat="server" EnableViewState="false"></asp:Literal>
    <br />
    <asp:ImageButton ID="btnSave" runat="server" 
        ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" onclick="btnSave_Click" />
    <asp:Panel ID="pnlTemp" runat="server" Visible="false">
    <div id="jsonoutput"></div>
    <a href="#" id="updateonly"><img src="../images/buttons/update.png" alt="Update Header and Footer" /></a><br />
    </asp:Panel>
    <asp:Literal ID="litIdField" runat="server" EnableViewState="false"></asp:Literal>
</asp:Content>

