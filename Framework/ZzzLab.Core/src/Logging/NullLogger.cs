using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace ZzzLab.Logging
{
    public class NullLogger : LoggerBase, ILogger
    {
        public NullLogger() : base()
        {
        }

        public NullLogger(string loggerName) : base(loggerName)
        {
        }

        public override void Log(LogLevel level,
                    object value,
                    [CallerMemberName] string methodName = null)
        {
            // Do Nothing
        }
    }
}