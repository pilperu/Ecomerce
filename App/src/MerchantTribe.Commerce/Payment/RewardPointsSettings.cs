using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Payment;

namespace MerchantTribe.Commerce.Payment
{
    public class RewardPointsSettings : MerchantTribe.Payment.MethodSettings
    {
        public RewardPointsSettings(long currentStoreId)
        {
            this.CurrentStoreId = currentStoreId;
        }

        public long CurrentStoreId { get; private set; }

        public int PointsIssuedPerDollarSpent
        {
            get { return GetIntSetting("PointsIssuedPerDollarSpent"); }
            set { SetIntSetting("PointsIssuedPerDollarSpent", value); }
        }
        public int PointsNeededForDollarCredit
        {
            get { return GetIntSetting("PointsNeededForDollarCredit"); }
            set { SetIntSetting("PointsNeededForDollarCredit", value); }
        }
    }
}
