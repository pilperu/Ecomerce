<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Promotions_Edit_Qualification.ascx.cs" Inherits="MerchantTribeStore.BVAdmin.Marketing.Promotions_Edit_Qualification" %>
<%@ Register src="../Controls/MessageBox.ascx" tagname="MessageBox" tagprefix="uc1" %>
<%@ Register src="../Controls/ProductPicker.ascx" tagname="ProductPicker" tagprefix="uc2" %>
<%@ Register src="../Controls/UserPicker.ascx" tagname="UserPicker" tagprefix="uc3" %>
<div style="overflow:auto;height:500px;">
<uc1:MessageBox ID="MessageBox1" runat="server" />
<asp:MultiView ID="MultiView1" runat="server">
<asp:View ID="viewProductBvin" runat="server">
    <h1>When Product Is...</h1>
    <table border="0" width="100%">
    <tr>
        <td align="left">        
        <asp:GridView ID="gvProductBvins" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="Bvin" GridLines="None" onrowdeleting="gvProductBvins_RowDeleting" 
        ShowHeader="False">
        <Columns>
            <asp:BoundField DataField="DisplayName" />
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:ImageButton ID="btnDeleteProductBvin" runat="server" CausesValidation="False" 
                        CommandName="Delete" Text="Delete" ImageUrl="~/bvadmin/images/buttons/SmallX.png" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>        
        </td>        
        <td align="left" width="45%"><asp:ImageButton ID="btnAddProductBvins" runat="server" 
                ImageUrl="~/bvadmin/images/buttons/Add.png" 
                onclick="btnAddProductBvins_Click" /><uc2:ProductPicker ID="ProductPicker1" runat="server" />
        </td>
    </tr>
    </table>    

</asp:View>
<asp:View ID="viewOrderHasProduct" runat="server">
    <h1>When Order Has Product(s)...</h1>        
    <table border="0" width="100%">
    <tr>
        <td align="left">        
        When Order has at least <asp:TextBox ID="OrderProductQuantityField" runat="server" Columns="10"></asp:TextBox> of 
        <asp:DropDownList id="lstOrderProductSetMode" runat="server">
            <asp:ListItem value="1" Text="ANY"></asp:ListItem>
            <asp:ListItem value="0" Text="ALL"></asp:ListItem> 
        </asp:DropDownList> of these products:
        <asp:GridView ID="gvOrderProducts" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="Bvin" GridLines="None" onrowdeleting="gvOrderProducts_RowDeleting" 
        ShowHeader="False">
        <Columns>
            <asp:BoundField DataField="DisplayName" />
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:ImageButton ID="btnDeleteOrderProduct" runat="server" CausesValidation="False" 
                        CommandName="Delete" Text="Delete" ImageUrl="~/bvadmin/images/buttons/SmallX.png" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>        
        </td>        
        <td align="left" width="45%"><asp:ImageButton ID="btnAddOrderProduct" runat="server" 
                ImageUrl="~/bvadmin/images/buttons/Add.png" 
                onclick="btnAddOrderProduct_Click" /><uc2:ProductPicker ID="ProductPickerOrderProducts" runat="server" />
        </td>
    </tr>
    </table>    
</asp:View>
<asp:View ID="viewOrderSubTotalIs" runat="server">
    <h1>When Order Sub Total Is...</h1>
    <p>When Sub Total is >= <asp:TextBox ID="OrderSubTotalIsField" runat="server" Columns="10"></asp:TextBox></p>
</asp:View>
<asp:View ID="viewProductType" runat="server">
    <h1>When Product Type Is...</h1>

    <asp:GridView ID="gvProductTypes" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="Bvin" GridLines="None" onrowdeleting="gvProductTypes_RowDeleting" 
        ShowHeader="False">
        <Columns>
            <asp:BoundField DataField="DisplayName" />
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:ImageButton ID="btnDeleteProductType" runat="server" CausesValidation="False" 
                        CommandName="Delete" Text="Delete" ImageUrl="~/bvadmin/images/buttons/SmallX.png" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView><br />
    &nbsp;
    <table>
    <tr>
        <td><asp:ImageButton ID="btnAddProductType" runat="server" 
                ImageUrl="~/bvadmin/images/buttons/SmallPlus.png" 
                AlternateText="Add New Product Type" onclick="btnAddProductType_Click" /></td><td><asp:DropDownList ID="lstProductTypes" runat="server">
    </asp:DropDownList></td>
        
    </tr>    
    </table>
</asp:View>
<asp:View ID="viewProductCategory" runat="server">
    <h1>When Product In Category...</h1>
    <asp:GridView ID="gvProductCategories" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="Bvin" GridLines="None" onrowdeleting="gvProductCategories_RowDeleting" 
        ShowHeader="False">
        <Columns>
            <asp:BoundField DataField="DisplayName" />
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:ImageButton ID="btnDeleteProductCategory" runat="server" CausesValidation="False" 
                        CommandName="Delete" Text="Delete" ImageUrl="~/bvadmin/images/buttons/SmallX.png" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView><br />
    &nbsp;
    <table>
    <tr>
        <td><asp:ImageButton ID="btnAddProductCategory" runat="server" 
                ImageUrl="~/bvadmin/images/buttons/SmallPlus.png" 
                AlternateText="Add New Product Category" onclick="btnAddProductCategory_Click" /></td><td><asp:DropDownList ID="lstProductCategories" runat="server">
    </asp:DropDownList></td>
        
    </tr>    
    </table>
</asp:View>
<asp:View ID="viewOrderHasCoupon" runat="server">
    <h1>When Order Has Any of These Coupon Codes...</h1>

    <asp:GridView ID="gvOrderCoupons" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="Bvin" GridLines="None" onrowdeleting="gvOrderCoupons_RowDeleting" 
        ShowHeader="False">
        <Columns>
            <asp:BoundField DataField="DisplayName" />
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:ImageButton ID="btnDeleteOrderCoupon" runat="server" CausesValidation="False" 
                        CommandName="Delete" Text="Delete" ImageUrl="~/bvadmin/images/buttons/SmallX.png" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView><br />
    &nbsp;    
    <table>
    <tr>
        <td>Add Coupon Code:<td>
        <td><asp:textbox ID="OrderCouponField" runat="server" Columns="20"></asp:textbox></td>
        <td><asp:ImageButton ID="ImageButton1" runat="server" 
                ImageUrl="~/bvadmin/images/buttons/SmallPlus.png" 
                AlternateText="Add New Coupon Code" onclick="btnAddOrderCoupon_Click" /></td>            
    </tr>    
    </table>
</asp:View>
<asp:View ID="viewUserId" runat="server">
    <h1>When User Is...</h1>
    <table border="0" width="100%">
    <tr>
        <td align="left">        
        <asp:GridView ID="gvUserIs" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="Bvin" GridLines="None" onrowdeleting="gvUserIs_RowDeleting" 
        ShowHeader="False">
        <Columns>
            <asp:BoundField DataField="DisplayName" />
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:ImageButton ID="btnDeleteUserIs" runat="server" CausesValidation="False" 
                        CommandName="Delete" Text="Delete" ImageUrl="~/bvadmin/images/buttons/SmallX.png" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>        
        </td>        
        <td align="left" width="45%"><uc3:UserPicker ID="UserPicker1" runat="server" /></td>
    </tr>
    </table>    

</asp:View>
<asp:View ID="viewUserIsInGroup" runat="server">
    <h1>When User Price Group Is...</h1>

    <asp:GridView ID="gvUserIsInGroup" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="Bvin" GridLines="None" onrowdeleting="gvUserIsInGroup_RowDeleting" 
        ShowHeader="False">
        <Columns>
            <asp:BoundField DataField="DisplayName" />
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:ImageButton ID="btnDeleteUserIsInGroup" runat="server" CausesValidation="False" 
                        CommandName="Delete" Text="Delete" ImageUrl="~/bvadmin/images/buttons/SmallX.png" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView><br />
    &nbsp;
    <table>
    <tr>
        <td><asp:ImageButton ID="btnUserIsInGroupAdd" runat="server" 
                ImageUrl="~/bvadmin/images/buttons/SmallPlus.png" 
                AlternateText="Add Price Group" onclick="btnAddUserIsInGroup_Click" /></td><td><asp:DropDownList ID="lstUserIsInGroup" runat="server">
    </asp:DropDownList></td>
        
    </tr>    
    </table>
</asp:View>
<asp:View ID="viewShippingMethodIs" runat="server">
    <h1>When Shipping Method Is...</h1>
    <asp:GridView ID="gvShippingMethodIs" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="Bvin" GridLines="None" onrowdeleting="gvShippingMethodIs_RowDeleting" 
        ShowHeader="False">
        <Columns>
            <asp:BoundField DataField="DisplayName" />
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:ImageButton ID="btnDeleteShippingMethodIs" runat="server" CausesValidation="False" 
                        CommandName="Delete" Text="Delete" ImageUrl="~/bvadmin/images/buttons/SmallX.png" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView><br />
    &nbsp;
    <table>
    <tr>
        <td><asp:ImageButton ID="btnAddShippingMethodIs" runat="server" 
                ImageUrl="~/bvadmin/images/buttons/SmallPlus.png" 
                AlternateText="Add Shipping Method" onclick="btnAddShippingMethodIs_Click" /></td><td><asp:DropDownList ID="lstShippingMethodIs" runat="server">
    </asp:DropDownList></td>        
    </tr>    
    </table>
</asp:View>



</asp:MultiView>
<asp:HiddenField ID="itemid" runat="server" />
<asp:HiddenField ID="promotionid" runat="server" />
</div>