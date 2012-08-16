<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_People_UserSignupConfig" title="Untitled Page" Codebehind="UserSignupConfig.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>User Sign-up Config</h1>
    <h2>Questions</h2><br />
    <asp:ImageButton ID="NewImageButton" AlternateText="New" runat="server" 
        ImageUrl="~/BVAdmin/Images/Buttons/New.png" onclick="NewImageButton_Click" />
    <br /><br />
    <asp:GridView ID="QuestionsGridView" runat="server" AutoGenerateColumns="False" 
        GridLines="none" CellPadding="5" DataKeyNames="bvin" 
        onrowcommand="QuestionsGridView_RowCommand" 
        onrowdeleting="QuestionsGridView_RowDeleting" 
        onrowediting="QuestionsGridView_RowEditing">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="Type" HeaderText="Type" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton AlternateText="Edit" CommandName="Edit" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Edit.png">
                    </asp:ImageButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>                
                    <asp:ImageButton AlternateText="Delete" CommandName="Delete" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Delete.png">
                    </asp:ImageButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton ID="moveUpButton" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Up.png" CommandName="MoveItem" CommandArgument="Up"/>
                    <asp:ImageButton ID="moveDownButton" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Down.png" CommandName="MoveItem" CommandArgument="Down" />
                </ItemTemplate>
                <ItemStyle Width="20px" />
            </asp:TemplateField>
        </Columns>
        <RowStyle CssClass="row" />
        <HeaderStyle CssClass="rowheader" />
        <AlternatingRowStyle CssClass="alternaterow" />
    </asp:GridView>
</asp:Content>

