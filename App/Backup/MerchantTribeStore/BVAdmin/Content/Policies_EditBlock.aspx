<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Content_Policies_EditBlock" title="Untitled Page" Codebehind="Policies_EditBlock.aspx.cs" %>

<%@ Register Src="../Controls/HtmlEditor.ascx" TagName="HtmlEditor" TagPrefix="uc2" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>Edit Policy Block</h1>
    <uc1:MessageBox ID="msg" runat="server" />
    <div style="text-align: center; margin: auto;">
       <asp:Panel ID="pnlMain" runat="server" >
        <table border="0" cellspacing="0" cellpadding="3">
            <tr>
                <td class="formlabel">
                    Name:</td>
                <td class="formfield">
                    <asp:TextBox ID="NameField" runat="server" Columns="80" TabIndex="2000" Width="700px"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="formlabel">
                    Description:</td>
                <td class="formfield">
                    <uc2:HtmlEditor ID="DescriptionField" runat="server" EditorHeight="300" EditorWidth="700"
                        EditorWrap="true" />
                </td>
            </tr>
            <tr>
                <td class="formlabel">
                    <asp:ImageButton ID="btnCancel" TabIndex="2501" runat="server" ImageUrl="../images/buttons/Cancel.png"
                        CausesValidation="False" onclick="btnCancel_Click"></asp:ImageButton></td>
                <td class="formfield">
                    <asp:ImageButton ID="btnSaveChanges" TabIndex="2500" runat="server" 
                        ImageUrl="../images/buttons/SaveChanges.png" onclick="btnSaveChanges_Click">
                    </asp:ImageButton></td>
            </tr>
        </table>
    </asp:Panel>
    </div>
    <asp:HiddenField ID="BvinField" runat="server" />
    <asp:HiddenField ID="PolicyIdField" runat="server" />
</asp:Content>


