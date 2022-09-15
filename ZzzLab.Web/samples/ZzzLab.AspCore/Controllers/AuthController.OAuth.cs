using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZzzLab.AspCore.Models;
using ZzzLab.Crypt;
using ZzzLab.Data;
using ZzzLab.Models.Auth;
using ZzzLab.Web;
using ZzzLab.Web.Auth;
using ZzzLab.Web.Models;
using ZzzLab.Web.Models.Auth;

namespace ZzzLab.AspCore.Controllers
{
    public partial class AuthController
    {
        private const string _Header = "yyyyMMddHHmmss";

        [HttpPost]
        [Route("Signin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("application/x-www-form-urlencoded")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OAuthSuccess<LoginInfo>))]
        public IActionResult SigninFromForm([FromForm] LoginRequest req)
        {
            OAuthRequest oauth = new OAuthRequest
            {
                GrantType = GrantType.password,
                UserName = req.UserId,
                Password = req.Password,
                ClientId = req.ClientId ?? Configurator.Get("CLIENT_ID")
            };

            return Authorize(oauth);
        }

        [HttpPost]
        [Route("Signin")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OAuthSuccess<LoginInfo>))]
        public IActionResult SigninFromBody([FromBody] LoginRequest req)
        {
            OAuthRequest oauth = new OAuthRequest
            {
                GrantType = GrantType.password,
                UserName = req.UserId,
                Password = req.Password,
                ClientId = req.ClientId ?? Configurator.Get("CLIENT_ID")
            };

            return Authorize(oauth);
        }

        [HttpGet]
        [Route("Signin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OAuthSuccess<LoginInfo>))]
        public IActionResult Signin([Required(ErrorMessage = "Code는 필수값입니다."), StringLength(1000, MinimumLength = 5)] string code)
        {
            OAuthRequest oauth = new OAuthRequest
            {
                GrantType = GrantType.authorization_code,
                Code = code,
                ClientId = Configurator.Get("CLIENT_ID")
            };

            return Authorize(oauth);
        }

        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/Api/Auth/Authorize")]
        [Consumes("application/x-www-form-urlencoded")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OAuthSuccess<LoginInfo>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(OAuthError))]
        public IActionResult Authorize()
        {
            OAuthRequest req = new OAuthRequest
            {
                GrantType = this.GetForm("grant_type").ToEnum<GrantType>(),
                UserName = this.GetForm("username"),
                Password = this.GetForm("password"),
                Code = this.GetForm("code"),
                RefreshToken = this.GetForm("refresh_token"),
                Scope = this.GetForm("scope"),
                ClientId = this.GetForm("Client_Id")
            };

            return Authorize(req);
        }

        [HttpPost]
        [Route("/Api/Auth/Authorize")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OAuthSuccess<LoginInfo>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(OAuthError))]
        public IActionResult Authorize([FromBody] OAuthRequest req)
        {
            try
            {
                if (req == null) return RestResult.BadRequest();
                //if (this.Request.IsAuth()) return RestResult.Redirect("/Logout");

                QueryParameterCollection parameters = new QueryParameterCollection
                {
                    { "UUID", this.GetGuid()},
                    { "LOGIN_IP", this.GetIp()},
                    { "USER_AGENT",this.GetUserAgent()},
                    { "MEMO", null },
                };

                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    req.ClientId ??= Configurator.Get("CLIENT_ID");

                    switch (req.GrantType)
                    {
                        case GrantType.password:
                            if (string.IsNullOrWhiteSpace(req.UserName) || string.IsNullOrWhiteSpace(req.Password)) return RestResult.BadRequest();

                            parameters.Set("USER_ID", req.UserName);

                            DataRow passowrdRow = DB.SelectRow(DB.GetQuery("LOGIN", "GET_LOGIN"), parameters);
                            if (passowrdRow == null) return RestResult.Unauthorized("존재하지 않는 아이디 입니다.");

                            string userPassword = passowrdRow.ToStringNullable("user_pwd");
                            string initPassword = HMACSHA512Crypt.Encrypt(req.UserName, req.Password);

                            if (string.IsNullOrWhiteSpace(userPassword))
                            {
                                // 최초 로그인시 현재 입력한 비밀번호로 셋팅함.
                                parameters.Set("USER_PWD", initPassword);
                                DB.Excute(DB.GetQuery("LOGIN", "UPDATE_PASSWORD"), parameters);
                            }
                            else if (initPassword.EqualsIgnoreCase(userPassword) == false)
                            {
                                return RestResult.Unauthorized("비밀번호가 틀립니다.");
                            }
                            break;

                        case GrantType.authorization_code:
                            if (string.IsNullOrWhiteSpace(req.Code) || string.IsNullOrWhiteSpace(req.ClientId)) return RestResult.BadRequest();
                            string? ota = GetOtaKey(req.Code, Configurator.Get("CLIENT_ID"));
                            if (string.IsNullOrWhiteSpace(ota)) return RestResult.Unauthorized();
                            req.UserName = BouncyCastleCrypt.Decrypt(ota, req.ClientId);
                            if (string.IsNullOrWhiteSpace(req.UserName)) return RestResult.Unauthorized();
                            break;

                        case GrantType.refresh_token:
                            req.ClientId ??= Configurator.Get("CLIENT_ID");

                            if (string.IsNullOrWhiteSpace(req.RefreshToken) || string.IsNullOrWhiteSpace(req.ClientId)) return RestResult.BadRequest();

                            if (BouncyCastleCrypt.Decrypt(req.RefreshToken, req.ClientId).Substring(_Header.Length).Equals(this.GetIp()) == false)
                            {
                                return RestResult.Unauthorized("존재하지 않는 아이디 입니다.");
                            }

                            parameters.Set("REFRESH_TOKEN", req.RefreshToken);

                            DataRow refreshRow = DB.SelectRow(DB.GetQuery("LOGIN", "GET_FROM_REFRESH_TOKEN"), parameters);
                            if (refreshRow == null) return RestResult.Unauthorized("존재하지 않는 아이디 입니다.");
                            req.UserName = refreshRow.ToString("user_id");

                            DB.Excute(DB.GetQuery("LOGIN", "SIGNOUT"), parameters);
                            parameters.Set("MEMO", $"Refresh : {refreshRow.ToString("uuid")}");
                            break;

                        default: return RestResult.BadRequest();
                    }

                    string refreshToken = BouncyCastleCrypt.Encrypt($"{DateTime.Now.ToString(_Header)}{this.GetIp()}", req.ClientId ?? Configurator.Get("CLIENT_ID"));
                    parameters.Set("USER_ID", req.UserName);
                    parameters.Set("REFRESH_TOKEN", refreshToken);
                    parameters.Set("LOGIN_TYPE", req.GrantType.ToString());

                    DB.Excute(DB.GetQuery("LOGIN", "SIGNIN"), parameters);

                    LoginInfo loginInfo = new LoginInfo().Set(DB.SelectRow(DB.GetQuery("USERS", "GET"), parameters));

                    if (loginInfo == null) return RestResult.BadRequest();
                    else
                    {
                        if (loginInfo.IsExpired) return RestResult.Unauthorized("사용 만료된 아이디 입니다.");
                        if (loginInfo.LoginEnabled == false) return RestResult.Unauthorized("로그인 불가능한 아이디 입니다.");
                    }

                    LoginUser user = new LoginUser();
                    user.Set(loginInfo);

                    OAuthSuccess<LoginUser> res = new OAuthSuccess<LoginUser>
                    {
                        AccessToken = GetToken(loginInfo),
                        TokenType = "Bearer",
                        ExpiresIn = 3600,
                        RefreshToken = refreshToken,
                        Scope = null,
                        User = user
                    };

                    return RestResult.Auth(res);
                }
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        [NonAction]
        private string GetToken(LoginEntity logininfo)
        {
            //if (logininfo == null) throw new ArgumentNullException(nameof(logininfo));
            //if (string.IsNullOrWhiteSpace(logininfo.Uuid)) return string.Empty;
            //if (string.IsNullOrWhiteSpace(logininfo.UserId)) return string.Empty;
            //if (string.IsNullOrWhiteSpace(logininfo.LoginIP)) return string.Empty;

            //return AuthTokenParser.Encrypt(logininfo.Uuid, logininfo.UserId, logininfo.LoginIP);

            return GenerateJWTToken(logininfo);
        }

        private string GenerateJWTToken(LoginEntity userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configurator.Get("JWT_SECURITY_KEY")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, userInfo.UserName),
                new Claim("userId", userInfo.UserId),
                new Claim(ClaimTypes.Role, "User" /*userInfo.AuthRole*/),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
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