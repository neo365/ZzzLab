using Newtonsoft.Json;

namespace ZzzLab.Web.Models
{
    public class RestServerErrorResult : RestErrorResult
    {
        /// <summary>
        /// 호출계층구조
        /// </summary>
        [JsonProperty(PropertyName = "stackTrace", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string? StackTrace { set; get; } = null;
    }
}