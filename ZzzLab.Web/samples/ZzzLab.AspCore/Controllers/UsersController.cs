using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;
using ZzzLab.AspCore.Models;
using ZzzLab.Data;
using ZzzLab.Models.Auth;
using ZzzLab.Web.Controller;
using ZzzLab.Web.Models;

namespace ZzzLab.AspCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : AuthApiControllerBase
    {
        /// <summary>
        /// 사용자 목록을 가져온다
        /// </summary>
        /// <param name="pageNum">페이지번호</param>
        /// <param name="pageRow">한번에 가져올 수</param>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestGridResponse<UserEntity>))]
        public IActionResult GetList(uint pageNum = 1, uint pageRow = 20)
            => GetList(new GridRequest
            {
                PageNum = pageNum,
                PageRow = pageRow
            });

        /// <summary>
        ///  사용자 목록을 가져온다
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestGridResponse<UserEntity>))]
        public IActionResult GetList(GridRequest req)
        {
            try
            {
                uint offset = ((req.PageNum > 0 ? req.PageNum : 1) - 1) * req.PageRow;

                string pagingSql = (req.PageRow == 0 ? "" : $" OFFSET {offset} LIMIT {req.PageRow}");

                string whereSql = "";
                string orderSql = "";

                if (req.Columns != null && req.Columns.Any())
                {
                    foreach (var column in req.Columns)
                    {
                        // SQL injection
                        if (string.IsNullOrWhiteSpace(column.FieldName) == false
                            && column.FieldName.CheckInjection())
                        {
                            return RestResult.BadRequest("불법접인 접근이 감지 되었습니다.");
                        }

                        if (string.IsNullOrWhiteSpace(column.Search) == false
                            && column.Search.CheckInjection())
                        {
                            return RestResult.BadRequest("불법접인 접근이 감지 되었습니다.");
                        }

                        if (string.IsNullOrWhiteSpace(column.Search) == false)
                        {
                            if (column.Search.Contains('*'))
                            {
                                whereSql += $" AND {column.FieldName} like '{column.Search.Replace('*', '%')}'";
                            }
                            else whereSql += $" AND {column.FieldName} = '{column.Search}'";
                        }

                        if (column.OrderBy == OrderBy.Ascending) orderSql += $" , {column.FieldName} ASC";
                        if (column.OrderBy == OrderBy.Descending) orderSql += $" , {column.FieldName} DESC";
                    }

                    if (string.IsNullOrWhiteSpace(orderSql) == false)
                    {
                        orderSql = $"ORDER BY {orderSql.TrimStart(',', ' ')}";
                    }
                }

                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    string sql = $"{DB.GetQuery("USERS", "LIST", whereSql, orderSql).TrimEnd(';', ' ')} {pagingSql}";

                    int recordsTotal = DB.SelectValue(DB.GetQuery("USERS", "LIST_RECORDS_TOTAL")).ToInt();
                    int recordsFiltered = DB.SelectValue(DB.GetQuery("USERS", "LIST_RECORDS_FILTERED", whereSql)).ToInt();

                    DataTable table = DB.Select(sql);

                    if (table == null || table.Rows.Count == 0) return RestResult.Grid(Enumerable.Empty<LoginInfo>(), recordsTotal, recordsFiltered);

                    List<UserEntity> list = new List<UserEntity>();
                    foreach (DataRow row in table.Rows)
                    {
                        list.Add(new LoginInfo().Set(row));
                    }

                    return RestResult.Grid(list, recordsTotal, recordsFiltered);
                }
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        /// <summary>
        /// 지정된 사용자의 정보를 가져온다.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestItemResponse<UserEntity>))]
        public IActionResult GetItem([Required(ErrorMessage = "UserId는 필수값입니다."), StringLength(50, MinimumLength = 3)] string userId)
        {
            try
            {
                QueryParameterCollection parameters = new QueryParameterCollection
                {
                    { "USER_ID", userId}
                };

                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    DataRow row = DB.SelectRow(DB.GetQuery("USERS", "GET"), parameters);
                    if (row == null) return RestResult.Fail();
                    return RestResult.Ok((UserEntity)new LoginInfo().Set(row));
                }
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        /// <summary>
        /// 사용자를 추가한다.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public IActionResult Insert(UserEntity req)
        {
            try
            {
                QueryParameterCollection parameters = new QueryParameterCollection
                {
                    { "USER_ID", req.UserId },
                    { "USER_NAME", req.UserName },
                    { "NICK_NAME", req.NickName ?? req.UserName },
                    { "COMPANY_ID", req.CompanyCode },
                    { "DEPT_ID", req.DeptCode },
                    { "EMAIL", req.Email },
                    { "MOBILE", req.Mobile },
                    { "AUTH_ROLE", req.AuthRole },
                    { "LOGIN_YN", req.IsLogin.ToYN() },
                    { "USED_YN", req.IsUsed.ToYN() },
                    { "MEMO", req.Memo },
                    { "WHEN_EXPIRED", req.WhenExpired?.ToString("yyyy-MM-dd")?? "9999-12-31"}
                };

                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    return RestResult.OkOrFail(DB.Excute(DB.GetQuery("USERS", "INSERT"), parameters));
                }
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        /// <summary>
        /// 사용자를 수정한다.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{userId}")]
        public IActionResult Modify([Required(ErrorMessage = "UserId는 필수값입니다."), StringLength(50, MinimumLength = 3)] string userId, UserEntity req)
        {
            if (userId.EqualsIgnoreCase(req.UserId) == false) return RestResult.BadRequest();
            try
            {
                QueryParameterCollection parameters = new QueryParameterCollection
                {
                    { "USER_ID", req.UserId },
                    { "USER_NAME", req.UserName },
                    { "NICK_NAME", req.NickName ?? req.UserName },
                    { "COMPANY_ID", req.CompanyCode },
                    { "DEPT_ID", req.DeptCode },
                    { "EMAIL", req.Email },
                    { "MOBILE", req.Mobile },
                    { "AUTH_ROLE", req.AuthRole },
                    { "LOGIN_YN", req.IsLogin.ToYN() },
                    { "USED_YN", req.IsUsed.ToYN() },
                    { "MEMO", req.Memo },
                    { "WHEN_EXPIRED", req.WhenExpired?.ToString("yyyy-MM-dd")?? "9999-12-31"}
                };

                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    return RestResult.OkOrFail(DB.Excute(DB.GetQuery("USERS", "UPDATE"), parameters));
                }
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        /// <summary>
        /// 사용자를 삭제한다.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{userId}")]
        public IActionResult Delete([Required(ErrorMessage = "UserId는 필수값입니다."), StringLength(50, MinimumLength = 3)] string userId)
        {
            try
            {
                QueryParameterCollection parameters = new QueryParameterCollection
                {
                    { "USER_ID", userId}
                };

                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    return RestResult.OkOrFail(DB.Excute(DB.GetQuery("USERS", "DELETE"), parameters));
                }
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }
    }
}