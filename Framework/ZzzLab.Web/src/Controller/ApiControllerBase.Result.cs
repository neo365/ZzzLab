using Microsoft.AspNetCore.Mvc;
using System.Data;
using ZzzLab.Web.Models;

namespace ZzzLab.Web.Controller
{
    public abstract partial class ApiControllerBase
    {
        #region OK

        [NonAction]
        public new IActionResult Ok()
            => this.OK();

        [NonAction]
        public virtual IActionResult OK()
            => RestResult.Ok(RestResult.BASE_OK_CODE, this.HttpContext.TraceIdentifier);

        [NonAction]
        public virtual IActionResult Ok(string message)
            => this.OK(message);

        [NonAction]
        public virtual IActionResult OK(string message)
            => RestResult.OkWithMessage(message, RestResult.BASE_OK_CODE, this.HttpContext.TraceIdentifier);

        [NonAction]
        public virtual IActionResult Ok<T>(T item)
            => this.OK<T>(item);

        [NonAction]
        public virtual IActionResult OK<T>(T item)
            => RestResult.OkWithItem(item, null, RestResult.BASE_OK_CODE, this.HttpContext.TraceIdentifier);

        [NonAction]
        public virtual IActionResult Ok<T>(T item, string message)
            => this.OK<T>(item, message);

        [NonAction]
        public virtual IActionResult OK<T>(T item, string message)
            => RestResult.OkWithItem(item, message, RestResult.BASE_OK_CODE, this.HttpContext.TraceIdentifier);

        #endregion OK

        #region Grid

        [NonAction]
        public virtual IActionResult Grid<T>(IEnumerable<T> items)
            => RestResult.Grid(items, trackingId: this.HttpContext.TraceIdentifier);

        [NonAction]
        public virtual IActionResult Grid<T>(IEnumerable<T> items, int recordsTotal = -1, int recordsFiltered = -1)
            => RestResult.Grid(items, recordsTotal: recordsTotal, recordsFiltered: recordsFiltered, trackingId: this.HttpContext.TraceIdentifier);

        [NonAction]
        public virtual IActionResult Grid(DataTable table)
            => RestResult.Grid(table, trackingId: this.HttpContext.TraceIdentifier);

        [NonAction]
        public virtual IActionResult Grid(DataTable table, int recordsTotal = -1, int recordsFiltered = -1)
            => RestResult.Grid(table, recordsTotal: recordsTotal, recordsFiltered: recordsFiltered, trackingId: this.HttpContext.TraceIdentifier);

        [NonAction]
        public virtual IActionResult GridEmpty(int recordsTotal = -1, int recordsFiltered = -1)
           => RestResult.GridEmpty(recordsTotal, recordsFiltered, trackingId: this.HttpContext.TraceIdentifier);

        #endregion Grid

        #region Fail

        [NonAction]
        public virtual IActionResult Fail()
           => RestResult.Fail(RestResult.BASE_FAIL_CODE, this.HttpContext.TraceIdentifier);

        [NonAction]
        public virtual IActionResult Fail(string message)
           => RestResult.FailWithMessage(message, RestResult.BASE_FAIL_CODE, this.HttpContext.TraceIdentifier);

        [NonAction]
        public virtual IActionResult Fail<T>(T item)
             => RestResult.FailWithItem(item, null, RestResult.BASE_FAIL_CODE, this.HttpContext.TraceIdentifier);

        [NonAction]
        public virtual IActionResult Fail<T>(T item, string message)
            => RestResult.FailWithItem(item, message, RestResult.BASE_FAIL_CODE, this.HttpContext.TraceIdentifier);

        #endregion Fail

        #region Problem

        [NonAction]
        public IActionResult Problem(Exception ex)
            => this.Fail(ex);

        [NonAction]
        public virtual IActionResult Fail(Exception ex)
            => RestResult.Fail(ex, RestResult.BASE_EXCEPTION_CODE, this.HttpContext.TraceIdentifier);

        #endregion Problem
    }
}