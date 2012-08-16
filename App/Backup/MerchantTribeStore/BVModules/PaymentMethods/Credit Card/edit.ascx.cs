using System;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Payment;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVModules_PaymentMethods_Credit_Card_edit : BVModule
    {

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.NotifyFinishedEditing();
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SaveData();
            this.NotifyFinishedEditing();
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                PopulateGatewaylist();
                LoadData();
            }
        }

        private void PopulateGatewaylist()
        {
            MerchantTribe.Commerce.Accounts.Store s = MyPage.MTApp.CurrentStore;
            if (s.PlanId == 0)
            {
                this.lstGateway.DataSource = MerchantTribe.Payment.Methods.AvailableMethod.FindAllPayPalOnly();
            }
            else
            {
                this.lstGateway.DataSource = MerchantTribe.Payment.Methods.AvailableMethod.FindAll();
            }
            this.lstGateway.DataValueField = "Id";
            this.lstGateway.DataTextField = "Name";
            this.lstGateway.DataBind();
        }

        private void LoadData()
        {
            this.lstCaptureMode.ClearSelection();
            if (MyPage.MTApp.CurrentStore.Settings.PaymentCreditCardAuthorizeOnly == true)
            {
                this.lstCaptureMode.SelectedValue = "1";
            }
            else
            {
                this.lstCaptureMode.SelectedValue = "0";
            }

            this.chkRequireCreditCardSecurityCode.Checked = MyPage.MTApp.CurrentStore.Settings.PaymentCreditCardRequireCVV;

            this.lstGateway.ClearSelection();
            this.lstGateway.SelectedValue = MyPage.MTApp.CurrentStore.Settings.PaymentCreditCardGateway;

            List<CardType> acceptedCards = MyPage.MTApp.CurrentStore.Settings.PaymentAcceptedCards;
            foreach (CardType t in acceptedCards)
            {
                switch (t)
                {
                    case CardType.Amex:
                        chkCardAmex.Checked = true;
                        break;
                    case CardType.DinersClub:
                        chkCardDiners.Checked = true;
                        break;
                    case CardType.Discover:
                        chkCardDiscover.Checked = true;
                        break;
                    case CardType.JCB:
                        chkCardJCB.Checked = true;
                        break;
                    case CardType.MasterCard:
                        chkCardMasterCard.Checked = true;
                        break;
                    case CardType.Visa:
                        chkCardVisa.Checked = true;
                        break;
                }
            }
        }

        private void SaveData()
        {
            if (this.lstCaptureMode.SelectedValue == "1")
            {
                MyPage.MTApp.CurrentStore.Settings.PaymentCreditCardAuthorizeOnly = true;
            }
            else
            {
                MyPage.MTApp.CurrentStore.Settings.PaymentCreditCardAuthorizeOnly = false;
            }
            MyPage.MTApp.CurrentStore.Settings.PaymentCreditCardRequireCVV = this.chkRequireCreditCardSecurityCode.Checked;
            MyPage.MTApp.CurrentStore.Settings.PaymentCreditCardGateway = this.lstGateway.SelectedValue;

            // Save Credit Card Types
            List<CardType> acceptedCards = new List<CardType>();
            if (chkCardAmex.Checked) acceptedCards.Add(CardType.Amex);
            if (chkCardVisa.Checked) acceptedCards.Add(CardType.Visa);
            if (chkCardMasterCard.Checked) acceptedCards.Add(CardType.MasterCard);
            if (chkCardDiscover.Checked) acceptedCards.Add(CardType.Discover);
            if (chkCardDiners.Checked) acceptedCards.Add(CardType.DinersClub);
            if (chkCardJCB.Checked) acceptedCards.Add(CardType.JCB);
            MyPage.MTApp.CurrentStore.Settings.PaymentAcceptedCards = acceptedCards;
            MyPage.MTApp.AccountServices.Stores.Update(MyPage.MTApp.CurrentStore);

        }

        protected void btnOptions_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SaveData();
            Response.Redirect("Payment_Edit_Gateway.aspx?id=" + this.lstGateway.SelectedValue + "&payid=" + this.BlockId);
        }
       
    }
}