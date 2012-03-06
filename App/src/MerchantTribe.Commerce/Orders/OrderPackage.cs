using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using MerchantTribe.CommerceDTO.v1.Orders;
using MerchantTribe.CommerceDTO.v1.Shipping;
using MerchantTribe.CommerceDTO.v1;

namespace MerchantTribe.Commerce.Orders
{
    public class OrderPackage: Content.IReplaceable
    {                        
        public long Id { get; set; }
        public long StoreId { get; set; }
        public System.DateTime LastUpdatedUtc {get;set;}
        public List<OrderPackageItem> Items {get;set;}
        public string Description {get;set;}
        public string OrderId {get;set;}
        public decimal Width {get;set;}
        public decimal Height {get;set;}
        public decimal Length {get;set;}
        public MerchantTribe.Shipping.LengthType SizeUnits { get; set; }
        public decimal Weight {get;set;}
        public MerchantTribe.Shipping.WeightType WeightUnits { get; set; }
        public string ShippingProviderId {get;set;}
        public string ShippingProviderServiceCode {get;set;}
        public string TrackingNumber {get;set;}
        public bool HasShipped {get;set;}
        public DateTime ShipDateUtc {get;set;}
        public decimal EstimatedShippingCost {get;set;}
        public string ShippingMethodId {get;set;}
        public CustomPropertyCollection CustomProperties { get; set; }

        public string ThirdPartyId
        {
            get
            {
                return this.CustomProperties.GetProperty("MerchantTribe", "3rdPartyId");
            }
            set
            {
                this.CustomProperties.SetProperty("MerchantTribe", "3rdPartyId", value);
            }
        }

        public OrderPackage()
        {
            this.Id = 0;
            this.StoreId = 0;
            this.LastUpdatedUtc = DateTime.UtcNow;
            this.Items = new List<OrderPackageItem>();
            this.Description = string.Empty;
            this.OrderId = string.Empty;
            this.Width = 0m;
            this.Height = 0m;
            this.Length = 0m;
            this.SizeUnits = MerchantTribe.Shipping.LengthType.Inches;
            this.Weight = 0m;
            this.WeightUnits = MerchantTribe.Shipping.WeightType.Pounds;
            this.ShippingProviderId = string.Empty;
            this.ShippingProviderServiceCode = string.Empty;
            this.ShippingMethodId = string.Empty;
            this.TrackingNumber = string.Empty;
            this.HasShipped = false;
            this.ShipDateUtc = DateTime.MinValue;
            this.EstimatedShippingCost = 0m;
            this.CustomProperties = new CustomPropertyCollection();
        }

        public OrderPackage Clone()
        {
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(memoryStream, this);
            memoryStream.Position = 0;
            OrderPackage newPackage = (OrderPackage)formatter.Deserialize(memoryStream);
            return newPackage;
        }


        public List<Content.HtmlTemplateTag> GetReplaceableTags(MerchantTribeApplication app)
        {
            List<Content.HtmlTemplateTag> result = new List<Content.HtmlTemplateTag>();
            result.Add(new Content.HtmlTemplateTag("[[Package.ShipDate]]", this.ShipDateUtc.ToString("d")));

            if (this.TrackingNumber == string.Empty)
            {
                result.Add(new Content.HtmlTemplateTag("[[Package.TrackingNumber]]", "None Available"));
            }
            else
            {
                result.Add(new Content.HtmlTemplateTag("[[Package.TrackingNumber]]", this.TrackingNumber));
            }

            bool tagsEntered = false;
            MerchantTribe.Commerce.Accounts.Store currentStore = app.CurrentRequestContext.CurrentStore;
            foreach (MerchantTribe.Shipping.IShippingService item in Shipping.AvailableServices.FindAll(currentStore))
            {
                if (item.Id.ToString() == this.ShippingProviderId)
                {
                    tagsEntered = true;
                    //Orders.Order order = Orders.Order.FindByBvin(this.OrderId);
                    //if (order != null && order.Bvin != string.Empty)
                    //{
                    //    //if (item.SupportsTracking()) {
                    //    //    if (this.TrackingNumber.Trim() != string.Empty) {
                    //    //        result.Add(new Content.EmailTemplateTag("[[Package.TrackingNumberLink]]", item.GetTrackingUrl(this.TrackingNumber)));
                    //    //    }
                    //    //    else {
                    //    //result.Add(new Content.EmailTemplateTag("[[Package.TrackingNumberLink]]", ""));
                    //    //    }
                    //    //}
                    //    //else {
                    //    result.Add(new Content.EmailTemplateTag("[[Package.TrackingNumberLink]]", ""));
                    //    //}
                    //}

                    result.Add(new Content.HtmlTemplateTag("[[Package.ShipperName]]", item.Name));
                    List<MerchantTribe.Shipping.IServiceCode> serviceCodes = item.ListAllServiceCodes();
                    bool shipperServiceFound = false;
                    foreach (MerchantTribe.Shipping.IServiceCode serviceCode in serviceCodes)
                    {
                        if (string.Compare(this.ShippingProviderServiceCode, serviceCode.Code, true) == 0)
                        {
                            shipperServiceFound = true;
                            result.Add(new Content.HtmlTemplateTag("[[Package.ShipperService]]", serviceCode.DisplayName));
                            break;
                        }
                    }

                    if (!shipperServiceFound)
                    {
                        result.Add(new Content.HtmlTemplateTag("[[Package.ShipperService]]", ""));
                    }
                }
            }

            if ((this.Items != null) && (this.Items.Count > 0))
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<table class=\"packageitems\">");
                sb.Append("<tr>");
                sb.Append("<td class=\"itemnamehead\">Name</td>");
                sb.Append("<td class=\"itemquantityhead\">Quantity</td>");
                sb.Append("</tr>");
                //sb.Append("<ul>")
                int count = 0;
                foreach (OrderPackageItem item in this.Items)
                {
                    if (item.Quantity > 0)
                    {
                        if (count % 2 == 0)
                        {
                            sb.Append("<tr>");
                        }
                        else
                        {
                            sb.Append("<tr class=\"alt\">");
                        }

                        //sb.Append("<li>")
                        Catalog.Product prod = app.CatalogServices.Products.Find(item.ProductBvin);
                        if (prod != null)
                        {
                            sb.Append("<td class=\"itemname\">");
                            sb.Append(prod.ProductName);
                            sb.Append("</td>");
                            sb.Append("<td class=\"itemquantity\">");
                            sb.Append(item.Quantity.ToString());
                            sb.Append("</td>");
                        }
                        //sb.Append("</li>")                    
                        sb.Append("</tr>");
                        count += 1;
                    }
                }
                sb.Append("</table>");

                //sb.Append("</ul>")
                result.Add(new Content.HtmlTemplateTag("[[Package.Items]]", sb.ToString()));
            }
            else
            {
                result.Add(new Content.HtmlTemplateTag("[[Package.Items]]", ""));
            }

            //these are only here so that they get added to the list of available tags
            if (!tagsEntered)
            {
                result.Add(new Content.HtmlTemplateTag("[[Package.TrackingNumberLink]]", ""));
                result.Add(new Content.HtmlTemplateTag("[[Package.ShipperName]]", ""));
                result.Add(new Content.HtmlTemplateTag("[[Package.ShipperService]]", ""));
            }

            return result;
        }
        public string ItemsToXml()
        {
            string result = string.Empty;

            try
            {
                StringWriter sw = new StringWriter();
                XmlSerializer xs = new XmlSerializer(Items.GetType());
                xs.Serialize(sw, Items);
                result = sw.ToString();
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
                result = string.Empty;
            }

            return result;
        }
        public bool ItemsFromXml(string data)
        {
            bool result = false;

            try
            {
                StringReader tr = new StringReader(data);
                XmlSerializer xs = new XmlSerializer(Items.GetType());
                Items = (List<OrderPackageItem>)xs.Deserialize(tr);
                if (Items != null)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
                Items = new List<OrderPackageItem>();
                result = false;
            }

            return result;
        }

        // DTO
        public OrderPackageDTO ToDto()
        {
            OrderPackageDTO dto = new OrderPackageDTO();

            dto.CustomProperties = new List<CommerceDTO.v1.CustomPropertyDTO>();
            foreach (CustomProperty prop in this.CustomProperties)
            {
                dto.CustomProperties.Add(prop.ToDto());
            }
            dto.Description = this.Description ?? string.Empty;
            dto.EstimatedShippingCost = this.EstimatedShippingCost;
            dto.HasShipped = this.HasShipped;
            dto.Height = this.Height;
            dto.Id = this.Id;
            dto.Items = new List<OrderPackageItemDTO>();
            {
                foreach (OrderPackageItem item in this.Items)
                {
                    dto.Items.Add(item.ToDto());
                }
            }
            dto.LastUpdatedUtc = this.LastUpdatedUtc;
            dto.Length = this.Length;
            dto.OrderId = this.OrderId ?? string.Empty;
            dto.ShipDateUtc = this.ShipDateUtc;
            dto.ShippingMethodId = this.ShippingMethodId ?? string.Empty;
            dto.ShippingProviderId = this.ShippingProviderId ?? string.Empty;
            dto.ShippingProviderServiceCode = this.ShippingProviderServiceCode ?? string.Empty;
            dto.SizeUnits = (LengthTypeDTO)((int)this.SizeUnits);
            dto.StoreId = this.StoreId;
            dto.TrackingNumber = this.TrackingNumber ?? string.Empty;
            dto.Weight = this.Weight;
            dto.WeightUnits = (WeightTypeDTO)((int)this.WeightUnits);
            dto.Width = this.Width;

            return dto;
        }
        public void FromDto(OrderPackageDTO dto)
        {
            if (dto == null) return;

            if (dto.CustomProperties != null)
            {
                this.CustomProperties.Clear();
                foreach (CustomPropertyDTO prop in dto.CustomProperties)
                {
                    CustomProperty p = new CustomProperty();
                    p.FromDto(prop);
                    this.CustomProperties.Add(p);
                }
            }
            this.Description = dto.Description ?? string.Empty;
            this.EstimatedShippingCost = dto.EstimatedShippingCost;
            this.HasShipped = dto.HasShipped;
            this.Height = dto.Height;
            this.Id = dto.Id;
            if (dto.Items != null)
            {
                this.Items.Clear();
                foreach (OrderPackageItemDTO item in dto.Items)
                {
                    OrderPackageItem pak = new OrderPackageItem();
                    pak.FromDto(item);
                    this.Items.Add(pak);
                }
            }
            this.LastUpdatedUtc = dto.LastUpdatedUtc;
            this.Length = dto.Length;
            this.OrderId = dto.OrderId ?? string.Empty;
            this.ShipDateUtc = dto.ShipDateUtc;
            this.ShippingMethodId = dto.ShippingMethodId ?? string.Empty;
            this.ShippingProviderId = dto.ShippingProviderId ?? string.Empty;
            this.ShippingProviderServiceCode = dto.ShippingProviderServiceCode ?? string.Empty;
            this.SizeUnits = (MerchantTribe.Shipping.LengthType)((int)dto.SizeUnits);
            this.StoreId = dto.StoreId;
            this.TrackingNumber = dto.TrackingNumber ?? string.Empty;
            this.Weight = dto.Weight;
            this.WeightUnits = (MerchantTribe.Shipping.WeightType)((int)dto.WeightUnits);
            this.Width = dto.Width;

        }



    }
}
