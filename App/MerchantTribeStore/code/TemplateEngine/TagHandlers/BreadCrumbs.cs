using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribeStore.Models;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class BreadCrumbs : ITagHandler
    {

        public string TagName
        {
            get { return "sys:breadcrumbs"; }
        }

        public void Process(List<ITemplateAction> actions, MerchantTribe.Commerce.MerchantTribeApplication app, ITagProvider tagProvider, ParsedTag tag, string innerContents)
        {
            List<BreadCrumbItem> extras = new List<BreadCrumbItem>();

            string[] parts = innerContents.Split(',');
            if (parts.Length > 0)
            {
                foreach (string p in parts)
                {
                    string[] linkParts = p.Split('|');
                    if (linkParts.Length > 0)
                    {
                        string name = linkParts[0].Trim();
                        if (name.Length > 0)
                        {
                            BreadCrumbItem item = new BreadCrumbItem();
                            item.Name = linkParts[0].Trim();
                            item.Link = "";
                            if (linkParts.Length > 1)
                            {
                                item.Link = linkParts[1].Trim();
                            }
                            extras.Add(item);
                        }
                    }
                }
            }

            string mode = tag.GetSafeAttribute("mode");
            if (mode == "manual")
            {
                actions.Add(new Actions.CallAction("BreadCrumb", "ManualTrail",
                        new { extras = extras }));
            }
            else
            {
                if (app.CurrentRequestContext.CurrentProduct != null)
                {
                    actions.Add(new Actions.CallAction("BreadCrumb", "ProductTrail",
                        new { product = app.CurrentRequestContext.CurrentProduct, extras = extras }));
                }
                else if (app.CurrentRequestContext.CurrentCategory != null)
                {
                    actions.Add(new Actions.CallAction("BreadCrumb", "CategoryTrail",
                        new { cat = app.CurrentRequestContext.CurrentCategory, extras = extras }));
                }
                else
                {
                    actions.Add(new Actions.CallAction("BreadCrumb", "ManualTrail",
                        new { extras = extras }));
                }
            }
        }
    }
}