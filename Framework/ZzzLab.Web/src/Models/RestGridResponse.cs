using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace ZzzLab.Web.Models
{
    public class RestGridResponse<T> : RestResponse
    {
        /// <summary>
        /// Grid Item
        /// </summary>
        [JsonProperty(PropertyName = "headers")]
        [JsonPropertyName("headers")]
        [XmlElement(ElementName = "headers")]
        public virtual IEnumerable<object>? Headers { set; get; }

        /// <summary>
        /// Grid Item
        /// </summary>
        [JsonProperty(PropertyName = "items")]
        [JsonPropertyName("items")]
        [XmlElement(ElementName = "items")]
        public virtual IEnumerable<T>? Items { set; get; }

        /// <summary>
        /// 이전버젼 호환성 유지를 위해
        /// </summary>
        [Obsolete]
        [JsonProperty(PropertyName = "data")]
        [JsonPropertyName("data")]
        [XmlElement(ElementName = "data")]
        public virtual IEnumerable<T>? Data => Items;

        /// <summary>
        /// RecordsTotal
        /// </summary>
        [JsonProperty(PropertyName = "recordsTotal")]
        [JsonPropertyName("recordsTotal")]
        [XmlElement(ElementName = "recordsTotal")]
        public virtual int RecordsTotal { set; get; } = 0;

        /// <summary>
        /// RecordsFiltered
        /// </summary>
        [JsonProperty(PropertyName = "recordsFiltered")]
        [JsonPropertyName("recordsFiltered")]
        [XmlElement(ElementName = "recordsFiltered")]
        public virtual int RecordsFiltered { set; get; } = 0;

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