using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;
using System.Text;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class FlexEditorPanel : ITagHandler
    {

        public string TagName
        {
            get { return "sys:flexeditor"; }
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
                sb.Append("<div id=\"flexedit\">");
                sb.Append("<div class=\"flexbuttonright\">");
                sb.Append("<a href=\"" + app.StoreUrl(false, false) + "bvadmin/catalog/Categories_FinishedEditing.aspx?id=\"" + editorModel.CategoryId + "\">");
                sb.Append("<img src=\"" + app.StoreUrl(false, false) + "images/system/flexedit/btnClose.png" + "\" alt=\"Close Editor\" />");
                sb.Append("</a>");
                sb.Append("</div>");
                if (editorModel.IsPreview == true)
                {
                    sb.Append("<div class=\"flexbuttonright\">");
                    sb.Append("<a href=\"" + app.StoreUrl(false,false) + editorModel.CurrentPageUrl + "\">");
                    sb.Append("<img src=\"" + app.StoreUrl(false,false) + "images/system/flexedit/btnPreviewOn.png\" alt=\"Preview Is On\" />");
                    sb.Append("</a>");
                    sb.Append("</div>");
                }
                else
                {
                    sb.Append("<div class=\"flexbuttonright\">");
                    sb.Append("<a href=\"" + app.StoreUrl(false,false) + editorModel.CurrentPageUrl + "?preview=1\">");
                    sb.Append("<img src=\"" + app.StoreUrl(false,false) + "images/system/flexedit/btnPreviewOff.png\" alt=\"Preview Is Off\" />");
                    sb.Append("</a>");
                    sb.Append("</div>");
                }
                
                sb.Append("<div class=\"dragpart dragbutton\" id=\"columncontainer\" ><img src=\"" + app.StoreUrl(false,false) + "images/system/flexedit/btnColumns.png\" alt=\"Columns\" /></div>");
                sb.Append("<div class=\"dragpart dragbutton\" id=\"htmlpart\" ><img src=\"" + app.StoreUrl(false,false) + "images/system/flexedit/btnHtml.png\" alt=\"HTML\" /></div>");
                sb.Append("<div class=\"dragpart dragbutton\" id=\"image\" ><img src=\"" + app.StoreUrl(false,false) + "images/system/flexedit/btnImage.png\" alt=\"Image\" /></div>");
                    
                sb.Append("<div class=\"hidden\" id=\"flexpageid\">" + editorModel.CategoryId + "</div>");
                sb.Append("<div class=\"hidden\" id=\"flexjsonurl\">" + app.StoreUrl(false,false) + "flexpartjson/" + editorModel.CategoryId + "</div>");
                sb.Append("<div class=\"hidden\" id=\"flexpageediting\"></div>");
                sb.Append("</div>");
            }

            return sb.ToString();
        }
    }
}