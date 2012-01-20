using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Web;
using System.Runtime.Serialization;
using MerchantTribe.CommerceDTO.v1.Catalog;
using MerchantTribe.CommerceDTO.v1;
using MerchantTribe.CommerceDTO.v1.Shipping;
using System.Linq;

namespace MerchantTribe.Commerce.Catalog
{

    [Serializable]
    public class Product : IEquatable<Product>, Orders.IPurchasable, Content.IReplaceable
    {

        #region "Custom Properties"
        private CustomPropertyCollection _CustomProperties = new CustomPropertyCollection();
        public CustomPropertyCollection CustomProperties
        {
            get { return _CustomProperties; }
            set { _CustomProperties = value; }
        }
        public bool CustomPropertyExists(string devId, string propertyKey)
        {
            bool result = false;
            for (int i = 0; i <= _CustomProperties.Count - 1; i++)
            {
                if (_CustomProperties[i].DeveloperId.Trim().ToLower() == devId.Trim().ToLower())
                {
                    if (_CustomProperties[i].Key.Trim().ToLower() == propertyKey.Trim().ToLower())
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
        public void CustomPropertySet(string devId, string key, string value)
        {
            bool found = false;

            for (int i = 0; i <= _CustomProperties.Count - 1; i++)
            {
                if (_CustomProperties[i].DeveloperId.Trim().ToLower() == devId.Trim().ToLower())
                {
                    if (_CustomProperties[i].Key.Trim().ToLower() == key.Trim().ToLower())
                    {
                        _CustomProperties[i].Value = value;
                        found = true;
                        break;
                    }
                }
            }

            if (found == false)
            {
                _CustomProperties.Add(new CustomProperty(devId, key, value));
            }
        }
        public string CustomPropertyGet(string devId, string key)
        {
            string result = string.Empty;

            for (int i = 0; i <= _CustomProperties.Count - 1; i++)
            {
                if (_CustomProperties[i].DeveloperId.Trim().ToLower() == devId.Trim().ToLower())
                {
                    if (_CustomProperties[i].Key.Trim().ToLower() == key.Trim().ToLower())
                    {
                        result = _CustomProperties[i].Value;
                        break;
                    }
                }
            }

            return result;
        }
        public bool CustomPropertyRemove(string devId, string key)
        {
            bool result = false;

            for (int i = 0; i <= _CustomProperties.Count - 1; i++)
            {
                if (_CustomProperties[i].DeveloperId.Trim().ToLower() == devId.Trim().ToLower())
                {
                    if (_CustomProperties[i].Key.Trim().ToLower() == key.Trim().ToLower())
                    {
                        _CustomProperties.Remove(_CustomProperties[i]);
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
        public string CustomPropertiesToXml()
        {
            string result = string.Empty;

            try
            {
                StringWriter sw = new StringWriter();
                XmlSerializer xs = new XmlSerializer(_CustomProperties.GetType());
                xs.Serialize(sw, _CustomProperties);
                result = sw.ToString();
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
                result = string.Empty;
            }

            return result;
        }
        public bool CustomPropertiesFromXml(string data)
        {
            bool result = false;

            try
            {
                StringReader tr = new StringReader(data);
                XmlSerializer xs = new XmlSerializer(_CustomProperties.GetType());
                _CustomProperties = (CustomPropertyCollection)xs.Deserialize(tr);
                if (_CustomProperties != null)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
                _CustomProperties = new CustomPropertyCollection();
                result = false;
            }
            return result;
        }
        #endregion

        public const int ProductNameMaxLength = 255;
        public const int SkuMaxLength = 50;
        public string Bvin { get; set; }
        public System.DateTime LastUpdated {get;set;} //System.DateTime.MinValue;
        public string Sku { get; set; }
        public string ProductName { get; set; }
        public string ProductTypeId { get; set; }        
        public decimal ListPrice { get; set; }        
        public decimal SitePrice { get; set; }
        public string SitePriceOverrideText { get; set; }
        public decimal SiteCost { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public bool TaxExempt { get; set; }
        public long TaxSchedule { get; set; }
        public Shipping.ShippableItem ShippingDetails { get; set; }
        public Shipping.ShippingMode ShippingMode
        {
            get
            {
                return this.ShippingDetails.ShippingSource;
            }
            set { this.ShippingDetails.ShippingSource = value; }
        }
        public ProductStatus Status { get; set; }
        public string ImageFileSmall { get; set; }
        public string ImageFileSmallAlternateText { get; set; }
        public string ImageFileMedium { get; set; }
        public string ImageFileMediumAlternateText { get; set; }
        public System.DateTime CreationDate
        {
            get { return CreationDateUtc.ToLocalTime(); }
            set { CreationDateUtc = value.ToUniversalTime(); }
        }
        [XmlIgnore()]
        public System.DateTime CreationDateUtc { get; set; }
        public int MinimumQty { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string ManufacturerId { get; set; }
        public string VendorId { get; set; }
        public bool GiftWrapAllowed { get; set; }
        public decimal GiftWrapPrice { get; set; }
        public string Keywords { get; set; }
        public string TemplateName { get; set; }
        public string PreContentColumnId { get; set; }
        public string PostContentColumnId { get; set; }
        public string UrlSlug { get; set; }        
        public bool IsNew
        {
            get { return ((System.DateTime.Now - CreationDate).Days <= WebAppSettings.NewProductBadgeDays); }
        }
        public string PreTransformLongDescription { get; set; }
        //public string ProductUrl { get; set; }
        public Catalog.ProductInventoryMode InventoryMode { get; set; }
        public bool IsAvailableForSale { get; set; }        
        public bool Featured { get; set; }
        public bool AllowReviews { get; set; }
        public List<ProductDescriptionTab> Tabs { get; set; }
        public void TabsFromXml(string xml)
        {
            if (xml == string.Empty) return;

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);

            // add blocks from xml
            XmlNodeList tabNodes;
            tabNodes = xdoc.SelectNodes("/DescriptionTabs/ProductDescriptionTab");

            this.Tabs.Clear();
            if (tabNodes != null)
            {
                for (int i = 0; i <= tabNodes.Count - 1; i++)
                {
                    ProductDescriptionTab t = new ProductDescriptionTab();
                    t.FromXmlString(tabNodes[i].OuterXml);
                    this.Tabs.Add(t);
                }
            }
        }
        public long StoreId { get; set; }
        public string TabsToXml()
        {
            string result = string.Empty;

            try
            {
                XmlWriterSettings writerSettings = new XmlWriterSettings();
                string response = string.Empty;
                StringBuilder sb = new StringBuilder();
                writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
                XmlWriter xw = XmlWriter.Create(sb, writerSettings);

                xw.WriteStartElement("DescriptionTabs");
                foreach (ProductDescriptionTab t in this.Tabs)
                {
                    t.ToXmlWriter(ref xw);
                }
                xw.WriteEndElement();
                xw.Flush();
                xw.Close();
                result = sb.ToString();
            }

            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
            }

            return result;
        }
        public bool IsTaxable()
        {

            bool chargeTaxOnNonShipping = false;

            if (this.TaxExempt)
            {
                return false;
            }

            if (this.ShippingDetails.IsNonShipping)
            {
                if (!chargeTaxOnNonShipping)
                {
                    return false;
                }
            }

            return true;
        }

        public bool HasOptions()
        {
            return this.Options.Count > 0;
        }
        public bool HasVariants()
        {
            return this.Variants.Count > 0;
        }
        protected bool SortingEnabled = true;

        //Sub Items
        //public Collection<string> Categories { get; private set; }
        public List<ProductImage> Images { get; set; }
        public List<ProductReview> Reviews { get; set; }
        public List<ProductReview> ReviewsApproved
        {
            get
            {
                List<ProductReview> result = new List<ProductReview>();
                foreach (ProductReview p in Reviews)
                {
                    if (p.Approved) { result.Add(p); }
                }
                return result;
            }
        }                
        public OptionList Options { get; set; }
        public VariantList Variants { get; set; }
        //public void PreloadOptionItems(List<OptionItem> items)
        //{
        //    foreach (Option o in this.Options)
        //    {
        //        o.LoadItemsFromList(items);
        //    }
        //}

        public Product()
        {
            this.Bvin = string.Empty;
            this.LastUpdated = System.DateTime.MinValue;
            this.Sku = string.Empty;
            this.ProductName = string.Empty;
            this.ProductTypeId = string.Empty;
            this.Images = new List<ProductImage>();
            this.ListPrice = 0m;
            this.Reviews = new List<ProductReview>();
            this.SitePrice = 0m;
            this.SitePriceOverrideText = string.Empty;
            this.SiteCost = 0m;
            this.MetaKeywords = string.Empty;
            this.MetaDescription = string.Empty;
            this.MetaTitle = string.Empty;
            this.TaxExempt = false;
            this.TaxSchedule = -1;
            this.Status = ProductStatus.Active;
            this.ImageFileSmall = string.Empty;
            this.ImageFileMedium = string.Empty;
            this.ImageFileSmallAlternateText = string.Empty;
            this.ImageFileMediumAlternateText = string.Empty;
            this.CreationDateUtc = DateTime.UtcNow;
            this.MinimumQty = 1;
            this.ShortDescription = string.Empty;
            this.LongDescription = string.Empty;
            this.ManufacturerId = string.Empty;
            this.VendorId = string.Empty;
            this.GiftWrapAllowed = false;
            this.Keywords = string.Empty;
            this.TemplateName = "";
            this.PreContentColumnId = string.Empty;
            this.PostContentColumnId = string.Empty;
            this.UrlSlug = string.Empty;
            //this.Categories = new Collection<string>();
            this.PreTransformLongDescription = string.Empty;            
            this.InventoryMode = ProductInventoryMode.AlwayInStock;
            this.GiftWrapPrice = 0m;
            this.Options = new OptionList();
            this.Variants = new VariantList();
            this.ShippingDetails = new Shipping.ShippableItem();
            this.Featured = false;
            this.AllowReviews = false;
            this.Tabs = new List<ProductDescriptionTab>();
            this.StoreId = 0;
            this.IsAvailableForSale = true;
        }


        // Pricing Functions
        private bool GetAdminGiftWrappable()
        {
            return WebAppSettings.GiftWrapAll;
        }
        private decimal GetAdminGiftWrapPrice()
        {
            return WebAppSettings.GiftWrapCharge;
        }
        public decimal GetCurrentPrice(string userBvin, decimal adjustment, Orders.LineItem li, string variantId)
        {
            decimal result = this.SitePrice;

            // pull basic site price from product
            if (variantId != string.Empty)
            {
                Catalog.Variant v = this.Variants.FindByBvin(variantId);
                if (v != null)
                {
                    if (v.Price >= 0) result = v.Price;
                }
            }

            if (li == null) return result;
            //if (li.AssociatedProduct == null) return result;            

            //if (li.AssociatedProduct.IsKit)
            //{
            //    KitSelectionData ksd = Services.KitService.GetKitSelectionData(li.KitSelections);
            //    if (li.KitSelections.SelectedValues.Count == 0)
            //    {
            //        result = ksd.DefaultPrice;
            //    }
            //    else
            //    {
            //        result = ksd.TotalPrice;
            //    }
            //    if (li != null)
            //    {
            //        if (li.Bvin.Trim().Length > 0)
            //        {
            //            result = ksd.TotalPrice;
            //        }
            //    }
            //}

            //else
            //{
            //    result = li.BasePrice;
            //    BusinessRules.ProductTaskContext c = new BusinessRules.ProductTaskContext(this);
            //    c.UserId = userBvin;
            //    c.SetProduct(li.AssociatedProduct);

            //    if (adjustment > 0)
            //    {
            //        c.UserPrice.AddAdjustment(new PriceAdjustment(adjustment, "Custom Adjustment"));
            //    }
            //    BusinessRules.Workflow.RunByName((BusinessRules.TaskContext)c, BusinessRules.WorkflowNames.ProductPricing);
            //    foreach (BusinessRules.WorkflowMessage item in c.Errors)
            //    {
            //        EventLog.LogEvent(item.Name, item.Description, Metrics.EventLogSeverity.Error);
            //    }
            //    result = c.UserPrice.PriceWithAdjustments();
            //}

            return result;
        }
        //public string GetSitePriceForDisplay(decimal adjustment)
        //{
        //    return GetSitePriceForDisplay(adjustment, string.Empty);
        //}
        //public string GetSitePriceForDisplay(decimal adjustment, string variantId)
        //{
        //    BusinessRules.ProductTaskContext c = new BusinessRules.ProductTaskContext(this);
        //    c.UserId = SessionManager.GetCurrentUserId();
        //    if (variantId != string.Empty)
        //    {
        //        Catalog.Variant v = this.Variants.FindByBvin(variantId);
        //        if (v != null)
        //        {
        //            if (v.Price >= 0) c.UserPrice.BasePrice = v.Price;
        //        }
        //    }
        //    if (adjustment > 0)
        //    {
        //        c.UserPrice.AddAdjustment(adjustment, "Custom Adjustment");
        //    }
        //    BusinessRules.Workflow.RunByName((BusinessRules.TaskContext)c, BusinessRules.WorkflowNames.ProductPricing);
        //    return c.UserPrice.DisplayPrice();

        //}

        // Choice/Option Related Function
                
        //public Collection<Product> GetUpSells()
        //{
        //    return Mapper.FindByUpSellId(this.Bvin);
        //}
        //public Collection<Product> GetUpSellsAdmin()
        //{
        //    return Mapper.FindByUpSellIdAdmin(this.Bvin);
        //}
        //public int GetUpSellCount()
        //{
        //    return Mapper.GetUpSellCount(this.Bvin);
        //}
        //public int GetUpSellCountAdmin()
        //{
        //    return Mapper.GetUpSellCountAdmin(this.Bvin);
        //}
        //public int GetCrossSellCount()
        //{
        //    return Mapper.GetCrossSellCount(this.Bvin);
        //}
        //public int GetCrossSellCountAdmin()
        //{
        //    return Mapper.GetCrossSellCountAdmin(this.Bvin);
        //}
        //public Collection<Product> GetCrossSells()
        //{
        //    return Mapper.FindByCrossSellId(this.Bvin);
        //}
        //public Collection<Product> GetCrossSellsAdmin()
        //{
        //    return Mapper.FindByCrossSellIdAdmin(this.Bvin);
        //}

        //public static Collection<Catalog.Product> StoreSearch(ProductStoreSearchCriteria criteria, string shopperId, bool ignoreMetrics, int startRowIndex, int maximumRows, ref int rowCount, Accounts.Store currentStore)
        //{
        //    // Record Keyword Searches for Reports
        //    if (currentStore.MetricsRecordSearches)
        //    {
        //        if (!ignoreMetrics)
        //        {
        //            string searchPhrase = criteria.Keyword;
        //            Metrics.SearchQuery.Insert(new Metrics.SearchQuery(shopperId, searchPhrase));
        //        }
        //    }

        //    return Mapper.StoreSearch(criteria, startRowIndex, maximumRows, ref rowCount);
        //}

        //public static Collection<ProductSearchResultGroup> SearchByComplexPhrase(ProductSearchCriteria criteria, string shopperId, bool ignoreMetrics, int maxResults, bool recordStoreMetrics)
        //{
        //    int maximumresults;
        //    if (maxResults == 0)
        //    {
        //        maximumresults = int.MaxValue;
        //    }
        //    else
        //    {
        //        maximumresults = maxResults;
        //    }

        //    int rowsReturned = 0;

        //    Collection<ProductSearchResultGroup> result = new Collection<ProductSearchResultGroup>();
        //    ProductSearchResultGroup resultGroup = null;
        //    try
        //    {

        //        string searchPhrase = criteria.Keyword;

        //        // Record Keyword Searches for Reports
        //        if (recordStoreMetrics == true)
        //        {
        //            if (ignoreMetrics == false)
        //            {
        //                Metrics.SearchQuery.Insert(new Metrics.SearchQuery(shopperId, searchPhrase));
        //            }
        //        }

        //        if (searchPhrase.IndexOfAny(",".ToCharArray()) > 0 | searchPhrase.IndexOfAny(" ".ToCharArray()) > 0)
        //        {

        //            // Try to match full phrase first
        //            Catalog.ProductSearchCriteria exactCriteria = criteria.Clone();
        //            exactCriteria.Status = Catalog.ProductStatus.Active;
        //            Collection<Catalog.Product> exactItems
        //                = Catalog.Product.FindByCriteria(exactCriteria, 0, maximumresults, ref rowsReturned);
        //            maximumresults -= rowsReturned;
        //            resultGroup = new ProductSearchResultGroup(" Matches for &quot;" + searchPhrase + "&quot;", exactItems);
        //            if (maximumresults <= 0)
        //            {
        //                resultGroup.InfoMessage = "Results were limited to " + maxResults + " items.";
        //            }
        //            result.Add(resultGroup);

        //            // We can try multiple word searches
        //            System.Collections.Specialized.StringCollection wordsToSearch = new System.Collections.Specialized.StringCollection();

        //            // Break phrase into individual keywords
        //            if (searchPhrase.IndexOfAny(",".ToCharArray()) > 0)
        //            {
        //                string[] commalist = searchPhrase.Split(",".ToCharArray());
        //                for (int i = 0; i <= commalist.Length - 1; i++)
        //                {
        //                    wordsToSearch.Add(commalist[i].Trim());
        //                }
        //            }
        //            else
        //            {
        //                // no comma separated list so try separating by spaces
        //                if (searchPhrase.IndexOfAny(" ".ToCharArray()) > 0)
        //                {
        //                    string[] spaceList = searchPhrase.Split(" ".ToCharArray());
        //                    for (int i = 0; i <= spaceList.Length - 1; i++)
        //                    {
        //                        wordsToSearch.Add(spaceList[i].Trim());
        //                    }
        //                }
        //            }

        //            if (wordsToSearch.Count > 1)
        //            {
        //                for (int i = 0; i <= wordsToSearch.Count - 1; i++)
        //                {
        //                    if (maximumresults > 0)
        //                    {
        //                        Catalog.ProductSearchCriteria itemCriteria = criteria.Clone();
        //                        itemCriteria.Status = Catalog.ProductStatus.Active;
        //                        itemCriteria.Keyword = wordsToSearch[i];
        //                        Collection<Catalog.Product> items
        //                            = Catalog.Product.FindByCriteria(itemCriteria, 0, maximumresults, ref rowsReturned);

        //                        maximumresults -= rowsReturned;

        //                        if (items.Count > 0)
        //                        {
        //                            // Exact matches found
        //                            resultGroup = new ProductSearchResultGroup("Exact Matches for &quot;" + wordsToSearch[i] + "&quot;", items);
        //                            if (maximumresults <= 0)
        //                            {
        //                                resultGroup.InfoMessage = "Results were limited to " + maxResults + " items.";
        //                            }
        //                            result.Add(resultGroup);
        //                        }
        //                        else
        //                        {
        //                            if (maximumresults > 0)
        //                            {
        //                                // No exact matches, try partial matches
        //                                items = Catalog.Product.FindByCriteria(itemCriteria, 0, maximumresults, ref rowsReturned);
        //                                maximumresults -= rowsReturned;
        //                                resultGroup = new ProductSearchResultGroup("Matches for &quot;" + wordsToSearch[i] + "&quot;", items);
        //                                if (maximumresults <= 0)
        //                                {
        //                                    resultGroup.InfoMessage = "Results were limited to " + maxResults + " items.";
        //                                }
        //                                result.Add(resultGroup);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        else
        //        {
        //            if (maximumresults > 0)
        //            {
        //                // No multiple words, try a partial match on the single word only    
        //                Catalog.ProductSearchCriteria singleCriteria = criteria.Clone();
        //                singleCriteria.Status = Catalog.ProductStatus.Active;
        //                Collection<Catalog.Product> items
        //                    = Catalog.Product.FindByCriteria(singleCriteria, 0, maximumresults, ref rowsReturned);
        //                maximumresults -= rowsReturned;
        //                resultGroup = new ProductSearchResultGroup("Matches for &quot;" + searchPhrase + "&quot;", items);
        //                if (maximumresults <= 0)
        //                {
        //                    resultGroup.InfoMessage = "Results were limited to " + maxResults + " items.";
        //                }
        //                result.Add(resultGroup);
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        EventLog.LogEvent(ex);
        //    }

        //    return result;
        //}


        //public static bool CopyCategoryPlacement(string oldBvin, string newBvin)
        //{
        //    Collection<Catalog.Category> categories = Catalog.Product.GetCategories(oldBvin);
        //    bool result = true;
        //    //foreach (Catalog.Category category in categories) {
        //    //    if (!Catalog.Category.AddProduct(category.Bvin, newBvin)) {
        //    //        result = false;
        //    //    }
        //    //}

        //    //if (!result) {
        //    //    Catalog.Category.RemoveProductFromAll(newBvin);
        //    //}

        //    return result;
        //}
        public Product Clone(bool cloneProductChoicesAndInputs, bool cloneProductImages)
        {
            Product result = new Product();

            result.AllowReviews = this.AllowReviews;
            result.Bvin = string.Empty;
            result.CreationDateUtc = DateTime.UtcNow;
            
            foreach (CustomProperty prop in this.CustomProperties)
            {
                result.CustomProperties.Add(prop.DeveloperId, prop.Key, prop.Value);                
            }

            result.Featured = this.Featured;
            result.GiftWrapAllowed = this.GiftWrapAllowed;
            result.GiftWrapPrice = this.GiftWrapPrice;
            if (cloneProductImages == true)
            {
                result.ImageFileMedium = this.ImageFileMedium;
                result.ImageFileMediumAlternateText = this.ImageFileMediumAlternateText;
                result.ImageFileSmall = this.ImageFileSmall;
                result.ImageFileSmallAlternateText = this.ImageFileSmallAlternateText;


                foreach (var img in this.Images)
                {
                    ProductImage imgClone = img.Clone();
                    imgClone.ProductId = string.Empty;
                    result.Images.Add(imgClone);
                }                
            }
            result.InventoryMode = this.InventoryMode;
            result.IsAvailableForSale = this.IsAvailableForSale;
            result.Keywords = this.Keywords;
            result.ListPrice = this.ListPrice;
            result.LongDescription = this.LongDescription;
            result.ManufacturerId = this.ManufacturerId;
            result.MetaDescription = this.MetaDescription;
            result.MetaKeywords = this.MetaKeywords;
            result.MetaTitle = this.MetaTitle;
            result.MinimumQty = this.MinimumQty;
            
            result.PostContentColumnId = this.PostContentColumnId;
            result.PreContentColumnId = this.PreContentColumnId;
            result.PreTransformLongDescription = this.PreTransformLongDescription;
            result.ProductName = this.ProductName;
            result.ProductTypeId = this.ProductTypeId;
            
            result.ShippingDetails.ExtraShipFee = this.ShippingDetails.ExtraShipFee;
            result.ShippingDetails.Height = this.ShippingDetails.Height;
            result.ShippingDetails.IsNonShipping = this.ShippingDetails.IsNonShipping;
            result.ShippingDetails.Length = this.ShippingDetails.Length;
            result.ShippingDetails.ShippingScheduleId = this.ShippingDetails.ShippingScheduleId;
            result.ShippingDetails.ShippingSource = this.ShippingDetails.ShippingSource;
            this.ShippingDetails.ShippingSourceAddress.CopyTo(result.ShippingDetails.ShippingSourceAddress);
            result.ShippingDetails.ShippingSourceId = this.ShippingDetails.ShippingSourceId;
            result.ShippingDetails.ShipSeparately = this.ShippingDetails.ShipSeparately;
            result.ShippingDetails.Weight = this.ShippingDetails.Weight;
            result.ShippingDetails.Width = this.ShippingDetails.Width;

            result.ShippingMode = this.ShippingMode;
            result.ShortDescription = this.ShortDescription;
            result.SiteCost = this.SiteCost;
            result.SitePrice = this.SitePrice;
            result.SitePriceOverrideText = this.SitePriceOverrideText;
            result.Sku = this.Sku;
            result.Status = this.Status;
            result.StoreId = this.StoreId;

            foreach (ProductDescriptionTab tab in this.Tabs)
            {
                result.Tabs.Add(new ProductDescriptionTab()
                {
                    HtmlData = tab.HtmlData,
                    SortOrder = tab.SortOrder,
                    TabTitle = tab.TabTitle,
                    LastUpdated = DateTime.UtcNow
                });
            }

            result.TaxExempt = this.TaxExempt;
            result.TaxSchedule = this.TaxSchedule;
            result.UrlSlug = string.Empty;
            result.VendorId = this.VendorId;

            if (cloneProductChoicesAndInputs == true)
            {
                foreach (var opt in this.Options)
                {
                    Option c = opt.Clone();
                    result.Options.Add(c);
                }                
                //result.Variants = this.Variants;
            }
                                                      
            result.Bvin = System.Guid.NewGuid().ToString();
        

                
            return result;
        }

        public Product Clone()
        {
            return Clone(true, true);
        }
        public Content.BVValidationResult Validate()
        {
            Content.BVValidationResult result = new Content.BVValidationResult();
            result.Success = true;
            return result;
        }

        public List<Content.HtmlTemplateTag> GetReplaceableTags(MerchantTribeApplication app)
        {
            List<Content.HtmlTemplateTag> result = new List<Content.HtmlTemplateTag>();

            result.Add(new Content.HtmlTemplateTag("[[Product.CreationDate]]", this.CreationDate.ToString()));
            result.Add(new Content.HtmlTemplateTag("[[Product.ExtraShipFee]]", this.ShippingDetails.ExtraShipFee.ToString("c")));
            result.Add(new Content.HtmlTemplateTag("[[Product.ImageFileSmall]]", this.ImageFileSmall));
            result.Add(new Content.HtmlTemplateTag("[[Product.ImageFileMedium]]", this.ImageFileMedium));

            string available = Content.SiteTerms.GetTerm(Content.SiteTermIds.InStock);
            string notAvailable = Content.SiteTerms.GetTerm(Content.SiteTermIds.OutOfStock);

            if (string.IsNullOrEmpty(available))
            {
                available = "Available";
            }

            if (string.IsNullOrEmpty(notAvailable))
            {
                notAvailable = "Backordered";
            }

            result.Add(new Content.HtmlTemplateTag("[[Product.Keywords]]", this.Keywords));
            result.Add(new Content.HtmlTemplateTag("[[Product.ListPrice]]", this.ListPrice.ToString("c")));
            result.Add(new Content.HtmlTemplateTag("[[Product.LongDescription]]", this.LongDescription));
            result.Add(new Content.HtmlTemplateTag("[[Product.ManufacturerId]]", this.ManufacturerId));
            result.Add(new Content.HtmlTemplateTag("[[Product.MetaKeywords]]", this.MetaKeywords));
            result.Add(new Content.HtmlTemplateTag("[[Product.MetaDescription]]", this.MetaDescription));
            result.Add(new Content.HtmlTemplateTag("[[Product.MetaTitle]]", this.MetaTitle));
            result.Add(new Content.HtmlTemplateTag("[[Product.MinimumQty]]", this.MinimumQty.ToString("#")));
            result.Add(new Content.HtmlTemplateTag("[[Product.NonShipping]]", this.ShippingDetails.IsNonShipping.ToString()));
            result.Add(new Content.HtmlTemplateTag("[[Product.PostContentColumnId]]", this.PostContentColumnId));
            result.Add(new Content.HtmlTemplateTag("[[Product.PreContentColumnId]]", this.PreContentColumnId));
            result.Add(new Content.HtmlTemplateTag("[[Product.PreTransformLongDescription]]", this.PreTransformLongDescription));
            result.Add(new Content.HtmlTemplateTag("[[Product.ProductName]]", this.ProductName));
            result.Add(new Content.HtmlTemplateTag("[[Product.ProductTypeId]]", this.ProductTypeId));
            result.Add(new Content.HtmlTemplateTag("[[Product.ShippingHeight]]", this.ShippingDetails.Height.ToString("#.##")));
            result.Add(new Content.HtmlTemplateTag("[[Product.ShippingLength]]", this.ShippingDetails.Length.ToString("#.##")));
            result.Add(new Content.HtmlTemplateTag("[[Product.ShippingWeight]]", this.ShippingDetails.Weight.ToString("#.##")));
            result.Add(new Content.HtmlTemplateTag("[[Product.ShippingWidth]]", this.ShippingDetails.Width.ToString("#.##")));
            result.Add(new Content.HtmlTemplateTag("[[Product.ShipSeparately]]", this.ShippingDetails.ShipSeparately.ToString()));
            result.Add(new Content.HtmlTemplateTag("[[Product.ShortDescription]]", this.ShortDescription));
            result.Add(new Content.HtmlTemplateTag("[[Product.SiteCost]]", this.SiteCost.ToString("c")));
            result.Add(new Content.HtmlTemplateTag("[[Product.SitePrice]]", this.SitePrice.ToString("c")));
            result.Add(new Content.HtmlTemplateTag("[[Product.SitePriceOverrideText]]", this.SitePriceOverrideText));
            result.Add(new Content.HtmlTemplateTag("[[Product.Sku]]", this.Sku));
            result.Add(new Content.HtmlTemplateTag("[[Product.TaxExempt]]", this.TaxExempt.ToString()));
            //result.Add(new Content.EmailTemplateTag("[[Product.TemplateName]]", this.TemplateName));
            result.Add(new Content.HtmlTemplateTag("[[Product.TypeProperties]]", this.GetTypeProperties(app)));
            result.Add(new Content.HtmlTemplateTag("[[Product.TypePropertiesDropShipper]]", this.GetTypeProperties(true, app)));
            result.Add(new Content.HtmlTemplateTag("[[Product.VendorId]]", this.VendorId));

            return result;
        }

        public string GetTypeProperties(MerchantTribeApplication app)
        {
            return GetTypeProperties(false, app);
        }
        private string GetPropertyValueFromList(List<ProductPropertyValue> values, long propertyId)
        {
            string temp = values.Where(y => y.PropertyID == propertyId).Select(y => y.StringValue).FirstOrDefault();
            if (temp != null) return temp;
            return string.Empty;
        }        
        public string GetTypeProperties(bool forDropShipper, MerchantTribeApplication app)
        {            
            string productId = this.Bvin;
            string productTypeId = this.ProductTypeId;

            List<ProductPropertyValue> propertyValues = app.CatalogServices.ProductPropertyValues.FindByProductId(this.Bvin);            
            System.Collections.Generic.List<Catalog.ProductProperty> props = app.CatalogServices.ProductPropertiesFindForType(productTypeId);

            List<ProductProperty> displayable = props.Where(y => (y.DisplayToDropShipper == true && forDropShipper == true)
                                                                || y.DisplayOnSite == true).ToList();

            if (displayable.Count < 1) return string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"typedisplay\">");
            foreach (var prop in displayable)
            {
                    string currentValue = app.CatalogServices.FormatProductPropertyChoiceValue(prop, 
                                                                    GetPropertyValueFromList(propertyValues, prop.Id));
                    // If text property is empty, do not display
                    if ((prop.TypeCode == Catalog.ProductPropertyType.TextField) && (currentValue == string.Empty))
                    {
                        continue;
                    }
                    sb.Append("<li>");
                    sb.Append("<span class=\"productpropertylabel\">");
                    sb.Append(prop.DisplayName);
                    sb.Append("</span>");
                    sb.Append("<span class=\"productpropertyvalue\">");
                    sb.Append(currentValue);
                    sb.Append("</span>");
                    sb.Append("</li>");                
            }
            sb.Append("</ul>");
            
            return sb.ToString();
        }

        // Equality Functions
        private static Collection<T> CopyCollection<T>(Collection<T> data)
        {
            Collection<T> result = new Collection<T>();
            foreach (T item in data)
            {
                result.Add(item);
            }
            return result;
        }
        bool System.IEquatable<Product>.Equals(Product other)
        {
            if (this.Bvin != other.Bvin)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        public string GetCartDestinationUrl(Orders.LineItem li)
        {
            if (WebAppSettings.RedirectToCartAfterAddProduct)
            {
                //if ((this.GetUpSellCount() > 0) && (WebAppSettings.DisplayUpSellsWhenAddingItemToCart))
                //{
                //    return "~/AdditionalProductInfo.aspx?id=" + System.Web.HttpUtility.UrlEncode(li.Bvin);
                //}
                //else if ((this.GetCrossSellCount() > 0) && (WebAppSettings.DisplayCrossSellsWhenAddingItemToCart))
                //{
                //    return "~/AdditionalProductAccessories.aspx?id=" + System.Web.HttpUtility.UrlEncode(li.Bvin);
                //}
                //else
                //{
                return "~/Cart.aspx";
                //}
            }
            else
            {
                return "";
            }
        }

        // DTO
        public void FromDto(ProductDTO dto)
        {
            this.AllowReviews = dto.AllowReviews;
            this.Bvin = dto.Bvin ?? string.Empty;
            this.CreationDateUtc = dto.CreationDateUtc;

            this.CustomProperties.Clear();
            foreach (CustomPropertyDTO prop in dto.CustomProperties)
            {
                CustomProperty prop1 = new CustomProperty();
                prop1.FromDto(prop);
                this.CustomProperties.Add(prop1);
            }
            
            this.Featured = dto.Featured;
            this.GiftWrapAllowed = dto.GiftWrapAllowed;
            this.GiftWrapPrice = dto.GiftWrapPrice;
            this.ImageFileMedium = dto.ImageFileMedium ?? string.Empty;
            this.ImageFileMediumAlternateText = dto.ImageFileMediumAlternateText ?? string.Empty;
            this.ImageFileSmall = dto.ImageFileSmall ?? string.Empty;
            this.ImageFileSmallAlternateText = dto.ImageFileSmallAlternateText ?? string.Empty;
            this.InventoryMode = (ProductInventoryMode)((int)dto.InventoryMode);
            this.IsAvailableForSale = dto.IsAvailableForSale;
            this.Keywords = dto.Keywords ?? string.Empty;
            this.ListPrice = dto.ListPrice;
            this.LongDescription = dto.LongDescription ?? string.Empty;
            this.ManufacturerId = dto.ManufacturerId ?? string.Empty;
            this.MetaDescription = dto.MetaDescription ?? string.Empty;
            this.MetaKeywords = dto.MetaKeywords ?? string.Empty;
            this.MetaTitle = dto.MetaTitle ?? string.Empty;
            this.MinimumQty = dto.MinimumQty;
            //this.Options = dto.Options;
            this.PostContentColumnId = dto.PostContentColumnId ?? string.Empty;
            this.PreContentColumnId = dto.PreContentColumnId ?? string.Empty;
            this.PreTransformLongDescription = dto.PreTransformLongDescription ?? string.Empty;
            this.ProductName = dto.ProductName ?? string.Empty;
            this.ProductTypeId = dto.ProductTypeId ?? string.Empty;
            this.ShippingDetails.FromDto(dto.ShippingDetails);
            this.ShippingMode = (Shipping.ShippingMode)((int)dto.ShippingMode);
            this.ShortDescription = dto.ShortDescription ?? string.Empty;
            this.SiteCost = dto.SiteCost;
            this.SitePrice = dto.SitePrice;
            this.SitePriceOverrideText = dto.SitePriceOverrideText ?? string.Empty;
            this.Sku = dto.Sku ?? string.Empty;
            this.Status = (ProductStatus)((int)dto.Status);
            this.StoreId = dto.StoreId;

            this.Tabs.Clear();
            foreach (ProductDescriptionTabDTO t in dto.Tabs)
            {
                ProductDescriptionTab tab = new ProductDescriptionTab();
                tab.FromDto(t);
                this.Tabs.Add(tab);
            }
            
            this.TaxExempt = dto.TaxExempt;
            this.TaxSchedule = dto.TaxSchedule;
            this.UrlSlug = dto.UrlSlug ?? string.Empty;
            //this.Variants = dto.Variants;
            this.VendorId = dto.VendorId ?? string.Empty;           
        }
        public ProductDTO ToDto()
        {
            ProductDTO dto = new ProductDTO();

            dto.AllowReviews = this.AllowReviews;
            dto.Bvin = this.Bvin;
            dto.CreationDateUtc = this.CreationDateUtc;

            dto.CustomProperties = new List<CustomPropertyDTO>();
            foreach (CustomProperty prop in this.CustomProperties)
            {
                dto.CustomProperties.Add(prop.ToDto());
            }
            
            dto.Featured = this.Featured;
            dto.GiftWrapAllowed = this.GiftWrapAllowed;
            dto.GiftWrapPrice = this.GiftWrapPrice;
            dto.ImageFileMedium = this.ImageFileMedium;
            dto.ImageFileMediumAlternateText = this.ImageFileMediumAlternateText;
            dto.ImageFileSmall = this.ImageFileSmall;
            dto.ImageFileSmallAlternateText = this.ImageFileSmallAlternateText;
            dto.InventoryMode = (ProductInventoryModeDTO)((int)this.InventoryMode);
            dto.IsAvailableForSale = this.IsAvailableForSale;
            dto.Keywords = this.Keywords;
            dto.ListPrice = this.ListPrice;
            dto.LongDescription = this.LongDescription;
            dto.ManufacturerId = this.ManufacturerId;
            dto.MetaDescription = this.MetaDescription;
            dto.MetaKeywords = this.MetaKeywords;
            dto.MetaTitle = this.MetaTitle;
            dto.MinimumQty = this.MinimumQty;
            //dto.Options = this.Options;
            dto.PostContentColumnId = this.PostContentColumnId;
            dto.PreContentColumnId = this.PreContentColumnId;
            dto.PreTransformLongDescription = this.PreTransformLongDescription;
            dto.ProductName = this.ProductName;
            dto.ProductTypeId = this.ProductTypeId;
            dto.ShippingDetails = this.ShippingDetails.ToDto();
            dto.ShippingMode = (ShippingModeDTO)((int)this.ShippingMode);
            dto.ShortDescription = this.ShortDescription;
            dto.SiteCost = this.SiteCost;
            dto.SitePrice = this.SitePrice;
            dto.SitePriceOverrideText = this.SitePriceOverrideText;
            dto.Sku = this.Sku;
            dto.Status = (ProductStatusDTO)((int)this.Status);
            dto.StoreId = this.StoreId;

            foreach (ProductDescriptionTab tab in this.Tabs)
            {
                dto.Tabs.Add(tab.ToDto());
            }
            
            dto.TaxExempt = this.TaxExempt;
            dto.TaxSchedule = this.TaxSchedule;
            dto.UrlSlug = this.UrlSlug;
            //dto.Variants = this.Variants;
            dto.VendorId = this.VendorId; 

            return dto;
        }

        #region IPurchasable Members

        public Orders.PurchasableSnapshot AsPurchasable(Catalog.OptionSelectionList selectionData, MerchantTribeApplication app)
        {
            return AsPurchasable(selectionData, app, true);
        }
        public Orders.PurchasableSnapshot AsPurchasable(OptionSelectionList selectionData, MerchantTribeApplication app, bool calculateUserPrice)
        {
            Orders.PurchasableSnapshot result = new Orders.PurchasableSnapshot();

            result.BasePrice = this.SitePrice;
            result.Name = this.ProductName;
            result.Sku = this.Sku;
            result.IsTaxExempt = !this.IsTaxable();
            result.ProductId = this.Bvin;
            result.SelectionData = selectionData;
            result.ShippingDetails = this.ShippingDetails;
            switch (this.ShippingDetails.ShippingSource)
            {
                case Shipping.ShippingMode.ShipFromManufacturer:
                    result.ShippingDetails.ShippingSourceId = this.ManufacturerId;
                    Contacts.VendorManufacturer m = app.ContactServices.Manufacturers.Find(this.ManufacturerId);
                    if (m != null)
                    {
                        result.ShippingDetails.ShippingSourceAddress = m.Address;
                    }
                    break;
                case Shipping.ShippingMode.ShipFromSite:
                    result.ShippingDetails.ShippingSourceAddress = app.ContactServices.Addresses.FindStoreContactAddress();
                    break;
                case Shipping.ShippingMode.ShipFromVendor:
                    result.ShippingDetails.ShippingSourceId = this.VendorId;
                    Contacts.VendorManufacturer v = app.ContactServices.Vendors.Find(this.VendorId);
                    if (v != null)
                    {
                        result.ShippingDetails.ShippingSourceAddress = v.Address;
                    }
                    break;
            }
            result.TaxScheduleId = this.TaxSchedule;


            decimal basePriceToModify = result.BasePrice;

            // See if we have adjustments based on options
            decimal adjustments = 0;
            decimal weightAdjustments = 0;
            if (this.HasOptions())
            {
                adjustments = selectionData.GetPriceAdjustmentForSelections(this.Options);
                weightAdjustments = selectionData.GetWeightAdjustmentForSelections(this.Options);
            }
            this.ShippingDetails.Weight += weightAdjustments;

            // See if we need to use a variant price as base
            if (this.HasVariants())
            {
                Catalog.Variant v = this.Variants.FindBySelectionData(selectionData, this.Options);
                if (v != null)
                {
                    result.VariantId = v.Bvin;
                    if (v.Price >= 0) basePriceToModify = v.Price;
                    if (v.Sku.Trim().Length > 0) result.Sku = v.Sku;
                }
            }

            // See if we need to calculate user group discounts on base price
            if (calculateUserPrice)
            {
                Membership.CustomerAccount account = app.CurrentCustomer;
                if ((account != null) && (account.Bvin != string.Empty))
                {
                    if ((account.PricingGroupId != string.Empty))
                    {
                        Contacts.PriceGroup pricingGroup = app.ContactServices.PriceGroups.Find(account.PricingGroupId);
                        if (pricingGroup != null)
                        {
                            decimal groupPrice = this.SitePrice;
                            groupPrice = pricingGroup.GetAdjustedPriceForThisGroup(this.SitePrice,
                                                                                    this.ListPrice,
                                                                                    this.SiteCost);
                            basePriceToModify = groupPrice;
                        }
                    }
                }
            }


            // Record option price adjustments on modified base price
            result.BasePrice = basePriceToModify + adjustments;


            if (this.HasOptions())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<ul class=\"lineitemoptions\">");
                foreach (Catalog.Option opt in this.Options)
                {
                    string desc = opt.CartDescription(selectionData);
                    if (desc.Length > 0)
                    {
                        sb.Append("<li>" + desc + "</li>");
                    }
                }
                sb.Append("</ul>");
                result.Description = sb.ToString();
            }
            return result;
        }
        #endregion
        
    }

}
