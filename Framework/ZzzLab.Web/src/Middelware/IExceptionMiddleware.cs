using Microsoft.AspNetCore.Http;

namespace ZzzLab.Web.Middelware
{
    public interface IExceptionMiddleware
    {
        Task Invoke(HttpContext context);
    }
}