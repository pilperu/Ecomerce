<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_ContentBlocks_RSS_Feed_Viewer_editor" Codebehind="editor.ascx.cs" %>

<div style="margin:50px auto 50px auto;width:650px;text-align:left;">
<asp:Panel id="pnlEditor" runat="server" DefaultButton="btnSave">
<div style="margin-bottom:10px;height:50px;width:650px;background-image: url('../../BVModules/ContentBlocks/Rss Feed Viewer/Images/AdminBg.png');">
<div style="padding:13px 10px 0px 40px;color:#fff;">Feed Url: <asp:TextBox ID="FeedField" runat="server" Columns="80"></asp:TextBox>
</div>
</div>
 <table border="0" cellspacing="0" cellpadding="3">
 <tr>
    <td class="formlabel">Show Title?</td>
    <td class="forminput"><asp:Checkbox runat="server" ID="chkShowTitle" /></td>
 </tr>
 <tr>
    <td class="formlabel">Show Description?</td>
    <td class="forminput"><asp:Checkbox runat="server" ID="chkShowDescription" /></td>
 </tr>
 <tr>
    <td class="formlabel">Maximum Items</td>
    <td class="forminput"><asp:TextBox runat="server" ID="MaxItemsField" Columns="6" Text="5"></asp:TextBox></td>
 </tr>
 <tr>
    <td class="formlabel"><asp:ImageButton ID="btnCancel" CausesValidation="false" 
            runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Cancel.png" 
            onclick="btnCancel_Click" /></td>
    <td class="forminput"><asp:ImageButton ID="btnSave" runat="Server" 
            ImageUrl="~/BVAdmin/Images/buttons/SaveChanges.png" onclick="btnSave_Click" /></td>
 </tr>
 </table>
 </asp:Panel>
 </div>
 <asp:HiddenField ID="EditBvinField" runat="server" />

