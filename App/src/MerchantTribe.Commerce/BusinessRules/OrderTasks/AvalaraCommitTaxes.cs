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
    public class AvalaraCommitTaxes: OrderTask
    {

        public override bool Execute(OrderTaskContext context)
        {
            if (context.MTApp.CurrentStore.Settings.Avalara.Enabled)
            {
                if (string.IsNullOrEmpty(context.Order.CustomProperties.GetProperty("bvsoftware",BVAvaTax.AvalaraOrderEditedPropertyName)))
                {
                    try
                    {
                        var originationAddress = Utilities.AvalaraUtilities.ConvertAddressToAvalara(context.MTApp.ContactServices.Addresses.FindStoreContactAddress());
                        var destinationAddress = Utilities.AvalaraUtilities.ConvertAddressToAvalara(context.Order.ShippingAddress);
                        var lines = Utilities.AvalaraUtilities.ConvertOrderLines(context.Order.Items);
                        var OrderIdentifier = Utilities.AvalaraUtilities.GetOrderIdentifier(context.Order, context.MTApp);
                        Utilities.AvalaraUtilities.CommitAvalaraTaxes(context.Order, OrderIdentifier, originationAddress, destinationAddress, lines, context.MTApp);
                    }
                    finally
                    {
                        context.MTApp.OrderServices.Orders.Update(context.Order);
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
            return "Avalara Commit Taxes";
        }

        public override string TaskId()
        {
            return "e3cff8c5-b691-4a2a-b96d-d70f508d81d2";
        }

        public override Task Clone()
        {
            return new AvalaraCommitTaxes();
        }
    }
}
