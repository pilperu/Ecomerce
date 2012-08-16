<%@ Control Language="C#"  AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_ContentBlocks_Html_editor" Codebehind="editor.ascx.cs" %>
<%@ Register Src="../../../BVAdmin/Controls/HtmlEditor.ascx" TagName="HtmlEditor"
    TagPrefix="uc1" %>
<asp:Panel ID="pnlMain" DefaultButton="btnSave" runat="server">
<table border="0" cellspacing="0" cellpadding="3">
<tr>
    <td class="formlabel">Html Code</td>
    <td class="formfield">
        &nbsp;<uc1:HtmlEditor ID="HtmlEditor1" runat="server" EditorHeight="400" EditorWidth="700"
            EditorWrap="false" />
    </td>
</tr>
<tr>
    <td class="formlabel">
        <asp:ImageButton ID="btnCancel" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/Cancel.png" onclick="btnCancel_Click" CausesValidation="false" /></td>
    <td class="formfield">
        <asp:ImageButton ID="btnSave" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" onclick="btnSave_Click" CausesValidation="false" /></td>
</tr>
</table></asp:Panel>