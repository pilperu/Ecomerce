<%@ Page Title="Create New Order" Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="true" CodeBehind="CreateOrder.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.Orders.CreateOrder" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc5" %>
<%@ Register Src="../../BVModules/Controls/CreditCardInput.ascx" TagName="CreditCardInput" TagPrefix="uc4" %>
<%@ Register Src="../Controls/ProductPicker.ascx" TagName="ProductPicker" TagPrefix="uc3" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>
<%@ Register Src="../Controls/AddressEditor.ascx" TagName="AddressEditor" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <uc2:MessageBox ID="MessageBox1" runat="server" />
    <table border="0" cellspacing="0" cellpadding="3" width="100%">
        <tr>
            <td width="33%">
                <h1>New Order</h1>
            </td>
            <td align="left" valign="top">
                <asp:Label ID="StatusField" runat="server"></asp:Label>
            </td>
            <td width="33%" valign="top" align="right">                
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:GridView Style="margin: 20px 0px 20px 0px; border-bottom: solid 1px #666;" GridLines="None"
                    ID="ItemsGridView" DataKeyNames="Id" runat="server" AutoGenerateColumns="False"
                    Width="100%" onrowdatabound="ItemGridView_RowDataBound" 
                    onrowdeleting="ItemGridView_RowDeleting" onrowediting="GridView1_RowEditing">
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
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ImageUrl="~/BVAdmin/Images/Buttons/x.png" AlternateText="Delete" />
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
            <td>
                &nbsp;</td>
            <td align="center" valign="top">
                <asp:ImageButton ID="btnUpdateQuantities" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Update.png"
                    AlternateText="Update" CausesValidation="False" TabIndex="100" 
                    onclick="btnUpdateQuantities_Click" /></td>
            <td align="right" valign="top">
                Sub Total:
                <asp:Label ID="SubTotalField2" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td colspan="3">
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
            <td colspan="3" style="height: 41px">
                <hr />
            </td>
        </tr>
        <tr>
            <td valign="top">
                <span class="lightlabel">Ship To:</span><br />
                <uc1:AddressEditor ID="ShipToAddress" runat="server" TabOrderOffSet="1000" />
                <asp:CheckBox ID="chkBillToSame" Checked="true" runat="server" AutoPostBack="true"
                    Text="Bill to Same Address" 
                    oncheckedchanged="chkBillToSame_CheckedChanged" /><br />&nbsp;<br />                  
                <span class="lightlabel"><asp:Label ID="EmailAddressLabel" runat="server" Text="E-mail:"></asp:Label></span>
                <asp:TextBox ID="EmailAddressTextBox" runat="server" TabIndex="3000" Columns="30"></asp:TextBox><asp:RegularExpressionValidator ID="EmailRegularExpressionValidator" runat="server"
                        ControlToValidate="EmailAddressTextBox" ErrorMessage="Invalid E-mail Address."
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator><br />&nbsp;
            </td>
            <td valign="top">
                <asp:Panel ID="pnlBillTo" runat="server" Visible="false">
                    <span class="lightlabel">Bill To:</span><br />
                    <uc1:AddressEditor ID="BillToAddress" runat="server" TabOrderOffSet="2000" />
                </asp:Panel>
                &nbsp;
            </td>
            <td align="left" valign="top">
                <table border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td><asp:Button ID="btnFindUsers" CausesValidation="false" Text="Find Users" 
                            runat="server" onclick="btnFindUsers_Click" /></td>
                    <td><asp:Button ID="btnNewUsers" CausesValidation="false" Text="New User" 
                            runat="server" onclick="btnNewUsers_Click" /></td>
                    <td><asp:Button ID="btnFindOrders" CausesValidation="false" Text="Find Orders" 
                            runat="server" onclick="btnFindOrders_Click" /></td>
                </tr>
                </table>
                <div class="controlarea1" style="padding:10px;">
                <asp:MultiView ID="viewFindUsers" runat="server" ActiveViewIndex="0">
                <asp:View ID="ViewFind" runat="server">
                     <strong>Search for User</strong><br />
                    <asp:Label ID="lblFindUserMessage" runat="server"></asp:Label>
                    <asp:Panel ID="pnlFindUser" runat="server" DefaultButton="btnFindUser">
        <table border="0" cellspacing="0" cellpadding="3">           
            <tr>
                <td class="formlabel">
                    Keyword:</td>
                <td class="formfield">
                    <asp:TextBox ID="FilterUserField" runat="server" Columns="15"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="formlabel">
                    &nbsp;</td>
                <td class="formfield">
                    <asp:Button ID="btnFindUser" runat="server" Text="Find User(s)" 
                        CausesValidation="false" onclick="btnFindUser_Click" />
                    </td>
            </tr>
        </table>
    </asp:Panel>  
            <asp:GridView ShowHeader="false" ID="GridView1" runat="server" AutoGenerateColumns="False"
                DataKeyNames="bvin" BorderColor="#CCCCCC" CellPadding="3" GridLines="None" 
                         Width="260" onrowediting="GridView1_RowEditing">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="lblUsername" runat="server" Text='<%# Bind("Email") %>'>'></asp:Label><br />
                            <span class="smalltext">
                                <asp:Label ID="lblFirstName" runat="server" Text='<%# Bind("FirstName") %>'>'></asp:Label>
                                <asp:Label ID="lblLastName" runat="server" Text='<%# Bind("LastName") %>'>'></asp:Label></span></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="SelectUserButton" runat="server" CausesValidation="false" CommandName="Edit"
                                ImageUrl="~/BVAdmin/Images/Buttons/Select.png" AlternateText="Select User"></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <RowStyle CssClass="row" />
                <HeaderStyle CssClass="rowheader" />
                <AlternatingRowStyle CssClass="alternaterow" />
            </asp:GridView>
        
                </asp:View>
                <asp:View ID="ViewNew" runat="server">
                    <strong>Add New User</strong><br />
                    <asp:Label ID="lblNewUserMessage" runat="server"></asp:Label>
                    <asp:Panel ID="pnlNewUser" runat="server" DefaultButton="btnNewUserSave">
        <table border="0" cellspacing="0" cellpadding="3">           
            <tr>
                <td class="formlabel">
                    Email:</td>
                <td class="formfield">
                    <asp:TextBox ID="NewUserEmailField" runat="server" Columns="15"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="formlabel">
                    First Name:</td>
                <td class="formfield">
                    <asp:TextBox ID="NewUserFirstNameField" runat="server" Columns="15"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="formlabel">
                    Last Name:</td>
                <td class="formfield">
                    <asp:TextBox ID="NewUserLastNameField" runat="server" Columns="15"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="formlabel">
                    &nbsp;</td>
                <td class="formfield">
                    <asp:Button ID="btnNewUserSave" runat="server" Text="Add New User" 
                        CausesValidation="false" onclick="btnNewUserSave_Click" />
                    </td>
            </tr>
        </table>
    </asp:Panel>
                </asp:View>
                <asp:View ID="ViewOrder" runat="server">
                    <strong>Find User By Order</strong>
                    <asp:Label ID="lblFindOrderMessage" runat="server"></asp:Label>
                    <asp:Panel ID="pnlFindUserByOrder" runat="server" DefaultButton="btnGoFindOrder">
                    <table border="0" cellspacing="0" cellpadding="3">
                    <tr>
                        <td align="right">Order Number:</td>
                        <td><asp:TextBox ID="FindOrderNumberField" runat="server" Columns="20"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td><asp:Button ID="btnGoFindOrder" CausesValidation="false" runat="server" 
                                Text="Find This Order" onclick="btnGoFindOrder_Click" /></td>
                    </tr>
                    </table>
                    </asp:Panel>
                </asp:View>
                </asp:MultiView>                                                    
                <asp:HiddenField ID="UserIdField" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <hr />
            </td>
        </tr>
        <tr>
            <td valign="top">
                <h3>
                    Shipping</h3>
                <asp:RadioButtonList ID="ShippingRatesList" runat="server" TabIndex="4000" 
                    onselectedindexchanged="ShippingRatesList_SelectedIndexChanged">
                </asp:RadioButtonList>
                <asp:ImageButton ID="btnCalculateShipping" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/GetShippingRates.png"
                    CausesValidation="False" TabIndex="4010" 
                    onclick="btnCalculateShipping_Click" />
                <asp:ImageButton ID="btnUpdateShipping" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Update.png"
                    AlternateText="Update Shipping Totals" CausesValidation="False" 
                    onclick="btnUpdateShipping_Click" />                  
            </td>
            <td valign="top">
                <h3>
                    Payment</h3>
                <table border="0" cellspacing="0" cellpadding="3">
                    <tr runat="server" id="rowNoPaymentNeeded" visible="false">
                        <td valign="top" class="radiobuttoncol"><asp:RadioButton ID="rbNoPayment" GroupName="PaymentGroup" runat="server" Checked="false" /></td>
                        <td><asp:Label ID="lblNoPaymentNeeded" runat="server"></asp:Label></td>
                    </tr>
                    <tr runat="server" id="rowCreditCard" visible="false">
                        <td valign="top">
                            <asp:RadioButton ID="rbCreditCard" GroupName="PaymentGroup" runat="server" Checked="false"
                                TabIndex="5000" /></td>
                        <td>
                            Credit Card<uc4:CreditCardInput ID="CreditCardInput1" runat="server" TabIndex="5001" />
                        </td>
                    </tr>
                    <tr runat="server" id="trPurchaseOrder" visible="false">
                        <td valign="top">
                            <asp:RadioButton ID="rbPurchaseOrder" GroupName="PaymentGroup" runat="server" Checked="false"
                                TabIndex="5010" /></td>
                        <td>
                            <asp:Label ID="lblPurchaseOrderDescription" runat="server"></asp:Label><br />
                            Purchase Order #
                            <asp:TextBox ID="PurchaseOrderField" runat="server" Columns="10" TabIndex="5011"></asp:TextBox></td>
                    </tr>
                    <tr runat="server" id="trCompanyAccount" visible="false">
                        <td valign="top" class="radiobuttoncol">
                        <asp:RadioButton ID="rbCompanyAccount" GroupName="PaymentGroup" runat="server" Checked="false" /></td>
                        <td><asp:Label ID="lblCompanyAccountDescription" runat="server"></asp:Label> #: <asp:textbox ID="accountnumber" ClientIDMode="Static" runat="server" Columns="10"></asp:textbox></td>
                    </tr>
                    <tr runat="server" id="rowCheck" visible="false">
                        <td valign="top">
                            <asp:RadioButton ID="rbCheck" GroupName="PaymentGroup" runat="server" Checked="false"
                                TabIndex="5020" /></td>
                        <td>
                            <asp:Label ID="lblCheckDescription" runat="server"></asp:Label></td>
                    </tr>
                    <tr runat="server" id="rowTelephone" visible="false">
                        <td valign="top">
                            <asp:RadioButton ID="rbTelephone" GroupName="PaymentGroup" runat="server" Checked="false"
                                TabIndex="5030" /></td>
                        <td>
                            <asp:Label ID="lblTelephoneDescription" runat="server"></asp:Label></td>
                    </tr>
                    <tr runat="server" id="trCOD" visible="false">
                        <td valign="top">
                            <asp:RadioButton ID="rbCOD" GroupName="PaymentGroup" runat="server" Checked="false"
                                TabIndex="5040" /></td>
                        <td>
                            <asp:Label ID="lblCOD" runat="server"></asp:Label></td>
                    </tr>
                </table>
                
            </td>
            <td valign="top" align="right">
                <table cellspacing="0" cellpadding="2" width="200" border="0">
                    <tr>
                        <td class="FormLabel" valign="top" align="left">
                            SubTotal:</td>
                        <td class="FormLabel" valign="top" align="right">
                            <asp:Label ID="SubTotalField" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="FormLabel" valign="top" align="left">
                            Tax:</td>
                        <td class="FormLabel" valign="top" align="right">
                            <asp:Label ID="TaxTotalField" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="FormLabel" valign="top" align="left">
                            Shipping:</td>
                        <td class="FormLabel" valign="top" align="right">
                            <asp:Label ID="ShippingTotalField" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="FormLabel" valign="top" align="left">
                            Handling:</td>
                        <td class="FormLabel" valign="top" align="right">
                            <asp:Label ID="HandlingTotalField" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="FormLabel" valign="top" align="left">
                            &nbsp;
                        </td>
                        <td class="FormLabel" valign="top" align="right" style="border-top: solid 1px #666;">
                            <strong>
                                <asp:Label ID="GrandTotalField" runat="server"></asp:Label></strong></td>
                    </tr>
                </table>
                <br />
                &nbsp;<br />
                Customer Instructions &nbsp;&nbsp;
                <br />
                <asp:TextBox ID="InstructionsField" runat="server" Columns="20" Height="100px" Rows="3"
                    TextMode="MultiLine" Width="300px" TabIndex="9000"></asp:TextBox>
                <br />
                <asp:Panel ID="pnlCoupons" runat="server" DefaultButton="btnAddCoupon" Width="300px">
                    Add Promotional Code:
                    <asp:TextBox ID="CouponField" runat="server" TabIndex="9100"></asp:TextBox>
                    <asp:ImageButton ID="btnAddCoupon" CausesValidation="false" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Go.png"
                        AlternateText="Add Coupon to Cart" onclick="btnAddCoupon_Click" /><br />
                    <asp:GridView CellPadding="3" CellSpacing="0" GridLines="none" ID="CouponGrid" runat="server"
                        AutoGenerateColumns="False" DataKeyNames="CouponCode" ShowHeader="False" 
                        onrowdeleting="CouponGrid_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="CouponCode" ShowHeader="False" />
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDeleteCoupon" ImageUrl="~/BVAdmin/Images/Buttons/x.png" runat="server"
                                        CausesValidation="false" CommandName="Delete" AlternateText="Delete Coupon" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                &nbsp;<br />
                <asp:ImageButton ID="btnSubmit" runat="server" AlternateText="Place Order" ImageUrl="~/BVAdmin/Images/Buttons/PlaceOrder.png"
                    TabIndex="9999" onclick="btnSubmit_Click" />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="BvinField" runat="server" Value="" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PreBodyCloseContent" runat="server">
</asp:Content>
