<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_ShippingHandling" Codebehind="ShippingHandling.aspx.cs" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
        <h1>Shipping | Handling</h1>
        <uc1:MessageBox ID="MessageBox1" runat="server" EnableViewState="false" />
        <h2 style="margin-bottom: 5px;">Handling
        </h2>
        <table>
            <tr>
                <td class="formlabel">Handling Fee Amount</td>
                <td class="formfield"><asp:TextBox ID="HandlingFeeAmountTextBox" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Handling Fee Amount Is Required." ControlToValidate="HandlingFeeAmountTextBox" Display="Dynamic">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="HandlingFeeAmountCustomValidator" runat="server" 
                        ErrorMessage="Handling Fee Must Be A Monetary Amount." 
                        ControlToValidate="HandlingFeeAmountTextBox" Display="Dynamic" 
                        onservervalidate="HandlingFeeAmountCustomValidator_ServerValidate">*</asp:CustomValidator></td>
            </tr>
            <tr>
                <td class="formlabel"></td>
                <td class="formfield">
                    <asp:RadioButtonList ID="HandlingRadioButtonList" runat="server">
                        <asp:ListItem Value="0">Per Item</asp:ListItem>
                        <asp:ListItem Value="1">Per Order</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td class="formlabel">Charge Handling On Non-Shipping Items</td>
                <td class="formfield">
                    <asp:CheckBox ID="NonShippingCheckBox" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ImageButton ID="CancelImageButton" runat="server" 
                        ImageUrl="~/BVAdmin/Images/Buttons/Cancel.png" CausesValidation="False" 
                        onclick="CancelImageButton_Click" />
                </td>
                <td>
                    <asp:ImageButton ID="SaveImageButton" runat="server" 
                        ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" 
                        onclick="SaveImageButton_Click" />
                </td>
            </tr>
        </table>
        
</asp:Content>



