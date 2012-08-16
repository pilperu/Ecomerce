using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Configuration_ThemesEditCSS_Update : BaseAdminJsonPage
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string themeid = Request.Form["themeid"];
                string css = Request.Form["css"];
                css = System.Web.HttpUtility.UrlDecode(css);

                Update(themeid, css);
            }

        }

        private void Update(string themeid, string css)
        {

            bool result = MTApp.ThemeManager().UpdateStyleSheet(themeid, css);

            if (result)
            {
                this.litOutput.Text = "{\"result\":true}";
            }
            else
            {
                this.litOutput.Text = "{\"result\":false}";
            }
        }

    }
}