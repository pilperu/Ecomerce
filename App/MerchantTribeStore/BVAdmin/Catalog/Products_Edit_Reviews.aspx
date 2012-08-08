<%@ Page MasterPageFile="~/BVAdmin/BVAdminNav.Master" Language="C#"
    AutoEventWireup="True" Inherits="MerchantTribeStore.products_products_edit_reviews" Codebehind="Products_Edit_Reviews.aspx.cs" %>
<%@ Register src="../Controls/ProductEditMenu.ascx" tagname="ProductEditMenu" tagprefix="uc5" %>
<%@ Register src="../Controls/ProductEditingDisplay.ascx" tagname="ProductEditing" tagprefix="uc5" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc5:ProductEditMenu ID="ProductNavigator" runat="server" />
    <uc5:ProductEditing ID="ProductEditing1" runat="server" />
</asp:Content>
    
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<h1>Customer Reviews</h1>
    <table class="onecolumn">
        <tr>
            <td id="AdminContentWithNav">                
                &nbsp;<br/>
                <asp:ImageButton ID="btnNew" runat="server" 
                    ImageUrl="../images/buttons/new.png" onclick="btnNew_Click">
                </asp:ImageButton><br/>
                &nbsp;
                <asp:DataList DataKeyField="Bvin" ID="dlReviews" runat="server" 
                    ondeletecommand="dlReviews_DeleteCommand" oneditcommand="dlReviews_EditCommand" 
                    onitemdatabound="dlReviews_ItemDataBound">
                    <ItemTemplate>
                        <div id="ReviewInfo">
                            <table border="0" cellspacing="0" cellpadding="3">
                                <tr>
                                    <td align="right" valign="top" class="Formlabel">
                                        <asp:Image ID="imgRating" runat="server" ImageUrl="~/BVModules/Themes/BVC5/images/buttons/Stars5.png"></asp:Image></td>
                                    <td align="left" valign="top" class="BVSmallText">
                                        <asp:Label ID="lblReviewDate" runat="server">
												<%#Eval("ReviewDate")%>
                                        </asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top" class="Formlabel">
                                        Review</td>
                                    <td align="left" valign="top" class="BVSmallText">
                                        <asp:Label ID="lblReview" runat="server">
												<%#Eval("Description")%>
                                        </asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                    <td align="left" valign="top">
                                        <div id="ReviewControls">
                                            <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="../images/buttons/Edit.png"
                                                CommandName="Edit" AlternateText="Edit"></asp:ImageButton>&nbsp;&nbsp;
                                            <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="../images/buttons/Delete.png"
                                                CommandName="Delete" AlternateText="Delete"></asp:ImageButton></div>
                                    </td>
                                </tr>
                            </table>
                            &nbsp;
                        </div>
                    </ItemTemplate>
                </asp:DataList>
            </td>
        </tr>
    </table>
</asp:Content>
