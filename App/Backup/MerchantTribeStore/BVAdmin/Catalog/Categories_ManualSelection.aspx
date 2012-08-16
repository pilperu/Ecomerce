<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Catalog_Categories_ManualSelection"
    Title="Category Selection" Codebehind="Categories_ManualSelection.aspx.cs" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>
<%@ Register Src="../Controls/ProductPicker.ascx" TagName="ProductPicker" TagPrefix="uc1" %>


<asp:Content ContentPlaceHolderID="headcontent" runat="server" ID="headcss">   
    <script type="text/javascript">

         function RemoveProduct(lnk) {

             var id = $(lnk).attr('id');
             id = id.replace('rem', '');
             var categoryid = '<%=CategoryBvin %>';

             $.post('Categories_RemoveProduct.aspx',
                   { "id": id,
                     "categoryid": categoryid
                   },
                   function () {
                       lnk.parent().parent().parent().parent().parent().slideUp('slow', function () {
                           lnk.parent().parent().parent().parent().parent().remove();
                       });
                   }
                  );                                                               
         }
         
     </script>
    <script type="text/javascript">                                         
        // Jquery Setup
        $(document).ready(function () {
            $(".selected-products").sortable({
                placeholder: 'ui-state-highlight',
                axis: 'y',
                opacity: '0.75',
                cursor: 'move',
                update: function (event, ui) {
                    //alert('Sending Sort:' + $(this).sortable('toArray'));
                    var sorted = $(this).sortable('toArray');
                    sorted += '';
                    $.post('Categories_SortProducts.aspx',
                                                { "ids": sorted,
                                                  "categoryid": "<%=CategoryBvin %>"
                                                }
                                                );
                }
            });
            $(".selected-products").disableSelection();

            $('.trash').click(function () {
                RemoveProduct($(this));
                return false;
            });

        });
    </script>
</asp:Content>
    
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>
        Select Products for Category</h1>
    <uc2:MessageBox ID="msg" runat="server" />
    <table border="0" cellspacing="0" cellpadding="0" width="900">
        <tr>
            <td class="formfield">
                <h2>
                    Selected Products
                </h2>
                
                <div class="selected-products">
                    <asp:Literal ID="litProducts" runat="server"></asp:Literal>
                </div>
                
                <asp:hyperlink class="actionlink" ID="lnkBack" runat="server">&laquo; Return to Category</asp:hyperlink>                                
                                                
            </td>
            <td style="width: 50px;">
                &nbsp;</td>
            <td class="formfield" style="width: 350px;">
                <h2>
                    Pick Products To Add</h2>
                <uc1:ProductPicker ID="ProductPicker1" runat="server" DisplayKits="true" />
                <br />
                <asp:ImageButton ID="btnAdd" runat="server" 
                    ImageUrl="~/BVAdmin/Images/Buttons/Add.png" onclick="btnAdd_Click" /></td>
        </tr>
    </table>
    <asp:HiddenField ID="BvinField" runat="server" />
</asp:Content>
