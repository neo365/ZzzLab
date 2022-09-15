using Microsoft.AspNetCore.Mvc;
using ZzzLab.AspCore.Models;
using ZzzLab.Data;
using ZzzLab.Web;
using ZzzLab.Web.Controller;
using ZzzLab.Web.Models;

namespace ZzzLab.AspCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : AuthApiControllerBase
    {
        /// <summary>
        /// 서버응답여부를 확인을 위해 구성
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Ping")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestResponse))]
        public IActionResult Ping()
        {
            return RestResult.Ok();
        }

        /// <summary>
        /// 서버응답여부를 확인을 위해 구성
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestResponse))]
        public IActionResult Login()
        {
            try
            {
                if (this.Request.IsAuth() == false) return RestResult.Fail();

                string? userId = this.Request.GetUserId();
                if (string.IsNullOrWhiteSpace(userId)) return RestResult.Fail();
                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {

                    QueryParameterCollection parameters = new QueryParameterCollection
                {
                    { "USER_ID", userId}
                };

                    LoginInfo loginInfo = new LoginInfo().Set(DB.SelectRow(DB.GetQuery("USERS", "GET"), parameters));
                    return RestResult.Ok(loginInfo);
                }
            }
            catch(Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }
    }
}