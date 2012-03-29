using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class ContentColumn: ITagHandler
    {

        public string TagName
        {
            get { return "sys:contentcolumn"; }
        }

        public void Process(List<ITemplateAction> actions, MerchantTribe.Commerce.MerchantTribeApplication app, ITagProvider tagProvider, ParsedTag tag, string innerContents)
        {
            string colId = tag.GetSafeAttribute("columnid");
            if (string.IsNullOrEmpty(colId))
            {
                colId = tag.GetSafeAttribute("id");
            }
            if (string.IsNullOrEmpty(colId))
            {
                colId = tag.GetSafeAttribute("columnname");
            }
            actions.Add(new Actions.CallAction("contentcolumn", "Index", new { id = colId, Area="" }));
        }
    }
}