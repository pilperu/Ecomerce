<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="true" CodeBehind="RewardsPoints.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.Marketing.RewardsPoints" %>
<%@ Register src="../Controls/MessageBox.ascx" tagname="MessageBox" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" runat="server">
<script type="text/javascript">

    function CalculateRatio() {
        var issued = $('#PointsPerDollarField').val();
        var redeem = $('#PointsCreditField').val();
        var ratio = issued / redeem;
        var ratiop = ratio * 100;
        $('#ratio').html(ratiop + '%');
    }

    $(document).ready(function () {
        $('#PointsCreditField').change(function () { CalculateRatio(); });
        $('#PointsPerDollarField').change(function () { CalculateRatio(); });
        CalculateRatio();
    });

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1>Rewards Points</h1>
    <uc1:MessageBox ID="MessageBox1" runat="server" />
<table class="formtable">
<tr>
    <td class="formlabel">Unused Points Issued to Customers:</td>
    <td class="formfield"><asp:Label ID="lblPointsIssued" runat="server"></asp:Label></td>
    <td class="formlabel"><asp:Label ID="lblPointsIssuedValue" runat="server"></asp:Label></td>
</tr>
<tr>
    <td class="formlabel">Points Reserved for Orders:</td>
    <td class="formfield"><asp:Label ID="lblPointsReserved" runat="server"></asp:Label></td>
    <td class="formlabel"><asp:Label ID="lblPointsReservedValue" runat="server"></asp:Label></td>
</tr>
<tr><td colspan="3">&nbsp;</td></tr>
<!--<tr>
    <td class="formlabel">Issue Points for Products:</td>
    <td class="formfield" colspan="2"><asp:CheckBox ID="chkPointsForProducts" runat="server" /></td>
</tr>-->
<tr>
    <td class="formlabel">&nbsp;</td>
    <td class="formfield" colspan="2"><asp:CheckBox ID="chkPointForDollars" runat="server" /> Enable Rewards Points</td>
</tr>
<tr>
    <td class="formlabel">Rewards Points Name:</td>
    <td class="formfield" colspan="2"><asp:TextBox ID="RewardsNameField" runat="server" Columns="50"></asp:TextBox></td>
</tr>
<tr>
    <td class="formlabel">Issue</td>
    <td class="formfield" colspan="2"><asp:TextBox ID="PointsPerDollarField" ClientIDMode="Static" runat="server" Columns="10"></asp:TextBox> points for each <%=String.Format("{0:c}",1) %> spent.</td>
</tr>
<tr>
    <td class="formlabel">Redeem</td>
    <td class="formfield" colspan="2"><asp:TextBox ID="PointsCreditField" ClientIDMode="Static" runat="server" Columns="10"></asp:TextBox> points for <%=String.Format("{0:c}",1) %> of credit</td>
</tr>
<tr>
    <td class="formlabel">Reward Ratio:</td>
    <td class="formfield" colspan="2"><div id="ratio"></div></td>
</tr>
<tr>
    <td class="formlabel">&nbsp;</td>
    <td class="formfield" colspan="2"><asp:ImageButton ID="btnSave" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" onclick="btnSave_Click" /></td>
</tr>
</table>
</asp:Content>
