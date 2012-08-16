<%@ Page ValidateRequest="false" Title="Edit Order" Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="true" CodeBehind="EditOrder.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.Orders.EditOrder" %>

<%@ Register Src="../Controls/AddressEditor.ascx" TagName="AddressEditor" TagPrefix="uc1" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>
<%@ Register Src="../Controls/ProductPicker.ascx" TagName="ProductPicker" TagPrefix="uc3" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc5" %>

<%@ Register src="OrderActions.ascx" tagname="OrderActions" tagprefix="uc3" %>
<%@ Register src="OrderStatusDisplay.ascx" tagname="OrderStatusDisplay" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" runat="server">
<script type="text/javascript">
    function doPrint() {
        if (window.print) {
            window.print();
        } else {
            alert('Please choose the print button from your browser.  Usually in the menu dropdowns at File: Print');
        }
    } 
</script>
<style type="text/css">
    #options {margin:10px 0;}
    #options label {display:block;width:150px;float:left;clear:both;text-align:right;font-weight:bold;margin:0 0 5px 0;}
    #options .choice {display:block;width:270px;float:left;margin:0 0 5px 20px;text-align:left;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <uc2:MessageBox ID="MessageBox1" runat="server" />
    <table border="0" cellspacing="0" cellpadding="3" width="100%">
        <tr>
            <td width="33%">
                <h1>                    
                    Edit 
                    Order
                    <asp:Label ID="OrderNumberField" runat="server" Text="000000"></asp:Label>
               </h1><asp:Label ID="TimeOfOrderField" runat="server"></asp:Label><br />                                              
            </td>
            <td align="left" valign="top">
                <uc2:orderstatusdisplay ID="OrderStatusDisplay1" runat="server" /></td>
            <td width="33%" valign="top" align="right" rowspan="3">
                <uc3:OrderActions ID="OrderActions1" runat="server" />
                &nbsp;<br />
                <asp:ImageButton ID="btnSaveChanges" runat="server" 
                    ImageUrl="~/bvadmin/images/buttons/SaveChangesGreen.png" 
                    onclick="btnSaveChanges_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="controlarea1">
                   <uc5:UserPicker UserNameFieldSize="50" ID="UserPicker1" runat="server" />                                                           
                   <asp:HiddenField ID="UserIdField" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <div class="controlarea2">
                    <em>Bill To:</em><br />
                    <uc1:AddressEditor TabOrderOffSet="2000" ID="BillingAddressEditor" runat="server" />
                </div>
            </td>
            <td valign="top"><div class="controlarea2">
                <em>Ship To:</em><br />
                <uc1:AddressEditor TabOrderOffSet="2500" ID="ShippingAddressEditor" runat="server" /></div>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:GridView Style="margin: 20px 0px 20px 0px; border-bottom: solid 1px #666;" GridLines="None"
                    ID="ItemsGridView" runat="server" AutoGenerateColumns="False" Width="100%" 
                    DataKeyNames="Id" onrowdatabound="ItemsGridView_RowDataBound1" 
                    onrowdeleting="ItemsGridView_RowDeleting">
                    <Columns>                     
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
                        <asp:TemplateField HeaderText="Price">
                            <ItemTemplate>
                                <asp:Label ID="AdjustedPriceField" runat="server" Text='<%# Bind("BasePricePerItem", "{0:c}") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quantity">
                            <ItemTemplate>                            
                                <asp:TextBox ID="QtyField" Columns="4" runat="server" Text='<%# Bind("Quantity","{0:0}") %>'></asp:TextBox>
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
                        <asp:TemplateField ShowHeader="False" Visible="false">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" CommandName="Update"
                                    ImageUrl="~/BVAdmin/Images/Buttons/Edit.png" AlternateText="Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="75" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ImageUrl="~/BVAdmin/Images/Buttons/SmallX.png" AlternateText="Delete" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle CssClass="row" />
                    <HeaderStyle CssClass="rowheader" />
                    <AlternatingRowStyle CssClass="alternaterow" />
                </asp:GridView>                
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Panel ID="pnlEditLineItem" runat="server" Visible="false" style="padding: 10px; margin:5px 0px 20px 0;" 
                CssClass="editorpanel" DefaultButton="btnCancelLineEdit"><h3>Edit Line Item</h3><asp:HiddenField ID="EditLineBvin" runat="server" />
                <asp:Label ID="lblLineEdit" runat="server"></asp:Label>                                                    
                <asp:ImageButton ID="btnCancelLineEdit" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Cancel.png" AlternateText="Cancel" CausesValidation="False" /> 
                <asp:ImageButton ID="btnSaveLineEdit" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" AlternateText="Update Line" />
                </asp:Panel>
                <asp:Panel ID="pnlAdd" runat="server" Style="padding: 10px; margin: 5px 0 20px 0;"
                    CssClass="editorpanel" DefaultButton="btnAddProductBySku">
                    <asp:Panel ID="pnlAddControls" runat="server" Visible="true">
                    Add SKU:
                    <asp:TextBox ID="NewSkuField" runat="Server" Columns="20" TabIndex="200"></asp:TextBox>
                    <asp:ImageButton ID="btnBrowseProducts" runat="server" AlternateText="Browse Products"
                        CausesValidation="False" ImageUrl="~/BVAdmin/Images/Buttons/Browse.png" 
                        onclick="btnBrowseProducts_Click" />&nbsp;
                    Quantity:
                    <asp:TextBox ID="NewProductQuantity" runat="server" Text="1" Columns="4" TabIndex="210"></asp:TextBox>
                    <asp:ImageButton CausesValidation="false" ID="btnAddProductBySku" runat="server"
                        AlternateText="Add Product To Order" ImageUrl="~/BVAdmin/Images/Buttons/AddToOrder.png"
                        TabIndex="220" onclick="btnAddProductBySku_Click" />
                    
                    <asp:HiddenField ID="AddProductSkuHiddenField" runat="server" />
                    <asp:Panel CssClass="controlarea1" ID="pnlProductPicker" runat="server" Visible="false">
                        <uc3:ProductPicker ID="ProductPicker1" runat="server" IsMultiSelect="false" DisplayPrice="true" DisplayInventory="false" DisplayKits="true" />
                        <asp:ImageButton ID="btnProductPickerCancel" CausesValidation="false" runat="server"
                            ImageUrl="~/BVAdmin/Images/Buttons/Close.png" 
                            AlternateText="Close Browser" onclick="btnProductPickerCancel_Click" />
                        <asp:ImageButton ID="btnProductPickerOkay" runat="server" AlternateText="Add Product To Order"
                            CausesValidation="false" 
                            ImageUrl="~/BVAdmin/Images/Buttons/AddToOrder.png" 
                            onclick="btnProductPickerOkay_Click" />
                    </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="pnlProductChoices" runat="server" Visible="false">                         
                        <asp:literal ID="litProductInfo" runat="server"></asp:literal><br />
                        <asp:PlaceHolder ID="phChoices" EnableViewState="false" runat="server"></asp:PlaceHolder><br />                                          
                        <asp:Literal ID="litMessage" runat="server" EnableViewState="false"></asp:Literal>
                        <asp:ImageButton ID="btnCloseVariants" CausesValidation="false" runat="server"
                            ImageUrl="~/BVAdmin/Images/Buttons/Close.png" 
                            AlternateText="Close Browser" onclick="btnCloseVariants_Click" />&nbsp;
                        <asp:ImageButton CausesValidation="false" ID="btnAddVariant" runat="server"
                        AlternateText="Add Product To Order" ImageUrl="~/BVAdmin/Images/Buttons/AddToOrder.png"
                        TabIndex="222" onclick="btnAddVariant_Click" />
                    </asp:Panel>
                </asp:Panel>                
            </td>
        </tr>      
        <tr>
            <td valign="top">
                <asp:Panel ID="pnlInstructions" runat="server">
                    <em>Customer's Instructions:</em><br />
                    <asp:TextBox ID="InstructionsField" runat="server" Columns="33" TextMode="multiLine" Wrap="true" Rows="5"></asp:TextBox></asp:Panel>
                &nbsp;
            </td>
            <td valign="top" align="right" colspan="2">
                <div class="padded" style="text-align:left;width:390px;float:right;">
                    <h4>Shipping Method</h4>
                    <asp:Literal ID="litShippingMethods" runat="server"></asp:Literal><br />
                    <h4>Force Shipping Price: <asp:TextBox ID="ShippingOverride" Width="50" runat="server"></asp:TextBox></h4>                
                </div>
                <asp:Literal id="litTotals" runat="server"></asp:Literal>
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
                <em>Codes Used:</em> &nbsp;<br />
                <asp:Panel ID="pnlCoupons" runat="server" DefaultButton="btnAddCoupon" Width="300px">                   
                <table border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td valign="top"><asp:ListBox width="200px" ID="lstCoupons" runat="server" DataTextField="CouponCode" DataValueField="CouponCode" Rows="5" SelectionMode="Multiple">                
                </asp:ListBox></td>
                    <td valign="middle"><asp:ImageButton ID="btnDeleteCoupon" runat="server" 
                            ImageUrl="~/BVAdmin/Images/Buttons/Delete.png" AlternateText="Delete Codes" 
                            onclick="btnDeleteCoupon_Click" /></td>
                </tr>
                </table>                
                Add Promotional Code:<br />
                    <asp:TextBox ID="CouponField" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="btnAddCoupon" runat="server" AlternateText="Add Code to Order"
                        CausesValidation="false" ImageUrl="~/BVAdmin/Images/Buttons/SmallRight.png" 
                        onclick="btnAddCoupon_Click" /><br />
                </asp:Panel>
            </td>
            <td valign="top">
                &nbsp;</td>
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
                            Authorized:</td>
                        <td class="formlabel">
                            <asp:Label ID="PaymentAuthorizedField" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="formfield">
                            Charged:</td>
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
                </td>
        </tr>
    </table>
    <asp:HiddenField ID="BvinField" runat="server" />
    <div style="width: 60px; margin: 10px auto 10px auto;">
        <asp:ImageButton OnClientClick="return window.confirm('Delete this order forever?');"
            ID="btnDelete" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Delete.png"
            AlternateText="Delete Order" CausesValidation="false" 
            onclick="btnDelete_Click" /></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PreBodyCloseContent" runat="server">
</asp:Content>





