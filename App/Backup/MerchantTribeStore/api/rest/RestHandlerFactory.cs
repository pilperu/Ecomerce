using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.api.rest
{
    public class RestHandlerFactory
    {
        public static IRestHandler Instantiate(string version, string modelname, MerchantTribe.Commerce.MerchantTribeApplication app)
        {
            switch (modelname.Trim().ToLowerInvariant())
            {
                case "categories":
                    return new CategoriesHandler(app);
                case "pricegroups":
                    return new PriceGroupsHandler(app);
                case "customeraccounts":
                    return new CustomerAccountHandler(app);
                case "affiliates":
                    return new AffiliatesHandler(app);
                case "taxschedules":
                    return new TaxSchedulesHandler(app);
                case "taxes":
                    return new TaxesHandler(app);
                case "vendors":
                    return new VendorsHandler(app);
                case "manufacturers":
                    return new ManufacturersHandler(app);
                case "producttypes":
                    return new ProductTypesHandler(app);
                case "productproperties":
                    return new ProductPropertiesHandler(app);
                case "productoptions":
                    return new ProductOptionsHandler(app);
                case "products":
                    return new ProductsHandler(app);
                case "productmainimage":
                    return new ProductsMainImageHandler(app);
                case "categoriesimagesicon":
                    return new CategoriesImagesIconHandler(app);
                case "categoriesimagesbanner":
                    return new CategoriesImagesBannerHandler(app);
                case "productfiles":
                    return new ProductFilesHandler(app);
                case "productfilesdata":
                    return new ProductFilesDataHandler(app);
                case "productfilesxproducts":
                    return new ProductFilesXProductsHandler(app);
                case "productrelationships":
                    return new ProductRelationshipsHandler(app);
                case "productinventory":
                    return new ProductInventoryHandler(app);
                case "productimages":
                    return new ProductImagesHandler(app);
                case "productimagesupload":
                    return new ProductImagesUploadHandler(app);
                case "productvolumediscounts":
                    return new ProductVolumeDiscountsHandler(app);
                case "productreviews":
                    return new ProductReviewsHandler(app);
                case "categoryproductassociations":
                    return new CategoryProductAssociationsHandler(app);
                case "searchmanager":
                    return new SearchManagerHandler(app);
                case "orders":
                    return new OrdersHandler(app);
                case "ordertransactions":
                    return new OrderTransactionsHandler(app);
                case "utilities":
                    return new UtilitiesHandler(app);
                case "wishlistitems":
                    return new WishListItemsHandler(app);
            }

            return null;
        }
    }
}