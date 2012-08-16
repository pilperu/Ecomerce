<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="true" CodeBehind="ShippingUSPSTester.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.Configuration.ShippingUSPSTester" %>
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
            <td class="formlabel">Zip Codes</td>
            <td class="formfield">From: <asp:textbox id="FromZipField" runat="server" Columns="7"></asp:textbox> To: <asp:TextBox ID="ToZipField" runat="server" Columns="7"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="formlabel">Service Type</td>
            <td class="formfield"><asp:DropDownList ID="lstServiceTypes" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
            <td class="formlabel">Packaging Type:</td>
            <td class="formfield"><asp:DropDownList ID="lstPackagingType" runat="server">
            <asp:ListItem Text="Auto-Selected Packaging" Value="-1"></asp:ListItem>
        <asp:ListItem Text="-------------------" Value="-1"></asp:ListItem>
        <asp:ListItem Text="First-Class Letter" Value="100"></asp:ListItem>
        <asp:ListItem Text="First-Class Large Envelope" Value="101"></asp:ListItem>
        <asp:ListItem Text="First-Class Parcel" Value="102"></asp:ListItem>
        <asp:ListItem Text="First-Class PostCard" Value="103"></asp:ListItem>
        <asp:ListItem Text="-------------------" Value="-1"></asp:ListItem>        
        <asp:ListItem Text="Flat Rate Box" Value="1"></asp:ListItem>
        <asp:ListItem Text="Flat Rate Box Small" Value="2"></asp:ListItem>
        <asp:ListItem Text="Flat Rate Box Medium" Value="3"></asp:ListItem>
        <asp:ListItem Text="Flat Rate Box Large" Value="4"></asp:ListItem>
        <asp:ListItem Text="Flat Rate Envelope" Value="5"></asp:ListItem>
        <asp:ListItem Text="Flat Rate Envelope Padded" Value="50"></asp:ListItem>
        <asp:ListItem Text="Flat Rate Envelope Legal" Value="51"></asp:ListItem>
        <asp:ListItem Text="Flat Rate Envelope Small" Value="52"></asp:ListItem>
        <asp:ListItem Text="Flat Rate Envelope Window" Value="53"></asp:ListItem>
        <asp:ListItem Text="Flat Rate Envelope Gift Card" Value="53"></asp:ListItem>
        <asp:ListItem Text="-------------------" Value="-1"></asp:ListItem>        
        <asp:ListItem Text="Variable" Value="0"></asp:ListItem>
        <asp:ListItem Text="Rectangular" Value="6"></asp:ListItem>
        <asp:ListItem Text="Non-Rectangular" Value="7"></asp:ListItem>  
        <asp:ListItem Text="Regional Box Rate A" Value="200"></asp:ListItem>
        <asp:ListItem Text="Regional Box Rate B" Value="201"></asp:ListItem>
        
        </asp:DropDownList></td>
        </tr>
        <tr>
            <td class="formlabel">Weight:</td>
            <td class="formfield"><asp:TextBox ID="WeightPoundsField" runat="server" Columns="3" value="0"></asp:TextBox>lbs. <asp:TextBox ID="WeightOuncesField" runat="server" Columns="3" value="0"></asp:TextBox>
                oz.</td>
        </tr>
        <tr>
            <td class="formlabel">Dimensions (inches):</td>
            <td class="formfield"><asp:TextBox ID="LengthField" runat="server" Columns="3" value="0"></asp:TextBox>L <asp:TextBox ID="WidthField" runat="server" Columns="3" value="0"></asp:TextBox>W <asp:TextBox ID="HeightField" runat="server" Columns="3" value="0"></asp:TextBox>H</td>
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


