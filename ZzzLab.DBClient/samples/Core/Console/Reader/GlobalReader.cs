using Microsoft.Extensions.Configuration;
using ZzzLab;
using ZzzLab.Configuration;

namespace ConsoleSample
{
    public class GlobalReader : IConfigurationLoader<KeyValuePair<string, string>>
    {
        public string? FilePath { private set; get; }

        public IEnumerable<string> WatchFiles { get; }

        public GlobalReader()
        {
            List<string> files = new List<string>(1)
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config.conf")
            };

            WatchFiles = files;
        }

        public IEnumerable<KeyValuePair<string, string>> Reader()
        {
            try
            {
                var config = new ConfigurationBuilder()
                                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                .AddJsonFile("config.conf").Build();

                return config.GetSection("global").Get<IDictionary<string, string>>();
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);

                return Enumerable.Empty<KeyValuePair<string, string>>();
            }
        }

        public void Writer(KeyValuePair<string, string> item)
        {
            Logger.Debug($"GlobalReader Writer => {item.Key}: {item.Value}");
        }
    }
}