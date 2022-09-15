using System.Diagnostics;
using ZzzLab.AspCore.Logging;
using ZzzLab.Logging;
using ILogger = ZzzLab.Logging.ILogger;

namespace ZzzLab.AspCore.ScheduleTask
{
    public static class ScheduleHelper
    {
        public static ILogger Logger { get; }

        static ScheduleHelper()
        {
            Logger = DatabaseLogger.Create("Schedule", AppConstant.ConnectionName ?? string.Empty);
            Logger.Message += ReciveMessage;
        }

        private static void ReciveMessage(object sender, LogEventArgs e)
        {
            Debug.WriteLine(e.ToString());
        }
    }
}