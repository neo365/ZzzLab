using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace ZzzLab.Web.Models
{
    public class RestResponse : ResponseBase
    {
        /// <summary>
        /// 성공여부
        /// </summary>
        [JsonProperty(PropertyName = "success")]
        [JsonPropertyName("success")]
        [XmlElement(ElementName = "success")]
        public virtual bool Success { set; get; } = false;

        /// <summary>
        /// 전달메세지
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        [JsonPropertyName("message")]
        [XmlElement(ElementName = "message")]
        public virtual string? Message { set; get; }

        #region To Convertor

        /// <summary>
        /// 처리 결과값을 json으로 리턴한다.
        /// </summary>
        /// <returns>json string</returns>
        public override string ToJson(JsonSerializerSettings? settings = null)
            => JsonConvert.SerializeObject(this, settings);

        /// <summary>
        /// 처리 결과값을 json으로 리턴한다.
        /// </summary>
        /// <returns>json string</returns>
        public override string ToString()
            => JsonConvert.SerializeObject(this);

        #endregion To Convertor
    }
}