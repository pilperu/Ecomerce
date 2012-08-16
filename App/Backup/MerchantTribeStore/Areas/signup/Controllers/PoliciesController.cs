using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MerchantTribeStore.Areas.signup.Controllers
{
    public class PoliciesController : Controller
    {        
        // GET: /signup/policies/privacy
        public ActionResult Privacy()
        {
            return View();
        }

        // GET: /signup/policies/refund
        public ActionResult Refund()
        {
            return View();
        }

        // GET: /signup/policies/terms
        public ActionResult Terms()
        {
            return View();
        }

    }
}
