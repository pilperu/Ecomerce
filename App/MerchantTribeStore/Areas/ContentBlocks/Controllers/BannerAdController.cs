using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Utilities;
using MerchantTribeStore.Areas.ContentBlocks.Models;
using MerchantTribeStore.Controllers.Shared;
using MvcMiniProfiler;

namespace MerchantTribeStore.Areas.ContentBlocks.Controllers
{
    public class BannerAdController : BaseAppController
    {
        //
        // GET: /ContentBlocks/BannerAd/
        public ActionResult Index(ContentBlock block)
        {
            var profiler = MvcMiniProfiler.MiniProfiler.Current;
            using (profiler.Step("Block:BannerAd"))
            {


                BannerAdViewModel model = new BannerAdViewModel();

                if (block != null)
                {
                    model.ImageUrl = block.BaseSettings.GetSettingOrEmpty("imageurl");
                    model.AltText = block.BaseSettings.GetSettingOrEmpty("alttext");
                    model.CssId = block.BaseSettings.GetSettingOrEmpty("cssid");
                    model.CssClass = block.BaseSettings.GetSettingOrEmpty("cssclass");
                    model.LinkUrl = block.BaseSettings.GetSettingOrEmpty("linkurl");
                    model.ImageUrl = TagReplacer.ReplaceContentTags(model.ImageUrl,
                                                                    MTApp,
                                                                    "",
                                                                    Request.IsSecureConnection);
                }

                return View(model);
            }
        }
    
        

    }
}
