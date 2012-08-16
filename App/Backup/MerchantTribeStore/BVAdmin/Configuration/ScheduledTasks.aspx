<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="true" CodeBehind="ScheduledTasks.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.Configuration.ScheduledTasks" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<h1>Scheduled Tasks</h1>
<!--<h2><asp:label ID="lblResults" runat="server" EnableViewState="false"></asp:label></h2>-->
            <div id="resultsgrid">
                <asp:Literal id="litPager1" runat="server" EnableViewState="false"></asp:Literal>
                <asp:Literal id="litResults" ClientIDMode="Static" runat="server" EnableViewState="false"></asp:Literal>
                <div class="clear"></div>
                <asp:Literal id="litPager2" runat="server" EnableViewState="false"></asp:Literal>
            </div>                
        &nbsp;
</asp:Content>
