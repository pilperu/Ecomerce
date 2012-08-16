<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Marketing_OffersEdit" title="Untitled Page" Codebehind="OffersEdit.aspx.cs" %>
<%@ PreviousPageType VirtualPath="~/BVAdmin/Marketing/Default.aspx" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register Assembly="MerchantTribe.Commerce" Namespace="MerchantTribe.Commerce" TagPrefix="cc1" %>
<%@ Register Src="../Controls/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<h1>Offers Edit</h1>
    <uc1:MessageBox ID="MessageBox1" runat="server" />    
<table width="100%">
                <tr>
                    <td colspan="3">
                        <h2>
                            General Information</h2>
                    </td>
                </tr>
                <tr>
                    <td class="formlabel">
                        Name</td>
                    <td class="formfield" colspan="2">
                        <asp:TextBox ID="OfferNameTextBox" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="OfferNameTextBox"
                            ErrorMessage="Name must be between 1 and 50 characters" ValidationExpression=".{1,50}">*</asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="OfferNameTextBox"
                            ErrorMessage="Offer Name Is Required.">*</asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td class="formlabel">
                        Start Date</td>
                    <td class="formfield" colspan="2">
                        <uc2:DatePicker ID="StartDatePicker" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="formlabel">
                        End Date</td>
                    <td class="formfield" colspan="2">
                        <uc2:DatePicker ID="EndDatePicker" runat="server" />
                        <asp:CustomValidator ID="CustomValidator1" runat="server" 
                            ErrorMessage="End date must be greater than start date." 
                            onservervalidate="CustomValidator1_ServerValidate">*</asp:CustomValidator></td>
                </tr>
                <tr>
                    <td colspan="3"><h2>Settings</h2></td>
                </tr>                 
                <asp:PlaceHolder ID="EditPlaceHolder" runat="server"></asp:PlaceHolder>                
                <tr>
                    <td colspan="2">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <h2>
                            Offer Use</h2>
                    </td>
                </tr>
                <tr>
                    <td>
                        Requires promotional code</td>
                    <td>
                        <asp:CheckBox ID="RequiresCouponCodeCheckBox" runat="server" /></td>
                </tr>
                <%-- 
                <tr>
                    <td>
                        Generate unique promotional codes</td>
                    <td>
                        <asp:CheckBox ID="UniquePromotionalCodesCheckBox" runat="server" /></td>
                </tr>
                --%>
                <tr>
                    <td>
                        Required promotion code</td>
                    <td>
                        <asp:TextBox ID="PromotionCodeTextBox" runat="server" Width="213px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        Use type</td>
                    <td>
                        <div><asp:RadioButton ID="UnlimitedRadioButton" runat="server" Checked="True" GroupName="UseType" Text="Unlimited" /></div>
                        <div><asp:RadioButton ID="PerCustomerRadioButton" runat="server" GroupName="UseType" Text="Per customer" />
                        <asp:TextBox ID="UsePerPersonTextBox" runat="server" Width="77px"></asp:TextBox>
                            <asp:Label ID="Label1" runat="server" Text="time(s)"></asp:Label>
                            <asp:CustomValidator ID="PerCustomerCustomValidator" runat="server" ControlToValidate="UsePerPersonTextBox"
                                
                                ErrorMessage="Number of times per customer must be set and has to be greater than 0." 
                                onservervalidate="PerCustomerCustomValidator_ServerValidate">*</asp:CustomValidator></div>
                        <div><asp:RadioButton ID="PerStoreRadioButton" runat="server" GroupName="UseType" Text="Per store" />
                            <asp:TextBox ID="UsePerStoreTextBox" runat="server" Width="73px"></asp:TextBox>
                            <asp:Label ID="Label2" runat="server" Text="time(s)"></asp:Label>
                            <asp:CustomValidator ID="PerStoreCustomValidator" runat="server" ControlToValidate="UsePerStoreTextBox"
                                
                                ErrorMessage="Number of times per store must be set and has to be greater than 0." 
                                onservervalidate="PerStoreCustomValidator_ServerValidate">*</asp:CustomValidator></div>
                    </td>
                </tr>                
                <tr>
                    <td colspan="2">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <h2>
                            Restrictions</h2>
                    </td>
                </tr>
                <tr>
                    <td>
                        Promotion may NOT be combined with
                        <br />
                        other promotional codes</td>
                    <td>
                        &nbsp;<asp:CheckBox ID="PromotionCodeCantBeCombinedCheckBox" runat="server" /></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:ImageButton ID="CancelImageButton" runat="server" 
                            ImageUrl="~/BVAdmin/Images/Buttons/Cancel.png" CausesValidation="False" 
                            onclick="CancelImageButton_Click" /><asp:ImageButton ID="SaveImageButton"
                            runat="server" ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" 
                            onclick="SaveImageButton_Click" /></td>
                </tr>
            </table>            
    <uc1:MessageBox ID="MessageBox2" runat="server" />    
</asp:Content>

