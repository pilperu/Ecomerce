<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Content_Policies" Title="Untitled Page" Codebehind="Policies.aspx.cs" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>
        Policies</h1>
    <uc1:MessageBox ID="msg" runat="server" />
    <asp:Panel id="pnlHeader" runat="Server" DefaultButton="btnNew">
    <table border="0" cellspacing="0" cellpadding="0" width="100%">
        <tr>
            <td align="left" valign="middle">
                <h2>
                    <asp:Label ID="lblResults" runat="server">No Results</asp:Label></h2>
            </td>
            <td align="right" valign="middle">
                <div style="display:none;">

                 New Policy Name:
                        <asp:TextBox ID="NewNameField" runat="server"></asp:TextBox>
                 <asp:ImageButton ID="btnNew" runat="server" AlternateText="Add New Policy" 
                     ImageUrl="~/BVAdmin/Images/Buttons/New.png" onclick="btnNew_Click" /></div></td>
        </tr>
    </table></asp:Panel>
    &nbsp;
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="bvin"
        BorderColor="#CCCCCC" CellPadding="3" GridLines="None" Width="100%" 
        onrowdeleting="GridView1_RowDeleting" onrowediting="GridView1_RowEditing">
        <Columns>
            <asp:BoundField DataField="Title" HeaderText="Policy Name" />
            <asp:CheckBoxField DataField="SystemPolicy" HeaderText="System Policy" />
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:ImageButton 
                        ID="LinkButton2" 
                        RunAt="server" 
                        ImageUrl="~/BVAdmin/Images/Buttons/Edit.png" 
                        CausesValidation="False" 
                        CommandName="Edit" 
                        AlternateText="Edit" />
                </ItemTemplate>                
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False" Visible="false">
                <ItemTemplate>
                    <asp:LinkButton OnClientClick="return window.confirm('Delete this policy?');" ID="LinkButton1"
                        runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <RowStyle CssClass="row" />
        <HeaderStyle CssClass="rowheader" />
        <AlternatingRowStyle CssClass="alternaterow" />
    </asp:GridView>
</asp:Content>
