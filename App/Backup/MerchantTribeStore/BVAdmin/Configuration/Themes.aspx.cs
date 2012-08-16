using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;
using MerchantTribe.Commerce.BusinessRules;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Contacts;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Metrics;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce.Taxes;
using MerchantTribe.Commerce.Utilities;

using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVAdmin_Configuration_Themes : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Themes";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            LoadThemes();
        }

        private void LoadThemes()
        {

            ThemeManager tm = MTApp.ThemeManager();

            List<ThemeView> installed = tm.FindInstalledThemes();
            List<ThemeView> available = tm.FindAvailableThemes();

            this.litInstalled.Text = RenderInstalled(installed);
            this.litAvailable.Text = RenderAvailable(available);
        }


        private void RenderSingleTheme(ThemeView t, StringBuilder sb)
        {
            string url = t.PreviewImageUrl;
            if (!url.StartsWith("http"))
            {
                url = Page.ResolveUrl("~" + t.PreviewImageUrl);
            }

            sb.Append("<img class=\"smallpreview\" src=\"" + url + "?uid=" + System.Guid.NewGuid().ToString() + "\" alt=\"" + t.Info.Title + "\" />");
            sb.Append("<h3>" + t.Info.Title + "</h3>");
            sb.Append("<label>Author:</label><span class=\"author\"><a href=\"" + t.Info.AuthorUrl + "\" target=\"_blank\">" + t.Info.Author + "</a></span><br />");
            sb.Append("<label>Version:</label><span class=\"author\">" + t.Info.Version + "</span><br />");
        }

        private string RenderInstalled(List<ThemeView> themes)
        {
            StringBuilder sb = new StringBuilder();

            string currentId = MTApp.CurrentRequestContext.CurrentStore.Settings.ThemeId;
            bool isFirst = true;

            sb.Append("<div class=\"themelist\">");
            foreach (ThemeView t in themes)
            {

                sb.Append("<div class=\"themeview controlarea1");
                if ((t.Info.UniqueIdAsString == currentId.ToLower()))
                {
                    sb.Append(" current");
                }
                if ((isFirst))
                {
                    sb.Append(" first");
                }
                isFirst = !isFirst;
                sb.Append("\">");

                RenderSingleTheme(t, sb);

                sb.Append("<div class=\"controls curved\">");
                if ((t.Info.UniqueIdAsString != currentId.ToLower()))
                {
                    sb.Append("<a href=\"ThemesSelect.aspx?id=" + t.Info.UniqueIdAsString + "\" ><img src=\"../images/buttons/SelectThemeBlack.png\" alt=\"Select This Theme\" /></a>");
                }
                sb.Append("<a href=\"ThemesDuplicate.aspx?id=" + t.Info.UniqueIdAsString + "\"><img src=\"../images/buttons/CopyBlack.png\" alt=\"Copy This Theme\" /></a>");
                sb.Append("<a onclick=\"return window.confirm('Delete this theme? There is no UNDO');\" href=\"ThemesDelete.aspx?id=" + t.Info.UniqueIdAsString + "\"><img src=\"../images/buttons/DeleteBlack.png\" alt=\"Delete This Theme\" /></a>");
                sb.Append("<a href=\"ThemesEdit.aspx?id=" + t.Info.UniqueIdAsString + "\"><img src=\"../images/buttons/EditBlack.png\" alt=\"Edit This Theme\" /></a>");
                sb.Append("</div>");
                sb.Append("</div>");
            }
            sb.Append("<div class=\"clear\"></div></div>");

            return sb.ToString();
        }

        private string RenderAvailable(List<ThemeView> themes)
        {
            StringBuilder sb = new StringBuilder();

            string currentId = MTApp.CurrentRequestContext.CurrentStore.Settings.ThemeId;
            bool isFirst = true;

            sb.Append("<div class=\"themelist\">");
            foreach (ThemeView t in themes)
            {

                sb.Append("<div class=\"themeview controlarea1");
                if ((isFirst))
                {
                    sb.Append(" first");
                }
                isFirst = !isFirst;
                sb.Append("\">");
                RenderSingleTheme(t, sb);
                sb.Append("<div class=\"controls curved\">");
                sb.Append("<a href=\"ThemesInstall.aspx?id=" + t.Info.UniqueIdAsString + "\" class=\"installtheme\"><img src=\"../images/buttons/ThemesInstall.png\" alt=\"Install This Theme\" /></a>");
                sb.Append("</div>");
                sb.Append("</div>");
            }
            sb.Append("<div class=\"clear\"></div></div>");

            return sb.ToString();
        }
    }
}
