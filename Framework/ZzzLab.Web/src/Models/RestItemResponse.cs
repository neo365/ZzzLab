using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace ZzzLab.Web.Models
{
    public class RestItemResponse<T> : RestResponse
    {
        /// <summary>
        /// Item
        /// </summary>
        [JsonProperty(PropertyName = "item")]
        [JsonPropertyName("item")]
        [XmlElement(ElementName = "item")]
        public virtual T? Item { set; get; }

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