using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZzzLab.Web.Models;

namespace ZzzLab.Web.Controller
{
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(RestErrorResult))]
    [Authorize]
    public abstract partial class AuthApiControllerBase : ApiControllerBaseEx
    {
        /// <summary>
        /// 인증키를 가져온다
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public virtual string? GetAuthKey()
            => this.Request.GetAuthkey();
    }
}