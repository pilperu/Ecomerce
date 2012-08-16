<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master"  AutoEventWireup="true" CodeBehind="ShippingUSPSInternationalTester.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.Configuration.ShippingUSPSInternationalTester" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" runat="server">
</asp:Content>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>US Postal Service Domestic Rates Tester</h1>
        <uc1:MessageBox ID="MessageBox1" runat="server" EnableViewState="false" />                
        <table border="0" width="100%">        
        <tr>
            <td class="formlabel">From Zip Code</td>
            <td class="formfield"><asp:textbox id="FromZipField" runat="server" Columns="7"></asp:textbox></td>
        </tr>
        <tr>
            <td class="formlabel">To Country</td>
            <td class="formfield"><asp:DropDownList ID="lstCountries" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
            <td class="formlabel">Service Type</td>
            <td class="formfield"><asp:DropDownList ID="lstServiceTypes" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
            <td class="formlabel">Weight:</td>
            <td class="formfield"><asp:TextBox ID="WeightPoundsField" runat="server" Columns="3" Text="0"></asp:TextBox>lbs. <asp:TextBox ID="WeightOuncesField" runat="server" Columns="3" Text="0"></asp:TextBox>
                oz.</td>
        </tr>
        <tr>
            <td class="formlabel">Dimensions (inches):</td>
            <td class="formfield"><asp:TextBox ID="LengthField" runat="server" Columns="3" Text="0"></asp:TextBox>L <asp:TextBox ID="WidthField" runat="server" Columns="3" Text="0"></asp:TextBox>W <asp:TextBox ID="HeightField" runat="server" Columns="3" Text="0"></asp:TextBox>H</td>
        </tr>
        <tr>
            <td class="formlabel">&nbsp;</td>
            <td class="formfield"><asp:Button ID="btnGetRates" runat="server" Text="Get Rates" 
                    onclick="btnGetRates_Click" /></td>
        </tr>
        </table>
        &nbsp;<br />
        <h2>Results</h2>
        <asp:Literal ID="litRates" runat="server"></asp:Literal><br />
        <div style="overflow:auto;height:500px;width:700px;">
            <asp:Literal ID="litMessages" runat="server"></asp:Literal><br />
            <asp:Literal ID="litXml" runat="server"></asp:Literal>
        </div>

</asp:Content>


