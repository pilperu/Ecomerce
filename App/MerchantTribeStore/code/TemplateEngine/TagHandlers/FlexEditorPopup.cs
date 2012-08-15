using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;
using System.Text;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class FlexEditorPopup : ITagHandler
    {

        public string TagName
        {
            get { return "sys:flexpopup"; }
        }

        public void Process(StringBuilder output,
                            MerchantTribe.Commerce.MerchantTribeApplication app,
                            dynamic viewBag,
                            ITagProvider tagProvider,
                            ParsedTag tag,
                            string innerContents)
        {
            output.Append(Render(app, viewBag));
        }

        public string Render(MerchantTribeApplication app, dynamic viewBag)
        {
            StringBuilder sb = new StringBuilder();

            MerchantTribeStore.Models.FlexPageEditorViewModel editorModel = null;
            if (viewBag.FlexEditorModel != null)
            {
                editorModel = viewBag.FlexEditorModel;
            }
        
            if (editorModel != null && editorModel.IsEditMode == true)
            {
                sb.Append("<div class=\"editormodal\">");
                sb.Append("<div class=\"editorpopover\">");
                sb.Append("<a id=\"editorclose\" href=\"#\">Close</a>");
                sb.Append("<form id=\"editorform\" action=\"\" method=\"post\"></form><br />");
                sb.Append("</div>");
                sb.Append("</div>");
            }

            return sb.ToString();
        }
    }
}