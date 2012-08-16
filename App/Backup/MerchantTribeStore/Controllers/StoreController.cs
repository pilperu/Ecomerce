using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore.Controllers
{
    public class StoreController : Shared.BaseStoreController
    {
        //
        // GET: /StoreNotFound
        public ActionResult NotFound()
        {
            return View();
        }

        //
        // GET: /StoreNotAvailable
        public ActionResult NotAvailable()
        {
            return View();
        }

        public ActionResult Closed()
        {
            if (MTApp.CurrentStore.Settings.StoreClosed == false)
            {
                return Redirect(MTApp.StoreUrl(false, true));
            }

            string message = MTApp.CurrentRequestContext.CurrentStore.Settings.StoreClosedDescription;
            if (string.IsNullOrEmpty(message))
            {
                message = "Our store is currently closed while we perform updates. We appreciate your patience.";
            }

            ViewBag.ClosedMessage = message;
            ViewBag.Title = MTApp.CurrentStore.StoreName;

            return View();
        }

        public ActionResult NoPermission()
        {
            ViewBag.Tit = MTApp.CurrentStore.StoreName;
            return View();
        }

        public ActionResult Error()
        {
            ViewBag.Title = MTApp.CurrentStore.StoreName;

            Response.StatusCode = 404;            
            string type = Request.QueryString["type"];
            if (string.Compare(type, "product", true) == 0)
            {
                ViewBag.ErrorHeader = SiteTerms.GetTerm(SiteTermIds.ErrorPageHeaderTextProduct);
                ViewBag.ErrorContent = SiteTerms.GetTerm(SiteTermIds.ErrorPageContentTextProduct);

                if (ViewBag.ErrorHeader == string.Empty)
                {
                    ViewBag.ErrorHeader = "Error finding product";
                }

                if (ViewBag.ErrorContent == string.Empty)
                {
                    ViewBag.ErrorContent = "An error occurred while trying to find the specified product.";
                }
                Response.StatusDescription = "Product Not Found";
            }
            else if (string.Compare(type, "category", true) == 0)
            {
                ViewBag.ErrorHeader = SiteTerms.GetTerm(SiteTermIds.ErrorPageHeaderTextCategory);
                ViewBag.ErrorContent = SiteTerms.GetTerm(SiteTermIds.ErrorPageContentTextCategory);

                if (ViewBag.ErrorHeader == string.Empty)
                {
                    ViewBag.ErrorHeader = "Error finding category";
                }

                if (ViewBag.ErrorContent == string.Empty)
                {
                    ViewBag.ErrorContent = "An error occurred while trying to find the specified category.";
                }
                Response.StatusDescription = "Category Not Found";
            }
            else
            {
                string requested = string.Empty;
                if (Request.QueryString["aspxerrorpath"] != null)
                {
                    requested = Request.QueryString["aspxerrorpath"];
                }
                else
                {
                    requested = Request.RawUrl;
                }

                MerchantTribe.Commerce.Content.CustomUrl url = MTApp.ContentServices.CustomUrls.FindByRequestedUrl(requested);
                if (url != null)
                {
                    if (url.Bvin != string.Empty)
                    {
                        if (url.IsPermanentRedirect)
                        {
                            Response.RedirectPermanent(url.RedirectToUrl);
                        }
                        else
                        {
                            Response.Redirect(url.RedirectToUrl);
                        }
                    }
                }



                ViewBag.ErrorHeader = SiteTerms.GetTerm(SiteTermIds.ErrorPageHeaderTextGeneric);
                ViewBag.ErrorContent = SiteTerms.GetTerm(SiteTermIds.ErrorPageContentTextGeneric);

                if (ViewBag.ErrorHeader == string.Empty)
                {
                    ViewBag.ErrorHeader = "Error finding page";
                }

                if (ViewBag.ErrorContent == string.Empty)
                {
                    ViewBag.ErrorContent = "An error occurred while processing your request.";
                }
            }

            return View();
        }
    }
}
