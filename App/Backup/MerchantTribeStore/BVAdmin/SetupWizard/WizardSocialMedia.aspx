<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/AdminWizard.master" AutoEventWireup="true" CodeBehind="WizardSocialMedia.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.SetupWizard.WizardSocialMedia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>
        Store Setup - Step 3 of 4 - Social Media</h1>
    <p class="flash-message-info">
        Do you have a Twitter account?</p>
    <p class="flash-message-minor">
        A Twitter account is a great free way to promote your store and increase visibility.</p>
    

    <div style="padding: 20px 100px;">        
        <a href="http://twitter.com" target="_blank">Create a Twitter Account Now</a><br />
        &nbsp;<br />
        Twitter Username<br />
        @<asp:TextBox ID="twitterhandle" runat="server" Columns="40"></asp:TextBox> (optional)
        <div style="padding: 10px 0 0 0;">
            <asp:ImageButton ID="btnSaveMain" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/SaveAndContinue.png"
                            OnClick="btnSaveMain_Click" /><br />
           &nbsp;<br />
           <a href="WizardComplete.aspx">Skip &raquo;</a>           
        </div>
    </div>
</asp:Content>
