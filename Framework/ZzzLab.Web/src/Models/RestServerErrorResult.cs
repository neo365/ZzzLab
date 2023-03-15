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
    }
}