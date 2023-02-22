using Newtonsoft.Json;
using System.Collections.Generic;
using ZzzLab.Data.Configuration;

namespace ZzzLab.Configuration
{
    public class ConfigurationInfo
    {
        [JsonProperty(PropertyName = "setting")]
        public Dictionary<string, string>? Global { get; set; }

        [JsonProperty(PropertyName = "connectionStrings")]
        public ConnectionConfig[]? DBconnections { get; set; }
    }
}