using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment.Methods
{
    public class StripeSettings : MethodSettings
    {
        public string StripeApiKey
        {
            get { return GetSettingOrEmpty("StripeApiKey"); }
            set { this.AddOrUpdate("StripeApiKey", value); }
        }
        public string CurrencyCode
        {
            get { string temp= GetSettingOrEmpty("CurrencyCode");
                if (temp.Length < 3) { temp = "usd"; }
                return temp;
            }
            set { this.AddOrUpdate("CurrencyCode", value); }
        }
        public bool DeveloperMode
        {
            get { return GetBoolSetting("DeveloperMode"); }
            set { SetBoolSetting("DeveloperMode", value); }
        }
    }
}
