using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Shipping;
using System.ComponentModel.DataAnnotations;

namespace MerchantTribeStore.Models
{
    public class EstimateShippingViewModel
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CountryId { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RegionId { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PostalCode { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string City { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public List<ShippingRateDisplay> Rates { get; set; }

        public EstimateShippingViewModel()
        {
            CountryId = MerchantTribe.Web.Geography.Country.UnitedStatesCountryBvin;
            RegionId = string.Empty;
            PostalCode = string.Empty;
            City = string.Empty;

            Rates = new List<ShippingRateDisplay>();
        }
    }
}