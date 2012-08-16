using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using MerchantTribeStore.Models;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class Pager : ITagHandler
    {
        public string TagName
        {
            get { return "sys:pager"; }
        }

        public void Process(StringBuilder output,
                            MerchantTribe.Commerce.MerchantTribeApplication app,
                            dynamic viewBag,
                            ITagProvider tagProvider,
                            ParsedTag tag,
                            string innerContents)
        {
            PagerViewModel model = new PagerViewModel();
            //model.TotalPages = tag.GetSafeAttributeAsInteger("totalpages");
            model.TotalItems = tag.GetSafeAttributeAsInteger("totalitems");
            model.PageSize = tag.GetSafeAttributeAsInteger("pagesize");
            if (model.PageSize < 1) model.PageSize = 1;
            model.CurrentPage = tag.GetSafeAttributeAsInteger("currentpage");            
            model.PagerUrlFormat = tag.GetSafeAttribute("urlformat");
            model.PagerUrlFormatFirst = tag.GetSafeAttribute("urlformatfirst");            
            
            Render(output, model);
        }

        private string IsCurrentPage(int page, PagerViewModel model)
        {
            if (page == model.CurrentPage) return "current";
            return string.Empty;
        }

        public void Render(StringBuilder sb, PagerViewModel model)
        {
            if (model.TotalPages <= 1) return;

            int pages = model.TotalPages;
            sb.Append("<div class=\"pager\">");
            sb.Append("<ul>");
            sb.Append("<li><a href=\"" + String.Format(model.PagerUrlFormatFirst, 1) + "\">|&lt;</a></li>");
            if (model.CurrentPage > 1)
            {
                sb.Append("<li><a href=\"" + String.Format(model.PagerUrlFormat, model.CurrentPage - 1) + "\">&laquo;</a></li>");
            }
            else
            {
                sb.Append("<li class=\"inactive\">&nbsp;</li>");
            }
            for (int i = 1; i <= pages; i++)
            {
                sb.Append("<li class=\"" + IsCurrentPage(i, model) + "\">");
                if (i == 1)
                {
                    sb.Append("<a href=\"" + String.Format(model.PagerUrlFormatFirst, i) + "\">" + i + "</a>");
                }
                else
                {
                    sb.Append("<a href=\"" + String.Format(model.PagerUrlFormat, i) + "\">" + i + "</a>");
                }
                sb.Append("</li>");
            }
            if (model.CurrentPage < pages)
            {
                sb.Append("<li><a href=\"" + String.Format(model.PagerUrlFormat, model.CurrentPage + 1) + "\">&raquo;</a></li>");
            }
            else
            {
                sb.Append("<li class=\"inactive\">&nbsp;</li>");
            }
            sb.Append("<li><a href=\"" + String.Format(model.PagerUrlFormat, pages) + "\">&gt;|</a></li>");
            sb.Append("</ul>");
            sb.Append("</div>");

        }

        public string RenderToString(PagerViewModel model)
        {
            StringBuilder sb = new StringBuilder();
            Render(sb, model);
            return sb.ToString();
        }
    }
}