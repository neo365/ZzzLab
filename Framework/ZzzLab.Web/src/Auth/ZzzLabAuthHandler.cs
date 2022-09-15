using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ZzzLab.Web.Authentication
{
    public class ZzzLabAuthHandler : AuthenticationHandler<AuthSchemeOptions>
    {
        public ZzzLabAuthHandler(IOptionsMonitor<AuthSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Create authenticated user
            var identities = new List<ClaimsIdentity> { new ClaimsIdentity("ZzzLab auth type") };
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identities), AuthSchemeOptions.Scheme);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}