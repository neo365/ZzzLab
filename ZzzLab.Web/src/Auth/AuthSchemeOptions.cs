using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Primitives;

namespace ZzzLab.Web.Authentication
{
    public class AuthSchemeOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        public static string Scheme => DefaultScheme;
        public StringValues AuthKey { get; set; }
    }
}