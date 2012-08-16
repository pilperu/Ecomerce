using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore.BVAdmin.Catalog
{
    public partial class RelatedItems_Delete : BaseAdminJsonPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                string relatedId = Request.Form["id"];
                string productId = Request.Form["productid"];
                Remove(relatedId, productId);
            }
        }

        private void Remove(string relatedId, string productId)
        {
            if (MTApp.CatalogServices.ProductRelationships.UnrelateProducts(productId, relatedId))
            {
                this.litOutput.Text = "{\"result\":true}";
                return;
            }
                        
            this.litOutput.Text = "{\"result\":false}";
            
        }

    }

}