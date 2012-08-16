<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_EventLog" Title="Untitled Page" Codebehind="EventLog.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>
        Event Log</h1>
    <table class="FormTable" cellspacing="0" cellpadding="5" width="100%" border="0">
        <tr>
            <td>
                <asp:ImageButton ID="ClearEventsButton" ImageUrl="../images/buttons/ClearAll.png"
                    OnClientClick="return window.confirm('Are you sure you want to clear all events?');"
                    runat="server" Visible="false" ToolTip="Clear All" onclick="ClearEventsButton_Click" />
            </td>
        </tr>
        <tr>
            <td valign="top" align="left">
                <strong>
                    <asp:Label runat="server" Text="Filter By Error Type: " ID="Filter"></asp:Label></strong>&nbsp;&nbsp;
                <asp:DropDownList ID="ddlFilter" runat="server">
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="1">Information</asp:ListItem>
                    <asp:ListItem Value="2">Warning</asp:ListItem>
                    <asp:ListItem Value="3">Error</asp:ListItem>
                </asp:DropDownList>
                <asp:ImageButton ID="btnNext" runat="server" ImageUrl="../images/buttons/right.png"
                    ToolTip="Next" onclick="btnNext_Click" />
            </td>
        </tr>
        <tr>
            <td class="FormLabel" valign="top">
                <asp:Literal ID="litOutput" runat="server" EnableViewState="false"></asp:Literal>                
            </td>
        </tr>
    </table>
</asp:Content>
