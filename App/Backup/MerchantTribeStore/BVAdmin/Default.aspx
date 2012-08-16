<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Default" title="Dashboard" Codebehind="Default.aspx.cs" %>
<%@ Register Src="Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>
<%@ Register src="Controls/DashboardOrderSummary.ascx" tagname="DashboardOrderSummary" tagprefix="uc4" %>
<%@ Register src="Controls/DashboardAlerts.ascx" tagname="DashboardAlerts" tagprefix="uc5" %>
<%@ Register src="Controls/DashboardSalesSummary.ascx" tagname="DashboardSalesSummary" tagprefix="uc7" %>
<asp:Content ID="headercontent" ContentPlaceHolderID="headcontent" runat="server">
    
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc2:MessageBox ID="MessageBox1" runat="server" EnableViewState="false" />
    <div id="dashboard" >
        <div class="dashcolnav">                                    
            <uc5:DashboardAlerts ID="DashboardAlerts1" runat="server" />            
            <div class="block">
                <h3>Order Summary</h3>
                <uc4:DashboardOrderSummary ID="DashboardOrderSummary1" runat="server" />                
            </div>
            <asp:Panel ID="pnlGettingStarted" runat="server" Visible="true">
                <div class="block">
                <h3>Getting Started</h3>
                <div style="padding:10px 0;border-bottom:solid 1px #ccc;">
                    <a style="float:left;" href="catalog/default.aspx" title="Add Products"><img src="images/buttons/Dashboard_AddProducts.png" alt="Add Products" /></a>
                    <a style="float:right;" href="catalog/categories.aspx" title="Add Categories"><img src="images/buttons/Dashboard_AddCategories.png" alt="Add Categories" /></a>                                                        
                    <div class="clear"></div>
                </div>                
                <a href="http://help.merchanttribestores.com/pages/videos-creating-products" target="_blank" class="quickstarticon" title="Tutorial Video"><img src="images/QuickStartVideo.png" alt="Tutorial Video" /></a>                
                <a href="http://help.merchanttribestores.com/pages/managing-products-and-categories" target="_blank" class="quickstarticon" title="User Manual"><img src="images/QuickStartManual.png" alt="User Manual" /></a>
                <a href="http://help.merchanttribestores.com/pages/quick-start-guide" target="_blank" class="quickstarticon" title="QuickStart"><img src="images/QuickStartQuick.png" alt="Quick Start" /></a>
                <div style="text-align:center;display:none;">
                <asp:LinkButton ID="lnkHideGettingStarted" runat="server" 
                        Text="Hide This" onclick="lnkHideGettingStarted_Click"></asp:LinkButton></div>
            </div>
            </asp:Panel>
            
            
        </div>
        <div class="dashcol"><asp:Literal ID="litFreePlan" runat="server" EnableViewState="false" />
                <div class="block">
                    <uc7:DashboardSalesSummary ID="DashboardSalesSummary1" runat="server" />                    
                </div>                
                <h3>News and Updates</h3>                
                &nbsp;
                <div id="newsfeed">
                <ul>
                    <li><strong>July 24</strong><br />Version 1.4: Added several updates including Stripe payment processor. Visit Stripe.com to sign up.</li>
                </ul>
                </div>
        </div>       
        <div class="clear">&nbsp;</div>
    </div>
</asp:Content>

