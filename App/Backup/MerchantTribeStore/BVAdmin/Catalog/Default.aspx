<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Catalog_Default" Title="BV Commerce Admin" Codebehind="Default.aspx.cs" %>

<%@ Register Src="../Controls/SimpleProductFilter.ascx" TagName="SimpleProductFilter"
    TagPrefix="uc1" %>  
<%@ Register src="../Controls/MessageBox.ascx" tagname="MessageBox" tagprefix="uc2" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">            
            <uc1:SimpleProductFilter ID="SimpleProductFilter" runat="server" />
            <div class="padded center"><asp:Literal runat="server" ID="litNewButton" EnableViewState="false"></asp:Literal></div>
            <div class="padded center"><asp:LinkButton runat="server" ID="btnRemoveSamples" 
                    Text="Remove Sample Products" onclick="btnRemoveSamples_Click"></asp:LinkButton></div>
</asp:Content>    
<asp:Content ID="maincontentplace" ContentPlaceHolderID="MainContent" runat="server">   
    <uc2:MessageBox ID="MessageBox1" runat="server" />
    <asp:Panel ID="pnlSamples" runat="server" Visible="false">
    <div class="flash-message-info">
    <p>You don't have any products in your store right now.<br />&nbsp;<br />
    Either <asp:Literal runat="server" ID="litNewButton3" EnableViewState="false"></asp:Literal> or 
    <asp:LinkButton ID="lnkAddSamples" runat="server" CssClass="btn" 
            Text="<b>Add Sample Products to This Store</b>" onclick="lnkAddSamples_Click"></asp:LinkButton><br />&nbsp;</p>        
    </div><br />&nbsp;
    </asp:Panel>
    <h1>Products</h1>        
            <h2><asp:label ID="lblResults" runat="server" EnableViewState="false"></asp:label></h2>
            <div id="categorygridtemplaterecords">
                <asp:Literal id="litPager1" runat="server" EnableViewState="false"></asp:Literal>
                <asp:Literal id="litResults" ClientIDMode="Static" runat="server" EnableViewState="false"></asp:Literal>
                <div class="clear"></div>
                <asp:Literal id="litPager2" runat="server" EnableViewState="false"></asp:Literal>
                <div class="padded center"><asp:Literal runat="server" ID="litNewButton2" EnableViewState="false"></asp:Literal></div>
            </div>                            
</asp:Content>
