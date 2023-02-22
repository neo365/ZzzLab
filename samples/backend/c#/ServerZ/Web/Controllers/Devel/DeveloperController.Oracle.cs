using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using ZzzLab.Data;
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
        [Route("DataBase/{configName}/TableList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestGridResponse<dynamic>))]
        public IActionResult GetTableList(string configName)
        {
            try
            {
                using (IDBHandler DB = DataBaseHandler.Create(configName))
                {
                    return RestResult.Grid(DB.GetTableList());
                }
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        /// <summary>
        /// 테이블 정보를 가져온다
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DataBase/{configName}/{schemaName}.{tableName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestGridResponse<dynamic>))]
        public IActionResult GetTable(string configName, string schemaName, string tableName)
        {
            try
            {
                //using (IDBHandler DB = DataBaseHandler.Create(configName))
                //{
                //    return RestResult.Ok(DB.GetTableInfo(schemaName, tableName));
                //}
                return RestResult.Ok();
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }

        /// <summary>
        /// 컬럼 정보를 가져온다
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DataBase/{configName}/{schemaName}.{tableName}/Columns")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestGridResponse<dynamic>))]
        public IActionResult GetColumns(string configName, string schemaName, string tableName)
        {
            try
            {
                //using (IDBHandler DB = DataBaseHandler.Create(configName))
                //{
                //    //return RestResult.Grid(DB.GetTableColumns(schemaName, tableName));
                //}

                return RestResult.Ok();
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
        [Route("DataBase/{configName}/{schemaName}.{tableName}/Document")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetTableDocument(string configName, string schemaName, string tableName)
        {
            try
            {
                //QueryParameterCollection parameters = new QueryParameterCollection
                //{
                //    { "SchemaName", "public"}
                //};

                //using (IExcel excel = new ExcelPOI())
                //{
                //    excel.Create();
                //    using (IDBHandler DB = DataBaseHandler.Create(AppConstant.ConnectionName))
                //    {
                //        DataTable table = DB.Select(DB.GetQuery("DEVEL_ORA_TABLE", "TABLE_LIST"), parameters);

                //        if (table == null || table.Rows.Count == 0) return RestResult.NotFound();

                //        foreach (DataRow row in table.Rows)
                //        {
                //            string tableName = row.ToString("table_name");
                //            parameters.Set("TableName", tableName);

                //            DataTable detailTable = DB.Select(DB.GetQuery("DEVEL_ORA_TABLE", "TABLE_DETAIL"), parameters);

                //            if (detailTable == null || detailTable.Rows.Count == 0) continue;

                //            excel.SetSheet(detailTable, tableName);
                //        }

                //        //excel.Save();
                //        using (MemoryStream ms = excel.ToStream())
                //        {
                //            return this.File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                //        }
                //    }
                //}

                return RestResult.Ok();
            }
            catch (Exception ex)
            {
                return RestResult.Fail(ex);
            }
        }
    }
}