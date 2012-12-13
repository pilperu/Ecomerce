using System;
using System.Collections.Generic;
using System.Text;

namespace BVSoftware.Avalara
{
    public class Line
    {
        public string No {get;set;}
        public string OriginCode {get;set;}
        public string DestinationCode {get;set;}
        public string ItemCode {get;set;}
        public string TaxCode {get;set;}
        public decimal Qty {get;set;}
        public decimal Amount {get;set;}
        public bool Discounted {get;set;}
        public string RevAcct {get;set;}
        public string Ref1 {get;set;}
        public string Ref2 {get;set;}
        public string ExemptionNo {get;set;}
        public string CustomerUsageType {get;set;}
        public string Description {get;set;}
        
    }
}
