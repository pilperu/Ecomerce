<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminPopup.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Orders_PrintOrder" Title="Print Order" Codebehind="PrintOrder.aspx.cs" %>

<%@ Register src="OrderActions.ascx" tagname="OrderActions" tagprefix="uc1" %>
<asp:Content ContentPlaceHolderID="headcontent" runat="server">

<script type="text/javascript">
    function doPrint() {
        if (window.print) {
            window.print();
        } else {
            alert('Please choose the print button from your browser.  Usually in the menu dropdowns at File: Print');
        }
    }

    $(document).ready(function () {
        if ($("#autoprint").html() == "1") {
            $("#autoprint").html('0');
            doPrint();
        }        
    });

</script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="BvcAdminPopupConent" runat="Server">
<div style="background:#fff;padding:10px;">
    <div class="printhidden">    
    <asp:Image runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Print.png" AlternateText="Print" onclick="javascript:doPrint();" /> 
     <asp:ImageButton ID="btnContinue" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/Ok.png" onclick="btnContinue_Click" />
    <table border="0" cellspacing="0" cellpadding="3" width="100%">
        <tr>
            <td width="66%" class="formfield">
                <h1>
                    Print Order</h1>
                    Template: <asp:DropDownList ID="TemplateField" runat="Server"></asp:DropDownList> 
                <asp:ImageButton ID="btnGenerate" runat="server" 
                    ImageUrl="~/BVAdmin/Images/Buttons/Go.png" onclick="btnGenerate_Click" />
            </td>
            <td class="formlabel">            
                <uc1:OrderActions ID="OrderActions1" runat="server" />                
            </td>
        </tr>    
    </table>
    </div>
    <div class="printbackground">
    <asp:DataList BackColor="#FFFFFF" ID="DataList1" runat="server" Width="100%" 
            onitemdatabound="DataList1_ItemDataBound">
        <ItemTemplate>
            <div class="printhidden">
                <hr/>
            </div>
            <asp:literal ID="litTemplate" runat="server"></asp:literal>            
        </ItemTemplate>
        <SeparatorTemplate>
            <div style="page-break-after: always;">&nbsp;</div>
        </SeparatorTemplate>
    </asp:DataList>
    </div>
    <div class="printhidden">
    <asp:ImageButton ID="btnContinue2" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/Ok.png" onclick="btnContinue2_Click" />
    </div>    

    <div class="clear"></div>
</div>    
<div id="autoprint"><asp:Literal ID="litAutoPrint" runat="server" Text="0"></asp:Literal></div>
</asp:Content>
