<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_Shipping_By_Weight_edit" Codebehind="edit.ascx.cs" %>
<h1>
    Edit Shipping Method - Rate Table By Total Weight</h1>
<asp:Panel ID="pnlMain" DefaultButton="btnSave" runat="server">
    <table border="0" cellspacing="0" cellpadding="5" class="formtable">
        <tr>
            <td class="formlabel">Name:</td>
            <td class="formfield">                
                <asp:TextBox ID="NameField" runat="server" Width="300"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="formlabel">
                After Getting Price, Adjust by:
            </td>
            <td class="formfield">
                <asp:TextBox ID="AdjustmentTextBox" runat="server" Columns="5"></asp:TextBox>
                &nbsp;<asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="AdjustmentTextBox"
                    ErrorMessage="Adjustment is not in the correct format." 
                    onservervalidate="CustomValidator1_ServerValidate">*</asp:CustomValidator>
                <asp:DropDownList ID="AdjustmentDropDownList" runat="server">
                    <asp:ListItem Selected="True" Value="1">Amount</asp:ListItem>
                    <asp:ListItem Value="2">Percentage</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
         <tr>
            <td class="formlabel">
                Shipping Zone:
            </td>
            <td class="formfield">                
                <asp:DropDownList ID="lstZones" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="formlabel">
                <asp:ImageButton ID="btnCancel" CausesValidation="false" runat="server" 
                    ImageUrl="~/BVAdmin/Images/Buttons/Cancel.png" onclick="btnCancel_Click" /></td>
            <td class="formfield">
                <asp:ImageButton ID="btnSave" runat="server" 
                    ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" onclick="btnSave_Click" /></td>
        </tr>
    </table>
    &nbsp;<br />
    <h2>Rate Table</h2>
    <div class="controlarea2 padded">
                <asp:GridView ID="GridView1" CellPadding="3" runat="server" AutoGenerateColumns="False"
                     onrowdeleting="GridView1_RowDeleting" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="When Total Weight is at least">
                            <ItemTemplate>
                                <asp:Label ID="lblLevel" runat="server" Text='<%# Bind("Level") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Charge This Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Rate", "{0:C}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowDeleteButton="True" />                        
                    </Columns>
                    <RowStyle CssClass="" />
                    <HeaderStyle CssClass="rowheader" />
                    <AlternatingRowStyle CssClass="" />
                </asp:GridView>
                &nbsp;<br />
                &nbsp;<br />
                <asp:Panel ID="pnlNew" runat="server" DefaultButton="btnNew">
                    Weight:
                    <asp:TextBox ID="NewLevelField" runat="server" Columns="7"></asp:TextBox>&nbsp;&nbsp;Charge:
                    <asp:TextBox ID="NewAmountField" runat="server" Columns="7"></asp:TextBox>
                    <asp:ImageButton ID="btnNew" runat="server" 
            
                        ImageUrl="~/BVAdmin/Images/Buttons/New.png" onclick="btnNew_Click" /></asp:Panel>
                        </div>
</asp:Panel>
