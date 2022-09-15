using Newtonsoft.Json;
using ZzzLab.Models.Auth;

namespace ZzzLab.Web.Models.Auth
{
    public class OAuthSuccess<T> where T : BaseUserEntity
    {
        [JsonProperty(PropertyName = "access_token")]
        public string? AccessToken { set; get; }

        [JsonProperty(PropertyName = "token_type")]
        public string? TokenType { set; get; }

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { set; get; } = 3600;

        [JsonProperty(PropertyName = "refresh_token")]
        public string? RefreshToken { set; get; }

        [JsonProperty(PropertyName = "scope")]
        public string? Scope { set; get; }

        public T? User { set; get; }
    }
}