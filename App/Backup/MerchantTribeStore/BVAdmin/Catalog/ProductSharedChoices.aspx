<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Catalog_ProductSharedChoices"
    Title="Untitled Page" Codebehind="ProductSharedChoices.aspx.cs" %>       
<%@ Register src="../Controls/MessageBox.ascx" tagname="MessageBox" tagprefix="uc1" %>

<asp:Content ContentPlaceHolderID="headcontent" runat="server" ID="headcss">
    <script type="text/javascript">
         function Remove(lnk) {

             var id = $(lnk).attr('id');
             $.post('ProductSharedChoices_Delete.aspx',
                                                { "id": id.replace('rem', '') },
                                                 function () {
                                                     lnk.parent().parent().slideUp('slow');
                                                     lnk.parent().parent().remove();
                                                 }
                                                );                                                               
         }
     </script>
    <script type="text/javascript">
        // Jquery Setup
        $(document).ready(function () {
            $('.trash').click(function () {
                if (window.confirm('Deleting this shared choice will affect ALL products that are \nassociated with this shared choice and will result in loss of inventory for \nthose products. Are you sure you want to continue?')) {
                    Remove($(this));
                }
                return false;
            });
        });
    </script>
</asp:Content>


        
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <uc1:MessageBox ID="MessageBox1" runat="server" />
    <h1>
        Shared Choices</h1>
    <table class="formtable controlarea1">
    <tr>
        <td><asp:DropDownList ID="SharedChoiceTypes" runat="server">
        <asp:ListItem Value="100">Drop Down List</asp:ListItem>
        <asp:ListItem Value="200">Radio Button List</asp:ListItem>
        <asp:ListItem Value="300">Checkboxes</asp:ListItem>
        <asp:ListItem Value="400">Html Description</asp:ListItem>
        <asp:ListItem Value="500">Text Input</asp:ListItem>        
    </asp:DropDownList></td>
    <td><asp:ImageButton ID="NewSharedChoiceImageButton" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/New.png" 
            onclick="NewSharedChoiceImageButton_Click" /></td>
    </tr>
    </table>
    &nbsp;<br />
     <asp:Literal id="litResults" ClientIDMode="Static" runat="server" EnableViewState="false"></asp:Literal>                    
</asp:Content>
