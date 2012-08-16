using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribeStore.Filters;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Web;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Orders;

namespace MerchantTribeStore.Areas.account.Controllers
{
    [CustomerSignedInFilter]
    public class WishListController : MerchantTribeStore.Controllers.Shared.BaseStoreController
    {
        //
        // GET: /account/saveditems/
        public ActionResult Index()
        {            
            ViewBag.MetaDescription = "Saved Items | " + MTApp.CurrentStore.Settings.MetaDescription;
            ViewBag.MetaKeywords = MTApp.CurrentStore.Settings.MetaKeywords;
            ViewBag.BodyClass = "myaccountpage";
            ViewBag.XButton = MTApp.ThemeManager().ButtonUrl("x", Request.IsSecureConnection);
            ViewBag.AddButton = MTApp.ThemeManager().ButtonUrl("AddToCart", Request.IsSecureConnection);

            List<WishListItem> items = MTApp.CatalogServices.WishListItems.FindByCustomerIdPaged(MTApp.CurrentCustomerId, 1, 50);
            List<Models.SavedItemViewModel> model = PrepItems(items);
            if (model.Count < 1)
            {
                FlashInfo("You do not have any saved items at at the moment.");
            }
            return View(model);
        }

        // /account/saveditems/delete
        [HttpPost]
        public ActionResult Delete(long itemid)
        {
            string customerId = MTApp.CurrentCustomerId;
            WishListItem wi = MTApp.CatalogServices.WishListItems.Find(itemid);
            if (wi != null)            
            {
                if (wi.CustomerId == customerId)
                {
                    MTApp.CatalogServices.WishListItems.Delete(itemid);
                }
            }
            return RedirectToAction("index");
        }

        // /account/saveditems/addtocart
        [HttpPost]
        public ActionResult AddToCart(long itemid)
        {
            string customerId = MTApp.CurrentCustomerId;
            WishListItem wi = MTApp.CatalogServices.WishListItems.Find(itemid);
            if (wi != null)
            {
                if (wi.CustomerId == customerId)
                {
                    // Add to Cart
                    Product p = MTApp.CatalogServices.Products.Find(wi.ProductId);

                    bool IsPurchasable = ValidateSelections(p, wi);
                    if ((IsPurchasable))
                    {
                        
                            LineItem li = MTApp.CatalogServices.ConvertProductToLineItem(p,
                                                                                            wi.SelectionData,
                                                                                            1,
                                                                                            MTApp);
                            Order Basket = SessionManager.CurrentShoppingCart(MTApp.OrderServices, MTApp.CurrentStore);
                            if (Basket.UserID != MTApp.CurrentCustomerId)
                            {
                                Basket.UserID = MTApp.CurrentCustomerId;
                            }

                            MTApp.AddToOrderWithCalculateAndSave(Basket, li);
                            SessionManager.SaveOrderCookies(Basket, MTApp.CurrentStore);

                            return Redirect("~/cart");                        
                    }            
                }
            }
            return RedirectToAction("index");
        }

        private bool ValidateSelections(Product p, WishListItem item)
        {
            bool result = false;

            if ((p.HasOptions()))
            {
                if ((p.HasVariants()))
                {
                    Variant v = p.Variants.FindBySelectionData(item.SelectionData, p.Options);
                    if ((v != null))
                    {
                        result = true;
                    }
                    else
                    {
                        return false;//model.ValidationMessage = "<div class=\"flash-message-warning\">The options you've selected aren't available at the moment. Please select different options.</div>";
                    }
                }
                else
                {
                    result = true;
                }

                // Make sure no "labels" are selected
                if (item.SelectionData.HasLabelsSelected())
                {
                    result = false;
                    return false; // model.ValidationMessage = "<div class=\"flash-message-warning\">Please make all selections before adding to cart.</div>";
                }
            }
            else
            {
                result = true;
            }

            return result;
        }

        private List<Models.SavedItemViewModel> PrepItems(List<WishListItem> items)
        {
            List<Models.SavedItemViewModel> result = new List<Models.SavedItemViewModel>();

            if (items == null) return result;

            foreach (var item in items)
            {
                Models.SavedItemViewModel m = new Models.SavedItemViewModel();
                m.SavedItem = item;
                m.FullProduct = new MerchantTribeStore.Models.SingleProductViewModel(MTApp.CatalogServices.Products.Find(item.ProductId), MTApp);
                result.Add(m);
            }

            return result;
        }
    }
}
