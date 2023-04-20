using Newtonsoft.Json;
using System.Text.Json.Serialization;
using ZzzLab.ExceptionEx;

namespace ZzzLab.Web.Models
{
    public class RestServerErrorResult : RestErrorResult
    {
        /// <summary>
        /// 오류명세
        /// </summary>
        [JsonPropertyName("error")]
        [JsonProperty(PropertyName = "error", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ExceptionInfo> Error { set; get; } = Enumerable.Empty<ExceptionInfo>();

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