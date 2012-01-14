using System;
using System.Xml.Serialization;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Metrics
{

	public class EventLogEntry
	{
        public string Bvin { get; set; }
        public DateTime EventTimeUtc { get; set; }
        public DateTime LastUpdatedUtc { get; set; }				
		public string Source {get;set;}
		public string Message {get;set;}
		public EventLogSeverity Severity {get;set;}
        public long StoreId { get; set; }

		public EventLogEntry()
		{
            this.Bvin = string.Empty;
            this.LastUpdatedUtc = DateTime.UtcNow;
			this.EventTimeUtc = DateTime.UtcNow;
            this.Source = string.Empty;
            this.Message = string.Empty;
            this.Severity = EventLogSeverity.None;
            this.StoreId = 0;
		}

		public EventLogEntry(Exception ex, long storeId): this()
		{
			EventTimeUtc = DateTime.UtcNow;
			Severity = EventLogSeverity.Error;
			if (ex != null) {
				if (ex.Source != null) {
					Source = ex.Source;
				}
				else {
					Source = string.Empty;
				}
				if (ex.Message != null) {
					Message = ex.Message + "[ " + ex.StackTrace + " ]";
				}
				else {
					Message = string.Empty;
				}
			}
            
            try
            {
                this.StoreId = storeId;
            }
            catch
            {
                // suppress
            }

		}

		public EventLogEntry(string src, string msg, EventLogSeverity sv, long storeId): this()
		{
			EventTimeUtc = DateTime.UtcNow;
            LastUpdatedUtc = DateTime.UtcNow;
			Source = src;
			Message = msg;
			Severity = sv;

            try
            {
                this.StoreId = storeId;
            }
            catch
            {
                // suppress
            }
		}	

	}

}

