<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_ThemesEdit" Codebehind="ThemesEdit.aspx.cs" %>
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
    <h1>Edit Theme</h1>
    <table class="formtable">
    <tr>
        <td class="formlable">Theme Name:</td>
        <td class="formfield"><asp:TextBox ID="ThemeNameField" ClientIDMode="Static" runat="server" Columns="40"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="formlable">Description:</td>
        <td class="formfield"><asp:TextBox ID="DescriptionField" TextMode="MultiLine" runat="server" Columns="40" Rows="5" Wrap="true"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="formlable">Author:</td>
        <td class="formfield"><asp:TextBox ID="AuthorField" runat="server" Columns="40"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="formlable">Author's Web Site:</td>
        <td class="formfield"><asp:TextBox ID="AuthorUrlField" runat="server" Columns="40"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="formlable">Version Number:</td>
        <td class="formfield"><asp:TextBox ID="VersionField" runat="server" Columns="40"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="formlable">Download Url:</td>
        <td class="formfield"><asp:TextBox ID="VersionUrlField" runat="server" Columns="40"></asp:TextBox></td>
    </tr>    
    <tr>
        <td class="formlable">
           &nbsp;
        </td>
        <td class="formfield"><asp:ImageButton ID="ImageButton1" runat="server" 
                onclick="ImageButton1_Click" 
                ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" />
        </td>
    </tr>
    </table>
    &nbsp;<br />
    &nbsp;<br />
    <div class="controlarea2 padded">
        <h3>Preview Image</h3>
        &nbsp;<br />
        <asp:Image ID="imgPreview" runat="server" Height="120px" Width="160px" /><br />
        &nbsp;<br />
        <table>
            <tr>
                <td>Change Preview Image:</td>
                <td><asp:FileUpload ID="fileupload1" runat="server" /></td>
                <td><asp:ImageButton ID="btnUpload" 
            runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Upload.png" 
            onclick="btnUpload_Click" /></td>
            </tr>
        </table>
    </div>
</asp:Content>

