<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_People_users_edit" Title="Untitled Page" Codebehind="Users_Edit.aspx.cs" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>

<%@ Register Src="../Controls/AddressEditor.ascx" TagName="AddressEditor" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>
        Edit Customer</h1>
    <uc2:MessageBox ID="MessageBox1" runat="server" />
    <div style="float: right; clear: both;">        
        <table style="margin: 10px 10px 20px 10px;" cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td valign="top" width="250">
                    &nbsp;<br />
                    <br />
                    </td>
                <td valign="top" width="210">
                    <br />
                </td>
            </tr>
        </table>
        <h2>
            Address Book</h2>
        <table border="0" cellpadding="5">
            <tr>
                <td class="FormLabel">
                    <asp:ImageButton ID="btnNewAddress" runat="server" ImageUrl="../images/buttons/New.png"
                        AlternateText="Add New Address" onclick="btnNewAddress_Click"></asp:ImageButton><br />
                    &nbsp;
                    <asp:DataList ID="AddressList" runat="server" CellSpacing="5" RepeatDirection="Horizontal"
                        RepeatColumns="3" DataKeyField="bvin" 
                        ondeletecommand="AddressList_DeleteCommand" 
                        oneditcommand="AddressList_EditCommand" 
                        onitemdatabound="AddressList_ItemDataBound">
                        <ItemTemplate>
                            <asp:Label CssClass="BVSmallText" runat="server" ID="AddressDisplay" Text='<%# DataBinder.Eval(Container, "DataItem.FirstName") %>'>
                            </asp:Label><br />
                            <asp:ImageButton CommandName="Edit" ID="EditButton" ImageUrl="../images/buttons/Edit.png"
                                AlternateText="Edit" runat="server" />&nbsp;
                            <asp:ImageButton CommandName="Delete" ID="DeleteButton" ImageUrl="../images/buttons/X.png"
                                AlternateText="Delete" runat="server" />
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
        </table>
        <h2>
            Order History</h2>
        <table border="0" cellpadding="5">
            <tr>
                <td class="FormLabel">
                    <asp:Label ID="lblItems" runat="server">Orders Found</asp:Label><br>
                    <div style="overflow: scroll; height: 250px;">
                        <asp:DataGrid ID="dgOrders" runat="server" Width="500px" AutoGenerateColumns="False"
                            GridLines="None" CellPadding="3" DataKeyField="bvin" 
                            oneditcommand="dgOrders_EditCommand">
                            <AlternatingItemStyle CssClass="alternaterow"></AlternatingItemStyle>
                            <HeaderStyle CssClass="rowheader"></HeaderStyle>
                            <ItemStyle CssClass="row" />
                            <Columns>
                                <asp:BoundColumn DataField="OrderNumber" HeaderText="Order Number"></asp:BoundColumn>
                                <asp:BoundColumn DataField="TotalGrand" HeaderText="Total" DataFormatString="{0:c}">
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="TimeOfOrderUtc" HeaderText="Date"></asp:BoundColumn>
                                <asp:TemplateColumn>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="DetailsButton" runat="server" ImageUrl="../images/buttons/orderdetails.png"
                                            CommandName="Edit" CausesValidation="false"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </td>
            </tr>
        </table>
        <h2>
            Search History</h2>
        <table border="0" cellpadding="5">
            <tr>
                <td class="FormLabel">
                    <div style="overflow: scroll; height: 250px;">
                        <asp:DataGrid ID="dgSearchHistory" runat="server" Width="500px" AutoGenerateColumns="false"
                            GridLines="none" AlternatingItemStyle-CssClass="alternaterow" ItemStyle-CssClass="row"
                            HeaderStyle-CssClass="rowheader" CellPadding="3" DataKeyField="bvin">
                            <Columns>
                                <asp:BoundColumn DataField="QueryPhrase" HeaderText="Query Phrase"></asp:BoundColumn>
                                <asp:BoundColumn DataField="LastUpdated" HeaderText="Date Searched"></asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </td>
            </tr>
        </table>
        <h2>
            Saved Items</h2>
        <table border="0" cellpadding="5">
            <tr>
                <td class="FormLabel">
                    <asp:DataList ID="DataList1" runat="server" DataKeyField="bvin" BorderColor="#CCCCCC"
                        CellPadding="2" GridLines="none" Width="100" RepeatColumns="2">
                        <ItemTemplate>
                            <a href="../../BVAdmin/Catalog/Products_Edit.aspx?id=<%#Eval("bvin") %>">
                                <img src="<%#Eval("ImageFileSmall") %>" border="none" />
                                <br />
                                <%#Eval("ProductName") %>
                            </a>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
        </table>
    </div>    
    <asp:Label ID="lblError" runat="server" CssClass="errormessage" EnableViewState="False"></asp:Label>
    <asp:Panel ID="pnlMain" runat="server" DefaultButton="btnSaveChanges">
        <table border="0" cellspacing="0" cellpadding="3">
            <tr>
                <td class="formlabel">
                    Email:</td>
                <td class="formfield">
                    <asp:TextBox ID="EmailField" runat="server" Columns="30" MaxLength="100" TabIndex="2001"
                        Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="val2Username" CssClass="errormessage" EnableClientScript="True"
                        runat="server" ControlToValidate="EmailField" Display="Dynamic" ErrorMessage="Please enter an email address"
                        Visible="True">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="valUsername"
                            CssClass="errormessage" runat="server" ControlToValidate="EmailField" Display="Dynamic"
                            ErrorMessage="Please enter a valid email address" ValidationExpression="^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$">*</asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="formlabel">
                    First Name:</td>
                <td class="formfield">
                    <asp:TextBox ID="FirstNameField" TabIndex="2002" runat="server" Columns="30" MaxLength="50"
                        Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valFirstName" CssClass="errormessage" EnableClientScript="True"
                        runat="server" ControlToValidate="FirstNameField" Display="Dynamic" ErrorMessage="Please enter a first name or nickname"
                        Visible="True">*</asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td class="formlabel">
                    Last Name:</td>
                <td class="formfield">
                    <asp:TextBox ID="LastNameField" TabIndex="2003" runat="server" Columns="30" MaxLength="50"
                        Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="errormessage"
                        EnableClientScript="True" runat="server" ControlToValidate="LastNameField" Display="Dynamic"
                        ErrorMessage="Please enter a last name" Visible="True">*</asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td class="formlabel">
                    Tax Exempt:</td>
                <td class="formfield">
                    <asp:CheckBox ID="chkTaxExempt" runat="server" TabIndex="2004"></asp:CheckBox></td>
            </tr>
            <tr>
                <td class="formlabel">
                    &nbsp;</td>
                <td class="formfield">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="formlabel">
                    Password:</td>
                <td class="formfield">
                    <asp:TextBox ID="PasswordField" runat="server" Columns="30" Width="200px" TabIndex="2006"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valPassword" runat="server" ControlToValidate="PasswordField"
                        ErrorMessage="A password is required" Display="Dynamic">*</asp:RequiredFieldValidator>&nbsp;
                    <asp:RegularExpressionValidator ID="PasswordRegularExpressionValidator" runat="server"
                        ControlToValidate="PasswordField" Display="Dynamic" ErrorMessage="Password must be at least 6 characters long.">*</asp:RegularExpressionValidator></td>
            </tr>          
            <tr>
                <td class="formlabel">
                    Account Locked:</td>
                <td class="formfield">
                    <asp:CheckBox ID="LockedField" runat="server" /></td>
            </tr>
            <tr>
                <td class="formlabel">
                    Notes:</td>
                <td class="formfield">
                    <asp:TextBox ID="CommentField" runat="server" Columns="30" Rows="3" TextMode="MultiLine"
                        Width="200px" TabIndex="2009"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="formlabel">
                    Pricing Group</td>
                <td class="formfield">
                    <asp:DropDownList ID="PricingGroupDropDownList" runat="server">
                    </asp:DropDownList></td>
            </tr>        
            <tr>
                <td class="formlabel">
                    <asp:ImageButton ID="btnCancel" TabIndex="2501" runat="server" ImageUrl="../images/buttons/Cancel.png"
                        CausesValidation="False" onclick="btnCancel_Click"></asp:ImageButton></td>
                <td class="formfield">
                    <asp:ImageButton ID="btnSaveChanges" TabIndex="2500" runat="server" 
                        ImageUrl="../images/buttons/SaveChanges.png" onclick="btnSaveChanges_Click">
                    </asp:ImageButton></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:HiddenField ID="BvinField" runat="server" />
    <div class="clear"></div>
</asp:Content>
