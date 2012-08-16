<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Orders_ViewOrder" Title="Untitled Page" Codebehind="ViewOrder.aspx.cs" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>

<%@ Register src="OrderStatusDisplay.ascx" tagname="OrderStatusDisplay" tagprefix="uc2" %>
<%@ Register src="OrderActions.ascx" tagname="OrderActions" tagprefix="uc3" %>

<asp:Content ContentPlaceHolderID="headcontent" runat="server" ID="headcontentnow">
<script type="text/javascript">
    function doPrint() {
        if (window.print) {
            window.print();
        } else {
            alert('Please choose the print button from your browser.  Usually in the menu dropdowns at File: Print');
        }
    } 
</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">    
    <uc1:MessageBox ID="MessageBox1" runat="server" />
    <table border="0" cellspacing="0" cellpadding="3" width="100%">
        <tr>
            <td>
                <h1>
                    Order
                    <asp:Label ID="OrderNumberField" runat="server" Text="000000"></asp:Label>
               </h1><asp:Label ID="TimeOfOrderField" runat="server"></asp:Label><br />               
               Fraud Score: <asp:Label ID="lblFraudScore" runat="server"></asp:Label>
            </td>
            <td width="33%" align="center" valign="top">
            <uc2:OrderStatusDisplay ID="OrderStatusDisplay1" runat="server" />
            </td>
            <td width="33%" valign="top" align="right" rowspan="2">
                <uc3:OrderActions ID="OrderActions1" runat="server" />
&nbsp;</td>
        </tr>
        <tr>
            <td valign="top">
                <span class="lightlabel">Bill To:</span><br />
                <asp:Label ID="BillingAddressField" runat="server"></asp:Label>
                <asp:Literal ID="EmailAddressField" runat="server"></asp:Literal>
            </td>
            <td valign="top">
                <asp:Panel ID="pnlShipTo" runat="server" Visible="true">
                    <span class="lightlabel">Ship To:</span><br />
                    <asp:Label ID="ShippingAddressField" runat="server"></asp:Label></asp:Panel>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:GridView Style="margin: 20px 0px 20px 0px; border-bottom: solid 1px #666;" GridLines="None"
                    ID="ItemsGridView" runat="server" AutoGenerateColumns="False" Width="100%" 
                    DataKeyNames="Id" 
                    onrowdatabound="ItemsGridView_RowDataBound">
                    <Columns>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:CheckBox ID="SelectedCheckBox" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SKU">
                            <ItemTemplate>
                                <asp:Label ID="SKUField" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Item">
                            <ItemTemplate>
                                <asp:Label ID="DescriptionField" runat="server"></asp:Label><br />
                                <asp:PlaceHolder ID="CartInputModifiersPlaceHolder" runat="server"></asp:PlaceHolder>
                                <asp:PlaceHolder ID="KitDisplayPlaceHolder" runat="server"></asp:PlaceHolder>
                                <asp:Literal ID="lblGiftWrap" runat="server" Text=""></asp:Literal><br /><br />
                                <asp:Literal ID="lblGiftWrapDetails" runat="server"></asp:Literal>
                                <asp:Literal ID="litDiscounts" runat="server"  ></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Shipping">
                            <ItemTemplate>
                                <asp:Label ID="ShippingStatusField" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Price">
                            <ItemTemplate>
                                <asp:Label ID="AdjustedPriceField" runat="server" Text='<%# Bind("BasePricePerItem", "{0:c}") %>'></asp:Label>
                                <asp:Literal ID="lblGiftWrapPrice" runat="server"></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quantity">
                            <ItemTemplate>
                                <asp:Label ID="QuantityField" runat="server" Text='<%# Bind("Quantity","{0:#}") %>'></asp:Label>
                                <asp:Literal ID="lblGiftWrapQty" runat="server"></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Line Total">
                            <ItemTemplate>
                                <asp:Label ID="LineTotalWithoutDiscounts" runat="server" Visible="false" ></asp:Label>
                                <asp:Label ID="LineTotalField" runat="server" Text='<%# Bind("LineTotal", "{0:c}") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle CssClass="row" />
                    <HeaderStyle CssClass="rowheader" />
                    <AlternatingRowStyle CssClass="alternaterow" />
                </asp:GridView>
                <asp:ImageButton ID="btnRMA" runat="server" 
                    ImageUrl="~/BVAdmin/Images/Buttons/ReturnSelectedItems.png" 
                    AlternateText="Create RMA" onclick="btnRMA_Click" Visible="False" /></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Panel ID="pnlInstructions" runat="server" Visible="false">
                    <em>Customer's Instructions:</em><br />
                    <asp:Label ID="InstructionsField" runat="server"></asp:Label></asp:Panel>
                &nbsp;
            </td>
            <td valign="top" align="right" colspan="2">
                <asp:Literal id="litTotals" runat="server"></asp:Literal>
                &nbsp;<br />
                <asp:DropDownList ID="lstEmailTemplate" runat="server">
                </asp:DropDownList>&nbsp;
                <asp:ImageButton ImageUrl="~/BVAdmin/Images/Buttons/Email.png" 
                    ID="btnSendStatusEmail" runat="server" ToolTip="Send Update by Email" 
                    AlternateText="Send Update By Email" onclick="btnSendStatusEmail_Click" 
                     />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <div style="height: 20px;">
                </div>
            </td>
        </tr>
        <tr>
            <td valign="top" class="controlarea2">
                <em>Private Notes</em>
                <asp:GridView GridLines="None" Style="margin-top: 5px;" ID="PrivateNotesField" runat="server"
                    ShowHeader="false" AutoGenerateColumns="False" Width="100%" 
                    DataKeyNames="Id" onrowdeleting="PrivateNotesField_RowDeleting">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lblAuditDate" runat="server" Text='<%# Bind("AuditDate","{0:d}") %>'></asp:Label><br />
                                <asp:Label ID="NoteField" runat="server" Text='<%# Bind("Note") %>'></asp:Label>
                                <div style="width: 75px; height: 30px; float: right;">
                                    <asp:ImageButton ID="btnDeleteNote" runat="server" ImageUrl="~/bvadmin/images/buttons/delete.png"
                                        CommandName="Delete" CommandArgument='<%# Bind("Id") %>' AlternateText="DeleteNote"
                                        OnClientClick="return window.confirm('Delete this note?');" /></div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="row" />
                    <AlternatingRowStyle CssClass="alternaterow" />
                </asp:GridView>
                <br />
                <asp:TextBox ID="NewPrivateNoteField" runat="server" Columns="33" ToolTip="Add a new note to this order"
                    Rows="3" TextMode="MultiLine"></asp:TextBox><br />
                <asp:ImageButton ID="btnNewPrivateNote" runat="server" 
                    ImageUrl="~/bvadmin/images/buttons/new.png" onclick="btnNewPrivateNote_Click">
                </asp:ImageButton>
            </td>
            <td valign="top" class="controlarea2">
                <em>Public Notes:</em>
                <asp:GridView GridLines="None" Style="margin-top: 8px;" Width="100%" ID="PublicNotesField"
                    runat="server" ShowHeader="False" AutoGenerateColumns="False" 
                    DataKeyNames="Id" onrowdeleting="PublicNotesField_RowDeleting">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lblAuditDate" runat="server" Text='<%# Bind("AuditDate","{0:d}") %>'></asp:Label><br />
                                <asp:Label ID="NoteField" runat="server" Text='<%# Bind("Note") %>'></asp:Label>
                                <div style="width: 75px; height: 30px; float: right;">
                                    <asp:ImageButton ID="btnDeleteNote" runat="server" ImageUrl="~/bvadmin/images/buttons/delete.png"
                                        CommandName="Delete" CommandArgument='<%# Bind("Id") %>' AlternateText="DeleteNote"
                                        OnClientClick="return window.confirm('Delete this note?');" /></div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="row" />
                    <AlternatingRowStyle CssClass="alternaterow" />
                </asp:GridView>
                <br />
                <asp:TextBox ID="NewPublicNoteField" runat="server" Columns="33" ToolTip="Add a new note to this order"
                    Rows="3" TextMode="MultiLine"></asp:TextBox><br />
                <asp:ImageButton ID="btnNewPublicNote" runat="server" 
                    ImageUrl="~/bvadmin/images/buttons/new.png" onclick="btnNewPublicNote_Click">
                </asp:ImageButton>
            </td>
            <td class="controlarea2" align="left" valign="top">
                <em>Payment Information:</em>
                <table class="controlarea1" style="margin-top: 8px; color: #666; font-size: 11px;"
                    border="0" cellspacing="0" cellpadding="3" width="100%">
                    <tr>
                        <td colspan="2" class="formfield" style="border-bottom: solid 1px #999;">
                            <asp:Label runat="server" ID="lblPaymentSummary"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="formfield">
                            Held/Reserved:</td>
                        <td class="formlabel">
                            <asp:Label ID="PaymentAuthorizedField" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="formfield">
                            Received:</td>
                        <td class="formlabel">
                            <asp:Label ID="PaymentChargedField" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="formfield" style="border-bottom: solid 1px #999; padding-bottom: 3px;">
                            Refunded:</td>
                        <td class="formlabel" style="border-bottom: solid 1px #999; padding-bottom: 3px;">
                            <asp:Label ID="PaymentRefundedField" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="formfield">
                            Amount Due:</td>
                        <td class="formlabel" style="color: #333;">
                            <strong>
                                <asp:Label ID="PaymentDueField" runat="server"></asp:Label></strong></td>
                    </tr>
                </table>
                &nbsp;<br />
                <em>Codes Used:</em><br />
                <asp:Label ID="CouponField" runat="server" CssClass="BVSmallText"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="BvinField" runat="server" />
    <div style="width: 60px; margin: 10px auto 10px auto;">
        <asp:ImageButton OnClientClick="return window.confirm('Delete this order forever?');"
            ID="btnDelete" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Delete.png"
            AlternateText="Delete Order" CausesValidation="false" onclick="btnDelete_Click" 
             /></div>
</asp:Content>
