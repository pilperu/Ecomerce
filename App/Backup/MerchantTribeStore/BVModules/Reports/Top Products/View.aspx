<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Reports_Products" title="Top Products" Codebehind="View.aspx.cs" %>

<%@ Register Src="~/BVAdmin/Controls/DateRangePicker.ascx" TagName="DateRangePicker" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>
        Sales By Product</h1>
<p>
    &nbsp;<uc1:DateRangePicker ID="DateRangeField" runat="server" />
    <p>
        <br /><br />
    <asp:ImageButton ImageUrl="~/BVAdmin/Images/Buttons/OK.png" id="btnShow" 
            runat="server" CausesValidation="False" Visible="false" 
            onclick="btnShow_Click" />
</p>             
        <asp:Label ID="lblResults" runat="server"></asp:Label>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="bvin"
        BorderColor="#CCCCCC" CellPadding="3" GridLines="None" Width="100%"  
        AllowPaging="True" AllowSorting="True" PagerSettings-Mode="Numeric" 
        onpageindexchanging="GridView1_PageIndexChanging" 
        onrowediting="GridView1_RowEditing">
        <Columns>
            <asp:BoundField DataField="SKU" HeaderText="SKU" />
            <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
            <asp:BoundField DataField="Total Ordered" HeaderText="Total Ordered" DataFormatString="{0:f0}" HtmlEncode="false" Visible="false" />
        </Columns>
        <RowStyle CssClass="row" />
        <HeaderStyle CssClass="rowheader" />
        <AlternatingRowStyle CssClass="alternaterow" />
    </asp:GridView>
    
</asp:Content>

