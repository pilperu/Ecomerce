<%@ Control Language="C#" AutoEventWireup="True"
    Inherits="MerchantTribeStore.BVAdmin_Controls_ProductFilter" Codebehind="ProductFilter.ascx.cs" %>
<div id="filterDiv">
    <table align="right">
        <tr>
            <td>
                <span>Filters:
                    <asp:DropDownList ID="FilterDropDownList" runat="server">
                    </asp:DropDownList></span>
            </td>
        </tr>
        <tr>
            <td>
                <span><asp:LinkButton ID="LoadFilterLinkButton" runat="server">Load Filter</asp:LinkButton></span>&nbsp;&nbsp;
                <span><asp:LinkButton ID="DeleteFilterLinkButton" runat="server">Delete Filter</asp:LinkButton></span>
            </td>
        </tr>
        <tr>
        <td></td>
        </tr>
        <tr>
            <td>
                <span>New Filter:
                    <asp:TextBox ID="FilterNameTextBox" runat="server" ValidationGroup="Filter"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FilterNameTextBox"
                        ErrorMessage="You must enter a name for the filter." ValidationGroup="Filter" CssClass="errormessage" ForeColor=" ">*</asp:RequiredFieldValidator></span>
            </td>
        </tr>
        <tr>
            <td>
                <span><asp:LinkButton ID="SaveFilterLinkButton" runat="server" ValidationGroup="Filter">Save Filter</asp:LinkButton></span>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td class="formlabel">
                Keyword</td>
            <td class="formfield" style="width: 367px">
                <asp:TextBox ID="KeywordTextBox" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="formlabel">
                Keyword Is Exact Match?</td>
            <td class="formfield" style="width: 367px">
                <asp:CheckBox ID="ExactMatchCheckBox" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="formlabel">
                Vendor</td>
            <td class="formfield" style="width: 367px">
                <asp:DropDownList ID="VendorDropDownList" runat="server" AppendDataBoundItems="True">
                    <asp:ListItem Selected="True">Any</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="formlabel">
                Manufacturer</td>
            <td class="formfield" style="width: 367px">
                <asp:DropDownList ID="ManufacturerDropDownList" runat="server" AppendDataBoundItems="True">
                    <asp:ListItem Selected="True">Any</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="formlabel">
                Price Range (leave blank for none)</td>
            <td class="formfield" style="width: 367px">
                <asp:TextBox ID="FromPriceTextBox" runat="server"></asp:TextBox>
                <asp:CustomValidator ID="MonetaryRegularExpressionValidator1" runat="server"
                    ControlToValidate="FromPriceTextBox" Display="Dynamic" ErrorMessage='"From" price range must be a monetary value' CssClass="errormessage" ForeColor=" ">*</asp:CustomValidator>
                to
                <asp:TextBox ID="ToPriceTextBox" runat="server"></asp:TextBox>
                <asp:CustomValidator ID="MonetaryRegularExpressionValidator2" runat="server"
                    ControlToValidate="ToPriceTextBox" Display="Dynamic" ErrorMessage='"To" price range must be a monetary value' CssClass="errormessage" ForeColor=" ">*</asp:CustomValidator></td>
        </tr>
        <tr>
            <td class="formlabel" style="height: 26px">
                Last X number of items added to store</td>
            <td class="formfield" style="height: 26px; width: 367px;">
                <asp:TextBox ID="NumberOfItemsAddedToStoreTextBox" runat="server" Width="38px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="formlabel" style="height: 26px">
                Items added in the past X days</td>
            <td class="formfield" style="height: 26px; width: 367px;">
                <asp:TextBox ID="ItemsAddedInThePastXDaysTextBox" runat="server" Width="38px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="formlabel">
                Product Type</td>
            <td class="formfield" style="width: 367px">
                <asp:DropDownList ID="ProductTypeDropDownList" runat="server" AppendDataBoundItems="True">
                    <asp:ListItem Selected="True">Any</asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td class="formlabel">
                Sort By</td>
            <td class="formfield" style="width: 367px">
                <asp:DropDownList ID="SortByDropDownList" runat="server" AppendDataBoundItems="True">
                    <asp:ListItem Value="0">Product Name</asp:ListItem>
                    <asp:ListItem Value="1">Manufacturer Name</asp:ListItem>
                    <asp:ListItem Value="2">Creation Date</asp:ListItem>
                    <asp:ListItem Value="3">Site Price</asp:ListItem>
                    <asp:ListItem Value="4">Vendor</asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td class="formlabel">
                Sort Order</td>
            <td class="formfield" style="width: 367px">
                <asp:DropDownList ID="SortOrderDropDownList" runat="server" AppendDataBoundItems="True">
                    <asp:ListItem Value="0">Ascending</asp:ListItem>
                    <asp:ListItem Value="1">Descending</asp:ListItem>
                </asp:DropDownList></td>
        </tr>
    </table>
</div>
<h2>
    Product Shared Choices</h2>
<asp:Panel ID="sharedChoicesPanel" CssClass="sharedChoicesDiv" runat="server" Visible="False">
    <table>
        <tr>
            <td>
                <asp:DropDownList ID="SharedChoiceDropDownList" runat="server" AutoPostBack="True">
                </asp:DropDownList></td>
            <td>
                <asp:DropDownList ID="SharedChoiceOptionDropDownList" runat="server">
                </asp:DropDownList></td>
            <td>
                <asp:ImageButton ID="AddChoiceImageButton" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Add.png" /></td>
        </tr>
    </table>
    <asp:GridView ID="ChoicesAndOptionsGridView" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField HeaderText="Choice Name" DataField="ChoiceName" />
            <asp:BoundField HeaderText="Choice Option Name" DataField="ChoiceOptionName" />
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:ImageButton ID="DeleteImageButton" runat="server" CausesValidation="false" CommandName="Delete"
                        ImageUrl="~/BVAdmin/Images/Buttons/Delete.png" AlternateText="Delete" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <RowStyle CssClass="row" />
        <HeaderStyle CssClass="rowheader" />
        <AlternatingRowStyle CssClass="alternaterow" />
        <EmptyDataTemplate>
            There are no currently selected Product Choices
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Panel>
