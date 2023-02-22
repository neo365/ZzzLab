using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZzzLab.Json;

namespace ZzzLab.Configuration
{
    public class GlobalLoader : IConfigurationLoader<KeyValuePair<string, string>>
    {
        public IEnumerable<string> WatchFiles { get; }

        public GlobalLoader()
        {
            WatchFiles = Converter.ToIEnumerable(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets\config.conf"));
        }

        public IEnumerable<KeyValuePair<string, string>>? Reader()
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets\config.conf");
                return JsonConvert.DeserializeObjectFromFile<ConfigurationInfo>(filePath).Global;
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);

                return Enumerable.Empty<KeyValuePair<string, string>>();
            }
        }

        public void Writer(KeyValuePair<string, string> item)
        {
            // Do Nothing
        }
    }
}