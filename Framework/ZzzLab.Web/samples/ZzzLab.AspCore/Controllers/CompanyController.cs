using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;
using ZzzLab.AspCore.Models;
using ZzzLab.Data;
using ZzzLab.Web.Controller;
using ZzzLab.Web.Models;

namespace ZzzLab.AspCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CompanyController : ApiControllerBase
    {
        /// <summary>
        /// 회사 목록을 가져온다
        /// </summary>
        /// <param name="pageNum">페이지번호</param>
        /// <param name="pageRow">한번에 가져올 수</param>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestGridResponse<CompanyEntity>))]
        
        public IActionResult GetList(uint pageNum = 1, uint pageRow = 20)
            => GetList(new GridRequest
            {
                PageNum = pageNum,
                PageRow = pageRow
            });

        /// <summary>
        ///  회사 목록을 가져온다
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestGridResponse<CompanyEntity>))]
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
                    string sql = $"{DB.GetQuery("COMPANY", "LIST", whereSql, orderSql).TrimEnd(';', ' ')} {pagingSql}";

                    int recordsTotal = DB.SelectValue(DB.GetQuery("COMPANY", "LIST_RECORDS_TOTAL")).ToInt();
                    int recordsFiltered = DB.SelectValue(DB.GetQuery("COMPANY", "LIST_RECORDS_FILTERED", whereSql)).ToInt();

                    DataTable table = DB.Select(sql);

                    if (table == null || table.Rows.Count == 0) return RestResult.Grid(Enumerable.Empty<LoginInfo>(), recordsTotal, recordsFiltered);

                    List<CompanyEntity> list = new List<CompanyEntity>();
                    foreach (DataRow row in table.Rows)
                    {
                        list.Add(new CompanyEntity().Set(row));
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
        /// 지정된 회사의 정보를 가져온다.
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{companyId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestItemResponse<CompanyEntity>))]
        public IActionResult GetItem([Required(ErrorMessage = "companyId는 필수값입니다."), StringLength(36, MinimumLength = 36)] string companyId)
        {
            try
            {
                QueryParameterCollection parameters = new QueryParameterCollection
                {
                    { "COMPANY_ID", companyId}
                };

                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    DataRow row = DB.SelectRow(DB.GetQuery("COMPANY", "GET"), parameters);
                    if (row == null) return RestResult.Fail();
                    return RestResult.Ok(new CompanyEntity().Set(row));
                }
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        /// <summary>
        /// 회사를 추가한다.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public IActionResult Insert(CompanyEntity req)
        {
            try
            {
                QueryParameterCollection parameters = new QueryParameterCollection
                {
                    { "COMPANY_ID", req.CompanyId },
                    { "COMPANY_NAME", req.CompanyName },
                    { "PARENT_ID", req.ParentId },
                    { "USED_YN", req.IsUsed.ToYN() },
                    { "MEMO", req.Memo }
                };

                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    return RestResult.OkOrFail(DB.Excute(DB.GetQuery("COMPANY", "INSERT"), parameters));
                }
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        /// <summary>
        /// 회사를 수정한다.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="req">CompanyEntity</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{companyId}")]
        public IActionResult Modify([Required(ErrorMessage = "companyId는 필수값입니다."), StringLength(36, MinimumLength = 36)] string companyId, CompanyEntity req)
        {
            if (companyId.EqualsIgnoreCase(req.CompanyId) == false) return RestResult.BadRequest();
            try
            {
                QueryParameterCollection parameters = new QueryParameterCollection
                {
                    { "COMPANY_ID", req.CompanyId },
                    { "COMPANY_NAME", req.CompanyName },
                    { "PARENT_ID", req.ParentId },
                    { "USED_YN", req.IsUsed.ToYN() },
                    { "MEMO", req.Memo }
                };

                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    return RestResult.OkOrFail(DB.Excute(DB.GetQuery("COMPANY", "UPDATE"), parameters));
                }
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        /// <summary>
        /// 회사를 삭제한다.
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{companyId}")]
        public IActionResult Delete([Required(ErrorMessage = "companyId는 필수값입니다."), StringLength(36, MinimumLength = 36)] string companyId)
        {
            try
            {
                QueryParameterCollection parameters = new QueryParameterCollection
                {
                    { "COMPANY_ID", companyId }
                };

                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    return RestResult.OkOrFail(DB.Excute(DB.GetQuery("COMPANY", "DELETE"), parameters));
                }
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }
    }
}