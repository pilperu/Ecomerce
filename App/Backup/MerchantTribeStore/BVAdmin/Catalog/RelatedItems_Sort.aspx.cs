using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MerchantTribeStore.BVAdmin.Catalog
{
    public partial class RelatedItems_Sort : BaseAdminJsonPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);


            if (!Page.IsPostBack)
            {
                string ids = Request.Form["ids"];
                string productId = Request.Form["productid"];
                Resort(ids, productId);
            }

        }

        private void Resort(string ids, string productId)
        {

            string[] sorted = ids.Split(',');
            List<string> l = new List<string>();
            foreach (string id in sorted)
            {
                // trim off "item" prefix used to ensure valid CSS ids that start with letters
                l.Add(id.Replace("item",""));
            }

            if ((MTApp.CatalogServices.ProductRelationships.ResortRelationships(productId, l)))
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