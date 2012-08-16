<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/AdminWizard.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_SetupWizard_WizardTheme" Codebehind="WizardTheme.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<h1>Store Setup - Step 1 of 4 - Themes</h1>
<p class="flash-message-info">Select a theme for your store.</p>
<p class="flash-message-minor">You can pick a different design later if you're not sure. Just select one to get started.</p>
    <div style="padding:20px 80px;">
        <asp:Literal ID="litThemes" runat="server" EnableViewState="false"></asp:Literal>
    </div>
    <div class="clear"></div>
</asp:Content>

