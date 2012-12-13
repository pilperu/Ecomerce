using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.BusinessRules;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Utilities;
using BVSoftware.Avalara;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{
    public class AvalaraResubmitTaxes: OrderTask
    {

        public override bool Execute(OrderTaskContext context)
        {
            if (context.MTApp.CurrentStore.Settings.Avalara.Enabled)
            {
                try
                {
                    var originationAddress = AvalaraUtilities.ConvertAddressToAvalara(context.MTApp.ContactServices.Addresses.FindStoreContactAddress());
                    var destinationAddress = AvalaraUtilities.ConvertAddressToAvalara(context.Order.ShippingAddress);
                    var lines = AvalaraUtilities.ConvertOrderLines(context.Order.Items);
                    AvalaraUtilities.CancelAvalaraTaxDocument(context.Order, context.MTApp);
                    var OrderIdentifier = AvalaraUtilities.GetOrderIdentifier(context.Order, context.MTApp);
                    AvalaraUtilities.CommitAvalaraTaxes(context.Order, OrderIdentifier, originationAddress, destinationAddress, lines, context.MTApp);
                }
                finally
                {
                    if (string.IsNullOrEmpty(context.Order.CustomProperties.GetProperty("bvsoftware", BVAvaTax.AvalaraOrderEditedPropertyName)))
                    {
                        context.Order.CustomProperties.SetProperty("bvsoftware", BVAvaTax.AvalaraOrderEditedPropertyName, "1");                        
                    }                    
                    context.MTApp.OrderServices.Orders.Update(context.Order);
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
            return "Avalara Resubmit Taxes";
        }

        public override string TaskId()
        {
            return "103ec8c5-a7b3-45f0-a948-69f4f074568e";
        }

        public override Task Clone()
        {
            return new AvalaraResubmitTaxes();   
        }
    }
}
