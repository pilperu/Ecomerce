<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_ContentBlocks_Banner_Ad_editor" Codebehind="editor.ascx.cs" %>
<asp:Panel ID="pnlMain" DefaultButton="btnSave" runat="server">
<table border="0" cellspacing="0" cellpadding="3">
<tr>
    <td class="formlabel">Image Url:</td>
    <td class="formfield"><asp:TextBox id="ImageUrlField" runat="Server" Width="400px"></asp:TextBox></td>
</tr>
<tr>
    <td class="formlabel">Alternate Text:</td>
    <td class="formfield"><asp:TextBox id="AlternateTextField" runat="Server" 
            Width="400px"></asp:TextBox></td>
</tr>
<tr>
    <td class="formlabel">CSS ID:</td>
    <td class="formfield"><asp:TextBox id="CssIdField" runat="Server" Width="400px"></asp:TextBox></td>
</tr>
<tr>
    <td class="formlabel">CSS Class:</td>
    <td class="formfield"><asp:TextBox id="CssClassField" runat="Server" Width="400px"></asp:TextBox></td>
</tr>
<tr>
    <td class="formlabel">Link Url:</td>
    <td class="formfield"><asp:TextBox id="LinkUrlField" runat="Server" Width="400px"></asp:TextBox></td>
</tr>
<tr>
    <td class="formlabel">
        <asp:ImageButton ID="btnCancel" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/Cancel.png" onclick="btnCancel_Click" /></td>
    <td class="formfield">
        <asp:ImageButton ID="btnSave" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" onclick="btnSave_Click" /></td>
</tr>
</table></asp:Panel>