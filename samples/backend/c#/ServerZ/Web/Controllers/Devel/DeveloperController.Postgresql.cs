using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.IO;
using ZzzLab.Data;
using ZzzLab.Office.Excel;
using ZzzLab.Web.Models;

namespace ZzzLab.Web.Controllers
{
    public partial class DeveloperController
    {
        /// <summary>
        /// 테이블 목록을 가져온다
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Postgresql/Table")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestGridResponse<dynamic>))]
        public IActionResult GetPostgresTableList()
        {
            try
            {
                QueryParameterCollection parameters = new QueryParameterCollection
                {
                    { "SchemaName", "public"}
                };

                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    return RestResult.Grid(DB.Select(DB.GetQuery("DEVEL_PG_TABLE", "TABLE_LIST"), parameters));
                }
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        /// <summary>
        /// 테이블 명세를 가져온다
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Postgresql/Table/{tableName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestGridResponse<dynamic>))]
        public IActionResult GetPostgresTable(string tableName)
        {
            try
            {
                QueryParameterCollection parameters = new QueryParameterCollection
                {
                    { "SchemaName", "public"},
                    { "TableName", tableName}
                };

                using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                {
                    return RestResult.Grid(DB.Select(DB.GetQuery("DEVEL_PG_TABLE", "TABLE_DETAIL"), parameters));
                }
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        /// <summary>
        /// 테이블 명세를 가져온다
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Postgresql/TableDocument")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPostgresDocument()
        {
            try
            {
                QueryParameterCollection parameters = new QueryParameterCollection
                {
                    { "SchemaName", "public"}
                };

                using (IExcel excel = new ExcelPOI())
                {
                    excel.Create();
                    using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                    {
                        DataTable table = DB.Select(DB.GetQuery("DEVEL_PG_TABLE", "TABLE_LIST"), parameters);

                        if (table == null || table.Rows.Count == 0) return RestResult.NotFound();

                        foreach (DataRow row in table.Rows)
                        {
                            string tableName = row.ToString("table_name");
                            parameters.Set("TableName", tableName);

                            DataTable detailTable = DB.Select(DB.GetQuery("DEVEL_PG_TABLE", "TABLE_DETAIL"), parameters);

                            if (detailTable == null || detailTable.Rows.Count == 0) continue;

                            excel.SetSheet(detailTable, tableName);
                        }

                        //excel.Save();
                        using (MemoryStream ms = excel.ToStream())
                        {
                            return this.File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }
    }
}