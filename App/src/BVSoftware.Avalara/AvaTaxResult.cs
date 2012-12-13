using System;
using System.Collections.Generic;
using System.Text;

namespace BVSoftware.Avalara
{
    public class AvaTaxResult
    {

        public bool Success { get; set; }
        public List<string> Messages { get; private set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalTax { get; set; }
        public string DocId { get; set; }

        public AvaTaxResult()
        {
            Success = false;
            Messages = new List<string>();
            TotalAmount = 0m;
            TotalTax = 0m;
            DocId = string.Empty;
        }
    }
}
