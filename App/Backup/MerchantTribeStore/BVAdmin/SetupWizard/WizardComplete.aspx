<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/AdminWizard.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_SetupWizard_WizardComplete" Codebehind="WizardComplete.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<script src="https://platform.twitter.com/widgets.js" type="text/javascript"></script>
<h1>Store Setup - Step 4 of 4 - Complete!</h1>
<p class="flash-message-info">Tweet about your new store <a href="<%=TweetUrl %>" class="twitter-share-button">Tweet</a></p>
<div style="width:500px;margin:0 auto;padding:50px 0;">
    <a class="hugelink" href="<%=Page.ResolveUrl("~/bvadmin") %>">Go to your Dashboard Page &raquo;</a>
</div>    
</asp:Content>



