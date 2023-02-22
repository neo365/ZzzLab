using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ZzzLab.Web
{
    internal class WebBuilder
    {
        internal static IHostApplicationLifetime? HostLifetime { set; get; }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureWebHostDefaults(configure =>
                {
                    AppConstant.WebPort = Configurator.Get("WebPort")?.ToIntNullable() ?? AppConstant.BASE_WEBPORT;

                    configure.UseStartup<Startup>();

                    configure.ConfigureLogging((ctx, logging) =>
                    {
                        logging.AddEventLog(options =>
                        {
                            options.SourceName = "RistService";
                        });
                    });
#if false
                    configure.UseUrls($"http://localhost:{AppConstant.WebPort}/");
#else
                    configure.UseKestrel(opts =>
                    {
                        opts.Listen(IPAddress.Loopback, port: AppConstant.WebPort);
                        opts.ListenAnyIP(AppConstant.WebPort);
                    });
#endif
                });

        public static void StartServer()
        {
            Task.Run(() => WebBuilder.CreateHostBuilder(System.Environment.GetCommandLineArgs()).Build().Run());
        }

        public static void StopServer()
        { 
            HostLifetime?.StopApplication();
            Thread.Sleep(1000);
        }

        public static void RestartServer()
        {
            StopServer();
            Thread.Sleep(1000);
            StartServer();
        }
    }
}