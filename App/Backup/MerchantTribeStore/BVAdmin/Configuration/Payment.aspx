<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_Payment" title="Untitled Page" Codebehind="Payment.aspx.cs" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Payment</h1>
    <uc1:MessageBox ID="MessageBox1" runat="server" />
    <table>        
        <tr>
            <td colspan="2">
                <asp:GridView ID="PaymentMethodsGrid" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="MethodId" CellPadding="3" BorderWidth="0px" GridLines="None" 
                    onrowdatabound="PaymentMethodsGrid_RowDataBound" 
                    onrowediting="PaymentMethodsGrid_RowEditing">
                    <Columns>
                        <asp:TemplateField HeaderText="Display at Checkout">
                            <ItemTemplate>
                                <asp:CheckBox id="chkEnabled" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("MethodName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" ImageUrl="~/BVAdmin/Images/Buttons/Edit.png" runat="server" CausesValidation="false" CommandName="Edit"
                                    AlternateText="Edit"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <fieldset>                    
                    <legend>Sign Up For:</legend>
                    <a href="http://altfarm.mediaplex.com/ad/ck/3484-23890-3840-92" target="_blank" style="display: block;">
                        <img src="http://images.paypal.com/en_US/i/bnr/paypal_mrb_banner.gif" alt="Sign up for Paypal" style="border: 0px none; margin-top: 10px;">
                    </a>                                                                        
                </fieldset>
             </td>
        </tr>
    </table>
    
    <div style="height:15px;"></div>
    <asp:ImageButton ID="btnSaveChanges" runat="server" 
        ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" 
        onclick="btnSaveChanges_Click" />
</asp:Content>

