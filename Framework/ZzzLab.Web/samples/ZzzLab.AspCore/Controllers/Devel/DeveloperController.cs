using Microsoft.AspNetCore.Mvc;
using ZzzLab.Web.Controller;

namespace ZzzLab.AspCore.Controllers
{
    [ApiController]
    [Route("/devel")]
    public partial class DeveloperController : AuthApiControllerBase
    {
    }
}