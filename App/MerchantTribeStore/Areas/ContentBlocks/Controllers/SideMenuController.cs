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
using MerchantTribeStore.Areas.ContentBlocks.Models;

namespace MerchantTribeStore.Areas.ContentBlocks.Controllers
{
    public class SideMenuController : Controller
    {
        //
        // GET: /ContentBlocks/SideMenu/

        public ActionResult Index(ContentBlock block)
        {

            SideMenuViewModel model = new SideMenuViewModel();

            if (block != null)
            {
                model.Title = block.BaseSettings.GetSettingOrEmpty("Title");

                List<ContentBlockSettingListItem> links = block.Lists.FindList("Links");
                if (links != null)
                {
                    foreach (ContentBlockSettingListItem l in links)
                    {
                        model.Items.Add(AddSingleLink(l));
                    }
                }
            }

            return View(model);
        }

        private SideMenuItem AddSingleLink(ContentBlockSettingListItem l)
        {
            SideMenuItem result = new SideMenuItem();
            result.Title = l.Setting4;
            result.Name = l.Setting1;
            result.Url = l.Setting2;
            if (l.Setting3 == "1")
            {
                result.OpenInNewWindow = true;
            }
            result.CssClass = l.Setting5;
            return result;
        }

    }
}
