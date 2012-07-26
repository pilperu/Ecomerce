using System;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Data;
using MerchantTribe.Commerce.Utilities;
using System.Collections.ObjectModel;
using System.Collections.Generic;

//Imports System.IO
//Imports System.Xml
using System.Web;
//Imports System.Web.Configuration

namespace MerchantTribe.Commerce
{
    /// <summary>
    /// These are application wide settings and apply to all stores in the system
    /// </summary>
    public partial class WebAppSettings
    {

        // Number of Minutes to Cache Settings
        const int CacheDefaultTime = 1440;
        #region " Constants "

        public const int DEFAULT_WEB_SERVICE_TIMEOUT = 60;
        public const string AdministratorsRoleId = "36CC0F07-2DE4-4d25-BF60-C80BC0214F09";
        public const string AdministratorUserId = "30";
        public const string DefaultDropShipEmailTemplateId = "49dc0811-0d44-44ab-8ac5-5675365eca98";
        public const string EmailTemplateGiftCard = "34f5dffd-03ab-4bc9-b305-cd15020045ca";
        public const string PaymentIdCreditCard = "4A807645-4B9D-43f1-BC07-9F233B4E713C";
        public const string PaymentIdTelephone = "9FD35C50-CDCB-42ac-9549-14119BECBD0C";
        public const string PaymentIdCheck = "494A61C8-D7E7-457f-B293-4838EF010C32";
        public const string PaymentIdCash = "7FCC4B3F-6E67-4f58-86B0-25BCCC035A0E";
        public const string PaymentIdPurchaseOrder = "26C948F3-22EF-4bcb-9AE9-DEB9839BF4A7";
        public const string PaymentIdCompanyAccount = "43AE5D2D-A62B-4EB3-BAAF-176EB509C9B5";
        public const string PaymentIdCashOnDelivery = "EE171EFD-9E4A-4eda-AD70-4CB99F28E06C";
        public const string PaymentIdGiftCertificate = "91a205f1-8c1c-4267-bed0-c8e410e7e680";
        public const string PaymentIdGoogleCheckout = "49de5510-dfe4-4b18-91a6-3dc9925566a1";
        public const string PaymentIdPaypalExpress = "33eeba60-e5b7-4864-9b57-3f8d614f8301";
        public const string PrintTemplatePackingSlip = "39e51ef8-280d-4358-b507-8d4d8ca348a1";
        public const string PrintTemplateAdminReceipt = "6c9c2d90-9f76-42aa-8453-b77ed44c283d";
        public const string PrintTemplateCustomerInvoice = "c090c3e7-974b-4a62-afdf-716395abec3d";
        public const string UPSProviderId = "";
        public const string ShippingUSPostalDomesticId = "B28F245B-8FE5-404E-A857-A6D01904A29A";
        public const string ShippingUSPostalInternationalId = "BD2CB7D9-CEF3-41D7-84A1-44FD420A1CF3";

        public const string SessionId = "merchanttribesessionid";
        public const string CartId = "merchanttribecartid";

        public const string VersioningFolder = "\\bvadmin\\versioning\\";
        public const string SqlVersioningFolder = "\\bvadmin\\versioning\\sql\\";
        public const string SqlFolder = "\\bvadmin\\bvsql\\";

        public const string GuestPasswordQueryStringName = "guest";

        public static string StoreKey
        {
            get
            {
                string key = ConfigurationManager.AppSettings["storekey"];
                if (key != null) return key;
                return string.Empty;
            }
        }
        public static bool IsHostedVersion
        {
            get
            {                
                if ((IsIndividualMode == false) && (IsCommercialVersion == false)) return true;
                return false;
            }
        }
        public static bool IsIndividualMode
        {
            get
            {
                try
                {
                    string key = ConfigurationManager.AppSettings["storekey"];
                    if (key == "D13400AA-B216-42F7-B29E-200A5401A41D") return false;
                }
                catch (Exception ex)
                {

                }

                return true;
            }
        }
        public static bool IsCommercialVersion
        {
            get
            {
                try
                {
                    string key = ConfigurationManager.AppSettings["storekey"];
                    if (key == "C589E3FB-9B9D-47EB-A694-690A97742C82") return true;
                }
                catch (Exception ex)
                {

                }

                return false;
            }
        }
        public static string ApplicationConnectionString
        {
            get
            {
                string result = string.Empty;
                try
                {
                    result = ConfigurationManager.ConnectionStrings["commerce6ConnectionString"].ConnectionString;
                }
                catch (Exception ex)
                {
                }

                return result; 
            } 
        }
        public static string ApplicationConnectionStringForEntityFramework
        {
            get
            {
                string result = string.Empty;

                try
                {
                    string connstring = ApplicationConnectionString.TrimEnd(';');
                    string template = ConfigurationManager.AppSettings["EfConnectionStringFormat"];
                    result = template.Replace("{0}", connstring);
                }
                catch (Exception ex)
                {
                }

                return result;
            }
        }
        public static string ApplicationBaseUrl
        {
            get { return ConfigurationManager.AppSettings["BaseApplicationUrl"]; }
        }
        //public static string ApplicationBaseImageUrl
        //{
        //    get { return ApplicationBaseUrl + "images/sites/"; }
        //}
        private static string _UnitTestPhysicalPath = string.Empty;
        public static void SetUnitTestPhysicalPath(string rootPhysicalPath)
        {
            _UnitTestPhysicalPath = rootPhysicalPath;
        }
        public static string ApplicationBaseImagePhysicalPath
        {
            get
            {
                // For Unit Testing
                if (_UnitTestPhysicalPath != string.Empty)
                {
                    return _UnitTestPhysicalPath + "\\images\\sites\\";
                }

                string result = string.Empty;

                result = System.Web.Hosting.HostingEnvironment.MapPath("~/images/sites");
                    if (!result.EndsWith("\\"))
                    {
                        result += "\\";
                    }

                return result;
            }
        }
        public static string ApplicationBuiltinThemesPath
        {
            get
            {
                string result = string.Empty;

                // For Unit Testing
                if (_UnitTestPhysicalPath != string.Empty)
                {
                    return _UnitTestPhysicalPath + "\\content\\themes\\";
                }

                result = System.Web.Hosting.HostingEnvironment.MapPath("~/content/themes");
                //if (HttpContext.Current != null)
                //{
                //    result = HttpContext.Current.Request.MapPath("~/content/themes");
                    if (!result.EndsWith("\\"))
                    {
                        result += "\\";
                    }
                //}
                return result;
            }
        }
        public static string ApplicationMailServer
        {
            get { return ConfigurationManager.AppSettings["MailServer"]; }
        }
        public static string ApplicationMailServerUsername
        {
            get { return ConfigurationManager.AppSettings["MailServerUsername"]; }
        }
        public static string ApplicationMailServerPassword
        {
            get { return ConfigurationManager.AppSettings["MailServerPassword"]; }
        }
        public static bool ApplicationMailServerSSL
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["MailServerSSL"]); }
        }
        public static string ApplicationMailServerPort
        {
            get { return ConfigurationManager.AppSettings["MailServerPort"]; }
        }
        public static bool ApplicationMailServerAsync
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["MailServerAsync"]); }
        }
        public static long BillingStoreId
        {
            get { return long.Parse(ConfigurationManager.AppSettings["BillingStoreId"]); }
        }

        public static string ApplicationPayPalUsername
        {
            get { return ConfigurationManager.AppSettings["AppPayPalUsername"]; }
        }
        public static string ApplicationPayPalPassword
        {
            get { return ConfigurationManager.AppSettings["AppPayPalPassword"]; }
        }
        public static string ApplicationPayPalSignature
        {
            get { return ConfigurationManager.AppSettings["AppPayPalSignature"]; }
        }
        
        #endregion

        private WebAppSettings()
        {

        }
             
        public static string AffiliateQueryStringName
        {
            get { return "affid"; }         
        }        
        public static bool ApplyTaxToShippingAddress
        {
            get { return true; }
        }                    
        
        public static string Cryptography3DesKey
        {
            get { return "EDBE6BF8A92A417cBCD3DB23120861B5DE780BA44DB44166888707607A2A16FBBADFD3E111D54396A5701CE43E0EC3FFAE5543370AF54228B65CB87D7E346048"; }
        }               
        public static string DefaultTextEditor
        {
            get { return "TinyMCE";
                //return "None";
            }
        }        
        //public static bool DisableInventory
        //{
        //    get { return true; }
        //}
        public static bool DisplayFullCreditCardNumber
        {
            get { return false; }
        }        
        public static bool DisplayUpSellsWhenAddingItemToCart
        {
            get { return false; }
        }                
        public static bool EnablePostalCodeValidation
        {
            get { return true; }
        }        
        public static bool EnableReturns
        {
            get { return false; }
        }        
        public static decimal GiftWrapCharge
        {
            get { return 0.00m; }
        }        
        public static decimal GiftCertificateMaximumAmount
        {
            get { return 9999m; }
        }        
        public static decimal GiftCertificateMinimumAmount
        {
            get { return 0.01m; }          
        }        
        public static int GiftCertificateValidDays
        {
            get { return 1825; }
        }        
        public static bool GiftWrapAll
        {
            get { return false; }
        }
        public static decimal GiftWrapRate
        {
            get { return 0.00m; }
        }                                       
        public static int InventoryLowHours
        {
            get { return 24; }
        }
        public static DateTime InventoryLowLastTimeRun
        {
            // set to the future so this never runs until it's implemented
            get { return DateTime.UtcNow.AddDays(2); }
        }
        public static string InventoryLowReportLinePrefix
        {
            get { return ""; }
        }
        public static bool InventoryEnabledNewProductDefault
        {
            get { return false; }
        }
        public static Catalog.StoreProductInventoryMode InventoryModeOLD
        {
            get { return Catalog.StoreProductInventoryMode.ReserveOnOrder; }
        }                    
        public static bool KitDisplayCollapsed
        {
            get { return false;}
        }
        public static string LastProductsViewedCookieName
        {
            get { return "MerchantTribeLastProductsViewed"; }
        }
        public static int LastProductsViewedMaxResults
        {
            get { return 3; }
        }
        public static bool InventoryReservedAtCheckout
        {
            get { return true; }
        }
        public static int MenuItemsPerRow
        {
            get { return 9;}
        }
        public static bool MergeCartItems
        {
            get { return false; }
        }
        public static bool NewProductBadgeAllowed
        {
            get { return false;}            
        }
        public static int NewProductBadgeDays
        {
            get { return 30; }
        }
        public static int PasswordMinimumLength
        {
            get { return 8; }
        }             
        public static string PaymentCheckDescription
        {
            get { return "Send a Check by Mail"; }
        }
        public static string PaymentCheckName
        {
            get { return "Check"; }
        }                        
        public static string PaymentTelephoneDescription
        {
            get { return "Call us after placing your order to arrange payment."; }
        }
        public static string PaymentTelephoneName
        {
            get { return "Phone"; }
        }
        public static string PaymentPurchaseOrderName
        {
            get { return "Purchase Order"; }
        }
        public static string PaymentCompanyAccountName
        {
            get { return "Company Account"; }
        }
        public static bool PaymentPurchaseOrderRequirePONumber
        {
            get { return true; }
        }
        public static string PaymentCODDescription
        {
            get { return "Cash On Delivery"; }
        }
        public static string PaymentCODName
        {
            get { return "COD"; }
        }
        public static string PaymentNoPaymentNeededDescription
        {
            get { return "No Payment Required."; }
        }        
        public static bool PerformanceAutoLoadProductsList
        {
            get { return false; }
        }
        public static int ProductLongDescriptionEditorHeight
        {
            get { return 150;}
        }               
        public static bool RememberUsers
        {
            get { return true; }
        }        
        public static bool RedirectToCartAfterAddProduct
        {
            get { return true; }
        }        
        public static bool ReverseOrderNotes
        {
            get { return false; }
        }        
        public static int RowsPerPage
        {
            get { return 10; }
        }
        public static bool SearchDisableBlankSearchTerms
        {
            get { return true; }
        }        
        public static string ShippingFedExServer
        {
            get { return "https://gateway.fedex.com/GatewayDC"; }           
        }
        public static string ShippingUpsServer
        {
            get { return "https://www.ups.com/ups.app/xml/"; }
        }
        public static string ShippingUSPostalPassword
        {
            get { return "643BVSOF1535"; }        
        }
        public static string ShippingUSPostalServer
        {
            get { return "http://production.shippingapis.com/ShippingAPI.dll";}
        }
        public static string ShippingUSPostalUsername
        {
            get { return "317UW02RM959"; }
        }
        public static decimal ShippingUSPostalFlatRateEnvelopeCutoff
        {
            get { return 1.0m; }
        }
        public static decimal ShippingUSPostalFlatRateBoxCutoff
        {
            get { return 4.0m; }
        }
        public static System.Globalization.CultureInfo SiteCulture
        {
            get { return new System.Globalization.CultureInfo(SiteCultureCode); }
        }
        public static string SiteCultureCode
        {
            get { return "en-US"; }
        }
        public static string ApplicationCountryBvin
        {
            get { return MerchantTribe.Web.Geography.Country.UnitedStatesCountryBvin; }
        }
        public static MerchantTribe.Shipping.WeightType ApplicationWeightUnits
        {
            get { return MerchantTribe.Shipping.WeightType.Pounds; }
        }
        public static MerchantTribe.Shipping.LengthType ApplicationLengthUnits
        {
            get { return MerchantTribe.Shipping.LengthType.Inches; }
        }
        public static bool SiteRootsOnDifferentTLDs
        {
            get { return false; }
        }     
        public static bool StoreAdminLinksInNewWindow
        {
            get { return true; }
        }
        public static int SuggestedItemsMaxResults
        {
            get { return 3; }
        }                
        public static bool TypePropertiesDisplayEmptyProperties
        {
            get { return false; }
        }
        public static bool UpsRestrictions
        {
            get { return false; }
        }
        public static string CustomerIdCookieName
        {
            get { return "MerchantTribeCustomerID"; }
        }        
        public static bool UseSsl
        {
            get { return true; }
        }        
        public static System.Globalization.CultureInfo WebAppSettingsCulture
        {
            get { return System.Globalization.CultureInfo.InvariantCulture; }
        }

       
        #region " Methods "

        //public static string GetStringSetting(string settingName)
        //{
        //    string result = string.Empty;
        //    result = Datalayer.CacheManager.GetStringFromCache(settingName);
        //    if (result == null)
        //    {
        //        result = GetSettingFromDB(settingName);
        //        Datalayer.CacheManager.UpsertStringInCache(settingName, result, CacheDefaultTime);
        //    }          
        //    return result;
        //}

        //public static decimal GetDecimalSetting(string settingName)
        //{
        //    string temp = GetStringSetting(settingName);
        //    decimal result = 0m;
        //    try
        //    {
        //        result = decimal.Parse(temp, WebAppSettingsCulture);
        //    }
        //    catch 
        //    {
        //        System.Diagnostics.Debug.Write("Decimal Parse Error");
        //    }
        //    return result;
        //}

        //public static int GetIntegerSetting(string settingName)
        //{
        //    string temp = GetStringSetting(settingName);
        //    int result = 0;
        //    try
        //    {
        //        result = int.Parse(temp, WebAppSettingsCulture);
        //    }
        //    catch 
        //    {
        //        System.Diagnostics.Debug.Write("Parse Integer Error");
        //    }
        //    return result;
        //}

        //public static long GetLongSetting(string settingName)
        //{
        //    string temp = GetStringSetting(settingName);
        //    long result = 0;
        //    try
        //    {
        //        result = long.Parse(temp, WebAppSettingsCulture);
        //    }
        //    catch
        //    {
        //        System.Diagnostics.Debug.Write("Parse integer error");
        //    }
        //    return result;
        //}

        //public static bool GetBooleanSetting(string settingName)
        //{
        //    bool result = false;
        //    if (GetStringSetting(settingName) == "1")
        //    {
        //        result = true;
        //    }
        //    return result;
        //}

        //public static string GetEncryptedStringSetting(string settingName)
        //{
        //    string result = string.Empty;
        //    result = GetStringSetting(settingName);
        //    BVSoftware.Cryptography.TripleDesEncryption crypto = new BVSoftware.Cryptography.TripleDesEncryption();
        //    if (result != string.Empty)
        //    {
        //        result = crypto.Decode(result);
        //    }
        //    crypto = null;
        //    return result;
        //}

        //private static string GetSettingFromDB(string settingName)
        //{
        //    string result = string.Empty;
        //    try
        //    {
        //        result = Mapper.Find(settingName);
        //        if (result == null)
        //        {
        //            result = string.Empty;
        //        }
        //    }
        //    catch
        //    {
        //        // throw error
        //        throw;
        //    }
        //    return result;
        //}

        //public static bool UpdateStringSetting(string name, string content)
        //{
        //    bool result = true;

        //    try
        //    {
        //        result = Mapper.Save(name, content);
        //        if (result == true)
        //        {
        //            Datalayer.CacheManager.UpsertStringInCache(name, content, CacheDefaultTime);                   
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        EventLog.LogEvent(ex);
        //    }

        //    return result;
        //}

        //public static bool UpdateDecimalSetting(string name, decimal content)
        //{
        //    return UpdateStringSetting(name, content.ToString(WebAppSettingsCulture));
        //}

        //public static bool UpdateIntegerSetting(string name, int content)
        //{
        //    return UpdateStringSetting(name, content.ToString(WebAppSettingsCulture));
        //}

        //public static bool UpdateLongSetting(string name, long content)
        //{
        //    return UpdateStringSetting(name, content.ToString(WebAppSettingsCulture));
        //}

        //public static bool UpdateBooleanSetting(string name, bool content)
        //{
        //    if (content == true)
        //    {
        //        return UpdateStringSetting(name, "1");
        //    }
        //    else
        //    {
        //        return UpdateStringSetting(name, "0");
        //    }
        //}

        //public static bool UpdateEncryptedStringSetting(string name, string content)
        //{
        //    string encoded = string.Empty;
        //    BVSoftware.Cryptography.TripleDesEncryption crypto = new BVSoftware.Cryptography.TripleDesEncryption();
        //    encoded = crypto.Encode(content);
        //    return UpdateStringSetting(name, encoded);
        //}

        //public static bool Load()
        //{
        //    bool result = true;
        //    try
        //    {
        //        DataTable dtSettings = ToDataTable();
        //        if (dtSettings != null)
        //        {
        //            for (int i = 0; i <= dtSettings.Rows.Count - 1; i++)
        //            {
        //                string tempName = (string)dtSettings.Rows[i]["SettingName"];
        //                string tempValue = (string)dtSettings.Rows[i]["SettingValue"];
        //                Datalayer.CacheManager.UpsertStringInCache(tempName, tempValue, CacheDefaultTime);

        //            }
        //            result = true;
        //        }
        //        else
        //        {
        //            result = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        EventLog.LogEvent(ex);
        //        throw new ArgumentException("BV Commerce", "BV Commerce was unable to load application settings. Make sure that the ASPNET account has read permissions to the config file. " + ex.Message);
        //    }
        //    return result;
        //}
     
        #endregion


    }
 
}