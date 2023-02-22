using System;
using System.IO;
using System.Reflection;
using ZzzLab.Configuration;
using ZzzLab.Data.Configuration;
using ZzzLab.MicroServer;

namespace ZzzLab
{
    public static class AppConstant
    {
        public static string AppName { private set; get; } = "DevTool";
        public static string DisplayName { private set; get; } = "DevTool™";
        public static string AppPath => AppDomain.CurrentDomain.BaseDirectory;
        public static string APPDATA_PATH => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), AppName, "LIMS");

        public const string AES_KEY = "5209cd0198da49808102aca17a5bfc01";

        internal const int BASE_WEBPORT = 8081;
        internal static string? ConnectionName { set; get; } = "BASE_DB";
        public static int WebPort { set; get; } = BASE_WEBPORT;

        static AppConstant()
        {
            Configurator.Initialize<GlobalLoader>(AppConstant.AES_KEY)
                .AddLogger(AppHelper.GetAppLogger())
                .UseDBClient<DBConfigurationLoader>();

            AppName = Configurator.Get("AppName") ?? "ServerZ";
            DisplayName = Configurator.Get("DisplayName") ?? "ServerZ™";

            Logger.Info($"{AppConstant.AppName} Config Load");
        }

        #region Versioning

        public static string AppVersion => GetVersionString();

        private static Version? GetVersion()
            => Assembly.GetExecutingAssembly().GetName().Version;

        private static string? _CaschedVersionString = null;

        private static string GetVersionString()
        {
            if (string.IsNullOrWhiteSpace(_CaschedVersionString) == false) return _CaschedVersionString;
            _CaschedVersionString = GetVersion()?.ToString();

            if (string.IsNullOrWhiteSpace(_CaschedVersionString)) return "x.x.x.x";

            return _CaschedVersionString;
        }

        #endregion Versioning
    }
}