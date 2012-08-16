<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Content_Policies_Edit" title="Untitled Page" Codebehind="Policies_Edit.aspx.cs" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>
        Edit Policy -
        <asp:Label ID="lblTitle" runat="server"></asp:Label></h1>
    <uc1:MessageBox ID="msg" runat="server" />    
    <asp:GridView ID="GridView1" runat="server" ShowHeader="false" 
        AutoGenerateColumns="False" DataKeyNames="bvin"
        BorderColor="#CCCCCC" CellPadding="3" GridLines="None" Width="100%" 
        onrowcancelingedit="GridView1_RowCancelingEdit" 
        onrowdatabound="GridView1_RowDataBound" onrowdeleting="GridView1_RowDeleting" 
        onrowupdating="GridView1_RowUpdating">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                <strong><asp:Label id="lblBlockName" runat="server"></asp:Label><br /></strong>
                <asp:Label ID="lblBlockDescription" runat="server"></asp:Label>                
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton ID="btnUp" runat="server" CommandName="Update" ImageUrl="~/BVAdmin/images/buttons/Up.png"
                        AlternateText="Move Up"></asp:ImageButton><br />
                    <asp:ImageButton ID="btnDown" runat="server" CommandName="Cancel" ImageUrl="~/BVAdmin/images/buttons/Down.png"
                        AlternateText="Move Down"></asp:ImageButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="30px" />
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink ID="lnkEdit" ImageUrl="~/BVAdmin/Images/Buttons/Edit.png" runat="server"
                        NavigateUrl='<%# Eval("bvin", "Policies_EditBlock.aspx?id={0}") %>' Text="Edit"></asp:HyperLink>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="80px" />
            </asp:TemplateField>
            <asp:CommandField ButtonType="Image" DeleteImageUrl="~/BVAdmin/Images/Buttons/X.png"
                ShowDeleteButton="True">
                <ItemStyle HorizontalAlign="Center" Width="80px" />
            </asp:CommandField>
        </Columns>
        <RowStyle CssClass="row" />
        <HeaderStyle CssClass="rowheader" />
        <AlternatingRowStyle CssClass="row" />
    </asp:GridView>
    &nbsp;
    <div style="text-align: right;">
        <asp:ImageButton ID="btnOk" runat="server" AlternateText="Back to Policy List" 
            ImageUrl="~/BVAdmin/Images/Buttons/OK.png" onclick="btnOk_Click" />&nbsp;&nbsp;
        <asp:ImageButton ID="btnNew" runat="server" AlternateText="Add New Policy Block"
            ImageUrl="~/BVAdmin/Images/Buttons/New.png" onclick="btnNew_Click" />
    </div>    
    <asp:HiddenField ID="PolicyIDField" runat="server" />
</asp:Content>

