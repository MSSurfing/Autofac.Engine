using System;
using System.Collections.Generic;
using System.Text;

namespace MSSurfing.Services.Logging
{
    public partial class Logger : ILogger
    {
        #region Fields
        private readonly IRepository<Log> _logRepository;
        #endregion

        #region Ctor
        public Logger(IRepository<Log> logRepository)
        {
            _logRepository = logRepository;
        }
        #endregion

        public Log InsertLog(LogLevel logLevel, string message)
        {
            var log = new Log()
            {
                Id = Guid.NewGuid(),
                Message = message,
                LogLevel = logLevel,
                CreatedOn = DateTime.Now
            };

            _logRepository.Insert(log);
            return log;
        }
    }
}
