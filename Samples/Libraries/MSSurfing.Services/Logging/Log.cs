using System;
using System.Collections.Generic;
using System.Text;

namespace MSSurfing.Services.Logging
{
    public partial class Log : BaseEntity
    {
        public int LogLevelId { get; set; }
        public LogLevel LogLevel
        {
            get => (LogLevel)this.LogLevelId;
            set => this.LogLevelId = (int)value;
        }
        public string Message { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
