using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using MerchantTribe.Shipping;
using System.Collections.Generic;
using MerchantTribe.Web.Geography;
using MerchantTribe.Shipping.FedEx.FedExRateServices;

namespace MerchantTribe.Shipping.FedEx
{

    public class RateService
    {

        private const int DefaultTimeout = 100000;

        public static string SendRequest(string serviceUrl, string postData)
        {
            return SendRequest(serviceUrl, postData, null);
        }

        public static string SendRequest(string serviceUrl, string postData, System.Net.WebProxy proxy)
        {
            WebResponse objResp;
            WebRequest objReq;
            string strResp = string.Empty;
            byte[] byteReq;

            try
            {
                byteReq = Encoding.UTF8.GetBytes(postData);
                objReq = WebRequest.Create(serviceUrl);
                objReq.Method = "POST";
                objReq.ContentLength = byteReq.Length;
                objReq.ContentType = "application/x-www-form-urlencoded";
                objReq.Timeout = DefaultTimeout;
                if (proxy != null)
                {
                    objReq.Proxy = proxy;
                }
                Stream OutStream = objReq.GetRequestStream();
                OutStream.Write(byteReq, 0, byteReq.Length);
                OutStream.Close();
                objResp = objReq.GetResponse();
                StreamReader sr = new StreamReader(objResp.GetResponseStream(), Encoding.UTF8, true);
                strResp += sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error SendRequest: " + ex.Message + " " + ex.Source);
            }

            return strResp;
        }

        private RateService()
        {

        }

        public static ShippingRate RatePackage(FedExGlobalServiceSettings globals,
                                       MerchantTribe.Web.Logging.ILogger logger,
                                       FedExServiceSettings settings,
                                       IShipment package)
        {
            ShippingRate result = new ShippingRate();

                // Get ServiceType
                ServiceType currentServiceType = ServiceType.FEDEXGROUND;
                currentServiceType = (ServiceType)settings.ServiceCode;

                // Get PackageType
                PackageType currentPackagingType = PackageType.YOURPACKAGING;
                currentPackagingType = (PackageType)settings.Packaging;

                // Set max weight by service
                CarrierCodeType carCode = GetCarrierCode(currentServiceType);

                result.EstimatedCost = RateSinglePackage(globals, 
                                                        logger,
                                                        package, 
                                                        currentServiceType, 
                                                        currentPackagingType, 
                                                        carCode);

            return result;
        }

      
        // Mappers between local enums and service enums
        private static CarrierCodeType GetCarrierCode(ServiceType service)
        {
            CarrierCodeType result = CarrierCodeType.FDXG;

            switch (service)
            {
                case ServiceType.EUROPEFIRSTINTERNATIONALPRIORITY:
                    result = CarrierCodeType.FDXE;
                    break;
                case ServiceType.FEDEX1DAYFREIGHT:
                    result = CarrierCodeType.FDXE;
                    break;
                case ServiceType.FEDEX2DAY:
                    result = CarrierCodeType.FDXE;
                    break;
                case ServiceType.FEDEX2DAYFREIGHT:
                    result = CarrierCodeType.FDXE;
                    break;
                case ServiceType.FEDEX3DAYFREIGHT:
                    result = CarrierCodeType.FDXE;
                    break;
                case ServiceType.FEDEXEXPRESSSAVER:
                    result = CarrierCodeType.FDXE;
                    break;
                case ServiceType.FEDEXGROUND:
                    result = CarrierCodeType.FDXG;
                    break;
                case ServiceType.FIRSTOVERNIGHT:
                    result = CarrierCodeType.FDXE;
                    break;
                case ServiceType.GROUNDHOMEDELIVERY:
                    result = CarrierCodeType.FDXG;
                    break;
                case ServiceType.INTERNATIONALECONOMY:
                    result = CarrierCodeType.FDXE;
                    break;
                case ServiceType.INTERNATIONALECONOMYFREIGHT:
                    result = CarrierCodeType.FDXE;
                    break;
                case ServiceType.INTERNATIONALFIRST:
                    result = CarrierCodeType.FDXE;
                    break;
                case ServiceType.INTERNATIONALPRIORITY:
                    result = CarrierCodeType.FDXE;
                    break;
                case ServiceType.INTERNATIONALPRIORITYFREIGHT:
                    result = CarrierCodeType.FDXE;
                    break;
                case ServiceType.PRIORITYOVERNIGHT:
                    result = CarrierCodeType.FDXE;
                    break;
                case ServiceType.STANDARDOVERNIGHT:
                    result = CarrierCodeType.FDXE;
                    break;
                default:
                    result = CarrierCodeType.FDXE;
                    break;
            }

            return result;
        }
        private static MerchantTribe.Shipping.FedEx.FedExRateServices.DropoffType GetDropOffType(DropOffType dropType)
        {
            DropoffType result = DropoffType.BUSINESS_SERVICE_CENTER;
            switch (dropType)
            {
                case DropOffType.BUSINESSSERVICECENTER:
                    return DropoffType.BUSINESS_SERVICE_CENTER;
                case DropOffType.DROPBOX:
                    return DropoffType.DROP_BOX;
                case DropOffType.REGULARPICKUP:
                    return DropoffType.REGULAR_PICKUP;
                case DropOffType.REQUESTCOURIER:
                    return DropoffType.REQUEST_COURIER;
                case DropOffType.STATION:
                    return DropoffType.STATION;
            }
            return result;
        }
        private static MerchantTribe.Shipping.FedEx.FedExRateServices.ServiceType GetServiceType(ServiceType serviceType)
        {
            MerchantTribe.Shipping.FedEx.FedExRateServices.ServiceType result = FedExRateServices.ServiceType.FEDEX_2_DAY;

            switch (serviceType)
            {
                case ServiceType.EUROPEFIRSTINTERNATIONALPRIORITY:
                    return FedExRateServices.ServiceType.EUROPE_FIRST_INTERNATIONAL_PRIORITY;
                case ServiceType.FEDEX1DAYFREIGHT:
                    return FedExRateServices.ServiceType.FEDEX_1_DAY_FREIGHT;
                case ServiceType.FEDEX2DAY:
                    return FedExRateServices.ServiceType.FEDEX_2_DAY;
                case ServiceType.FEDEX2DAY_AM:
                    return FedExRateServices.ServiceType.FEDEX_2_DAY_AM;
                case ServiceType.FEDEX2DAYFREIGHT:
                    return FedExRateServices.ServiceType.FEDEX_2_DAY_FREIGHT;
                case ServiceType.FEDEX3DAYFREIGHT:
                    return FedExRateServices.ServiceType.FEDEX_3_DAY_FREIGHT;
                case ServiceType.FEDEXEXPRESSSAVER:
                    return FedExRateServices.ServiceType.FEDEX_EXPRESS_SAVER;
                case ServiceType.FIRSTOVERNIGHT:
                    return FedExRateServices.ServiceType.FEDEX_FIRST_FREIGHT;
                case ServiceType.FEDEXGROUND:
                    return FedExRateServices.ServiceType.FEDEX_GROUND;
                case ServiceType.GROUNDHOMEDELIVERY:
                    return FedExRateServices.ServiceType.GROUND_HOME_DELIVERY;
                case ServiceType.INTERNATIONALECONOMY:
                    return FedExRateServices.ServiceType.INTERNATIONAL_ECONOMY;
                case ServiceType.INTERNATIONALECONOMYFREIGHT:
                    return FedExRateServices.ServiceType.INTERNATIONAL_ECONOMY_FREIGHT;
                case ServiceType.INTERNATIONALFIRST:
                    return FedExRateServices.ServiceType.INTERNATIONAL_FIRST;
                case ServiceType.INTERNATIONALPRIORITY:
                    return FedExRateServices.ServiceType.INTERNATIONAL_PRIORITY;
                case ServiceType.INTERNATIONALPRIORITYFREIGHT:
                    return FedExRateServices.ServiceType.INTERNATIONAL_PRIORITY_FREIGHT;
                case ServiceType.PRIORITYOVERNIGHT:
                    return FedExRateServices.ServiceType.PRIORITY_OVERNIGHT;
                case ServiceType.STANDARDOVERNIGHT:
                    return FedExRateServices.ServiceType.STANDARD_OVERNIGHT;       
                case ServiceType.SMARTPOST:
                    return FedExRateServices.ServiceType.SMART_POST;
            }

            return result;
        }
        private static MerchantTribe.Shipping.FedEx.FedExRateServices.PackagingType GetPackageType(PackageType packageType)
        {
            PackagingType result = PackagingType.YOUR_PACKAGING;

            switch (packageType)
            {
                case PackageType.FEDEX10KGBOX:
                    return PackagingType.FEDEX_10KG_BOX;
                case PackageType.FEDEX25KGBOX:
                    return PackagingType.FEDEX_25KG_BOX;
                case PackageType.FEDEXBOX:
                    return PackagingType.FEDEX_BOX;
                case PackageType.FEDEXENVELOPE:
                    return PackagingType.FEDEX_ENVELOPE;
                case PackageType.FEDEXPAK:
                    return PackagingType.FEDEX_PAK;
                case PackageType.FEDEXTUBE:
                    return PackagingType.FEDEX_TUBE;
                case PackageType.YOURPACKAGING:
                    return PackagingType.YOUR_PACKAGING;
            }

            return result;
        }
        private static string GetCountryCode(CountrySnapShot country)
        {
            string result = "US";
            var actual = Country.FindByBvin(country.Bvin);
            if (actual != null)
            {
                return actual.IsoCode;
            }
            return result;
        }


        private static decimal RateSinglePackage(FedExGlobalServiceSettings globalSettings,
                                                 MerchantTribe.Web.Logging.ILogger logger,
                                                 IShipment pak, 
                                                 ServiceType service, 
                                                 PackageType packaging,
                                                 CarrierCodeType carCode)
        {
            decimal result = 0m;




            try
            {



                // Auth Header Data
                var req = new FedExRateServices.RateRequest();
                req.WebAuthenticationDetail = new WebAuthenticationDetail();
                req.WebAuthenticationDetail.UserCredential = new WebAuthenticationCredential();
                req.WebAuthenticationDetail.UserCredential.Key = globalSettings.UserKey;
                req.WebAuthenticationDetail.UserCredential.Password = globalSettings.UserPassword;
                req.ClientDetail = new ClientDetail();
                req.ClientDetail.AccountNumber = globalSettings.AccountNumber;
                req.ClientDetail.MeterNumber = globalSettings.MeterNumber;
                req.ClientDetail.IntegratorId = "BVSoftware";
                req.Version = new VersionId();

                // Basic Transaction Data
                req.TransactionDetail = new TransactionDetail();
                req.TransactionDetail.CustomerTransactionId = System.Guid.NewGuid().ToString();
                req.ReturnTransitAndCommit = false;
                req.CarrierCodes = new CarrierCodeType[1] { carCode };

                // Shipment Details
                req.RequestedShipment = new RequestedShipment();

                req.RequestedShipment.LabelSpecification = new LabelSpecification();
                req.RequestedShipment.LabelSpecification.ImageType = ShippingDocumentImageType.PDF;
                req.RequestedShipment.LabelSpecification.LabelFormatType = LabelFormatType.COMMON2D;
                req.RequestedShipment.LabelSpecification.CustomerSpecifiedDetail = new CustomerSpecifiedLabelDetail();

                req.RequestedShipment.DropoffType = GetDropOffType(globalSettings.DefaultDropOffType);
                req.RequestedShipment.PackagingType = GetPackageType(packaging);
                req.RequestedShipment.TotalWeight = new Weight();
                req.RequestedShipment.TotalWeight.Value = Math.Round(pak.Items.Sum(y => y.BoxWeight), 1);
                if (pak.Items[0].BoxWeightType == Shipping.WeightType.Kilograms)
                {
                    req.RequestedShipment.TotalWeight.Units = WeightUnits.KG;
                }
                else
                {
                    req.RequestedShipment.TotalWeight.Units = WeightUnits.LB;
                }

                // Uncomment these lines to get insured values passed in
                //
                //var totalValue = pak.Items.Sum(y => y.BoxValue);
                //req.RequestedShipment.TotalInsuredValue = new Money();
                //req.RequestedShipment.TotalInsuredValue.Amount = totalValue;
                //req.RequestedShipment.TotalInsuredValue.Currency = "USD";

                req.RequestedShipment.PackageCount = pak.Items.Count.ToString();

                req.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[pak.Items.Count];
                for (int i = 0; i < pak.Items.Count; i++)
                {
                    req.RequestedShipment.RequestedPackageLineItems[i] = new RequestedPackageLineItem();
                    req.RequestedShipment.RequestedPackageLineItems[i].GroupNumber = "1";
                    req.RequestedShipment.RequestedPackageLineItems[i].GroupPackageCount = (i + 1).ToString();                    
                    req.RequestedShipment.RequestedPackageLineItems[i].Weight = new Weight();
                    req.RequestedShipment.RequestedPackageLineItems[i].Weight.Value = pak.Items[i].BoxWeight;
                    req.RequestedShipment.RequestedPackageLineItems[i].Weight.Units = pak.Items[i].BoxWeightType == Shipping.WeightType.Kilograms ? WeightUnits.KG : WeightUnits.LB;
                    req.RequestedShipment.RequestedPackageLineItems[i].Dimensions = new Dimensions();
                    req.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Height = pak.Items[i].BoxHeight.ToString();
                    req.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Length = pak.Items[i].BoxLength.ToString();
                    req.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Width = pak.Items[i].BoxWidth.ToString();
                    req.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Units = pak.Items[i].BoxLengthType == LengthType.Centimeters ? LinearUnits.CM : LinearUnits.IN;
                }

                req.RequestedShipment.Recipient = new Party();
                req.RequestedShipment.Recipient.Address = new FedExRateServices.Address();
                req.RequestedShipment.Recipient.Address.City = pak.DestinationAddress.City;
                req.RequestedShipment.Recipient.Address.CountryCode = GetCountryCode(pak.DestinationAddress.CountryData);
                req.RequestedShipment.Recipient.Address.PostalCode = pak.DestinationAddress.PostalCode;
                req.RequestedShipment.Recipient.Address.Residential = globalSettings.ForceResidentialRates;
                req.RequestedShipment.Recipient.Address.StateOrProvinceCode = pak.DestinationAddress.RegionData.Abbreviation; // GetStateCode(pak.DestinationAddress.RegionData);
                req.RequestedShipment.Recipient.Address.StreetLines = new string[2] { pak.DestinationAddress.Street, pak.DestinationAddress.Street2 };

                if (service == ServiceType.GROUNDHOMEDELIVERY)
                {
                    req.RequestedShipment.Recipient.Address.Residential = true;
                    req.RequestedShipment.Recipient.Address.ResidentialSpecified = true;
                }
                else if (service == ServiceType.FEDEXGROUND)
                {
                    req.RequestedShipment.Recipient.Address.Residential = false;
                    req.RequestedShipment.Recipient.Address.ResidentialSpecified = true;
                }
                else
                {
                    req.RequestedShipment.Recipient.Address.ResidentialSpecified = false;
                }

                req.RequestedShipment.Shipper = new Party();
                req.RequestedShipment.Shipper.AccountNumber = globalSettings.AccountNumber;
                req.RequestedShipment.Shipper.Address = new FedExRateServices.Address();
                req.RequestedShipment.Shipper.Address.City = pak.SourceAddress.City;
                req.RequestedShipment.Shipper.Address.CountryCode = GetCountryCode(pak.SourceAddress.CountryData);
                req.RequestedShipment.Shipper.Address.PostalCode = pak.SourceAddress.PostalCode;
                req.RequestedShipment.Shipper.Address.Residential = false;
                req.RequestedShipment.Shipper.Address.StateOrProvinceCode = pak.SourceAddress.RegionData.Abbreviation;
                req.RequestedShipment.Shipper.Address.StreetLines = new string[2] { pak.SourceAddress.Street, pak.SourceAddress.Street2 };


                var svc = new FedExRateServices.RateService();
                RateReply res = svc.getRates(req);

                if (res.HighestSeverity == NotificationSeverityType.ERROR ||
                    res.HighestSeverity == NotificationSeverityType.FAILURE)
                {
                    if (globalSettings.DiagnosticsMode == true)
                    {
                        foreach (var err in res.Notifications)
                        {
                            logger.LogMessage("FEDEX", err.Message, Web.Logging.EventLogSeverity.Debug);
                        }
                    }
                    result = 0m;
                }
                else
                {
                    result = 0m;

                    var lookingForService = GetServiceType(service);
                    var matchingResponse = res.RateReplyDetails.Where(y => y.ServiceType == lookingForService).FirstOrDefault();
                    if (matchingResponse != null)
                    {
                        var matchedRate = matchingResponse.RatedShipmentDetails.Where(
                            y => y.ShipmentRateDetail.RateType == ReturnedRateType.PAYOR_ACCOUNT_PACKAGE ||
                                y.ShipmentRateDetail.RateType == ReturnedRateType.PAYOR_ACCOUNT_SHIPMENT).FirstOrDefault();
                        if (matchedRate != null)                        
                        {
                            result = matchedRate.ShipmentRateDetail.TotalNetCharge.Amount;
                        }
                    }                                                                
                }
            }
            catch (Exception ex)
            {
                result = 0m;
                logger.LogException(ex);
            }

            return result;
        }

    }

}