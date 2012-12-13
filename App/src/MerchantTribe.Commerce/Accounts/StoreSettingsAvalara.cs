using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Accounts
{
    public class StoreSettingsAvalara
    {
        private StoreSettings parent = null;

        public StoreSettingsAvalara(StoreSettings s)
        {
            parent = s;
        }

        public bool IsModuleActive
        {
            get
            {
                return parent.IsModuleActive("avatax");
            }
        }
        public bool Enabled
        {
            get { return parent.GetPropBool("AvalaraEnabled"); }
            set { parent.SetProp("AvalaraEnabled", value); }
        }
        public bool DebugMode
        {
            get { return parent.GetPropBool("AvalaraDebugMode"); }
            set { parent.SetProp("AvalaraDebugMode", value); }
        }
        public string Username
        {
            get { return parent.GetProp("AvalaraUsername"); }
            set { parent.SetProp("AvalaraUsername", value); }
        }
        public string Password
        {
            get { return parent.GetProp("AvalaraPassword"); }
            set { parent.SetProp("AvalaraPassword", value); }
        }
        public string ServiceUrl
        {
            get { return parent.GetProp("AvalaraServiceUrl"); }
            set { parent.SetProp("AvalaraServiceUrl", value); }
        }        
        public string Account
        {
            get { return parent.GetProp("AvalaraAccount"); }
            set { parent.SetProp("AvalaraAccount", value); }
        }
        public string LicenseKey
        {
            get { return parent.GetProp("AvalaraLicenseKey"); }
            set { parent.SetProp("AvalaraLicenseKey", value); }
        }
        public string CompanyCode
        {
            get { return parent.GetProp("AvalaraCompanyCode"); }
            set { parent.SetProp("AvalaraCompanyCode", value); }
        }        
            
    }
}
