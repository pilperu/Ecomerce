<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="true" CodeBehind="Api.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.Configuration.Api" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>

<asp:Content ID="header" ContentPlaceHolderID="headcontent" runat="server">
<script type="text/javascript">

    function RemoveApiKey(lnk) {
        var id = lnk.attr('id');
        var idr = id.replace('remove', '');
        $.post('ApiRemoveKey.aspx',
            { "id": idr },
            function () {
                lnk.parent().parent().slideUp('slow', function () { lnk.parent().remove(); });
            }
            );
    }
       

    // Jquery Setup
    $(document).ready(function () {
        $('.removeapikey').click(function () {
            RemoveApiKey($(this));
            return false;
        });
    });                      // End Doc Ready
 </script>
</asp:Content>

<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
        <h1>Api</h1>        
        <uc1:MessageBox ID="MessageBox1" runat="server" />
        <div class="editorpanel">
        <p>API keys allow other programs and computers to access your store information for importing, exporting and reporting. Always keep API keys a secret. Never share then with anyone.<br />&nbsp;</p>
        <p>To turn off API access entirely, revoke all API Keys listed below and no-one can access your store.<br />&nbsp;</p>
        <p>You should consider changing your API Keys on a regular basis by creating a new one and revoking the old one as a security measure.<br />&nbsp;</p>
        </div>
        <br />&nbsp;
        <div class="editorpanel">
        API users can call &quot;Clear Products&quot; or &quot;Clear Categories&quot; until <asp:Literal ID="litTimeLimit" runat="server"></asp:Literal>.<br />
        <asp:LinkButton ID="btnResetClearTime" runat="server" 
                Text="<b>Allow Clear Operations for the next 60 minutes</b>" CssClass="btn" 
                onclick="btnResetClearTime_Click"></asp:LinkButton>
        </div>
        <br />&nbsp;
        <div>
                <h3>Current API Keys:</h3>
                <asp:Literal ID="litApiKeys" runat="server" EnableViewState="false"></asp:Literal><br />                
                <asp:LinkButton ID="lnkCreateApiKey" runat="server"
                 Text="<b>Create New API Key</b>" CssClass="btn" 
                    onclick="lnkCreateApiKey_Click"></asp:LinkButton>
        </div>
        
</asp:Content>


