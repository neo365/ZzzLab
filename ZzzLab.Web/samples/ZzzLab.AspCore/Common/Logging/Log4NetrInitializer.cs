using log4net.Appender;
using log4net.Core;
using System.Diagnostics;
using ZzzLab.Data.Configuration;

namespace ZzzLab.AspCore.Logging
{
    /// <summary>
    /// Log4net에 대한 구현
    /// </summary>
    public static class Log4NetrInitializer
    {
        internal static void AddDatabaseAppender(string name, ConnectionConfig config)
            => AddAppender(new DatabaseAppender(name, config));

        internal static void AddDatabaseAppender(DatabaseAppender appender)
            => AddAppender(appender);

        internal static void AddRollingFileAppender(RollingFileAppender appender)
            => AddAppender(appender);

        private static void AddAppender(object obj)
        {
            log4net.Repository.Hierarchy.Hierarchy hierarchy = (log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository();

            if (obj is IAppender appender)
            {
                if (hierarchy.Root.GetAppender(appender.Name) != null)
                {
                    Debug.WriteLine($"====> '{appender.Name}' is Duplicated");
                    return;
                }

                hierarchy.Root.AddAppender(appender);
            }
            else throw new InvalidArgumentException(nameof(obj));

            if (obj is IOptionHandler optionHandler)
            {
                optionHandler.ActivateOptions();
            }

            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;
        }
    }
}