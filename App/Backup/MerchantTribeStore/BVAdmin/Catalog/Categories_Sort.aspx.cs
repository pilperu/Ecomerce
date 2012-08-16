using System.Collections.Generic;
using System.Web.UI;
using MerchantTribe.Commerce.Catalog;
using System;
using System.Collections.ObjectModel;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Catalog_Categories_Sort : BaseAdminJsonPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string ids = Request.Form["ids"];
                string parentId = Request.Form["bvin"];
                if (parentId.StartsWith("children"))
                {
                    parentId = parentId.Substring(8);
                }

                Resort(ids, parentId);
            }

        }

        private void Resort(string ids, string parentId)
        {

            List<CategorySnapshot> children = MTApp.CatalogServices.Categories.FindChildren(parentId);

            string[] sorted = ids.Split(',');
            List<string> l = new List<string>();
            foreach (string id in sorted)
            {
                l.Add(id);
            }

            int counter = 1;
            foreach (string s in l)
            {
                foreach (CategorySnapshot child in children)
                {
                    if (child.Bvin == s)
                    {
                        Category c = MTApp.CatalogServices.Categories.Find(child.Bvin);
                        c.SortOrder = counter;
                        counter += 1;
                        MTApp.CatalogServices.Categories.Update(c);
                    }
                }
            }

            this.litOutput.Text = "true";

        }

    }
}