<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminPopup.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_GettingStartedChecklist" title="Untitled Page" Codebehind="GettingStartedChecklist.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BvcAdminPopupConent" Runat="Server">
			<div id="popup">
				<h3>Getting Started Checklist</h3>
				<ul>
					<li>
						<a href="#" onclick="JavaScript:window.open('ChangeEmail.aspx','GettingStarted','width=500, height=400, menubar=no, scrollbars=yes, resizable=yes, status=no, toolbar=no')">Change admin username and 
							password from default</a></li>
					<li>
						<a href="Content/StoreInfo.aspx" target="_blank">Setup Contact Information</a></li>
					<li>
						<a href="Content/StoreInfo.aspx" target="_blank">Upload your logo</a></li>
					<li>
						<a href="Configuration/Themes.aspx" target="_blank">Select a Theme</a></li>
					<li>
						<a href="Content/Default.aspx" target="_blank">Setup your homepage</a></li>
					<li>
						<a href="Configuration/Security.aspx" target="_blank">Turn on SSL</a></li>
					<li>
					    <a href="Configuration/MailServer.aspx" target="_blank">Configure Mail Server</a></li>
					<li>
						<a href="Configuration/Payment.aspx" target="_blank">Setup Payment Methods</a></li>
					<li>
						<a href="Configuration/Shipping.aspx" target="_blank">Setup Shipping Methods</a></li>					
					<li>
						<a href="Catalog/Categories.aspx" target="_blank">Add Categories</a></li>
					<li>
						<a href="Catalog/Default.aspx" target="_blank">Add Products</a></li>
				    <li>
						<a href="Content/Policies.aspx" target="_blank">Change Privacy Policy</a></li>
					<li>
						<a href="Content/Policies.aspx" target="_blank">Change Terms and Conditions</a></li>
					<li>
						<a href="Content/Policies.aspx" target="_blank">Change Return Policy</a></li>
					<li>
						<a href="Content/Policies.aspx" target="_blank">Change Shipping Policy</a></li>					
					<li>
						<a href="Content/MetaTags.aspx" target="_blank">Setup Meta Tags</a></li>				
					<li>
						<a href="Content/EmailTemplates.aspx" target="_blank">Setup Email Templates</a></li>
					<li>
					<a href="Configuration/Default.aspx" target="_blank">More Settings and Options</a></li>
				</ul>
			</div>
			<div style="text-align:center">
			&nbsp;<br>
			&nbsp;<a onClick="JavaScript:window.close();" href="#"><img src="Images/Buttons/OK.png" border="0" alt="Close Window"></a><br>
			&nbsp;<br>
			</div>
</asp:Content>

