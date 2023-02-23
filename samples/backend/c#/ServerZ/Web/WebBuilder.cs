using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ZzzLab.Web.Configuration;
using ZzzLab.Web.Middelware;

namespace ZzzLab.Web
{
    internal class WebHostHelper
    {
        private static ILifetimeJob? _HostLifetimeJob;
        internal static ILifetimeJob? HostLifetimeJob
        {
            set
            {
                if(value != null)
                {
                   //value.
                }

                _HostLifetimeJob = value;
            }
            get => _HostLifetimeJob;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddConsole();

                    loggingBuilder.AddProvider();
                })
                .ConfigureWebHostDefaults(configure =>
                {
                    AppConstant.WebPort = Configurator.Get("WebPort")?.ToIntNullable() ?? AppConstant.BASE_WEBPORT;

                    configure.UseStartup<Startup>();

                    configure.ConfigureLogging((ctx, logging) =>
                    {
                        logging.AddEventLog(options =>
                        {
                            options.SourceName = AppConstant.AppName;
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

        public static void Start()
        {
            Task.Run(() => WebHostHelper.CreateHostBuilder(System.Environment.GetCommandLineArgs()).Build().Run());
        }

        public static void Stop()
            => _HostLifetimeJob?.Stop();

        public static void Restart()
        {
            Stop();
            Thread.Sleep(1000);
            Start();
        }
    }
}