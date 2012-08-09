using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class PartialViewHandler: ITagHandler
    {
        string _tagName = string.Empty;
        string _viewName = string.Empty;
        object _model = null;

        public PartialViewHandler(string tagName, string viewName, object model)
        {
            this._tagName = tagName;
            this._viewName = viewName;
            this._model = model;
        }

        public string TagName
        {
            get { return _tagName; }
        }

        public void Process(List<ITemplateAction> actions, 
                            MerchantTribe.Commerce.MerchantTribeApplication app, 
                            dynamic viewBag,
                            ITagProvider tagProvider, 
                            ParsedTag tag, 
                            string innerContents)
        {
            actions.Add(new Actions.PartialView(_viewName, _model));
        }            
    }
}