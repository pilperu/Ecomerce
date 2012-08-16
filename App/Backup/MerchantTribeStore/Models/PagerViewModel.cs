using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MerchantTribeStore.Models
{
    public class PagerViewModel
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int PageSize { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int CurrentPage { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int TotalItems { get; set; }

        public int TotalPages
        {
            get { return MerchantTribe.Web.Paging.TotalPages(this.TotalItems, this.PageSize); }
        }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PagerUrlFormat { get; set; }
        private string _PagerUrlFormatFirst = string.Empty;
        // Url for first page 
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PagerUrlFormatFirst 
        {
            get
            {
                if (_PagerUrlFormatFirst.Trim().Length < 1) return PagerUrlFormat;
                return _PagerUrlFormatFirst;
            }
            set { _PagerUrlFormatFirst = value; }         
        }

        public PagerViewModel()
        {
            PageSize = 9;
            CurrentPage = 1;
            TotalItems = 0;
            PagerUrlFormat = "";
            PagerUrlFormatFirst = "";
        }
    }
}