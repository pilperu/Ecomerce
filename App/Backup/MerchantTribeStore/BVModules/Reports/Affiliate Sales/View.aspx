<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Reports_Affiliates" title="Affiliate Sales" Codebehind="View.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <script type="text/javascript">				
		function toggle(id)
		{			
				var carrotName = id+'Carrot';
				var detailsDiv = document.getElementById(id);
				
				if (detailsDiv){				    
				    if (detailsDiv.currentStyle){				        
				        if (detailsDiv.currentStyle.display != "none"){
				            detailsDiv.style.display = "none";
				        } else {				            
                            detailsDiv.style.display = "block";
				        }
				    } else {
                    	if (document.defaultView.getComputedStyle(detailsDiv, '').getPropertyValue("display") != "none"){
                            detailsDiv.style.display = "none";                    	
                    	} else {
                    	    detailsDiv.style.display = "block";
                    	}			
				    }
				}				
		}		
    </script>
    <h1>Sales By Affiliate</h1>
    <div>
        <asp:ImageButton ID="PreviousImageButton" runat="server" 
            AlternateText="Previous" ImageUrl="~/BVAdmin/Images/Buttons/Left.png" 
            onclick="PreviousImageButton_Click" />
        &nbsp;Month:&nbsp;
        <asp:DropDownList ID="MonthDropDownList" runat="server">
            <asp:ListItem Enabled="true" Text="January" Value="1" Selected="true"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="February" Value="2" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="March" Value="3" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="April" Value="4" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="May" Value="5" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="June" Value="6" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="July" Value="7" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="August" Value="8" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="September" Value="9" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="October" Value="10" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="November" Value="11" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="December" Value="12" Selected="false"></asp:ListItem>
        </asp:DropDownList>
        Year:&nbsp;         
        <asp:DropDownList ID="YearDropDownList" runat="server">
            <asp:ListItem Enabled="true" Text="1996" Selected="true"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="1997" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="1998" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="1999" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="2000" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="2001" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="2002" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="2003" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="2004" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="2005" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="2006" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="2007" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="2008" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="2009" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="2010" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="2011" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="2012" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="2013" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="2014" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="2015" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="2016" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="2017" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="2018" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="2019" Selected="false"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="2020" Selected="false"></asp:ListItem>
        </asp:DropDownList>
        &nbsp;<asp:ImageButton ID="NextImageButton" runat="server" AlternateText="Next" 
            ImageUrl="~/BVAdmin/Images/Buttons/Right.png" onclick="NextImageButton_Click" />
    </div>
    <br />
    <div>
        Affiliate:&nbsp;
        <asp:DropDownList ID="AffiliatesDropDownList" runat="server">
        </asp:DropDownList>
        &nbsp;
        <asp:ImageButton ID="ViewImageButton" AlternateText="View" 
            ImageUrl="~/BVAdmin/Images/Buttons/View.png" runat="server" 
            onclick="ViewImageButton_Click" /><br /><br />
    </div>
    <div>
        <asp:DataList ID="AffiliatesDataList" runat="server" ExtractTemplateRows="true" 
            Style="width: 100%;" onitemdatabound="AffiliatesDataList_ItemDataBound">
            <ItemStyle CssClass="row" />
            <HeaderStyle CssClass="rowheader" />
            <AlternatingItemStyle CssClass="alternaterow" />
            <SeparatorTemplate>
                <asp:Table runat="server">
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="4">
                            <div style="border-top: solid 1px black; width: 100%;"></div>                        
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </SeparatorTemplate>
            <HeaderTemplate>
                <asp:Table runat="server">
                    <asp:TableHeaderRow runat="server" CssClass="rowheader">
                        <asp:TableHeaderCell>Referrals</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Sales</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Conversion</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Commission</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Table ID="AffiliateTable" runat="server">
                    <asp:TableRow>
                       <asp:TableCell ColumnSpan="4"><%# Eval("DisplayName") %></asp:TableCell>
                    </asp:TableRow>                
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="ReferralsLabel" runat="server" Text="Label"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="SalesLabel" runat="server" Text="Label"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="ConversionLabel" runat="server" Text="Label"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="CommissionLabel" runat="server" Text="Label"></asp:Label>                        
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="4">
                            
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="4">
                            <asp:Literal id="openDiv" Runat="server"></asp:Literal>
                            <div id="Details" runat="server">
                                <asp:GridView ID="OrdersGridView" runat="server" AutoGenerateColumns="false" style="width: 100%; margin-left: 20px;">
                                    <Columns>
                                        <asp:BoundField HeaderText="Order #" DataField="OrderNumber" />
                                        <asp:BoundField HeaderText="User"  DataField="UserID" />
                                        <asp:BoundField HeaderText="Item Total" DataField="TotalOrderAfterDiscounts" HtmlEncode="false" DataFormatString="{0:c}" />
                                        <asp:BoundField HeaderText="Time" DataField="TimeOfOrder" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        There were no orders found for this affiliate
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                            <asp:Literal id="closeDiv" Runat="server"></asp:Literal><br>
                        </asp:TableCell>
                    </asp:TableRow>                    
                </asp:Table>                
            </ItemTemplate>
        </asp:DataList>
    </div>
</asp:Content>

