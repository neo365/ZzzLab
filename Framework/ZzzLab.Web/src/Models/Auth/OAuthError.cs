using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace ZzzLab.Models.Auth
{
    public class OAuthError
    {
        [JsonProperty(PropertyName = "error")]
        [JsonPropertyName("error")]
        [XmlElement(ElementName = "error")]
        public string? Error { set; get; }

        [JsonProperty(PropertyName = "error_description")]
        [JsonPropertyName("error")]
        [XmlElement(ElementName = "error")]
        public string? ErrorDescription { set; get; }

        /// <summary>
        /// 에러 코드 
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        [JsonPropertyName("code")]
        [XmlElement(ElementName = "code")]
        public virtual int Code { get; set; } = 20000;
    }
}