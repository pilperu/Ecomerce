using System;
using System.Data;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using MerchantTribe.Shipping;
using System.IO;

namespace MerchantTribe.Commerce.Shipping
{

	public class ShippingMethod
	{
        // Variables    
		private decimal _Adjustment = 0m;
		private ShippingMethodAdjustmentType _AdjustmentType = ShippingMethodAdjustmentType.None;
		private string _Name = "";
		private string _ShippingProviderId = string.Empty;
        private long _ZoneId = 0;
        private ServiceSettings _Settings = new ServiceSettings();

        private string _bvin = string.Empty;
        private System.DateTime _LastUpdated = System.DateTime.MinValue;

		// Properties   
        public virtual string Bvin
        {
            get { return _bvin; }
            set { _bvin = value; }
        }
        //protected XmlWriterSettings BVXmlWriterSettings
        //{
        //    get { return _BVXmlWriterSettings; }
        //}
        //protected XmlReaderSettings BVXmlReaderSettings
        //{
        //    get { return _BVXmlReaderSettings; }
        //}
        public virtual System.DateTime LastUpdated
        {
            get { return _LastUpdated; }
            set { _LastUpdated = value; }
        }
		public decimal Adjustment {
			get { return _Adjustment; }
			set { _Adjustment = value; }
		}
		public ShippingMethodAdjustmentType AdjustmentType {
			get { return _AdjustmentType; }
			set { _AdjustmentType = value; }
		}
		public string Name {
			get { return _Name; }
			set { _Name = value; }
		}
		public string ShippingProviderId {
			get { return _ShippingProviderId; }
			set { _ShippingProviderId = value; }
		}
        public long ZoneId
        {
            get { return _ZoneId; }
            set { _ZoneId = value; }
        }
        public ServiceSettings Settings
        {
            get { return _Settings; }
            set { _Settings = value; }
        }
        public long StoreId { get; set; }

		// Methods
		public ShippingMethod()
		{
            StoreId = 0;
		}

		public Collection<ShippingRateDisplay> GetRates(Orders.Order o, Accounts.Store currentStore)
		{
            Collection<ShippingRateDisplay> result = new Collection<ShippingRateDisplay>();
            List<ShippingGroup> groups = o.GetShippingGroups();
            return GetRates(groups, currentStore);        
		}

        private MerchantTribe.Shipping.IShipment ConvertGroupsToShipments(List<ShippingGroup> groups)
        {
            MerchantTribe.Shipping.IShipment result = new MerchantTribe.Shipping.Shipment();            
            foreach (ShippingGroup g in groups)
            {                
                result.Items.Add(g.AsIShippable());
                result.DestinationAddress = g.DestinationAddress;
                result.SourceAddress = g.SourceAddress;
            }
            return result;
        }

		public Collection<ShippingRateDisplay> GetRates(List<ShippingGroup> groups, Accounts.Store currentStore)
		{            
            Collection<ShippingRateDisplay> result = new Collection<ShippingRateDisplay>();

            MerchantTribe.Shipping.IShippingService p = Shipping.AvailableServices.FindById(this.ShippingProviderId, currentStore);
			if (p != null) {
                p.BaseSettings.Clear();
                p.BaseSettings.Merge(this.Settings);

				List<IShippingRate> tempRates = p.RateShipment(ConvertGroupsToShipments(groups));
				if (tempRates != null) {
					for (int i = 0; i <= tempRates.Count - 1; i++) {
						ShippingRateDisplay r= new ShippingRateDisplay(tempRates[i]);
						r.ShippingMethodId = this.Bvin;
						if (r.DisplayName == string.Empty) {
							r.DisplayName = this.Name;
						}
						AdjustRate(r);
						result.Add(r);
					}
				}
			}

			return result;
		}

		private void AdjustRate(ShippingRateDisplay r)
		{
			switch (this.AdjustmentType) {
				case ShippingMethodAdjustmentType.Amount:
                    r.Rate = r.Rate + this.Adjustment;
                    break;
				case ShippingMethodAdjustmentType.None:
				// Do Nothing
                    break;
				case ShippingMethodAdjustmentType.Percentage:
                    r.Rate = r.Rate + (r.Rate * (this.Adjustment / 100m));
                    break;
			}
		}
	        				
	}

}
