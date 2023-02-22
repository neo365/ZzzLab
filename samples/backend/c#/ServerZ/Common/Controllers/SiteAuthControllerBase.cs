using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZzzLab.Web.Attributes;
using ZzzLab.Web.Models;

namespace ZzzLab.Web.Controllers
{
    [AuthRequired]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(RestErrorResult))]
    public abstract partial class SiteAuthControllerBase
    {
    }
}