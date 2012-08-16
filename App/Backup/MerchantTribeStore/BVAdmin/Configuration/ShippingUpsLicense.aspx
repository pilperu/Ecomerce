<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="true" CodeBehind="ShippingUpsLicense.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.Configuration.ShippingUpsLicense" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:MessageBox ID="msg" runat="server" />
    <h1>
        UPS OnLine<sup>®</sup> Tools License Application</h1>
    <br />
    <table border="0" cellspacing="0" cellpadding="5">
        <tr>
            <td class="formlabel">&nbsp;</td>
            <td class="formfield" align="left" valign="top">
                <b>License Agreement:</b><br />
                <asp:Label Visible="True" ID="lblLicense" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="formlabel" align="right" valign="top">
                &nbsp;
            </td>
            <td class="formfield" align="left" valign="top">
                <a onclick="JavaScript:if (window.print) {window.print();} else { alert('Please choose the print button from your browser.  Usually in the menu dropdowns at File: Print'); } "
                    href="#">
                    <asp:Image ID="imgPrint" runat="server" ImageUrl="~/BVAdmin/images/buttons/Print.png">
                    </asp:Image></a>
            </td>
        </tr>
        <tr>
            <td class="formlabel" align="right" valign="top">
                Primary Contact Name:<br />
                (i.e. John Doe)
            </td>
            <td class="formfield" align="left" valign="top">
                <asp:TextBox ID="inName" MaxLength="35" runat="server" Columns="40" />
            </td>
        </tr>
        <tr>
            <td class="formlabel" align="right" valign="top">
                Company:<br />
                (i.e. BV Software)
            </td>
            <td class="formfield" align="left" valign="top">
                <asp:TextBox ID="inCompany" MaxLength="35" runat="server" Columns="40" />
            </td>
        </tr>
        <tr>
            <td class="formlabel" align="right" valign="top">
                Company Web Site:<br />
                (http://merchanttribe.com)
            </td>
            <td class="formfield" align="left" valign="top">
                <asp:TextBox ID="inURL" MaxLength="254" runat="server" Columns="40" />
            </td>
        </tr>
        <tr>
            <td class="formlabel" align="right" valign="top">
                Contact Title<br />
                (i.e. Developer)
            </td>
            <td class="formfield" align="left" valign="top">
                <asp:TextBox ID="inTitle" MaxLength="35" runat="server" Columns="40" />
            </td>
        </tr>
        <tr>
            <td class="formlabel" align="right" valign="top">
                Address Line 1<br />
                (i.e. 1234 Any Street)
            </td>
            <td class="formfield" align="left" valign="top">
                <asp:TextBox ID="inAddress1" MaxLength="35" runat="server" Columns="40" />
            </td>
        </tr>
        <tr>
            <td class="formlabel" align="right" valign="top">
                Address Line 2<br />
                (optional)
            </td>
            <td class="formfield" align="left" valign="top">
                <asp:TextBox ID="inAddress2" MaxLength="35" runat="server" Columns="40" />
            </td>
        </tr>
        <tr>
            <td class="formlabel" align="right" valign="top">
                Address Line 3<br />
                (optional)
            </td>
            <td class="formfield" align="left" valign="top">
                <asp:TextBox ID="inAddress3" MaxLength="35" runat="server" Columns="40" />
            </td>
        </tr>
        <tr>
            <td class="formlabel" align="right" valign="top">
                City:<br />
                (i.e. Chicago)
            </td>
            <td class="formfield" align="left" valign="top">
                <asp:TextBox ID="inCity" MaxLength="30" runat="server" Columns="40" />
            </td>
        </tr>
        <tr>
            <td class="formlabel" align="right" valign="top">
                State:<br />
                (U.S. and Canada ONLY)
            </td>
            <td class="formfield" align="left" valign="top">
                <asp:DropDownList ID="inState" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="formlabel" align="right" valign="top">
                Postal Code:
            </td>
            <td class="formfield" align="left" valign="top">
                <asp:TextBox ID="inZip" MaxLength="9" runat="server" Columns="10" />
            </td>
        </tr>
        <tr>
            <td class="formlabel" align="right" valign="top">
                Country:
            </td>
            <td class="formfield" align="left" valign="top">
                <asp:DropDownList ID="inCountry" runat="server">
                    <asp:listitem value="US">United States</asp:listitem>
                    <asp:listitem value="AR">Argentina</asp:listitem>
                    <asp:listitem value="AU">Australia</asp:listitem>
                    <asp:listitem value="AT">Austria</asp:listitem>
                    <asp:listitem value="BE">Belgium</asp:listitem>
                    <asp:listitem value="BR">Brazil</asp:listitem>
                    <asp:listitem value="CA">Canada</asp:listitem>
                    <asp:listitem value="CL">Chile</asp:listitem>
                    <asp:listitem value="CR">Costa Rica</asp:listitem>
                    <asp:listitem value="DK">Denmark</asp:listitem>
                    <asp:listitem value="DO">Dominican Republic</asp:listitem>
                    <asp:listitem value="FI">Finland</asp:listitem>
                    <asp:listitem value="FR">France</asp:listitem>
                    <asp:listitem value="DE">Germany</asp:listitem>
                    <asp:listitem value="GR">Greece</asp:listitem>
                    <asp:listitem value="GT">Guatemala</asp:listitem>
                    <asp:listitem value="HK">Hong Kong</asp:listitem>
                    <asp:listitem value="IN">India</asp:listitem>
                    <asp:listitem value="IR">Ireland</asp:listitem>
                    <asp:listitem value="IT">Italy</asp:listitem>
                    <asp:listitem value="JP">Japan</asp:listitem>
                    <asp:listitem value="LU">Luxembourg</asp:listitem>
                    <asp:listitem value="MY">Malaysia</asp:listitem>
                    <asp:listitem value="MX">Mexico</asp:listitem>
                    <asp:listitem value="NL">Netherlands</asp:listitem>
                    <asp:listitem value="NZ">New Zealand</asp:listitem>
                    <asp:listitem value="NO">Norway</asp:listitem>
                    <asp:listitem value="PA">Panama</asp:listitem>
                    <asp:listitem value="PE">Peru</asp:listitem>
                    <asp:listitem value="PH">Philippines</asp:listitem>
                    <asp:listitem value="PT">Portugal</asp:listitem>
                    <asp:listitem value="PR">Puerto Rico</asp:listitem>
                    <asp:listitem value="SG">Singapore</asp:listitem>
                    <asp:listitem value="ZA">South Africa</asp:listitem>
                    <asp:listitem value="KR">South Korea</asp:listitem>
                    <asp:listitem value="ES">Spain</asp:listitem>
                    <asp:listitem value="SE">Sweden</asp:listitem>
                    <asp:listitem value="CH">Switzerland</asp:listitem>
                    <asp:listitem value="GB">United Kingdom</asp:listitem>
                    <asp:listitem value="US">United States</asp:listitem>
                    <asp:listitem value="VI">U.S. Virgin Islands</asp:listitem>
                    <asp:listitem value="VE">Venezuela</asp:listitem>                
                </asp:DropDownList>
                
                
            </td>
        </tr>
        <tr>
            <td class="formlabel" align="right" valign="top">
                Phone Number:<br />
                (required)
            </td>
            <td class="formfield" align="left" valign="top">
                <asp:TextBox ID="inPhone" MaxLength="14" runat="server" Columns="14" />
            </td>
        </tr>
        <tr>
            <td class="formlabel" align="right" valign="top">
                Contact E-Mail:<br />
                (required)
            </td>
            <td class="formfield" align="left" valign="top">
                <asp:TextBox ID="inEmail" MaxLength="50" runat="server" Columns="40" />
            </td>
        </tr>
        <tr>
            <td class="formlabel" align="right" valign="top">
                UPS Account Number:<br />
                (required for UPS Shipping)
            </td>
            <td class="formfield" align="left" valign="top">
                <asp:TextBox ID="inUPSAccountNumber" MaxLength="100" runat="server" Columns="40"
                    CssClass="formfield" /><br />
                To open a UPS Account, click <a href="https://www.ups.com/myups/login?returnto=https%3a//www.ups.com/account/us/start%3floc%3den_US&amp;reasonCode=-1&amp;appid=OPENACCT"
                    target="_blank">here</a> or call 1-800-PICK-UPS.
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" colspan="2" class="formlabel">
                I would like a UPS Sales Representative to contact me about opening a UPS shipping
                account or to answer questions about UPS services. Yes:
                <asp:RadioButton ID="rbContactYes" GroupName="rbContact" Checked="False" runat="server" />
                No:
                <asp:RadioButton ID="rbContactNo" GroupName="rbContact" Checked="False" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="formlabel" align="left" valign="top">
                <asp:ImageButton ID="btnCancel" CausesValidation="False" runat="server" 
                    ImageUrl="~/BVAdmin/images/buttons/cancel.png" onclick="btnCancel_Click" />
            </td>
            <td class="formfield" align="right" valign="top">
                <asp:ImageButton ID="btnAccept" runat="server" 
                    ImageUrl="~/BVAdmin/images/buttons/IAgree.png" onclick="btnAccept_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
