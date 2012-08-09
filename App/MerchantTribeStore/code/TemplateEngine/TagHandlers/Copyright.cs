using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class Copyright: ITagHandler
    {

        public string TagName
        {
            get { return "sys:copyright"; }
        }

        public void Process(List<ITemplateAction> actions, 
                            MerchantTribe.Commerce.MerchantTribeApplication app, 
                            dynamic viewBag,
                            ITagProvider tagProvider, 
                            ParsedTag tag, 
                            string innerContents)
        {            
            string byParam = tag.GetSafeAttribute("by");
            
            dynamic model = new ExpandoObject();
            model.By = byParam;

            actions.Add(new Actions.LiteralText(Render(model)));
        }

        private string Render(dynamic model)
        {
            string result = 
            "<span class=\"copyright\">Copyright &copy; " + DateTime.Now.Year.ToString() + "</span>&nbsp;";
            if (!String.IsNullOrEmpty(model.By))
            {
                result += model.By;
            }
            return result;
        }
    }
}