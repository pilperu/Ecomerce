<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_Shipping_Zones_Edit" Codebehind="Shipping_Zones_Edit.aspx.cs" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <h1>Edit Shipping Zone</h1>
    <uc1:MessageBox ID="msg" runat="server" />
    <div class="controlarea1">
    Zone Name: <asp:TextBox ID="ZoneNameField" runat="server" Columns="30"></asp:TextBox>
    </div>
    <div class="editorcontrols">
        <asp:ImageButton ID="btnSaveChanges" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" 
            onclick="btnSaveChanges_Click" />
    </div>
    &nbsp;<br />
    &nbsp;<br />
    <h2>Areas In This Zone</h2>

    <table width="100%" id="ratetable">
        <asp:Literal ID="litAreas" runat="server">
        </asp:Literal>        
   </table>
   <div class="controlarea2">
            <table>
            <tr>                
                <td><asp:DropDownList ID="lstCountry" TabIndex="300" runat="server" 
                                AutoPostBack="True" onselectedindexchanged="lstCountry_SelectedIndexChanged">
                            </asp:DropDownList></td>
                <td> <asp:DropDownList ID="lstState" TabIndex="1000" runat="server">
                            </asp:DropDownList></td>
                <td><asp:ImageButton ID="btnNew" runat="server" 
                        ImageUrl="~/BVAdmin/Images/Buttons/New.png" onclick="btnNew_Click" /></td>
            </tr>
            </table>
   </div>

</asp:Content>
