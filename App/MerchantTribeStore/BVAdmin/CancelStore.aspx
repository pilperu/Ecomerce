<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_CancelStore" Codebehind="CancelStore.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<div style="width:700px;margin:10px auto;">
    <h1>
                Cancel Store</h1>
            <div class="flash-message-error">
                Are you sure you want to cancel this store?</div>
            <div class="editorpanel">
                <h3>
                    You are cancelling: <strong>
                        <asp:Label ID="lblStoreName" runat="server"></asp:Label></strong></h3>
                &nbsp;<br />
                <p>
                    * Your store will be shut down <strong>immediately</strong>.
                </p>
                <p>
                    * You will lose all of your store data including your <strong>order history and product
                        catalog</strong>
                </p>
                <p>
                    * You will <strong>Not</strong> be able to create a store with this name again.
                </p>
                &nbsp;<br />
                <p class="tiny">
                    BV Software is not responsible for any loss of data or services if you cancel this
                    store.
                </p>
                </div>
                &nbsp;
                 <div class="editorpanel">
                <table border="0" cellspacing="0" cellpadding="0" width="100%">
                    <tr>
                        <td class="l">
                            <a href="account.aspx" class="btn"><b>Keep My Store</b></a>
                        </td>
                        <td class="r">                            
                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn" 
                                Text="<b>Delete This Store Forever</b>" onclick="btnDelete_Click"></asp:LinkButton>                            
                        </td>
                    </tr>
                </table>                
            </div>
</div>
</asp:Content>

