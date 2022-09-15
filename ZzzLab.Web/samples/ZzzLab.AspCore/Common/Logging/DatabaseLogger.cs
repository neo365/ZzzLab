using ZzzLab.Data.Configuration;
using ILogger = ZzzLab.Logging.ILogger;

namespace ZzzLab.AspCore.Logging
{
    public class DatabaseLogger : Log4NetLoggerBase, ILogger
    {
        private static ConnectionCollection Connector
            => (ConnectionCollection)Configurator.Setting.Dbconnector;

        protected DatabaseLogger(string loggerName, string connectionName) : base(loggerName)
        {
            ConnectionConfig config = Connector[connectionName];

            DatabaseAppender appender = new DatabaseAppender(loggerName, config);
            Log4NetrInitializer.AddDatabaseAppender(appender);
        }

        public static ILogger Create(string loggerName, string connectionName)
        => new DatabaseLogger(loggerName, connectionName);
    }
}