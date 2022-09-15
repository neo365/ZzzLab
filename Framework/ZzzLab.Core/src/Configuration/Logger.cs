using System.Runtime.CompilerServices;

namespace ZzzLab
{
    public static class Logger
    {
        internal delegate void LogWriterDelegate(object value, [CallerMemberName] string methodName = null);

        internal static LogWriterDelegate DebugFunc;
        internal static LogWriterDelegate InfoFunc;
        internal static LogWriterDelegate WarningFunc;
        internal static LogWriterDelegate ErrorFunc;
        internal static LogWriterDelegate FatalFunc;

        public static void Debug(object value, [CallerMemberName] string methodName = null)
            => DebugFunc?.Invoke(value, methodName);

        public static void Info(object value, [CallerMemberName] string methodName = null)
            => InfoFunc?.Invoke(value, methodName);

        public static void Warning(object value, [CallerMemberName] string methodName = null)
            => WarningFunc?.Invoke(value, methodName);

        public static void Error(object value, [CallerMemberName] string methodName = null)
            => ErrorFunc?.Invoke(value, methodName);

        public static void Fatal(object value, [CallerMemberName] string methodName = null)
            => FatalFunc?.Invoke(value, methodName);
    }
}