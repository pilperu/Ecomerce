<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_ContentBlocks_Side_Menu_editor" Codebehind="editor.ascx.cs" %>
<div style="float: right;width:450px;margin-bottom:20px;text-align:left;">
 <h2>Modify/Create Link</h2>
 <asp:Panel id="pnlEditor" runat="server" DefaultButton="btnNew">
 <table border="0" cellspacing="0" cellpadding="3">
 <tr>
    <td class="formlabel">Link Text:</td>
    <td class="forminput"><asp:TextBox ID="LinkTextField" runat="Server" Columns="40"></asp:TextBox></td>
 </tr>
 <tr>
    <td class="formlabel">Link Url:</td>
    <td class="forminput"><asp:TextBox ID="LinkField" runat="Server" Columns="40"></asp:TextBox></td>
 </tr>
  <tr>
    <td class="formlabel">
        Tool Tip:</td>
    <td class="forminput"><asp:TextBox ID="AltTextField" runat="Server" Columns="40"></asp:TextBox></td>
 </tr>
 <tr>
    <td class="formlabel">
        CSS Class:</td>
    <td class="forminput"><asp:TextBox ID="CssClassField" runat="Server" Columns="40"></asp:TextBox></td>
 </tr>
 <tr>
    <td class="formlabel">Open in New Window:</td>
    <td class="forminput"><asp:CheckBox ID="OpenInNewWindowField" runat="server" /></td>
 </tr>
  <tr>
    <td class="formlabel"><asp:ImageButton ID="btnCancel" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/Cancel.png" onclick="btnCancel_Click" /></td>
    <td class="forminput"><asp:ImageButton ID="btnNew" runat="Server" 
            ImageUrl="~/BVAdmin/Images/buttons/New.png" onclick="btnNew_Click" /></td>
 </tr>
 </table>
 <asp:HiddenField ID="EditBvinField" runat="server" />
 </asp:Panel>
 </div>
 <div style="text-align:left;">&nbsp;
<asp:Panel ID="pnlMain" runat="server" DefaultButton="btnOkay">    
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                    CellPadding="3" GridLines="None" DataKeyNames="Id" Width="400px" 
                    onrowcancelingedit="GridView1_RowCancelingEdit" 
                    onrowdeleting="GridView1_RowDeleting" onrowediting="GridView1_RowEditing" 
                    onrowupdating="GridView1_RowUpdating">
                    <Columns>
                        <asp:HyperLinkField DataNavigateUrlFields="Setting2" DataTextField="Setting1" HeaderText="Link"
                            Target="_blank" />
                        <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton id="btnUp" runat="server" CommandName="Update" ImageUrl="~/BVAdmin/Images/buttons/up.png" AlternateText="Move Up" ></asp:ImageButton><br />
                    <asp:ImageButton id="btnDown" runat="server" CommandName="Cancel" ImageUrl="~/BVAdmin/Images/buttons/down.png" AlternateText="Move Down" ></asp:ImageButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="30px" />
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton id="btnEdit" runat="server" CommandName="Edit" ImageUrl="~/BVAdmin/Images/buttons/Edit.png" AlternateText="Edit" ></asp:ImageButton><br />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="80px" />
            </asp:TemplateField>
            <asp:CommandField ButtonType="Image" DeleteImageUrl="~/BVAdmin/images/Buttons/X.png"
                ShowDeleteButton="True" >
                <ItemStyle HorizontalAlign="Center" Width="30px" />
            </asp:CommandField>
                    </Columns>
                    <RowStyle CssClass="row" />
                    <HeaderStyle CssClass="rowheader" />
                    <AlternatingRowStyle CssClass="alternaterow" />
                </asp:GridView>
         &nbsp;<br />
          <table border="0" cellspacing="0" cellpadding="3">
 <tr>
    <td class="formlabel">Menu Title:</td>
    <td class="forminput"><asp:TextBox ID="TitleField" runat="Server" Columns="30"></asp:TextBox></td>
 </tr>
 </table>
         <asp:ImageButton ID="btnOkay" runat="server" 
                    ImageUrl="~/bvadmin/images/buttons/Ok.png" onclick="btnOkay_Click" />            
</asp:Panel>
</div>