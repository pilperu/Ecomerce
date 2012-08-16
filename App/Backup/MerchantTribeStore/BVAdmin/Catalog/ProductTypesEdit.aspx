<%@ Page MasterPageFile="~/BVAdmin/BVAdmin.master"  ValidateRequest="False" Language="C#"
    AutoEventWireup="True" Inherits="MerchantTribeStore.Product_ProductTypes_Edit" Codebehind="ProductTypesEdit.aspx.cs" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="headcontent" runat="server" ID="headcss">   
    <script type="text/javascript">

        function RemoveProduct(lnk) {

            var id = $(lnk).attr('id');
            id = id.replace('rem', '');
            var typeid = '<%=TypeId %>';

            $.post('ProductTypes_RemoveProperty.aspx',
                   { "id": id,
                       "typeid": typeid
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
                    $.post('ProductTypes_SortProperties.aspx',
                                                { "ids": sorted,
                                                    "typeid": "<%=TypeId %>"
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

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Edit Product Type</h1>
    <uc1:MessageBox ID="msg" runat="server" />
    <table class="FormTable">
        <tr>
            <td class="FormLabel" align="right">
                Product Type&nbsp;Name&nbsp;
            </td>
            <td class="FormLabel" align="left">
                <asp:TextBox ID="ProductTypeNameField" runat="server" CssClass="FormInput" Columns="40"></asp:TextBox><asp:RequiredFieldValidator
                    ID="RequiredFieldValidator1" runat="server" ErrorMessage="Product Type Name is Required"
                    ControlToValidate="ProductTypeNameField">*</asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td class="FormLabel" align="right">
                <asp:LinkButton ID="lnkClose" runat="server" onclick="lnkClose_Click">
                <asp:Image ID="imgCLose" runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Cancel.png" />
                </asp:LinkButton>
                </td>
            <td class="FormLabel" align="left">
                <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save Changes" 
                    ImageUrl="~/BVAdmin/images/buttons/SaveChanges.png" onclick="btnSave_Click">
                </asp:ImageButton></td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td class="FormLabel" colspan="2">
                <table cellspacing="0" cellpadding="3" border="0" width="900">
                    <tr>                        
                        <td width="50%" class="FormLabel" valign="top" align="left">
                            Selected&nbsp;Properties<br />
                             <div class="selected-products">
                                <asp:Literal ID="litProducts" runat="server"></asp:Literal>
                            </div>
                            </td>
                        <td class="FormLabel" valign="middle" align="center">
                            &nbsp;<br />
                            &nbsp;<br />
                            &nbsp;<br />
                            <asp:ImageButton ID="btnAddProperty" runat="server" 
                                ImageUrl="~/BVAdmin/images/buttons/Add.png" onclick="btnAddProperty_Click">
                            </asp:ImageButton><br />
                            <br />
                            </td>
                        <td width="30%" class="FormLabel" valign="top" align="left">
                            Available Properties<br />
                            <asp:ListBox ID="lstAvailableProperties" runat="server" Rows="10" SelectionMode="Multiple">
                            </asp:ListBox></td>
                    </tr>
                </table>
            </td>
        </tr>        
    </table><asp:HiddenField ID="BvinField" runat="server" />
</asp:Content>
