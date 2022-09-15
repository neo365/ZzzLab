using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ZzzLab.Web.Auth;
using ZzzLab.Web.Configuration;
using ZzzLab.Web.Models;

namespace ZzzLab.Web.Attributes
{
    /// <summary>
    /// 로그인 관련 Attribute
    /// 로그인이 필요한 controller에 [AuthRequired] 표기시 로그인 체크를 진행함.
    /// 로그인은 http Header에 x-authorization을 넣거나
    /// query string에 token을 주면 체크 가능.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [AttributeUsage(validOn: AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class AuthRequiredAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private static IAuthorization? Instance
            => WebBuilder.AuthConfig;

        public AuthRequiredAttribute()
        {
        }

        /// <summary>
        /// Called asynchronously before the action, after model binding is complete.
        /// </summary>
        /// <param name="context">The Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext.</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                if (Instance == null)
                {
                    context.Result = RestResult.BadRequest("IAuthorizationConfiguration이 구성되지 않았습니다.");
                    return;
                }
                else if (Instance.IsAuth(context.HttpContext.Request, out string message) == false)
                {
                    context.Result = RestResult.Unauthorized(message);
                }
            }
            catch (Exception ex)
            {
                context.Result = RestResult.Problem(ex);
            }
        }
    }
}