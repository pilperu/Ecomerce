using System;
using System.Collections.ObjectModel;
using System.Data;

namespace MerchantTribe.Commerce.Shipping
{

	public class ShippingGroup
	{
		private Collection<Orders.LineItem> _items = new Collection<Orders.LineItem>();
		private Contacts.Address _sourceAddress = new Contacts.Address();
		private Contacts.Address _destinationAddress = new Contacts.Address();
		private ShippingMode _ShippingMode = ShippingMode.None;
		private string _shipId = string.Empty;
		private decimal _length = 0;
		private decimal _width = 0;
		private decimal _height = 0;
		private decimal _weight = 0;
		private bool _shipSeparately = false;
		private bool _oversize = false;

		public Collection<Orders.LineItem> Items {
			get { return _items; }
		}
		public Contacts.Address SourceAddress {
			get { return _sourceAddress; }
			set { _sourceAddress = value; }
		}
		public Contacts.Address DestinationAddress {
			get { return _destinationAddress; }
			set { _destinationAddress = value; }
		}
		public ShippingMode ShippingMode {
			get { return _ShippingMode; }
			set { _ShippingMode = value; }
		}
		public string ShipId {
			get { return _shipId; }
			set { _shipId = value; }
		}

		public decimal Length {
			get { return _length; }
			set { _length = value; }
		}

		public decimal Width {
			get { return _width; }
			set { _width = value; }
		}
		public decimal Height {
			get { return _height; }
			set { _height = value; }
		}
		public decimal Weight {
			get { return _weight; }
			set { _weight = value; }
		}
		public bool ShipSeperately {
			get { return _shipSeparately; }
			set { _shipSeparately = value; }
		}
		public bool Oversize {
			get { return _oversize; }
			set { _oversize = value; }
		}
		public static Shipping.DimensionCalculator DimensionCalculator {
            get { return new MerchantTribe.Commerce.Shipping.DefaultDimensionCalculator(); }
		}

		public virtual void GenerateDimensions()
		{
			if (ShippingGroup.DimensionCalculator != null) {
				ShippingGroup.DimensionCalculator.GenerateDimensions(this);
			}
		}

		public ShippingGroup Clone(bool CopyBvins)
		{
			ShippingGroup result = new ShippingGroup();

			foreach (Orders.LineItem li in _items) {
				result.Items.Add(li.Clone(CopyBvins));
			}
			_sourceAddress.CopyTo(result.SourceAddress);
			_destinationAddress.CopyTo(result.DestinationAddress);
			result.ShippingMode = _ShippingMode;
			result.ShipId = string.Empty;
			result.Length = _length;
			result.Width = _width;
			result.Height = _height;
			result.Weight = _weight;
			result.ShipSeperately = _shipSeparately;
			result.GenerateDimensions();

			return result;
		}

        public MerchantTribe.Shipping.IShippable AsIShippable()
        {
            this.GenerateDimensions();
            MerchantTribe.Shipping.Shippable result = new MerchantTribe.Shipping.Shippable();

            result.BoxHeight = this.Height;
            result.BoxLength = this.Length;            
            result.BoxWeight = this.Weight;
            result.BoxWidth = this.Width;

            foreach (Orders.LineItem li in this.Items)
            {
                result.QuantityOfItemsInBox += (int)li.Quantity;
                result.BoxValue += li.LineTotal;
            }

            return result;
        }
	}
}
