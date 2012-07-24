using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Utilities;
using MerchantTribeStore.Areas.ContentBlocks.Models;
using MerchantTribeStore.Controllers.Shared;

namespace MerchantTribeStore.Areas.ContentBlocks.Controllers
{
    public class ImageRotatorController : BaseAppController
    {
        //
        // GET: /ContentBlocks/ImageRotator/
        public ActionResult Index(ContentBlock block)
        {
            ImageRotatorViewModel model = new ImageRotatorViewModel();

            if (block != null)
            {
                var imageList = block.Lists.FindList("Images");
                foreach (var listItem in imageList)
                {
                    ImageRotatorImageViewModel img = new ImageRotatorImageViewModel();
                    img.ImageUrl = ResolveUrl(listItem.Setting1);
                    img.Url = listItem.Setting2;
                    if (img.Url.StartsWith("~"))
                    {
                        img.Url = Url.Content(img.Url);
                    }
                    img.NewWindow = (listItem.Setting3 == "1");
                    img.Caption = listItem.Setting4;
                    model.Images.Add(img);
                }
                string cleanId = MerchantTribe.Web.Text.ForceAlphaNumericOnly(block.Bvin);
                model.CssId = "rotator" + cleanId;
                model.CssClass = block.BaseSettings.GetSettingOrEmpty("cssclass");
              
                model.Height = block.BaseSettings.GetIntegerSetting("Height");
                model.Width = block.BaseSettings.GetIntegerSetting("Width");

                if (block.BaseSettings.GetBoolSetting("ShowInOrder") == false)
                {
                    RandomizeList(model.Images);
                }
            }
                    
            return View(model);
        }

        private string ResolveUrl(string raw)
        {
            if (raw.Trim().ToLowerInvariant().StartsWith("http")) return raw;
            return MerchantTribe.Commerce.Storage.DiskStorage.AssetUrl(
                MTApp, MTApp.CurrentStore.Settings.ThemeId,
                raw, Request.IsSecureConnection);
        }

        private void RandomizeList(List<ImageRotatorImageViewModel> list)
        {
            System.Random rand = new Random();
            for (int i = list.Count -1; i >= 0;i--)
            {
                int n = rand.Next(i + 1);            
                // Swap
                ImageRotatorImageViewModel temp = list[i];
                list[i] = list[n];
                list[n] = temp;
            }
        }
    }
}
