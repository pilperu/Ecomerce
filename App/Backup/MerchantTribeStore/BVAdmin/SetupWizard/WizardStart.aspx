<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/AdminWizard.master" AutoEventWireup="true" CodeBehind="WizardStart.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.SetupWizard.WizardStart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" Runat="Server">
<script type="text/javascript">
    function CheckChanged() {
        var chk = $('#chkagree');
        if (chk.attr('checked')) {
            $('#lnkcontinue').show();
        }
        else {
            $('#lnkcontinue').hide();
        }
    }
    // Document Ready Function
    $(document).ready(function () {
        $('#chkagree').click(function () { CheckChanged(); return true; });
        CheckChanged();
    });             // End Document Ready
        
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<h1>End User License Agreement</h1>
<div style="text-align:center;background:#f0f0f0;border:solid 1px #999;margin:20px 0;">
<iframe src="EULA.txt" scrolling="auto" height="350" width="100%"></iframe>
</div>
<table width="100%">
<tr>
    <td align="left">
        <a href="http://www.merchanttribe.com">I do NOT agree (Cancel Install)</a>
    </td>
    <td align="right"><input type="checkbox" id="chkagree" /><label for="chkagree">I agree to the end user license agreement. </label>
        <a id="lnkcontinue" href="WizardTheme.aspx" style="display:none"><asp:Image ID="imgContinue" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Continue.png"/></a>   
    </td>
</tr>
</table>
</asp:Content>

