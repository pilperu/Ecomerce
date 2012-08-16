using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_MessageBox : MerchantTribe.Commerce.Content.BVUserControl, IMessageBox
    {

        public void ShowOk(string msg)
        {
            ShowMessage(msg, DisplayMessageType.Success);
        }

        public void ShowError(string msg)
        {
            ShowMessage(msg, DisplayMessageType.Error);
        }

        public void ShowInformation(string msg)
        {
            ShowMessage(msg, DisplayMessageType.Information);
        }

        public void ShowMinor(string msg)
        {
            ShowMessage(msg, DisplayMessageType.Minor);
        }

        public void ShowQuestion(string msg)
        {
            ShowMessage(msg, DisplayMessageType.Question);
        }

        public void ShowWarning(string msg)
        {
            ShowMessage(msg, DisplayMessageType.Warning);
        }

        public void ShowException(System.Exception ex)
        {
            string msg = ex.Message + "<br>" + ex.Source + "<br>" + ex.StackTrace;
            ShowMessage(msg, DisplayMessageType.Exception);
        }

        public void ShowMessage(string msg, MerchantTribe.Commerce.Content.DisplayMessageType msgType)
        {
            this.litMain.Text += "<div class=\"";
            switch (msgType)
            {
                case DisplayMessageType.Error:
                    this.litMain.Text += "flash-message-error";
                    break;
                case DisplayMessageType.Exception:
                    this.litMain.Text += "flash-message-exception";
                    break;
                case DisplayMessageType.Information:
                    this.litMain.Text += "flash-message-info";
                    break;
                case DisplayMessageType.Question:
                    this.litMain.Text += "flash-message-question";
                    break;
                case DisplayMessageType.Success:
                    this.litMain.Text += "flash-message-success";
                    break;
                case DisplayMessageType.Warning:
                    this.litMain.Text += "flash-message-warning";
                    break;
                case DisplayMessageType.Minor:
                    this.litMain.Text += "flash-message-minor";
                    break;
            }

            this.litMain.Text += "\">" + msg + "</div>";
        }

        public void ClearMessage()
        {
            this.litMain.Text = string.Empty;
        }

        public void RenderViolations(List<MerchantTribe.Web.Validation.RuleViolation> violations)
        {
            if (violations == null) return;

            foreach (MerchantTribe.Web.Validation.RuleViolation v in violations)
            {
                ShowWarning(v.ErrorMessage);
            }
        }
    }
}