<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_Shipping" title="Untitled Page" Codebehind="Shipping.aspx.cs" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Shipping | Methods</h1>
        <uc1:MessageBox ID="MessageBox1" runat="server" EnableViewState="false" />                
        <asp:ImageButton ID="btnNew" runat="server" 
        AlternateText="Add New Shipping Method" 
        ImageUrl="~/BVAdmin/Images/Buttons/New.png" onclick="btnNew_Click" /><asp:DropDownList ID="lstProviders" runat="Server"></asp:DropDownList><br /><br />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="bvin"
            BorderColor="#CCCCCC" CellPadding="3" GridLines="None" 
        onrowdeleting="GridView1_RowDeleting" onrowediting="GridView1_RowEditing" 
        Width="100%">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Name" />
                <asp:CommandField ShowEditButton="True" />
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton OnClientClick="return window.confirm('Delete this Shipping Method?');" ID="LinkButton1"
                            runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle CssClass="row" />
            <HeaderStyle CssClass="rowheader" />
            <AlternatingRowStyle CssClass="alternaterow" />
        </asp:GridView>    
</asp:Content>

