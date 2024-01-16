using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZzzLab.Crypt;
using ZzzLab.Data.Configuration;
using ZzzLab.Json;

namespace ZzzLab.Configuration.DataBase
{
    internal class ConfigurationLoader : IConfigurationLoader<ConnectionConfig>
    {
        private static readonly string[] BASE_CONFIGPATH = {
                Path.Combine(Configurator.APP_PATH, "Config.conf"),
                Path.Combine(Configurator.APPDATA_PATH, "Config.conf"),
                Path.Combine(Configurator.APP_PATH, "assets",  "Config.conf"),
                Path.Combine(Configurator.APP_PATH, "assets", "config", "Config.conf"),
                Path.Combine(Configurator.APP_PATH, "bin", "assets",  "Config.conf"),
                Path.Combine(Configurator.APP_PATH, "bin",  "Config.conf")
            };

        public IEnumerable<string> WatchFiles { private set; get; }

        private string GetFilePath(string[] filePath = null)
        {
            filePath ??= BASE_CONFIGPATH;

            foreach (string f in filePath)
            {
                if (File.Exists(f)) return f;
            }

            return null;
        }

        public IEnumerable<ConnectionConfig> Reader()
        {
            string filePath = GetFilePath();

            if (string.IsNullOrWhiteSpace(filePath)) return Enumerable.Empty<ConnectionConfig>();

            List<string> files = new List<string>(1)
            {
                filePath
            };

            WatchFiles = files;

            string json = File.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(json)) throw new InitializeException();

            if (json.Trim().StartsWith("{") == false)
            {
                if (string.IsNullOrWhiteSpace(Configurator.CryptKey)) throw new InitializeException();

                json = AESCrypt.Decrypt(json, Configurator.CryptKey);
            }

            IEnumerable<ConnectionConfig> config = JsonConvert.DeserializeObject<IEnumerable<ConnectionConfig>>(json);

            if (config == null || config.Any() == false) return Enumerable.Empty<ConnectionConfig>();

            return config;
        }

        public void Writer(ConnectionConfig item)
        {
            Logger.Debug($"GlobalReader Writer => {item}");
        }
    }
}