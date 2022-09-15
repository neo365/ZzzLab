using log4net;
using System.Reflection;
using System.Runtime.CompilerServices;
using ZzzLab.Logging;
using LogLevel = ZzzLab.Logging.LogLevel;

namespace ZzzLab.AspCore.Logging
{
    public abstract class Log4NetLoggerBase : LoggerBase
    {
        protected readonly ILog log;

        protected Log4NetLoggerBase()
            : this(Assembly.GetExecutingAssembly().GetName().Name ?? String.Empty)
        {
        }

        protected Log4NetLoggerBase(string name) : base(name)
        {
            log = LogManager.GetLogger(name);
        }

        public override void Log(LogLevel level,
            object value,
            [CallerMemberName] string? methodName = null)
        {
            if (level == LogLevel.Off) return;
            if (PrintLevel > level) return;

            try
            {
                switch (level)
                {
                    case LogLevel.Debug:
                        log.Debug(value);
                        break;

                    case LogLevel.Info:
                        log.Info(value);
                        break;

                    case LogLevel.Warning:
                        log.Warn(value);
                        break;

                    case LogLevel.Error:
                        log.Error(value);
                        break;

                    case LogLevel.Fatal:
                        log.Fatal(value);
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }
}