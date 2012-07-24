using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Utilities;
using MerchantTribeStore.Controllers.Shared;
using MerchantTribeStore.Models;

namespace MerchantTribeStore.Areas.ContentBlocks.Controllers
{
    public class ProductRotatorController : BaseAppController
    {
        //
        // GET: /ContentBlocks/ProductRotator/
        public ActionResult Index(ContentBlock block)
        {
            Product p = LoadProduct(block);
            return View(p);
        }

        private Product LoadProduct(ContentBlock b)
        {
            List<ContentBlockSettingListItem> myProducts = b.Lists.FindList("Products");
            if (myProducts != null)
            {
                if (myProducts.Count > 0)
                {
                    int displayIndex = GetProductIndex(myProducts.Count - 1);

                    ContentBlockSettingListItem data = myProducts[displayIndex];
                    string bvin = data.Setting1;
                    Product p = MTApp.CatalogServices.Products.Find(bvin);
                    return p;                    
                }
            }
            return null;
        }

        private int GetProductIndex(int maxIndex)
        {
            int result = 0;

            result = MerchantTribe.Web.RandomNumbers.RandomInteger(maxIndex, 0);

            return result;
        }


    }
}
