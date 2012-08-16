<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_People_Affiliates_Edit" title="Untitled Page" Codebehind="Affiliates_Edit.aspx.cs" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc3" %>

<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc2" %>
<%@ Register Src="../Controls/AddressEditor.ascx" TagName="AddressEditor" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
 <h1>
        Edit Affiliate</h1>    
    <div>
        <uc3:MessageBox ID="MessageBox1" runat="server" />    
    </div>    
    <div style="float: right;width:450px;margin-bottom:20px;">        
        <h2>Address</h2>        
        <uc1:AddressEditor ID="AddressEditor1" runat="server" />
    </div>    
    <asp:Label ID="lblError" runat="server" CssClass="errormessage"></asp:Label>
    <asp:Panel ID="pnlMain" runat="server" DefaultButton="btnSaveChanges">
        <h2>General Information</h2>
        <table border="0" cellspacing="0" cellpadding="3">
            <tr>
                <td class="formlabel">
                    Enabled:</td>
                <td class="formfield">
                    <asp:CheckBox ID="chkEnabled" runat="server" /></td>
            </tr>
            <tr>
                <td class="formlabel">
                    Name:</td>
                <td class="formfield" style="width: 398px">
                    <asp:TextBox ID="DisplayNameField" runat="server" Columns="30" MaxLength="100" TabIndex="2000"
                        Width="200px"></asp:TextBox><asp:RequiredFieldValidator ID="valName" runat="server"
                            ErrorMessage="Please enter a Name" ControlToValidate="DisplayNameField">*</asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td class="formlabel">
                    Referral ID:</td>
                <td class="formfield">
                    <asp:TextBox ID="ReferralIdField" runat="server" Columns="30" TabIndex="2001"
                        Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="formlabel">
                    Commission:</td>
                <td class="formfield">
                    <asp:DropDownList ID="lstCommissionType" runat="server">
                    <asp:ListItem Value="1">Percentage of Sale</asp:ListItem>
                    <asp:ListItem Value="2">Flat Rate Commission</asp:ListItem>
                </asp:DropDownList>&nbsp;
                <asp:TextBox ID="CommissionAmountField" Columns="5" runat="server" CssClass="FormInput" />
                </td>
            </tr>
            <tr>
                <td class="formlabel">
                    Referral Days:</td>
                <td class="formfield">
                    <asp:TextBox ID="ReferralDaysField" runat="server" Columns="5" TabIndex="2001"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="formlabel">
                    Tax ID:</td>
                <td class="formfield">
                    <asp:TextBox ID="TaxIdField" runat="server" Columns="30" TabIndex="2001"
                        Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="formlabel">
                    Web Site Url:</td>
                <td class="formfield">
                    <asp:TextBox ID="WebsiteUrlField" runat="server" Columns="30" TabIndex="2001"
                        Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="formlabel">
                    Driver's License:</td>
                <td class="formfield">
                    <asp:TextBox ID="DriversLicenseField" runat="server" Columns="30" TabIndex="2001"
                        Width="200px"></asp:TextBox>
                </td>
            </tr>          
            <tr>
                <td class="formlabel">
                    Notes:</td>
                <td class="formfield">
                    <asp:TextBox ID="NotesTextBox" runat="server" Rows="4" TextMode="MultiLine" Width="300px"></asp:TextBox>
                </td>
            </tr>            
            <tr>
                <td class="formlabel">
                    Sample Url:</td>
                <td class="formfield">
                    <strong><asp:Label ID="SampleUrlLabel" runat="server"></asp:Label></strong>
                </td>
            </tr>
        </table>                        
        <div class="editorcontrols">
            <asp:ImageButton ID="btnCancel" TabIndex="2501" runat="server" ImageUrl="../images/buttons/Cancel.png"
                CausesValidation="False" style="display: inline;" 
                onclick="btnCancel_Click"></asp:ImageButton>
            <asp:ImageButton ID="btnSaveChanges" TabIndex="2500" runat="server" 
                ImageUrl="../images/buttons/SaveChanges.png" style="display: inline;" 
                onclick="btnSaveChanges_Click">
            </asp:ImageButton>
        </div>            
    </asp:Panel>
    <div class="clear"></div>
    <asp:HiddenField ID="BvinField" runat="server" />
</asp:Content>

