<%@ Control Language="C#" AutoEventWireup="True"
    Inherits="MerchantTribeStore.BVAdmin_Controls_ProductEditMenu" Codebehind="ProductEditMenu.ascx.cs" %>
<asp:Panel ID="pnlMenu" runat="server" DefaultButton="btnContinue">
<ul class="navmenu">
    <li>
        <asp:LinkButton ID="lnkGeneral" runat="server" Text="Name, Description, Price" 
            onclick="lnkGeneral_Click"></asp:LinkButton></li>
    <li>
        <asp:LinkButton ID="lnkAdditionalImages" runat="server"
            Text="Additional Images" onclick="lnkAdditionalImages_Click1"></asp:LinkButton></li>
    <li>
        <asp:LinkButton ID="lnkCategories" runat="server" Text="Categories" 
            onclick="lnkCategories_Click"></asp:LinkButton></li>
    <li>
        <asp:LinkButton ID="lnkCustomerChoices" runat="server" Text="Choices - Edit" 
            onclick="lnkCustomerChoices_Click"></asp:LinkButton></li>
    <li>
        <asp:LinkButton ID="lnkVariants" runat="server" Text="Choices - Variants" 
            onclick="lnkVariants_Click"></asp:LinkButton></li>  
    <li>
        <asp:LinkButton ID="lnkFiles" runat="server" Text="File Downloads" 
            onclick="lnkFiles_Click1"></asp:LinkButton></li>      
    <li>
        <asp:LinkButton ID="lnkInventory" runat="server" Text="Inventory" 
            onclick="lnkInventory_Click1"></asp:LinkButton></li>                
    <li>
        <asp:LinkButton ID="lnkUpSellCrossSell" runat="server" Text="Related Items" OnClick="lnkUpSellCrossSell_Click"></asp:LinkButton></li>
    <li>
        <asp:LinkButton ID="lnkInfoTabs" runat="server" Text="Info Tabs" onclick="lnkInfoTabs_Click" 
            ></asp:LinkButton></li>
    <li>
        <asp:LinkButton ID="lnkProductReviews" runat="server" Text="Reviews" 
            onclick="lnkProductReviews_Click"></asp:LinkButton></li>
    <li>
        <asp:LinkButton ID="lnkVolumeDiscounts" runat="server" Text="Volume Discounts" 
            onclick="lnkVolumeDiscounts_Click"></asp:LinkButton></li>
    
    <li><div class="padded"><asp:ImageButton runat="server" ID="btnContinue" 
            ImageUrl="~/BVAdmin/Images/Buttons/Close.png" 
            onclick="btnContinue_Click" /></div></li>
</ul>
</asp:Panel>

