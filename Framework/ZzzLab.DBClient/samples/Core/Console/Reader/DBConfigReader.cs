using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using ZzzLab;
using ZzzLab.Data;
using ZzzLab.Data.Configuration;

namespace ConsoleSample
{
    public class DBConfigurationReader : IDBConfigurationLoader
    {
        public string? FilePath { private set; get; }

        public IEnumerable<string> WatchFiles { get; }

        public DBConfigurationReader()
        {
            List<string> files = new List<string>(1)
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config.conf")
            };

            WatchFiles = files;
        }

        public IEnumerable<ConnectionConfig> Reader()
        {
            var config = new ConfigurationBuilder()
                            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                            .AddJsonFile("config.conf").Build();
            return config
                 .GetSection("database:connections")
                 .Get<ConnectionConfig[]>();
        }

        public void Writer(ConnectionConfig item)
        {
            // Do Nothing
        }

        public IEnumerable<SqlEntity> QueryReader()
        {
            List<SqlEntity> list = new List<SqlEntity>();

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"sql");

            if (Directory.Exists(filePath) == false) return Enumerable.Empty<SqlEntity>();

            string[] files = Directory.GetFiles(filePath, "*.sql", SearchOption.AllDirectories);

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

                    key = sqlline.Trim().Substring("--[".Length).Replace("]", string.Empty);
                    query = string.Empty;
                }
                else
                {
                    //query += " " + sqlline.Replace(";", string.Empty);
                    query += " " + sqlline + System.Environment.NewLine;
                }
            }

            if (string.IsNullOrWhiteSpace(key) == false) list.Add(SqlEntity.Create(fileName, key, query));

            Debug.WriteLine(query);
            Debug.WriteLine(SQLUtils.Formatter(query));

            return list;
        }

        public bool QueryWriter(params SqlEntity[] collection)
            => QueryWriter((IEnumerable<SqlEntity>)collection);

        public bool QueryWriter(IEnumerable<SqlEntity> collection)
        {
            if (collection == null || collection.Any() == false) return false;

            foreach (SqlEntity entity in collection)
            {
                string fileName = $"{entity.Section.ToUpper()}.sql";
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"sql", fileName);

                string text = Environment.NewLine
                    + $"--[{entity.Label}]" + Environment.NewLine
                    + entity.Command + Environment.NewLine;

                if (File.Exists(filePath) == false) File.Create(filePath);

                File.AppendAllText(filePath, text);
            }

            return true;
        }
    }
}