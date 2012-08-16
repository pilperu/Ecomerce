<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_ContentBlocks_Category_Menu_editor" Codebehind="editor.ascx.cs" %>
<asp:Panel ID="pnlMain" DefaultButton="btnSave" runat="server">
<table border="0" cellspacing="0" cellpadding="3">
<tr>
    <td class="formlabel">Menu Title</td>
    <td class="formfield">
        <asp:TextBox ID="TitleField" runat="server" Width="225px"></asp:TextBox></td>
</tr>
<tr>
    <td class="formlabel">Display Mode</td>
    <td class="formfield">
        <asp:DropDownList ID="ModeField" runat="server">
            <asp:ListItem Value="0">Show Root Categories Only</asp:ListItem>
            <asp:ListItem Value="1">Show All Categories</asp:ListItem>
            <asp:ListItem Value="2">Show Children, Peers and Parents</asp:ListItem>
            <asp:ListItem Value="3">Show All Categories for Current Parent</asp:ListItem>
        </asp:DropDownList></td>
</tr>
<tr>
    <td class="formlabel">Include &quot;Home&quot; link?</td>
    <td class="formfield">
        <asp:CheckBox ID="HomeLinkField" runat="server" /></td>
</tr>
<tr>
    <td class="formlabel">Show product count?</td>
    <td class="formfield">
        <asp:CheckBox ID="ProductCountCheckBox" runat="server" /></td>
</tr>
<tr>
    <td class="formlabel">Show sub-category count?</td>
    <td class="formfield">
        <asp:CheckBox ID="SubCategoryCountCheckBox" runat="server" /></td>
</tr>
<tr>
    <td class="formlabel">Maximum Tree Depth for All Categories Mode:</td>
    <td class="formfield">
        <asp:TextBox ID="MaximumDepth" runat="server"></asp:TextBox></td>
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