using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ZzzLab.Logging
{
    public interface ILogger
    {
        string Name { get; }

        LogLevel PrintLevel { get; }

        void Debug(object value, [CallerMemberName] string methodName = null);

        void Info(object value, [CallerMemberName] string methodName = null);

        void Warning(object value, [CallerMemberName] string methodName = null);

        void Error(object value, [CallerMemberName] string methodName = null);

        void Fatal(object value, [CallerMemberName] string methodName = null);

        void Log(LogLevel level, object value, [CallerMemberName] string methodName = null);

        void Log(TraceLevel level, object value, [CallerMemberName] string methodName = null);

        event LogEventHandler Message;
    }
}