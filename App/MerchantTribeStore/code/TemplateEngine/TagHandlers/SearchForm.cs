using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        public void Process(List<ITemplateAction> actions, MerchantTribe.Commerce.MerchantTribeApplication app, ITagProvider tagProvider, ParsedTag tag, string innerContents)
        {
            SearchFormViewModel model = new SearchFormViewModel();
            string rootUrl = app.StoreUrl(false, true);
            model.SearchFormUrl = rootUrl + "search";                        
            model.ButtonImageUrl = app.ThemeManager().ButtonUrl("Go", app.IsCurrentRequestSecure());
                        
            actions.Add(new Actions.PartialView("~/views/shared/_SearchForm.cshtml", model));
        }
    }
}