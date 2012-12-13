using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerchantTribe.CommerceDTO.v1.Catalog
{
    public class VariantOptionDataDTO
    {
        public string ChoiceId { get; set; }
        public string ChoiceItemId { get; set; }

        public VariantOptionDataDTO()
        {
            this.ChoiceId = string.Empty;
            this.ChoiceItemId = string.Empty;
        }
    }
    public class ProductVariantSkuUpdateDTO
    {
        public string ProductBvin { get; set; }
        public string Sku { get; set; }
        public List<VariantOptionDataDTO> MatchingOptions { get; set; }

        public ProductVariantSkuUpdateDTO()
        {
            this.ProductBvin = string.Empty;
            this.Sku = string.Empty;
            this.MatchingOptions = new List<VariantOptionDataDTO>();
        }
    }
}
