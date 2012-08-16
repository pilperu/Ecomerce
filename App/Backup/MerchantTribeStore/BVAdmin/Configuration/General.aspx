<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Configuration_General" title="Untitled Page" Codebehind="General.aspx.cs" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<asp:Content ID="headcontent" ContentPlaceHolderID="headcontent" runat="server">
<script type="text/javascript">

    function CheckChanged() {
        var chk = $('#uselogoimage').attr('checked');

        if (chk) {
            $('.logo-text-controls').hide();
            $('.logo-image-controls').show();
        }
        else {
            $('.logo-text-controls').show();
            $('.logo-image-controls').hide();
        }
        return true;
    }


    function Remove301(lnk) {
        var id = lnk.attr('id');
        var idr = id.replace('remove', '');
        $.post('GeneralRemoveDomain.aspx',
            { "id": idr },
            function () {
                lnk.parent().slideUp('slow', function () { lnk.parent().remove(); });
            }
            );
        }
       
</script>

<script type="text/javascript">
    // Jquery Setup
    $(document).ready(function () {

        $('#uselogoimage').click(function () {
            CheckChanged();
        });

        CheckChanged();

        $('.remove301').click(function () {
            Remove301($(this));
            return false;
        });


    });                      // End Doc Ready
    </script>

</asp:Content>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

        <h1>Store Name &amp; Logo Settings</h1>
       
        <uc1:MessageBox ID="MessageBox1" runat="server" />
        <asp:Label id="lblError" runat="server" CssClass="errormessage"></asp:Label>
        <asp:Panel ID="pnlMain" runat="server" DefaultButton="btnSave">
       
        
        <div class="editorpanel">
            <table border="0" cellspacing="0" cellpadding="3" class="formtable">
                <tr>
                    <td class="formlabel">&nbsp;</td>
                    <td class="formfield">
                        <asp:CheckBox ID="chkClosed" runat="server" /> Close Store Temporarily</td>
                </tr>
                <tr>
                    <td class="formlabel">Closed Message</td>
                    <td class="formfield">
                        <asp:TextBox ID="ClosedMessageField" runat="server" Width="300px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="formlabel">Guest Password</td>
                    <td class="formfield">
                        <asp:TextBox ID="GuestPassword" runat="server" Width="300px"></asp:TextBox> (allows guests to view closed store)</td>
                </tr>

                <tr><td colspan="2">&nbsp;</td></tr>
                <tr>
                    <td class="formlabel">&nbsp;</td>
                    <td class="formfield">
                        <asp:CheckBox ID="chkHideGettingStarted" runat="server" /> Hide Getting Started Help</td>
                </tr>
                <tr>
                    <td class="formlabel">Store Name:</td>
                    <td class="formfield">
                        <asp:TextBox ID="SiteNameField" Columns="50" Width="300px" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:CheckBox id="uselogoimage" runat="server" ClientIdMode="static" /> Use an Image for Store Logo</td>
                </tr>                
                <tr class="logo-text-controls" style="display:none;">
                    <td class="formlabel" class="vt">
                           Store Logo Text:
                     </td>
                     <td class="formfield">
                            <asp:TextBox id="LogoTextField" runat="server" Columns="50"></asp:TextBox>                            
                     </td>
                </tr>                                
                                <tr class="logo-image-controls" style="display:none;">
                                    <td class="formlabel" class="vt">
                                        Store Logo Image:
                                    </td>
                                    <td class="formfield">                                                                                                                                                                                                                                                
                                        <img id="imgstorelogo" src="<%=LogoImage%>"
                                            class="iconimage" alt="Store Logo" /><br />
                                       <asp:FileUpload id="imgupload" runat="server" clientidmode="static" Columns="40" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formlabel">
                                        Use SSL:
                                    </td>
                                    <td class="formfield">                                                                                                                                                                                                                                                
                                        <asp:CheckBox ID="chkUseSSL" Text="Use SSL (https://) security when needed" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formlabel">
                                        Force Domains:
                                    </td>
                                    <td class="formfield">                                                                                                                                                                                                                                                
                                        <asp:CheckBox ID="chkForceDomains" Text="When off you can use 'localhost' or other relative domains" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">&nbsp;</td>
                                </tr>
                                <tr>
                    <td class="formlabel" class="vt">&nbsp;</td>
                    <td class="formfield" class="vt">
                        <asp:ImageButton ID="btnSave" CausesValidation="true"
                            runat="server" ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" 
                            onclick="btnSave_Click"></asp:ImageButton></td>
                </tr>   
                </table>

<asp:Panel ID="pnlRequestForm" runat="server">
            <table border="0" cellspacing="0" cellpadding="3" class="formtable">
                <tr>
                    <td class="formfield" class="vt" colspan="2">
                            &nbsp;<br />
                            &nbsp;<br />
                           <h3>Custom Domain Name Setup:</h3>
                           * Custom Domains are included in paid plans, $99/year for free store plans
                           <ol style="margin:10px 0 20px 30px;">
                                <li>Fill out the form below</li>
                                <li>A BV Software Representative will contact you with instructions</li>
                                <li>It will take <b>24 to 48 hours</b> for us to configure everything.</li>
                           </ol>
                     </td>
                </tr>                                              
                <tr>
                    <td class="formlabel" class="vt">
                           Domain Name You'd Like to Use:
                     </td>
                     <td class="formfield">
                        <asp:TextBox id="RequestedDomain" clientidmode="static" runat="server" columns="50"></asp:TextBox>                     
                     </td>
                </tr>   
                <tr>
                    <td class="formlabel" class="vt">
                           Do you own this domain already?
                     </td>
                     <td class="formfield">
                        <asp:RadioButtonList ID="lstOwnAlready" runat="server">
                            <asp:ListItem Value="YES">Yes, I own the domain name</asp:ListItem>
                            <asp:ListItem Value="NO" Selected="True">No, please check to see if it is available for purchase</asp:ListItem>
                        </asp:RadioButtonList>                                     
                     </td>
                </tr>  
                <tr>
                    <td class="formlabel" class="vt">
                           Do you have an SSL Certificate?
                     </td>
                     <td class="formfield">
                        <asp:RadioButtonList ID="lstHaveSSL" runat="server">
                            <asp:ListItem Value="YES">Yes, I already have an SSL Certificate for this Domain</asp:ListItem>
                            <asp:ListItem Value="NO" Selected="True">I'm not sure. Take care of it for me please. ($99/year)</asp:ListItem>
                        </asp:RadioButtonList><br />
                     </td>
                </tr> 
                <tr>
                    <td class="formlabel" class="vt">
                           Contact Name:
                     </td>
                     <td class="formfield">
                        <asp:TextBox ID="ContactName" runat="server" Columns="50"></asp:TextBox>
                     </td>
                </tr>    
                <tr>
                    <td class="formlabel" class="vt">
                           Contact Email:
                     </td>
                     <td class="formfield">
                        <asp:TextBox ID="ContactEmail" runat="server" Columns="50"></asp:TextBox>
                     </td>
                </tr>    
                <tr>
                    <td class="formlabel" class="vt">
                           Contact Phone:
                     </td>
                     <td class="formfield">
                        <asp:TextBox ID="ContactPhone" runat="server" Columns="50"></asp:TextBox>
                     </td>
                </tr>    
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td class="formlabel" class="vt">&nbsp;</td>
                    <td class="formfield" class="vt">
                        <asp:ImageButton ID="btnSend" CausesValidation="false"
                            runat="server" ImageUrl="../images/buttons/Submit.png" onclick="btnSend_Click" 
                            ></asp:ImageButton></td>
                </tr>                                                                                                                  
            </table>
            </asp:Panel>

            </div>
            &nbsp;
            <div>
                <h3>Primary Custom Domain Name:</h3>
                This can break your store if you don't have DNS setup first!<br />
                <asp:TextBox ID="CustomDomainField" runat="server" Width="400px"></asp:TextBox>
                <asp:ImageButton ID="btnUpdateCustomDomain" CausesValidation="false"
                            runat="server" ImageUrl="~/BVAdmin/Images/Buttons/Update.png" onclick="btnUpdateCustomDomain_Click" 
                            ></asp:ImageButton>
            </div>
            &nbsp;
            <div>
                <h3>Alternate Custom Domain Names:</h3>
                <asp:Literal ID="litCustomDomains" runat="server" EnableViewState="false"></asp:Literal><br />
                <asp:TextBox ID="NewCustomDomain" runat=server Width="400px"></asp:TextBox> 
                <asp:LinkButton ID="lnkAddCustomDomain" runat="server"
                 Text="<b>Add Alternate Custom Domain</b>" CssClass="btn" 
                    onclick="lnkAddCustomDomain_Click"></asp:LinkButton>
            </div>
        </asp:Panel>
</asp:Content>

