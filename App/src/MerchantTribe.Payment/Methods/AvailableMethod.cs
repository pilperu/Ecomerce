using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment.Methods
{
    public class AvailableMethod
    {
        public string Id { get; set; }
        public string Name { get; set; }
        //public bool Selected { get; set; }

        public AvailableMethod()
        {
        }
        public AvailableMethod(Method m)
        {
            this.Id = m.Id;
            this.Name = m.Name;
        }
        public AvailableMethod(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public static List<AvailableMethod> FindAll()
        {
            List<AvailableMethod> result = new List<AvailableMethod>();

            result.Add(new AvailableMethod(new TestGateway()));
            result.Add(new AvailableMethod(new AuthorizeNet()));
            result.Add(new AvailableMethod(new CybersourceSOAP()));
            result.Add(new AvailableMethod(new PayLeap()));
            result.Add(new AvailableMethod(new PayFlowPro()));
            result.Add(new AvailableMethod(new PayPalPaymentsPro()));            
            return result;
        }

        public static List<AvailableMethod> FindAllPayPalOnly()
        {
            List<AvailableMethod> result = new List<AvailableMethod>();

            result.Add(new AvailableMethod(new TestGateway()));
            //result.Add(new AvailableMethod(new AuthorizeNet()));
            //result.Add(new AvailableMethod(new PayLeap()));
            result.Add(new AvailableMethod(new PayFlowPro()));
            result.Add(new AvailableMethod(new PayPalPaymentsPro()));

            return result;
        }

        //public static Method Create(Guid id)
        //{
        //    return Create(id.ToString().Replace("{", "").Replace("}", ""));
        //}

        public static Method Create(string id)        
        {
            switch (id)
            {
                case "828F3F70-EF01-4db6-A385-C5467CF91587":
                    return new AuthorizeNet();
                case "FCACE46F-7B9C-4b49-82B6-426CF522C0C6":
                    return new TestGateway();
                case "6FC76AD8-66BF-47b0-8982-1C4118F01645":
                    return new PayLeap();
                case "6EF0F678-9C67-4ade-A95D-9E8DE2C7780B":
                    return new PayFlowPro();
                case "0B81046B-7A24-4512-8A6B-6C4C59D4C503":
                    return new PayPalPaymentsPro();
                case "C36CDBAB-28B0-4F47-B61C-87CED8EE7222":
                    return new CybersourceSOAP();
            }
            return null;
        }

    }
}
