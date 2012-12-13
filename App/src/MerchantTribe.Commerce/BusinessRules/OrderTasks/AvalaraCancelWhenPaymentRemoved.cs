using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.BusinessRules;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Orders;
using BVSoftware.Avalara;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{
    public class AvalaraCancelWhenPaymentRemoved:OrderTask
    {

        public override bool Execute(OrderTaskContext context)
        {
            if (context.MTApp.CurrentStore.Settings.Avalara.Enabled)
            {
                if (string.IsNullOrEmpty(context.Order.CustomProperties.GetProperty("bvsoftware",BVAvaTax.AvalaraOrderEditedPropertyName)))
                {
                    if (context.Order.PaymentStatus != OrderPaymentStatus.Paid)
                    {
                        if (!string.IsNullOrEmpty(context.Order.CustomProperties.GetProperty("bvsoftware", BVAvaTax.AvalaraTaxPropertyName)))
                        {
                            Utilities.AvalaraUtilities.CancelAvalaraTaxDocument(context.Order, context.MTApp);
                        }
                    }
                }                
            }
            return true;
        }

        public override bool Rollback(OrderTaskContext context)
        {
            return true;
        }

        public override string TaskName()
        {
            return "Avalara Cancel Taxes When Payment Removed";
        }

        public override string TaskId()
        {
            return "d10d9ffa-dda1-48ed-bfed-5a088cf685d7";
        }

        public override Task Clone()
        {
            return new AvalaraCancelWhenPaymentRemoved();
        }
    }
}
