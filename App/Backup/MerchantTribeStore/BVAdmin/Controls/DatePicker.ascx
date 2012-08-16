<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Controls_DatePicker" Codebehind="DatePicker.ascx.cs" %>
<asp:TextBox ID="DateTextBox" runat="server"></asp:TextBox>
<asp:CustomValidator ID="DateCustomValidator" runat="server" 
    ControlToValidate="DateTextBox" CssClass="errormessage" ForeColor=" " 
    onservervalidate="DateCustomValidator_ServerValidate">*</asp:CustomValidator>
<asp:RequiredFieldValidator ID="DateRequiredValidator" runat="server" ControlToValidate="DateTextBox" CssClass="errormessage" ForeColor=" ">*</asp:RequiredFieldValidator>
<asp:ImageButton ID="CalendarShowImageButton" runat="server" 
    ImageUrl="~/BVAdmin/Images/Buttons/Calendar.png" CausesValidation="False" 
    onclick="CalendarShowImageButton_Click" />
<div style="position: relative;"><asp:Calendar ID="Calendar" runat="server" 
        Visible="False" style="position: absolute; top: 10px; left: 10px;" 
        BackColor="White" onselectionchanged="Calendar_SelectionChanged"></asp:Calendar></div>