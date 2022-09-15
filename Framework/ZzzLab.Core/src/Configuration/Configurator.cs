using System;
using System.Collections.Generic;
using System.IO;
using ZzzLab.Configuration;

namespace ZzzLab
{
    public static class Configurator
    {
        public static string APP_PATH => AppDomain.CurrentDomain.BaseDirectory;
        public static string APP_NAME => Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);
        public static string APPDATA_PATH { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), APP_NAME);
        public static string CryptKey { internal set; get; }
        public static dynamic Global => Instance?.Global;
        public static dynamic Setting => Instance?.Setting;

        internal static ConfigBuilder Instance;

        /// <summary>
        /// Configuration Initializer
        /// </summary>
        /// <param name="cryptKey">암호키 지정</param>
        public static IConfigBuilder Initialize(string cryptKey = null)
        {
            if (Instance == null) Instance = new ConfigBuilder();
            else return Instance;

            CryptKey = cryptKey;

            return Instance;
        }

        public static IConfigBuilder Initialize<T>(string cryptKey = null) where T : IConfigurationLoader<KeyValuePair<string, string>>
        {
            if (Instance == null)
            {
                if (Activator.CreateInstance(typeof(T)) is IConfigurationLoader<KeyValuePair<string, string>> reader)
                {
                    Instance = new ConfigBuilder(reader);
                }
                else throw new InvalidTypeException(typeof(T));
            }

            return Initialize(cryptKey);
        }

        public static string Get(string name)
        => (Instance != null && Instance.Global.ContainsKey(name) ? Instance.Global[name] as string : null);

        public static void Set(string name, string value)
            => Instance?.Writer(name, value);

        public static string[] AllKeys
        {
            get => Instance?.Global.Keys;
        }
    }
}