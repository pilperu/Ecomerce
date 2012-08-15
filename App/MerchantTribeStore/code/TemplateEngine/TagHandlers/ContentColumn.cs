﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using MerchantTribe.Commerce;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class ContentColumn: ITagHandler
    {

        public string TagName
        {
            get { return "sys:contentcolumn"; }
        }

        public void Process(StringBuilder output, 
                            MerchantTribe.Commerce.MerchantTribeApplication app, 
                            dynamic viewBag, 
                            ITagProvider tagProvider, 
                            ParsedTag tag, 
                            string innerContents)
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

            RenderColumn(output, colId, app, viewBag);            
        }

        public void RenderColumn(StringBuilder sb, string colId, MerchantTribeApplication app, dynamic viewBag)
        {
            var column = LocateColumn(colId, app);
            if (column != null)
            {
                foreach (var block in column.Blocks)
                {
                    sb.Append(MerchantTribeStore.Areas.ContentBlocks.RenderControllers.ContentBlockRenderFactory.RenderBlock(block, app, viewBag));
                }
            }
        }
        public string RenderColumnToString(string colId, MerchantTribeApplication app, dynamic viewBag)
        {            
            StringBuilder sb = new StringBuilder();
            RenderColumn(sb, colId, app, viewBag);
            return sb.ToString();
        }

        private MerchantTribe.Commerce.Content.ContentColumn LocateColumn(string id, MerchantTribeApplication app)
        {
            MerchantTribe.Commerce.Content.ContentColumn result = null;            
            result = app.ContentServices.Columns.Find(id);
            if (result == null)
            {
                result = app.ContentServices.Columns.FindByDisplayName(id);
            }
            return result;
        }
    }
}