using System;
using System.Collections.Generic;
using System.Text;

namespace MSSurfing.Services.Logging
{
    public partial interface ILogger
    {
        Log InsertLog(LogLevel logLevel, string message);
    }
}
