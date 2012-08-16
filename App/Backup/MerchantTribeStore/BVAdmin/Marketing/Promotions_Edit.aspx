<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="true" CodeBehind="Promotions_Edit.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.Marketing.Promotions_Edit" %>
<%@ Register src="../Controls/MessageBox.ascx" tagname="MessageBox" tagprefix="uc1" %>
<%@ Register src="../Controls/DropDownDate.ascx" tagname="DropDownDate" tagprefix="uc2" %>
<%@ Register src="Promotions_Edit_Qualification.ascx" tagname="Promotions_Edit_Qualification" tagprefix="uc3" %>
<%@ Register src="Promotions_Edit_Actions.ascx" tagname="Promotions_Edit_Actions" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" runat="server">
<script type="text/javascript">

    $(document).ready(function () {

        $('.editrequester').click(function () {
            var id = $(this).attr('id');
            $('#currenteditid').val(id);
            $('#btnTest').click();
            return false;
        });

        $('.deleterequester').click(function () {
            var id = $(this).attr('id');
            if (confirm('Delete this item?'))
            {
                $('#currentdeleteid').val(id);
                $('#btnDelete').click();
            }
            return false;
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<uc1:MessageBox ID="MessageBox1" runat="server" />
<h1>Edit Promotion</h1>    
<table>
<tr>
    <td class="formlabel">Enabled:</td>
    <td class="formfield"><asp:CheckBox ID="chkEnabled" runat="server" /></td>
</tr>
<tr>
    <td class="formlabel">Start Date:</td>
    <td class="formfield"><uc2:DropDownDate ID="DateStartField" runat="server" /></td>
</tr>
<tr>
    <td class="formlabel">End Date:</td>
    <td class="formfield"><uc2:DropDownDate ID="DateEndField" runat="server" /></td>
</tr>
<tr>
    <td class="formlabel">Name:</td>
    <td class="formfield"><asp:TextBox ID="NameField" runat="server" Width="500px"></asp:TextBox></td>
</tr>
<tr>
    <td class="formlabel">Customer Description:</td>
    <td class="formfield"><asp:TextBox ID="CustomerDescriptionField" runat="server" Width="500px"></asp:TextBox></td>
</tr>
<tr><td colspan="2">&nbsp;</td></tr>
<tr>
    <td class="formlabel"><asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="Promotions.aspx">&laquo; Back to Promotions</asp:HyperLink></td>
    <td class="formfield"><asp:ImageButton ID="btnSave" runat="server" runat="server" 
            ImageUrl="~/bvadmin/images/buttons/SaveChanges.png" 
            AlternateText="Save and Close" onclick="btnSave_Click" /></td>
</tr>
</table>
&nbsp;<br />
&nbsp;<br />
<table width="100%">
<tr>
    <td width="45%"><h2>Qualifications</h2>
    <asp:Literal ID="litQualifications" runat="server"></asp:Literal><br />
    &nbsp;<br />    
    <table>
    <tr>
        <td><asp:DropDownList ID="lstNewQualification" runat="server"></asp:DropDownList></td>
        <td><asp:ImageButton ID="btnNewQualification" runat="server" 
                ImageUrl="~/bvadmin/images/buttons/SmallPlus.png" 
                onclick="btnNewQualification_Click" /></td>
    </tr>
    </table>    
    </td>
    <td>&nbsp;</td>
    <td width="45%"><h2>Actions</h2>
    <asp:Literal ID="litActions" runat="server"></asp:Literal><br />
    &nbsp;<br />    
    <table>
    <tr>
        <td><asp:DropDownList ID="lstNewAction" runat="server"></asp:DropDownList></td>
        <td><asp:ImageButton ID="btnNewAction" runat="server" 
                ImageUrl="~/bvadmin/images/buttons/SmallPlus.png" 
                onclick="btnNewAction_Click" /></td>
    </tr>
    </table>        
    </td>
</tr>
</table>

<asp:Panel ID="pnlEditQualification" runat="server" Visible="false">
    <div class="modaldialog curved">
        <div class="padded">
            <uc3:Promotions_Edit_Qualification ID="Promotions_Edit_Qualification1" 
                runat="server" /><br />
                &nbsp;<br />           
            <asp:LinkButton id="btnCloseQualificationEditor" runat="server" 
                Text="&laquo; Close" onclick="btnCloseQualificationEditor_Click"></asp:LinkButton>&nbsp;&nbsp;&nbsp;
            <asp:ImageButton ID="btnSaveQualifications" runat="server" 
                ImageUrl="~/bvadmin/images/buttons/SaveChanges.png" 
                onclick="btnSaveQualifications_Click1" />                
        </div>  
    </div>    
    <div class="overlay" style="display:block;"></div>    
</asp:Panel>
<asp:Panel ID="pnlEditAction" runat="server" Visible="false">
    <div class="modaldialog curved">
        <div class="padded">
            <uc4:Promotions_Edit_Actions ID="Promotions_Edit_Actions1" runat="server" /><br />
                &nbsp;<br />           
            <asp:LinkButton id="btnCloseActionEditor" runat="server" Text="&laquo; Close" 
                onclick="btnCloseActionEditor_Click"></asp:LinkButton>&nbsp;&nbsp;&nbsp;
            <asp:ImageButton ID="btnSaveActions" runat="server" 
                ImageUrl="~/bvadmin/images/buttons/SaveChanges.png" 
                onclick="btnSaveActions_Click1" />                    
        </div>  
    </div>    
    <div class="overlay" style="display:block;"></div>    
</asp:Panel>
<div style="display:none">
    <asp:Button ID="btnTest" ClientIDMode="Static" runat="server" Text="Test" onclick="btnTest_Click" />
    <asp:HiddenField ID="currenteditid" runat="server" ClientIDMode="Static" Value="" />    
    <asp:Button ID="btnDelete" ClientIDMode="Static" runat="server" Text="Test" onclick="btnDelete_Click" />
    <asp:HiddenField ID="currentdeleteid" runat="server" ClientIDMode="Static" Value="" />    
</div>
</asp:Content>
