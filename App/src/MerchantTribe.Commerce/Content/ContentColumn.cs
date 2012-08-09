using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Text;

namespace MerchantTribe.Commerce.Content
{
	public class ContentColumn
	{        
        public string Bvin {get;set;}
        public long StoreId { get; set; }
        public DateTime LastUpdated {get;set;}
		public string DisplayName {get;set;}
		public bool SystemColumn {get;set;}
		public List<ContentBlock> Blocks {get;set;}

        public ContentColumn()
        {
            this.Bvin = string.Empty;
            this.StoreId = 0;
            this.LastUpdated = DateTime.UtcNow;
            this.DisplayName = string.Empty;
            this.SystemColumn = false;
            this.Blocks = new List<ContentBlock>();
        }

        private XmlWriterSettings _BVXmlWriterSettings = new XmlWriterSettings();
        private XmlReaderSettings _BVXmlReaderSettings = new XmlReaderSettings();
        public virtual string ToXml(bool omitDeclaration)
        {
            string response = string.Empty;
            StringBuilder sb = new StringBuilder();
            _BVXmlWriterSettings.ConformanceLevel = ConformanceLevel.Fragment;
            XmlWriter xw = XmlWriter.Create(sb, _BVXmlWriterSettings);
            ToXmlWriter(ref xw);
            xw.Flush();
            xw.Close();
            response = sb.ToString();
            return response;
        }
        public virtual bool FromXmlString(string x)
        {
            System.IO.StringReader sw = new System.IO.StringReader(x);
            XmlReader xr = XmlReader.Create(sw);
            bool result = FromXml(ref xr);
            sw.Dispose();
            xr.Close();
            return result;
        }
        public bool FromXml(ref System.Xml.XmlReader xr)
        {
            throw new NotImplementedException();
        }

        public void ToXmlWriter(ref System.Xml.XmlWriter xw)
        {
            if (xw != null)
            {
                xw.WriteStartElement("ContentColumn");

                xw.WriteElementString("Bvin", this.Bvin);
                xw.WriteElementString("DisplayName", this.DisplayName);

                xw.WriteStartElement("SystemColumn");
                xw.WriteValue(this.SystemColumn);
                xw.WriteEndElement();

                xw.WriteStartElement("Blocks");
                foreach (ContentBlock b in this.Blocks)
                {
                    b.ToXmlWriter(ref xw);
                }
                xw.WriteEndElement();


                xw.WriteEndElement(); // end Column
            }
        }

	}
}
