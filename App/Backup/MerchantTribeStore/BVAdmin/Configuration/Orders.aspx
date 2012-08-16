<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_Orders" title="Untitled Page" Codebehind="Orders.aspx.cs" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>
        Orders</h1>         
         <asp:Label id="lblError" runat="server" CssClass="errormessage"></asp:Label>
        <uc1:MessageBox ID="MessageBox1" runat="server" />
        <asp:Panel ID="pnlMain" runat="server" DefaultButton="btnSave">        
        <table border="0" cellspacing="0" cellpadding="3" width="100%">
        <tr>
            <td class="formlabel">Maximum Order Quantity:</td>
            <td class="formfield">
                <asp:TextBox ID="OrderLimiteQuantityField" Columns="9" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="formlabel">Maximum Order Weight:</td>
            <td class="formfield">
                <asp:TextBox ID="OrderLimitWeightField" Columns="9" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="formlabel">Over Maximum Message:</td>
            <td class="formfield">
                <asp:TextBox ID="OrderLimitErrorMessage" width="200" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="formlabel">Allow 0 Dollar Orders:</td>
            <td class="formfield">
                <asp:CheckBox ID="ZeroDollarOrdersCheckBox" runat="server" />                
            </td>
        </tr>
         <tr>
            <td class="formlabel">Last Order Number:</td>
            <td class="formfield">
                <asp:TextBox ID="LastOrderNumberField" Columns="9" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="LastOrderNumberBVRequiredFieldValidator" 
                    runat="server" ControlToValidate="LastOrderNumberField" ErrorMessage="Required"></asp:RequiredFieldValidator> 
                <asp:RegularExpressionValidator ID="LastOrderNumberBVRegularExpressionValidator" runat="server" ControlToValidate="LastOrderNumberField" ValidationExpression="\d{1,10}" ErrorMessage="Last order number must be numeric."></asp:RegularExpressionValidator>
            </td>
        </tr>        
        <tr>
            <td class="formlabel">Require Terms Agreement During Checkout:</td>
            <td class="formfield">
                <asp:CheckBox ID="ForceSiteTermsCheckBox" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="formlabel">Reject Orders with Failed Credit Cards Automatically:</td>
            <td class="formfield">
                <asp:CheckBox ID="chkRejectFailedCC" runat="server" />
            </td>
        </tr>
        <tr>
             <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
                <td class="formlabel">
                    <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="../images/buttons/Cancel.png"
                        CausesValidation="False" onclick="btnCancel_Click"></asp:ImageButton></td>
                <td class="formfield"><asp:ImageButton ID="btnSave" CausesValidation="true"
                            runat="server" ImageUrl="../images/buttons/SaveChanges.png" 
                        onclick="btnSave_Click"></asp:ImageButton></td>
         </tr>         
        </table>
        </asp:Panel>
</asp:Content>

