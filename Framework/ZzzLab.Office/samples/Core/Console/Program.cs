using ZzzLab;
using ZzzLab.Logging;
using ZzzLab.Json;
using ZzzLab.Crypt;
using ZzzLab.Office;
using ZzzLab.Office.PDF;

namespace ConsoleSample
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("== Console Test Start ==============");

            try
            {
                ZzzLab.Logging.IZLogger logger = new PrintLogger();
                logger.Message += Logger_Message;

                Configurator.Initialize<ConfigurationLoader>("test")
                            //.Use<IServiceBuilder>();
                            //.AddLogger(logger)
                            .AddLogger<PrintLogger>("dummy", Logger_Message)
                    ;

                Logger.Debug("Hello, World!");

                int sourcePagecount = PdfToImage.ToFile(@"C:\Temp\diff\1.pdf", @"C:\Temp\diff\result", ImageType.PNG);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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