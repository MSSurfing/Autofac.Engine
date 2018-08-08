using System;
using System.Collections.Generic;
using System.Text;

namespace MSSurfing.Services.Logging
{
    public static class LoggingExtensions
    {
        #region Utilities
        private static void FilteredLog(ILogger logger, LogLevel level, string message, Exception exception = null)
        {
            if (exception is System.Threading.ThreadAbortException)
                return;

            if (exception != null)
                message = $"log{{ message:{message};exception :{exception.ToString()}}}";
            logger.InsertLog(level, message);
        }
        #endregion

        public static void Debug(this ILogger logger, string message, Exception exception = null)
        {
            FilteredLog(logger, LogLevel.Debug, message, exception);
        }

        public static void Information(this ILogger logger, string message, Exception exception = null)
        {
            FilteredLog(logger, LogLevel.Information, message, exception);
        }

        public static void Warning(this ILogger logger, string message, Exception exception = null)
        {
            FilteredLog(logger, LogLevel.Warning, message, exception);
        }
        public static void Error(this ILogger logger, string message, Exception exception = null)
        {
            FilteredLog(logger, LogLevel.Error, message, exception);
        }
    }
}
