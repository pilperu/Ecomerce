<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Orders_Default" title="Untitled Page" Codebehind="Default.aspx.cs" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>

<%@ Register src="../Controls/DateRangePicker.ascx" tagname="DateRangePicker" tagprefix="uc1" %>
<asp:Content ContentPlaceHolderID="headcontent" runat="server" ID="headcontentnow">
<script type="text/javascript">
    function doPrint() {
        if (window.print) {
            window.print();
        } else {
            alert('Please choose the print button from your browser.  Usually in the menu dropdowns at File: Print');
        }
    }

    function BatchPrint() {

        var ids = "";

        $('.pickercheck').each(function () {
            if ($(this).attr('checked') == true) {
                var checkId = $(this).attr('id');
                checkId = checkId.replace('check', '')
                ids += checkId;
                ids += ",";
            }
        });

        if (ids == "") {
            alert("Please select at least one order to print first.");
            return;
        }

        var templateId = $('#lstPrintTemplate').val();

        var destination = "PrintOrder.aspx?templateid=" + templateId + "&autoprint=1&id=" + ids;
         window.location.href = destination;
    }

    $(document).ready(function () {
        $(".pickerallbutton").click(function () {
            if ($(".pickerallbutton").html() == 'All') {
                $(".pickercheck").attr('checked', true);
                $(".pickerallbutton").html('None');
            } else {
                $(".pickercheck").attr('checked', false);
                $(".pickerallbutton").html('All');
            }
            return false;
        });

        $("#btnBatchPrint").click(function () {
            BatchPrint();
            return false;
        });
    });

</script>
</asp:Content>
<asp:Content ID="sidebar" ContentPlaceHolderID="NavContent" runat="server">
    <div class="controlarea1">
        <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnGo">
        Search:

        <table>
        <tr>
            <td><asp:TextBox ID="FilterField" runat="server" Width="140px" style="margin-top:5px;"></asp:TextBox></td>
            <td><asp:ImageButton ID="btnGo" runat="server" AlternateText="Filter Results" ImageUrl="~/BVAdmin/Images/Buttons/SmallRight.png" onclick="btnGo_Click" /></td>
        </tr>
        <tr>
            <td colspan="2"><asp:DropDownList id="lstStatus" runat="server" Width="200" 
                onselectedindexchanged="lstStatus_SelectedIndexChanged" 
                AutoPostBack="True">            
            <asp:ListItem Value="F37EC405-1EC6-4a91-9AC4-6836215FBBBC" Text="New Orders"></asp:ListItem>            
            <asp:ListItem Value="e42f8c28-9078-47d6-89f8-032c9a6e1cce" Text="Ready for Payment"></asp:ListItem>
            <asp:ListItem Value="0c6d4b57-3e46-4c20-9361-6b0e5827db5a" Text="Ready for Shipping"></asp:ListItem>
            <asp:ListItem Value="09D7305D-BD95-48d2-A025-16ADC827582A" Text="Completed"></asp:ListItem>
            <asp:ListItem Value="88B5B4BE-CA7B-41a9-9242-D96ED3CA3135" Text="On Hold"></asp:ListItem>
            <asp:ListItem Value="A7FFDB90-C566-4cf2-93F4-D42367F359D5" Text="Cancelled"></asp:ListItem>            
            <asp:ListItem Value="" Text="All Orders"></asp:ListItem>
        </asp:DropDownList></td>
        </tr>
        <tr>
            <td colspan="2"><asp:DropDownList id="lstPaymentStatus" runat="server" Width="200" 
                onselectedindexchanged="lstPaymentStatus_SelectedIndexChanged" 
                AutoPostBack="True">
            <asp:ListItem Value="" Text="- Any Payment -"></asp:ListItem>
            <asp:ListItem Value="1" Text="Unpaid"></asp:ListItem>
            <asp:ListItem Value="2" Text="Partially Paid"></asp:ListItem>
            <asp:ListItem Value="3" Text="Paid"></asp:ListItem>
            <asp:ListItem Value="4" Text="Over Paid"></asp:ListItem>
        </asp:DropDownList></td>
        </tr>
        <tr>
            <td colspan="2"><asp:DropDownList id="lstShippingStatus" runat="server" Width="200" 
                onselectedindexchanged="lstShippingStatus_SelectedIndexChanged" 
                AutoPostBack="True">
            <asp:ListItem Value="" Text="- Any Shipping -"></asp:ListItem>
            <asp:ListItem Value="1" Text="Unshipped"></asp:ListItem>
            <asp:ListItem Value="2" Text="Partially Shipping"></asp:ListItem>
            <asp:ListItem Value="3" Text="Shipped"></asp:ListItem>            
        </asp:DropDownList></td>
        </tr>
        <tr>
            <td colspan="2"><uc1:DateRangePicker ID="DateRangePicker1" runat="server" /></td>
        </tr>
        <tr>
            <td colspan="2"><asp:CheckBox ID="chkNewestFirst" runat="server" Text="Newest Items First" 
                AutoPostBack="True" oncheckedchanged="chkNewestFirst_CheckedChanged" /></td>
        </tr>
        </table>
        </asp:Panel>
    </div>  
    &nbsp;<br />
    <div class="controlarea1">
    Print Selected Orders:<br />
    <asp:DropDownList ID="lstPrintTemplate" ClientIDMode="Static" runat="server"></asp:DropDownList><br />
    <asp:Image ID="btnBatchPrint" ClientIDMode="Static" runat="server" ImageUrl="~/bvadmin/images/buttons/print.png" AlternateText="Print Selected Order" />
    </div>              
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1><asp:literal ID="litH1" runat="server" Text="Order Manager"></asp:literal></h1>
    <div id="OrderManagerActions" class="padded"  runat="server" visible="false">
        <asp:LinkButton ID="lnkAcceptAll" runat="server" CssClass="btn" 
            Text="<b>Accept All New Orders</b>" onclick="lnkAcceptAll_Click" Visible="false"></asp:LinkButton>
        <asp:LinkButton ID="lnkChargeAll" runat="server" CssClass="btn" 
            Text="<b>Charge All and Mark for Shipping</b>" onclick="lnkChargeAll_Click" Visible="false"></asp:LinkButton>        
        <asp:LinkButton ID="lnkShipAll" runat="server" CssClass="btn" 
            Text="<b>Ship All Orders</b>" onclick="lnkShipAll_Click" Visible="false"></asp:LinkButton>
        <asp:LinkButton ID="lnkPrintPacking" runat="server" CssClass="btn" 
            Text="<b>Print Packing Slips & Ship All</b>" onclick="lnkPrintPacking_Click" Visible="false"></asp:LinkButton>    
    </div>    
    <uc2:MessageBox ID="MessageBox1" runat="server" />                
    <asp:Literal ID="litPager" runat="server" EnableViewState="false" />
    <asp:Literal ID="litMain" runat="server" EnableViewState="false" />
    <asp:Literal ID="litPager2" runat="server" EnableViewState="false" />

</asp:Content>

