using System;
using System.Collections.Generic;
using MerchantTribe.Commerce.Content;
using System.Text;

namespace MerchantTribeStore
{

    public partial class BVAdmin_SetupWizard_WizardTheme : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Setup Wizard | Themes";
            this.CurrentTab = AdminTabType.Configuration;
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            LoadThemes();
            LoadSamples();
        }

        private void LoadThemes()
        {
            ThemeManager tm = MTApp.ThemeManager();
            List<ThemeView> available = tm.FindAvailableThemes(true);
            this.litThemes.Text = RenderThemes(available);
        }

        private string RenderThemes(List<ThemeView> themes)
        {
            StringBuilder sb = new StringBuilder();


            foreach (ThemeView t in themes)
            {
                sb.Append("<div class=\"themeview controlarea1 wizard\">");

                sb.Append("<img class=\"smallpreview\" src=\"" + Page.ResolveUrl("~" + t.PreviewImageUrl) + "?uid=" + System.Guid.NewGuid().ToString() + "\" alt=\"" + t.Info.Title + "\" />");
                sb.Append("<h3>" + t.Info.Title + "</h3>");

                sb.Append("<div style=\"padding:7px 0 0 0;\">");
                sb.Append("<a href=\"WizardThemeInstall.aspx?id=" + t.Info.UniqueIdAsString + "\" class=\"installtheme\"><img src=\"" + Page.ResolveUrl("~/bvadmin/images/buttons/ChooseThisTheme.png") + "\" alt=\"Choose This Theme\" /></a>");
                sb.Append("</div>");

                sb.Append("</div>");


            }

            return sb.ToString();
        }

        private void LoadSamples()
        {
            if (MTApp.CatalogServices.Products.FindAllCount() == 0)
            {
                MTApp.AddSampleProductsToStore();
            }
        }


    }
}