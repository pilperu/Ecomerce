using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce.Content.Parts;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore.Controllers
{
    public class FlexPartJsonController : Shared.BaseAppController
    {                       
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Index(string pageid, string partid)        
        {

            JsonResult result = new JsonResult();

            MTApp.CurrentRequestContext.FlexPageId = pageid;
            MTApp.CurrentRequestContext.UrlHelper = this.Url;

            Category flexpage = MTApp.CatalogServices.Categories.Find(pageid);
            if (flexpage != null)
            {                
                IContentPart part = flexpage.FindFlexPart(partid);                                
                result.Data = part.ProcessJsonRequest(Request.Form, MTApp, flexpage);                                
            }

            return result;
        }

    }
}
