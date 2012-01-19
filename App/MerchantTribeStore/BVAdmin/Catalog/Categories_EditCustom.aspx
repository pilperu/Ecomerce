<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Catalog_Categories_EditCustom" Codebehind="Categories_EditCustom.aspx.cs" %>
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
        Edit Custom Page</h1>    
    <uc3:MessageBox ID="MessageBox1" runat="server" />
    <asp:Label ID="lblError" runat="server" CssClass="errormessage"></asp:Label>
    <uc2:CategoryBreadCrumbTrail ID="CategoryBreadCrumbTrail1" runat="server" /><br />    
    <asp:Panel ID="pnlMain" runat="server">
        <div class="controlarea2 padded">
        <table class="formtable" border="0" cellspacing="0" cellpadding="3" width="100%">
            <tr>
                <td class="formlabel">&nbsp;</td>
                <td colspan="2" class="formfield"><asp:CheckBox ID="chkShowSidebar" runat="server" text="Show Sidebar?" /></td>
            </tr>
            <tr>
                <td class="formlabel">
                    Name:</td>
                <td class="formfield">
                    <asp:TextBox ID="NameField" runat="server" ClientIDMode="Static" MaxLength="100" TabIndex="2000"
                        Width="650px"></asp:TextBox><asp:RequiredFieldValidator ControlToValidate="NameField" ErrorMessage="Please enter a name">*</asp:RequiredFieldValidator></td>
                <td><asp:PlaceHolder ID="inStore" runat="Server"></asp:PlaceHolder></td>
            </tr>
            <tr>
                <td class="formlabel">
                    HTML:</td>
                <td colspan="2" class="formfield"><uc1:HtmlEditor ID="DescriptionField" runat="server" EditorHeight="600" EditorWidth="800"
                        EditorWrap="true" TabIndex="2001" />
                </td>
            </tr>            
            <tr>
                <td class="formlabel">Page Name:
                    </td>
                <td colspan="2" class="formfield">
                    /<asp:TextBox ID="RewriteUrlField" ClientIDMode="Static" runat="server" Width="700px" TabIndex="2022"></asp:TextBox>
                    <br />
                    <uc4:UrlsAssociated ID="UrlsAssociated1" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="formlabel">Meta Title:</td>
                <td colspan="2" class="formfield">
                    <asp:TextBox ID="MetaTitleField" runat="server" MaxLength="512" TabIndex="2002" Width="750px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="formlabel">Meta Desc:</td>
                <td colspan="2" class="formfield">
                  <asp:TextBox ID="MetaDescriptionField" runat="server" MaxLength="255" TabIndex="2003" Width="750px" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="formlabel">Meta Keywords:</td>
                <td colspan="2" class="formfield">
                  <asp:TextBox ID="MetaKeywordsField" runat="server" MaxLength="255" TabIndex="2004" Width="750px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="formlabel" style="height: 26px">
                    &nbsp;</td>
                <td class="formfield">
                    <asp:CheckBox ID="chkHidden" runat="server" Text="Hide Category (unless home page)" TabIndex="2500" /></td>
            </tr>
        </table>
        </div>
        <div class="editorcontrols">
        <asp:ImageButton ID="btnCancel" TabIndex="2500" runat="server" ImageUrl="../images/buttons/Cancel.png"
                        CausesValidation="False" onclick="btnCancel_Click"></asp:ImageButton>
<asp:ImageButton ID="UpdateButton" TabIndex="2501" runat="server" 
                        ImageUrl="../images/buttons/Update.png" onclick="UpdateButton_Click">
                    </asp:ImageButton>
                    <asp:ImageButton ID="btnSaveChanges" runat="server" ImageUrl="../images/buttons/SaveChanges.png"
                        TabIndex="2502" onclick="btnSaveChanges_Click" />
        </div>
    </asp:Panel>
    <a href="../content/StoreAssets.aspx" target="_blank">Need to Upload Some Images for this page?</a>
    <asp:HiddenField ID="BvinField" runat="server" />
    <asp:HiddenField ID="ParentIDField" runat="Server" />
</asp:Content>
