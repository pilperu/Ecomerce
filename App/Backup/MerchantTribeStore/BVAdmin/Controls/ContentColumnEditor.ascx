<%@ Control Language="C#" AutoEventWireup="True"
    Inherits="MerchantTribeStore.BVAdmin_Controls_ContentColumnEditor" Codebehind="ContentColumnEditor.ascx.cs" %>
<div class="contentcolumneditor">
    <div style="margin: 0 0 10px 0;">
        <asp:DropDownList ID="lstBlocks" runat="server">
        </asp:DropDownList>&nbsp;<asp:ImageButton ID="btnNew" runat="server" AlternateText="Add New Content Block"
            ImageUrl="~/BVAdmin/Images/Buttons/New.png" onclick="btnNew_Click" /><br />
        <asp:Label ID="lblTitle" runat="server"></asp:Label>
    </div>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="bvin"
        BorderColor="#ccc" ShowHeader="false" CellPadding="0" CellSpacing="0" 
        GridLines="None" onrowcancelingedit="GridView1_RowCancelingEdit" 
        onrowdatabound="GridView1_RowDataBound" onrowdeleting="GridView1_RowDeleting" 
        onrowupdating="GridView1_RowUpdating">
        <Columns>
            <asp:TemplateField>
                <itemtemplate>
                <div class="controlarea2">
                <asp:ImageButton ID="btnUp" runat="server" CommandName="Update" ImageUrl="~/BVAdmin/images/buttons/Up.png"
                    AlternateText="Move Up"></asp:ImageButton> <asp:ImageButton ID="btnDown" runat="server" CommandName="Cancel" ImageUrl="~/BVAdmin/images/buttons/Down.png" AlternateText="Move Down"></asp:ImageButton> <asp:HyperLink ID="lnkEdit" ImageUrl="~/BVAdmin/Images/Buttons/Edit.png" runat="server" NavigateUrl='<%# Eval("bvin", "~/BVAdmin/Content/Columns_EditBlock.aspx?id={0}") %>' Text="Edit"></asp:HyperLink> <asp:ImageButton id="btnDelete" runat="server" imageurl="~/BVAdmin/Images/Buttons/X.png" commandname="Delete"></asp:ImageButton></div>
            </itemtemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle CssClass="rowheader" />
    </asp:GridView>
</div>
