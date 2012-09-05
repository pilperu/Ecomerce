using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MerchantTribeStore.Models
{
    public class MiniPagerViewModel
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int TotalPages { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int CurrentPage { get; set; }
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

        public MiniPagerViewModel()
        {
            CurrentPage = 1;
            TotalPages = 0;
            PagerUrlFormat = "";
            PagerUrlFormatFirst = "";
        }
    }
}