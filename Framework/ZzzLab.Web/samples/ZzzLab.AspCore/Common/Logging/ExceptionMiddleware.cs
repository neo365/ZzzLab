using Microsoft.AspNetCore.Diagnostics;
using ZzzLab.Net.Http;
using ZzzLab.Web.Middelware;
using ZzzLab.Web.Models;
using static System.Net.Mime.MediaTypeNames;

namespace ZzzLab.AspCore.Logging
{
    /// <summary>
    /// 사용자 Web Exception 처리를 위한 모듈
    /// </summary>
    public class ExceptionMiddleware : IExceptionMiddleware
    {
        public ExceptionMiddleware()
        {
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = Text.Plain;

            string message = "서버에러가 발생하였습니다.";

            var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            if (error != null)
            {
                context.Response.ContentType = Application.Json;
                var exception = error.InnerException ?? error;

                RestServerErrorResult res = new RestServerErrorResult()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Error = exception.Message,
                    ErrorDescription = StatusCodes.Status500InternalServerError.ToStatusMessage(),
                    StackTrace = exception.StackTrace
                };

                message = res.ToString();
            }

            try
            {
                await context.Response.WriteAsync(message);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
            }
        }
    }
}