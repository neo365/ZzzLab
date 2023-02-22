using System;
using System.Diagnostics;
using ZzzLab.Logging;
using ZzzLab.Web.Hubs;

namespace ZzzLab.MicroServer
{
    internal class AppHelper
    {
        internal static ILogger AppLogger { private set; get; } = new PrintLogger("PrintOut");

        internal static ILogger GetAppLogger(string name = "PrintOut")
        {
            AppLogger = new PrintLogger(name);

            AppLogger.Message += (s, e) =>
            {
                Debug.WriteLine(e.ToString());
                Console.WriteLine(e.ToString());
                NotifyHub.DebugMessage(e.ToString());
            };

            return AppLogger;
        }

        internal static void LoggerPrint(object sender, LogEventArgs e)
        {
            Debug.WriteLine(e.ToString());
            Console.WriteLine(e.ToString());
            NotifyHub.DebugMessage(e.ToString());
        }
    }
}