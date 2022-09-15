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
    public class DeptController : AuthApiControllerBase
    {
        /// <summary>
        /// �μ� ����� �����´�
        /// </summary>
        /// <param name="pageNum">��������ȣ</param>
        /// <param name="pageRow">�ѹ��� ������ ��</param>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestGridResponse<DeptEntity>))]
        public IActionResult GetList(uint pageNum = 1, uint pageRow = 20)
            => GetList(new GridRequest
            {
                PageNum = pageNum,
                PageRow = pageRow
            });

        /// <summary>
        ///  �μ� ����� �����´�
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestGridResponse<DeptEntity>))]
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
                            return RestResult.BadRequest("�ҹ����� ������ ���� �Ǿ����ϴ�.");
                        }

                        if (string.IsNullOrWhiteSpace(column.Search) == false
                            && column.Search.CheckInjection())
                        {
                            return RestResult.BadRequest("�ҹ����� ������ ���� �Ǿ����ϴ�.");
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
                    string sql = $"{DB.GetQuery("DEPT", "LIST", whereSql, orderSql).TrimEnd(';', ' ')} {pagingSql}";

                    int recordsTotal = DB.SelectValue(DB.GetQuery("DEPT", "LIST_RECORDS_TOTAL")).ToInt();
                    int recordsFiltered = DB.SelectValue(DB.GetQuery("DEPT", "LIST_RECORDS_FILTERED", whereSql)).ToInt();

                    DataTable table = DB.Select(sql);

                    if (table == null || table.Rows.Count == 0) return RestResult.Grid(Enumerable.Empty<LoginInfo>(), recordsTotal, recordsFiltered);

                    List<DeptEntity> list = new List<DeptEntity>();
                    foreach (DataRow row in table.Rows)
                    {
                        list.Add(new DeptEntity().Set(row));
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
        /// ������ �μ��� ������ �����´�.
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{deptId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestItemResponse<DeptEntity>))]
        public IActionResult GetItem([Required(ErrorMessage = "deptId�� �ʼ����Դϴ�."), StringLength(36, MinimumLength = 36)] string deptId)
        {
            try
            {
                QueryParameterCollection parameters = new QueryParameterCollection
                {
                    { "DEPT_ID", deptId}
                };

                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    DataRow row = DB.SelectRow(DB.GetQuery("DEPT", "GET"), parameters);
                    if (row == null) return RestResult.Fail();
                    return RestResult.Ok(new DeptEntity().Set(row));
                }
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        /// <summary>
        /// �μ��� �߰��Ѵ�.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public IActionResult Insert(DeptEntity req)
        {
            try
            {
                QueryParameterCollection parameters = new QueryParameterCollection
                {
                    { "DEPT_ID", req.DeptId },
                    { "COMPANY_ID", req.DeptId },
                    { "DEPT_NAME", req.DeptName },
                    { "PARENT_ID", req.ParentId },
                    { "USED_YN", req.IsUsed.ToYN() },
                    { "MEMO", req.Memo }
                };

                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    return RestResult.OkOrFail(DB.Excute(DB.GetQuery("DEPT", "INSERT"), parameters));
                }
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        /// <summary>
        /// �μ��� �����Ѵ�.
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="req">DeptEntity</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{deptId}")]
        public IActionResult Modify([Required(ErrorMessage = "deptId�� �ʼ����Դϴ�."), StringLength(36, MinimumLength = 36)] string deptId, DeptEntity req)
        {
            if (deptId.EqualsIgnoreCase(req.DeptId) == false) return RestResult.BadRequest();
            try
            {
                QueryParameterCollection parameters = new QueryParameterCollection
                {
                    { "DEPT_ID", req.DeptId },
                    { "COMPANY_ID", req.DeptId },
                    { "DEPT_NAME", req.DeptName },
                    { "PARENT_ID", req.ParentId },
                    { "USED_YN", req.IsUsed.ToYN() },
                    { "MEMO", req.Memo }
                };

                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    return RestResult.OkOrFail(DB.Excute(DB.GetQuery("DEPT", "UPDATE"), parameters));
                }
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        /// <summary>
        /// �μ��� �����Ѵ�.
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{deptId}")]
        public IActionResult Delete([Required(ErrorMessage = "deptId�� �ʼ����Դϴ�."), StringLength(36, MinimumLength = 36)] string deptId)
        {
            try
            {
                QueryParameterCollection parameters = new QueryParameterCollection
                {
                    { "DEPT_ID", deptId }
                };

                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    return RestResult.OkOrFail(DB.Excute(DB.GetQuery("DEPT", "DELETE"), parameters));
                }
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }
    }
}