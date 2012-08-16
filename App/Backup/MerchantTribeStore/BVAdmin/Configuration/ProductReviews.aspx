<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_ProductReviews" title="Untitled Page" Codebehind="ProductReviews.aspx.cs" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
  
    <h1>Product Reviews</h1>
        
        		<table cellspacing="0" cellpadding="0" border="0" id="container">
				<tr>
					<td><uc1:MessageBox ID="msg" runat="server" />
					<table cellspacing="0" cellpadding="5" border="0">
						<tr>
							<td class="FormLabel" valign="top" align="right">Moderate Product Reviews?</td>
							<td valign="top" align="left"><asp:checkbox id="chkProductReviewModerate" Runat="server"></asp:checkbox></td>
						</tr>
						<tr>
							<td class="FormLabel" valign="top" align="right">Allow Product Rating?</td>
							<td valign="top" align="left"><asp:checkbox id="chkProductReviewShowRating" Runat="server"></asp:checkbox></td>
						</tr>						
						<tr>
							<td class="FormLabel" valign="top" align="right">Show how many reviews at first?</td>
							<td valign="top" align="left"><asp:textbox id="ProductReviewCountField" runat="server" CssClass="FormInput" Columns="3">3</asp:textbox></td>
						</tr>
						<tr>
							<td align="left"><asp:imagebutton id="btnCancel" Runat="server" 
                                    CausesValidation="False" ImageUrl="../images/buttons/Cancel.png" 
                                    onclick="btnCancel_Click"></asp:imagebutton></td>
							<td align="right"><asp:imagebutton id="btnSave" Runat="server" 
                                    ImageUrl="../images/buttons/OK.png" onclick="btnSave_Click"></asp:imagebutton></td>
						</tr>
					</table></td>
				</tr>
			</table>	
</asp:Content>

