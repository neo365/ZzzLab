using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Claims;
using ZzzLab.Crypt;
using ZzzLab.Data;
using ZzzLab.Web;
using ZzzLab.Web.Attributes;
using ZzzLab.Web.Models;

namespace ZzzLab.AspCore.Controllers
{
    public partial class AuthController
    {
        /// <summary>
        /// OTA를 생성한다.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Slo")]
        //[AuthRequired]
        [Authorize]
        public IActionResult MakeOtaId()
            => MakeOtaId(this.Request.GetUserId() ?? string.Empty);

        /// <summary>
        /// OTA를 생성한다.
        /// </summary>
        /// <returns></returns>
        [AuthRequired]
        [HttpGet]
        [Route("Slo/{userId}")]
        public IActionResult MakeOtaId([Required(ErrorMessage = "UserId는 필수값입니다."), StringLength(50, MinimumLength = 3)] string userId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                // or
                var userid = identity?.FindFirst("userId")?.Value;

            }

            if (string.IsNullOrWhiteSpace(userId)) return RestResult.BadRequest();

            try
            {
                string clientKey = Configurator.Get("CLIENT_ID");
                return RestResult.OkOrFail(MakeOtaKey(userId, clientKey));
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        private static string? MakeOtaKey(string userId, string clientKey)
        {
            try
            {
                string tempKey = BouncyCastleCrypt.Encrypt(userId, clientKey);
                string otakey = BouncyCastleCrypt.Encrypt($"{DateTime.Now:yyyyMMddHHmmssfff}{tempKey}", tempKey);

                QueryParameterCollection parameters = new QueryParameterCollection
                {
                    { "OTA_KEY", otakey},
                    { "USER_ID", userId},
                    { "CLIENT_KEY", clientKey}
                };

                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    DB.Excute(DB.GetQuery("OTA", "INSERTED"), parameters);
                }

                return otakey;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return null;
        }

        private static string? GetOtaKey(string? code, string clientKey)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));
            if (string.IsNullOrWhiteSpace(clientKey)) throw new ArgumentNullException(nameof(clientKey));

            try
            {
                QueryParameterCollection parameters = new QueryParameterCollection
                {
                    { "OTA_KEY", code},
                };

                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    DataRow row = DB.SelectRow(DB.GetQuery("OTA", "AUTH"), parameters);

                    if (row == null) return null;

                    string userId = row.ToString("user_id");
                    string seed = BouncyCastleCrypt.Encrypt(userId, clientKey);
                    string useridKey = BouncyCastleCrypt.Decrypt(code, seed).Substring("yyyyMMddHHmmssfff".Length);

                    return useridKey;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return null;
        }
    }
}