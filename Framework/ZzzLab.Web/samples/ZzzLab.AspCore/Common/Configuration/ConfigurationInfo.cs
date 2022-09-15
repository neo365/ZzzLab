using Newtonsoft.Json;
using ZzzLab.Data.Configuration;
using ZzzLab.Web.Configuration;

namespace ZzzLab.AspCore.Configuration
{
    public class ConfigurationInfo
    {
        [JsonProperty(PropertyName = "global")]
        public Dictionary<string, string>? Global { get; set; }

        [JsonProperty(PropertyName = "connectionStrings")]
        public IEnumerable<ConnectionConfig>? DBconnections { get; set; }

        [JsonProperty(PropertyName = "authSettings")]
        public JwtSettings? AuthSettings { get; set; }
    }
}