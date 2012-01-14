using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribeStore.Models;
using MerchantTribe.Web.Geography;
using System.Text;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore.Controllers
{
    public class EstimateShippingController : Shared.BaseStoreController
    {

        private EstimateShippingViewModel BuildViewModel()
        {            
            EstimateShippingViewModel model = new EstimateShippingViewModel();
            ViewBag.GetRatesButton = this.MTApp.ThemeManager().ButtonUrl("GetRates", Request.IsSecureConnection);
            ViewBag.Countries = MTApp.CurrentStore.Settings.FindActiveCountries();

            if (SessionManager.CurrentUserHasCart(MTApp.CurrentStore))
            {
                Order basket = SessionManager.CurrentShoppingCart(MTApp.OrderServices, MTApp.CurrentStore);
                if (basket.ShippingAddress.CountryBvin != string.Empty)
                {
                    model.CountryId = basket.ShippingAddress.CountryBvin;                    
                }
                if (basket.ShippingAddress.RegionBvin != string.Empty)
                {
                    model.RegionId = basket.ShippingAddress.RegionBvin;                    
                }
                model.City = basket.ShippingAddress.City;
                model.PostalCode = basket.ShippingAddress.PostalCode;                                
            }

            return model;
        }

        //
        // GET: /EstimateShipping/
        public ActionResult Index()
        {
            EstimateShippingViewModel model = BuildViewModel();
            GetRates(model);            

            // Populate Data for DropDownLists
            Country currentCountry = Country.FindByBvin(model.CountryId);
            if (currentCountry != null) ViewBag.Regions = currentCountry.Regions;

            return View(model);
        }

        [HttpPost]
        [ActionName("Index")]        
        public ActionResult IndexPost(EstimateShippingViewModel posted)
        {
            EstimateShippingViewModel model = BuildViewModel();
            if (posted != null)
            {
                model.PostalCode = posted.PostalCode;
                model.CountryId = posted.CountryId;
                model.RegionId = posted.RegionId;
            }
            GetRates(model);

            return View(model);
        }

        private class JsonResponse
        {
            public string Regions = "<option value=\"\">- Not Required -</option>";
        }

        [HttpPost]            
        public ActionResult GetRegions(string id)
        {
            string regionid = Request.Form["regionid"];

            JsonResponse result = new JsonResponse();            
            Country c = Country.FindByBvin(id);
            if ((c != null))
            {
                StringBuilder sb = new StringBuilder();
                result.Regions = string.Empty;

                foreach (Region r in c.Regions)
                {
                    sb.Append("<option ");
                    if (r.Abbreviation == regionid)
                    {
                        sb.Append(" selected=\"selected\" ");
                    }
                    sb.Append(" value=\"" + r.Abbreviation + "\">" + r.Name + "</option>");
                }

                result.Regions = sb.ToString();
            }

            return new JsonResult() { Data = result };
        }

        private void GetRates(EstimateShippingViewModel model)
        {
            if (SessionManager.CurrentUserHasCart(MTApp.CurrentStore) == true)
            {
                Order Basket = SessionManager.CurrentShoppingCart(MTApp.OrderServices, MTApp.CurrentStore);
                if (Basket != null)
                {
                    Basket.ShippingAddress.PostalCode = model.PostalCode;
                    
                    if (model.CountryId != string.Empty)
                    {
                        Country current = MerchantTribe.Web.Geography.Country.FindByBvin(model.CountryId);
                        if (current != null)
                        {
                            Basket.ShippingAddress.CountryBvin = model.CountryId;
                            Basket.ShippingAddress.CountryName = current.DisplayName;
                            Basket.ShippingAddress.RegionBvin = model.RegionId;
                            Basket.ShippingAddress.RegionName = model.RegionId;
                        }                        
                    }                                                            

                    MTApp.OrderServices.Orders.Update(Basket);

                    SortableCollection<ShippingRateDisplay> Rates;
                    Rates = MTApp.OrderServices.FindAvailableShippingRates(Basket, MTApp.CurrentStore);

                    if (Rates.Count < 1)
                    {
                        TempData["message"] = "Unable to estimate at this time";
                    }                    
                    model.Rates = Rates.ToList();
                }
            }
        }

        public class ShippingRatesJsonResponse
        {
            public string totalsastable = string.Empty;
            public string rates = "";
        }

        [HttpPost]
        public ActionResult GetRatesAsRadioButtons(FormCollection form)
        {
            ShippingRatesJsonResponse result = new ShippingRatesJsonResponse();

            string country = form["country"] ?? string.Empty;
            string firstname = form["firstname"] ?? string.Empty;
            string lastname = form["lastname"] ?? string.Empty;
            string address = form["address"] ?? string.Empty;
            string city = form["city"] ?? string.Empty;
            string state = form["state"] ?? string.Empty;
            string zip = form["zip"] ?? string.Empty;
            string orderid = form["orderid"] ?? string.Empty;

            Order o = MTApp.OrderServices.Orders.FindForCurrentStore(orderid);
            o.ShippingAddress.FirstName = firstname;
            o.ShippingAddress.LastName = lastname;
            o.ShippingAddress.Line1 = address;
            o.ShippingAddress.City = city;
            o.ShippingAddress.PostalCode = zip;
            Country c = Country.FindByBvin(country);
            if ((c != null))
            {
                o.ShippingAddress.CountryBvin = country;
                o.ShippingAddress.CountryName = c.DisplayName;
                foreach (Region r in c.Regions)
                {
                    if ((r.Abbreviation == state))
                    {
                        o.ShippingAddress.RegionBvin = r.Abbreviation;
                        o.ShippingAddress.RegionName = r.Name;
                    }
                }
            }

            SortableCollection<ShippingRateDisplay> rates = MTApp.OrderServices.FindAvailableShippingRates(o, MTApp.CurrentStore);

            string rateKey = o.ShippingMethodUniqueKey;
            bool rateIsAvailable = false;

            // See if rate is available
            if ((rateKey.Length > 0))
            {
                foreach (MerchantTribe.Commerce.Shipping.ShippingRateDisplay r in rates)
                {
                    if ((r.UniqueKey == rateKey))
                    {
                        rateIsAvailable = true;
                        MTApp.OrderServices.OrdersRequestShippingMethod(r, o);
                    }
                }
            }

            // if it's not availabe, pick the first one or default
            if ((rateIsAvailable == false))
            {
                if ((rates.Count > 0))
                {
                    MTApp.OrderServices.OrdersRequestShippingMethod(rates[0], o);
                    rateKey = rates[0].UniqueKey;
                }
                else
                {
                    o.ClearShippingPricesAndMethod();
                }
            }

            result.rates = HtmlRendering.ShippingRatesToRadioButtons(rates, 300, o.ShippingMethodUniqueKey);

            MTApp.CalculateOrderAndSave(o);
            SessionManager.SaveOrderCookies(o, MTApp.CurrentStore);

            result.totalsastable = o.TotalsAsTable();

            //System.Threading.Thread.Sleep(500)
            return new PreJsonResult(MerchantTribe.Web.Json.ObjectToJson(result));            
        }
    }
}
