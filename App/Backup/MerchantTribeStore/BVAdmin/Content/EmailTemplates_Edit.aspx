<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master"
    AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Content_EmailTemplates_Edit"
    Title="Untitled Page" Codebehind="EmailTemplates_Edit.aspx.cs" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>
<%@ Register Src="../Controls/HtmlEditor.ascx" TagName="HtmlEditor" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>
        Edit 
        Html Template</h1>
    <uc2:MessageBox ID="MessageBox1" runat="server" />
    <asp:Label ID="lblError" runat="server" CssClass="errormessage"></asp:Label>
    <table border="0" cellspacing="0" cellpadding="0" width="910">
        <tr>
            <td colspan="2" align="center">
                <asp:Panel ID="pnlMain" runat="server">
                    <table border="0" cellspacing="0" cellpadding="3">
                        <tr>
                            <td class="formlabel">
                                Template Name:
                            </td>
                            <td class="formfield">
                                <asp:TextBox ID="DisplayNameField" runat="server" Columns="50" Width="400px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DisplayNameField"
                                    ErrorMessage="A Template Name is Required">*</asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td class="formlabel">
                                Template Type:
                            </td>
                            <td class="formfield"><asp:DropDownList ID="lstTemplateType" runat="server">
                                    <asp:ListItem Value="0" Text="Custom"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="New Order to Customer"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="New Order To Admin"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Order Update"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="Order Shipment"></asp:ListItem>
                                    <asp:ListItem Value="100" Text="Forgot Password"></asp:ListItem>
                                    <asp:ListItem Value="101" Text="Email a Friend"></asp:ListItem>
                                    <asp:ListItem Value="102" Text="Contact Form to Admin"></asp:ListItem>
                                    <asp:ListItem Value="200" Text="Drop Shipping Notice"></asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="formlabel">
                                From Email:
                            </td>
                            <td class="formfield">
                                <asp:TextBox ID="FromField" runat="server" Columns="50" Width="400px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="formlabel">
                                Subject:
                            </td>
                            <td class="formfield">
                                <asp:TextBox ID="SubjectField" runat="server" Columns="50" Width="400px"></asp:TextBox></td>
                        </tr>                        
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <table border="0" cellspacing="0" cellpadding="3">
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
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <span class="smalltext">Body:</span><br />
                <asp:TextBox ID="BodyField" runat="server" Columns="60" Rows="10" Height="380px"
                    Width="600px" TextMode="multiline" Wrap="false"></asp:TextBox><input type="button" id="btnHTMLBody" value="<" onclick="MoveEmailVars()" runat="server" /><br />
                &nbsp;<br />
                <span class="smalltext">Repeating Section:</span><br />
                <asp:TextBox ID="RepeatingSectionField" runat="server" Columns="60" Rows="10" Height="175px"
                    Width="600px" TextMode="MultiLine" Wrap="false"></asp:TextBox><input type="button" id="btnHTMLRepeating" value="<" onclick="MoveEmailVars2()" runat="server" /><br />
            </td>
            <td align="right" valign="top">
                <span class="smalltext">Available Tags:</span><br />
                <asp:ListBox ID="Tags" style="font-size:11px;" runat="server" Width="220" 
                    Height="600px" SelectionMode="single"></asp:ListBox></td>
        </tr>
        
    </table>
    <asp:HiddenField ID="BvinField" runat="server" />
</asp:Content>
