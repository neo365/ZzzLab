using System.Diagnostics;
using ZzzLab;
using ZzzLab.Json;
using ZzzLab.Logging;
using ZzzLab.Scheduler;

namespace ConsoleSample
{
    public class Program
    {
        internal static PrintLogger ScheduleLogger = new PrintLogger();

        private static void Main()
        {
            ScheduleLogger.Message += LoggerPrint;
            Configurator.Initialize<GlobalLoader>("test")
                .AddLogger<PrintLogger>("Sample", LoggerPrint)
                ;

            Logger.Info("Configuration Success.");

            try
            {
                ScheduleManager.AddJob<Test01Job>("0 0/1 * 1/1 * ? *");
                ScheduleManager.AddJob<Test02Job>(60);

                while (true)
                {
                    Thread.Sleep(5000);
                    Debug.WriteLine(ScheduleManager.GetAllJobs().ToArray().ToJson());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadKey();

            ScheduleManager.Shutdown();
        }

        private static void LoggerPrint(object sender, LogEventArgs e)
        {
            if (e.Level >= LogLevel.Info)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}