using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Marketing;
using MerchantTribe.Commerce.Marketing.PromotionQualifications;
using MerchantTribe.Commerce.Content;
using System.Collections.ObjectModel;
using MerchantTribe.Commerce.Shipping;

namespace MerchantTribeStore.BVAdmin.Marketing
{
    public partial class Promotions_Edit_Qualification : BVUserControl
    {

        private Promotion GetCurrentPromotion()
        {            
            string promoid = this.promotionid.Value;
            long pid = 0;
            long.TryParse(promoid, out pid);
            Promotion p = MyPage.MTApp.MarketingServices.Promotions.Find(pid);
            return p;            
        }
        private IPromotionQualification GetCurrentQualification(Promotion p)
        {
            if (p == null) return null;
            string itemId = this.itemid.Value;
            long temp = 0;
            long.TryParse(itemId, out temp);
            IPromotionQualification q = p.GetQualification(temp);
            return q;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.UserPicker1.UserSelected += new BVAdmin_Controls_UserPicker.UserSelectedDelegate(UserPicker1_UserSelected);
            this.UserPicker1.MessageBox = this.MessageBox1;          
        }
        
        public void LoadQualification(Promotion p, string id)
        {
            if (p == null) return;
            this.itemid.Value = id;
            this.promotionid.Value = p.Id.ToString();

            LoadCorrectEditor();
        }

        private void LoadCorrectEditor()
        {
            Promotion p = GetCurrentPromotion();
            IPromotionQualification q = GetCurrentQualification(p);
            if (q == null) return;
            
            this.MultiView1.Visible = true;

            switch (q.TypeId.ToString().ToUpper())
            {
                case PromotionQualificationBase.TypeIdAnyProduct:
                    this.MessageBox1.ShowInformation("This qualification does not have any configuration options.");
                    this.MultiView1.Visible = false;
                    break;
                case PromotionQualificationBase.TypeIdProductBvin:
                    this.MultiView1.SetActiveView(this.viewProductBvin);
                    break;
                case PromotionQualificationBase.TypeIdProductCategory:
                    this.MultiView1.SetActiveView(this.viewProductCategory);
                    LoadProductCategoryEditor((ProductCategory)q);
                    break;
                case PromotionQualificationBase.TypeIdProductType:
                    this.MultiView1.SetActiveView(this.viewProductType);
                    LoadProductTypeEditor((ProductType)q);                    
                    break;
                case PromotionQualificationBase.TypeIdOrderHasCoupon:
                    this.MultiView1.SetActiveView(this.viewOrderHasCoupon);
                    LoadOrderHasCouponEditor((OrderHasCoupon)q);
                    break;
                case PromotionQualificationBase.TypeIdAnyOrder:
                    this.MessageBox1.ShowInformation("This qualification does not have any configuration options.");
                    this.MultiView1.Visible = false;
                    break;
                case PromotionQualificationBase.TypeIdOrderSubTotalIs:
                    this.MultiView1.SetActiveView(this.viewOrderSubTotalIs);
                    LoadOrderSubTotalIsEditor((OrderSubTotalIs)q);
                    break;
                case PromotionQualificationBase.TypeIdOrderHasProducts:
                    this.MultiView1.SetActiveView(this.viewOrderHasProduct);
                    LoadOrderProductEditor((OrderHasProducts)q);
                    break;
                case PromotionQualificationBase.TypeIdUserIs:
                    this.MultiView1.SetActiveView(this.viewUserId);
                    LoadUserIsEditor((UserIs)q);
                    break;
                case PromotionQualificationBase.TypeIdUserIsInGroup:
                    this.MultiView1.SetActiveView(this.viewUserIsInGroup);
                    LoadUserIsInGroupEditor((UserIsInGroup)q);
                    break;
                case PromotionQualificationBase.TypeIdAnyShippingMethod:
                    this.MessageBox1.ShowInformation("Any shipping method does not have any configuration options.");
                    this.MultiView1.Visible = false;
                    break;
                case PromotionQualificationBase.TypeIdShippingMethodIs:
                    this.MultiView1.SetActiveView(viewShippingMethodIs);
                    LoadShippingMethodIsEditor((ShippingMethodIs)q);
                    break;
            }
        }

        public bool SaveQualification()
        {
            Promotion p = GetCurrentPromotion();
            if (p == null) return false;
            IPromotionQualification q = GetCurrentQualification(p);
            if (q == null) return false;

            return SaveEditor(p, q);            
        }

        private bool SaveEditor(Promotion p, IPromotionQualification q)
        {
            switch (q.TypeId.ToString().ToUpper())
            {
                case PromotionQualificationBase.TypeIdAnyProduct: // all saved at time of edit, no extra save here
                case PromotionQualificationBase.TypeIdProductBvin:
                case PromotionQualificationBase.TypeIdProductCategory:
                case PromotionQualificationBase.TypeIdProductType:
                case PromotionQualificationBase.TypeIdUserIs:
                case PromotionQualificationBase.TypeIdUserIsInGroup:                                    
                case PromotionQualificationBase.TypeIdOrderHasCoupon:
                case PromotionQualificationBase.TypeIdAnyShippingMethod:
                case PromotionQualificationBase.TypeIdShippingMethodIs:
                case PromotionQualificationBase.TypeIdAnyOrder:
                    return true;
                case PromotionQualificationBase.TypeIdOrderSubTotalIs:
                    decimal ototal = ((OrderSubTotalIs)q).Amount;
                    decimal parsedototal = 0;
                    if (decimal.TryParse(this.OrderSubTotalIsField.Text, out parsedototal))
                    {
                        ototal = parsedototal;
                    }
                    ((OrderSubTotalIs)q).Amount = ototal;                    
                    return MyPage.MTApp.MarketingServices.Promotions.Update(p);                      
                case PromotionQualificationBase.TypeIdOrderHasProducts:
                    int qty1 = ((OrderHasProducts)q).Quantity;
                    int parsedqty1 = 1;
                    if (int.TryParse(this.OrderProductQuantityField.Text, out parsedqty1))
                    {
                        qty1 = parsedqty1;
                    }
                    QualificationSetMode setmode = ((OrderHasProducts)q).SetMode;
                    int parsedsetmode = 1;
                    if (int.TryParse(this.lstOrderProductSetMode.SelectedValue, out parsedsetmode))
                    {
                        setmode = (QualificationSetMode)parsedsetmode;
                    }
                    ((OrderHasProducts)q).Quantity = qty1;
                    ((OrderHasProducts)q).SetMode = setmode;
                    return MyPage.MTApp.MarketingServices.Promotions.Update(p);                    
            }

            return false;
        }

        private class FriendlyBvinDisplay
        {
            public string bvin { get; set; }
            public string DisplayName { get; set; }
        }

         // Product Type Editor        
        private void LoadProductTypeEditor(ProductType q)
        {
            List<MerchantTribe.Commerce.Catalog.ProductType> allTypes = MyPage.MTApp.CatalogServices.ProductTypes.FindAll();
            allTypes.Insert(0, new MerchantTribe.Commerce.Catalog.ProductType() { Bvin = "0", ProductTypeName = "Generic" });

            List<FriendlyBvinDisplay> displayData = new List<FriendlyBvinDisplay>();

            foreach (string bvin in q.CurrentTypeIds())
            {
                FriendlyBvinDisplay item = new FriendlyBvinDisplay();
                item.bvin = bvin;
                item.DisplayName = bvin;

                var t = allTypes.Where(y => y.Bvin == bvin).FirstOrDefault();
                if (t != null)
                {                    
                    item.DisplayName = t.ProductTypeName;
                    allTypes.Remove(t);
                }
                displayData.Add(item);
            }

            this.lstProductTypes.DataSource = allTypes;
            this.lstProductTypes.DataValueField = "Bvin";
            this.lstProductTypes.DataTextField = "ProductTypeName";
            this.lstProductTypes.DataBind();

            this.gvProductTypes.DataSource = displayData;
            this.gvProductTypes.DataBind();
        }        
        protected void btnAddProductType_Click(object sender, ImageClickEventArgs e)
        {
            Promotion p = GetCurrentPromotion();
            ProductType q = (ProductType)GetCurrentQualification(p);
            if (q == null) return;
            q.AddProductType(this.lstProductTypes.SelectedValue);
            MyPage.MTApp.MarketingServices.Promotions.Update(p);
            LoadProductTypeEditor(q);
        }
        protected void gvProductTypes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Promotion p = GetCurrentPromotion();
            ProductType q = (ProductType)GetCurrentQualification(p);
            if (q == null) return;
            string bvin = (string)e.Keys[0];
            q.RemoveProductType(bvin);            
            MyPage.MTApp.MarketingServices.Promotions.Update(p);
            LoadProductTypeEditor(q);
        }

        // Product Category Editor
        private void LoadProductCategoryEditor(ProductCategory q)
        {

            List<MerchantTribe.Commerce.Catalog.CategorySnapshot> allCats = MyPage.MTApp.CatalogServices.Categories.FindAll();
            Collection<System.Web.UI.WebControls.ListItem> available = MerchantTribe.Commerce.Catalog.Category.ListFullTreeWithIndents(allCats, true);

            List<FriendlyBvinDisplay> displayData = new List<FriendlyBvinDisplay>();

            foreach (string bvin in q.CurrentCategoryIds())
            {
                FriendlyBvinDisplay item = new FriendlyBvinDisplay();
                item.bvin = bvin;
                item.DisplayName = bvin;

                var t = available.Where(y => y.Value == bvin).FirstOrDefault();
                if (t != null)
                {
                    item.DisplayName = t.Text;
                    available.Remove(t);
                }
                displayData.Add(item);
            }

            this.lstProductCategories.Items.Clear();            
            foreach (System.Web.UI.WebControls.ListItem li in available)
            {
                this.lstProductCategories.Items.Add(li);
            }

            this.gvProductCategories.DataSource = displayData;
            this.gvProductCategories.DataBind();
        }
        protected void btnAddProductCategory_Click(object sender, ImageClickEventArgs e)
        {
            Promotion p = GetCurrentPromotion();
            ProductCategory q = (ProductCategory)GetCurrentQualification(p);
            if (q == null) return;
            q.AddCategoryId(this.lstProductCategories.SelectedValue);
            MyPage.MTApp.MarketingServices.Promotions.Update(p);
            LoadProductCategoryEditor(q);
        }
        protected void gvProductCategories_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Promotion p = GetCurrentPromotion();
            ProductCategory q = (ProductCategory)GetCurrentQualification(p);
            if (q == null) return;
            string bvin = (string)e.Keys[0];
            q.RemoveCategoryId(bvin);
            MyPage.MTApp.MarketingServices.Promotions.Update(p);
            LoadProductCategoryEditor(q);
        }

        // Product Bvin Editor
        private void LoadProductBvinEditor(ProductBvin q)
        {
            this.ProductPicker1.LoadSearch();
            
            List<FriendlyBvinDisplay> displayData = new List<FriendlyBvinDisplay>();

            foreach (string bvin in q.CurrentProductIds())
            {
                FriendlyBvinDisplay item = new FriendlyBvinDisplay();
                item.bvin = bvin;
                item.DisplayName = bvin;

                MerchantTribe.Commerce.Catalog.Product p = MyPage.MTApp.CatalogServices.Products.Find(item.bvin);
                if (p != null)
                {
                    item.DisplayName = "[" + p.Sku + "] " + p.ProductName;                    
                }
                displayData.Add(item);
            }

            this.gvProductBvins.DataSource = displayData;
            this.gvProductBvins.DataBind();
        }
        protected void gvProductBvins_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Promotion p = GetCurrentPromotion();
            ProductBvin q = (ProductBvin)GetCurrentQualification(p);
            if (q == null) return;
            string bvin = (string)e.Keys[0];
            q.RemoveProductBvin(bvin);
            MyPage.MTApp.MarketingServices.Promotions.Update(p);
            LoadProductBvinEditor(q);
        }        
        protected void btnAddProductBvins_Click(object sender, ImageClickEventArgs e)
        {
            Promotion p = GetCurrentPromotion();
            ProductBvin q = (ProductBvin)GetCurrentQualification(p);
            if (q == null) return;
            foreach (string bvin in this.ProductPicker1.SelectedProducts)
            {
                q.AddProductBvin(bvin);
            }            
            MyPage.MTApp.MarketingServices.Promotions.Update(p);
            LoadProductBvinEditor(q);
        }
                
        // Order has Coupon Editor
        private void LoadOrderHasCouponEditor(OrderHasCoupon q)
        {
            List<FriendlyBvinDisplay> displayData = new List<FriendlyBvinDisplay>();

            foreach (string coupon in q.CurrentCoupons())
            {
                FriendlyBvinDisplay item = new FriendlyBvinDisplay();
                item.bvin = coupon;
                item.DisplayName = coupon;
                displayData.Add(item);
            }
            this.gvOrderCoupons.DataSource = displayData;
            this.gvOrderCoupons.DataBind();
        }
        protected void btnAddOrderCoupon_Click(object sender, ImageClickEventArgs e)
        {
            string code = this.OrderCouponField.Text.Trim();
            
            Promotion p = GetCurrentPromotion();
            OrderHasCoupon q = (OrderHasCoupon)GetCurrentQualification(p);
            if (q == null) return;

            q.AddCoupon(code);            
            MyPage.MTApp.MarketingServices.Promotions.Update(p);
            LoadOrderHasCouponEditor(q);
        }
        protected void gvOrderCoupons_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Promotion p = GetCurrentPromotion();
            OrderHasCoupon q = (OrderHasCoupon)GetCurrentQualification(p);
            if (q == null) return;
            string coupon = (string)e.Keys[0];
            q.RemoveCoupon(coupon);
            MyPage.MTApp.MarketingServices.Promotions.Update(p);
            LoadOrderHasCouponEditor(q);
        }

        // Order has Products Editor
        private void LoadOrderProductEditor(OrderHasProducts q)
        {
            this.ProductPickerOrderProducts.LoadSearch();

            List<FriendlyBvinDisplay> displayData = new List<FriendlyBvinDisplay>();

            foreach (string bvin in q.CurrentProductIds())
            {
                FriendlyBvinDisplay item = new FriendlyBvinDisplay();
                item.bvin = bvin;
                item.DisplayName = bvin;

                MerchantTribe.Commerce.Catalog.Product p = MyPage.MTApp.CatalogServices.Products.Find(item.bvin);
                if (p != null)
                {
                    item.DisplayName = "[" + p.Sku + "] " + p.ProductName;
                }
                displayData.Add(item);
            }

            this.gvOrderProducts.DataSource = displayData;
            this.gvOrderProducts.DataBind();
        }
        protected void gvOrderProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Promotion p = GetCurrentPromotion();
            OrderHasProducts q = (OrderHasProducts)GetCurrentQualification(p);
            if (q == null) return;
            string bvin = (string)e.Keys[0];
            q.RemoveProductBvin(bvin);
            MyPage.MTApp.MarketingServices.Promotions.Update(p);
            LoadOrderProductEditor(q);
        }
        protected void btnAddOrderProduct_Click(object sender, ImageClickEventArgs e)
        {
            Promotion p = GetCurrentPromotion();
            OrderHasProducts q = (OrderHasProducts)GetCurrentQualification(p);
            if (q == null) return;
            foreach (string bvin in this.ProductPickerOrderProducts.SelectedProducts)
            {
                q.AddProductBvin(bvin);
            }
            MyPage.MTApp.MarketingServices.Promotions.Update(p);
            LoadOrderProductEditor(q);
        }

        // Order Sub Total Is Editor
        private void LoadOrderSubTotalIsEditor(OrderSubTotalIs q)
        {
            this.OrderSubTotalIsField.Text = q.Amount.ToString();
        }

        // User Is Editor      
        private void LoadUserIsEditor(UserIs q)
        {            
            List<FriendlyBvinDisplay> displayData = new List<FriendlyBvinDisplay>();

            foreach (string bvin in q.UserIds())
            {
                FriendlyBvinDisplay item = new FriendlyBvinDisplay();
                item.bvin = bvin;
                item.DisplayName = bvin;

                MerchantTribe.Commerce.Membership.CustomerAccount c = MyPage.MTApp.MembershipServices.Customers.Find(item.bvin);
                if (c != null)
                {
                    item.DisplayName = c.Email;
                }
                displayData.Add(item);
            }

            this.gvUserIs.DataSource = displayData;
            this.gvUserIs.DataBind();
        }
        protected void gvUserIs_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Promotion p = GetCurrentPromotion();
            UserIs q = (UserIs)GetCurrentQualification(p);
            if (q == null) return;
            string bvin = (string)e.Keys[0];
            q.RemoveUserId(bvin);
            MyPage.MTApp.MarketingServices.Promotions.Update(p);
            LoadUserIsEditor(q);
        }        
        void UserPicker1_UserSelected(MerchantTribe.Commerce.Controls.UserSelectedEventArgs e)
        {
            Promotion p = GetCurrentPromotion();
            UserIs q = (UserIs)GetCurrentQualification(p);
            if (q == null) return;            
            q.AddUserId(e.UserAccount.Bvin);
            MyPage.MTApp.MarketingServices.Promotions.Update(p);
            LoadUserIsEditor(q);
        }

        // User Is in Group
        private void LoadUserIsInGroupEditor(UserIsInGroup q)
        {
            List<MerchantTribe.Commerce.Contacts.PriceGroup> allGroups = MyPage.MTApp.ContactServices.PriceGroups.FindAll();

            List<FriendlyBvinDisplay> displayData = new List<FriendlyBvinDisplay>();

            foreach (string bvin in q.CurrentGroupIds())
            {
                FriendlyBvinDisplay item = new FriendlyBvinDisplay();
                item.bvin = bvin;
                item.DisplayName = bvin;

                var t = allGroups.Where(y => y.Bvin == bvin).FirstOrDefault();
                if (t != null)
                {
                    item.DisplayName = t.Name;
                    allGroups.Remove(t);
                }
                displayData.Add(item);
            }

            this.lstUserIsInGroup.DataSource = allGroups;
            this.lstUserIsInGroup.DataValueField = "Bvin";
            this.lstUserIsInGroup.DataTextField = "Name";
            this.lstUserIsInGroup.DataBind();

            this.gvUserIsInGroup.DataSource = displayData;
            this.gvUserIsInGroup.DataBind();
        }
        protected void btnAddUserIsInGroup_Click(object sender, ImageClickEventArgs e)
        {
            Promotion p = GetCurrentPromotion();
            UserIsInGroup q = (UserIsInGroup)GetCurrentQualification(p);
            if (q == null) return;
            q.AddGroup(this.lstUserIsInGroup.SelectedValue);
            MyPage.MTApp.MarketingServices.Promotions.Update(p);
            LoadUserIsInGroupEditor(q);
        }
        protected void gvUserIsInGroup_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Promotion p = GetCurrentPromotion();
            UserIsInGroup q = (UserIsInGroup)GetCurrentQualification(p);
            if (q == null) return;
            string bvin = (string)e.Keys[0];
            q.RemoveGroup(bvin);
            MyPage.MTApp.MarketingServices.Promotions.Update(p);
            LoadUserIsInGroupEditor(q);
        }

        // Shipping Method Is
        private void LoadShippingMethodIsEditor(ShippingMethodIs q)
        {
            List<ShippingMethod> available = MyPage.MTApp.OrderServices.ShippingMethods.FindAll(MyPage.MTApp.CurrentStore.Id);
            
            List<FriendlyBvinDisplay> displayData = new List<FriendlyBvinDisplay>();

            foreach (string itemid in q.ItemIds())
            {
                FriendlyBvinDisplay item = new FriendlyBvinDisplay();
                item.bvin = itemid;
                item.DisplayName = itemid;

                var t = available.Where(y => y.Bvin == itemid).FirstOrDefault();
                if (t != null)
                {
                    item.DisplayName = t.Name;
                    available.Remove(t);
                }
                displayData.Add(item);
            }

            this.lstShippingMethodIs.Items.Clear();
            this.lstShippingMethodIs.DataSource = available;
            this.lstShippingMethodIs.DataTextField = "Name";
            this.lstShippingMethodIs.DataValueField = "Bvin";
            this.lstShippingMethodIs.DataBind();

            this.gvShippingMethodIs.DataSource = displayData;
            this.gvShippingMethodIs.DataBind();
        }
        protected void btnAddShippingMethodIs_Click(object sender, ImageClickEventArgs e)
        {
            Promotion p = GetCurrentPromotion();
            ShippingMethodIs q = (ShippingMethodIs)GetCurrentQualification(p);
            if (q == null) return;
            q.AddItemId(this.lstShippingMethodIs.SelectedValue);
            MyPage.MTApp.MarketingServices.Promotions.Update(p);
            LoadShippingMethodIsEditor(q);
        }
        protected void gvShippingMethodIs_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Promotion p = GetCurrentPromotion();
            ShippingMethodIs q = (ShippingMethodIs)GetCurrentQualification(p);
            if (q == null) return;
            string bvin = (string)e.Keys[0];
            q.RemoveItemId(bvin);
            MyPage.MTApp.MarketingServices.Promotions.Update(p);
            LoadShippingMethodIsEditor(q);
        }

    }
}