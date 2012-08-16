<%@ Page MasterPageFile="~/BVAdmin/BVAdminNav.master" ValidateRequest="False" Language="C#"
    AutoEventWireup="True" Inherits="MerchantTribeStore.Taxes_Edit" Codebehind="Taxes_Edit.aspx.cs" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    
    <h1>Edit Tax Schedule</h1>
    <uc1:MessageBox ID="msg" runat="server" />
    <div class="controlarea1">
    Schedule Name: <asp:TextBox ID="ScheduleNameField" runat="server" Columns="30"></asp:TextBox>
    </div>
    <div class="editorcontrols">
        <asp:ImageButton ID="btnSaveChanges" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" 
            onclick="btnSaveChanges_Click" />
    </div>
    &nbsp;<br />
    &nbsp;<br />
    <h2>Rates</h2>

    <table width="100%" id="ratetable">
        <thead>
            <tr>                
                <th>Country</th>
                <th>Region</th>
                <th>Postal Code</th>
                <th>Rate</th>
                <th>Applies to Shipping?</th>
                <th>&nbsp;</th>
            </tr>
        </thead>
        <asp:Literal ID="litRates" runat="server">
        </asp:Literal>
        <tr>
            <td colspan="6">&nbsp;</td>
        </tr>
        <tr>                
                <td><asp:DropDownList ID="lstCountry" TabIndex="300" runat="server" 
                                AutoPostBack="True" onselectedindexchanged="lstCountry_SelectedIndexChanged">
                            </asp:DropDownList></td>
                <td> <asp:DropDownList ID="lstState" TabIndex="1000" runat="server">
                            </asp:DropDownList></td>
                <td><asp:TextBox ID="postalCode" TabIndex="1100" runat="server" CssClass="FormInput"
                                Columns="10"></asp:TextBox></td>
                <td><asp:TextBox ID="Rate" TabIndex="1200" runat="server" CssClass="FormInput" Columns="10"></asp:TextBox></td>
                <td><asp:CheckBox ID="ApplyToShippingCheckBox" runat="server" /></td>
                <td><asp:ImageButton ID="btnNew" runat="server" 
                        ImageUrl="~/BVAdmin/Images/Buttons/New.png" onclick="btnNew_Click" /></td>
            </tr>
   </table>

</asp:Content>
