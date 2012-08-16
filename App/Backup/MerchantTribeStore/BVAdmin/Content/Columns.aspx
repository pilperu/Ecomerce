<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Content_Columns" title="Untitled Page" Codebehind="Columns.aspx.cs" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<h1>Content Columns</h1>
    <uc1:MessageBox ID="msg" runat="server" />
   <table border="0" cellspacing="0" cellpadding="0" width="100%">
                <tr>
                    <td align="left" valign="middle"><h2>
                        <asp:Label ID="lblResults" runat="server">No Results</asp:Label></h2></td>                   
                    <td class="formlabel" align="right" valign="middle">
                        New Column Name:
                        <asp:TextBox ID="NewNameField" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="btnNew" runat="server" 
                            AlternateText="Add New Content Column" 
                            ImageUrl="~/BVAdmin/Images/Buttons/New.png" onclick="btnNew_Click" /></td>
                </tr>
            </table>
        &nbsp;
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="bvin"
        BorderColor="#CCCCCC" CellPadding="3" GridLines="None" Width="100%" 
        onrowdeleting="GridView1_RowDeleting" onrowediting="GridView1_RowEditing">
        <Columns>
            <asp:BoundField DataField="DisplayName" HeaderText="Column Name" />
            <asp:CheckBoxField DataField="SystemColumn" HeaderText="System Column" />
            <asp:CommandField ShowEditButton="True" />
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton OnClientClick="return window.confirm('Delete this content column?');" ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                        Text="Delete"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <RowStyle CssClass="row" />
        <HeaderStyle CssClass="rowheader" />
        <AlternatingRowStyle CssClass="alternaterow" />
    </asp:GridView>
</asp:Content>

