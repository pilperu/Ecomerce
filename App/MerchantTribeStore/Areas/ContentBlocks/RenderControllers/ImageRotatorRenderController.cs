using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribeStore.Areas.ContentBlocks.Models;
using MerchantTribe.Commerce.Utilities;
using System.Text;
using MerchantTribe.Commerce;

namespace MerchantTribeStore.Areas.ContentBlocks.RenderControllers
{
    public class ImageRotatorRenderController : BaseRenderController, IContentBlockRenderController
    {
        public string Render(MerchantTribe.Commerce.MerchantTribeApplication app, dynamic viewBag, MerchantTribe.Commerce.Content.ContentBlock block)
        {
            ImageRotatorViewModel model = new ImageRotatorViewModel();

            if (block != null)
            {
                var imageList = block.Lists.FindList("Images");
                foreach (var listItem in imageList)
                {
                    ImageRotatorImageViewModel img = new ImageRotatorImageViewModel();
                    img.ImageUrl = ResolveUrl(listItem.Setting1, app);
                    img.Url = listItem.Setting2;
                    if (img.Url.StartsWith("~"))
                    {
                        img.Url = app.CurrentRequestContext.UrlHelper.Content(img.Url);
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

            return RenderModel(model);
        }

        public static void RenderSingleImage(StringBuilder sb, ImageRotatorImageViewModel img, int height, int width)
        {                        
            sb.Append("<a href=\"" + img.Url + "\" ");
            if (img.NewWindow == true)
            { sb.Append(" target=\"_blank\""); }
            sb.Append(">");
            sb.Append("<div style=\"height:" + height + "px;overflow:hidden\">");
            sb.Append("<img src=\"" + img.ImageUrl + "\" alt=\"" + HttpUtility.HtmlEncode(img.Caption) + "\" width=\"" + width + "\" />");
            sb.Append("</div>");
            sb.Append("</a>");            
        }

        private string RenderModel(ImageRotatorViewModel model)
        {
            StringBuilder sb = new StringBuilder();

            if (model.Images.Count > 0)
            {
                sb.Append("<div class=\"" + model.CssClass + "\">\n");
                sb.Append("<ul id=\"" + model.CssId + "\" class=\"imagerotatorlist\" style=\"height:" + model.Height + "px;width:" + model.Width + "px;\">");
                
                for (int i = 0; i < model.Images.Count; i++)
                {
                    var img = model.Images[i];
                    sb.Append("<li ");
                    if (i == 0)
                    { sb.Append("class=\"show\""); }
                    sb.Append(">");
                    RenderSingleImage(sb, img, model.Height, model.Width);
                    sb.Append("</li>");
                }
                sb.Append("</ul>\n");
                sb.Append("</div>");
                sb.Append("<script type=\"text/javascript\">\n");
                sb.Append(" $(document).ready(function() {");
                sb.Append(" StartRotator('" + model.CssId + "', " + model.Pause + ");");
                sb.Append(" });");
                sb.Append("\n</script>");
                sb.Append("\n<style type=\"text/css\">");
                
                sb.Append("\n#" + model.CssId + " {");            
                if (model.Height > 0)
                {
                    sb.Append("height: " + model.Height + "px;");
                }
                if (model.Width > 0)
                {
                    sb.Append("width: " + model.Width + "px;");
                }                    
                sb.Append("position:relative;");
                sb.Append("margin:0;padding:0;");
                sb.Append("}");
            
                sb.Append("\n#" + model.CssId + " li { ");       
                sb.Append("list-style:none;");
                sb.Append("float:left;");
                sb.Append("position:absolute;");
                sb.Append("margin:0;padding:0;");
                sb.Append("}");
            
                sb.Append("\n#" + model.CssId + " li.show { ");       
                sb.Append("z-index:500;");
                sb.Append("}");
                sb.Append("</style>");
            }
   
            return sb.ToString();
        }

        private static string ResolveUrl(string raw, MerchantTribeApplication app)
        {
            // full url
            string tester = raw.Trim().ToLowerInvariant();
            if (tester.StartsWith("http:") || tester.StartsWith("https:")
                || tester.StartsWith("//")) return raw;

            // tag replaced url {{img}} or {{assets}
            if (tester.StartsWith("{{"))
            {
                return TagReplacer.ReplaceContentTags(raw, app, "");   
            }

            // app relative url
            if (tester.StartsWith("~"))
            {
                return app.CurrentRequestContext.UrlHelper.Content(raw);
            }
            
            // old style theme asset
            return MerchantTribe.Commerce.Storage.DiskStorage.AssetUrl(
                app, app.CurrentStore.Settings.ThemeId,
                raw, app.IsCurrentRequestSecure());
        }

        private void RandomizeList(List<ImageRotatorImageViewModel> list)
        {
            System.Random rand = new Random();
            for (int i = list.Count - 1; i >= 0; i--)
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