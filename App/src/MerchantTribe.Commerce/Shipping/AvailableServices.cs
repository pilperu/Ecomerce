using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Shipping;
using MerchantTribe.Shipping.FedEx;

namespace MerchantTribe.Commerce.Shipping
{    
    public class AvailableServices
    {

        public static List<IShippingService> FindAll(Accounts.Store currentStore)
        {
            
            List<IShippingService> result = new List<IShippingService>();

            result = Service.FindAll();

            //result.Add(new MerchantTribe.Shipping.Fedex.FedExProvider());
            //FedEx            
            MerchantTribe.Shipping.FedEx.FedExGlobalServiceSettings fedexGlobal = new FedExGlobalServiceSettings();
            fedexGlobal.UserKey = currentStore.Settings.ShippingFedExKey;
            fedexGlobal.UserPassword = currentStore.Settings.ShippingFedExPassword;
            fedexGlobal.AccountNumber = currentStore.Settings.ShippingFedExAccountNumber;
            fedexGlobal.MeterNumber = currentStore.Settings.ShippingFedExMeterNumber;
            fedexGlobal.DefaultDropOffType = (MerchantTribe.Shipping.FedEx.DropOffType)currentStore.Settings.ShippingFedExDropOffType;
            fedexGlobal.DefaultPackaging = (MerchantTribe.Shipping.FedEx.PackageType)currentStore.Settings.ShippingFedExDefaultPackaging;
            fedexGlobal.DiagnosticsMode = currentStore.Settings.ShippingFedExDiagnostics;
            fedexGlobal.ForceResidentialRates = currentStore.Settings.ShippingFedExForceResidentialRates;
            fedexGlobal.UseListRates = currentStore.Settings.ShippingFedExUseListRates;
            result.Add(new MerchantTribe.Shipping.FedEx.FedExProvider(fedexGlobal, new EventLog()));

            // Load US Postal
            MerchantTribe.Shipping.USPostal.USPostalServiceGlobalSettings uspostalGlobal = new MerchantTribe.Shipping.USPostal.USPostalServiceGlobalSettings();
            uspostalGlobal.DiagnosticsMode = currentStore.Settings.ShippingUSPostalDiagnostics;
            result.Add(new MerchantTribe.Shipping.USPostal.DomesticProvider(uspostalGlobal, new EventLog()));
            result.Add(new MerchantTribe.Shipping.USPostal.InternationalProvider(uspostalGlobal, new EventLog()));
             
            // Load UPS
            MerchantTribe.Shipping.Ups.UPSServiceGlobalSettings upsglobal = new MerchantTribe.Shipping.Ups.UPSServiceGlobalSettings();
            upsglobal.AccountNumber = currentStore.Settings.ShippingUpsAccountNumber;
            upsglobal.LicenseNumber = currentStore.Settings.ShippingUpsLicense;
            upsglobal.Username = currentStore.Settings.ShippingUpsUsername;
            upsglobal.Password = currentStore.Settings.ShippingUpsPassword;
            upsglobal.DefaultPackaging = (MerchantTribe.Shipping.Ups.PackagingType)currentStore.Settings.ShippingUpsDefaultPackaging;
            upsglobal.DiagnosticsMode = currentStore.Settings.ShippingUPSDiagnostics;
            upsglobal.ForceResidential = currentStore.Settings.ShippingUpsForceResidential;
            upsglobal.IgnoreDimensions = currentStore.Settings.ShippingUpsSkipDimensions;
            upsglobal.PickUpType = (MerchantTribe.Shipping.Ups.PickupType)currentStore.Settings.ShippingUpsPickupType;
            result.Add(new MerchantTribe.Shipping.Ups.UPSService(upsglobal, new EventLog()));            
            
            return result;
        }
        
        public static IShippingService FindById(string id, Accounts.Store currentStore)
        {
            foreach (IShippingService svc in FindAll(currentStore))
            {
                if (svc.Id == id)
                {
                    return svc;
                }
            }            
            return null;
        }
    }
}
