using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using ZzzLab.Models.Auth;

namespace ZzzLab.Web.Models.Auth
{
    public class OAuthSuccess<T> where T : BaseUserEntity
    {
        [JsonProperty(PropertyName = "access_token")]
        [JsonPropertyName("access_token")]
        [XmlElement(ElementName = "access_token")]
        public string? AccessToken { set; get; }

        [JsonProperty(PropertyName = "token_type")]
        [JsonPropertyName("token_type")]
        [XmlElement(ElementName = "token_type")]
        public string? TokenType { set; get; }

        [JsonProperty(PropertyName = "expires_in")]
        [JsonPropertyName("expires_in")]
        [XmlElement(ElementName = "expires_in")]
        public int ExpiresIn { set; get; } = 3600;

        [JsonProperty(PropertyName = "refresh_token")]
        [JsonPropertyName("refresh_token")]
        [XmlElement(ElementName = "refresh_token")]
        public string? RefreshToken { set; get; }

        [JsonProperty(PropertyName = "scope")]
        [JsonPropertyName("scope")]
        [XmlElement(ElementName = "scope")]
        public string? Scope { set; get; }

        [JsonProperty(PropertyName = "user")]
        [JsonPropertyName("user")]
        [XmlElement(ElementName = "user")]
        public T? User { set; get; }

        /// <summary>
        /// 에러 코드 
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        [JsonPropertyName("code")]
        [XmlElement(ElementName = "code")]
        public virtual int Code { get; set; } = 20000;
    }
}