using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;
using System.Text;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class CategoryBanner : ITagHandler
    {

        public string TagName
        {
            get { return "sys:category-banner"; }
        }

        public void Process(StringBuilder output,
                            MerchantTribe.Commerce.MerchantTribeApplication app,
                            dynamic viewBag,
                            ITagProvider tagProvider,
                            ParsedTag tag,
                            string innerContents)
        {
            var category = app.CurrentRequestContext.CurrentCategory;
            if (category != null)
            {
                if (category.BannerImageUrl.Trim().Length > 0)
                {
                    string bannerUrl = MerchantTribe.Commerce.Storage.DiskStorage.CategoryBannerUrl(
                                        app,
                                        category.Bvin,
                                        category.BannerImageUrl,
                                        app.IsCurrentRequestSecure());
                    output.Append("<div id=\"categorybanner\">");
                    output.Append("<img src=\"" + bannerUrl + "\" alt=\"" + HttpUtility.HtmlEncode(category.Name) + "\" />");
                    output.Append("</div>");                    
                }
            }
        }
    }
}