<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Catalog_GiftCertificateEdit" title="Edit Gift Certificate" Codebehind="GiftCertificateEdit.aspx.cs" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>
<%@ Register Src="../Controls/HtmlEditor.ascx" TagName="HtmlEditor" TagPrefix="uc1" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>
        Edit Gift Certificate</h1>
    <div style="float: right; width: 240px; text-align: center;">
        
        <h2>
            Small Image</h2>
        <div class="controlarea1" style="margin: 0 0 10px 0;">
            <asp:Image ID="imgPreviewSmall" runat="server" ImageUrl="../images/NoImageAvailable.gif">
            </asp:Image><br />
            <asp:TextBox ID="ImageFileSmallField" runat="server" Columns="25" CssClass="FormInput"
                Width="220px" TabIndex="2700"></asp:TextBox>
            <a href="javascript:popUpWindow('?returnScript=SetSmallImage&WebMode=1');">
                <asp:Image runat="server" ImageUrl="~/BVAdmin/images/buttons/Select.png" ID="imgSelect1" /></a><br />
        </div>
        <h2>
            Medium Image</h2>
        <div class="controlarea1" style="margin: 0 0 10px 0;">
            <asp:Image ID="imgPreviewMedium" runat="server" ImageUrl="../images/NoImageAvailable.gif">
            </asp:Image><br />
            <asp:TextBox ID="ImageFileMediumField" runat="server" Columns="25" CssClass="FormInput"
                Width="220px" TabIndex="2800"></asp:TextBox>
            <a href="javascript:popUpWindow('?returnScript=SetMediumImage&WebMode=1');">
                <asp:Image runat="server" ImageUrl="~/BVAdmin/images/buttons/Select.png" ID="Image1" /></a>
        </div>
    </div>
    <uc2:MessageBox ID="MessageBox1" runat="server" />
    <asp:Label ID="lblError" runat="server" CssClass="errormessage"></asp:Label>    
    <asp:Panel ID="pnlMain" runat="server" DefaultButton="btnSaveChanges">
        <table cellspacing="0" cellpadding="3" border="0">
            <tr>
                <td class="formlabel">&nbsp;
                    </td>
                <td class="formfield">
                    <asp:ImageButton ID="btnCancel2" TabIndex="9006" runat="server" ImageUrl="../images/buttons/Cancel.png"
                        CausesValidation="False" onclick="btnCancel2_Click"></asp:ImageButton>&nbsp;<asp:ImageButton 
                        ID="btnSave2" TabIndex="9005" runat="server" 
                        ImageUrl="../images/buttons/SaveChanges.png" onclick="btnSave2_Click">
                    </asp:ImageButton></td>
            </tr>
            <tr>
                <td class="formlabel" align="right" valign="top">
                    Active:</td>
                <td class="formfield" valign="top" align="left">
                    <asp:CheckBox Checked="true" ID="chkActive" TabIndex="1600" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td class="formlabel" align="right" valign="top">
                    Gift Certificate Type:</td>
                <td class="formfield" valign="top" align="left">
                    <asp:RadioButtonList ID="GiftCertificateTypeRadioButtonList" runat="server" 
                        AutoPostBack="True" 
                        onselectedindexchanged="GiftCertificateTypeRadioButtonList_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="1">Fixed Price</asp:ListItem>
                        <asp:ListItem Value="2">Customer Selected Price</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>            
            <tr>
                <td class="formlabel" align="right" valign="top">
                    SKU:</td>
                <td class="formfield" valign="top" align="left">
                    <asp:TextBox ID="SkuField" TabIndex="1650" runat="server" Columns="40" CssClass="FormInput"
                        Width="300px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" CssClass="errormessage"
                        ControlToValidate="SkuField" ForeColor=" " ErrorMessage="SKU is required">*</asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td class="formlabel" align="right" valign="top">
                    Name:</td>
                <td class="formfield" valign="top" align="left">
                    <asp:TextBox ID="ProductNameField" TabIndex="1700" runat="server" Columns="40" CssClass="FormInput"
                        Width="300px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valName" runat="server" CssClass="errormessage" ControlToValidate="ProductNameField"
                        ForeColor=" " ErrorMessage="Product name is required">*</asp:RequiredFieldValidator></td>
            </tr>
            <%--<tr>
                <td class="formlabel" align="right" valign="top">
                    Product Type:</td>
                <td class="formfield" valign="top" align="left"><asp:DropDownList ID="lstProductType" TabIndex="2600" runat="server" Width="300px">
                </asp:DropDownList></td>
            </tr>--%>
            <%--<tr>
                <td class="formlabel" align="right" valign="top">
                    MSRP:</td>
                <td class="formfield" align="left">
                    <asp:TextBox ID="ListPriceField" TabIndex="1900" runat="server" Columns="10" CssClass="FormInput"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="errormessage"
                        ControlToValidate="ListPriceField" ForeColor=" " ErrorMessage="List Price is required">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="errormessage"
                        ControlToValidate="ListPriceField" ForeColor=" " ErrorMessage="Msrp must be numeric."
                        ValidationExpression="[0-9.,]+" Display="Dynamic">*</asp:RegularExpressionValidator>
                    Cost:
                    <asp:TextBox ID="CostField" runat="server" Columns="10" CssClass="FormInput" TabIndex="1900"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="CostField"
                        CssClass="errormessage" ErrorMessage="Cost is required" ForeColor=" ">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="CostField"
                        CssClass="errormessage" Display="Dynamic" ErrorMessage="Cost must be numeric."
                        ForeColor=" " ValidationExpression="[0-9.,]+">*</asp:RegularExpressionValidator></td>
            </tr>--%>
            <tr id="PriceRow" runat="server">
                <td class="formlabel" align="right" valign="top">
                    Price:</td>
                <td class="formfield" align="left">
                    <asp:TextBox ID="SitePriceField" TabIndex="2000" runat="server" Columns="10" CssClass="FormInput"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="errormessage"
                        ControlToValidate="SitePriceField" ForeColor=" " ErrorMessage="Site Price is required">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="CustomValidator2" runat="server" CssClass="errormessage"
                        ControlToValidate="SitePriceField" ForeColor=" " ErrorMessage="Price must be a monetary amount."
                        Display="Dynamic" onservervalidate="CustomValidator2_ServerValidate">*</asp:CustomValidator></td>
            </tr>
            <tr>
                <td class="formlabel" align="right" valign="top">
                    &nbsp;</td>
                <td class="formfield" valign="top" align="left">
                    <table runat="server" id="tblGeneral">
                    </table>
                </td>
            </tr>
            <tr>
                <td class="formlabel" align="right" valign="top">
                    Description:</td>
                <td class="formfield" valign="top" align="left">
                    <uc1:HtmlEditor ID="LongDescriptionField" runat="server" EditorHeight="120" EditorWidth="325"
                        EditorWrap="true" TabIndex="2100" />
                </td>
            </tr>
            <tr>
                <td class="formlabel" align="right" valign="top">
                    Short Description:</td>
                <td class="formfield" valign="top" align="left">
                    <asp:TextBox ID="ShortDescriptionField" TabIndex="2200" runat="server" TextMode="MultiLine"
                        MaxLength="255" Rows="3" Columns="40" CssClass="FormInput" Width="325px"></asp:TextBox><br />
                    <asp:TextBox ID="CountField" runat="server" Columns="5" ReadOnly="true" Text="255"></asp:TextBox>
                    characters left
                </td>
            </tr>
            <%--<tr>
                <td class="formlabel" align="right" valign="top">
                    Manufacturer:</td>
                <td class="formfield" valign="top" align="left">
                    <asp:DropDownList ID="lstManufacturers" TabIndex="2600" runat="server" Width="300px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td class="formlabel" align="right" valign="top">
                    Vendor:</td>
                <td class="formfield" valign="top" align="left">
                    <asp:DropDownList ID="lstVendors" TabIndex="2700" runat="server" Width="300px">
                    </asp:DropDownList></td>
            </tr>--%>
             <tr>
                <td class="formlabel">
                    Pre-Content Column</td>
                <td class="formfield">
                    <asp:DropDownList ID="PreContentColumnIdField" runat="server" Width="300px" TabIndex="2300">
                        <asp:ListItem Value=""> - None -</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
               <tr>
                <td class="formlabel">
                    Post-Content Column</td>
                <td class="formfield">
                    <asp:DropDownList ID="PostContentColumnIdField" runat="server" Width="300px" TabIndex="2400">
                        <asp:ListItem Value=""> - None -</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td class="formlabel">
                    Rewrite Url To:</td>
                <td class="formfield">
                    <asp:TextBox ID="RewriteUrlField" runat="server" Columns="30" Width="300px" TabIndex="2500"></asp:TextBox></td>
            </tr>      
            <tr>
                <td class="formlabel">
                    Certificate Id Pattern:</td>
                <td class="formfield">
                    <asp:TextBox ID="CertificateIdTextBox" runat="server" Columns="30" Width="300px" TabIndex="2600"></asp:TextBox>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="CertificateIdTextBox"
                        Display="Dynamic" ErrorMessage="Certificate id pattern is invalid." 
                        onservervalidate="CustomValidator1_ServerValidate">*</asp:CustomValidator></td>
            </tr>
            <tr>
                <td class="formlabel">
                </td>
                <td class="formfield">
                    <p>Gift certificate id's are case insensitive and must have at least 6 random characters. The random characters are 
                    # for a random number, ! for a random letter, and ? for a random letter or number. For example GIFT######! could result in 
                    a gift certificate code issued as GIFT948593A.</p>
                </td>
            </tr>
            <tr>
                <td class="formlabel">&nbsp;
                    </td>
                <td class="formfield"><asp:ImageButton ID="btnCancel" TabIndex="9001" 
                        runat="server" ImageUrl="../images/buttons/Cancel.png"
                        CausesValidation="False" onclick="btnCancel_Click"></asp:ImageButton>&nbsp;<asp:ImageButton 
                        ID="btnSaveChanges" TabIndex="9000" runat="server" 
                        ImageUrl="../images/buttons/SaveChanges.png" onclick="btnSaveChanges_Click">
                    </asp:ImageButton></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:HiddenField ID="BvinField" runat="server" />
</asp:Content>

