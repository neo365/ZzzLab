using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ZzzLab.Logging
{
    public class PrintLogger : LoggerBase, ILogger
    {
        public PrintLogger() : base()
        {
        }

        public PrintLogger(string loggerName) : base(loggerName)
        {
        }

        public override void Log(
            LogLevel level,
            object value,
            [CallerMemberName] string methodName = null
        )
        {
            DateTime logDateTime = DateTime.Now;

            StackTrace st = new StackTrace(true);
            methodName = st.GetFrame(3).GetMethod().ReflectedType.ToString();

            OnMessage(level, methodName, value.ToString(), logDateTime);
        }
    }
}