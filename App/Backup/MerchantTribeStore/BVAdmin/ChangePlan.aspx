<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_ChangePlan" Codebehind="ChangePlan.aspx.cs" %>

<%@ Register src="Controls/MessageBox.ascx" tagname="MessageBox" tagprefix="uc1" %>
<%@ Register src="../BVModules/Controls/CreditCardInput.ascx" tagname="CreditCardInput" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" Runat="Server">
<style>
    /* Price Table */
.priceplans {margin:10px 10px;}
.priceplans td {padding:3px 0px;color:#666;vertical-align:middle;text-align:center;
                border-bottom:solid 1px #fff;border-right:solid 1px #fff;width:100px;}
.priceplans .pnone, 
.priceplans .desc {background-color:#fff;border:0;color:#666;text-align:right;}
.priceplans .row td.p1, .priceplans .row td.p5 {background-color:#e6e6f5;}
.priceplans .rowalt td.p1, .priceplans .rowalt td.p5 {background-color:#f0f0f6;}
.priceplans .row td.p2, .priceplans .row td.p4 {background-color:#efefff;}
.priceplans .rowalt td.p2, .priceplans .rowalt td.p4 {background-color:#f8f8ff;}
.priceplans .row td.p3 {background-color:#fdf9b8;}
.priceplans .rowalt td.p3 {background-color:#fdfce3;}
.priceplans tr.planname td {width:100px;padding:0;font-weight:bold;font-size:16px;}

</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div style="width:700px;margin:10px auto;">
        <h1>Change Store Plan</h1>
        <uc1:MessageBox ID="MessageBox1" runat="server" />
        
        <asp:Panel ID="pnlCreditCard" runat="server">
            <div class="editorpanel">
                <h3>Credit Card Information</h3>
                <p>You do not currently have a credit card on file. A valid credit card is required to switch to a non-free plan.</p>
                &nbsp;<br />
                <uc1:CreditCardInput ID="CreditCardInput1" runat="server" /><br />
                Billing Zip Code: <asp:textbox ID="txtZipCode" runat="server" Columns="10"></asp:textbox><br />&nbsp;<br />
            </div>
            &nbsp;
        </asp:Panel>
        
        <h2>Select Your New Plan</h2>
        <table class="priceplans">           
            <tr class="planname">
                <td class="pnone">&nbsp;</td>
                <td>Trial</td>
                <td>Basic</td>
                <td>Plus</td>
                <td>Premium</td>
                <td>Max</td>
            </tr>            
            <tr class="rowalt">
                <td class="desc">Sales Cap:</td>
                <td class="p1">$1000/mo</td>
                <td class="p2">Unlimited</td>
                <td class="p3">Unlimited</td>
                <td class="p4">Unlimited</td>
                <td class="p5">Unlimited</td>
            </tr>
            <tr class="row">
                <td class="desc">Max Products:</td>
                <td class="p1">10</td>
                <td class="p2">100</td>
                <td class="p3">5,000</td>
                <td class="p4">10,000</td>
                <td class="p5">50,000</td>
            </tr>
            <tr class="rowalt">
                <td class="desc">Transfer:</td>
                <td class="p1">1GB</td>
                <td class="p2">10GB</td>
                <td class="p3">100GB</td>
                <td class="p4">250GB</td>
                <td class="p5">1,000GB</td>
            </tr>
            <tr class="row">
                <td class="desc">Monthly:</td>
                <td class="p1">$0</td>
                <td class="p2">$49</td>
                <td class="p3">$99</td>
                <td class="p4">$199</td>
                <td class="p5">$499</td>
            </tr>                              
            <tr class="rowalt">
                <td class="desc">PayPal:</td>
                <td class="p1">Yes</td>
                <td class="p2">Yes</td>
                <td class="p3">Yes</td>
                <td class="p4">Yes</td>
                <td class="p5">Yes</td>
            </tr>
            <tr id="trCC" class="row" runat="server">
                <td class="desc">Credit Cards:</td>
                <td class="p1">&nbsp;</td>
                <td class="p2">Yes</td>
                <td class="p3">Yes</td>
                <td class="p4">Yes</td>
                <td class="p5">Yes</td>
            </tr>
            <tr id="trPO" class="rowalt" runat="server">
                <td class="desc">Purchase Orders:</td>
                <td class="p1">&nbsp;</td>
                <td class="p2">&nbsp;</td>
                <td class="p3">Yes</td>
                <td class="p4">Yes</td>
                <td class="p5">Yes</td>
            </tr>
            <tr id="trCOD" class="row" runat="server">
                <td class="desc">COD:</td>
                <td class="p1">&nbsp;</td>
                <td class="p2">&nbsp;</td>
                <td class="p3">Yes</td>
                <td class="p4">Yes</td>
                <td class="p5">Yes</td>
            </tr>
            <tr class="planchoose">
                <td class="pnone">&nbsp;</td>
                <td><asp:LinkButton ID="btnFree" runat="server" CssClass="btn" Text="<b>Switch</b>" 
                        onclick="btnFree_Click"></asp:LinkButton></td>
                <td><asp:LinkButton ID="btnBasic" runat="server" CssClass="btn" 
                        Text="<b>Switch</b>" onclick="btnBasic_Click"></asp:LinkButton></td>
                <td><asp:LinkButton ID="btnPlus" runat="server" CssClass="btn" Text="<b>Switch</b>" 
                        onclick="btnPlus_Click"></asp:LinkButton></td>
                <td><asp:LinkButton ID="btnPremium" runat="server" CssClass="btn" 
                        Text="<b>Switch</b>" onclick="btnPremium_Click"></asp:LinkButton></td>
                <td><asp:LinkButton ID="btnMax" runat="server" CssClass="btn" Text="<b>Switch</b>" 
                        onclick="btnMax_Click"></asp:LinkButton></td>
            </tr>
            </table>
            &nbsp;          
            <div class="editorpanel">
            <p><b>Upgrading:</b><br /></p>
                Your store will immediately move to the new rate and your card will be charged a 
                pro-rated amount for the current month under your old plan and the remainder of 
                the month under the new plan. The next billing cycle your card will be charged 
                the full new plan rate.<br />
            &nbsp;<br />
            <p><b>Downgrading</b><br />Downgrading will change your store functionality immediately to the new features. You will be billed at the lower rate on your next regular bill. Free plans have no charge as always.</p>
            </div>
    </div>


            
</asp:Content>

