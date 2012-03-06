using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.Models
{
    public class StoneEdgeTrackingData
    {
        public string TrackingNumber { get; set; }
        public string Carrier { get; set; }
        public DateTime? PickUpDate { get; set; }

        public StoneEdgeTrackingData()
        {
            TrackingNumber = string.Empty;
            Carrier = string.Empty;
            PickUpDate = null;
        }
    }

    public enum StoneEdgeErrorType
    {
        General = 0,
        Orders = 1,
        Products = 2,
        Customers = 3
    }
    public enum StoneEdgeFunction
    {
        SendSampleError = -1,   // General
        Unknown = 0,
        SendVersion = 1,        

        OrderCount = 100,        // Orders
        DownloadOrders = 101,

        GetProductsCount = 200,  // Products
        DownloadProds = 201,

        GetCustomersCount = 300,  // Customers
        DownloadCustomers = 301,

        DownloadQoh = 400, // Quantity on Hand
        QohReplace = 401, 
        InvUpdate = 402,

        UpdateStatus = 500 // Shipment Status
    }

    public class StoneEdgeError
    {
        public StoneEdgeErrorType ErrorType { get; set; }
        public string Message { get; set; }
        
        public StoneEdgeError()
        {
            ErrorType = StoneEdgeErrorType.General;
            Message = string.Empty;
        }
    }

    public class StoneEdgeIntegrationModel
    {
        private decimal _SetiVersion = 5.9m;
        public string SetiVersion { get { return _SetiVersion.ToString("0.000"); } }        

        // Properties
        public StoneEdgeFunction SetiFunction { get; set; }

        // Constructors
        public StoneEdgeIntegrationModel()
        {
            this.SetiFunction = StoneEdgeFunction.Unknown;            
        }
        public StoneEdgeIntegrationModel(string setiFunction)
        {            
            StoneEdgeFunction temp = StoneEdgeFunction.Unknown;
            Enum.TryParse(setiFunction, true, out temp);
            this.SetiFunction = temp;
        }        
    }
}