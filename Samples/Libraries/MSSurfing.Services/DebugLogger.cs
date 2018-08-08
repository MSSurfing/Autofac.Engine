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
                var _logger = EngineContext.Resolve<ILogger>();
                _logger.Debug(message, exception);
            }
        }
    }
}
