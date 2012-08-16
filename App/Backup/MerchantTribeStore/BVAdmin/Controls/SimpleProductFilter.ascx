<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Controls_SimpleProductFilter" Codebehind="SimpleProductFilter.ascx.cs" %>
<asp:Panel ID="Panel1" runat="server" DefaultButton="btnGo">
    <div class="controlarea1">        
            Search Products by Keyword:<br />
            <asp:TextBox ID="FilterField" runat="server" Width="135px"></asp:TextBox>&nbsp;<asp:ImageButton 
                ID="btnGo" runat="server" AlternateText="Filter Results" 
                ImageUrl="~/BVAdmin/Images/Buttons/Go.png" onclick="btnGo_Click" /><br />
            
            
            <asp:DropDownList ID="ProductTypeFilter" runat="server" AutoPostBack="True" 
                Width="170px" onselectedindexchanged="ProductTypeFilter_SelectedIndexChanged">                                        
                </asp:DropDownList><br />
                <asp:DropDownList ID="CategoryFilter" runat="server" AutoPostBack="True" 
                Width="170px" onselectedindexchanged="CategoryFilter_SelectedIndexChanged">                    
                </asp:DropDownList><br />
                 <asp:DropDownList ID="ManufacturerFilter" runat="server" 
                AutoPostBack="True" Width="170px" 
                onselectedindexchanged="ManufacturerFilter_SelectedIndexChanged">                    
                </asp:DropDownList><br />
                <asp:DropDownList ID="VendorFilter" runat="server" AutoPostBack="True" 
                Width="170px" onselectedindexchanged="VendorFilter_SelectedIndexChanged">                    
                </asp:DropDownList><br />
                <asp:DropDownList ID="StatusFilter" runat="server" AutoPostBack="True" 
                Width="170px" onselectedindexchanged="StatusFilter_SelectedIndexChanged">                    
                </asp:DropDownList><br />
                <asp:DropDownList ID="InventoryStatusFilter" runat="server" 
                AutoPostBack="True" Width="170px" 
                onselectedindexchanged="InventoryStatusFilter_SelectedIndexChanged">                    
                </asp:DropDownList>
   </div>        
</asp:Panel>