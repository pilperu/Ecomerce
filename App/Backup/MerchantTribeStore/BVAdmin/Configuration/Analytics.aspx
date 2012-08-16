<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_Analytics" title="Untitled Page" Codebehind="Analytics.aspx.cs" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<h1>Analytics Settings</h1>
<uc1:MessageBox ID="MessageBox1" runat="server" />
        <asp:Label id="lblError" runat="server" CssClass="errormessage"></asp:Label>
        <asp:Panel ID="pnlMain" runat="server" DefaultButton="btnSave">
        <table border="0" cellspacing="0" cellpadding="3">
        <tr id="MerchantTribeAnalyticsRow" runat="server" visible="false">
            <td class="formlabel">Use MerchantTribe Analytics:</td>
            <td class="formfield">
                <asp:CheckBox ID="chkUseMerchantTribeAnalytics" runat="server" />
            </td>
        </tr>
        <tr id="MerchantTribeAnalyticsRow2" runat="server" visible="false">
                        <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td class="formlabel">Use Google Tracking:</td>
            <td class="formfield">
                <asp:CheckBox ID="chkGoogleTracker" runat="server" /></td>
        </tr>
        <tr>
            <td class="formlabel">Google Tracking Id:</td>
            <td class="formfield">
                <asp:TextBox ID="GoogleTrackingIdField" Columns="50" Width="300px" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
                        <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td class="formlabel">Use Google Adwords Conversion</td>
            <td class="formfield">
                <asp:CheckBox ID="chkGoogleAdwords" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="formlabel">Adwords Conversion Id:</td>
            <td class="formfield">
                <asp:TextBox ID="GoogleAdwordsConversionIdField" Columns="50" Width="300px" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="formlabel">Adwords Label:</td>
            <td class="formfield">
                <asp:TextBox ID="GoogleAdwordsLabelField" Columns="50" Width="300px" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="formlabel">Adwords Background Color:</td>
            <td class="formfield">
                <asp:TextBox ID="GoogleAdwordsBackgroundColorField" Columns="50" Width="300px" runat="server"></asp:TextBox></td>
        </tr>       
        <tr>
                     <td colspan="2">&nbsp;</td>
        </tr>         
        <tr>
            <td class="formlabel">Use Google Ecommerce Tracking:</td>
            <td class="formfield">
                <asp:CheckBox ID="chkGoogleEcommerce" runat="server" /></td>
        </tr>
        <tr>
            <td class="formlabel">Google Store Name:</td>
            <td class="formfield">
                <asp:TextBox ID="GoogleEcommerceStoreNameField" Columns="50" Width="300px" runat="server"></asp:TextBox></td>
        </tr>  
        <tr>
            <td class="formlabel">Google Category Name:</td>
            <td class="formfield">
                <asp:TextBox ID="GoogleEcommerceCategoryNameField" Columns="50" Width="300px" runat="server"></asp:TextBox></td>
        </tr>  
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
         <tr>
            <td class="formlabel">Use Yahoo Sales Tracking:</td>
            <td class="formfield">
                <asp:CheckBox ID="chkYahoo" runat="server" /></td>
        </tr>
        <tr>
            <td class="formlabel">Yahoo Account Id:</td>
            <td class="formfield">
                <asp:TextBox ID="YahooAccountIdField" Columns="50" Width="300px" runat="server"></asp:TextBox></td>
        </tr>  
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
         <tr>
            <td class="formlabel">Other Meta Tags:</td>
            <td class="formfield">
                <span class="smalltext">Add any additional meta tags here. For example:<br />
                &lt;meta name=&quot;google-site-verification&quot; content=&quot;FqmYcwmgR326V7S3v4oEhxQsacwmgR32jsIgFw&quot; /&gt;</span>
                <asp:TextBox ID="AdditionalMetaTagsField" TextMode="MultiLine" Wrap="false" 
                    Rows="4" Columns="50" Width="500px" runat="server"></asp:TextBox></td>
        </tr>        
           <tr>
            <td class="formlabel">Bottom of Page Tags:</td>
            <td class="formfield">
                <span class="smalltext">Add any analytics or HTML tags that should come right before the &lt;/body&gt; tag.</span>
                <asp:TextBox ID="BottomAnalyticsField" TextMode="MultiLine" Wrap="false" 
                    Rows="4" Columns="50" Width="500px" runat="server"></asp:TextBox></td>
        </tr>        
        <tr>  <td>&nbsp;</td>
                <td class="formfield"><asp:ImageButton ID="btnSave" CausesValidation="true"
                            runat="server" ImageUrl="../images/buttons/SaveChanges.png" 
                        onclick="btnSave_Click"></asp:ImageButton></td>
        </tr>
        </table>
        </asp:Panel>
</asp:Content>

