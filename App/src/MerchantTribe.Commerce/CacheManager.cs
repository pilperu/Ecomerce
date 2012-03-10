using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce
{
    public class CacheManager
    {
        public static System.Web.Caching.Cache Current()
        {
            return System.Web.HttpRuntime.Cache;
        }
        
        private static void StoreItem<T>(string key, T item, int minutes)
        {
            var cache = Current();
            if (cache != null)
            {
                cache.Insert(key, item, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, minutes, 0));
            }
        }
        private static void ClearItem(string key)
        {
            var cache = Current();
            if (cache != null) cache.Remove(key);
        }

        // Store Id by Name
        public static void AddStoreIdByName(string storeName, long id)
        {
            StoreItem<long>("sid-" + storeName, id, 60);            
        }
        public static long GetStoreIdByName(string storeName)
        {
            var cache = Current();
            if (cache != null)
            {
                var i = cache["sid-" + storeName];
                if (i != null)
                {
                    return (long)i;
                }
            }
            return -1;
        }
        public static void ClearStoreIdByName(string storeName)
        { 
            ClearItem("sid-" + storeName);            
        }

        // Store
        public static void AddStore(Accounts.Store store)
        {
            StoreItem<Accounts.Store>("store-" + store.Id, store, 60);            
        }
        public static Accounts.Store GetStoreById(long id)
        {         
            var cache = Current();
            if (cache != null)
            {
                var i = cache["store-" + id];
                if (i != null)
                {
                    return (Accounts.Store)i;
                }
            }
            return null;
        }
        public static void ClearStoreById(long id)
        {
            ClearItem("store-" + id);
        }

        // Products
        public static void AddProduct(Catalog.Product p)
        {
            StoreItem<Catalog.Product>("product-" + p.Bvin + "-" + p.StoreId, p, 60);
        }
        public static Catalog.Product GetProduct(string bvin, long storeId)
        {
            var cache = Current();
            if (cache != null)
            {
                var i = cache["product-" + bvin + "-" + storeId];
                if (i != null)
                {
                    return (Catalog.Product)i;
                }
            }
            return null;
        }
        public static void ClearProduct(string bvin, long storeId)
        {
            ClearItem("product-" + bvin + "-" + storeId);
        }        

        // Generic String Cache
        public static void AddString(string key, string value, int minutesToCache)
        {
            StoreItem<string>(key, value, minutesToCache);
        }
        public static string GetString(string key)
        {
            var cache = Current();
            if (cache != null)
            {
                var i = cache[key];
                if (i != null)
                {
                    return (string)i;
                }
            }
            return null;
        }
        public static void ClearString(string key)
        {
            ClearItem(key);
        }
    }
}
