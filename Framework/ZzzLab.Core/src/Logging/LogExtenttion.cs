using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ZzzLab.Logging
{
    public static class LogExtenttion
    {
        public static LogLevel ToLogLevel(this TraceLevel level)
        {
            switch (level)
            {
                case TraceLevel.Off: return LogLevel.None;
                case TraceLevel.Verbose: return LogLevel.Debug;
                case TraceLevel.Info: return LogLevel.Information;
                case TraceLevel.Warning: return LogLevel.Warning;
                case TraceLevel.Error: return LogLevel.Error;
                default:
                    break;
            }

            return LogLevel.Critical;
        }
    }
}