<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Controls_ProductPicker"
    CodeBehind="ProductPicker.ascx.cs" %>
<div id="productpicker">
    <asp:Panel ID="pnlMain" Width="300" runat="server" DefaultButton="btnGo">
        <div class="controlarea1" style="margin: 5px 0px 10px 0px;">
            <div style="padding: 5px;">
                Search:
                <asp:TextBox ID="FilterField" runat="server" Width="160px"></asp:TextBox>
                <asp:ImageButton ID="btnGo" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Go.png"
                    CausesValidation="False" OnClick="btnGo_Click" /><br />
                <asp:DropDownList ID="ManufacturerFilter" runat="server" Width="280px" AutoPostBack="True"
                    OnSelectedIndexChanged="ManufacturerFilter_SelectedIndexChanged">
                    <asp:ListItem Text="- Any Manufacturer -"></asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:DropDownList ID="VendorFilter" runat="server" Width="280px" AutoPostBack="True"
                    OnSelectedIndexChanged="VendorFilter_SelectedIndexChanged">
                    <asp:ListItem Text="- Any Vendor -"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="CategoryFilter" runat="server" Width="280px" AutoPostBack="True"
                    OnSelectedIndexChanged="CategoryFilter_SelectedIndexChanged">
                    <asp:ListItem Text="- Any Category -"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </asp:Panel>
    <div style="height: 30px;">
        <div style="float: left;">
            Page:
            <asp:DropDownList ID="lstPage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstPage_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div style="float: right;">
            Items per page:
            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem Selected="True" Value="10"></asp:ListItem>
                <asp:ListItem>25</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div class="productpickergrid">
        <asp:GridView CellPadding="2" ID="GridView1" runat="server" AutoGenerateColumns="False"
            DataKeyNames="bvin" Width="100%" GridLines="Horizontal" OnRowDataBound="GridView1_RowDataBound">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <a href="#" class="pickerallbutton">All</a>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="pickercheck">
                            <asp:CheckBox ID="chkSelected" runat="server" />
                            <asp:Literal ID="radioButtonLiteral" runat="server"></asp:Literal>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="sku" HeaderText="SKU" />
                <asp:BoundField DataField="ProductName" HeaderText="Name" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Label ID="PriceLabel" runat="server" Text="" EnableViewState="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Label ID="InventoryLabel" runat="server" Text="" EnableViewState="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle CssClass="row" />
            <HeaderStyle CssClass="rowheader" />
            <AlternatingRowStyle CssClass="alternaterow" />
        </asp:GridView>
    </div>
    <asp:HiddenField ID="ExcludeCategoryBvinField" runat="server" />
    <asp:HiddenField ID="currentpagefield" runat="server" />
</div>
