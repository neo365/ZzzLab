using Newtonsoft.Json;
using System.Net;
using System.Runtime.Serialization;

namespace ZzzLab.Web.Models
{
    [DataContract(Name = "restReponse")]
    public class RestErrorResult : ResponseBase
    {
        /// <summary>
        /// 상태코드 Http 규약을 따른다.
        /// </summary>
        [JsonProperty(PropertyName = "statusCode")]
        public override int StatusCode { get; set; } = (int)HttpStatusCode.InternalServerError;

        /// <summary>
        /// 오류내용
        /// </summary>
        [JsonProperty(PropertyName = "error", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string? Error { set; get; }

        /// <summary>
        /// 오류설명
        /// </summary>
        [JsonProperty(PropertyName = "errorDescription", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string? ErrorDescription { set; get; }

        /// <summary>
        /// 오류 참조 URL
        /// </summary>
        [JsonProperty(PropertyName = "errorUri", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string? ErrorUri { set; get; }

        /// <summary>
        /// 오류코드
        /// </summary>
        [JsonProperty(PropertyName = "errorCode", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string? ErrorCode { set; get; }

        #region ToString

        /// <summary>
        /// 처리 결과값을 json으로 리턴한다.
        /// </summary>
        /// <returns>json string</returns>
        public override string ToString()
            => JsonConvert.SerializeObject(this);

        #endregion ToString
    }
}