using System.Collections.ObjectModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Content;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVModules_ContentBlocks_Side_Menu_adminview : BVModule
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            LoadMenu();
        }

        private void LoadMenu()
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            if (b != null)
            {
                this.TitlePlaceHolder.Controls.Clear();
                string title = b.BaseSettings.GetSettingOrEmpty("Title");
                if (title.Trim().Length > 0)
                {
                    this.TitlePlaceHolder.Controls.Add(new LiteralControl("<h4>" + title + "</h4>"));
                }

                this.MenuControl.Controls.Clear();
                MenuControl.EnableViewState = false;
                List<ContentBlockSettingListItem> links = b.Lists.FindList("Links");
                if (links != null)
                {
                    this.MenuControl.Controls.Add(new LiteralControl("<ul>"));
                    foreach (ContentBlockSettingListItem l in links)
                    {
                        AddSingleLink(l);
                    }
                    this.MenuControl.Controls.Add(new LiteralControl("</ul>"));
                }
            }
        }

        private void AddSingleLink(ContentBlockSettingListItem l)
        {
            this.MenuControl.Controls.Add(new LiteralControl("<li>"));
            HyperLink m = new HyperLink();
            m.ToolTip = l.Setting4;
            m.Text = l.Setting1;
            m.NavigateUrl = l.Setting2;
            if (l.Setting3 == "1")
            {
                m.Target = "_blank";
            }
            m.EnableViewState = false;
            this.MenuControl.Controls.Add(m);
            this.MenuControl.Controls.Add(new LiteralControl("</li>"));
        }
    }
}