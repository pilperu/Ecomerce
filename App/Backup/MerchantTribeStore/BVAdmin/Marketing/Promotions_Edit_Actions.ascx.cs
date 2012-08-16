using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Marketing;
using MerchantTribe.Commerce.Marketing.PromotionActions;
using MerchantTribe.Commerce.Content;
using System.Collections.ObjectModel;

namespace MerchantTribeStore.BVAdmin.Marketing
{
    public partial class Promotions_Edit_Actions : BVUserControl
    {
        private Promotion GetCurrentPromotion()
        {
            string promoid = this.promotionid.Value;
            long pid = 0;
            long.TryParse(promoid, out pid);
            Promotion p = MyPage.MTApp.MarketingServices.Promotions.Find(pid);
            return p;
        }
        private IPromotionAction GetCurrentAction(Promotion p)
        {
            if (p == null) return null;
            string itemId = this.itemid.Value;
            long temp = 0;
            long.TryParse(itemId, out temp);
            IPromotionAction a = p.GetAction(temp);
            return a;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void LoadAction(Promotion p, string id)
        {
            if (p == null) return;
            this.itemid.Value = id;
            this.promotionid.Value = p.Id.ToString();

            LoadCorrectEditor();
        }

        private void LoadCorrectEditor()
        {
            Promotion p = GetCurrentPromotion();
            IPromotionAction a = GetCurrentAction(p);
            if (a == null) return;

            switch (a.TypeId.ToString().ToUpper())
            {
                case PromotionActionBase.TypeIdProductPriceAdjustment:
                    this.MultiView1.SetActiveView(this.viewAdjustProductPrice);                    
                    this.AmountField.Text = ((ProductPriceAdjustment)a).Amount.ToString();
                    if (((ProductPriceAdjustment)a).AdjustmentType == AmountTypes.Percent)
                    {
                        this.lstAdjustmentType.ClearSelection();
                        this.lstAdjustmentType.Items.FindByValue("1").Selected = true;
                    }
                    else
                    {
                        this.lstAdjustmentType.ClearSelection();
                        this.lstAdjustmentType.Items.FindByValue("0").Selected = true;
                    }
                    break;
                case PromotionActionBase.TypeIdOrderTotalAdjustment:
                    this.MultiView1.SetActiveView(this.viewAdjustOrderTotal);
                    this.OrderTotalAmountField.Text = ((OrderTotalAdjustment)a).Amount.ToString();
                    if (((OrderTotalAdjustment)a).AdjustmentType == AmountTypes.Percent)
                    {
                        this.lstOrderTotalAdjustmentType.ClearSelection();
                        this.lstOrderTotalAdjustmentType.Items.FindByValue("1").Selected = true;
                    }
                    else
                    {
                        this.lstOrderTotalAdjustmentType.ClearSelection();
                        this.lstOrderTotalAdjustmentType.Items.FindByValue("0").Selected = true;
                    }                    
                    break;
                case PromotionActionBase.TypeIdOrderShippingAdjustment:
                    this.MultiView1.SetActiveView(this.viewOrderShippingAdjustment);
                    this.OrderShippingAdjustmentAmount.Text = ((OrderShippingAdjustment)a).Amount.ToString();
                    if (((OrderShippingAdjustment)a).AdjustmentType == AmountTypes.Percent)
                    {
                        this.lstOrderShippingAdjustmentType.ClearSelection();
                        this.lstOrderShippingAdjustmentType.Items.FindByValue("1").Selected = true;
                    }
                    else
                    {
                        this.lstOrderShippingAdjustmentType.ClearSelection();
                        this.lstOrderShippingAdjustmentType.Items.FindByValue("0").Selected = true;
                    }
                    break;
            }
        }

        public bool SaveAction()
        {
            Promotion p = GetCurrentPromotion();
            if (p == null) return false;
            IPromotionAction a = GetCurrentAction(p);
            if (a == null) return false;

            return SaveEditor(p, a);
        }

        private bool SaveEditor(Promotion p, IPromotionAction a)
        {
            switch (a.TypeId.ToString().ToUpper())
            {
                case PromotionActionBase.TypeIdProductPriceAdjustment:
                    decimal adjustmentTemp = ((ProductPriceAdjustment)a).Amount;
                    decimal parsedAdjustment = 0;
                    if (decimal.TryParse(this.AmountField.Text, out parsedAdjustment))
                    {
                        adjustmentTemp = parsedAdjustment;
                    }
                    ((ProductPriceAdjustment)a).Amount = adjustmentTemp;
                    if (this.lstAdjustmentType.SelectedValue == "1")
                    {
                        ((ProductPriceAdjustment)a).AdjustmentType = AmountTypes.Percent;
                    }
                    else
                    {
                        ((ProductPriceAdjustment)a).AdjustmentType = AmountTypes.MonetaryAmount;
                    }
                    return MyPage.MTApp.MarketingServices.Promotions.Update(p);
                case PromotionActionBase.TypeIdOrderTotalAdjustment:
                    decimal adjustmentTemp2 = ((OrderTotalAdjustment)a).Amount;
                    decimal parsedAdjustment2 = 0;
                    if (decimal.TryParse(this.OrderTotalAmountField.Text, out parsedAdjustment2))
                    {
                        adjustmentTemp2 = parsedAdjustment2;
                    }
                    ((OrderTotalAdjustment)a).Amount = adjustmentTemp2;
                    if (this.lstOrderTotalAdjustmentType.SelectedValue == "1")
                    {
                        ((OrderTotalAdjustment)a).AdjustmentType = AmountTypes.Percent;
                    }
                    else
                    {
                        ((OrderTotalAdjustment)a).AdjustmentType = AmountTypes.MonetaryAmount;
                    }
                    return MyPage.MTApp.MarketingServices.Promotions.Update(p);
                case PromotionActionBase.TypeIdOrderShippingAdjustment:
                    decimal adjustmentOrderShipping = ((OrderShippingAdjustment)a).Amount;
                    decimal parsedAdjustmentOrderShipping = 0;
                    if (decimal.TryParse(this.OrderShippingAdjustmentAmount.Text, out parsedAdjustmentOrderShipping))
                    {
                        adjustmentOrderShipping = parsedAdjustmentOrderShipping;
                    }
                    ((OrderShippingAdjustment)a).Amount = adjustmentOrderShipping;
                    if (this.lstOrderShippingAdjustmentType.SelectedValue == "1")
                    {
                        ((OrderShippingAdjustment)a).AdjustmentType = AmountTypes.Percent;
                    }
                    else
                    {
                        ((OrderShippingAdjustment)a).AdjustmentType = AmountTypes.MonetaryAmount;
                    }
                    return MyPage.MTApp.MarketingServices.Promotions.Update(p);       
            }

            return false;
        }

        private class FriendlyBvinDisplay
        {
            public string bvin { get; set; }
            public string DisplayName { get; set; }
        }
    }
}