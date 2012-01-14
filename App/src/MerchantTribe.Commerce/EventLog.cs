using System;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Data;
using log4net;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce
{
    public class EventLog: MerchantTribe.Web.Logging.ILogger
    {

        public EventLog()
        {

        }

        /// <summary>
        /// Records and event to the BV Commerce event log
        /// </summary>
        /// <param name="source">The source of the event</param>
        /// <param name="message">The description or information about the event</param>
        /// <returns>True if the event was recorded, otherwise false</returns>
        public static bool LogEvent(string source, string message, EventLogSeverity severity)
        {
           
            ILog Log = LogManager.GetLogger(source);

            switch (severity)
            {
                case EventLogSeverity.Debug:
                    Log.Debug(message);
                    break;
                case EventLogSeverity.Error:
                    Log.Error(message);
                    break;
                case EventLogSeverity.Fatal:
                    Log.Error(message);
                    break;
                case EventLogSeverity.Information:
                    Log.Info(message);
                    break;
                case EventLogSeverity.None:
                    Log.Info(message);
                    break;
                case EventLogSeverity.Warning:
                    Log.Warn(message);
                    break;
            }

            return true;    
        }


        public static bool StoreLogEvent(long storeId, string source, string message, EventLogSeverity severity)
        {
            bool ret = false;
            Metrics.EventLogEntry e = new Metrics.EventLogEntry(source, message, severity, storeId);
            e.StoreId = storeId;
            try
            {
                RequestContext context = new RequestContext();
                context.CurrentStore = new Accounts.Store() { Id = storeId };
                Metrics.EventLogRepository repository = new Metrics.EventLogRepository(context);
                ret = repository.Create(e);
                repository.Roll();
            }
            catch(Exception ex)
            {
                LogEvent(ex);
                return false;
            }
            return true;
        }
        public static bool StoreLogEvent(long storeId, Exception ex)
        {
            StoreLogEvent(storeId, "Exception", ex.Message + " " + ex.StackTrace, EventLogSeverity.Error);
            return true;
        }

        /// <summary>
        /// Logs an exception to the BV Commerce log
        /// </summary>
        /// <param name="ex">Exception to be recorded</param>
        /// <returns>True if the exception was recorded, otherwise false</returns>
        public static bool LogEvent(Exception ex)
        {
            return LogEvent(ex, EventLogSeverity.Error);            
        }
        public static bool LogEvent(Exception ex, EventLogSeverity severity)
        {
            ILog Log = LogManager.GetLogger("MerchantTribe.System");
            switch (severity)
            {
                case EventLogSeverity.Debug:
                    Log.Debug(ex);
                    break;
                case EventLogSeverity.Error:
                    Log.Error(ex);
                    break;
                case EventLogSeverity.Fatal:
                    Log.Error(ex);
                    break;
                case EventLogSeverity.Information:
                    Log.Info(ex);
                    break;
                case EventLogSeverity.None:
                    Log.Info(ex);
                    break;
                case EventLogSeverity.Warning:
                    Log.Warn(ex);
                    break;
            }
            return true;
        }
                            

        #region ILogger Members

        public void LogMessage(string message)
        {
            EventLog.LogEvent("Logger STORE 0", message, EventLogSeverity.Information);
        }

        public void LogException(Exception ex)
        {
            EventLog.LogEvent(ex);
        }
        
        public void LogException(Exception ex, EventLogSeverity severity)
        {
            EventLog.LogEvent(ex, severity);            
        }

        public void LogMessage(string source, string message, EventLogSeverity severity)
        {
            EventLog.LogEvent(source, message, severity);
        }


        public void LogMessageStore(string message, long storeId)
        {
            EventLog.StoreLogEvent(storeId, "Logger", message, EventLogSeverity.Information);
        }

        public void LogExceptionStore(Exception ex, long storeId)
        {
            EventLog.StoreLogEvent(storeId, ex);
        }
        #endregion
    }

}