<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Header" Codebehind="Header.ascx.cs" %>
<script src="https://platform.twitter.com/widgets.js" type="text/javascript"></script>
<div id="header">
    <div id="branding">
        <div id="brand">
            <%=StoreName%> - <em><%=AppVersion %></em>
        </div>
    </div>
    <div id="mainmenu">
        <% if (!HideMenu)
           { %>
        <div class="menu">
            <%=RenderedMenu%>
        </div>       
        <div id="gotolinks">
            <a href="<%=BaseUrl%>bvadmin/Account.aspx">My Account</a>&nbsp;&nbsp;
            <a href="<%=BaseUrl%>adminaccount/logout">Log Out</a>&nbsp;&nbsp;
            <a href="<%=BaseStoreUrl%>">Go To Store</a>
        </div>
        <% } %>
    </div>
</div>
