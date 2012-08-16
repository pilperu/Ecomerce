<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="editor.ascx.cs" Inherits="MerchantTribeStore.BVModules.ContentBlocks.ImageRotator.editor" %>
<div style="float: right;width:450px;margin-bottom:20px;text-align:left;">
 <h2>Add/Modify Image</h2>
 <asp:Panel id="pnlEditor" runat="server" DefaultButton="btnNew">
 <table border="0" cellspacing="0" cellpadding="3">
 <tr>
    <td class="formlabel">Image Url:</td>
    <td class="forminput"><asp:TextBox ID="ImageUrlField" runat="Server" Columns="20"></asp:TextBox>
        <a href="javascript:popUpWindow('?returnScript=SetImage&WebMode=1');"><asp:Image ID="imgSelect1" runat="server" ImageUrl="~/BVAdmin/images/buttons/Select.png" /></a></td>
 </tr>
 <tr>
    <td class="formlabel">Link To:</td>
    <td class="forminput"><asp:TextBox ID="ImageLinkField" runat="Server" Columns="40"></asp:TextBox></td>
 </tr>
  <tr>
    <td class="formlabel">
        Tool Tip:</td>
    <td class="forminput"><asp:TextBox ID="AltTextField" runat="Server" Columns="40"></asp:TextBox></td>
 </tr>
 <tr>
    <td class="formlabel">Open in New Window:</td>
    <td class="forminput"><asp:CheckBox ID="chkOpenInNewWindow" runat="server" /></td>
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
                        <asp:ImageField DataImageUrlField="Setting1" HeaderText="Image" NullDisplayText="No Image">
                        </asp:ImageField>
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
            <table border="0" cellspacing="0" cellpadding="3">
            <tr>
                <td class="formlabel">&nbsp;</td>
                <td class="forminput"><asp:CheckBox ID="chkShowInOrder" CssClass="formlabel" Text="Rotate images in the order shown above" runat="server" /></td>
           </tr>
           <tr>
            <td class="formlabel">CSS Class:</td>
            <td class="forminput"><asp:TextBox ID="cssclass" runat="server" Columns="40"></asp:TextBox>(optional)</td>
           </tr>
           <tr>
                <td class="formlabel">Pause for:</td>
                <td class="forminput"><asp:TextBox ID="PauseField" Columns="5" runat="server" >2</asp:TextBox> seconds</td>
           </tr>
            <tr>
                <td class="formlabel">Size:</td>
                <td class="forminput">w: <asp:TextBox id="WidthField" runat="server" Columns="5"></asp:TextBox> h: <asp:TextBox id="HeighField" runat="server" Columns="5"></asp:TextBox></td>
           </tr>
               <tr>
                <td class="formlabel">&nbsp;</td>
                <td class="forminput"><asp:ImageButton ID="btnOkay" runat="server" 
                        ImageUrl="~/bvadmin/images/buttons/Ok.png" onclick="btnOkay_Click" /></td>
           </tr>
            </table>                     
</asp:Panel>
</div>