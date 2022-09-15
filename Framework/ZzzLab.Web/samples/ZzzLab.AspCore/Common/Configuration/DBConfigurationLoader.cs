using ZzzLab.Data;
using ZzzLab.Data.Configuration;
using ZzzLab.Json;

namespace ZzzLab.AspCore.Configuration
{
    public class DBConfigurationLoader : IDBConfigurationLoader
    {
        private readonly string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets\config.conf");
        private readonly string SqlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets\sql");
        public IEnumerable<string> WatchFiles { get; }

        public DBConfigurationLoader()
        {
            List<string> files = new List<string>
            {
               ConfigFilePath,
               SqlFilePath
            };

            WatchFiles = files;
        }

        public IEnumerable<ConnectionConfig> Reader()
        {
            ConfigurationInfo configInfo = JsonConvert.DeserializeObjectFromFile<ConfigurationInfo>(ConfigFilePath);

            if (configInfo.DBconnections == null || configInfo.DBconnections.Any() == false) return Enumerable.Empty<ConnectionConfig>();

            foreach (ConnectionConfig config in configInfo.DBconnections)
            {
                if (string.IsNullOrWhiteSpace(config.ConnectionString))
                {
                    config.ConnectionString = DBClient.CreateConnectionString(config.ServerType, config.Host, config.Port.ToInt(), config.Database, config.UserId, config.Password);
                }
            }

            AppConstant.ConnectionName = "ZzzLab.Postgres";

            return configInfo.DBconnections;
        }

        public bool Addconfig(ConnectionConfig item)
        {
            ConfigurationInfo configInfo = JsonConvert.DeserializeObjectFromFile<ConfigurationInfo>(ConfigFilePath);

            if (configInfo.DBconnections == null || configInfo.DBconnections.Any() == false) return true;

            List<ConnectionConfig> list = new List<ConnectionConfig>(configInfo.DBconnections);

            if (list.Any(x => x.Name.EqualsIgnoreCase(item.Name))) throw new DuplicateItemException();

            list.Add(item);

            configInfo.DBconnections = list;

            File.WriteAllText(ConfigFilePath, JsonConvert.SerializeObject(configInfo));
            return true;
        }

        public bool RemoveConfig(string name)
        {
            ConfigurationInfo configInfo = JsonConvert.DeserializeObjectFromFile<ConfigurationInfo>(ConfigFilePath);

            if (configInfo.DBconnections == null || configInfo.DBconnections.Any() == false) return true;

            List<ConnectionConfig> list = new List<ConnectionConfig>(configInfo.DBconnections);

            ConnectionConfig? item = list.Find(x => x.Name.EqualsIgnoreCase(name));

            if (item == null) return true;

            if (list.Remove(item))
            {
                configInfo.DBconnections = list;

                File.WriteAllText(ConfigFilePath, JsonConvert.SerializeObject(configInfo));
                return true;
            }
            else return false;
        }

        public void Writer(ConnectionConfig item)
        {
            Logger.Debug($"v Writer => {item.Name}: {item.ConnectionString}");
        }

        public IEnumerable<SqlEntity> QueryReader()
        {
            List<SqlEntity> list = new List<SqlEntity>();

            if (Directory.Exists(SqlFilePath) == false) return Enumerable.Empty<SqlEntity>();

            string[] files = Directory.GetFiles(SqlFilePath, "*.sql", SearchOption.AllDirectories);

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
                    //query += " " + sqlline.Replace(";", string.Empty);
                    query += " " + sqlline + System.Environment.NewLine;
                }
            }

            if (string.IsNullOrWhiteSpace(key) == false) list.Add(SqlEntity.Create(fileName, key, query));

            return list;
        }

        public bool QueryWriter(params SqlEntity[] collection)
            => QueryWriter((IEnumerable<SqlEntity>)collection);

        public bool QueryWriter(IEnumerable<SqlEntity> collection)
        {
            throw new NotImplementedException();
        }
    }
}