using System.Diagnostics;
using System.Net;
using ZzzLab.AspCore.Configuration;
using ZzzLab.Configuration;
using ZzzLab.Data;
using ZzzLab.Data.Configuration;
using ZzzLab.Logging;
using ZzzLab.Web.Configuration;

namespace ZzzLab.AspCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfigBuilder builder = Configurator.Initialize<GlobalLoader>("test")
                        .AddLogger<PrintLogger>("dummy", Logger_Message)
                        .UseDBClient<DBConfigurationLoader>()
                        .UseWebAuth<UserAuthConfiguration>()
                        .UseJWTAuth<UserAuthConfiguration>();

            //builder.AddLogger(DatabaseLogger.Create("Sample", AppConstant.ConnectionName ?? string.Empty));

            Logger.Debug("Hello, World!");

            DBClient.Message += (message) => Debug.WriteLine(message);

            //string test = Configurator.Get("CLIENT_KEY");

            CreateHostBuilder(args).Build().Run();
        }

        private static void Logger_Message(object sender, LogEventArgs e)
        {
            Debug.WriteLine(e.ToString());
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    // 윈도우 이벤트 로그에 로그를 남긴다.
                    webBuilder.ConfigureLogging((ctx, logging) =>
                    {
                        logging.AddEventLog(options =>
                        {
                            options.SourceName = "OpenApi";                            
                        });
                    });

#if APP_SUPORT || false

                    int webPort = Configurator.Get("WebPort")?.ToInt() ?? 5051;

#if false
                    webBuilder.UseUrls($"http://localhost:{webPort}/");
#else
                    webBuilder.UseKestrel(opts =>
                    {
                        opts.Listen(IPAddress.Loopback, port: webPort);
                        opts.ListenAnyIP(webPort);
                    });
#endif
#endif
                });
    }
}