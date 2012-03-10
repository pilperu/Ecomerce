using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Catalog
{
    public class CatalogService
    {
        private RequestContext context = null;

        public CategoryRepository Categories { get; private set; }
        public CategoryProductAssociationRepository CategoriesXProducts { get; private set; }
        public ProductRelationshipRepository ProductRelationships { get; private set; }
        public ProductImageRepository ProductImages { get; private set; }
        public ProductReviewRepository ProductReviews { get; private set; }
        public VariantRepository ProductVariants { get; private set; }
        public OptionRepository ProductOptions { get; private set; }
        public ProductOptionAssociationRepository ProductsXOptions { get; private set; }
        public ProductRepository Products { get; private set; }
        public ProductFileRepository ProductFiles { get; private set; }
        public ProductVolumeDiscountRepository VolumeDiscounts { get; private set; }
        public ProductPropertyValueRepository ProductPropertyValues { get; set; }
        public ProductInventoryRepository ProductInventories { get; private set; }
        public ProductTypeRepository ProductTypes { get; private set; }
        public ProductTypePropertyAssociationRepository ProductTypesXProperties { get; private set; }
        public ProductPropertyRepository ProductProperties { get; private set; }
        public WishListItemRepository WishListItems { get; private set; }

        public static CatalogService InstantiateForMemory(RequestContext c)
        {
            return new CatalogService(c,
                                     CategoryRepository.InstantiateForMemory(c),
                                     CategoryProductAssociationRepository.InstantiateForMemory(c),
                                     ProductRepository.InstantiateForMemory(c),
                                     ProductRelationshipRepository.InstantiateForMemory(c),
                                     ProductImageRepository.InstantiateForMemory(c),
                                     ProductReviewRepository.InstantiateForMemory(c),
                                     VariantRepository.InstantiateForMemory(c),
                                     OptionRepository.InstantiateForMemory(c),
                                     ProductOptionAssociationRepository.InstantiateForMemory(c),
                                     ProductFileRepository.InstantiateForMemory(c),
                                     ProductVolumeDiscountRepository.InstantiateForMemory(c),
                                     ProductPropertyValueRepository.InstantiateForMemory(c),
                                     ProductInventoryRepository.InstantiateForMemory(c),
                                     ProductTypeRepository.InstantiateForMemory(c),
                                     ProductTypePropertyAssociationRepository.InstantiateForMemory(c),
                                     ProductPropertyRepository.InstantiateForMemory(c),
                                     WishListItemRepository.InstantiateForMemory(c));
        }
        public static CatalogService InstantiateForDatabase(RequestContext c)
        {
            return new CatalogService(c,
                                     CategoryRepository.InstantiateForDatabase(c),
                                     CategoryProductAssociationRepository.InstantiateForDatabase(c),
                                     ProductRepository.InstantiateForDatabase(c),
                                     ProductRelationshipRepository.InstantiateForDatabase(c),
                                     ProductImageRepository.InstantiateForDatabase(c),
                                     ProductReviewRepository.InstantiateForDatabase(c),
                                     VariantRepository.InstantiateForDatabase(c),
                                     OptionRepository.InstantiateForDatabase(c),
                                     ProductOptionAssociationRepository.InstantiateForDatabase(c),
                                     ProductFileRepository.InstantiateForDatabase(c),
                                     ProductVolumeDiscountRepository.InstantiateForDatabase(c),
                                     ProductPropertyValueRepository.InstantiateForDatabase(c),
                                     ProductInventoryRepository.InstantiateForDatabase(c),
                                     ProductTypeRepository.InstantiateForDatabase(c),
                                     ProductTypePropertyAssociationRepository.InstantiateForDatabase(c),
                                     ProductPropertyRepository.InstantiateForDatabase(c),
                                     WishListItemRepository.InstantiateForDatabase(c));
        }
        public CatalogService(RequestContext c,
                              CategoryRepository categories,
                              CategoryProductAssociationRepository crosses,
                              ProductRepository products,
                              ProductRelationshipRepository relationships,
                              ProductImageRepository productImages,
                              ProductReviewRepository productReviews,
                              VariantRepository productVariants,
                              OptionRepository productOptions,
                              ProductOptionAssociationRepository productsXOptions,
                              ProductFileRepository productFiles,
                              ProductVolumeDiscountRepository volumeDiscounts,
                              ProductPropertyValueRepository propertyValues,
                              ProductInventoryRepository inventory,
                              ProductTypeRepository types,
                              ProductTypePropertyAssociationRepository typesXProperties,
                              ProductPropertyRepository properties,
                              WishListItemRepository wishItems)
        {
            context = c;
            Categories = categories;
            CategoriesXProducts = crosses;
            ProductRelationships = relationships;
            this.Products = products;
            this.ProductImages = productImages;
            this.ProductReviews = productReviews;
            this.ProductVariants = productVariants;
            this.ProductOptions = productOptions;
            this.ProductsXOptions = productsXOptions;
            this.ProductFiles = productFiles;
            this.VolumeDiscounts = volumeDiscounts;
            this.ProductPropertyValues = propertyValues;
            this.ProductInventories = inventory;
            this.ProductTypes = types;
            this.ProductTypesXProperties = typesXProperties;
            this.ProductProperties = properties;
            this.WishListItems = wishItems;
        }

        //public bool DoesCategoryNameExist(string categoryName, string parentId)
        //{
        //    bool result = false;

        //    string compareName = categoryName.Trim().ToLower();
            
        //    int total = 0;

        //    List<Category> dtPeers = Categories.FindChildren(parentId, 1, int.MaxValue, ref total);

        //    if (dtPeers != null)
        //    {
        //        for (int i = 0; i <= dtPeers.Count - 1; i++)
        //        {
        //            string compareName2 = dtPeers[i].Name;
        //            compareName2 = compareName2.Trim().ToLower();
        //            if (compareName == compareName2)
        //            {
        //                result = true;
        //                break;
        //            }
        //            compareName2 = null;
        //        }
        //    }

        //    return result;
        //}


        public string EditorRouteForCategory(CategorySourceType type, string bvin)
        {
            string prefix = "/bvadmin/catalog/";

            switch (type)
            {
                case CategorySourceType.DrillDown:
                    return prefix + "Categories_EditDrillDown.aspx?id=" + bvin;
                case CategorySourceType.CustomPage:
                    return prefix + "Categories/Custom/Edit/" + bvin;
                case CategorySourceType.FlexPage:
                    return prefix + "Categories_EditFlexPage.aspx?id=" + bvin;
                case CategorySourceType.CustomLink:
                    return prefix + "Categories_EditCustomLink.aspx?id=" + bvin;
                default:
                    return prefix + "Categories_Edit.aspx?id=" + bvin;
            }
        }

        public List<CategorySnapshot> FindCategoriesForProduct(string productBvin)
        {
            List<CategoryProductAssociation> crosses = CategoriesXProducts.FindForProduct(productBvin, 1, int.MaxValue);

            if (crosses == null) return new List<CategorySnapshot>();

            var bvins = (from x in crosses
                         select x.CategoryId).ToList();

            return Categories.FindManySnapshots(bvins);            
        }
      
        public List<Product> FindProductForCategoryWithSort(string categoryBvin, Catalog.CategorySortOrder sortOrder, bool showUnavailable)
        {
            int temp = -1;
            int sensibleLimit = 2000;
            return FindProductForCategoryWithSort(categoryBvin, sortOrder, showUnavailable, 1, sensibleLimit, ref temp);
        }

        public List<Product> FindProductForCategoryWithSort(string categoryBvin, Catalog.CategorySortOrder sortOrder, bool showUnavailable, 
                                                            int pageNumber, int pageSize, ref int rowCount)
        {
            List<Product> result = new List<Product>();            
            result = Products.FindByCategoryId(categoryBvin, sortOrder, showUnavailable, pageNumber, pageSize, ref rowCount);            
            return result;
        }

        // Products and Line Items
        public Orders.LineItem ConvertProductToLineItem(Orders.IPurchasable p, OptionSelectionList selections, int quantity, MerchantTribeApplication app)
        {
            Orders.LineItem li = new Orders.LineItem();
            
            if (p != null)
            {
                Orders.PurchasableSnapshot snapshot = p.AsPurchasable(selections, app, true);
                if (snapshot != null)
                {
                    li.BasePricePerItem = snapshot.BasePrice;
                    li.ProductId = snapshot.ProductId;
                    li.ProductName = snapshot.Name;
                    li.ProductShippingHeight = snapshot.ShippingDetails.Height;
                    li.ProductShippingLength = snapshot.ShippingDetails.Length;
                    li.ProductShippingWeight = snapshot.ShippingDetails.Weight;
                    li.ProductShippingWidth = snapshot.ShippingDetails.Width;
                    li.ProductShortDescription = snapshot.Description;
                    li.ProductSku = snapshot.Sku;
                    li.Quantity = quantity;
                    li.SelectionData = snapshot.SelectionData;
                    li.ShippingSchedule = snapshot.ShippingDetails.ShippingScheduleId;
                    li.VariantId = snapshot.VariantId;
                    li.TaxSchedule = snapshot.TaxScheduleId;
                    li.ShipFromAddress = snapshot.ShippingDetails.ShippingSourceAddress;
                    li.ShipFromMode = snapshot.ShippingDetails.ShippingSource;
                    li.ShipFromNotificationId = snapshot.ShippingDetails.ShippingSourceId;
                    li.ShipSeparately = snapshot.ShippingDetails.ShipSeparately;
                    li.ExtraShipCharge = snapshot.ShippingDetails.ExtraShipFee;
                }
            }

            return li;
        }
        
        public bool SaveProductToWishList(Orders.IPurchasable p, OptionSelectionList selections, int quantity, MerchantTribeApplication app)
        {
            WishListItem wi = new WishListItem();            
            if (p != null)
            {
                Orders.PurchasableSnapshot snapshot = p.AsPurchasable(selections, app, true);
                if (snapshot != null)
                {                    
                    wi.ProductId = snapshot.ProductId;
                    wi.Quantity = quantity;
                    wi.SelectionData = snapshot.SelectionData;
                    wi.CustomerId = app.CurrentCustomerId; 
                }
            }
            return WishListItems.Create(wi);
        }
        

        //Variants
        public void VariantsValidate(Product p)
        {
            // Check for Variants that contain non-associated options and delete
            this.VariantsRemoveInvalid(p);

            // Check for Variants that do not have all selections because a 
            // new variant option may have been added
            this.UpdateShortVariants(p);

            

            // Clear and Rebuild Inventory Objects            
            Product reloadedProduct = Products.Find(p.Bvin);
            InventoryGenerateForProduct(reloadedProduct);
            CleanUpInventory(reloadedProduct);

            p = Products.Find(p.Bvin);            
        }
        public void VariantsReloadForProduct(Product p)
        {
            p.Variants.Clear();
            p.Variants.AddRange(ProductVariants.FindByProductId(p.Bvin));
        }
        public void VariantsRemoveInvalid(Product p)
        {
            foreach (Variant v in p.Variants)
            {
                if (Catalog.OptionSelection.ContainsInvalidSelectionForOptions(p.Options, v.Selections))
                {
                    ProductVariants.Delete(v.Bvin);
                }
            }
            VariantsReloadForProduct(p);
        }
        private void UpdateShortVariants(Product p)
        {
            // Find out how many variants we have.
            List<Option> variantOptions = p.Options.VariantsOnly();
            if (variantOptions == null) return;
            int variantCount = variantOptions.Count();
            if (variantCount < 1) return;

            foreach (Variant v in p.Variants)
            {
                if (v.Selections.Count < variantCount)
                {
                    AddMissingOptions(v, variantOptions);
                }
            }
        }
        private void AddMissingOptions(Variant v, List<Option> options)
        {
            foreach (Option opt in options)
            {
                if (!v.Selections.ContainsSelectionForOption(opt.Bvin))
                {
                    if (opt.Items.Count > 0)
                    {
                        v.Selections.Add(new OptionSelection(opt.Bvin, opt.Items[0].Bvin));
                    }
                }
            }
            ProductVariants.Update(v);
        }
        public void VariantsGenerateAllPossible(Product p)
        {
            // Make sure we clear out anything not needed
            VariantsValidate(p);

            List<OptionSelectionList> possibleSelections = VariantsGenerateAllPossibleSelections(p.Options);

            foreach (OptionSelectionList selections in possibleSelections)
            {
                Variant v = new Variant();
                v.ProductId = p.Bvin;
                v.Selections.AddRange(selections);
                if (p.Variants.Count < WebAppSettings.MaxVariants)
                {
                    if (!p.Variants.ContainsKey(v.UniqueKey()))
                    {
                        ProductVariants.Create(v);
                    }
                }
            }
        }
        public List<OptionSelectionList> VariantsGenerateAllPossibleSelections(OptionList options)
        {
            List<OptionSelectionList> data = new List<OptionSelectionList>();

            List<Option> variantOptions = options.VariantsOnly();
            if (variantOptions == null) return data;
            if (variantOptions.Count < 1) return data;

            OptionSelectionList selections = new OptionSelectionList();
            GenerateVariantSelections(data, variantOptions, 0, selections);

            return data;
        }
        // Loop through all possible variants and generate selection data
        private void GenerateVariantSelections(List<OptionSelectionList> data,
                                                List<Option> options,
                                                int optionIndex,
                                                OptionSelectionList tempSelections)
        {
            if (optionIndex > (options.Count - 1))
            {
                // we've hit all options so add the selections to the data
                OptionSelectionList temp = new OptionSelectionList();
                temp.AddRange(tempSelections);
                //tempSelections.RemoveAt(tempSelections.Count() - 1);
                //tempSelections.Clear();
                data.Add(temp);

            }
            else
            {
                Option opt = options[optionIndex];
                foreach (OptionItem oi in opt.Items)
                {
                    if (oi.IsLabel == false)
                    {
                        OptionSelectionList localList = new OptionSelectionList();
                        localList.AddRange(tempSelections);
                        localList.Add(new OptionSelection(opt.Bvin, oi.Bvin));
                        GenerateVariantSelections(data, options, optionIndex + 1, localList);
                    }
                }

            }
        }

        public void ValidateVariantsForSharedOption(Option o)
        {
            if (o.IsShared)
            {
                List<Product> productsUsing = ProductsFindByOption(o.Bvin);

                foreach (Product p in productsUsing)
                {
                    ProductsReloadOptions(p);
                    VariantsReloadForProduct(p);
                    VariantsValidate(p);
                }
            }
        }
        public List<Product> ProductsFindByOption(string optionBvin)
        {
            List<ProductOptionAssociation> crosses = this.ProductsXOptions.FindForOption(optionBvin, 1, int.MaxValue);
            List<string> ids = new List<string>();
            foreach (ProductOptionAssociation x in crosses)
            {
                ids.Add(x.ProductBvin);
            }
            return Products.FindMany(ids);
        }

        //Product Options
        
        public bool ProductsAddOption(Product p, string optionBvin)
        {
            Option opt = ProductOptions.Find(optionBvin);
            if (opt == null) return false;

            bool result = ProductsXOptions.AddOptionToProduct(p.Bvin, optionBvin);
            ProductsReloadOptions(p);

            if (opt.IsVariant)
            {
                VariantsValidate(p);
            }

            return result;
        }
        public bool ProductsRemoveOption(Product p, string optionBvin)
        {
            Option opt = ProductOptions.Find(optionBvin);
            if (opt == null) return false;

            bool result = ProductsXOptions.RemoveOptionFromProduct(p.Bvin, optionBvin);

            ProductsReloadOptions(p);

            // delete an option if it's not shared
            if (result)
            {
                if (!opt.IsShared)
                {
                    result = ProductOptions.Delete(opt.Bvin);
                }
            }

            if (opt.IsVariant)
            {
                VariantsValidate(p);
            }

            return result;
        }        
        public void ProductsReloadOptions(Product p)
        {
            p.Options.Clear();
            p.Options.AddRange(ProductOptions.FindByProductId(p.Bvin));
        }

        //Products
        public bool ProductsCreateWithInventory(Product item, bool rebuildSearchIndex)
        {
            if (item == null) return false;
            if (item.UrlSlug.Trim().Length < 1)
            {
                item.UrlSlug = MerchantTribe.Web.Text.Slugify(item.ProductName, true, true);
            }

            bool result = this.Products.Create(item);
            if (rebuildSearchIndex)
            {
                SearchManager manager = new SearchManager();
                manager.IndexSingleProduct(item);
            }
            if (result)
            {
                InventoryGenerateForProduct(item);
                UpdateProductVisibleStatusAndSave(item);
            }
            return result;
        }
        public bool ProductsCreateWithInventory(Product item)
        {
            return ProductsCreateWithInventory(item, false);
        }       
        public bool ProductsUpdateWithSearchRebuild(Product item)
        {
            bool success = this.Products.Update(item);
            if (success)
            {
                SearchManager manager = new SearchManager();
                manager.IndexSingleProduct(item);
            }
            return success;
        }
        public List<Product> FindProductsForFile(string fileId)
        {
            List<Product> result = new List<Product>();
            List<string> bvins = ProductFiles.FindProductIdsForFile(fileId);
            result = Products.FindMany(bvins);
            return result;
        }
        public List<Product> FindProductsMatchingKey(string key, int pageNumber, int pageSize, ref int totalCount)
        {
            totalCount = this.ProductPropertyValues.FindCountProductIdsMatchingKey(key);            
            List<string> matches = this.ProductPropertyValues.FindProductIdsMatchingKey(key, pageNumber, pageSize);
            return Products.FindMany(matches);
        }                

        //Product Inventory
        public void UpdateProductVisibleStatusAndSave(string productBvin)
        {
            Product p = Products.Find(productBvin);
            UpdateProductVisibleStatusAndSave(p);
        }
        public void UpdateProductVisibleStatusAndSave(Catalog.Product p)
        {
            InventoryUpdateProductVisibleStatus(p);
            Products.Update(p);
        }
        public bool InventoryUpdateWithStatusSave(ProductInventory inv)
        {
            bool result = ProductInventories.Update(inv);
            if (result) UpdateProductVisibleStatusAndSave(inv.ProductBvin);
            return result;
        }
        public bool InventoryCreateWithStatusSave(ProductInventory inv)
        {
            bool result = ProductInventories.Create(inv);
            if (result) UpdateProductVisibleStatusAndSave(inv.ProductBvin);
            return result;
        }
        public bool InventoryAdjustAvailableQuantity(string productId, string variantId, int adjustment)
        {
            bool result = false;
            ProductInventory inv = ProductInventories.FindByProductIdAndVariantId(productId, variantId);
            if (inv != null)
            {
                inv.QuantityOnHand += adjustment;
                result = ProductInventories.Update(inv);
                UpdateProductVisibleStatusAndSave(inv.ProductBvin);
            }
            return result;
        }
        public bool InventorySetAvailableQuantity(string productId, string variantId, int quantity)
        {
            bool result = false;
            ProductInventory inv = ProductInventories.FindByProductIdAndVariantId(productId, variantId);
            if (inv != null)
            {
                inv.QuantityOnHand = quantity;
                result = ProductInventories.Update(inv);
                UpdateProductVisibleStatusAndSave(inv.ProductBvin);
            }
            return result;
        }
        public int InventoryReserveQuantity(string productBvin, string variantId, int quantity)
        {
            int result = InventoryReserveQuantity(productBvin, variantId, quantity, true);
            UpdateProductVisibleStatusAndSave(productBvin);
            return result;
        }
        public int InventoryReserveQuantity(string productBvin, string variantId, int quantity, bool ReserveZeroWhenQuantityTooLow)
        {
            Catalog.ProductInventory inv = ProductInventories.FindByProductIdAndVariantId(productBvin, variantId);

            // If no inventory, assume available
            if (inv == null) return quantity;

            Catalog.Product prod = Products.Find(productBvin);
            if (prod != null)
            {
                if (prod.InventoryMode == ProductInventoryMode.AlwayInStock) return quantity;

                if (inv != null && inv.Bvin != string.Empty)
                {
                    switch (prod.InventoryMode)
                    {
                        case ProductInventoryMode.AlwayInStock:
                            inv.QuantityReserved += quantity;
                            ProductInventories.Update(inv);
                            return quantity;
                        case ProductInventoryMode.WhenOutOfStockAllowBackorders:
                            inv.QuantityReserved += quantity;
                            ProductInventories.Update(inv);
                            return quantity;                            
                        case ProductInventoryMode.WhenOutOfStockShow:
                            if (inv.QuantityAvailableForSale < quantity)
                            {
                                if (ReserveZeroWhenQuantityTooLow)
                                {
                                    return 0;
                                }
                                else
                                {
                                    inv.QuantityReserved += inv.QuantityAvailableForSale;
                                    ProductInventories.Update(inv);
                                    return inv.QuantityAvailableForSale;
                                }
                            }
                            else
                            {
                                inv.QuantityReserved += quantity;
                                ProductInventories.Update(inv);
                                return quantity;
                            }                            
                        case ProductInventoryMode.WhenOutOfStockHide:
                            if (inv.QuantityAvailableForSale < quantity)
                            {
                                if (ReserveZeroWhenQuantityTooLow)
                                {
                                    return 0;
                                }
                                else
                                {
                                    inv.QuantityReserved += inv.QuantityAvailableForSale;
                                    ProductInventories.Update(inv);
                                    return inv.QuantityAvailableForSale;
                                }
                            }
                            else
                            {
                                inv.QuantityReserved += quantity;
                                ProductInventories.Update(inv);
                                return quantity;
                            }                            
                    }
                    return 0;
                }
                else
                {
                    if (prod != null)
                    {
                        if (prod.InventoryMode == ProductInventoryMode.AlwayInStock)
                        {
                            return 0;
                        }
                        else
                        {
                            return quantity;
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
            }

            return 0;
        }
        public bool InventoryUnreserveQuantity(string productBvin, string variantId, int quantity)
        {
            bool result = false;
            ProductInventory inv = ProductInventories.FindByProductIdAndVariantId(productBvin, variantId);
            if (inv != null)
            {
                inv.QuantityReserved -= quantity;
                ProductInventories.Update(inv);
                UpdateProductVisibleStatusAndSave(productBvin);
            }
            return result;
        }
        public bool InventoryShipQuantity(string productBvin, string variantId, int quantity)
        {
            bool result = false;
            ProductInventory inv = ProductInventories.FindByProductIdAndVariantId(productBvin, variantId);
            if (inv != null)
            {
                inv.QuantityReserved -= quantity;
                inv.QuantityOnHand -= quantity;
                ProductInventories.Update(inv);
                UpdateProductVisibleStatusAndSave(productBvin);
            }
            return result;
        }
        public bool InventoryUnshipQuantity(string productBvin, string variantId, int quantity)
        {
            bool result = false;
            ProductInventory inv = ProductInventories.FindByProductIdAndVariantId(productBvin, variantId);
            if (inv != null)
            {
                inv.QuantityReserved += quantity;
                inv.QuantityOnHand += quantity;
                ProductInventories.Update(inv);
                UpdateProductVisibleStatusAndSave(productBvin);
            }
            return result;
        }
        public void InventoryGenerateForProduct(Product localProduct)
        {
            if (localProduct == null) return;

            if (localProduct.HasVariants())
            {
                foreach (Variant v in localProduct.Variants)
                {
                    InventoryGenerateSingleInventory(localProduct.Bvin, v.Bvin, 0, 0);
                }                
            }
            else
            {
                InventoryGenerateSingleInventory(localProduct.Bvin, string.Empty, 0, 0);
            }            
        }
        public void CleanUpInventory(Product p)
        {
            if (p == null) return;
            List<ProductInventory> allInventory = ProductInventories.FindByProductId(p.Bvin);
            if (allInventory == null) return;
            if (allInventory.Count < 1) return;

            if (p.HasVariants())
            {                
                foreach (ProductInventory inv in allInventory)
                {
                    if (inv.VariantId.Trim() == string.Empty)
                    {
                        // Remove non-variant inventory levels
                        ProductInventories.Delete(inv.Bvin);
                    }

                    if (p.Variants.Where(y => y.Bvin == inv.VariantId).Count() <= 0)
                    {
                        // Remove variant inventory levels that don't apply anymore
                        ProductInventories.Delete(inv.Bvin);
                    }
                }
            }
            else
            {
                // Remove all variant inventory levels
                foreach (ProductInventory inv in allInventory)
                {
                    if (inv.VariantId.Trim() != string.Empty)
                    {
                        ProductInventories.Delete(inv.Bvin);
                    }
                }
            }
        }
        private void InventoryGenerateSingleInventory(string bvin, string variantId, int onHand, int lowStockPoint)
        {
            ProductInventory i = new ProductInventory();
            i.LowStockPoint = lowStockPoint;
            i.ProductBvin = bvin;
            i.QuantityOnHand = onHand;
            i.QuantityReserved = 0;
            i.VariantId = variantId;
            InventoryCreateWithStatusSave(i);            
        }        
        private void InventoryUpdateProductVisibleStatus(Catalog.Product product)
        {
            if (product == null) return;
            product.IsAvailableForSale = InventoryIsProductVisible(product);
        }
        public bool InventoryIsProductVisible(Catalog.Product product)
        {
            if (product.Status == ProductStatus.Disabled)
            {
                return false;
            }
            else
            {
                if (product.InventoryMode == ProductInventoryMode.AlwayInStock)
                {
                    return true;
                }
                else
                {
                    List<Catalog.ProductInventory> inv = ProductInventories.FindByProductId(product.Bvin);

                    // no inventory info so assume it's available
                    if (inv == null) return true;
                    if (inv.Count < 1) return true;

                    bool foundStock = false;
                    foreach (ProductInventory piv in inv)
                    {
                        if (piv.InventoryEvaluateStatus(product) == ProductInventoryStatus.Available)
                        {
                            return true;
                        }
                        else
                        {
                            if (product.InventoryMode != ProductInventoryMode.WhenOutOfStockHide)
                            {
                                return true;
                            }
                        }
                    }

                    if (!foundStock) return false;
                }
            }
            return true;
        }
        public InventoryCheckData InventoryCheck(Product p, string variantId)
        {
            InventoryCheckData result = new InventoryCheckData();
            result.Qty = InventoryQuantityAvailableForPurchase(p, variantId);

            switch (p.InventoryMode)
            {
                case ProductInventoryMode.AlwayInStock:
                    result.IsInStock = true;
                    result.InventoryMessage = "In Stock";
                    result.IsAvailableForSale = true;
                    break;
                case ProductInventoryMode.WhenOutOfStockAllowBackorders:
                    if (result.Qty < 1)
                    {
                        result.IsInStock = false;
                        result.InventoryMessage = "<span class=\"inventorybackordered\">Backordered</span>";
                        result.IsAvailableForSale = true;
                    }
                    else
                    {
                        result.IsInStock = true;
                        result.InventoryMessage = "In Stock (" + result.Qty.ToString() + ")";
                        result.IsAvailableForSale = true;
                    }
                    break;
                case ProductInventoryMode.WhenOutOfStockHide:
                    if (result.Qty < 1)
                    {
                        result.IsInStock = false;
                        result.InventoryMessage = "<span class=\"inventoryoutofstock\">Out of Stock</span>";
                        result.IsAvailableForSale = false;
                    }
                    else
                    {
                        result.IsInStock = true;
                        result.InventoryMessage = "In Stock (" + result.Qty.ToString() + ")";
                        result.IsAvailableForSale = true;
                    }
                    break;
                case ProductInventoryMode.WhenOutOfStockShow:
                    if (result.Qty < 1)
                    {
                        result.IsInStock = false;
                        result.InventoryMessage = "<span class=\"inventoryoutofstock\">Out of Stock</span>";
                        result.IsAvailableForSale = false;
                    }
                    else
                    {
                        result.IsInStock = true;
                        result.InventoryMessage = "In Stock (" + result.Qty.ToString() + ")";
                        result.IsAvailableForSale = true;
                    }
                    break;
            }

            return result;
        }
        public int InventoryQuantityAvailableForPurchase(Product p, string variantId)
        {
            int result = 0;

            if (p.InventoryMode == ProductInventoryMode.AlwayInStock) return 9999;

            List<ProductInventory> inv = ProductInventories.FindByProductId(p.Bvin);
            if (variantId != string.Empty)
            {
                var vi = (from pi in inv
                          where pi.VariantId == variantId
                          select pi).SingleOrDefault();
                if (vi != null)
                {
                    result = vi.QuantityAvailableForSale;
                }
            }
            else
            {
                // no variants
                result = (from pi in inv
                          select pi).Sum(y => y.QuantityAvailableForSale);
            }

            return result;
        }

        // Line Item Inventory
        public bool InventoryLineItemShipQuantity(Orders.LineItem li, int quantity)
        {
            li.QuantityShipped += quantity;
            InventoryShipQuantity(li.ProductId, li.VariantId, quantity);
            return true;
        }
        public bool InventoryLineItemUnShipQuantity(Orders.LineItem li, int quantity)
        {
            li.QuantityShipped -= quantity;
            InventoryUnshipQuantity(li.ProductId, li.VariantId, quantity);
            return true;
        }
        public bool InventoryLineItemReturnQuantity(Orders.LineItem li, int receivedQuantity, bool returnToInventory)
        {
            li.QuantityReturned += receivedQuantity;

            if (returnToInventory)
            {
                return InventoryAdjustAvailableQuantity(li.ProductId, li.VariantId, receivedQuantity);
            }
            else
            {
                return true;
            }
        }
        public int InventoryLineItemReserveInventory(Orders.LineItem li)
        {
            return InventoryLineItemReserveInventory(li, li.Quantity);
        }
        public bool InventoryLineItemUnreserveInventory(Orders.LineItem li)
        {
            return InventoryLineItemUnreserveInventory(li, li.Quantity);
        }
        public int InventoryLineItemReserveInventory(Orders.LineItem li, int qty)
        {
            if (qty < 0)
            {
                throw new ArgumentException("Cannot be negative", "qty");
            }

            return InventoryLineItemReserveSingleItemInventory(li.ProductId, li.VariantId, qty);
        }
        public bool InventoryLineItemUnreserveInventory(Orders.LineItem li, int qty)
        {
            if (qty < 0)
            {
                throw new ArgumentException("Cannot be negative", "qty");
            }

            return InventoryLineItemUnreserveSingleItemInventory(li.ProductId, li.VariantId, qty);
        }
        private int InventoryLineItemReserveSingleItemInventory(string productBvin, string variantId, int qty)
        {
            return InventoryReserveQuantity(productBvin, variantId, qty);
        }
        private bool InventoryLineItemUnreserveSingleItemInventory(string productBvin, string variantId, int qty)
        {
            return InventoryUnreserveQuantity(productBvin, variantId, qty);
        }

        // ProductTypes
        public bool ProductTypeDestroy(string bvin)
        {
            // Remove Associated Properties First
            this.ProductTypesXProperties.DeleteForProductType(bvin);

            // Then remove type
            return this.ProductTypes.Delete(bvin);
        }
        public bool ProductTypeAddProperty(string typeBvin, long propertyId)
        {
            ProductTypePropertyAssociation item = this.ProductTypesXProperties.FindForTypeAndProperty(typeBvin, propertyId);
            if (item == null)
            {
                item = new ProductTypePropertyAssociation();
                item.ProductTypeBvin = typeBvin;
                item.PropertyId = propertyId;
                return this.ProductTypesXProperties.Create(item);
            }
            return true;
        }
        public bool ProductTypeRemoveProperty(string typeBvin, long propertyId)
        {
            return this.ProductTypesXProperties.DeleteForTypeAndProperty(typeBvin, propertyId);
        }
        public bool ProductTypeMovePropertyUp(string productTypeBvin, long propertyId)
        {
            bool ret = false;

            List<ProductTypePropertyAssociation> peers = new List<ProductTypePropertyAssociation>();
            peers = this.ProductTypesXProperties.FindByProductType(productTypeBvin);

            if (peers != null)
            {
                int currentSort = 0;
                int targetSort = 0;
                long targetId = -1;
                bool foundTarget = false;

                for (int i = 0; i <= peers.Count - 1; i++)
                {

                    if (peers[i].Id == propertyId)
                    {
                        currentSort = peers[i].SortOrder;
                        // process last request
                        foundTarget = true;
                    }
                    else
                    {

                        if (foundTarget == false)
                        {
                            targetSort = peers[i].SortOrder;
                            targetId = peers[i].Id;
                        }
                    }

                }

                if (foundTarget == true & currentSort >= 1)
                {
                    if (peers.Count > 1)
                    {
                        this.ProductTypesXProperties.UpdateSortOrder(productTypeBvin, targetId, currentSort);
                        this.ProductTypesXProperties.UpdateSortOrder(productTypeBvin, propertyId, targetSort);
                        ret = true;
                    }
                }

            }

            peers = null;

            return ret;
        }
        public bool ProductTypeMovePropertyDown(string productTypeBvin, long propertyId)
        {
            bool ret = false;

            List<ProductTypePropertyAssociation> peers = new List<ProductTypePropertyAssociation>();
            peers = this.ProductTypesXProperties.FindByProductType(productTypeBvin);

            if (peers != null)
            {
                int currentSort = 0;
                int targetSort = 0;
                long targetId = -1;
                bool foundCurrent = false;
                bool foundTarget = false;

                for (int i = 0; i <= peers.Count - 1; i++)
                {
                    if (foundCurrent)
                    {
                        targetId = peers[i].Id;
                        targetSort = peers[i].SortOrder;
                        foundCurrent = false;
                        foundTarget = true;
                    }
                    if (peers[i].Id == propertyId)
                    {
                        currentSort = peers[i].SortOrder;
                        foundCurrent = true;
                    }

                }

                if (foundTarget == true)
                {
                    if (peers.Count > 1)
                    {
                        this.ProductTypesXProperties.UpdateSortOrder(productTypeBvin, propertyId, targetSort);
                        this.ProductTypesXProperties.UpdateSortOrder(productTypeBvin, targetId, currentSort);
                        ret = true;
                    }
                }

            }

            peers = null;

            return ret;
        }

        // ProductProperties
        public bool ProductPropertiesDestroy(long id)
        {
            this.ProductTypesXProperties.DeleteForProperty(id);
            return this.ProductProperties.Delete(id);
        }
        public List<ProductProperty> ProductPropertiesFindForType(string productTypeId)
        {
            List<ProductTypePropertyAssociation> crosses = this.ProductTypesXProperties.FindByProductType(productTypeId);
            List<long> ids = new List<long>();
            foreach (ProductTypePropertyAssociation x in crosses)
            {
                ids.Add(x.PropertyId);
            }

            // FindMany sorts by PropertyName so we
            // need to resort based on option order
            // in ProductXOption table
            List<ProductProperty> unsorted = this.ProductProperties.FindMany(ids);
            List<ProductProperty> result = new List<ProductProperty>();
            foreach (ProductTypePropertyAssociation cross in crosses)
            {
                ProductProperty found = unsorted.Where(y => y.Id == cross.PropertyId).FirstOrDefault();
                if (found != null) result.Add(found);
            }

            return result;
        }
        public List<ProductProperty> ProductPropertiesFindNotAssignedToType(string productTypeId)
        {            
            List<ProductProperty> assigned = ProductPropertiesFindForType(productTypeId);
            List<ProductProperty> all = this.ProductProperties.FindAll();
            return all.Where(y => assigned.Contains(y) == false).ToList();            
        }
        public string FormatProductPropertyChoiceValue(ProductProperty prop, string value)
        {
            if (prop == null) return value;

            switch (prop.TypeCode)
            {
                case Catalog.ProductPropertyType.MultipleChoiceField:
                    foreach (Catalog.ProductPropertyChoice choice in prop.Choices)
                    {
                        long temp = -1;
                        long.TryParse(value, out temp);
                        if (choice.Id == temp)
                        {
                            return choice.ChoiceName;
                        }
                    }
                    return "";
                case Catalog.ProductPropertyType.CurrencyField:
                    System.Globalization.CultureInfo info = new System.Globalization.CultureInfo(prop.CultureCode);
                    decimal temp2 = -1;
                    decimal.TryParse(value, out temp2);
                    return string.Format(info.NumberFormat, "{0:c}", temp2);
                default:
                    return value;
            }
        }
                
    }
}
