using Newtonsoft.Json;
using System.Net;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace ZzzLab.Web.Models
{
    public class ResponseBase
    {
        /// <summary>
        /// 상태코드 Http 규약을 따른다.
        /// </summary>
        [JsonProperty(PropertyName = "statusCode")]
        [JsonPropertyName("statusCode")]
        [XmlElement(ElementName = "statusCode")]
        public virtual int StatusCode { get; set; } = (int)HttpStatusCode.OK;

        /// <summary>
        /// 에러 코드 
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        [JsonPropertyName("code")]
        [XmlElement(ElementName = "code")]
        public virtual int Code { get; set; } = 20000;

        /// <summary>
        /// 로그추척 ID
        /// </summary>
        [JsonProperty(PropertyName = "trakingId")]
        [JsonPropertyName("trakingId")]
        [XmlElement(ElementName = "trakingId")]
        public virtual string TrakingId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 연결 서버
        /// </summary>
        [JsonProperty(PropertyName = "host")]
        [JsonPropertyName("host")]
        [XmlElement(ElementName = "host")]
        public virtual string Host => System.Environment.MachineName;

        /// <summary>
        /// Response Time
        /// </summary>
        [JsonProperty(PropertyName = "currentTime")]
        [JsonPropertyName("currentTime")]
        [XmlElement(ElementName = "currentTime")]
        public virtual DateTime CurrentTime { get; } = DateTime.Now;

        internal ResponseBase()
        {
        }

        #region To Convertor

        /// <summary>
        /// 처리 결과값을 json으로 리턴한다.
        /// </summary>
        /// <returns>json string</returns>
        public virtual string ToJson(JsonSerializerSettings? settings = null)
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