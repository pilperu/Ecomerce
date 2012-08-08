<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master"
    AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Catalog_Products_Edit"
    Title="Untitled Page" Codebehind="Products_Edit.aspx.cs" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc3" %>
<%@ Register Src="../Controls/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Register Src="../Controls/HtmlEditor.ascx" TagName="HtmlEditor" TagPrefix="uc1" %>
<%@ Register src="../Controls/ProductEditMenu.ascx" tagname="ProductEditMenu" tagprefix="uc5" %>
<%@ Register src="../Controls/UrlsAssociated.ascx" tagname="UrlsAssociated" tagprefix="uc4" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc5:ProductEditMenu ID="ProductNavigator" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <script type="text/javascript">
    String.prototype.trim = function () {
        return this.replace(/^\s*/, "").replace(/\s*$/, "");
    }


 String.prototype.padL = function (nLength, sChar) {
  var sreturn = this;
  while (sreturn.length < nLength) {
   sreturn = String(sChar) + sreturn;
  }
  return sreturn;

}


$(document).ready(function () {

    $('#productnamefield').change(function () {
        var rawName = $(this).val();                
        var cleanName = $('#RewriteUrlField').val();
        $.post('../Controllers/Slugify.aspx',
            { "input": rawName },
            function (data) {
                if (cleanName === '') {
                    $('#RewriteUrlField').val(data);
                }
            });
    });

    $('#imgupload').change(function () { $('.autoactivatebutton').click(); });

});
 
    </script>    
        
        <h1>Edit Product</h1>
    <div class="rf center">        
        <div class="controlarea1" style="margin: 0 0 10px 0;">
            <asp:Image ID="imgPreviewSmall" runat="server" ImageUrl="../images/NoImageAvailable.gif" CssClass="ProdImagePreview">
            </asp:Image><br />
            <asp:FileUpload ID="imgupload" runat="server" ClientIDMode="Static" Columns="40" />            
            <br />
            Picture Description:
            <asp:TextBox ID="SmallImageAlternateTextField" runat="server" Columns="25" CssClass="FormInput" Width="220px" TabIndex="6601"></asp:TextBox>
        </div>        
        <asp:HyperLink ID="lnkViewInStore" runat="server" Target="_blank" NavigateUrl="" ImageUrl="~/BVAdmin/Images/Buttons/ViewInStore.png"></asp:HyperLink>                                
        <br />&nbsp;<br />&nbsp;<br />
        <asp:HyperLink ID="lnkClone" runat="server" class="btn" NavigateUrl=""><b>Clone This Product</b></asp:HyperLink>
    </div>

    <asp:Panel ID="pnlMain" runat="server" CssClass="producteditpanel">
        <uc3:MessageBox ID="MessageBox1" runat="server" />
        <asp:Label ID="lblError" runat="server" CssClass="errormessage"></asp:Label>
        <table class="formtable" cellspacing="0" cellpadding="3" border="0">
            <tr>
                <td class="formfield" colspan="2">
                    <asp:ImageButton ID="btnCancel2" TabIndex="9006" runat="server" ImageUrl="../images/buttons/Cancel.png"
                        CausesValidation="False" onclick="btnCancel2_Click"></asp:ImageButton>
                    <asp:ImageButton ID="update1" CssClass="autoactivatebutton" runat="server" 
                        ImageUrl="../images/buttons/update.png" onclick="update1_Click"/>
                    <asp:ImageButton ID="btnSave2" TabIndex="9005" runat="server" 
                        ImageUrl="../images/buttons/SaveChanges.png" onclick="btnSave2_Click">
                    </asp:ImageButton>
                </td>
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
                    Featured:</td>
                <td class="formfield" valign="top" align="left">
                    <asp:CheckBox ID="chkFeatured" TabIndex="1610" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td class="formlabel" align="right" valign="top">
                    SKU:</td>
                <td class="formfield" valign="top" align="left">
                    <asp:TextBox ID="SkuField" TabIndex="1650" runat="server" Columns="40" CssClass="FormInput"
                        Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator id="valSkuRequired" runat="server" ControlToValidate="SkuField" ErrorMessage = "Sku is Required"
                    Text="*"></asp:RequiredFieldValidator>                    
                    <asp:CustomValidator ID="valSkuLength" runat="server" 
                        ControlToValidate="SkuField" 
                        ErrorMessage="Sku can not be more than 50 characters long." 
                        onservervalidate="valSkuLength_ServerValidate">*</asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td class="formlabel" align="right" valign="top">
                    Name:</td>
                <td class="formfield" valign="top" align="left">
                    <asp:TextBox ID="productnamefield" ClientIdMode="static" TabIndex="1700" runat="server" Columns="40" CssClass="FormInput"
                        Width="180px"></asp:TextBox>                                        
                    <asp:RequiredFieldValidator ID="valNameRequired" runat="server" 
                        ControlToValidate="ProductNameField" ErrorMessage="Name is Required">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="valNameLength" runat="server" 
                        ControlToValidate="ProductNameField" 
                        ErrorMessage="Name can not be longer than 255 characters." 
                        onservervalidate="valNameLength_ServerValidate">*</asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td class="formlabel" align="right" valign="top">
                    Product Type:</td>
                <td class="formfield" valign="top" align="left">
                    <asp:DropDownList ID="lstProductType" TabIndex="1800" runat="server" Width="200px"
                        AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:CustomValidator ID="ProductTypeCustomValidator" runat="server" 
                        ErrorMessage="Test" 
                        onservervalidate="ProductTypeCustomValidator_ServerValidate">*</asp:CustomValidator></td>
            </tr>
            <asp:Literal ID="ProductTypePropertiesLiteral" runat="server">
            </asp:Literal><tr><td colspan="2"><h2>Pricing</h2></td></tr>
            <tr>
                <td class="formlabel" align="right" valign="top">
                    MSRP:</td>
                <td class="formfield" align="left">
                    <asp:TextBox ID="ListPriceField" TabIndex="2000" runat="server" Columns="10" CssClass="FormInput"></asp:TextBox>                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="errormessage"
                        ControlToValidate="ListPriceField" ForeColor=" " ErrorMessage="List Price is required">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="ListPriceField"
                        CssClass="errormessage" ErrorMessage="Msrp must be a currency value">*</asp:CustomValidator>
                    Cost:
                    <asp:TextBox ID="CostField" runat="server" Columns="10" CssClass="FormInput" TabIndex="2100"></asp:TextBox>                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="CostField"
                        CssClass="errormessage" ErrorMessage="Cost is required" ForeColor=" ">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="CostField"
                        CssClass="errormessage" ErrorMessage="Cost must be a currency value">*</asp:CustomValidator></td>
            </tr>
            <tr>
                <td class="formlabel" align="right" valign="top">
                    Price:</td>
                <td class="formfield" align="left">
                    <asp:TextBox ID="SitePriceField" TabIndex="2200" runat="server" Columns="10" CssClass="FormInput"></asp:TextBox>                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="errormessage"
                        ControlToValidate="SitePriceField" ForeColor=" " ErrorMessage="Site Price is required">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="CustomValidator3" runat="server" ControlToValidate="SitePriceField"
                        CssClass="errormessage" ErrorMessage="Price must be a currency value">*</asp:CustomValidator>&nbsp;
                        Text: <asp:TextBox ID="PriceOverrideTextBox" runat="server" Columns="10" TabIndex="2300"></asp:TextBox>&nbsp;
                    </td>
            </tr>
            <tr><td colspan="2"><h2>Properties</h2></td></tr>
            <tr>
                <td class="formlabel" align="right" valign="top">
                    Manufacturer:</td>
                <td class="formfield" valign="top" align="left">
                    <asp:DropDownList ID="lstManufacturers" TabIndex="2400" runat="server" Width="200px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td class="formlabel" align="right" valign="top">
                    Vendor:</td>
                <td class="formfield" valign="top" align="left">
                    <asp:DropDownList ID="lstVendors" TabIndex="2500" runat="server" Width="200px">
                    </asp:DropDownList></td>
            </tr>           
            <tr>
                <td class="formlabel" align="right" valign="top">
                    &nbsp;</td>
                <td class="formfield" valign="top" align="left">
                    <table runat="server" id="tblGeneral">
                    </table>
                </td>
            </tr>            
            <tr><td colspan="2"><h2>Description</h2></td></tr>
            <tr>
                <td class="formfield" colspan="2" valign="top" align="left">
                    <uc1:HtmlEditor ID="LongDescriptionField" runat="server" EditorHeight="120" EditorWidth="420"
                        EditorWrap="true" TabIndex="2600" />
                        </td>
            </tr>
            <tr>
                <td class="formLabel" align="right" valign="top">
                    Alternate Search Keywords:</td>
                <td>
                    <asp:TextBox ID="Keywords" runat="server" Columns="40" Rows="1" Width="225px" TabIndex="3100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="formLabel" align="right" valign="top">
                    Meta Title:</td>
                <td>
                    <asp:TextBox ID="MetaTitleField" runat="server" Columns="40" Rows="1" Width="225px" TabIndex="3200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="formLabel" align="right" valign="top">
                    Meta Description:</td>
                <td>
                    <asp:TextBox ID="MetaDescriptionField" runat="server" TextMode="multiLine" Columns="40" Rows="3" Width="225" TabIndex="3300"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="formLabel" align="right" valign="top">
                    Meta Keywords:</td>
                <td>
                    <asp:TextBox ID="MetaKeywordsField" runat="server" TextMode="multiLine" Columns="40" Rows="3" Width="225px" TabIndex="3400"></asp:TextBox>
                </td>
            </tr>
            
            <tr><td colspan="2"><h2>Tax</h2></td></tr>
            <tr>
                <td class="formlabel">
                    Tax Schedule:</td>
                <td class="formfield">
                    <asp:DropDownList ID="TaxClassField" runat="server" Width="200px" TabIndex="4000">
                        <asp:ListItem> - None -</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td class="formlabel">
                    Tax Exempt?</td>
                <td class="formfield">
                    <asp:CheckBox Checked="false" ID="TaxExemptField" TabIndex="4100" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr><td colspan="2"><h2>Shipping</h2></td></tr>
             <tr>
                <td class="formLabel" align="right" valign="top">
                    Weight:</td>
                <td class="formfield">
                    <asp:TextBox ID="WeightField" runat="server" Columns="5" Rows="1" TabIndex="5000"></asp:TextBox>                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="WeightField"
                        Display="Dynamic" ErrorMessage="Weight is required.">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="CustomValidator4" runat="server" ControlToValidate="WeightField"
                        Display="Dynamic" ErrorMessage="Weight must be a numeric value.">*</asp:CustomValidator></td>
            </tr>
            <tr>
                <td class="formLabel" align="right" valign="top">
                    Dimensions:</td>
                <td class="formfield">
                    <asp:TextBox ID="LengthField" runat="server" Columns="5" Rows="1" TabIndex="5100"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="LengthField"
                        Display="Dynamic" ErrorMessage="Length is required.">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="CustomValidator5" runat="server" ControlToValidate="LengthField"
                        Display="Dynamic" ErrorMessage="Length must be a numeric value.">*</asp:CustomValidator>
                    L x <asp:TextBox ID="WidthField" runat="server" Columns="5" Rows="1" TabIndex="5200"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="WidthField"
                        Display="Dynamic" ErrorMessage="Width is required.">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="CustomValidator6" runat="server" ControlToValidate="WidthField"
                        Display="Dynamic" ErrorMessage="Width must be a numeric value.">*</asp:CustomValidator>
                    W x <asp:TextBox ID="HeightField" runat="server" Columns="5" Rows="1" TabIndex="5300"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="HeightField"
                        Display="Dynamic" ErrorMessage="Height is required.">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="CustomValidator7" runat="server" ControlToValidate="HeightField"
                        Display="Dynamic" ErrorMessage="Height must be a numeric value.">*</asp:CustomValidator>
                    H
                </td>
            </tr>
            <tr>
                <td class="formLabel" align="right" valign="top">
                    Extra Ship Fee:</td>
                <td class="formfield">
                    <asp:TextBox ID="ExtraShipFeeField" runat="server" Columns="5" Rows="1" TabIndex="5400"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ExtraShipFeeField"
                        Display="Dynamic" ErrorMessage="Extra Ship Fee is required.">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="CustomValidator8" runat="server" ControlToValidate="ExtraShipFeeField"
                        Display="Dynamic" ErrorMessage="Extra Ship Fee must be a numeric value.">*</asp:CustomValidator></td>
            </tr>
            <tr>
                <td class="formLabel" align="right" valign="top">
                    Ship Mode:</td>
                <td class="formfield">
                    <asp:DropDownList ID="ShipTypeField" runat="server">
                    <asp:ListItem Text="Ship from Store" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Drop Ship from Manufacturer" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Drop Ship from Vendor" Value="2"></asp:ListItem>
                    </asp:DropDownList><br />
                    <asp:CheckBox ID="chkNonShipping" runat="server" Text="Non-Shipping Product" /><br />
                    <asp:CheckBox ID="chkShipSeparately" runat="server" 
                        Text="Ships in a Separate Box" />
                </td>
            </tr>            
            <tr style="display:none;"><td colspan="2"><h2>Gift Wrap</h2></td></tr>
            <tr style="display:none;">
                <td class="formlabel">Gift Wrap:</td>
                <td class="formfield"><asp:CheckBox ID="chkGiftWrapAllowed" runat="server" /></td>
            </tr>
            <tr style="display:none;">
                <td class="formlabel">Gift Wrap Charge:</td>
                <td class="formfield"><asp:TextBox ID="txtGiftWrapCharge" runat="server" Columns="5" Rows="1" TabIndex="6050" Text="0.00"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="BVRequiredFieldValidator1" runat="server" ControlToValidate="txtGiftWrapCharge"
                        Display="Dynamic" ErrorMessage="Gift Wrap Charge is required.">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="BVCustomValidator1" runat="server" ControlToValidate="txtGiftWrapCharge"
                        CssClass="errormessage" ErrorMessage="Gift Wrap Charge must be a currency value">*</asp:CustomValidator></td>
            </tr>
            <tr><td colspan="2"><h2>Advanced</h2></td></tr>
            <tr>
                <td class="formlabel">Allow Reviews:</td>
                <td><asp:CheckBox id="chkAllowReviews" runat="Server"></asp:CheckBox></td>
            </tr>
            <tr>
                <td class="formlabel">
                    Page Name:</td>
                <td class="formfield">/<asp:TextBox ID="RewriteUrlField" ClientIdMode="Static" runat="server" 
                        Columns="30" Width="180px" TabIndex="6000"></asp:TextBox>
                    <br />
                    <uc4:UrlsAssociated ID="UrlsAssociated1" runat="server" />
                </td>
            </tr>
             <tr>
                <td class="formLabel" align="right" valign="top">
                    Minimum Qty:</td>
                <td class="formfield">
                    <asp:TextBox ID="MinimumQtyField" runat="server" Columns="5" Rows="1" TabIndex="6050" Text="0"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="MinimumQtyField"
                        Display="Dynamic" ErrorMessage="Minimum Quantity is required.">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator6" runat="server" ControlToValidate="MinimumQtyField"
                            Display="Dynamic" ErrorMessage="Minimum Quantity must be numeric." ValidationExpression="[0-9]{1,6}">*</asp:RegularExpressionValidator></td>
            </tr>
             <tr>
                <td class="formlabel">
                    Pre-Content Column</td>
                <td class="formfield">
                    <asp:DropDownList ID="PreContentColumnIdField" runat="server" Width="200px" TabIndex="6100">
                        <asp:ListItem> - None -</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td class="formlabel">
                    Post-Content Column</td>
                <td class="formfield">
                    <asp:DropDownList ID="PostContentColumnIdField" runat="server" Width="200px" TabIndex="6200">
                        <asp:ListItem> - None -</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td class="formfield" colspan="2">
                    <asp:ImageButton ID="btnCancel" TabIndex="6300" runat="server" ImageUrl="../images/buttons/Cancel.png"
                        CausesValidation="False" onclick="btnCancel_Click"></asp:ImageButton>&nbsp;
                    <asp:ImageButton ID="update2" runat="server" 
                        ImageUrl="../images/buttons/update.png" TabIndex="6400" 
                        onclick="update2_Click"/>
                    &nbsp;<asp:ImageButton ID="btnSaveChanges" TabIndex="6500" runat="server" 
                        ImageUrl="../images/buttons/SaveChanges.png" onclick="btnSaveChanges_Click">
                    </asp:ImageButton></td>
            </tr>
        </table>
    </asp:Panel>
    <div style="padding:50px 0 0 0;text-align:left;">
            <asp:ImageButton ID="btnDelete" runat="server" 
                ImageUrl="../images/buttons/delete.png" AlternateText="delete" 
                onclick="btnDelete_Click" />
    </div>
    <asp:HiddenField ID="BvinField" runat="server" />
</asp:Content>
