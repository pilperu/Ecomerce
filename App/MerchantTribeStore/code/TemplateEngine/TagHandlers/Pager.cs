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


            // Only show 10 pages at a time, sliding window based on current page
            int _RenderStartPage = 1;
            int _RenderEndPage = model.TotalPages;
            if (model.TotalPages > 10)
            {
                if (model.CurrentPage < 5)
                {
                    _RenderStartPage = 1;
                    _RenderEndPage = 10;
                }
                else if (model.CurrentPage > model.TotalPages - 5)
                {
                    _RenderStartPage = model.TotalPages - 9;
                    _RenderEndPage = model.TotalPages;
                }
                else
                {
                    _RenderStartPage = model.CurrentPage - 4;
                    _RenderEndPage = model.CurrentPage + 5;
                }
            }

            int pages = _RenderEndPage - _RenderStartPage + 1;
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

            for (int i = _RenderStartPage; i <= _RenderEndPage; i++)
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

            if (_RenderEndPage < model.TotalPages)
            {
                sb.Append("<a href=\"" + String.Format(model.PagerUrlFormat, _RenderEndPage + 1) + "\">...</a>");
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