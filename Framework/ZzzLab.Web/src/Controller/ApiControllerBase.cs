using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using ZzzLab.Web.Models;

namespace ZzzLab.Web.Controller
{
    [Produces("application/json")]
    //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestItemResponse<object>))]
    //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(RestErrorResult))]
    //[ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(RestErrorResult))]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(RestErrorResult))]
    public abstract class ApiControllerBase : ControllerBase
    {
        // 한번 가져온 파라미터는 저장해놓고 쓰자.
        // 같은 일을 두번 하지는 말자.
        private IDictionary<string, object?>? _CashedParameters;

        private dynamic? _CashedRequest;

        /// <summary>
        /// Form, querystring 을 가져온다.
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public virtual dynamic GetRequest()
        {
            if (_CashedRequest != null) return _CashedRequest;

            _CashedParameters = this.Request.GetParameters();
            _CashedRequest = _CashedParameters.Aggregate(new ExpandoObject() as IDictionary<string, object?>,
                            (a, p) => { a.Add(p); return a; });

            return _CashedRequest;
        }

        /// <summary>
        /// Form, querystring 을 가져온다.
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public virtual IDictionary<string, object?> GetParameters()
        {
            if (_CashedParameters != null) return _CashedParameters;

            _CashedParameters = this.Request.GetParameters();
            _CashedRequest = _CashedParameters.Aggregate(new ExpandoObject() as IDictionary<string, object?>,
                (a, p) => { a.Add(p); return a; });

            return _CashedParameters;
        }

        /// <summary>
        /// Query string에서 값을 가져온다.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [NonAction]
        public virtual string? GetQuery(string key)
            => this.Request.GetQueryString(key);

        /// <summary>
        /// Form에서 값을 가져온다.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [NonAction]
        public virtual string? GetForm(string key)
            => this.Request.GetFormParameter(key);

        /// <summary>
        /// 쿠키값을 가져온다.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [NonAction]
        public virtual string? GetCookie(string name)
            => this.Request.GetCookie(name);

        /// <summary>
        /// 헤더 값을 가져온다.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [NonAction]
        public virtual string? GetHeader(string name)
            => this.Request.GetHeader(name);

        /// <summary>
        /// User-Agent 값을 가져온다.
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public virtual string? GetUserAgent()
            => this.Request.GetHeader("User-Agent");

        /// <summary>
        /// Referer 값을 가져온다.
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public virtual string? GetReferer()
            => this.Request.GetHeader("Referer");

        /// <summary>
        /// 접속 IP를 가져온다.
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public virtual string? GetIp()
            => this.Request.GetIp();

        /// <summary>
        /// GUID를 가져온다.
        /// </summary>
        /// <returns></returns>
        [NonAction]
        protected virtual string GetGuid()
            => Guid.NewGuid().ToString();
    }
}