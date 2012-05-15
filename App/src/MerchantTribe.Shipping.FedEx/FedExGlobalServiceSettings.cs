using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.FedEx
{
    public class FedExGlobalServiceSettings
    {
        public string UserKey { get; set; }
        public string UserPassword { get; set; }
        public string AccountNumber { get; set; }
        public string MeterNumber { get; set; }
        public PackageType DefaultPackaging { get; set; }
        public bool UseListRates { get; set; }
        public DropOffType DefaultDropOffType { get; set; }
        public bool ForceResidentialRates { get; set; }
        public bool DiagnosticsMode { get; set; }        

        public FedExGlobalServiceSettings()
        {
            UserKey = string.Empty;
            UserPassword = string.Empty;
            AccountNumber = string.Empty;
            MeterNumber = string.Empty;
            DefaultPackaging = PackageType.YOURPACKAGING;
            UseListRates = false;
            DefaultDropOffType = DropOffType.REGULARPICKUP;
            ForceResidentialRates = false;
            DiagnosticsMode = false;
        }
    }
}
