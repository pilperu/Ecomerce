<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Orders_ReceivePayments"
    Title="Payments" Codebehind="ReceivePayments.aspx.cs" %>

<%@ Register Src="../../BVModules/Controls/CreditCardInput.ascx" TagName="CreditCardInput"
    TagPrefix="uc2" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register src="ReceivePaymentsControl.ascx" tagname="ReceivePaymentsControl" tagprefix="uc3" %>
<%@ Register src="OrderActions.ascx" tagname="OrderActions" tagprefix="uc4" %>
<%@ Register src="OrderStatusDisplay.ascx" tagname="OrderStatusDisplay" tagprefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <uc1:MessageBox id="MessageBox1" runat="server" />
    <table border="0" cellspacing="0" cellpadding="3" width="100%">
        <tr>
            <td width="33%" class="formfield">
                <h1>Order <asp:Label ID="OrderNumberField" runat="server" Text="000000"></asp:Label> Payments</h1>
            <table class="controlarea1" style="color: #666; font-size: 11px;" border="0" cellspacing="0"
                    cellpadding="3" width="100%">
                    <tr>
                        <td colspan="2" class="formfield" style="border-bottom: solid 1px #999;">
                            Total Payment Status for Order</td>
                    </tr>
                    <tr>
                        <td class="formfield">
                            Held/Reserved:</td>
                        <td class="formlabel">
                            <asp:Label ID="PaymentAuthorizedField" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="formfield">
                            Received:</td>
                        <td class="formlabel">
                            <asp:Label ID="PaymentChargedField" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="formfield" style="border-bottom: solid 1px #999; padding-bottom: 3px;">
                            Refunded:</td>
                        <td class="formlabel" style="border-bottom: solid 1px #999; padding-bottom: 3px;">
                            <asp:Label ID="PaymentRefundedField" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="formfield">
                            Amount Due:</td>
                        <td class="formlabel" style="color: #333;">
                            <strong>
                                <asp:Label ID="PaymentDueField" runat="server"></asp:Label></strong></td>
                    </tr>
                </table><br />&nbsp;</td>

            </td>
            <td align="left" valign="top">
                <uc5:OrderStatusDisplay ID="OrderStatusDisplay1" runat="server" /><br />
                &nbsp;<br />
            <td class="formlabel" width="33%"><uc4:OrderActions ID="OrderActions1" runat="server" />
            </td>
        </tr>
        <tr>
            <td><h3>Transactions</h3></td>
            <td colspan="2"><h3>Actions</h3></td>
        </tr>
        <tr>
            <td valign="top">
                <div class="padded">
                    <asp:Literal ID="litTransactions" runat="server" EnableViewState="false" />                
                </div>
            </td>
            <td class="formfield" valign="top" colspan="2">
                <div class="padded">
                <uc3:ReceivePaymentsControl ID="ReceivePaymentsControl1" runat="server" />                
                </div>
            </td>            
        </tr>
    </table>
    <asp:HiddenField ID="BvinField" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="headcontent" runat="server" ID="headcontentnow">
<script type="text/javascript">
    function doPrint() {
        if (window.print) {
            window.print();
        } else {
            alert('Please choose the print button from your browser.  Usually in the menu dropdowns at File: Print');
        }
    } 
</script>
</asp:Content>
