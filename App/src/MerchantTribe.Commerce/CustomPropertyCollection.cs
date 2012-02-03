using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Text;

namespace MerchantTribe.Commerce
{
    [Serializable()]    
    public class CustomPropertyCollection : Collection<CustomProperty>
    {

        public CustomProperty this[string val]
        {
            get
            {
                foreach (CustomProperty value in this.Items)
                {
                    if (string.Compare(value.Key, val, true) == 0)
                    {
                        if (string.Compare(value.DeveloperId, "bvsoftware", true) == 0)
                        {
                            return value;
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (CustomProperty item in this.Items)
                {
                    if (string.Compare(item.Key, val, true) == 0)
                    {
                        if (string.Compare(item.DeveloperId, "bvsoftware", true) == 0)
                        {
                            item.Value = value.Value;
                        }
                    }
                }
            }
        }
        public CustomProperty this[string val, string developerId]
        {
            get
            {
                foreach (CustomProperty value in this.Items)
                {
                    if (string.Compare(value.Key, val, true) == 0)
                    {
                        if (string.Compare(value.DeveloperId, developerId, true) == 0)
                        {
                            return value;
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (CustomProperty item in this.Items)
                {
                    if (string.Compare(item.Key, val, true) == 0)
                    {
                        if (string.Compare(item.DeveloperId, developerId, true) == 0)
                        {
                            item.Value = value.Value;
                        }
                    }
                }
            }
        }
        public void Add(string devId, string key, string value)
        {
            CustomProperty item = new CustomProperty(devId, key, value);
            if (this[key, devId] == null)
            {
                this.Items.Add(item);
            }
            else
            {
                this[key, devId].Value = value;
            }
        }

        public bool Exists(string devId, string propertyKey)
        {
            bool result = false;
            for (int i = 0; i <= this.Count - 1; i++)
            {
                if (this[i].DeveloperId.Trim().ToLower() == devId.Trim().ToLower())
                {
                    if (this[i].Key.Trim().ToLower() == propertyKey.Trim().ToLower())
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
        public void SetProperty(string devId, string key, string value)
        {
            bool found = false;

            for (int i = 0; i <= this.Count - 1; i++)
            {
                if (this[i].DeveloperId.Trim().ToLower() == devId.Trim().ToLower())
                {
                    if (this[i].Key.Trim().ToLower() == key.Trim().ToLower())
                    {
                        this[i].Value = value;
                        found = true;
                        break;
                    }
                }
            }

            if (found == false)
            {
                this.Add(new CustomProperty(devId, key, value));
            }
        }
        public void SetProperty(string devId, string key, int value)
        {
            SetProperty(devId, key, value.ToString());            
        }
        public string GetProperty(string devId, string key)
        {
            string result = string.Empty;

            for (int i = 0; i <= this.Count - 1; i++)
            {
                if (this[i].DeveloperId.Trim().ToLower() == devId.Trim().ToLower())
                {
                    if (this[i].Key.Trim().ToLower() == key.Trim().ToLower())
                    {
                        result = this[i].Value;
                        break;
                    }
                }
            }

            return result;
        }
        public int GetPropertyAsInt(string devId, string key)
        {
            int result = 0;

            for (int i = 0; i <= this.Count - 1; i++)
            {
                if (this[i].DeveloperId.Trim().ToLower() == devId.Trim().ToLower())
                {
                    if (this[i].Key.Trim().ToLower() == key.Trim().ToLower())
                    {
                        result = this[i].GetValueAsInt();
                        break;
                    }
                }
            }

            return result;
        }
        public bool Remove(string devId, string key)
        {
            bool result = false;

            for (int i = 0; i <= this.Count - 1; i++)
            {
                if (this[i].DeveloperId.Trim().ToLower() == devId.Trim().ToLower())
                {
                    if (this[i].Key.Trim().ToLower() == key.Trim().ToLower())
                    {
                        this.Remove(this[i]);
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
        public string ToXml()
        {
            string result = string.Empty;

            try
            {
                StringWriter sw = new StringWriter();
                XmlSerializer xs = new XmlSerializer(this.GetType());
                xs.Serialize(sw, this);
                result = sw.ToString();
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
                result = string.Empty;
            }

            return result;
        }
        public static CustomPropertyCollection FromXml(string data)
        {
            CustomPropertyCollection result = new CustomPropertyCollection();

            try
            {
                StringReader tr = new StringReader(data);
                XmlSerializer xs = new XmlSerializer(result.GetType());
                result = (CustomPropertyCollection)xs.Deserialize(tr);
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
                result = new CustomPropertyCollection();                
            }
            return result;
        }        

    }

}
