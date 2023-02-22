using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZzzLab.Data;
using ZzzLab.Data.Configuration;
using ZzzLab.Json;

namespace ZzzLab.Configuration
{
    public class DBConfigurationLoader : IDBConfigurationLoader
    {
        public IEnumerable<string> WatchFiles { get; }

        public DBConfigurationLoader()
        {
            List<string> files = new List<string>
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets\config.conf"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets\sql")
            };

            WatchFiles = files;
        }

        public IEnumerable<ConnectionConfig>? Reader()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets\config.conf");

            ConfigurationInfo configInfo = JsonConvert.DeserializeObjectFromFile<ConfigurationInfo>(filePath);

            if (configInfo.DBconnections == null || configInfo.DBconnections.Any() == false) return Enumerable.Empty<ConnectionConfig>();

            foreach (ConnectionConfig config in configInfo.DBconnections)
            {
                if (string.IsNullOrWhiteSpace(config.ConnectionString))
                {
                    config.ConnectionString = DBClient.CreateConnectionString(config.ServerType, config.Host, config.Port.ToInt(), config.Database, config.UserId, config.Password);
                }
            }

            return configInfo.DBconnections;
        }

        public IEnumerable<SqlEntity> QueryReader()
        {
            List<SqlEntity> list = new List<SqlEntity>();

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets\sql");

            if (Directory.Exists(filePath) == false) return Enumerable.Empty<SqlEntity>();

            string[] files = Directory.GetFiles(filePath, "*.sql", SearchOption.AllDirectories);

            if (files != null && files.Any())
            {
                foreach (string f in files)
                {
                    try
                    {
                        list.AddRange(ReadFile(f));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                    }
                }
            }

            return list;
        }

        private static IEnumerable<SqlEntity> ReadFile(string file)
        {
            List<SqlEntity> list = new List<SqlEntity>();

            string fileName = Path.GetFileNameWithoutExtension(file);
            string[]? sqlArray = null;

            string text = File.ReadAllText(file).TrimStart('\n', '\r', ' ');

            if (text.StartsWithOr("--", "/*")) sqlArray = File.ReadAllLines(file);

            if (sqlArray == null || sqlArray.Any() == false) return list;

            string key = string.Empty;
            string query = string.Empty;

            foreach (string line in sqlArray)
            {
                string sqlline = line.TrimEnd('\r');

                if (sqlline.Trim().StartsWith("--["))
                {
                    if (string.IsNullOrWhiteSpace(key) == false)
                    {
                        list.Add(SqlEntity.Create(fileName, key, query));
                    }

                    key = sqlline.Trim()["--[".Length..].Replace("]", string.Empty);
                    query = string.Empty;
                }
                else
                {
                    query += " " + sqlline + System.Environment.NewLine;
                }
            }

            if (string.IsNullOrWhiteSpace(key) == false) list.Add(SqlEntity.Create(fileName, key, query));

            return list;
        }

        public void Writer(ConnectionConfig item)
            => throw new NotSupportedException();

        public bool QueryWriter(params SqlEntity[] collection)
            => throw new NotSupportedException();

        public bool QueryWriter(IEnumerable<SqlEntity> collection)
            => throw new NotSupportedException();
    }
}