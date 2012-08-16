<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Orders_OrderActions" Codebehind="OrderActions.ascx.cs" %>
<asp:HyperLink ID="lnkDetails" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/OrderDetails.png"></asp:HyperLink><br />
<asp:HyperLink ID="lnkEditOrder" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/EditOrder.png"></asp:HyperLink><br />
<asp:HyperLink ID="lnkPayment" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Payment.png"></asp:HyperLink><br />
<asp:HyperLink ID="lnkShipping" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Shipping.png"></asp:HyperLink><br />
<asp:Image ID="imgPrintNow" runat="server" ImageUrl="~/bvadmin/images/buttons/PrintSplit.png" onclick="javascript:doPrint();" /><asp:HyperLink ID="lnkPrint" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/OptionsSplit.png"></asp:HyperLink><br />
<asp:HyperLink ID="lnkManager" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Close.png"></asp:HyperLink><br />
