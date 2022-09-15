using System.Diagnostics;

namespace ZzzLab.Logging
{
    public static class LogExtenttion
    {
        public static LogLevel ToLogLevel(this TraceLevel level)
        {
            switch (level)
            {
                case TraceLevel.Off: return LogLevel.Off;
                case TraceLevel.Verbose: return LogLevel.Debug;
                case TraceLevel.Info: return LogLevel.Info;
                case TraceLevel.Warning: return LogLevel.Warning;
                case TraceLevel.Error: return LogLevel.Error;
                default:
                    break;
            }

            return LogLevel.Fatal;
        }
    }
}