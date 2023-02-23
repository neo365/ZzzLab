using System;
using System.Diagnostics;
using ZzzLab.Logging;
using ZzzLab.Web.Hubs;

namespace ZzzLab.MicroServer
{
    internal class AppHelper
    {
        internal static IZLogger AppLogger { private set; get; } = new PrintLogger("PrintOut");

        internal static IZLogger GetAppLogger(string name = "PrintOut")
        {
            AppLogger = new PrintLogger(name);

            AppLogger.Message += (s, e) =>
            {
                string message = $"[{e.Level} | {e.LogDateTime.To24Hours()}] {e.Value}";

                Debug.WriteLine(message);
                Console.WriteLine(message);
                NotifyHub.DebugMessage(message);
            };

            return AppLogger;
        }
    }
}