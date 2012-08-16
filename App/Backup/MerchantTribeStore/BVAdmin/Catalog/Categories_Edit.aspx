<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Catalog_Categories_Edit"
    Title="Edit Category" Codebehind="Categories_Edit.aspx.cs" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc3" %>
<%@ Register Src="../Controls/CategoryBreadCrumbTrail.ascx" TagName="CategoryBreadCrumbTrail"
    TagPrefix="uc2" %>
<%@ Register Src="../Controls/HtmlEditor.ascx" TagName="HtmlEditor" TagPrefix="uc1" %>

<%@ Register src="../Controls/UrlsAssociated.ascx" tagname="UrlsAssociated" tagprefix="uc4" %>

<asp:Content ID="headcontent" ContentPlaceHolderID="headcontent" runat="server">

<script type="text/javascript">
    // Jquery Setup
    $(document).ready(function () {

        $("#NameField").change(function () {

            rawName = $("#NameField").val();
            cleanName = $("#RewriteUrlField").val();
            
            $.post('../Controllers/Slugify.aspx',
                        { "input": rawName },
                        function (data) {                            
                            if (cleanName == "") {
                                $("#RewriteUrlField").val(data);
                            }
                        });
        });


    });
    </script>

</asp:Content>
    
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>
        Edit Category</h1>    
    <uc3:MessageBox ID="MessageBox1" runat="server" />
    <asp:Label ID="lblError" runat="server" CssClass="errormessage"></asp:Label>
    <div style="float: right;margin-bottom:50px;">
        <h2>
            Display Template</h2>        
        <asp:DropDownList ID="TemplateList" runat="server" AutoPostBack="True" TabIndex="2011">
            <asp:ListItem Value="Grid">Grid</asp:ListItem>
            <asp:ListItem Value="SimpleList">Simple List</asp:ListItem>            
        </asp:DropDownList><br />
        &nbsp;        
        <h2>Select Products</h2>
        <asp:ImageButton ID="btnSelectProducts" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/Select.png" TabIndex="2017" 
            onclick="btnSelectProducts_Click">
                        </asp:ImageButton>
        &nbsp;
        <h2>Page Name</h2>
        /<asp:TextBox ID="RewriteUrlField" ClientIDMode="Static" runat="server" Columns="30" Width="250px" TabIndex="2022"></asp:TextBox><br />
        <uc4:UrlsAssociated ID="UrlsAssociated1" runat="server" /><br />
        <h2>Images</h2>
        <table border="0" cellspacing="0" cellpadding="3">
            <tr>
                <td class="formlabel">
                    Icon:</td>
                <td class="formfield">
                    <img id="iconimage" src="<%=IconImage%>" class="iconimage" alt="Store Logo" width="100" /><br />
                    <asp:ImageButton ID="delIcon" runat="server" 
                        ImageUrl="~/bvadmin/images/buttons/x.png" AlternateText="Delete" 
                        onclick="delIcon_Click" /><asp:FileUpload id="iconupload" runat="server" ClientIDMode="static" Columns="40" />
                </td>
            </tr>
            <tr>
                <td class="formlabel">
                    Banner:</td>
                <td class="formfield">
                    <img id="bannerimage" src="<%=BannerImage%>" class="bannerimage" alt="Banner Image" width="100" /><br />
                    <asp:ImageButton ID="delBanner" runat="server" 
                        ImageUrl="~/bvadmin/images/buttons/x.png" AlternateText="Delete" 
                        onclick="delBanner_Click" /><asp:FileUpload id="bannerupload" runat="server" ClientIDMode="static" Columns="40" /></td>
            </tr>
        </table>
        &nbsp;        
        <asp:PlaceHolder ID="inStore" runat="Server"></asp:PlaceHolder>
        &nbsp;
        
    </div>        
    <asp:Panel ID="pnlMain" runat="server">
        <uc2:CategoryBreadCrumbTrail ID="CategoryBreadCrumbTrail1" runat="server" />
        <table border="0" cellspacing="0" cellpadding="3">
            <tr>
                <td class="formlabel">
                    Name:</td>
                <td class="formfield">
                    <asp:TextBox ID="NameField" runat="server" ClientIDMode="Static" Columns="30" MaxLength="100" TabIndex="2000"
                        Width="300px"></asp:TextBox><asp:RequiredFieldValidator ID="valName" runat="server"
                            ErrorMessage="Please enter a name" ControlToValidate="NameField">*</asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td class="formlabel">
                    Description:</td>
                <td class="formfield">
                    <uc1:HtmlEditor ID="DescriptionField" runat="server" EditorHeight="120" EditorWidth="300"
                        EditorWrap="true" TabIndex="2001" />
                </td>
            </tr>
            <tr>
                <td class="formlabel">
                    Page Title:</td>
                <td class="formfield">
                    <asp:TextBox ID="MetaTitleField" runat="server" Columns="30" MaxLength="512" TabIndex="2002"
                        Width="300px"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="formlabel">
                    Meta Description:</td>
                <td class="formfield">
                    <asp:TextBox ID="MetaDescriptionField" runat="server" MaxLength="255" Columns="30"
                        TabIndex="2003" Width="300px" Height="75px" Rows="4" TextMode="MultiLine"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="formlabel">
                    Meta Keywords:</td>
                <td class="formfield">
                    <asp:TextBox ID="MetaKeywordsField" runat="server" Columns="30" MaxLength="255" TabIndex="2004"
                        Width="300px"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="formlabel">
                    Search Keywords:</td>
                <td class="formfield">
                    <asp:TextBox ID="keywords" runat="server" Columns="30" MaxLength="512" TabIndex="2005"
                        Width="300px"></asp:TextBox></td>
            </tr>
               <tr>
                <td class="formlabel">
                    Pre-Content Column</td>
                <td class="formfield">
                    <asp:DropDownList ID="PreContentColumnIdField" runat="server" TabIndex="2006">
                        <asp:ListItem Value=""> - None -</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
               <tr>
                <td class="formlabel">
                    Post-Content Column</td>
                <td class="formfield">
                    <asp:DropDownList ID="PostContentColumnIdField" runat="server" TabIndex="2007">
                        <asp:ListItem Value=""> - None -</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>            
            <tr>
                <td class="formlabel">
                    Sort Order</td>
                <td class="formfield">
                    <asp:DropDownList ID="SortOrderDropDownList" runat="server" TabIndex="2008">
                        <asp:ListItem Value="1">Manual Order</asp:ListItem>
                        <asp:ListItem Value="2">Product Name</asp:ListItem>
                        <asp:ListItem Value="3">Product Price Ascending</asp:ListItem>
                        <asp:ListItem Value="4">Product Price Descending</asp:ListItem>                                
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td class="formlabel" style="height: 26px">
                    &nbsp;</td>
                <td class="formfield">
                    <asp:CheckBox ID="chkHidden" runat="server" Text="Hide Category" TabIndex="2009" /></td>
            </tr>
            <tr>
                <td class="formlabel">
                    &nbsp;</td>
                <td class="formfield">
                    <asp:CheckBox ID="chkShowTitle" runat="server" Text="Show Title" Checked="True" TabIndex="2011" /></td>
            </tr>
            <tr>
                <td class="formlabel">
                    <asp:ImageButton ID="btnCancel" TabIndex="2500" runat="server" ImageUrl="../images/buttons/Cancel.png"
                        CausesValidation="False" onclick="btnCancel_Click"></asp:ImageButton></td>
                <td class="formfield">
                    <asp:ImageButton ID="UpdateButton" TabIndex="2501" runat="server" 
                        ImageUrl="../images/buttons/Update.png" onclick="UpdateButton_Click">
                    </asp:ImageButton>
                    <asp:ImageButton ID="btnSaveChanges" runat="server" ImageUrl="../images/buttons/SaveChanges.png"
                        TabIndex="2502" onclick="btnSaveChanges_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:HiddenField ID="BvinField" runat="server" />
    <asp:HiddenField ID="ParentIDField" runat="Server" />
</asp:Content>
