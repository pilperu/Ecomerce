using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce.Contacts;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MerchantTribeStore.Models
{
    
    public class AddressViewModel
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Bvin { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public DateTime LastUpdatedUtc { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public long StoreId { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string NickName { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FirstName { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MiddleInitial { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LastName { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Company { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Line1 { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Line2 { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Line3 { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string City { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RegionName { get; set; }
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RegionBvin { get; set; }

        private string _PostalCode = string.Empty;
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PostalCode
        {
            get { return _PostalCode; }
            set { _PostalCode = value.Replace("-", ""); }
        }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CountryName { get; set; }
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CountryBvin { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Phone { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Fax { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string WebSiteUrl { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CountyName { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CountyBvin { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UserBvin { get; set; }
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public AddressTypes AddressType { get; set; }

        public AddressViewModel()
        {
            this.Init();
        }
        public AddressViewModel(Address a)
        {
            Init();
            this.StoreId = a.StoreId;
            this.NickName = a.NickName;
            this.FirstName = a.FirstName;
            this.MiddleInitial = a.MiddleInitial;
            this.LastName = a.LastName;
            this.Company = a.Company;
            this.Line1 = a.Line1;
            this.Line2 = a.Line2;
            this.Line3 = a.Line3;
            this.City = a.City;
            this.RegionName = a.RegionName;
            this.RegionBvin = a.RegionBvin;
            this.PostalCode = a.PostalCode;
            this.CountryName = a.CountryName;
            this.CountryBvin = a.CountryBvin;
            this.CountyBvin = a.CountyBvin;
            this.CountyName = a.CountyName;
            this.Phone = a.Phone;
            this.Fax = a.Fax;
            this.WebSiteUrl = a.WebSiteUrl;
            this.UserBvin = a.UserBvin;
            this.AddressType = a.AddressType;
            this.LastUpdatedUtc = a.LastUpdatedUtc;
        }
        private void Init()
        {
            this.StoreId = 0;
            this.NickName = string.Empty;
            this.FirstName = string.Empty;
            this.MiddleInitial = string.Empty;
            this.LastName = string.Empty;
            this.Company = string.Empty;
            this.Line1 = string.Empty;
            this.Line2 = string.Empty;
            this.Line3 = string.Empty;
            this.City = string.Empty;
            this.RegionName = string.Empty;
            this.RegionBvin = string.Empty;
            this.PostalCode = string.Empty;
            this.CountryName = "US";
            this.CountryBvin = MerchantTribe.Web.Geography.Country.FindByISOCode("US").Bvin;
            this.CountyBvin = string.Empty;
            this.CountyName = string.Empty;
            this.Phone = string.Empty;
            this.Fax = string.Empty;
            this.WebSiteUrl = string.Empty;
            this.UserBvin = string.Empty;
            this.AddressType = AddressTypes.General;
            this.LastUpdatedUtc = DateTime.UtcNow;
        }
        public void CopyTo(Address a)
        {            
            a.StoreId = this.StoreId;
            a.NickName = this.NickName;
            a.FirstName = this.FirstName;
            a.MiddleInitial = this.MiddleInitial;
            a.LastName = this.LastName;
            a.Company = this.Company;
            a.Line1 = this.Line1;
            a.Line2 = this.Line2;
            a.Line3 = this.Line3;
            a.City = this.City;
            a.RegionName = this.RegionName;
            a.RegionBvin = this.RegionBvin;
            a.PostalCode = this.PostalCode;
            a.CountryName = this.CountryName;
            a.CountryBvin = this.CountryBvin;
            a.CountyBvin = this.CountyBvin;
            a.CountyName = this.CountyName;
            a.Phone = this.Phone;
            a.Fax = this.Fax;
            a.WebSiteUrl = this.WebSiteUrl;
            a.UserBvin = this.UserBvin;
            a.AddressType = this.AddressType;
            a.LastUpdatedUtc = this.LastUpdatedUtc;
        }
        public string ToHtmlString()
        {
            StringBuilder sb = new StringBuilder();
            if (NickName.Trim().Length > 0)
            {
                sb.Append("<em>" + NickName + "</em><br />");
            }
            if (LastName.Length > 0 || FirstName.Length > 0)
            {
                sb.Append(FirstName);
                if (MiddleInitial.Trim().Length > 0)
                {
                    sb.Append(" " + MiddleInitial);
                }
                sb.Append(" " + LastName + "<br />");
                if (Company.Trim().Length > 0)
                {
                    sb.Append(Company + "<br />");
                }
            }
            if (Line1.Length > 0)
            {
                sb.Append(Line1 + "<br />");
            }
            if (Line2.Trim().Length > 0)
            {
                sb.Append(Line2 + "<br />");
            }
            if (Line3.Trim().Length > 0)
            {
                sb.Append(Line3 + "<br />");
            }

            MerchantTribe.Web.Geography.Country c = MerchantTribe.Web.Geography.Country.FindByBvin(CountryBvin);
            MerchantTribe.Web.Geography.Region r = c.Regions.Where(y => y.Abbreviation == RegionBvin).FirstOrDefault();

            if (r != null)
            {
                sb.Append(City + ", " + r.Abbreviation + " " + _PostalCode + "<br />");
            }
            else
            {
                if (RegionName.Trim().Length > 0)
                {
                    sb.Append(City + ", " + RegionName + " " + _PostalCode + "<br />");
                }
                else
                {
                    sb.Append(City + ", " + _PostalCode + "<br />");
                }
            }
            if (c != null)
            {
                sb.Append(c.DisplayName + "<br />");
            }

            if (Phone.Trim().Length > 0)
            {
                sb.Append(Phone + "<br />");
            }
            if (Fax.Trim().Length > 0)
            {
                sb.Append("Fax: " + Fax + "<br />");
            }
            if (WebSiteUrl.Trim().Length > 0)
            {
                sb.Append(WebSiteUrl + "<br />");
            }
            return sb.ToString();
        }


    }

}