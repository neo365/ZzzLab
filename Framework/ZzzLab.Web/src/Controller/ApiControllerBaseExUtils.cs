using Microsoft.AspNetCore.Mvc;
using System.Data;
using ZzzLab.Web.Models;

namespace ZzzLab.Web.Controller
{
    internal static class ApiControllerBaseExUtils
    {
        #region OK

        public static IActionResult OkResult(this ApiControllerBase res)
            => RestResult.Ok(RestResult.BASE_OK_CODE, res.HttpContext.TraceIdentifier);

        public static IActionResult OkResult(this ApiControllerBase res, string message)
            => RestResult.OkWithMessage(message, RestResult.BASE_OK_CODE, res.HttpContext.TraceIdentifier);

        public static IActionResult OkResult<T>(this ApiControllerBase res, T item)
            => RestResult.OkWithItem(item, null, RestResult.BASE_OK_CODE, res.HttpContext.TraceIdentifier);

        public static IActionResult OkResult<T>(this ApiControllerBase res, T item, string message)
            => RestResult.OkWithItem(item, message, RestResult.BASE_OK_CODE, res.HttpContext.TraceIdentifier);

        #endregion OK

        #region Grid

        public static IActionResult GridResult<T>(this ApiControllerBase res, IEnumerable<T> items)
            => RestResult.Grid(items, trackingId: res.HttpContext.TraceIdentifier);

        public static IActionResult GridResult(this ApiControllerBase res, DataTable table)
            => RestResult.Grid(table, trackingId: res.HttpContext.TraceIdentifier);

        #endregion Grid

        #region Fail

        public static IActionResult FailResult(this ApiControllerBase res)
            => RestResult.Fail(RestResult.BASE_FAIL_CODE, res.HttpContext.TraceIdentifier);

        public static IActionResult FailResult(this ApiControllerBase res, string message)
            => RestResult.FailWithMessage(message, RestResult.BASE_FAIL_CODE, res.HttpContext.TraceIdentifier);

        public static IActionResult FailResult<T>(this ApiControllerBase res, T item)
            => RestResult.FailWithItem(item, null, RestResult.BASE_FAIL_CODE, res.HttpContext.TraceIdentifier);

        public static IActionResult FailResult<T>(this ApiControllerBase res, T item, string message)
            => RestResult.FailWithItem(item, message, RestResult.BASE_FAIL_CODE, res.HttpContext.TraceIdentifier);

        #endregion Fail

        public static IActionResult ErrorResult(this ApiControllerBase res, Exception ex)
            => RestResult.Fail(ex, RestResult.BASE_EXCEPTION_CODE, res.HttpContext.TraceIdentifier);
    }
}