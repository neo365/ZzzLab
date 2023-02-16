using Newtonsoft.Json;

namespace ZzzLab.Web.Models.Auth
{
    public class OAuthError
    {
        [JsonProperty(PropertyName = "error")]
        public string? Error { set; get; }

        [JsonProperty(PropertyName = "error_description")]
        public string? ErrorDescription { set; get; }
    }
}