using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Web;
using ZzzLab.Web.Auth;
using ZzzLab.Web.Configuration;

namespace ZzzLab.Web
{
    public static partial class RequestExtension
    {
        /// <summary>
        /// 로그인 여부 체크
        /// </summary>
        /// <param name="request">The Microsoft.AspNetCore.Http.Request</param>
        /// <returns>로그인 여부</returns>
        public static bool IsAuth(this HttpRequest request)
            => IsAuth(request, out _);

        /// <summary>
        /// 로그인 여부 체크
        /// </summary>
        /// <param name="request">요청 Web Request</param>
        /// <param name="message">에러시 상세사항 리턴</param>
        /// <returns>로그인중인지 여부</returns>
        public static bool IsAuth(this HttpRequest request, out string message)
        {
            if (WebBuilder.AuthConfig == null)
            {
                message = "IAuthorizationConfiguration 이 설정되지 않았습니다.";
                return false;
            }

            return WebBuilder.AuthConfig.IsAuth(request, out message);
        }

        /// <summary>
        /// Access Token을 가져온다.
        /// Http Header => Cookie => Request 순으로 가져온다.
        /// </summary>
        /// <param name="request">The Microsoft.AspNetCore.Http.Request</param>
        /// <returns>Access Token</returns>
        public static string? GetAuthkey(this HttpRequest request)
        {
            if (request == null) return null;

            string? value = request.GetHeader("AccessToken");
            if (HasValue(value)) return value;

            value = request.GetHeader("authorization")?.Remove("Bearer ").Trim();
            if (HasValue(value)) return value;

            // 쿠키에 "Bearer"가 붙는 경우가 존재
            value = request.GetCookie("accessToken")?.Remove("Bearer ").Trim();
            if (HasValue(value)) return value;

            // UrlEncode > UrlDecode로 변경 처리. cth 20220215
            value = request.GetRequest("token");
            if (HasValue(value)) return HttpUtility.UrlDecode(value);

            return null;
        }

        /// <summary>
        /// 사용자 id를 가져온다.
        /// Access Token => Http Header => Cookie => Request 순으로 가져온다.
        /// 없을 경우 null을 리턴한다.
        /// </summary>
        /// <param name="request">The Microsoft.AspNetCore.Http.Request</param>
        /// <returns>사용자 아이디</returns>
        public static string? GetUserId(this HttpRequest request)
        {
            if (request == null) return null;

            string? userId = request.HttpContext.User.Claims.FirstOrDefault((Claim c) => c.Type == "userId")?.Value;

            if (string.IsNullOrWhiteSpace(userId) == false) return userId;

            string? value = GetAuthkey(request);
            if (HasValue(value))
            {
                try
                {
                    AuthTokenParser parser = AuthTokenParser.Decrypt(value);
                    if (parser.Success) return parser.UserId;
                }
                catch (Exception ex)
                {
                    Logger.Warning(ex);
                }
            }

            value = request.GetHeader("userId");
            if (HasValue(value)) return value;

            value = request.GetCookie("userId");
            if (HasValue(value)) return value;

            value = request.GetRequest("userId");
            if (HasValue(value)) return value;

            return null;
        }

        private static bool HasValue(string? value)
            => (string.IsNullOrWhiteSpace(value) == false && value.EqualsIgnoreCase("undefined") == false);
    }
}