<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Orders_ReceivePaymentsControl" Codebehind="ReceivePaymentsControl.ascx.cs" %>
<%@ Register src="../Controls/MessageBox.ascx" tagname="MessageBox" tagprefix="uc1" %>
<%@ Register src="../../BVModules/Controls/CreditCardInput.ascx" tagname="CreditCardInput" tagprefix="uc2" %>
<uc1:messagebox ID="MessageBox1" runat="server" />                
<table border="0" cellspacing="0" cellspacing="0">
<tr>
    <td><asp:LinkButton id="lnkCC" runat="server" CssClass="btn" 
            Text="<b>Credit Cards</b>" onclick="lnkCC_Click"></asp:LinkButton></td>
    <td><asp:LinkButton id="lnkPayPal" runat="server" CssClass="btn" Text="<b>PayPal</b>" onclick="lnkPayPal_Click"></asp:LinkButton></td>
    <td><asp:LinkButton id="lnkPO" runat="server" CssClass="btn" 
            Text="<b>PO</b>" onclick="lnkPO_Click"></asp:LinkButton></td>
    <td><asp:LinkButton id="lnkCompanyAccount" runat="server" CssClass="btn" 
            Text="<b>Comp. Acct.</b>" onclick="lnkCompanyAccount_Click"></asp:LinkButton></td>
    <td><asp:LinkButton id="lnkCash" runat="server" CssClass="btn" Text="<b>Cash</b>" 
            onclick="lnkCash_Click"></asp:LinkButton></td>
    <td><asp:LinkButton id="lnkCheck" runat="server" CssClass="btn" Text="<b>Check</b>" 
            onclick="lnkCheck_Click"></asp:LinkButton></td>
    <td><asp:LinkButton id="lnkPoints" runat="server" CssClass="btn" Text="<b>Points</b>" 
            onclick="lnkPoints_Click"></asp:LinkButton></td>
</tr>
</table>
<asp:MultiView ID="mvPayments" runat="server">
    <asp:View ID="viewCreditCards" runat="server">
        <div class="controlarea2 padded">
            
            <table border="0" cellspacing="0" cellpadding="3" width="100%">
            <tr>
                <td width="50%"><h4>Pending Holds</h4>
                <table>
                <tr>
                    <td class="formlabel">Hold:</td>
                    <td class="formfield"><asp:DropDownList ID="lstCreditCardAuths" runat="server"></asp:DropDownList></td>                    
                </tr>
                <tr>
                    <td class="formlabel">Amount:</td>
                    <td class="formfield"><asp:TextBox ID="CreditCardAuthAmount" runat="server" Columns="10"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="formlabel"><asp:LinkButton ID="lnkCreditCardVoidAuth" runat="server" 
                            CssClass="btn" Text="<b>Void Hold</b>" onclick="lnkCreditCardVoidAuth_Click" /></td>
                    <td class="formfield"><asp:LinkButton ID="lnkCreditCardCaptureAuth" runat="server" 
                            CssClass="btn" Text="<b>Capture Hold</b>" 
                            onclick="lnkCreditCardCaptureAuth_Click" /></td>
                </tr>
                </table><br />
                &nbsp;
                </td>
                <td><h4>New Charges</h4>                
                <table>
                <tr>
                    <td class="formlabel">Card:</td>
                    <td class="formfield"><asp:DropDownList ID="lstCreditCards" runat="server"></asp:DropDownList></td>                    
                </tr>
                <tr>
                    <td class="formlabel">Amount:</td>
                    <td class="formfield"><asp:TextBox ID="CreditCardChargeAmount" runat="server" Columns="10"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="formlabel"><asp:LinkButton ID="lnkCreditCardNewAuth" runat="server" 
                            CssClass="btn" Text="<b>Hold Funds</b>" onclick="lnkCreditCardNewAuth_Click" /></td>
                    <td class="formfield"><asp:LinkButton ID="lnkCreditCardCharge" runat="server" 
                            CssClass="btn" Text="<b>Charge Card</b>" onclick="lnkCreditCardCharge_Click" /></td>
                </tr>
                </table><br />
                &nbsp;
                </td>
            </tr>
            <tr>
                <td width="50%"><h4>Refunds</h4>                    
                    <table>
                    <tr>
                        <td class="formlabel">Charge:</td>
                        <td class="formfield"><asp:DropDownList ID="lstCreditCardCharges" runat="server"></asp:DropDownList></td>                    
                    </tr>
                    <tr>
                        <td class="formlabel">Amount:</td>
                        <td class="formfield"><asp:TextBox ID="CreditCardRefundAmount" runat="server" Columns="10"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="formlabel">&nbsp;</td>
                        <td class="formfield"><asp:LinkButton ID="lnkCreditCardRefund" runat="server" 
                                CssClass="btn" Text="<b>Refund Charge</b>" 
                                onclick="lnkCreditCardRefund_Click" /></td>
                    </tr>
                    </table><br />
                    &nbsp;
                </td>
                <td><h4>Add a New Card</h4>
                    <uc2:CreditCardInput ID="CreditCardInput1" runat="server" />                    
                    <div style="width:85px;float:left;">&nbsp;</div>
                    <asp:LinkButton ID="lnkCreditCardAddInfo" runat="server" CssClass="btn" 
                        Text="<b>Save Card to Order</b>" onclick="lnkCreditCardAddInfo_Click" /><br />
                    &nbsp;
                </td>
            </tr>
            </table>
        </div>    
    </asp:View>
    <asp:View ID="viewPayPal" runat="server">
        <div class="controlarea2 padded">
            
            <table border="0" cellspacing="0" cellpadding="3" width="100%">
            <tr>
                <td width="50%"><h4>PayPal Holds</h4>
                <table>
                <tr>
                    <td class="formlabel">Hold:</td>
                    <td class="formfield"><asp:DropDownList ID="lstPayPalHold" runat="server"></asp:DropDownList></td>                    
                </tr>
                <tr>
                    <td class="formlabel">Amount:</td>
                    <td class="formfield"><asp:TextBox ID="PayPalHoldAmount" runat="server" Columns="10"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="formlabel">
                        <asp:LinkButton ID="lnkPayPalVoidHold" runat="server" 
                            CssClass="btn" Text="<b>Void Hold</b>" onclick="lnkPayPalVoidHold_Click" /></td>
                    <td class="formfield">
                        <asp:LinkButton ID="lnkPayPalCaptureHold" runat="server" 
                            CssClass="btn" Text="<b>Capture Hold</b>" onclick="lnkPayPalCaptureHold_Click" 
                             /></td>
                </tr>
                </table><br />
                &nbsp;
                </td>
            </tr>
            <tr>
                <td><h4>Refunds</h4>                    
                    <table>
                    <tr>
                        <td class="formlabel">Charge:</td>
                        <td class="formfield"><asp:DropDownList ID="lstPayPalRefund" runat="server"></asp:DropDownList></td>                    
                    </tr>
                    <tr>
                        <td class="formlabel">Amount:</td>
                        <td class="formfield"><asp:TextBox ID="PayPalRefundAmount" runat="server" Columns="10"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="formlabel">&nbsp;</td>
                        <td class="formfield"><asp:LinkButton ID="lnkPayPalRefund" runat="server" 
                                CssClass="btn" Text="<b>Refund Charge</b>" 
                                onclick="lnkPayPalRefund_Click" /></td>
                    </tr>
                    </table><br />
                    &nbsp;
                </td>               
            </tr>
            </table>
        </div>    
    </asp:View>
    <asp:View ID="viewPO" runat="server">
        <div class="controlarea2 padded">
            <h4>Purchase Order</h4>   
            <table class="formtable">
            <tr>
                <td class="formlabel">PO Number:</td>
                <td class="formfield"><asp:DropDownList id="lstPO" runat="server"></asp:DropDownList></td>
                <td rowspan="5" style="width:200px;font-size:11px;padding:0 0 0 10px;"><b>About Purchase Orders</b><br />
                <p style="margin:0 0 1em 0;">A purchase order is a promise to pay by a company. Accepting a PO is the same as accepting cash and the system will release the order for shipping. It is your responsibility to send an invoice to the customer and collect payment on the PO.</p>
                <p>Before accepting a PO, make sure the company has a good credit rating. Once accepted you can't undo the credit to the order.</p></td>
            </tr>
            <tr>
                <td class="formlabel">
                    &nbsp;</td>
                <td class="formfield">
                    <asp:LinkButton ID="lnkPOAccept" runat="server" 
                        class="btn" Text="<b>Accept Purchase Order</b>" 
                        onclick="lnkPOAccept_Click" /></td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;<br />                
                <hr />
                &nbsp;<br /></td>
            </tr>
            <tr>
                <td class="formlabel">PO Number:</td>
                <td class="formfield"><asp:TextBox ID="PONewNumber" runat="server" Columns="20"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="formlabel">Amount:</td>
                <td class="formfield"><asp:TextBox ID="PONewAmount" runat="server" Columns="10"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="formlabel">
                    &nbsp;</td>
                <td class="formfield">
                    <asp:LinkButton ID="lnkPOAdd" runat="server" 
                        class="btn" Text="<b>Add Purchase Order Information</b>" 
                        onclick="lnkPOAdd_Click" /></td>
            </tr>
            </table><br />
            &nbsp;
        </div>    
    </asp:View>
    <asp:View ID="viewCompanyAccount" runat="server">
        <div class="controlarea2 padded">
            <h4>Company Account</h4>   
            <table class="formtable">
            <tr>
                <td class="formlabel">Account Number:</td>
                <td class="formfield"><asp:DropDownList id="lstCompanyAccount" runat="server"></asp:DropDownList></td>
                <td rowspan="5" style="width:200px;font-size:11px;padding:0 0 0 10px;"><b>About Company Accounts</b><br />
                <p style="margin:0 0 1em 0;">Company Accounts must be verified offline. Accepting a company account as payment is the same as accepting cash and the system will release the order for shipping.</p>
                </td>
            </tr>
            <tr>
                <td class="formlabel">
                    &nbsp;</td>
                <td class="formfield">
                    <asp:LinkButton ID="lnkCompanyAccountAccept" runat="server" 
                        class="btn" Text="<b>Accept CompanyAccount</b>" 
                        onclick="lnkCompanyAccountAccept_Click" /></td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;<br />                
                <hr />
                &nbsp;<br /></td>
            </tr>
            <tr>
                <td class="formlabel">Account Number:</td>
                <td class="formfield"><asp:TextBox ID="CompanyAccountNewNumber" runat="server" Columns="20"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="formlabel">Amount:</td>
                <td class="formfield"><asp:TextBox ID="CompanyAccountNewAmount" runat="server" Columns="10"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="formlabel">
                    &nbsp;</td>
                <td class="formfield">
                    <asp:LinkButton ID="lnkCompanyAccountAdd" runat="server" 
                        class="btn" Text="<b>Add Company Account Information</b>" 
                        onclick="lnkCompanyAccountAdd_Click" /></td>
            </tr>
            </table><br />
            &nbsp;
        </div>    
    </asp:View>
    <asp:View ID="viewCash" runat="server">
        <div class="controlarea2 padded">
            <h4>Cash</h4>    
            <table class="formtable">
            <tr>
                <td class="formlabel">Amount:</td>
                <td class="formfield"><asp:TextBox ID="CashAmount" runat="server" Columns="10"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="formlabel"><asp:LinkButton ID="btnCashRefund" runat="server" class="btn" 
                        Text="<b>Refund Cash</b>" onclick="btnCashRefund_Click" /></td>
                <td class="formfield"><asp:LinkButton ID="btnCashReceive" runat="server" 
                        class="btn" Text="<b>Receive Cash</b>" onclick="btnCashReceive_Click" /></td>
            </tr>
            </table>
        </div>    
    </asp:View>
    <asp:View ID="viewCheck" runat="server">
        <div class="controlarea2 padded">           
            <h4>Check</h4>    
            <table class="formtable">
            <tr>
                <td class="formlabel">Check Number:</td>
                <td class="formfield"><asp:TextBox ID="CheckNumberField" runat="server" Columns="10"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="formlabel">Amount:</td>
                <td class="formfield"><asp:TextBox ID="CheckAmountField" runat="server" Columns="10"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="formlabel">
                    <asp:LinkButton ID="lnkCheckReturn" runat="server" class="btn" 
                        Text="<b>Return Check</b>" onclick="lnkCheckReturn_Click" /></td>
                <td class="formfield">
                    <asp:LinkButton ID="lnkCheckReceive" runat="server" 
                        class="btn" Text="<b>Receive Check</b>" onclick="lnkCheckReceive_Click" /></td>
            </tr>
            </table>        
        </div>    
    </asp:View>  
    <asp:View ID="viewPoints" runat="server">
        <div class="controlarea2 padded">
            
            <table border="0" cellspacing="0" cellpadding="3" width="100%">
            <tr>
                <td width="50%"><h4>Points On Holds</h4>
                <table>
                <tr>
                    <td class="formlabel">Hold:</td>
                    <td class="formfield"><asp:DropDownList ID="lstPointsHeld" runat="server"></asp:DropDownList></td>                    
                </tr>
                <tr>
                    <td class="formlabel">Amount:</td>
                    <td class="formfield"><asp:TextBox ID="PointsHeldAmount" runat="server" Columns="10"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="formlabel"><asp:LinkButton ID="lnkPointsVoidAuth" runat="server" 
                            CssClass="btn" Text="<b>Void Hold</b>" onclick="lnkPointsVoidAuth_Click" /></td>
                    <td class="formfield"><asp:LinkButton ID="lnkPointsCaptureAuth" runat="server" 
                            CssClass="btn" Text="<b>Capture Hold</b>" 
                            onclick="lnkPointsCaptureAuth_Click" /></td>
                </tr>
                </table><br />
                &nbsp;
                </td>
                <td><h4>Pay with Points</h4>                
                <table>
                <tr>
                    <td class="formlabel">Points Available:</td>
                    <td class="formfield"><asp:Label ID="lblPointsAvailable" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="formlabel">Amount:</td>
                    <td class="formfield"><asp:TextBox ID="PointsNewAmountField" runat="server" Columns="10"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="formlabel"><asp:LinkButton ID="lnkPointsNewAuth" runat="server" 
                            CssClass="btn" Text="<b>Hold Points</b>" onclick="lnkPointsNewAuth_Click" /></td>
                    <td class="formfield"><asp:LinkButton ID="lnkPointsNewCharge" runat="server" 
                            CssClass="btn" Text="<b>Pay With Points</b>" onclick="lnkPointsNewCharge_Click" /></td>
                </tr>
                </table><br />
                &nbsp;
                </td>
            </tr>
            <tr>
                <td width="50%"><h4>Refund to Points</h4>                    
                    <table>
                    <tr>
                        <td class="formlabel">Charge:</td>
                        <td class="formfield"><asp:DropDownList ID="lstPointsRefundable" runat="server"></asp:DropDownList></td>                    
                    </tr>
                    <tr>
                        <td class="formlabel">Amount:</td>
                        <td class="formfield"><asp:TextBox ID="PointsRefundAmount" runat="server" Columns="10"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="formlabel">&nbsp;</td>
                        <td class="formfield"><asp:LinkButton ID="lnkPointsRefund" runat="server" 
                                CssClass="btn" Text="<b>Refund To Points</b>" 
                                onclick="lnkPointsRefund_Click" /></td>
                    </tr>
                    </table><br />
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            </table>
        </div>    
    </asp:View>  
</asp:MultiView>
