using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore
{

    partial class BVModules_Controls_MessageBox : MerchantTribe.Commerce.Content.BVUserControl, IMessageBox
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
            this.pnlMain.Visible = true;
            HtmlGenericControl li = new HtmlGenericControl("li");
            li.Attributes.Add("class", "errorline");
            HtmlGenericControl icondiv = new HtmlGenericControl("div");
            li.Controls.Add(icondiv);
            icondiv.Attributes.Add("class", "icon");
            Image img = new Image();
            img.ImageUrl = GetIconType(msgType);
            icondiv.Controls.Add(img);

            HtmlGenericControl divMessage = new HtmlGenericControl("div");
            divMessage.Attributes.Add("class", "message");

            Label MessageLabel = new Label();
            MessageLabel.Text = msg;
            divMessage.Controls.Add(MessageLabel);

            li.Controls.Add(divMessage);
            this.MessageList.Controls.Add(li);
        }

        public void ClearMessage()
        {

        }

        public string GetIconType(DisplayMessageType msgType)
        {
            switch (msgType)
            {
                case MerchantTribe.Commerce.Content.DisplayMessageType.Error:
                    return MyPage.MTApp.ThemeManager().ButtonUrl("messageerror", Request.IsSecureConnection);
                case MerchantTribe.Commerce.Content.DisplayMessageType.Exception:
                    return MyPage.MTApp.ThemeManager().ButtonUrl("messageexception", Request.IsSecureConnection);
                case MerchantTribe.Commerce.Content.DisplayMessageType.Information:
                    return MyPage.MTApp.ThemeManager().ButtonUrl("messageinformation", Request.IsSecureConnection);
                case MerchantTribe.Commerce.Content.DisplayMessageType.Question:
                    return MyPage.MTApp.ThemeManager().ButtonUrl("messagequestion", Request.IsSecureConnection);
                case MerchantTribe.Commerce.Content.DisplayMessageType.Success:
                    return MyPage.MTApp.ThemeManager().ButtonUrl("messageok", Request.IsSecureConnection);
                case MerchantTribe.Commerce.Content.DisplayMessageType.Warning:
                    return MyPage.MTApp.ThemeManager().ButtonUrl("messagewarning", Request.IsSecureConnection);
            }
            return string.Empty;
        }

    }
}