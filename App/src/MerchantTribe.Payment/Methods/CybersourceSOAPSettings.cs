using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment.Methods
{
    public class CybersourceSOAPSettings: MethodSettings
    {
        public string MerchantId
        {
            get { return GetSettingOrEmpty("MerchantId"); }
            set { this.AddOrUpdate("MerchantId", value); }
        }        
        public string TransactionKey
        {
            get { return GetSettingOrEmpty("TransactionKey"); }
            set { this.AddOrUpdate("TransactionKey", value); }
        }
        public bool TestMode
        {
            get { return GetBoolSetting("TestMode"); }
            set { SetBoolSetting("TestMode", value); }
        }
        public string CurrencyCode
        {
            get { return GetSettingOrEmpty("CurrencyCode"); }
            set { this.AddOrUpdate("CurrencyCode", value); }
        }
    }
}
