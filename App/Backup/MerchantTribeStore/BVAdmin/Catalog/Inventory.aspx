<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Catalog_Inventory" title="Inventory" Codebehind="Inventory.aspx.cs" %>

<%@ Register Src="../Controls/SimpleProductFilter.ascx" TagName="SimpleProductFilter"
    TagPrefix="uc3" %>

<%@ Register Src="../Controls/InventoryModifications.ascx" TagName="InventoryModifications"
    TagPrefix="uc2" %>

<%@ Register Src="../Controls/ProductFilter.ascx" TagName="ProductFilter" TagPrefix="uc1" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Inventory Edit</h1>
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            &nbsp;
            <uc3:SimpleProductFilter ID="SimpleProductFilter" runat="server" />
        </asp:View>
        <asp:View ID="View2" runat="server">
            <asp:Label id="topLabel" runat="server">Note: Changing the page will save all changes on current page.</asp:Label>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                Width="100%" DataKeyNames="Bvin" AllowPaging="True" 
                DataSourceID="ObjectDataSource1" 
                onpageindexchanging="GridView1_PageIndexChanging" 
                onrowdatabound="GridView1_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="ProductName" HeaderText="Name" />
                    <asp:BoundField DataField="Sku" HeaderText="Sku" />
                    <asp:BoundField DataField="SitePrice" HeaderText="Site Price" HtmlEncode="False" DataFormatString="{0:c}" />
                    <asp:TemplateField HeaderText="Qty Available">
                        <ItemTemplate>
                            <asp:Label ID="QuantityAvailableLabel" runat="server" Text="Label"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Out Of Stock Point">
                        <ItemTemplate>
                            <asp:Label ID="OutOfStockPointLabel" runat="server" Text="Label"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity Reserved">
                        <ItemTemplate>
                            <asp:Label ID="QuantityReservedLabel" runat="server" Text="Label"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField>
                        <ItemTemplate>
                            <uc2:InventoryModifications id="InventoryModifications" runat="server"></uc2:InventoryModifications>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <RowStyle CssClass="row" />
                <HeaderStyle CssClass="rowheader" />
                <AlternatingRowStyle CssClass="alternaterow" />
                <EmptyDataTemplate>
                    No Products Were Found.
                </EmptyDataTemplate>
                <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
            </asp:GridView>
            <asp:Label id="BottomLabel" runat="server">Note: Changing the page will save all changes on current page.</asp:Label>
            <asp:ImageButton ID="SaveChangesImageButton" runat="server" 
                ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" 
                onclick="SaveChangesImageButton_Click" />
            <asp:ImageButton ID="ImageButton3" runat="server" 
                ImageUrl="~/BVAdmin/Images/Buttons/Cancel.png" onclick="ImageButton3_Click" />
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True" OldValuesParameterFormatString="original_{0}"
                SelectCountMethod="GetRowCount" SelectMethod="FindByCriteria" 
                TypeName="MerchantTribe.Commerce.Catalog.Product" 
                onselected="ObjectDataSource1_Selected" 
                onselecting="ObjectDataSource1_Selecting">
                <SelectParameters>
                    <asp:SessionParameter Name="criteria" SessionField="InventoryCriteria" Type="Object" />
                    <asp:Parameter Direction="Output" Name="rowCount" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </asp:View>
    </asp:MultiView>
</asp:Content>

