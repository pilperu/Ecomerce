<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="true" CodeBehind="Administrators.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.People.Administrators" %>
<%@ Register src="../Controls/MessageBox.ascx" tagname="MessageBox" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" runat="server">
</asp:Content>


<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">            
             <div class="controlarea1">
                        <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnGo">
                            Add New Administrator:
                            <table>
                            <tr>
                                <td><asp:TextBox ID="NewAdminField" runat="server" Width="140px" style="margin-top:5px;"></asp:TextBox></td>
                                <td><asp:ImageButton ID="btnGo" runat="server" AlternateText="Add Administrator" 
                                ImageUrl="~/BVAdmin/Images/Buttons/SmallRight.png" onclick="btnGo_Click" /></td>
                            </tr>
                            </table>
                            </asp:Panel>
            </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<uc1:MessageBox ID="MessageBox1" runat="server" />
<h1>Administrators</h1>                   
    <asp:Literal ID="litAdministrators" ClientIDMode="Static" runat="server" EnableViewState="false"></asp:Literal>
    <div class="clear"></div>
</asp:Content>

