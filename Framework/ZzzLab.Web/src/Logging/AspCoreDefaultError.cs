using Microsoft.AspNetCore.Http;
using ZzzLab.Web.Models;

namespace ZzzLab.Web.Logging
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    /// <example>
    /// {
    //    "type":"https://tools.ietf.org/html/rfc7231#section-6.5.1",
    //    "title":"One or more validation errors occurred.",
    //    "status":400,
    //    "traceId":"00-68f5202642426c19e7b6c0c75e7feb37-1fb7b4a1b61b09f0-00",
    //    "errors":{"reqNo":["The value 'undefined' is not valid."]
    //    }
    //}
    /// </example>
    public class AspCoreDefaultError
    {
        public string? TraceId { set; get; }
        public string? Type { set; get; }
        public string? Title { set; get; }
        public int? Status { set; get; }
        public IDictionary<string, IEnumerable<string>?>? Errors { set; get; }

        internal RestErrorResult ToRestResult()
        {
            RestErrorResult res = new RestErrorResult
            {
                TrakingId = TraceId ?? Guid.NewGuid().ToString(),
                StatusCode = Status ?? StatusCodes.Status500InternalServerError,
                ErrorMessage = $"{Title} ({Type})",
            };

            if (Errors != null && Errors.Count > 0)
            {
                string errMsg = "";
                foreach (var error in Errors)
                {
                    if (error.Value == null || error.Value.Any() == false) continue;

                    foreach (var value in error.Value)
                    {
                        errMsg += value + " ";
                    }
                }

                res.ErrorDescription = errMsg;
            }

            return res;
        }
    }
}