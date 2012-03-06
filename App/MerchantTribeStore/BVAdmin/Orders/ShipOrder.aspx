<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Orders_ShipOrder" Title="Untitled Page" Codebehind="ShipOrder.aspx.cs" %>

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
            <td width="33%" class="formfield">
                <h1>
                  <asp:label ID="lblOrderNumber" runat="server"></asp:label>Shipping</h1>
                <span class="lightlabel">Ship To:</span><br />
                <asp:Label ID="ShippingAddressField" runat="server"></asp:Label>
            </td>
            <td width="33%" align="center" valign="top">
                <uc2:OrderStatusDisplay ID="OrderStatusDisplay1" runat="server" />
            </td>
            <td class="formlabel">
                <uc3:OrderActions ID="OrderActions1" runat="server" />                
            </td>
        </tr>
        <tr>
            <td class="formfield" colspan="2">
                Shipping Total: <asp:Label ID="ShippingTotalLabel" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="formfield" colspan="3">                
                <asp:GridView Style="margin: 20px 0px 20px 0px; border-bottom: solid 1px #666;" GridLines="None"
                    ID="ItemsGridView" runat="server" AutoGenerateColumns="False" Width="100%" 
                    DataKeyNames="Id" 
                    onrowdatabound="ItemsGridView_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="SKU">
                            <ItemTemplate>
                                <asp:Label ID="SKUField" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Item">
                            <ItemTemplate>
                                <asp:Label ID="DescriptionField" runat="server"></asp:Label>
                                <asp:PlaceHolder ID="CartInputModifiersPlaceHolder" runat="server"></asp:PlaceHolder>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ordered">
                            <ItemTemplate>
                                <asp:Label ID="QuantityField" runat="server" Text='<%# Bind("Quantity","{0:#}") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Shipped">
                            <ItemTemplate>
                                <asp:Label ID="shipped" runat="server" Text="0"></asp:Label></ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:TextBox ID="QtyToShip" runat="server" Text="0" Columns="5"></asp:TextBox></ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle CssClass="row" />
                    <HeaderStyle CssClass="rowheader" />
                    <AlternatingRowStyle CssClass="alternaterow" />
                </asp:GridView>
                <asp:Panel ID="pnlShip" runat="server" DefaultButton="btnShipItems" CssClass="controlarea2 padded">
                    <table border="0" cellspacing="0" cellpadding="3" width="100%">
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="UserSelectedShippingMethod" runat="server" Text=""></asp:Label> 
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                Shipping By:<br />
                                <asp:DropDownList id="lstTrackingProvider" runat="server">
                                    <asp:ListItem Value="0">Other</asp:ListItem>
                                    <asp:ListItem Value="1">UPS</asp:ListItem>
                                    <asp:ListItem Value="2">FedEx</asp:ListItem>
                                    <asp:ListItem Value="3">US Postal</asp:ListItem>
                                </asp:DropDownList></td>                                
                                <td align="left" valign="top">                                
                                Tracking Number:<br />
                                <asp:TextBox ID="TrackingNumberField" runat="Server" Columns="20"></asp:TextBox></td>
                            <td align="right" valign="top">
                                <asp:ImageButton ID="btnShipByUPS" runat="server" 
                                    ImageUrl="~/BVAdmin/Images/Buttons/ShipByUPS.png" AlternateText="Ship By UPS" 
                                    ToolTip="Ship By UPS" onclick="btnShipByUPS_Click" Visible="False" /><br />
                                <asp:ImageButton ID="btnShipItems" runat="Server" ImageUrl="~/BVAdmin/Images/Buttons/ShipItems.png"
                                    AlternateText="Ship Items" ToolTip="Ship Items" 
                                    onclick="btnShipItems_Click" /><br />
                                <asp:ImageButton id="btnCreatePackage" runat="Server" 
                                    AlternateText="Create Package" ToolTip="Create Package" 
                                    ImageUrl="~/BVAdmin/Images/Buttons/CreatePackage.png" 
                                    onclick="btnCreatePackage_Click" Visible="False"></asp:ImageButton>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <div class="spacer" style="height:40px;"></div>
                <h2>Packages</h2>
                <asp:GridView ID="PackagesGridView" GridLines="none" BorderWidth="0" 
                    ShowHeader="false" DataKeyNames="Id" runat="server" 
                    AutoGenerateColumns="False" onrowcommand="PackagesGridView_RowCommand" 
                    onrowdatabound="PackagesGridView_RowDataBound" 
                    onrowdeleting="PackagesGridView_RowDeleting">
                    <Columns>
                        <asp:TemplateField>
                        <ItemTemplate>
                            <div class="controlarea2" style="width:590px;">
                            <div style="float:right;width:30px;height:30px;">
                            <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                    ImageUrl="~/BVAdmin/Images/Buttons/X.png"></asp:ImageButton></div>
                            <table border="0" cellspacing="0" cellpadding="3" >
                            <tr>
                                <td class="formlabel">Ship Date:</td>
                                <td class="formfield"><asp:Label ID="ShipDateField" runat="server" Text='<%# Bind("ShipDateUtc") %>'>'></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="formlabel">Shipped By:</td>
                                <td class="formfield"><asp:Label ID="ShippedByField" runat="server" Text='<%# Bind("ShippingProviderId") %>'>'></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="formlabel">Tracking:</td>
                                <td class="formfield">
                                    <asp:TextBox ID="TrackingNumberTextBox" runat="server"></asp:TextBox>
                                    <asp:ImageButton ID="TrackingNumberUpdateImageButton" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Update.png" CommandName="TrackingNumberUpdate" CommandArgument='<%# Eval("Id") %>' />
                                    <asp:HyperLink ID="TrackingLink" runat="server" Target="_blank" NavigateUrl="#" Text="No Tracking Information"></asp:HyperLink>
                                </td>
                            </tr>
                            <tr>
                                <td class="formlabel">Description:</td>
                                <td class="formfield"><asp:Label ID="pakdescription" runat="Server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="formlabel">Items:</td>
                                <td class="formfield"><asp:Label ID="items" runat="Server"></asp:Label></td>
                            </tr>
                            </table>
                            </div>
                        </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" VerticalAlign="top" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
            </tr>
    </table>            
        
</asp:Content>
