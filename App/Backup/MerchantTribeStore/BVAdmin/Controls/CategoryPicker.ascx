<%@ Control Language="C#" AutoEventWireup="True"
    Inherits="MerchantTribeStore.BVAdmin_Controls_CategoryPicker" Codebehind="CategoryPicker.ascx.cs" %>
&nbsp;<asp:Panel ID="Panel1" runat="server" Width="600px" ScrollBars="Horizontal">
<asp:GridView CellPadding="2" ID="CategoriesGridView" runat="server" AutoGenerateColumns="False"
    DataKeyNames="bvin" Width="100%" GridLines="Horizontal">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:CheckBox ID="chkSelected" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Text" HeaderText="Name" />
    </Columns>
    <RowStyle CssClass="row" />
    <HeaderStyle CssClass="rowheader" />
    <AlternatingRowStyle CssClass="alternaterow" />
</asp:GridView>
</asp:Panel>
