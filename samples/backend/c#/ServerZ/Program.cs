using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ZzzLab.Web;

namespace ZzzLab.MicroServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger.Info($"{AppConstant.AppName} Start: Console Mode");

            WebBuilder.CreateHostBuilder(args).Build().Run();
        }
    }
}