using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;
using System.Text;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class Copyright: ITagHandler
    {

        public string TagName
        {
            get { return "sys:copyright"; }
        }

        public void Process(StringBuilder output, 
                            MerchantTribe.Commerce.MerchantTribeApplication app, 
                            dynamic viewBag,
                            ITagProvider tagProvider, 
                            ParsedTag tag, 
                            string innerContents)
        {            
            string byParam = tag.GetSafeAttribute("by");
            
            dynamic model = new ExpandoObject();
            model.By = byParam;

            output.Append(Render(model));
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