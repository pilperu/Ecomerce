<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_ContentBlocks_Product_Grid_adminview" Codebehind="adminview.ascx.cs" %>
<div class="decoratedblock">
    <h4>
        Product Grid</h4>
    <div class="blockcontent">
        <asp:DataList ID="DataList1" runat="server" RepeatDirection="Horizontal" DataKeyField="Id"
            RepeatLayout="Table">
            <ItemTemplate>
                <table>
                    <tr>
                        <td>
                            <a href="<%#Eval("Setting5") %>">
                                <img src="../../<%#Eval("Setting4") %>" border="none" /></a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a href="<%#Eval("Setting5") %>">
                                <%#Eval("Setting3") %>
                            </a>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:DataList>
    </div>
</div>
