using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Catalog;


namespace MerchantTribeStore.BVAdmin.Content
{
    public partial class CustomUrlRemove : BaseAdminJsonPage
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string urlId = Request.Form["id"];
                Delete(urlId);
            }
        }

        private void Delete(string id)
        {
            bool result = false;
            result = MTApp.ContentServices.CustomUrls.Delete(id);
            if ((result))
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