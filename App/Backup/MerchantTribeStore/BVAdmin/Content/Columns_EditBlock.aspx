<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master"
    AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Content_Columns_EditBlock"
    Title="Untitled Page" Codebehind="Columns_EditBlock.aspx.cs" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>
        <asp:Label ID="TitleLabel" runat="server" Text="Edit Content Block"></asp:Label></h1>
    <uc1:MessageBox ID="msg" runat="server" />
    <div style="text-align: center; margin: auto;">
        <asp:PlaceHolder ID="phEditor" runat="server"></asp:PlaceHolder>
        <div style="margin-top: 100px; margin-bottom: 10px; margin-right: auto; text-align: left;
            width: 475px;">
            <h2>
                Advanced Options</h2>
                <table border="0" cellspacing="0" cellpadding="2">
                    <tr>
                        <td class="formlabel">
                            Copy To:</td>
                        <td class="formfield">
                            <asp:DropDownList ID="CopyToList" runat="server" Width="350px">
                            </asp:DropDownList>
                            <asp:ImageButton ID="btnGoCopy" runat="server" 
                                ImageUrl="~/BVAdmin/Images/Buttons/Go.png" onclick="btnGoCopy_Click" /></td>
                    </tr>
                    <tr>
                        <td class="formlabel">
                            Move To:</td>
                        <td class="formfield">
                            <asp:DropDownList ID="MoveToList" runat="server" Width="350px">
                            </asp:DropDownList>
                            <asp:ImageButton ID="btnGoMove" runat="server" 
                                ImageUrl="~/BVAdmin/Images/Buttons/Go.png" onclick="btnGoMove_Click" /></td>
                    </tr>
                </table>            
        </div>
    </div>
    <asp:HiddenField ID="BlockIDField" runat="server" />
</asp:Content>
