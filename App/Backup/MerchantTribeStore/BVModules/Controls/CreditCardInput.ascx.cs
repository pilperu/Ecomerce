
using MerchantTribe.Commerce;
using System.Collections.ObjectModel;
using MerchantTribe.Web.Validation;
using System.Collections.Generic;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Payment;

namespace MerchantTribeStore
{

    partial class BVModules_Controls_CreditCardInput : MerchantTribe.Commerce.Content.BVUserControl, MerchantTribe.Web.Validation.IValidatable
    {

        private int _tabIndex = -1;

        public string CardNumber
        {
            get
            {
                string result = this.cccardnumber.Text.Trim();
                if (result.StartsWith("*") == false)
                {
                    result = MerchantTribe.Payment.CardValidator.CleanCardNumber(result);
                }
                return result;
            }
            set { this.cccardnumber.Text = value; }
        }
        public string CardHolderName
        {
            get { return this.cccardholder.Text.Trim(); }
            set { this.cccardholder.Text = value; }
        }
        public string SecurityCode
        {
            get { return this.ccsecuritycode.Text.Trim(); }
            set { this.ccsecuritycode.Text = value; }
        }
        public string CardCode
        {
            get
            {
                MerchantTribe.Payment.CardType result = MerchantTribe.Payment.CardValidator.GetCardTypeFromNumber(this.CardNumber);
                switch (result)
                {
                    case MerchantTribe.Payment.CardType.Amex:
                        return "A";
                    case MerchantTribe.Payment.CardType.DinersClub:
                        return "C";
                    case MerchantTribe.Payment.CardType.Discover:
                        return "D";
                    case MerchantTribe.Payment.CardType.JCB:
                        return "J";
                    case MerchantTribe.Payment.CardType.Maestro:
                        return "MAESTRO";
                    case MerchantTribe.Payment.CardType.MasterCard:
                        return "M";
                    case MerchantTribe.Payment.CardType.Solo:
                        return "SOLO";
                    case MerchantTribe.Payment.CardType.Switch:
                        return "SWITCH";
                    case MerchantTribe.Payment.CardType.Visa:
                        return "V";
                    default:
                        return "UNKNOWN";
                }
            }
        }
        public int ExpirationMonth
        {
            get { return int.Parse(this.ccexpmonth.SelectedValue); }
            set
            {
                if (this.ccexpmonth.Items.FindByValue(value.ToString()) != null)
                {
                    this.ccexpmonth.ClearSelection();
                    this.ccexpmonth.Items.FindByValue(value.ToString()).Selected = true;
                }
            }
        }
        public int ExpirationYear
        {
            get { return int.Parse(this.ccexpyear.SelectedValue); }
            set
            {
                if (this.ccexpyear.Items.FindByValue(value.ToString()) != null)
                {
                    this.ccexpyear.ClearSelection();
                    this.ccexpyear.Items.FindByValue(value.ToString()).Selected = true;
                }
            }
        }
        public int TabIndex
        {
            get { return _tabIndex; }
            set { _tabIndex = value; }
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            InitializeFields();
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (this.Page is BaseAdminPage)
            {
                //this.cvvdesclink.Visible = false;
            }
            if (TabIndex != -1)
            {
                cccardnumber.TabIndex = (short)(this.TabIndex + 1);
                ccexpmonth.TabIndex = (short)(this.TabIndex + 2);
                ccexpyear.TabIndex = (short)(this.TabIndex + 3);
                ccsecuritycode.TabIndex = (short)(this.TabIndex + 4);
                cccardholder.TabIndex = (short)(this.TabIndex + 5);
            }

            DisplayAcceptedCards();
        }

        private void DisplayAcceptedCards()
        {
            foreach (CardType t in MyPage.MTApp.CurrentStore.Settings.PaymentAcceptedCards)
            {
                switch (t)
                {
                    case CardType.Visa:
                        this.litCardsAccepted.Text += "<span class=\"cc-visa\"></span>";
                        break;
                    case CardType.MasterCard:
                        this.litCardsAccepted.Text += "<span class=\"cc-mastercard\"></span>";
                        break;
                    case CardType.Amex:
                        this.litCardsAccepted.Text += "<span class=\"cc-amex\"></span>";
                        break;
                    case CardType.Discover:
                        this.litCardsAccepted.Text += "<span class=\"cc-discover\"></span>";
                        break;
                    case CardType.DinersClub:
                        this.litCardsAccepted.Text += "<span class=\"cc-diners\"></span>";
                        break;
                    case CardType.JCB:
                        this.litCardsAccepted.Text += "<span class=\"cc-jcb\"></span>";
                        break;
                }
            }
        }

        public void InitializeFields()
        {
            LoadMonths();
            LoadYears();
        }

        private void LoadYears()
        {
            ccexpyear.Items.Clear();
            ccexpyear.Items.Add(new System.Web.UI.WebControls.ListItem("----", "0"));

            int CurrentYear = System.DateTime.UtcNow.ToLocalTime().Year;
            for (int iTempCounter = 0; iTempCounter <= 9; iTempCounter += 1)
            {
                System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem();
                liTemp.Text = (iTempCounter + CurrentYear).ToString();
                liTemp.Value = (iTempCounter + CurrentYear).ToString();
                ccexpyear.Items.Add(liTemp);
                liTemp = null;
            }
        }

        private void LoadMonths()
        {
            ccexpmonth.Items.Clear();
            ccexpmonth.Items.Add(new System.Web.UI.WebControls.ListItem("--", "0"));
            for (int iTempCounter = 1; iTempCounter <= 12; iTempCounter += 1)
            {
                System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem();
                liTemp.Text = iTempCounter.ToString();
                liTemp.Value = iTempCounter.ToString();
                ccexpmonth.Items.Add(liTemp);
                liTemp = null;
            }
        }

        public void LoadFromPayment(OrderTransaction t)
        {
            LoadFromCardData(t.CreditCard);
        }

        public void LoadFromCardData(MerchantTribe.Payment.CardData card)
        {
            ExpirationMonth = card.ExpirationMonth;
            ExpirationYear = card.ExpirationYear;
            CardHolderName = card.CardHolderName;
            if (card.CardNumber.Trim().Length >= 4)
            {
                CardNumber = "****-****-****-" + card.CardNumberLast4Digits;
            }

            //if (op.CustomPropertyExists("bvsoftware", "issuenumber")) {
            //    ccissuenumber.Text = op.CustomPropertyGet("bvsoftware", "issuenumber");
            //}
        }

        public void CopyToPayment(OrderTransaction ot)
        {
            ot.CreditCard.ExpirationMonth = ExpirationMonth;
            ot.CreditCard.ExpirationYear = ExpirationYear;
            ot.CreditCard.CardHolderName = CardHolderName;
            if (CardNumber.StartsWith("*") == false)
            {
                ot.CreditCard.CardNumber = CardNumber;
            }
            ot.CreditCard.SecurityCode = SecurityCode;
            //if (ccissuenumber.Text.Trim() != string.Empty) {
            //    ot.CustomPropertySet("bvsoftware", "issuenumber", ccissuenumber.Text);
            //}
        }

        public MerchantTribe.Payment.CardData GetCardData()
        {
            MerchantTribe.Payment.CardData result = new MerchantTribe.Payment.CardData();
            result.CardHolderName = CardHolderName;
            if (CardNumber.StartsWith("*"))
            {
                result.CardNumber = "";
            }
            else
            {
                result.CardNumber = CardNumber;
            }
            result.SecurityCode = SecurityCode;
            result.ExpirationMonth = ExpirationMonth;
            result.ExpirationYear = ExpirationYear;

            return result;
        }

        public System.Collections.Generic.List<MerchantTribe.Web.Validation.RuleViolation> GetRuleViolations()
        {
            List<MerchantTribe.Web.Validation.RuleViolation> violations = new List<MerchantTribe.Web.Validation.RuleViolation>();

            // Card Number
            if (CardNumber.StartsWith("****-****-****-"))
            {
            }
            // Ignore for now
            //If paymentId.Trim.Length > 0 Then
            //End If
            //For i As Integer = 0 To thisOrder.Payments.Length - 1
            //    With thisOrder.Payments(i)
            //        If .PaymentType = BVSoftware.BVC.Interfaces.PaymentRecordType.Information Then
            //            If .PaymentMethod = BVSoftware.BVC.Interfaces.PaymentMethod.CreditCard Then
            //                testCardNumber = .CreditCardNumber
            //            End If
            //        End If
            //    End With
            //Next
            else
            {
                CardNumber = MerchantTribe.Payment.CardValidator.CleanCardNumber(CardNumber);
            }

            if ((!MerchantTribe.Payment.CardValidator.IsCardNumberValid(CardNumber)))
            {
                violations.Add(new RuleViolation("Credit Card Number", "", "Please enter a valid credit card number", "cccardnumber"));
            }


            MerchantTribe.Payment.CardType cardTypeCheck = MerchantTribe.Payment.CardValidator.GetCardTypeFromNumber(this.CardNumber);
            List<CardType> acceptedCards = MyPage.MTApp.CurrentStore.Settings.PaymentAcceptedCards;
            if (!acceptedCards.Contains(cardTypeCheck))
            {
                violations.Add(new RuleViolation("Card Type Not Accepted", "", "That card type is not accepted by this store. Please use a different card.", "cccardnumber"));
            }

            ValidationHelper.RequiredMinimum(1, "Card Expiration Year", ExpirationYear, violations, "ccexpyear");
            ValidationHelper.RequiredMinimum(1, "Card Expiration Month", ExpirationMonth, violations, "ccexpmonth");
            ValidationHelper.Required("Name on Card", CardHolderName, violations, "cccardholder");

            if (MyPage.MTApp.CurrentStore.Settings.PaymentCreditCardRequireCVV == true)
            {
                ValidationHelper.RequiredMinimum(3, "Card Security Code", SecurityCode.Length, violations, "ccsecuritycode");
            }

            SetErrorCss(violations);

            return violations;
        }

        public bool IsValid()
        {
            if (GetRuleViolations().Count > 0)
            {
                return false;
            }
            return true;
        }

        public void ClearCssViolations()
        {
            SetErrorCss(new List<MerchantTribe.Web.Validation.RuleViolation>());
        }

        private void SetErrorCss(List<MerchantTribe.Web.Validation.RuleViolation> violations)
        {

            // Clear Out Previous Error Classes
            this.cccardnumber.CssClass = "";
            this.cccardholder.CssClass = "";
            this.ccexpyear.CssClass = "";
            this.ccexpmonth.CssClass = "";
            this.ccsecuritycode.CssClass = "";
            this.ccissuenumber.CssClass = "";

            // Tag controls with violations with CSS class
            foreach (MerchantTribe.Web.Validation.RuleViolation v in violations)
            {
                System.Web.UI.WebControls.WebControl cntrl = (System.Web.UI.WebControls.WebControl)this.FindControl(v.ControlName);
                if ((cntrl != null))
                {
                    cntrl.CssClass = "input-validation-error";
                }
            }

        }

    }
}