using Newtonsoft.Json;
using System.Net;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using ZzzLab.ExceptionEx;

namespace ZzzLab.Web.Models
{
    [DataContract(Name = "restReponse")]
    public class RestErrorResult : ResponseBase
    {
        /// <summary>
        /// 상태코드 Http 규약을 따른다.
        /// </summary>
        [JsonPropertyName("statusCode")]
        [JsonProperty(PropertyName = "statusCode")]
        [XmlElement(ElementName = "statusCode")]
        public override int StatusCode { get; set; } = (int)HttpStatusCode.InternalServerError;

        /// <summary>
        /// 오류메세지
        /// </summary>
        [JsonPropertyName("errorMessage")]
        [JsonProperty(PropertyName = "errorMessage", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "errorMessage")]
        public virtual string? ErrorMessage { set; get; }

        /// <summary>
        /// 오류설명
        /// </summary>
        [JsonPropertyName("errorDescription")]
        [JsonProperty(PropertyName = "errorDescription", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "errorDescription")]
        public virtual string? ErrorDescription { set; get; }

        /// <summary>
        /// 오류 참조 URL
        /// </summary>
        [JsonPropertyName("errorUri")]
        [JsonProperty(PropertyName = "errorUri", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "errorUri")]
        public virtual string? ErrorUri { set; get; }

        /// <summary>
        /// 오류코드
        /// </summary>
        [JsonPropertyName("errorCode")]
        [JsonProperty(PropertyName = "errorCode", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "errorCode")]
        public virtual string? ErrorCode { set; get; }

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