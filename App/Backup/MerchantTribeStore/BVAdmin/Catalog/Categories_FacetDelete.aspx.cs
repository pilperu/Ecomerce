using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore.BVAdmin.Catalog
{
    public partial class Categories_FacetDelete : BaseAdminJsonPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string catId = Request.Form["id"];
                Delete(catId);
            }
        }

        private void Delete(string id)
        {
            bool result = false;
            long facetId = 0;
            long.TryParse(id, out facetId);

            CategoryFacetManager manager = CategoryFacetManager.InstantiateForDatabase(MTApp.CurrentRequestContext);
            result = manager.Delete(facetId);
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