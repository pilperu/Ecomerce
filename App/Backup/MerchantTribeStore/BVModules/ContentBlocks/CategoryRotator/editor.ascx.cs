using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.BusinessRules;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Contacts;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Marketing;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Metrics;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce.Taxes;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore
{

    partial class BVModules_ContentBlocks_Category_Rotator_editor : BVModule
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadData();
            }
        }

        private void BindCategoryGridView(ContentBlock b)
        {
            List<ContentBlockSettingListItem> settings = b.Lists.FindList("Categories");
            Collection<Category> categories = new Collection<Category>();
            foreach (ContentBlockSettingListItem item in settings)
            {
                Category category = MyPage.MTApp.CatalogServices.Categories.Find(item.Setting1);
                if (category != null && category.Bvin != string.Empty)
                {
                    categories.Add(category);
                }
            }
            CategoriesGridView.DataSource = categories;
            CategoriesGridView.DataBind();
        }

        private void LoadData()
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            if (b != null)
            {
                this.chkShowInOrder.Checked = b.BaseSettings.GetBoolSetting("ShowInOrder");
            }
            BindCategoryGridView(b);
        }

        protected void AddImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            if (b != null)
            {

                List<ContentBlockSettingListItem> settings = b.Lists.FindList("Categories");

                foreach (string category in CategoryPicker1.SelectedCategories)
                {
                    bool @add = true;
                    foreach (ContentBlockSettingListItem item in settings)
                    {
                        if (item.Setting1 == category)
                        {
                            @add = false;
                            break;
                        }
                    }
                    if (@add)
                    {
                        ContentBlockSettingListItem c = new ContentBlockSettingListItem();
                        c.Setting1 = category;
                        c.ListName = "Categories";
                        b.Lists.AddItem(c);
                        MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);
                    }
                }
                BindCategoryGridView(b);
            }
        }

        protected void btnOK_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            if (b != null)
            {
                b.BaseSettings.SetBoolSetting("ShowInOrder", this.chkShowInOrder.Checked);
                MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);
            }
            this.NotifyFinishedEditing();
        }

        protected void CategoriesGridView_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            List<ContentBlockSettingListItem> settings = b.Lists.FindList("Categories");
            foreach (ContentBlockSettingListItem item in settings)
            {
                if (item.Setting1 == (string)CategoriesGridView.DataKeys[e.RowIndex].Value)
                {
                    b.Lists.RemoveItem(item.Id);
                }
            }
            MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);
            BindCategoryGridView(b);
        }

        protected void CategoriesGridView_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            List<ContentBlockSettingListItem> settings = b.Lists.FindList("Categories");

            string bvin = string.Empty;
            bvin = ((GridView)sender).DataKeys[e.RowIndex].Value.ToString();
            foreach (ContentBlockSettingListItem item in settings)
            {
                if (item.Setting1 == bvin)
                {
                    b.Lists.MoveItemUp(item.Id, "Categories");
                    break;
                }
            }
            MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);
            BindCategoryGridView(b);
        }

        protected void CategoriesGridView_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            List<ContentBlockSettingListItem> settings = b.Lists.FindList("Categories");

            string bvin = string.Empty;
            bvin = ((GridView)sender).DataKeys[e.RowIndex].Value.ToString();
            foreach (ContentBlockSettingListItem item in settings)
            {
                if (item.Setting1 == bvin)
                {
                    b.Lists.MoveItemDown(item.Id, "Categories");
                }
            }
            MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);
            BindCategoryGridView(b);
        }
    }
}