using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Configuration_ThemesEditHeaderFooter_Update : BaseAdminJsonPage
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string themeid = Request.Form["themeid"];
                string headerhtml = Request.Form["headerhtml"];
                headerhtml = System.Web.HttpUtility.UrlDecode(headerhtml);
                string footerhtml = Request.Form["footerhtml"];
                footerhtml = System.Web.HttpUtility.UrlDecode(footerhtml);

                Update(themeid, headerhtml, footerhtml);
            }

        }

        private void Update(string themeid, string headerhtml, string footerhtml)
        {

            bool result = MerchantTribe.Commerce.Storage.DiskStorage.WriteCustomFooter(MTApp.CurrentStore.Id, themeid, footerhtml);
            if (result)
            {
                result = MerchantTribe.Commerce.Storage.DiskStorage.WriteCustomHeader(MTApp.CurrentStore.Id, themeid, headerhtml);
            }

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