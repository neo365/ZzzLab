using Microsoft.AspNetCore.Mvc;
using System;
using ZzzLab.Data;
using ZzzLab.Web.Controller;
using ZzzLab.Web.Models;

namespace ZzzLab.Web.Controllers
{
    [ApiController]
    [Route("/Api/[controller]")]
    public class UserController : ApiControllerBase
    {
        [Route("List")]
        [HttpGet]
        public IActionResult GetList()
        {
            try
            {
                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    return RestResult.Grid(DB.Select(DB.GetQuery("USERS", "LIST")));
                }
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        [Route("{userId}")]
        [HttpGet]
        public IActionResult Get(string userId)
        {
            try
            {
                return RestResult.Ok(userId);
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        [Route("User/{userId}")]
        [HttpPut]
        public IActionResult Update(string userId)
        {
            try
            {
                return RestResult.Ok(userId);
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        [Route("User")]
        [HttpPost]
        public IActionResult Add(string userId)
        {
            try
            {
                return RestResult.Ok(userId);
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        [Route("User/{userId}")]
        [HttpDelete]
        public IActionResult Delete(string userId)
        {
            try
            {
                return RestResult.Ok(userId);
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }
    }
}