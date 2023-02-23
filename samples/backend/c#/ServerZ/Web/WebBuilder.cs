using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ZzzLab.Web
{
    internal class WebHostHelper
    {
        private static IHostApplicationLifetime? _HostLifetime { set; get; }
        internal static IHostApplicationLifetime? HostLifetime
        {
            set
            {
                if(value != null)
                {
                    value.
                }

                _HostLifetime = value;
            }
            get => _HostLifetime;
        }

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
            Task.Run(() => WebHostHelper.CreateHostBuilder(System.Environment.GetCommandLineArgs()).Build().Run());
        }

        public static void StopServer()
        {
            _HostLifetime?.StopApplication();
        }

        public static void RestartServer()
        {
            StopServer();
            Thread.Sleep(1000);
            StartServer();
        }
    }
}