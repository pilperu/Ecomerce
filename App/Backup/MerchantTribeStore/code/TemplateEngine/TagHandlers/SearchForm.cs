using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class SearchFormViewModel
    {
        public string SearchFormUrl { get; set; }
        public string ButtonImageUrl { get; set; }
    }

    public class SearchForm : ITagHandler
    {

        public string TagName
        {
            get { return "sys:searchform"; }
        }

        public void Process(StringBuilder output, 
                            MerchantTribe.Commerce.MerchantTribeApplication app, 
                            dynamic viewBag,
                            ITagProvider tagProvider, 
                            ParsedTag tag, 
                            string innerContents)
        {
            SearchFormViewModel model = new SearchFormViewModel();
            string rootUrl = app.StoreUrl(false, true);
            model.SearchFormUrl = rootUrl + "search";                        
            model.ButtonImageUrl = app.ThemeManager().ButtonUrl("Go", app.IsCurrentRequestSecure());

            Render(output, model);            
        }

        private void Render(StringBuilder sb, SearchFormViewModel model)
        {               
            sb.Append("<form class=\"searchform\" action=\"" + model.SearchFormUrl + "\" method=\"get\">");
            sb.Append("<input type=\"text\" name=\"q\" class=\"searchinput\" /> <input class=\"searchgo\" type=\"image\" src=\"" + model.ButtonImageUrl + "\" alt=\"Search\" />");
            sb.Append("</form>");            
        }

    }
}