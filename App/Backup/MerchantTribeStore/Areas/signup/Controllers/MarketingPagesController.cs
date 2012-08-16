using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribeStore.Areas.signup.Models;

namespace MerchantTribeStore.Areas.signup.Controllers
{
    public class MarketingPagesController : BaseSignupController
    {        
        // GET: /signup
        public ActionResult Pricing()
        {            
            return View();
        }

        #region Redirected Actions

        // GET: /signup/about
        public ActionResult About()
        {
            return RedirectPermanent("http://MerchantTribe.com/about");            
        }

        // GET: /signup/desgin
        public ActionResult Design()
        {
            return RedirectPermanent("http://merchanttribe.com/features/design");            
        }

        // GET: /signup/features
        public ActionResult Features()
        {
            return RedirectPermanent("http://merchanttribe.com/features");            
        }
                
        // GET: /signup/home
        public ActionResult Home()
        {
            return RedirectPermanent("http://merchanttribe.com");            
        }

        // GET: /signup/paypal
        public ActionResult PayPalOffer()
        {
            return RedirectPermanent("http://merchanttribe.com");            
        }

        // GET: /signup/themes
        public ActionResult Themes()
        {
            return RedirectPermanent("http://merchanttribe.com/features/design");            
        }

        // GET: /signup/promote
        public ActionResult Promote()
        {
            return RedirectPermanent("http://merchanttribe.com/features/customer-relationships");            
        }

        // GET: /signup/secure
        public ActionResult Secure()
        {
            return RedirectPermanent("http://merchanttribe.com/features/security");            
        }

        // GET: /signup/sell
        public ActionResult Sell()
        {
            return RedirectPermanent("http://merchanttribe.com/features/products-and-orders");                
        }
        
        // GET: /signup/tour
        public ActionResult Tour()
        {
            return RedirectPermanent("http://merchanttribe.com/features");
        }

        #endregion

    }
}
