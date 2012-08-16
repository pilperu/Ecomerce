<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Controls_OptionItemEditor" Codebehind="OptionItemEditor.ascx.cs" %>
<div class="padded">
    <asp:Literal ID="litItem" runat="server" EnableViewState="false"></asp:Literal>    
    <asp:Panel ID="pnlNewForm" runat="server" DefaultButton="btnNew" CssClass="editorcontrols">
        <asp:TextBox ID="NewNameField" Text="New item" runat="server"> 
        </asp:TextBox><asp:ImageButton ID="btnNew" runat="server" 
            ImageUrl="~/bvadmin/images/buttons/new.png" AlternateText="Add New Item" 
            onclick="btnNew_Click" />
    </asp:Panel>
</div>    
<div class="modal controlarea1">
<h1>Edit Item</h1>
<table class="formtable">
<tr>
    <td class="formlabel">Name:</td>
    <td class="formfield"><input type="text" cols="30" id="dialog-name" /></td>
</tr>
<tr>
    <td class="formlabel">Is a Label:</td>
    <td class="formfield"><input type="checkbox" id="dialog-label" /></td>
</tr>
<tr>
    <td class="formlabel">Price Adjustment:</td>
    <td class="formfield"><input type="text" cols="30" id="dialog-price" /></td>
</tr>
<tr>
    <td class="formlabel">Weight Adjustment:</td>
    <td class="formfield"><input type="text" cols="30" id="dialog-weight" /></td>
</tr>
</table>
<div class="editorcontrols">
    <a href="#" id="dialog-close">Close</a> <a href="#" id="dialog-save"><img src="../images/buttons/SaveChanges.png" alt="Save Changes" /></a>
</div>
    <input type="hidden" id="dialog-bvin" />
</div>