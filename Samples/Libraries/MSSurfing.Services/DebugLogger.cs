using Autofac.Engine;
using MSSurfing.Services.Logging;
using System.Diagnostics;

namespace MSSurfing.Services
{
    public static class DebugLogger
    {
        [Conditional("DEBUG")]
        public static void Debug(string message, System.Exception exception = null)
        {
            DebugIf(true, message: message, exception: exception);
        }

        [Conditional("DEBUG")]
        public static void DebugIf(bool condition, string message, System.Exception exception = null)
        {
            if (condition)
            {
                ILogger _logger = null;

                try { _logger = EngineContext.Resolve<ILogger>(); } catch { }
                _logger?.Debug(message, exception);
            }
        }

        [Conditional("DEBUG")]
        public static void Debug(ILogger logger, string message, System.Exception exception = null)
        {
            DebugIf(logger, true, message: message, exception: exception);
        }

        [Conditional("DEBUG")]
        public static void DebugIf(ILogger logger, bool condition, string message, System.Exception exception = null)
        {
            if (condition)
            {
                if (logger == null)
                    try { logger = EngineContext.Resolve<ILogger>(); } catch { }
                logger?.Debug(message, exception);
            }
        }
    }
}
