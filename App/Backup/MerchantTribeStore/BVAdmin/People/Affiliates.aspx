<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_People_Affiliates" title="Untitled Page" Codebehind="Affiliates.aspx.cs" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">            
            <div class="controlarea1">
                        <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnGo">
                            Search:
                            <asp:TextBox ID="FilterField" runat="server" Width="200px"></asp:TextBox>
                            <asp:ImageButton ID="btnGo" runat="server" AlternateText="Filter Results" 
                                ImageUrl="~/BVAdmin/Images/Buttons/Go.png" onclick="btnGo_Click" /></asp:Panel>
            </div>            
            <div class="padded center"><asp:Literal runat="server" ID="litNewButton" EnableViewState="false"></asp:Literal>
            <asp:ImageButton ID="btnNew" runat="server" 
                            AlternateText="Add New Affiliate" 
                            ImageUrl="~/BVAdmin/Images/Buttons/New.png" onclick="btnNew_Click" /></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Affiliates</h1>
    
            <h2><asp:label ID="lblResults" runat="server" EnableViewState="false"></asp:label></h2>
            <div id="resultsgrid">
                <asp:Literal id="litPager1" runat="server" EnableViewState="false"></asp:Literal>
                <asp:Literal id="litResults" ClientIDMode="Static" runat="server" EnableViewState="false"></asp:Literal>
                <div class="clear"></div>
                <asp:Literal id="litPager2" runat="server" EnableViewState="false"></asp:Literal>
            </div>                
        &nbsp;
</asp:Content>