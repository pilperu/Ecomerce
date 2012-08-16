using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.IO;
using MerchantTribe.Web;

namespace MerchantTribeStore.Controllers
{
    public class XmlResult : ActionResult
    {
        private object objectToSerialize;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlResult"/> class.
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize to XML.</param>
        public XmlResult(object objectToSerialize)
        {
            this.objectToSerialize = objectToSerialize;
        }

        /// <summary>
        /// Gets the object to be serialized to XML.
        /// </summary>
        public object ObjectToSerialize
        {
            get { return this.objectToSerialize; }
        }

        /// <summary>
        /// Serialises the object that was passed into the constructor to XML and writes the corresponding XML to the result stream.
        /// </summary>
        /// <param name="context">The controller context for the current request.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (this.objectToSerialize != null)
            {
                context.HttpContext.Response.Clear();
                context.HttpContext.Response.ContentType = "text/xml";
                
                //context.HttpContext.Response.Output.Write(MerchantTribe.Web.Xml.ObjectToXml(this.objectToSerialize));

                var xs = new System.Xml.Serialization.XmlSerializer(this.objectToSerialize.GetType());
                xs.Serialize(context.HttpContext.Response.Output, this.objectToSerialize);                                
            }
        }
      
    }
    
}