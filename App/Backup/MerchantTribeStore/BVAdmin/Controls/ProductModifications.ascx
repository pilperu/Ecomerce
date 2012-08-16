<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Controls_ProductModifications" Codebehind="ProductModifications.ascx.cs" %>
<%@ Register Src="EnumeratedValueModifierField.ascx" TagName="EnumeratedValueModifierField"
    TagPrefix="uc7" %>
<%@ Register Src="HtmlModifierField.ascx" TagName="HtmlModifierField" TagPrefix="uc6" %>
<%@ Register Src="IntegerModifierField.ascx" TagName="IntegerModifierField" TagPrefix="uc5" %>
<%@ Register Src="FloatModifierField.ascx" TagName="FloatModifierField" TagPrefix="uc4" %>
<%@ Register Src="BooleanModifierField.ascx" TagName="BooleanModifierField" TagPrefix="uc3" %>
<%@ Register Src="StringModifierField.ascx" TagName="StringModifierField" TagPrefix="uc2" %>
<%@ Register Src="MonetaryModifierField.ascx" TagName="MonetaryModifierField" TagPrefix="uc1" %>
<asp:Panel ID="ProductModificationPanel" CssClass="productModificationDiv" runat="server">
    <table>
        <tr runat="server">
            <td><asp:CheckBox ID="ProductNameCheckBox" runat="server" CssClass="modificationSelected" />Product Name</td>
            <td>
                <uc2:StringModifierField ID="ProductNameStringModifierField" runat="server" />                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="ProductSkuCheckBox" runat="server" CssClass="modificationSelected" />Product Sku</td>
            <td>
                <uc2:StringModifierField ID="ProductSkuStringModifierField" runat="server" />                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="ProductTypeCheckBox" runat="server" CssClass="modificationSelected" />Product Type</td>
            <td>
            </td>
        </tr>        
        <tr runat="server">
            <td><asp:CheckBox ID="ListPriceCheckBox" runat="server" CssClass="modificationSelected" />List Price</td>
            <td>
                <uc1:MonetaryModifierField ID="ListPriceMonetaryModifierField" runat="server" />
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="SitePriceCheckBox" runat="server" CssClass="modificationSelected" />Site Price</td>
            <td>
                <uc1:MonetaryModifierField ID="SitePriceMonetaryModifierField" runat="server" />
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="SiteCostCheckBox" runat="server" CssClass="modificationSelected" />Site Cost</td>
            <td>
                <uc1:MonetaryModifierField ID="SiteCostMonetaryModifierField" runat="server" />
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="MetaKeywordsCheckBox" runat="server" CssClass="modificationSelected" />Meta Keywords</td>
            <td>
                <uc2:StringModifierField ID="MetaKeywordsStringModifierField" runat="server" />                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="MetaDescriptionCheckBox" runat="server" CssClass="modificationSelected" />Meta Description</td>
            <td>
                <uc2:StringModifierField ID="MetaDescriptionStringModifierField" runat="server" />                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="MetaTitleCheckBox" runat="server" CssClass="modificationSelected" />Meta Title</td>
            <td>
                <uc2:StringModifierField ID="MetaTitleStringModifierField" runat="server" />                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="TaxExemptCheckBox" runat="server" CssClass="modificationSelected" />Tax Exempt</td>
            <td>
                <uc3:BooleanModifierField ID="TaxExemptBooleanModifierField" runat="server" DisplayMode="YesNo" />                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="TaxClassCheckBox" runat="server" CssClass="modificationSelected" />Tax Class</td>
            <td>               
                <uc7:EnumeratedValueModifierField ID="TaxClassEnumeratedValueModifierField" runat="server" />
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="NonShippingCheckBox" runat="server" CssClass="modificationSelected" />Non-Shipping</td>
            <td>
                <uc3:BooleanModifierField ID="NonShippingBooleanModifierField" runat="server" DisplayMode="YesNo" />                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="ShipSeperatelyCheckBox1" runat="server" CssClass="modificationSelected" />Ship Seperately</td>
            <td>
                <uc3:BooleanModifierField ID="ShipSeperatelyBooleanModifierField" runat="server" DisplayMode="YesNo" />                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="ShippingModeCheckBox" runat="server" CssClass="modificationSelected" />Drop Ship Mode</td>
            <td>
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="ShippingWeightCheckBox" runat="server" CssClass="modificationSelected" />Shipping Weight</td>
            <td>
                <uc4:FloatModifierField ID="ShippingWeightFloatModifierField" runat="server" />                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="ShippingLengthCheckBox" runat="server" CssClass="modificationSelected" />Shipping Length</td>
            <td>
                <uc4:FloatModifierField ID="ShippingLengthFloatModifierField" runat="server" />                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="ShippingWidthCheckBox" runat="server" CssClass="modificationSelected" />Shipping Width</td>
            <td>
                <uc4:FloatModifierField ID="ShippingWidthFloatModifierField" runat="server" />                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="ShippingHeightCheckBox" runat="server" CssClass="modificationSelected" />Shipping Height</td>
            <td>
                <uc4:FloatModifierField ID="ShippingHeightFloatModifierField" runat="server" />                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="StatusCheckBox" runat="server" CssClass="modificationSelected" />Product State</td>
            <td>
                <uc3:BooleanModifierField ID="ProductStateBooleanModifierField" runat="server" DisplayMode="EnabledDisabled" />                                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="ImageFileSmallCheckBox" runat="server" CssClass="modificationSelected" />Image File Small</td>
            <td>
                <uc2:StringModifierField ID="ImageFileSmallStringModifierField" runat="server" />
                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="ImageFileMediumCheckBox" runat="server" CssClass="modificationSelected" />Image File Medium</td>
            <td>
                <uc2:StringModifierField ID="ImageFileMediumStringModifierField" runat="server" />                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="MinimumQuantityCheckBox" runat="server" CssClass="modificationSelected" />Minimum Quantity</td>
            <td>
                <uc5:IntegerModifierField ID="MinimumQuantityIntegerModifierField" runat="server" />                
            </td>        
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="VariantDisplayModeCheckBox" runat="server" CssClass="modificationSelected" />Variant Display Mode</td>
            <td>
                
            </td>        
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="ShortDescriptionCheckBox" runat="server" CssClass="modificationSelected" />Short Description</td>
            <td>
                <uc2:StringModifierField ID="ShortDescriptionStringModifierField" runat="server" />
                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="LongDescriptionCheckBox" runat="server" CssClass="modificationSelected" />Long Description</td>
            <td>
                <uc6:HtmlModifierField ID="LongDescriptionHtmlModifierField" runat="server" />
                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="ManufacturerCheckBox" runat="server" CssClass="modificationSelected" />Manufacturer</td>
            <td>
                <uc7:EnumeratedValueModifierField ID="ManufacturerEnumeratedValueModifierField" runat="server" DisplayNone="true" />
                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="VendorCheckBox" runat="server" CssClass="modificationSelected" />Vendor</td>
            <td>
                <uc7:EnumeratedValueModifierField ID="VendorEnumeratedValueModifierField" runat="server"
                    DisplayNone="true" />
                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="GiftWrapAllowedCheckBox" runat="server" CssClass="modificationSelected" />Gift Wrap Allowed</td>
            <td>
                <uc3:BooleanModifierField ID="GiftWrapAllowedBooleanModifierField" runat="server" DisplayMode="YesNo" />
                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="ExtraShipFeeCheckBox" runat="server" CssClass="modificationSelected" />Extra Ship Fee</td>
            <td>
                <uc1:MonetaryModifierField ID="ExtraShipFeeMonetaryModifierField" runat="server" />
                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="KeyWordsCheckBox" runat="server" CssClass="modificationSelected" />Key Words</td>
            <td>
                <uc2:StringModifierField ID="KeyWordsStringModifierField" runat="server" />
                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="ProductTemplateCheckBox" runat="server" CssClass="modificationSelected" />Product Template</td>
            <td>
                <uc7:EnumeratedValueModifierField ID="ProductTemplateEnumeratedValueModifierField"
                    runat="server" DisplayNone="false" />
                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="PreContentCheckBox" runat="server" CssClass="modificationSelected" />Pre-content Column</td>
            <td>
                <uc7:EnumeratedValueModifierField ID="PreContentColumnEnumeratedValueModifierField"
                    runat="server" DisplayNone="true" />
                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="PostContentCheckBox" runat="server" CssClass="modificationSelected" />Post-content Column</td>
            <td>
                <uc7:EnumeratedValueModifierField ID="PostContentColumnEnumeratedValueModifierField"
                    runat="server" DisplayNone="true" />
                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="RewriteUrlCheckBox" runat="server" CssClass="modificationSelected" />Url to rewrite</td>
            <td>
                <uc2:StringModifierField ID="UrlToRewriteStringModifierField" runat="server" />
                
            </td>
        </tr>
        <tr runat="server">
            <td><asp:CheckBox ID="SitePriceOverrideCheckBox" runat="server" CssClass="modificationSelected" />Site Price Override</td>
            <td>
                <uc2:StringModifierField ID="SitePriceOverrideStringModifierField" runat="server" />
                
            </td>
        </tr>
    </table>
</asp:Panel>
