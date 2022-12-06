using ZzzLab;
using ZzzLab.Data;
using ZzzLab.Data.Configuration;
using ZzzLab.Logging;

namespace ConsoleSample
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("== Console Test Start ==============");

            Configurator.Initialize<GlobalReader>("test")
                        .AddLogger<PrintLogger>("dummy", Logger_Message)
                        .UseDBClient<DBConfigurationReader>()
            ;

            Logger.Debug("Hello, World!");

            using (IDBHandler DB = DataBaseHandler.Create("SAMPLE.Postgres"))
            {
                Logger.Debug(DB.SelectValue("SELECT now()"));
            }

            using (IDBHandler DB = DataBaseHandler.Create("SAMPLE.Oracle"))
            {
                Logger.Debug(DB.SelectValue("SELECT SYSDATE FROM DUAL"));
            }

            using (IDBHandler DB = DataBaseHandler.Create("RIST_LIMS"))
            {
                Logger.Debug(DB.SelectValue("SELECT SYSDATE FROM DUAL"));
            }

            QueryParameterCollection parameters = new QueryParameterCollection
            {
                { "log_id", Guid.NewGuid().ToString() },
                { "machine_name", Environment.MachineName },
                { "date_log", DateTime.Now.To24Hours() },
                { "log_level", "Debug" },
                { "stacktrace","stacktrace" },
                { "logger", "test" },
                { "message","message" },
            };


            string DefaultSql = ""
                + "INSERT INTO debug_logger ("
                + " log_id,  machine_name, date_log, stacktrace, log_level, logger, message "
                + " ) VALUES( "
                + " #{log_id}, #{machine_name}, #{date_log}, #{stacktrace}, #{log_level}, #{logger}, #{message}"
                + " ) ";


            using (IDBHandler DB = DataBaseHandler.Create("RIST_LIMS"))
            {
                Logger.Debug(DB.Excute(DefaultSql, parameters));
            }

            Console.WriteLine("== Console Test End  ==============");
            Console.ReadKey();
        }

        private static void Logger_Message(object sender, LogEventArgs e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}