<%@ Page MasterPageFile="~/BVAdmin/BVAdmin.Master" ValidateRequest="False" Language="C#"
    AutoEventWireup="True" Inherits="MerchantTribeStore.Products_ProductProperties_Edit" Codebehind="ProductTypePropertiesEdit.aspx.cs" %>

<%@ Register Src="../Controls/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Edit Type Property</h1>
    <uc1:MessageBox ID="msg" runat="server" />    
    <table class="FormTable">
        <tr>
            <td class="FormLabel" align="right">
                Property Name
            </td>
            <td class="FormLabel" align="left">
                <asp:TextBox ID="PropertyNameField" runat="server" CssClass="FormInput" Columns="40"></asp:TextBox><asp:RequiredFieldValidator
                    ID="RequiredFieldValidator1" runat="server" ErrorMessage="Property Name is Required"
                    ControlToValidate="PropertyNameField">*</asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td class="FormLabel" align="right">
                Display Name
            </td>
            <td class="FormLabel" align="left">
                <asp:TextBox ID="DisplayNameField" runat="server" CssClass="FormInput" Columns="40"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="FormLabel" align="right">
                Display On Site?
            </td>
            <td class="FormLabel" align="left">
                <asp:CheckBox ID="chkDisplayOnSite" runat="server"></asp:CheckBox></td>
        </tr>
        <tr>
            <td class="FormLabel" align="right">
                Display To Drop Shipper?
            </td>
            <td class="FormLabel" align="left">
                <asp:CheckBox ID="chkDisplayToDropShipper" runat="server"></asp:CheckBox></td>
        </tr>
        <asp:Panel ID="pnlCultureCode" Visible="False" runat="server">
            <tr>
                <td class="FormLabel" align="right">
                    Currency Symbol
                </td>
                <td class="FormLabel" align="left">
                    <asp:DropDownList ID="lstCultureCode" runat="server" CssClass="FormInput">
                    </asp:DropDownList></td>
            </tr>
        </asp:Panel>
        <tr>
            <td class="FormLabel" align="right">                
                <p id="ChoiceNote" runat="server" style="width: 150px;"></p>
            </td>
            <td class="FormLabel" align="left">
                <asp:TextBox ID="DefaultValueField" runat="server" CssClass="FormInput" Columns="40"></asp:TextBox>
                <table cellspacing="0" cellpadding="3" border="0">
                    <tr>
                        <td>
                            <asp:ListBox ID="lstDefaultValue" runat="server" Rows="10"></asp:ListBox></td>
                        <td style="width: 163px">
                            <asp:Panel ID="pnlChoiceControls" Visible="False" runat="server">
                                <asp:ImageButton ID="btnMoveUp" runat="server" ImageUrl="~/BVAdmin/images/buttons/up.png"
                                    AlternateText="Move Up" onclick="btnMoveUp_Click"></asp:ImageButton>
                                <br />
                                <br />
                                <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/BVAdmin/images/buttons/delete.png"
                                    AlternateText="Delete" onclick="btnDelete_Click"></asp:ImageButton>
                                <br />
                                <asp:TextBox ID="NewChoiceField" CssClass="FormInput" Columns="20" runat="server"></asp:TextBox>
                                <asp:ImageButton ID="btnNewChoice" runat="server" ImageUrl="~/BVAdmin/images/buttons/new.png"
                                    AlternateText="New Choice" onclick="btnNewChoice_Click"></asp:ImageButton>
                                <br />
                                <br />
                                <asp:ImageButton ID="btnMoveDown" runat="server" ImageUrl="~/BVAdmin/images/buttons/down.png"
                                    AlternateText="Move Down" onclick="btnMoveDown_Click"></asp:ImageButton>
                            </asp:Panel>
                            &nbsp;
                        </td>
                    </tr>
                </table>            
                <uc2:DatePicker ID="DefaultDate" runat="server" InvalidFormatErrorMessage="Date is not in a valid format."
                    RequiredErrorMessage="Date is required." />
            </td>
        </tr>
        <tr>
            <td class="FormLabel" align="left">
                <asp:ImageButton ID="btnCancel" runat="server" AlternateText="Cancel" ImageUrl="~/BVAdmin/images/buttons/Cancel.png"
                    CausesValidation="False" onclick="btnCancel_Click"></asp:ImageButton></td>
            <td class="FormLabel" align="right">
                <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save Changes" 
                    ImageUrl="~/BVAdmin/images/buttons/SaveChanges.png" onclick="btnSave_Click">
                </asp:ImageButton></td>
        </tr>
    </table>
</asp:Content>
