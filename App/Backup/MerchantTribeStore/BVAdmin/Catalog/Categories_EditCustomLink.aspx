<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="true" CodeBehind="Categories_EditCustomLink.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.Catalog.Categories_EditCustomLink" %>
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
        Edit 
        Custom Link Category</h1>    
    <uc3:MessageBox ID="MessageBox1" runat="server" />
    <asp:Label ID="lblError" runat="server" CssClass="errormessage"></asp:Label>
    <div style="float: right;margin-bottom:50px;">
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
        </table>
        &nbsp;        
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
                    Link To:</td>
                <td class="formfield">
                    <asp:TextBox ID="LinkToField" runat="server" Columns="30" MaxLength="1024" TabIndex="2001"
                        Width="300px"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="formlabel">
                    Page Title:</td>
                <td class="formfield">
                    <asp:TextBox ID="MetaTitleField" runat="server" Columns="30" MaxLength="512" TabIndex="2002"
                        Width="300px"></asp:TextBox></td>
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
                    &nbsp;</td>
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

