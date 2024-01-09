using Microsoft.AspNetCore.Mvc;

namespace ZzzLab.Web.Controller
{
    public abstract class ApiControllerBaseEx : ApiControllerBase
    {
        #region OK

        [NonAction]
        public virtual IActionResult OK()
            => this.OkResult();

        [NonAction]
        public virtual IActionResult OK(string message)
            => this.OkResult(message);

        [NonAction]
        public virtual IActionResult Ok<T>(T item)
            => this.OkResult<T>(item);

        [NonAction]
        public virtual IActionResult OK<T>(T item, string message)
            => this.OkResult<T>(item, message);

        #endregion OK

        #region Grid

        [NonAction]
        public virtual IActionResult Grid<T>(IEnumerable<T> item)
            => this.GridResult<T>(item);

        #endregion Grid

        #region Fail

        [NonAction]
        public virtual IActionResult Fail()
            => this.FailResult();

        [NonAction]
        public virtual IActionResult Fail(string message)
            => this.FailResult(message);

        [NonAction]
        public virtual IActionResult Fail<T>(T item)
            => this.FailResult<T>(item);

        [NonAction]
        public virtual IActionResult Fail<T>(T item, string message)
            => this.FailResult<T>(item, message);

        #endregion Fail

        [NonAction]
        public virtual IActionResult Fail(Exception ex)
            => this.ErrorResult(ex);
    }
}