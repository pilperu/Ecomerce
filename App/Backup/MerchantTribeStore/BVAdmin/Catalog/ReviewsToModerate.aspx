<%@ Page MasterPageFile="~/BVAdmin/BVAdmin.Master" Language="C#" AutoEventWireup="True"
    Inherits="MerchantTribeStore.Products_ReviewsToModerate" Codebehind="ReviewsToModerate.aspx.cs" %>
        
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Product Reviews to Moderate</h1>
    <asp:Label ID="lblNoReviews" runat="server" Text="No Reviews to Moderate" Visible="false"></asp:Label>
    <asp:DataList DataKeyField="bvin" ID="dlReviews" runat="server" 
        ondeletecommand="dlReviews_DeleteCommand" oneditcommand="dlReviews_EditCommand" 
        onitemdatabound="dlReviews_ItemDataBound" 
        onupdatecommand="dlReviews_UpdateCommand">
        <ItemTemplate>
            <div id="ReviewInfo">
                <table border="0" cellspacing="0" cellpadding="3">
                    <tr>
                        <td align="right" valign="top" class="Formlabel">
                            <asp:Image ID="imgRating" runat="server" ImageUrl="../../images/buttons/Stars5.png">
                            </asp:Image></td>
                        <td align="left" valign="top" class="BVSmallText">
                            <asp:Label ID="lblReviewDate" runat="server">
												<%#Eval("ReviewDate")%>
                            </asp:Label></td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" class="Formlabel">
                            Product:</td>
                        <td align="left" valign="top" class="BVSmallText">
                            <asp:Label ID="lblProductID" runat="server">
												<%#Eval("ProductName")%>
                            </asp:Label></td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" class="Formlabel">
                            Review:</td>
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
                                <asp:ImageButton ID="btnApprove" CommandName="Update" runat="server" ImageUrl="../images/buttons/Approve.png"
                                    AlternateText="Approve"></asp:ImageButton>&nbsp;&nbsp;
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
</asp:Content>
