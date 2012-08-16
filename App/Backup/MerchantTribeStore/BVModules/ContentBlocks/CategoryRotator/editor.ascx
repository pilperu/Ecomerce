<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_ContentBlocks_Category_Rotator_editor" Codebehind="editor.ascx.cs" %>
<%@ Register Src="../../../BVAdmin/Controls/CategoryPicker.ascx" TagName="CategoryPicker"
    TagPrefix="uc1" %>
<asp:Panel ID="pnlMain" DefaultButton="btnOk" runat="server">
<table border="0" cellspacing="0" cellpadding="3">
<tr>
    <td class="formlabel">
        <asp:GridView ID="CategoriesGridView" runat="server" DataKeyNames="bvin" 
            AutoGenerateColumns="false" 
            onrowcancelingedit="CategoriesGridView_RowCancelingEdit" 
            onrowdeleting="CategoriesGridView_RowDeleting" 
            onrowupdating="CategoriesGridView_RowUpdating">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Category Name" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnUp" runat="server" CommandName="Update" ImageUrl="~/BVAdmin/Images/buttons/up.png"
                            AlternateText="Move Up"></asp:ImageButton><br />
                        <asp:ImageButton ID="btnDown" runat="server" CommandName="Cancel" ImageUrl="~/BVAdmin/Images/buttons/down.png"
                            AlternateText="Move Down"></asp:ImageButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                </asp:TemplateField>
                <asp:CommandField ButtonType="Image" DeleteImageUrl="~/BVAdmin/images/Buttons/X.png"
                    ShowDeleteButton="True">
                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                </asp:CommandField>
            </Columns>
            <EmptyDataTemplate>
                There are no selected categories.
            </EmptyDataTemplate>
            <RowStyle CssClass="row" />
            <HeaderStyle CssClass="rowheader" />
            <AlternatingRowStyle CssClass="alternaterow" />
        </asp:GridView>
    </td>        
    <td class="formlabel">
        <asp:ImageButton ID="AddImageButton" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/Add.png" onclick="AddImageButton_Click" />
    </td>
    <td class="formfield">
        <uc1:CategoryPicker ID="CategoryPicker1" runat="server" />
    </td>
</tr>
<tr>
    <td class="formlabel">
        &nbsp;</td>
    <td class="formfield" colspan="2">
        <asp:CheckBox ID="chkShowInOrder" CssClass="formlabel" Text="Rotate products in the order shown above"
            runat="server" /></td>
</tr>
<tr>
    <td class="formlabel">
        &nbsp;<asp:ImageButton ID="btnOk" runat="server" 
            ImageUrl="~/bvadmin/images/buttons/Ok.png" onclick="btnOK_Click" /></td>
    <td class="formfield" colspan="2">
        </td>
</tr>
</table></asp:Panel>
