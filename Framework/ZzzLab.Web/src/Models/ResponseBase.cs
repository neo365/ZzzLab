using Newtonsoft.Json;
using System.Net;

namespace ZzzLab.Web.Models
{
    public class ResponseBase
    {
        /// <summary>
        /// 상태코드 Http 규약을 따른다.
        /// </summary>
        [JsonProperty(PropertyName = "statusCode")]
        public virtual int StatusCode { get; set; } = (int)HttpStatusCode.OK;

        /// <summary>
        /// 로그추척 ID
        /// </summary>
        [JsonProperty(PropertyName = "trakingId")]
        public virtual string TrakingId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 연결 서버
        /// </summary>
        [JsonProperty(PropertyName = "host")]
        public virtual string Host => System.Environment.MachineName;

        /// <summary>
        /// Response Time
        /// </summary>
        [JsonProperty(PropertyName = "currentTime")]
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