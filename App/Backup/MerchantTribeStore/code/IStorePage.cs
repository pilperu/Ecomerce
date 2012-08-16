using System.Web.Mvc;

namespace MerchantTribeStore
{
    public interface IStorePage
    {
        ViewDataDictionary ViewData { get; set; }
        void AddBodyClass(string css);
    }
}