using ZzzLab;
using ZzzLab.Configuration;

namespace ConsoleSample
{
    public class GlobalLoader : IConfigurationLoader<KeyValuePair<string, string>>
    {
        public IEnumerable<string> WatchFiles { get; }

        public GlobalLoader()
        {
            List<string> files = new List<string>(1)
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.conf")
            };

            WatchFiles = files;
        }

        public IEnumerable<KeyValuePair<string, string>> Reader()
        {
            return Enumerable.Empty<KeyValuePair<string, string>>();
        }

        public void Writer(KeyValuePair<string, string> item)
        {
            Logger.Debug($"Writer => {item.Key}: {item.Value}");
        }
    }
}