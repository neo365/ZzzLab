using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZzzLab.AspCore.Models;
using ZzzLab.Data;
using ZzzLab.Web;
using ZzzLab.Web.Auth;

namespace ZzzLab.AspCore.Configuration
{
    public class UserAuthConfiguration : IUserRepository<LoginInfo>, IAuthorization
    {
        #region IAuthorizationConfiguration

        /// <summary>
        /// 로그인 여부를 가져온다.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual bool IsAuth(HttpRequest request, out string message)
        {
            string? token = request.GetAuthkey();
            string? userId = request.GetUserId();
            string? clientIp = request.GetIp();
            string? useragent = request.GetUserAgent();
            message = "";

            // Token 처리
            if (string.IsNullOrWhiteSpace(token))
            {
                message = "token 값이 없습니다.";
                return false;
            }

            AuthTokenParser parser = AuthTokenParser.Decrypt(token);
            if (parser.Success == false)
            {
                message = parser.Message ?? string.Empty;
                return false;
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                message = "userid 값이 없습니다.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(clientIp))
            {
                message = "ip 값이 없습니다.";
                return false;
            }

            if (userId.Equals(parser.UserId) == false)
            {
                message = "userId가 다릅니다.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(useragent))
            {
                message = "불법적인 접근이 감지 되었습니다.";
                return false;
            }

            try
            {
                QueryParameterCollection parameters = new QueryParameterCollection
            {
                { "UUID", parser.Uuid },
                { "USER_ID", parser.UserId },
                { "expired", Configurator.Get("SESSION_TIMEOUT")?.ToInt() ?? 20},
                { "WATCH_URL", $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}"},
            };

                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    if (DB.SelectValue(DB.GetQuery("LOGIN", "IS_EXPIRED"), parameters)?.ToBooleanNullable() ?? false)
                    {
                        message = "세션이 만료 되었습니다.";
                        return false;
                    }

                    DB.Excute(DB.GetQuery("LOGIN", "SESSION_EXPIRED"), parameters);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual bool IsAuth(HttpRequest request)
            => IsAuth(request, out _);

        #endregion IAuthorizationConfiguration

        public bool ValidateCredentials(string? userId, string? password)
        {
            return false;
        }

        /// <summary>
        /// 사용자 정보를 가져온다.
        /// </summary>
        /// <param name="userId">사용자 아이디</param>
        /// <returns>사용자 정보</returns>
        public LoginInfo? FindByUserId(string? userId)
        {
            return default;
        }

        /// <summary>
        /// 사용자 정보를 가져온다.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="loginInfo"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool TryFindByUserId(string? userId, out LoginInfo? loginInfo, out string? message)
        {
            loginInfo = default;
            message = null;

            try
            {
                loginInfo = FindByUserId(userId);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 인증토큰을 생성한다.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public string GenerateToken(LoginInfo login)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configurator.Get("JWT_SECURITY_KEY")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
            var claims = new[]
            {
                new Claim("Name", login.UserName),
                new Claim("UserId", login.UserId),
                new Claim(ClaimTypes.Role, login.AuthRole),
                new Claim(JwtRegisteredClaimNames.Jti, login.Uuid),
            };

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: Configurator.Get("JWT_ISSUER"),
                audience: Configurator.Get("JWT_AUDIENCE"),
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}