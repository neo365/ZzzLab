using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZzzLab.Web.Models;

namespace ZzzLab.Web.Controller
{
    public abstract partial class ApiControllerBase
    {
        /// <summary>
        /// Http Status : 400
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public new IActionResult BadRequest()
            =>  RestResult.BadRequest(this.HttpContext.TraceIdentifier);

        /// <summary>
        /// Http Status : 400
        /// </summary>
        /// <param name="message">오류 메세지</param>
        /// <returns></returns>
        [NonAction]
        public IActionResult BadRequest(string message)
            => RestResult.BadRequest(message, this.HttpContext.TraceIdentifier);

        /// <summary>
        /// Http Status : 401
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public new IActionResult Unauthorized()
            => RestResult.Unauthorized(this.HttpContext.TraceIdentifier);

        /// <summary>
        /// Http Status : 401
        /// </summary>
        /// <param name="message">오류메세지</param>
        /// <returns></returns>
        [NonAction]
        public IActionResult Unauthorized(string message)
            => RestResult.Unauthorized(message, this.HttpContext.TraceIdentifier);


        /// <summary>
        /// Http Status : 403
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public new IActionResult Forbid()
            => this.Forbidden();

        /// <summary>
        ///  Http Status : 403
        /// </summary>
        /// <param name="message">오류메세지</param>
        /// <returns></returns>
        [NonAction]
        public IActionResult Forbid(string message)
            => this.Forbidden(message);

        /// <summary>
        /// Http Status : 403
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public IActionResult Forbidden()
            => RestResult.Forbidden(this.HttpContext.TraceIdentifier);

        /// <summary>
        ///  Http Status : 403
        /// </summary>
        /// <param name="message">오류메세지</param>
        /// <returns></returns>
        [NonAction]
        public IActionResult Forbidden(string message)
            => RestResult.Forbidden(message, this.HttpContext.TraceIdentifier);

        /// <summary>
        /// Http Status : 404
        /// </summary>
        /// <returns></returns>
        public new IActionResult NotFound()
            => this.PageNotFound();

        /// <summary>
        /// Http Status : 404
        /// </summary>
        /// <param name="message">오류메세지</param>
        /// <returns></returns>
        [NonAction]
        public IActionResult NotFound(string message)
            => this.PageNotFound(message);

        /// <summary>
        /// Http Status : 404
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public IActionResult PageNotFound()
            => RestResult.PageNotFound(this.HttpContext.TraceIdentifier);

        /// <summary>
        /// Http Status : 404
        /// </summary>
        /// <param name="message">오류메세지</param>
        /// <returns></returns>
        [NonAction]
        public IActionResult PageNotFound(string message)
            => RestResult.PageNotFound(message, this.HttpContext.TraceIdentifier);
    }
}