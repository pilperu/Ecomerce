<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_ContentBlocks_Product_Grid_editor" Codebehind="editor.ascx.cs" %>
<%@ Register Src="../../../BVAdmin/Controls/ProductPicker.ascx" TagName="ProductPicker"
    TagPrefix="uc1" %>
<div style="float: right; width: 450px; margin-bottom: 20px; text-align: left;">
    <h2>
        Products</h2>

    <asp:Panel ID="pnlEditor" runat="server" DefaultButton="btnNew">
        <uc1:ProductPicker ID="ProductPicker1" runat="server" DisplayKits="true" />
        <table border="0" cellspacing="0" cellpadding="3">
            <tr>
                <td class="forminput">
                    <asp:ImageButton ID="btnNew" runat="Server" 
                        ImageUrl="~/BVAdmin/Images/buttons/Add.png" onclick="btnNew_Click" /></td>
                <td class="formlabel">
                    </td>
            </tr>
        </table>
        <asp:HiddenField ID="EditBvinField" runat="server" />
    </asp:Panel>
</div>
<div style="text-align: left;">
    &nbsp;
    <asp:Panel ID="pnlMain" runat="server" DefaultButton="btnOkay">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="3"
            GridLines="None" DataKeyNames="Id" Width="400px" 
            onrowcancelingedit="GridView1_RowCancelingEdit" 
            onrowdeleting="GridView1_RowDeleting" onrowupdating="GridView1_RowUpdating">
            <Columns>
                <asp:TemplateField HeaderText="Product Image">
                <ItemTemplate>
                <img src="../../<%#Eval("Setting4") %>" />
                </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Setting3" HeaderText="Product Name" />
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
            <RowStyle CssClass="row" />
            <HeaderStyle CssClass="rowheader" />
            <AlternatingRowStyle CssClass="alternaterow" />
        </asp:GridView>
        <table border="0" cellspacing="0" cellpadding="3">
            <tr>
                <td class="formlabel">
                    &nbsp;Grid Columns</td>
                <td class="forminput">
                    <asp:TextBox ID="GridColumnsField" runat="server" Columns="5"></asp:TextBox>
                    <asp:RegularExpressionValidator ControlToValidate="GridColumnsField" runat="server" ID="valGridColumns" ForeColor=" " CssClass="errormessage"
                    ValidationExpression="[1-9]" Display="dynamic" ErrorMessage="Please Enter a Numeric Value"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="formlabel">
                    &nbsp;</td>
                <td class="forminput">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="formlabel">
                    &nbsp;</td>
                <td class="forminput">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="formlabel">
                    &nbsp;</td>
                <td class="forminput">
                    <asp:ImageButton ID="btnOkay" runat="server" 
                        ImageUrl="~/bvadmin/images/buttons/Ok.png" onclick="btnOkay_Click" />
                    <asp:ImageButton ID="btnCancel" runat="server" 
                        ImageUrl="~/BVAdmin/Images/Buttons/Cancel.png" CausesValidation="False" 
                        onclick="btnCancel_Click" /></td>
            </tr>
        </table>
    </asp:Panel>
</div>
